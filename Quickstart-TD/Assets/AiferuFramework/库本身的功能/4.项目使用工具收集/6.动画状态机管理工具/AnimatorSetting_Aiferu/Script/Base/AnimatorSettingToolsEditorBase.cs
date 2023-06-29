//
//  AnimatorSettingsEditorBase.cs
//  AiferuFramework
//
//  Created by Aiferu on 2023/2/28.
//
//
#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using AiferuFramework;
namespace AiferuFramework.AnimatorSettingTools
{
    /// <summary>
    /// 动画状态机管理工具的基础数据和方法
    /// </summary>
    public class AnimatorSettingToolsEditorBase : UIToolKitEditorBase
    {
        #region Field
        #endregion

        #region Property
        /// <summary>
        /// 当前插件路径
        /// </summary>
        public static string ToolsFilePath
        {
            get
            {
                return UIToolKitEditorBase.GetAssetPreprocessingToolsFilePath("AnimatorSetting_Aiferu");
            }

        }

        /// <summary>
        /// 当前插件蒙皮对象数据保存路径
        /// </summary>
        public static string FilePath_SkinObjectDataPath
        {
            get
            {
                return ToolsFilePath + "/Data/SkinObjectData";
            }
        }

        /// <summary>
        /// 当前插件动画素材保存路径
        /// </summary>
        public static string FilePath_AnimationClipDataPath
        {
            get
            {
                return "Assets";
            }
        }

        /// <summary>
        /// 当前插件UIToolsKit窗口对象路径
        /// </summary>
        public static string FilePath_ToolsWindowObjectPath
        {
            get
            {
                return ToolsFilePath + "/UIToolKit/AnimatorSettings/AnimatorSettingToolsWindows.uxml";
            }
        }

        /// <summary>
        /// 当前插件基础数据存储文件路径
        /// </summary>
        public static string FilePath_BaseDataPath
        {
            get
            {
                return ToolsFilePath + "/Data/AnimatorSettingToolsBaseData/AnimatorSettingToolsBaseData.asset";
            }
        }

        /// <summary>
        /// 当前插件蒙皮对象存储文件路径
        /// </summary>
        public static string FilePath_SkinObjectPath
        {
            get
            {
                return "Assets";
            }
        }

        /// <summary>
        /// 当前插件生成的动画状态机对象存储文件路径
        /// </summary>
        public static string FilePath_AnimatorSavePath
        {
            get
            {
                return "Assets/AAA_Animator/Animator";
            }
        }

        #endregion


        #region UnityOriginalEvent

        #endregion

        #region Function

        /// <summary>
        /// 输出对应路径下的所有蒙皮预制体
        /// </summary>
        /// <param name="path">路径变形</param>
        /// <returns></returns>
        public static List<GameObject> GetAllSkinProfabFormPath(string path = "Assets")
        {
            List<GameObject> list = new List<GameObject>();
            List<GameObject> list2 = new List<GameObject>();
            list = UIToolKitEditorBase.GetAllProfabFromPath(path);
            if (list == null)
            {
                return null;
            }
            foreach (var item in list)
            {
                if (item.name.ToLower().Contains("_skin") && !item.name.Contains("_teli"))
                {
                    //Debug.Log(item);
                    list2.Add(item);
                }
                else
                {
                    Debug.LogError("蒙皮预制体路径下的非蒙皮文件:" + item);
                }

            }
            if (list2.Count == 0)
            {
                Debug.LogError("选定路径中没有蒙皮预制体");

            }
            return list2;
        }

        /// <summary>
        /// 基于输入的蒙皮预制体链表,创建对应的动画状态机管理工具对象数据
        /// </summary>
        /// <param name="skinProfabList">蒙皮预制体链表</param>
        public static void CreateAnimatorSettingToolsObjectDataScriptableObjectFile(List<GameObject> skinProfabList)
        {
            foreach (var item in skinProfabList)
            {
                string dataFileName = "";
                string dataFilePath = AnimatorSettingToolsEditorBase.FilePath_SkinObjectDataPath;
                string dataFileSuffix = ".asset";

                //写入数据
                AnimatorSettingToolsObjectData animatorSettingToolsObjectData = ScriptableObject.CreateInstance<AnimatorSettingToolsObjectData>();
                animatorSettingToolsObjectData.SkinProfabName = item.name;
                animatorSettingToolsObjectData.SkinProfab = item;
                dataFileName = animatorSettingToolsObjectData.SkinProfabName;
                animatorSettingToolsObjectData.ObjectDataSavePath = dataFilePath + "/" + dataFileName + dataFileSuffix;
                //保存数据
                //创建对应序列化对象
                CreateScriptableObjectFile(dataFileName, dataFilePath, dataFileSuffix, animatorSettingToolsObjectData, false);
            }
        }

        /// <summary>
        /// 加载输入路径中的所有AnimatorSettingToolsObjectData对象
        /// </summary>
        /// <param name="dataFilePath">数据路径</param>
        /// <returns>输入路径中的所有AnimatorSettingToolsObjectData对象</returns>
        public static List<AnimatorSettingToolsObjectData> GetAllAnimatorSettingToolsObjectDataScriptableObjectFormPath(string dataFilePath)
        {
            List<AnimatorSettingToolsObjectData> animatorSettingToolsObjectList = new List<AnimatorSettingToolsObjectData>();
            List<string> scriptableObjectPathList = new List<string>();
            scriptableObjectPathList = EditorBase.GetAllScriptableObjectPathFormPath(dataFilePath);
            foreach (var scriptableObjectPath in scriptableObjectPathList)
            {
                animatorSettingToolsObjectList.Add(AssetDatabase.LoadAssetAtPath<AnimatorSettingToolsObjectData>(scriptableObjectPath));
            }

            return animatorSettingToolsObjectList;
        }

        /// <summary>
        /// 获取输入路径下的所有符合AnimatorSettingTools要求的动画片段,路径缺省默认返回所有动画片段
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns>动画片段对象链表</returns>
        public static List<AnimationClip> GetAnimatorSettingToolsAllAnimationClipFromPath(string path = "Assets")
        {
            List<AnimationClip> AnimationClipList = new List<AnimationClip>();
            List<AnimationClip> AnimationClipList2 = new List<AnimationClip>();

            AnimationClipList = EditorBase.GetAllAnimationClipFromPath(path);

            foreach (var animationClip in AnimationClipList)
            {
                if (animationClip.name.Contains("_SkinCopy"))
                {
                    AnimationClipList2.Add(animationClip);
                }
            }
            return AnimationClipList2;
        }

        /// <summary>
        /// 获取输入路径下的所有符合AnimatorSettingTools要求的动画片段路径,路径缺省默认返回所有动画片段路径
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns>动画片段对象路径链表</returns>
        public static List<string> GetAnimatorSettingToolsAllAnimationClipPathFromPath(string path = "Assets")
        {
            List<string> AnimationClipList = new List<string>();
            List<string> AnimationClipList2 = new List<string>();

            AnimationClipList = EditorBase.GetAllAnimationClipPathFromPath(path);
            foreach (var animationClip in AnimationClipList)
            {
                if (animationClip.Contains("_SkinCopy"))
                {
                    AnimationClipList2.Add(animationClip);
                }
            }
            return AnimationClipList2;
        }

        #endregion
    }
}
#endif