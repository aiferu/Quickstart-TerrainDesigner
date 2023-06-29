#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System;
using System.Collections.Generic;
using System.IO;
namespace AiferuFramework.AssetsManagementTools
{
    public class AssetPreprocessingTools : EditorWindow
    {
        /// <summary>
        /// 当前插件的相对位置
        /// </summary>
        private static string AssetPreprocessingToolsFilePath = "Assets/Scipts/Editor/AssetsManagementTools/AssetPreProcessingTools";
        private static VisualElement root;
        //InputSettings文件夹下的所有文件路径
        List<FileInfo> m_inputSettingsFilePaths = new List<FileInfo>();
        List<ModelPreprocessorConfig> m_inputSettingsFile_Mesh = new List<ModelPreprocessorConfig>();
        List<TexturePreprocessorConfig> m_inputSettingsFile_Texture = new List<TexturePreprocessorConfig>();
        List<AudioPreprocessorConfig> m_inputSettingsFile_Audio = new List<AudioPreprocessorConfig>();


        #region UIToolKit对象属性
        //ListView
        private ListView m_listView_InputSettings_Mesh;
        private ListView m_listView_InputSettings_Texture;
        private ListView m_listView_InputSettings_Audio;
        private ListView m_listView_JudgeMentCondition;

        //VisualElement
        private VisualElement m_VE_JudgeMentCondition;

        //Button
        private Button m_button_AddJudgeMentConditionItemConfig;
        private Button m_button_Save;
        private Button m_button_Clear;
        private Button m_button_NewInputSetting;
        private Button m_button_Refresh;
        //label
        private Label m_label_FileType;

        //textField
        private IntegerField m_integerField_Priority;
        #endregion

        //数据属性
        private object[] m_InputSettingConfigGUIDList;

        void Awake()
        {

            AssetPreprocessingToolsFilePath = GetAssetPreprocessingToolsFilePath();

        }


        private void OnDestroy()
        {
            Save();
        }

        public void CreateGUI()
        {

            #region UIToolKit初始化
            UIToolKitWindowsIntialize();
            UIToolKitPropertyIntialize();
            #endregion

            #region 数据初始化
            //加载所有的InputSettings文件
            string InputSettingsFilePath = AssetPreprocessingToolsFilePath + "/Data/InputSettings";
            //获取InputSettings文件夹下的所有文件路径

            if (Directory.Exists(InputSettingsFilePath))
            {
                DirectoryInfo directory = new DirectoryInfo(InputSettingsFilePath);
                FileInfo[] files = directory.GetFiles("*", SearchOption.AllDirectories);

                foreach (FileInfo file in files)
                {
                    if (file.Name.EndsWith(".meta"))
                    {
                        continue;
                    }
                    m_inputSettingsFilePaths.Add(file);
                    //  Debug.Log(file.Name);
                }

                foreach (var item in m_inputSettingsFilePaths)
                {
                    //希望可以根据优先级排序
                    BasePreprocessorConfig basePreprocessor = AssetDatabase.LoadAssetAtPath<BasePreprocessorConfig>(((FileInfo)item).ToString().Replace(System.Environment.CurrentDirectory + "\\", ""));
                    switch (basePreprocessor.FileType)
                    {
                        case BasePreprocessorConfig.InputFileType.Texture:
                            m_inputSettingsFile_Texture.Add(((TexturePreprocessorConfig)basePreprocessor));
                            break;
                        case BasePreprocessorConfig.InputFileType.Model:
                            m_inputSettingsFile_Mesh.Add(((ModelPreprocessorConfig)basePreprocessor));
                            break;
                        case BasePreprocessorConfig.InputFileType.Audio:
                            m_inputSettingsFile_Audio.Add(((AudioPreprocessorConfig)basePreprocessor));
                            break;
                        default:
                            Debug.Log("BasePreprocessorConfig.cs的代码可能有问题");
                            break;
                    }
                }
            }
            //排序
            BubbleSort(m_inputSettingsFile_Mesh);
            BubbleSort(m_inputSettingsFile_Texture);
            BubbleSort(m_inputSettingsFile_Audio);



            m_listView_InputSettings_Mesh.itemsSource = m_inputSettingsFile_Mesh;
            m_listView_InputSettings_Audio.itemsSource = m_inputSettingsFile_Audio;
            m_listView_InputSettings_Texture.itemsSource = m_inputSettingsFile_Texture;
            #endregion

        }
        #region 排序
        static void BubbleSort(int[] intArray)
        {
            int temp = 0;
            bool swapped;
            for (int i = 0; i < intArray.Length; i++)
            {
                swapped = false;
                for (int j = 0; j < intArray.Length - 1 - i; j++)
                    if (intArray[j] > intArray[j + 1])
                    {
                        temp = intArray[j];
                        intArray[j] = intArray[j + 1];
                        intArray[j + 1] = temp;
                        if (!swapped)
                            swapped = true;
                    }
                if (!swapped)
                    return;
            }
        }

        static void BubbleSort(List<ModelPreprocessorConfig> intArray)
        {
            ModelPreprocessorConfig temp = CreateInstance<ModelPreprocessorConfig>();
            bool swapped;
            for (int i = 0; i < intArray.Count; i++)
            {
                swapped = false;
                for (int j = 0; j < intArray.Count - 1 - i; j++)
                    if (intArray[j].priority > intArray[j + 1].priority)
                    {
                        temp = intArray[j];
                        intArray[j] = intArray[j + 1];
                        intArray[j + 1] = temp;
                        if (!swapped)
                            swapped = true;
                    }
                if (!swapped)
                    return;
            }
        }

        static void BubbleSort(List<AudioPreprocessorConfig> intArray)
        {
            AudioPreprocessorConfig temp = CreateInstance<AudioPreprocessorConfig>();
            bool swapped;
            for (int i = 0; i < intArray.Count; i++)
            {
                swapped = false;
                for (int j = 0; j < intArray.Count - 1 - i; j++)
                    if (intArray[j].priority > intArray[j + 1].priority)
                    {
                        temp = intArray[j];
                        intArray[j] = intArray[j + 1];
                        intArray[j + 1] = temp;
                        if (!swapped)
                            swapped = true;
                    }
                if (!swapped)
                    return;
            }
        }

        static void BubbleSort(List<TexturePreprocessorConfig> intArray)
        {
            TexturePreprocessorConfig temp = CreateInstance<TexturePreprocessorConfig>();
            bool swapped;
            for (int i = 0; i < intArray.Count; i++)
            {
                swapped = false;
                for (int j = 0; j < intArray.Count - 1 - i; j++)
                    if (intArray[j].priority > intArray[j + 1].priority)
                    {
                        temp = intArray[j];
                        intArray[j] = intArray[j + 1];
                        intArray[j + 1] = temp;
                        if (!swapped)
                            swapped = true;
                    }
                if (!swapped)
                    return;
            }
        }

        #endregion



        /// <summary>
        /// 存储该控件的更改
        /// </summary>
        public void Save()
        {
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("保存成功");
        }

        /// <summary>
        /// 刷新该控件的更改
        /// </summary>
        public void Refresh()
        {
            RefreshListView_InputSettings((BasePreprocessorConfig)Selection.activeObject);
            Save();
            Debug.Log("刷新成功");
        }

        /// <summary>
        /// 获取当前插件的相对位置
        /// </summary>
        public static string GetAssetPreprocessingToolsFilePath()
        {
            return AssetDatabase.GUIDToAssetPath(AssetDatabase.FindAssets("AssetPreprocessingTools_Aiferu")[0]);
        }

        #region UIToolKit
        /// <summary>
        /// UIToolKit打开窗口方法
        /// </summary>
        //[MenuItem("AiferuFramework/UI Toolkit/AssetPreprocessingTools")]
        [MenuItem("AiferuFramework/库本身的功能/4.项目使用工具收集/4.美术资产导入工具", false, 4004)]
        public static void ShowExample()
        {
            AssetPreprocessingTools wnd = GetWindow<AssetPreprocessingTools>();
            wnd.titleContent = new GUIContent("AssetPreprocessingTools");
        }
        /// <summary>
        /// UIToolKit窗口初始化
        /// </summary>
        private void UIToolKitWindowsIntialize()
        {
            // Each editor window contains a root VisualElement object
            root = rootVisualElement;

            // Import UXML 
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(AssetPreprocessingToolsFilePath + "/UIToolKit/MainMenu/AssetPreprocessingTools.uxml");
            //克隆UXML文件中的东西并初始化
            visualTree.CloneTree(root);
        }
        /// <summary>
        /// UIToolkit引用属性初始化
        /// </summary>
        void UIToolKitPropertyIntialize()
        {
            ALLListViewIntialize();

            AllButtonIntialize();

            AllLabelIntialize();

            AllTextFieldIntialize();
            //临时的东西
            m_VE_JudgeMentCondition = root.Q<VisualElement>("VE_JudgementCondition");
            m_listView_JudgeMentCondition = root.Q<ListView>("ListView_JudgementCondition");

        }


        #endregion

        #region 初始化所有的ListView
        private void ALLListViewIntialize()
        {
            m_listView_InputSettings_Mesh_Intialize();
            m_listView_InputSettings_Audio_Intialize();
            m_listView_InputSettings_Texture_Intialize();
        }

        #region 初始化 ListView_InputSettings_Mesh
        /// <summary>
        /// 初始化InputSettingsIntialize
        /// </summary>
        private void m_listView_InputSettings_Mesh_Intialize()
        {
            m_listView_InputSettings_Mesh = root.Q<ListView>("ListView_InputSettings_Mesh");
            //将ListView的子对象初始化 MakeListItem方法定义了当前ListView的子对象为什么类型
            m_listView_InputSettings_Mesh.makeItem = ListView_InputSettings_Mesh_MakeListItem;
            //为ListView的每个子对象赋值
            m_listView_InputSettings_Mesh.bindItem = ListView_InputSettings_Mesh_BindListItem;
            //当ListView中的选择改变时调用
            m_listView_InputSettings_Mesh.onSelectionChange += ListView_InputSettings_Mesh_onSelectionChange;
        }

        /// <summary>
        /// 当LIstView中的选择改变时调用
        /// </summary>
        /// <param name="obj"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void ListView_InputSettings_Mesh_onSelectionChange(IEnumerable<object> obj)
        {

            m_button_AddJudgeMentConditionItemConfig.SetEnabled(true);

            foreach (var item in obj)
            {
                //var go = CreateInstance<BasePreprocessorConfig>();
                RefreshListView_InputSettings((BasePreprocessorConfig)item);

                //将返回的对象设置为选中对象
                Selection.activeObject = (UnityEngine.Object)item;
                //显示文件类型
                m_label_FileType.text = "文件类型:" + ((BasePreprocessorConfig)item).FileType.ToString();
                m_intergerField_Priorty_Intialize((ModelPreprocessorConfig)item);

            }
        }

        /// <summary>
        /// 为ListView的每个子对象赋值
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void ListView_InputSettings_Mesh_BindListItem(VisualElement arg1, int arg2)
        {
            Label label = arg1 as Label;
            var go = m_inputSettingsFile_Mesh[arg2];
            label.text = go.name;
        }

        /// <summary>
        /// 定义当前ListView的子对象
        /// </summary>
        /// <returns></returns>
        private VisualElement ListView_InputSettings_Mesh_MakeListItem()
        {
            Label label = new Label();
            return label;
        }
        #endregion

        #region 初始化 ListView_InputSettings_Audio
        /// <summary>
        /// 初始化InputSettingsIntialize
        /// </summary>
        private void m_listView_InputSettings_Audio_Intialize()
        {
            m_listView_InputSettings_Audio = root.Q<ListView>("ListView_InputSettings_Audio");
            //将ListView的子对象初始化 MakeListItem方法定义了当前ListView的子对象为什么类型
            m_listView_InputSettings_Audio.makeItem = ListView_InputSettings_Audio_MakeListItem;
            //为ListView的每个子对象赋值
            m_listView_InputSettings_Audio.bindItem = ListView_InputSettings_Audio_BindListItem;
            //当ListView中的选择改变时调用
            m_listView_InputSettings_Audio.onSelectionChange += ListView_InputSettings_Audio_onSelectionChange;
        }

        /// <summary>
        /// 当LIstView中的选择改变时调用
        /// </summary>
        /// <param name="obj"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void ListView_InputSettings_Audio_onSelectionChange(IEnumerable<object> obj)
        {

            m_button_AddJudgeMentConditionItemConfig.SetEnabled(true);
            foreach (var item in obj)
            {
                //var go = CreateInstance<BasePreprocessorConfig>();
                RefreshListView_InputSettings((BasePreprocessorConfig)item);

                //将返回的对象设置为选中对象
                Selection.activeObject = (UnityEngine.Object)item;
                //显示文件类型
                m_label_FileType.text = "文件类型:" + ((BasePreprocessorConfig)item).FileType.ToString();
                m_intergerField_Priorty_Intialize((AudioPreprocessorConfig)item);
            }
        }

        /// <summary>
        /// 为ListView的每个子对象赋值
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void ListView_InputSettings_Audio_BindListItem(VisualElement arg1, int arg2)
        {
            Label label = arg1 as Label;
            var go = m_inputSettingsFile_Audio[arg2];
            label.text = go.name;
        }

        /// <summary>
        /// 定义当前ListView的子对象
        /// </summary>
        /// <returns></returns>
        private VisualElement ListView_InputSettings_Audio_MakeListItem()
        {
            Label label = new Label();
            return label;
        }
        #endregion

        #region 初始化 ListView_InputSettings_Texture
        /// <summary>
        /// 初始化InputSettingsIntialize
        /// </summary>
        private void m_listView_InputSettings_Texture_Intialize()
        {
            m_listView_InputSettings_Texture = root.Q<ListView>("ListView_InputSettings_Texture");
            //将ListView的子对象初始化 MakeListItem方法定义了当前ListView的子对象为什么类型
            m_listView_InputSettings_Texture.makeItem = ListView_InputSettings_Texture_MakeListItem;
            //为ListView的每个子对象赋值
            m_listView_InputSettings_Texture.bindItem = ListView_InputSettings_Texture_BindListItem;
            //当ListView中的选择改变时调用
            m_listView_InputSettings_Texture.onSelectionChange += ListView_InputSettings_Texture_onSelectionChange;
        }

        /// <summary>
        /// 当LIstView中的选择改变时调用
        /// </summary>
        /// <param name="obj"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void ListView_InputSettings_Texture_onSelectionChange(IEnumerable<object> obj)
        {

            m_button_AddJudgeMentConditionItemConfig.SetEnabled(true);
            foreach (var item in obj)
            {
                //var go = CreateInstance<BasePreprocessorConfig>();
                RefreshListView_InputSettings((BasePreprocessorConfig)item);

                //将返回的对象设置为选中对象
                Selection.activeObject = (UnityEngine.Object)item;
                //显示文件类型
                m_label_FileType.text = "文件类型:" + ((BasePreprocessorConfig)item).FileType.ToString();
                m_intergerField_Priorty_Intialize((TexturePreprocessorConfig)item);
            }
        }

        /// <summary>
        /// 为ListView的每个子对象赋值
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void ListView_InputSettings_Texture_BindListItem(VisualElement arg1, int arg2)
        {
            Label label = arg1 as Label;
            var go = m_inputSettingsFile_Texture[arg2];
            label.text = go.name;
        }

        /// <summary>
        /// 定义当前ListView的子对象
        /// </summary>
        /// <returns></returns>
        private VisualElement ListView_InputSettings_Texture_MakeListItem()
        {
            Label label = new Label();
            return label;
        }






        #endregion

        #region 通用方法

        /// <summary>
        /// 刷新ListView_InputSettings
        /// </summary>
        /// <param name="basePreprocessor"></param>
        private void RefreshListView_InputSettings(BasePreprocessorConfig basePreprocessor)
        {
            m_listView_JudgeMentCondition.hierarchy.Clear();
            switch (basePreprocessor.FileType)
            {
                case BasePreprocessorConfig.InputFileType.Texture:
                    List<JudgeMentConditionItemConfig> JudgeMentConditionItemConfigs1 = (((TexturePreprocessorConfig)basePreprocessor).judgeMentConditionItemConfigs);
                    ShowJudgeMentConditionItemCongfigs(JudgeMentConditionItemConfigs1);
                    break;
                case BasePreprocessorConfig.InputFileType.Model:
                    List<JudgeMentConditionItemConfig> JudgeMentConditionItemConfigs2 = (((ModelPreprocessorConfig)basePreprocessor).judgeMentConditionItemConfigs);
                    ShowJudgeMentConditionItemCongfigs(JudgeMentConditionItemConfigs2);
                    break;
                case BasePreprocessorConfig.InputFileType.Audio:
                    List<JudgeMentConditionItemConfig> JudgeMentConditionItemConfigs3 = (((AudioPreprocessorConfig)basePreprocessor).judgeMentConditionItemConfigs);
                    ShowJudgeMentConditionItemCongfigs(JudgeMentConditionItemConfigs3);
                    break;
                default:
                    Debug.Log("BasePreprocessorConfig.cs的代码可能有问题");
                    break;
            }
        }

        /// <summary>
        /// 显示判断条件框
        /// </summary>
        /// <param name="JudgeMentConditionItemConfigs"></param>
        private void ShowJudgeMentConditionItemCongfigs(List<JudgeMentConditionItemConfig> JudgeMentConditionItemConfigs)
        {

            foreach (var item in JudgeMentConditionItemConfigs)
            {
                JudgeMentConditionItem VE_JudgeMentConditionItem = new JudgeMentConditionItem(AssetPreprocessingToolsFilePath, item, (AssetPreprocessingTools)this);
                //m_VE_JudgeMentCondition.Add(VE_JudgeMentConditionItem);
                m_listView_JudgeMentCondition.hierarchy.Add(VE_JudgeMentConditionItem);
            }

        }

        #endregion

        #endregion

        #region 初始化所有的Button
        /// <summary>
        /// 初始化所有的Button
        /// </summary>
        private void AllButtonIntialize()
        {
            //
            m_button_AddJudgeMentConditionItemConfig = root.Q<Button>("Button_AddJudgeMentConditionItemConfig");
            m_button_AddJudgeMentConditionItemConfig.clicked += button_AddJudgeMentConditionItemConfig_OnClicked;
            m_button_AddJudgeMentConditionItemConfig.SetEnabled(false);

            m_button_Save = root.Q<Button>("Button_Save");
            m_button_Save.clicked += button_Save_OnClicked;

            m_button_Clear = root.Q<Button>("Button_Clear");
            m_button_Clear.clicked += button_Clear_OnClicked;

            m_button_NewInputSetting = root.Q<Button>("Button_NewInputSetting");
            m_button_NewInputSetting.clicked += button_NewInputSetting_OnClicked;

            m_button_Refresh = root.Q<Button>("Button_Refresh");
            m_button_Refresh.clicked += button_Refresh_OnClicked;


        }

        private void button_Refresh_OnClicked()
        {
            //刷新

            Refresh();
        }

        private void button_NewInputSetting_OnClicked()
        {
            ////新建序列化文件
            //string assetsPath = ToolsFilePath + "/Data/InputSettings";
            //#region 创建对应文件
            ////创建对应序列化对象
            //JudgeMentConditionItemConfig judgeMentConditionItemConfigdata = ScriptableObject.CreateInstance<JudgeMentConditionItemConfig>();

            ////检查保存路径
            //if (!Directory.Exists(assetsPath))
            //{
            //    Directory.CreateDirectory(assetsPath);
            //}
            //int ID = 0;
            //string fullPath = assetsPath + "/" + "JudgeMentConditionItemConfigData0.asset";

            ////生成不重复的文件名
            //GenerateNonuplicateFilenames(assetsPath, ref ID, ref fullPath);

            //judgeMentConditionItemConfigdata.filePath = fullPath;

            ////删除原有文件,生成新文件
            //AssetDatabase.DeleteAsset(fullPath);
            //AssetDatabase.CreateAsset(judgeMentConditionItemConfigdata, fullPath);
            //AssetDatabase.Refresh();
            //Debug.Log("创建判断条件成功");
            //#endregion


            //刷新
            Refresh();
        }

        private void button_Clear_OnClicked()
        {
            ////存储文件
            ////文件处理
            string configAssetFilePath = AssetPreprocessingToolsFilePath + "/CompiledAllJudgeMentConditionItemConfigData/";
            string configAssetPath = configAssetFilePath + "CompiledAllJudgeMentConditionItemConfig.asset";

            if (!Directory.Exists(configAssetFilePath))
            {
                Directory.CreateDirectory(configAssetFilePath);
            }
            if (!File.Exists(configAssetPath))
            {
                CompiledAllJudgeMentConditionItemConfig compiledAllJudgeMentConditionItemConfig = CreateInstance<CompiledAllJudgeMentConditionItemConfig>();
                ////删除原有文件,生成新文件
                AssetDatabase.DeleteAsset(configAssetPath);
                //Debug.Log(configAssetPath);
                AssetDatabase.CreateAsset(compiledAllJudgeMentConditionItemConfig, configAssetPath);
                AssetDatabase.Refresh();
                //Debug.Log("创建判断条件成功");
                //Debug.Log("保存成功");
                EditorUtility.SetDirty(compiledAllJudgeMentConditionItemConfig);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }

        }

        private void button_Save_OnClicked()
        {
            string configAssetFilePath = AssetPreprocessingToolsFilePath + "/Data/CompiledAllJudgeMentConditionItemConfigData/";
            string configAssetPath = configAssetFilePath + "CompiledAllJudgeMentConditionItemConfig.asset";
            CompiledAllJudgeMentConditionItemConfig com = AssetDatabase.LoadAssetAtPath<CompiledAllJudgeMentConditionItemConfig>(configAssetPath);
            com.TexturteJudgeMent = m_inputSettingsFile_Texture;
            com.MeshJudgeMent = m_inputSettingsFile_Mesh;
            com.AudioJudgeMent = m_inputSettingsFile_Audio;
            EditorUtility.SetDirty(com);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        //Button事件
        private void button_AddJudgeMentConditionItemConfig_OnClicked()
        {
            string assetsPath = AssetPreprocessingToolsFilePath + "/Data/JudgeMentConditionItemConfigData";
            #region 创建对应文件
            //创建对应序列化对象
            JudgeMentConditionItemConfig judgeMentConditionItemConfigdata = ScriptableObject.CreateInstance<JudgeMentConditionItemConfig>();

            //检查保存路径
            if (!Directory.Exists(assetsPath))
            {
                Directory.CreateDirectory(assetsPath);
            }
            int ID = 0;
            string fullPath = assetsPath + "/" + "JudgeMentConditionItemConfigData0.asset";

            //生成不重复的文件名
            GenerateNonuplicateFilenames(assetsPath, ref ID, ref fullPath);

            judgeMentConditionItemConfigdata.filePath = fullPath;

            //删除原有文件,生成新文件
            AssetDatabase.DeleteAsset(fullPath);
            AssetDatabase.CreateAsset(judgeMentConditionItemConfigdata, fullPath);
            AssetDatabase.Refresh();
            Debug.Log("创建判断条件成功");
            #endregion

            #region 添加对配置文件的引用

            switch (((BasePreprocessorConfig)(Selection.activeObject)).FileType)
            {
                case BasePreprocessorConfig.InputFileType.Texture:
                    ((TexturePreprocessorConfig)Selection.activeObject).judgeMentConditionItemConfigs.Add(judgeMentConditionItemConfigdata);

                    break;
                case BasePreprocessorConfig.InputFileType.Model:
                    ((ModelPreprocessorConfig)Selection.activeObject).judgeMentConditionItemConfigs.Add(judgeMentConditionItemConfigdata);
                    break;
                case BasePreprocessorConfig.InputFileType.Audio:
                    ((AudioPreprocessorConfig)Selection.activeObject).judgeMentConditionItemConfigs.Add(judgeMentConditionItemConfigdata);
                    break;
                default:
                    Debug.Log("BasePreprocessorConfig.cs的代码可能有问题");
                    break;
            }
            //保存序列化对象
            EditorUtility.SetDirty(Selection.activeObject);
            Refresh();

            #endregion

        }

        /// <summary>
        /// 生成不重复的文件名
        /// </summary>
        /// <param name="assetsPath"></param>
        /// <param name="ID"></param>
        /// <param name="fullPath"></param>
        private static void GenerateNonuplicateFilenames(string assetsPath, ref int ID, ref string fullPath)
        {
            if (File.Exists(fullPath))
            {
                fullPath = assetsPath + "/" + "JudgeMentConditionItemConfigData" + ID++.ToString() + ".asset";
                GenerateNonuplicateFilenames(assetsPath, ref ID, ref fullPath);
            }
        }
        #endregion

        #region 初始化所有的Label
        /// <summary>
        /// 初始化所有的Label
        /// </summary>
        private void AllLabelIntialize()
        {
            m_label_FileType = root.Q<Label>("Label_FileType");
        }
        #endregion

        #region 初始化所有的TextField

        private void AllTextFieldIntialize()
        {
        }

        private void m_intergerField_Priorty_Intialize(AudioPreprocessorConfig audioPreprocessorConfig)
        {
            m_integerField_Priority = root.Q<IntegerField>("IntegerField_Priority");
            SerializedObject so = new SerializedObject(audioPreprocessorConfig);
            m_integerField_Priority.Bind(so);
            m_integerField_Priority.bindingPath = "priority";
        }
        private void m_intergerField_Priorty_Intialize(ModelPreprocessorConfig audioPreprocessorConfig)
        {
            m_integerField_Priority = root.Q<IntegerField>("IntegerField_Priority");
            SerializedObject so = new SerializedObject(audioPreprocessorConfig);
            m_integerField_Priority.Bind(so);
            m_integerField_Priority.bindingPath = "priority";

        }
        private void m_intergerField_Priorty_Intialize(TexturePreprocessorConfig audioPreprocessorConfig)
        {
            m_integerField_Priority = root.Q<IntegerField>("IntegerField_Priority");
            SerializedObject so = new SerializedObject(audioPreprocessorConfig);
            m_integerField_Priority.Bind(so);
            m_integerField_Priority.bindingPath = "priority";
        }
        #endregion
    }
}
#endif