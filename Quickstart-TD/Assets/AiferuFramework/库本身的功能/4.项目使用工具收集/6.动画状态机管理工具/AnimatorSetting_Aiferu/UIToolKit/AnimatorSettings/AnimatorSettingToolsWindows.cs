#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System;
using System.Collections.Generic;
using System.IO;
using AiferuFramework.AssetsManagementTools;
using UnityEditor.Animations;
using System.Text;

namespace AiferuFramework.AnimatorSettingTools
{

    public class AnimatorSettingToolsWindows : EditorWindow
    {
        #region 基础属性
        private static VisualElement root;

        struct AnimatorSettingToolsTemporaryData
        {
            public List<AnimatorSettingToolsObjectData> animatorSettingToolsObjectDataList;
            public AnimatorSettingToolsBaseData baseData;
        }

        private AnimatorSettingToolsTemporaryData animatorSettingToolsTemporaryData;

        /// <summary>
        /// 存储当前动画分组是否已经生成对应的动画状态机
        /// </summary>
        private Dictionary<AnimatorSettingToolsBaseData.AnimationGroup, string> AnimationGroupData;
        #region 窗口初始化
        //[MenuItem("Window/UI Toolkit/AnimatorSettings")]
        [MenuItem("AiferuFramework/库本身的功能/4.项目使用工具收集/6.动画控制器管理工具", false, 4006)]
        public static void ShowExample()
        {
            AnimatorSettingToolsWindows wnd = GetWindow<AnimatorSettingToolsWindows>();
            wnd.titleContent = new GUIContent("AnimatorSettings");
        }
        /// <summary>
        /// UIToolKit窗口初始化
        /// </summary>
        private void UIToolKitWindowsIntialize()
        {
            // Each editor window contains a root VisualElement object
            root = rootVisualElement;

            // Import UXML 
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(AnimatorSettingToolsEditorBase.FilePath_ToolsWindowObjectPath);
            //克隆UXML文件中的东西并初始化
            visualTree.CloneTree(root);
        }
        #endregion
        #endregion

        #region 临时字段

        #endregion

        #region UIToolKit对象属性

        #region Button
        private Button m_Button_RefreshData;
        private Button m_Button_ClearData;
        private Button m_Button_CrearAnimator;
        private Button m_Button_CreateAnimator;
        private Button m_Button_CreateAnimationClip;
        private Button m_Button_SelectBaseData;
        #endregion

        #region ListView
        private ListView m_ListView_SkinProfabData;
        #endregion

        #region EnumField
        private EnumField m_EnumField_AnimatorTemplate;
        private EnumField m_EnumField_AnimationGraph;
        #endregion

        #region TextField
        private TextField m_TextField_Name;
        #endregion

        #region Lablel
        private Label m_Label_SkinProfabItemCount;
        #endregion

        #endregion

        #region Main方法
        private void CreateGUI()
        {
            #region UIToolkit初始化
            UIToolKitWindowsIntialize();

            UIToolKitPropertyIntialize();
            #endregion
            DataIntialize();
        }

        private void OnDestroy()
        {
            Save();
        }


        #endregion

        #region 数据初始化
        private void DataIntialize()
        {
            animatorSettingToolsTemporaryData = LoadData();
            //初始化列表数据
            m_ListView_SkinProfabData.itemsSource = animatorSettingToolsTemporaryData.animatorSettingToolsObjectDataList;
            m_Label_SkinProfabItemCount.text = "Count:" + animatorSettingToolsTemporaryData.animatorSettingToolsObjectDataList.Count;

        }

        private void AnimatorGroupDataDictionaryIntialize()
        {
            AnimationGroupData = new Dictionary<AnimatorSettingToolsBaseData.AnimationGroup, string>();
            for (int i = 1; i < (int)AnimatorSettingToolsBaseData.AnimationGroup.Count; i++)
            {
                AnimationGroupData.Add((AnimatorSettingToolsBaseData.AnimationGroup)i, null);

            }
        }


        /// <summary>
        /// 加载所有的数据
        /// </summary>
        private AnimatorSettingToolsTemporaryData LoadData()
        {
            AnimatorSettingToolsTemporaryData animatorSettingToolsTemporaryData = new AnimatorSettingToolsTemporaryData();
            animatorSettingToolsTemporaryData.animatorSettingToolsObjectDataList = AnimatorSettingToolsEditorBase.GetAllAnimatorSettingToolsObjectDataScriptableObjectFormPath(AnimatorSettingToolsEditorBase.FilePath_SkinObjectDataPath);
            animatorSettingToolsTemporaryData.baseData = AssetDatabase.LoadAssetAtPath<AnimatorSettingToolsBaseData>(AnimatorSettingToolsEditorBase.FilePath_BaseDataPath);
            return animatorSettingToolsTemporaryData;
        }

        /// <summary>
        /// 创建数据
        /// 创建方法时会删除掉之前的所有数据,重新创建文件,然后重新刷新界面
        /// </summary>
        private void CreateData()
        {
            DeleteData();
            #region 遍历所有的包含_Skin字段的预制体
            List<GameObject> skinProfabList = new List<GameObject>();
            skinProfabList = AnimatorSettingToolsEditorBase.GetAllSkinProfabFormPath(AnimatorSettingToolsEditorBase.FilePath_SkinObjectPath);
            #endregion
            #region 为所有预制体创建对应的数据结构
            AnimatorSettingToolsEditorBase.CreateAnimatorSettingToolsObjectDataScriptableObjectFile(skinProfabList);
            #endregion
            RefreshView();
        }

        /// <summary>
        /// 删除数据
        /// 删除方法会删除掉所有数据,然后重新刷新界面
        /// </summary>
        private void DeleteData()
        {
            //获取数据存储路径
            string dataPath = AnimatorSettingToolsEditorBase.FilePath_SkinObjectDataPath;
            if (Directory.Exists(dataPath))
            {
                //删除路径下的所有文件
                EditorBase.DelectDir(dataPath);
                Debug.LogError("已删除所有数据,来自AnimatorSettingTools");
            }
            RefreshView();
        }

        /// <summary>
        /// 刷新数据 增删数据
        /// 刷新数据方法会遍历所有数据,根据蒙皮预制体链表创建或删除数据对象
        /// </summary>
        private void RefreshData()
        {
            List<GameObject> skinProfabList = new List<GameObject>();
            List<AnimatorSettingToolsObjectData> animatorSettingToolsObjectList = new List<AnimatorSettingToolsObjectData>();
            List<string> scriptableObjectPathList = new List<string>();

            List<GameObject> needCreateDataObjectList = new List<GameObject>();
            List<AnimatorSettingToolsObjectData> needDeleteDataObjectList = new List<AnimatorSettingToolsObjectData>();

            #region 遍历所有的包含_Skin字段的预制体
            skinProfabList = AnimatorSettingToolsEditorBase.GetAllSkinProfabFormPath(AnimatorSettingToolsEditorBase.FilePath_SkinObjectPath);
            #endregion
            #region 获取所有序列化数据对象
            string dataFilePath = AnimatorSettingToolsEditorBase.FilePath_SkinObjectDataPath;

            animatorSettingToolsObjectList = AnimatorSettingToolsEditorBase.GetAllAnimatorSettingToolsObjectDataScriptableObjectFormPath(dataFilePath);

            #endregion
            #region 比对出需要删除和需要创建的文件
            foreach (var skinProfab in skinProfabList)
            {
                int sameAmountOfData = 0;
                foreach (var animatorSettingToolsObject in animatorSettingToolsObjectList)
                {
                    if (skinProfab.name == animatorSettingToolsObject.name)
                    {
                        sameAmountOfData++;
                    }
                }
                if (sameAmountOfData == 0)
                {
                    Debug.Log("这个蒙皮文件是新的:" + skinProfab.name);
                    needCreateDataObjectList.Add(skinProfab);
                }
            }

            foreach (var animatorSettingToolsObject in animatorSettingToolsObjectList)
            {
                int sameAmountOfData = 0;
                foreach (var skinProfab in skinProfabList)
                {
                    if (skinProfab.name == animatorSettingToolsObject.name)
                    {
                        sameAmountOfData++;
                    }
                }
                if (sameAmountOfData == 0)
                {
                    Debug.Log("这个数据文件需要删除:" + animatorSettingToolsObject.name);
                    needDeleteDataObjectList.Add(animatorSettingToolsObject);
                }
            }
            #endregion
            #region 删除多余的数据
            scriptableObjectPathList = EditorBase.GetAllScriptableObjectPathFormPath(dataFilePath);
            foreach (var scriptableObjectPath in scriptableObjectPathList)
            {
                bool deleted = false;
                foreach (var needDeleteDataObjectName in needDeleteDataObjectList)
                {
                    if (deleted == true)
                    {
                        continue;
                    }
                    if (needDeleteDataObjectName.SkinProfabName == AssetDatabase.LoadAssetAtPath<AnimatorSettingToolsObjectData>(scriptableObjectPath).SkinProfabName)
                    {
                        Debug.Log("删除多余的数据:" + needDeleteDataObjectName);
                        AnimatorSettingToolsEditorBase.DelectDir(scriptableObjectPath);
                        deleted = true;
                    }
                }
            }
            #endregion
            #region 为新增的预制体创建数据
            AnimatorSettingToolsEditorBase.CreateAnimatorSettingToolsObjectDataScriptableObjectFile(needCreateDataObjectList);
            #endregion

            Debug.LogError("已刷新数据,来自AnimatorSettingTools");
            RefreshView();
        }


        /// <summary>
        /// 刷新显示视图
        /// 刷新视图方法会遍历现有数据,显示在列表中
        /// </summary>
        private void RefreshView()
        {
            DataIntialize();
            Debug.LogError("已刷新列表,来自AnimatorSettingTools");
        }

        private static void Save()
        {
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("保存成功");
        }

        #endregion

        #region UIToolKit属性初始化
        /// <summary>
        /// UIToolkit引用属性初始化
        /// </summary>
        void UIToolKitPropertyIntialize()
        {
            ALLListViewIntialize();

            AllButtonIntialize();

            AllLabelIntialize();

            AllTextFieldIntialize();

            ALLEnumFieldIntialize();
        }
        #region Button
        /// <summary>
        /// 初始化所有的Button
        /// </summary>
        private void AllButtonIntialize()
        {
            m_Button_RefreshData = root.Q<Button>("Button_RefreshData");
            m_Button_RefreshData.clicked += Button_RefreshData_OnClicked;

            m_Button_ClearData = root.Q<Button>("Button_ClearData");
            m_Button_ClearData.clicked += Button_ClearData_OnClicked;

            m_Button_CrearAnimator = root.Q<Button>("Button_CrearAnimator");
            m_Button_CrearAnimator.clicked += Button_CrearAnimator_OnClicked;

            m_Button_CreateAnimator = root.Q<Button>("Button_CreateAnimator");
            m_Button_CreateAnimator.clicked += Button_CreateAnimator_OnClicked;

            m_Button_CreateAnimationClip = root.Q<Button>("Button_CreateAnimationClip");
            m_Button_CreateAnimationClip.clicked += Button_CreateAnimationClip_OnClicked;

            m_Button_SelectBaseData = root.Q<Button>("Button_SelectBaseData");
            m_Button_SelectBaseData.clicked += Button_SelectBaseData_OnClicked;

        }

        private void Button_SelectBaseData_OnClicked()
        {
            Selection.activeObject = animatorSettingToolsTemporaryData.baseData;
        }

        private void Button_CreateAnimationClip_OnClicked()
        {
            Debug.Log("创建动画片段");
            List<string> allFbxPaths = new List<string>();
            allFbxPaths.Clear();
            AssetsTools.GetALLFilesFBX(AnimatorSettingToolsEditorBase.FilePath_SkinObjectPath, out allFbxPaths);
            Debug.Log(allFbxPaths.Count);
            foreach (var fbxPath in allFbxPaths)
            {
                if (fbxPath.Contains("Animation"))
                {
                    //创建动画片段
                    AnimationClip clip = AssetDatabase.LoadAssetAtPath<AnimationClip>(fbxPath);
                    AnimationClip clipTemp = new AnimationClip();
                    EditorUtility.CopySerialized(clip, clipTemp);

                    string clipSavePath = fbxPath.Substring(0, fbxPath.IndexOf("Animation") + 10) + "Clip";

                    if (!Directory.Exists(clipSavePath))
                    {
                        //路径不存在则创建路径
                        Directory.CreateDirectory(clipSavePath);
                    }
                    clipSavePath = clipSavePath + "\\" + clip.name + "_SkinCopy.anim";

                    Debug.Log("创建了新的动画片段:" + clipSavePath);
                    AssetDatabase.CreateAsset(clipTemp, clipSavePath);
                    Save();

                }
            }
        }

        private void Button_CrearAnimator_OnClicked()
        {
            //获取数据存储路径
            string dataPath = AnimatorSettingToolsEditorBase.FilePath_AnimatorSavePath;
            if (Directory.Exists(dataPath))
            {
                //删除路径下的所有文件
                EditorBase.DelectDir(dataPath);
                Debug.LogError("已删除所有Animator数据,来自AnimatorSettingTools");
            }
            RefreshView();
        }


        //! 根据数据创建动画状态机
        // 动画分组数据 动画模板数据
        //! 将动画状态机赋值给对应的预制体


        /// <summary>
        /// 根据数据创建动画状态机对象
        /// </summary>
        private void Button_CreateAnimator_OnClicked()
        {
            #region 临时属性
            AnimatorGroupDataDictionaryIntialize();
            //所有对象数据
            List<AnimatorSettingToolsObjectData> animatorSettingToolsObjectDataList = new List<AnimatorSettingToolsObjectData>();
            //所有动画对象
            List<AnimationClip> skinCopyanimationClipList = new List<AnimationClip>();
            List<string> skinCopyAnimationClipPathList = new List<string>();
            List<string> AnimationClipNames = new List<string>();

            #endregion

            #region 获取数据
            //读取所有的对象数据
            animatorSettingToolsObjectDataList = AnimatorSettingToolsEditorBase.GetAllAnimatorSettingToolsObjectDataScriptableObjectFormPath(AnimatorSettingToolsEditorBase.FilePath_SkinObjectDataPath);
            //读取所有的动画对象
            skinCopyanimationClipList = AnimatorSettingToolsEditorBase.GetAnimatorSettingToolsAllAnimationClipFromPath(AnimatorSettingToolsEditorBase.FilePath_AnimationClipDataPath);
            //读取所有的动画对象路径
            skinCopyAnimationClipPathList = AnimatorSettingToolsEditorBase.GetAnimatorSettingToolsAllAnimationClipPathFromPath(AnimatorSettingToolsEditorBase.FilePath_AnimationClipDataPath);
            //获取支持的动画名称
            AnimationClipNames = animatorSettingToolsTemporaryData.baseData.AnimationClipNames;
            #endregion

            foreach (var animatorSettingToolsObjectData in animatorSettingToolsObjectDataList)
            {

                if (animatorSettingToolsObjectData.AnimationGroup == AnimatorSettingToolsBaseData.AnimationGroup.Other || animatorSettingToolsObjectData.AnimatorTemplate == AnimatorSettingToolsBaseData.AnimatorTemplate.Other)
                {
                    //当动画状态机模板或者动画分组为other时,不生成动画状态机
                    continue;
                }
                //拿取数据
                string animationGroupName = animatorSettingToolsTemporaryData.baseData.animationGroup[Convert.ToInt32(animatorSettingToolsObjectData.AnimationGroup) - 1];
                string animatorTemplateControllerPath = Application.dataPath.Remove(Application.dataPath.Length - 6, 6) + animatorSettingToolsTemporaryData.baseData.animatorTemplatePath[Convert.ToInt32(animatorSettingToolsObjectData.AnimatorTemplate) - 1];
                string animatorTemplateControllerName = AssetDatabase.LoadAssetAtPath<AnimatorController>(animatorSettingToolsTemporaryData.baseData.animatorTemplatePath[Convert.ToInt32(animatorSettingToolsObjectData.AnimatorTemplate) - 1]).name;

                //读取animator
                string txt = File.ReadAllText(animatorTemplateControllerPath);
                List<string> GUIDList = new List<string>();
                List<string> NewGUIDList = new List<string>();

                //拿到动画状态机的所有GUID
                for (int i = 0; i < txt.Length - 6; i++)
                {
                    string lingshi = txt.Substring(i, 6);
                    //Debug.Log(lingshi);
                    if (lingshi == "guid: ")
                    {

                        //Debug.Log(lingshi);
                        string lingshi2 = "";
                        for (int j = 0; j < txt.Length; j++)
                        {
                            //Debug.Log(lingshi2);
                            if (txt[i + j + 6] != ',')
                            {
                                lingshi2 += txt[i + j + 6].ToString();

                            }
                            else
                            {
                                GUIDList.Add(new String(lingshi2.ToCharArray()));
                                lingshi2 = "";
                                break;
                            }
                        }
                    }
                }

                //根据Guid获取Animation名称
                foreach (var guid in GUIDList)
                {
                    //获取模板状态机中使用的动画片段
                    AnimationClip tempAnimationClip = AssetDatabase.LoadAssetAtPath<AnimationClip>(AssetDatabase.GUIDToAssetPath(guid));

                    if (tempAnimationClip == null)
                    {
                        Debug.LogError("动画状态机模板:" + animatorTemplateControllerPath + " 中有空动画状态");
                    }
                    //Debug.Log(tempAnimationClip.name);

                    //根据名称,查找可以使用的animationClip
                    for (int i = 0; i < AnimationClipNames.Count; i++)
                    {

                        //当模板状态机中的动画包含我们的关键字时,才能替换
                        if (tempAnimationClip.name.ToUpper().Contains(AnimationClipNames[i].ToUpper()))
                        {

                            //遍历所有的skincopy动画
                            foreach (var skinCopyAnimationClipPath in skinCopyAnimationClipPathList)
                            {
                                AnimationClip skinCopyAnimationClip = AssetDatabase.LoadAssetAtPath<AnimationClip>(skinCopyAnimationClipPath);
                                //Assets/GameResources/Lobby/FBX/Role/Mesh/NonHuman/Fans/Animation/Clip/C_L_Fans_0254_GirlBumpPling_Idle_Copy.anim

                                if (animationGroupName == null)
                                    break;
                                //当skincopy动画片段包含动画关键字//且包含对应的动画分组名称时
                                if (skinCopyAnimationClip.name.ToUpper().Contains(AnimationClipNames[i].ToUpper()) && skinCopyAnimationClip.name.ToUpper().Contains(animationGroupName.ToUpper()))
                                {
                                    string a = skinCopyAnimationClip.name.ToUpper().Replace(AnimationClipNames[i].ToUpper(), "").Replace("SkinCopy".ToUpper(), "");
                                    if (a.Substring(a.Length - 2, 2) != "__")
                                    {
                                        Debug.Log(a);
                                        continue;
                                    }
                                    //Debug.Log(skinCopyAnimationClip.name);
                                    //说明该动画可以替换
                                    NewGUIDList.Add(AssetDatabase.AssetPathToGUID(skinCopyAnimationClipPath));

                                    //替换一下
                                    txt = txt.Replace(guid, AssetDatabase.AssetPathToGUID(skinCopyAnimationClipPath));

                                    //改名字
                                    txt = txt.Replace(animatorTemplateControllerName, animatorSettingToolsObjectData.SkinProfabName);

                                    break;
                                }
                            }
                        }
                    }
                }
                //获取当前对象动画分组的状态机路径
                string animatorSavePath;
                AnimationGroupData.TryGetValue(animatorSettingToolsObjectData.AnimationGroup, out animatorSavePath);
                //当同一个动画分组没有对应的动画状态机时
                if (animatorSavePath == null)
                {
                    animatorSavePath = AnimatorSettingToolsEditorBase.FilePath_AnimatorSavePath + "/" + animatorSettingToolsObjectData.SkinProfabName + "_Animator.controller";
                    AnimationGroupData[animatorSettingToolsObjectData.AnimationGroup] = animatorSavePath;
                    animatorSettingToolsObjectData.BuildAnimatorSavePath = animatorSavePath;
                    if (!Directory.Exists(AnimatorSettingToolsEditorBase.FilePath_AnimatorSavePath))
                    {
                        Directory.CreateDirectory(AnimatorSettingToolsEditorBase.FilePath_AnimatorSavePath);
                    }
                    FileStream fs = new FileStream(animatorSavePath, FileMode.Create);
                    StreamWriter sw = new StreamWriter(fs, Encoding.Default);
                    sw.Write(txt);
                    sw.Close();
                    fs.Close();
                    Save();
                    //创建前先判断该状态机是否已经创建完成
                }
                else
                {
                    animatorSettingToolsObjectData.BuildAnimatorSavePath = animatorSavePath;
                }


            }
            //根据动画模板选择复制哪个动画控制器
            //根据动画分组确定需要使用哪个动画
            Debug.Log("Animator创建成功");
            AnimatorControllerLoadToSkinProfab();

        }

        private void AnimatorControllerLoadToSkinProfab()
        {
            #region 临时属性
            //所有对象数据
            List<AnimatorSettingToolsObjectData> animatorSettingToolsObjectDataList = new List<AnimatorSettingToolsObjectData>();

            #endregion

            #region 获取数据
            //读取所有的对象数据
            animatorSettingToolsObjectDataList = AnimatorSettingToolsEditorBase.GetAllAnimatorSettingToolsObjectDataScriptableObjectFormPath(AnimatorSettingToolsEditorBase.FilePath_SkinObjectDataPath);
            #endregion

            foreach (var animatorSettingToolsObjectData in animatorSettingToolsObjectDataList)
            {
                if (animatorSettingToolsObjectData.AnimationGroup == AnimatorSettingToolsBaseData.AnimationGroup.Other || animatorSettingToolsObjectData.AnimatorTemplate == AnimatorSettingToolsBaseData.AnimatorTemplate.Other)
                {
                    //当动画状态机模板或者动画分组为other时,退出
                    continue;
                }
                //当没有对应路径文件时退出
                if (animatorSettingToolsObjectData.BuildAnimatorSavePath == null)
                {
                    Debug.Log(animatorSettingToolsObjectData.SkinProfab);
                    continue;
                }
                //清楚对应的动画控制器
                animatorSettingToolsObjectData.SkinProfab.GetComponent<Animator>().runtimeAnimatorController = null;
                Save();
                //获取 SkinProfabGUID
                string SkinProfabGUID = File.ReadAllText(Application.dataPath + "/" + animatorSettingToolsObjectData.ObjectDataSavePath.Remove(0, 6));
                SkinProfabGUID = SkinProfabGUID.Substring(SkinProfabGUID.IndexOf("SkinProfab: {fileID:") + 20);
                //Debug.Log(SkinProfabGUID);
                SkinProfabGUID = SkinProfabGUID.Substring(0, SkinProfabGUID.IndexOf("type: 3}"));
                //Debug.Log(SkinProfabGUID);
                SkinProfabGUID = SkinProfabGUID.Substring(SkinProfabGUID.IndexOf("guid: ") + 6);
                SkinProfabGUID = SkinProfabGUID.Substring(0, SkinProfabGUID.IndexOf(","));

                //Debug.Log(SkinProfabGUID);

                //动画状态机
                string SkinProfabPath = AssetDatabase.GUIDToAssetPath(SkinProfabGUID);
                if (animatorSettingToolsObjectData.BuildAnimatorSavePath != null)
                {

                    //读取SkinProfab数据
                    string SkinProfabtxt = File.ReadAllText(Application.dataPath.Substring(0, Application.dataPath.Length - 6) + SkinProfabPath);
                    Debug.Log(SkinProfabtxt);


                    SkinProfabtxt = SkinProfabtxt.Replace("m_Controller: {fileID: 0}", "m_Controller: {fileID: 9100000, guid: " + AssetDatabase.AssetPathToGUID(animatorSettingToolsObjectData.BuildAnimatorSavePath) + ", type: 2}");
                    //保存

                    FileStream fs = new FileStream(Application.dataPath + "/" + SkinProfabPath.Replace("Assets/", ""), FileMode.Create);
                    StreamWriter sw = new StreamWriter(fs, Encoding.Default);
                    sw.Write(SkinProfabtxt);
                    sw.Close();
                    fs.Close();
                    Save();
                }
            }
        }

        private void Button_ClearData_OnClicked()
        {
            DeleteData();
        }

        private void Button_RefreshData_OnClicked()
        {
            RefreshData();
        }

        #endregion

        #region ListView
        /// <summary>
        /// 初始化所有的ListView
        /// </summary>
        private void ALLListViewIntialize()
        {
            m_ListView_SkinProfabData_Intialize();
        }

        #region 初始化m_ListView_SkinProfabData
        private void m_ListView_SkinProfabData_Intialize()
        {
            m_ListView_SkinProfabData = root.Q<ListView>("ListView_SkinProfabData");
            //将ListView的子对象初始化 MakeListItem方法定义了当前ListView的子对象为什么类型
            m_ListView_SkinProfabData.makeItem = m_ListView_SkinProfabData_MakeListItem;
            //为ListView的每个子对象赋值
            m_ListView_SkinProfabData.bindItem = m_ListView_SkinProfabData_BindItem;
            //当ListView中的选择改变时调用
            m_ListView_SkinProfabData.onSelectionChange += m_ListView_SkinProfabData_OnSelectionChange;
        }

        private VisualElement m_ListView_SkinProfabData_MakeListItem()
        {
            Label label = new Label();
            return label;
        }

        private void m_ListView_SkinProfabData_BindItem(VisualElement arg1, int arg2)
        {
            Label label = arg1 as Label;

            label.text = animatorSettingToolsTemporaryData.animatorSettingToolsObjectDataList[arg2].SkinProfabName;

        }

        private void m_ListView_SkinProfabData_OnSelectionChange(IEnumerable<object> obj)
        {
            foreach (var item in obj)
            {
                var go = item as AnimatorSettingToolsObjectData;
                //将返回的对象设置为选中对象
                Selection.activeObject = go.SkinProfab;
                m_TextField_Name.value = go.SkinProfabName;

                //数据绑定
                m_EnumField_AnimatorTemplate.Init(go.AnimatorTemplate);
                m_EnumField_AnimationGraph.Init(go.AnimationGroup);


                SerializedObject so = new SerializedObject(go);
                m_EnumField_AnimatorTemplate.Bind(so);
                m_EnumField_AnimationGraph.Bind(so);
            }
        }


        #endregion
        #endregion

        #region EnumField
        /// <summary>
        /// 初始化所有的EnumField
        /// </summary>
        private void ALLEnumFieldIntialize()
        {
            m_EnumField_AnimatorTemplate = root.Q<EnumField>("EnumField_AnimatorTemplate");
            m_EnumField_AnimatorTemplate.bindingPath = "AnimatorTemplate";

            m_EnumField_AnimationGraph = root.Q<EnumField>("EnumField_AnimationGraph");
            m_EnumField_AnimationGraph.bindingPath = "AnimationGroup";
        }
        #endregion

        #region TextField
        /// <summary>
        /// 初始化所有的TextField
        /// </summary>
        private void AllTextFieldIntialize()
        {
            m_TextField_Name = root.Q<TextField>("TextField_Name");
            //m_TextField_Name.bindingPath = "m_Name";
        }
        #endregion

        #region Lablel
        /// <summary>
        /// 初始化所有的Lablel
        /// </summary>
        private void AllLabelIntialize()
        {
            m_Label_SkinProfabItemCount = root.Q<Label>("Label_SkinProfabItemCount");
        }
        #endregion

        #endregion

    }
}
#endif