//
//  EditorBase.cs
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
using System.IO;
using System;

namespace AiferuFramework
{
    /// <summary>
    /// 编辑器脚本基类
    /// </summary>
    public class EditorBase
    {
        #region Field

        #endregion

        #region Property

        #endregion

        #region UnityOriginalEvent

        #endregion

        #region Function

        /// <summary>
        /// 获取当前插件的相对位置
        /// </summary>
        /// <param name="key">当前工具的父文件夹名称</param>
        /// <returns></returns>
        public static string GetAssetPreprocessingToolsFilePath(string key)
        {
            return AssetDatabase.GUIDToAssetPath(AssetDatabase.FindAssets(key)[0]); ;
        }

        /// <summary>
        /// 获取输入路径下的所有预制体,路径缺省默认返回所有预制体
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns>预制体对象链表</returns>
        public static List<GameObject> GetAllProfabFromPath(string path = "Assets")
        {
            List<GameObject> prefabObject = new List<GameObject>();
            //遍历所有的Prefab
            string[] PrefabURL = AssetDatabase.FindAssets("t:Prefab", new string[] { path });
            foreach (var item in PrefabURL)
            {
                prefabObject.Add(AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.GUIDToAssetPath(item)));
            }
            if (prefabObject.Count == 0)
            {
                Debug.LogError("选定路径下没有预制体");
            }
            return prefabObject;
        }

        /// <summary>
        /// 获取输入路径下的所有动画片段,路径缺省默认返回所有动画片段
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns>动画片段对象链表</returns>
        public static List<AnimationClip> GetAllAnimationClipFromPath(string path = "Assets")
        {
            List<AnimationClip> AnimationClipList = new List<AnimationClip>();
            //遍历所有的AnimationClip
            string[] AnimationClipURl = AssetDatabase.FindAssets("t:AnimationClip", new string[] { path });
            foreach (var item in AnimationClipURl)
            {
                AnimationClipList.Add(AssetDatabase.LoadAssetAtPath<AnimationClip>(AssetDatabase.GUIDToAssetPath(item)));
            }
            if (AnimationClipList.Count == 0)
            {
                Debug.LogError("选定路径下没有动画片段");
            }
            return AnimationClipList;
        }

        /// <summary>
        /// 获取输入路径下的所有动画片段的路径,路径缺省默认返回所有动画片段
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns>动画片段对象路径链表</returns>
        public static List<string> GetAllAnimationClipPathFromPath(string path = "Assets")
        {
            List<string> AnimationClipPathList = new List<string>();
            //遍历所有的AnimationClip
            string[] AnimationClipURl = AssetDatabase.FindAssets("t:AnimationClip", new string[] { path });
            foreach (var item in AnimationClipURl)
            {
                AnimationClipPathList.Add(AssetDatabase.GUIDToAssetPath(item));
            }
            if (AnimationClipPathList.Count == 0)
            {
                Debug.LogError("选定路径下没有动画片段");
            }
            return AnimationClipPathList;
        }

        /// <summary>
        /// 生成在指定路径下不重复的文件名称
        /// </summary>
        /// <param name="assetsPath">指定的文件夹路径</param>
        /// <param name="fileName">要生成的基础文件名称</param>
        /// <param name="fileSuffix">要保存的文件后缀,例如".assets"</param>
        /// <param name="ID">文件序号</param>
        /// <param name="fullPath">要保存的文件路径,包含文件名,函数会先判断是否有重名,没有的话就不会修改他</param>
        public static void GenerateNonuplicateFilenames(string assetsPath, string fileName, string fileSuffix, ref int ID, ref string fullPath)
        {
            if (File.Exists(fullPath))
            {
                fullPath = assetsPath + "/" + fileName + ID++.ToString() + fileSuffix;
                GenerateNonuplicateFilenames(assetsPath, fileName, fileSuffix, ref ID, ref fullPath);
            }
        }

        /// <summary>
        /// 创建序列化对象文件,默认会自动规避同名文件,可关闭
        /// </summary>
        /// <param name="dataFileName">数据文件名称不带后缀</param>
        /// <param name="dataFilePath">数据文件保存文件夹路径</param>
        /// <param name="dataFileSuffix">数据文件后缀</param>
        /// <param name="ObjectData">序列化对象文件</param>
        /// <param name="AvoidFilesWithTheSameName">规避同名文件,默认为false</param>
        /// <returns>返回文件存放路径</returns>
        public static string CreateScriptableObjectFile(string dataFileName, string dataFilePath, string dataFileSuffix, UnityEngine.Object ObjectData, bool AvoidFilesWithTheSameName = false)
        {

            //检查保存路径
            if (!Directory.Exists(dataFilePath))
            {
                Directory.CreateDirectory(dataFilePath);
            }

            int ID = 0;

            string fullPath = dataFilePath + "/" + dataFileName + dataFileSuffix;

            //生成不重复的文件名
            if (AvoidFilesWithTheSameName)
            {
                EditorBase.GenerateNonuplicateFilenames(dataFilePath, dataFileName, dataFileSuffix, ref ID, ref fullPath);
            }
            //Debug.Log(fullPath);
            //删除原有文件,生成新文件
            AssetDatabase.DeleteAsset(fullPath);
            AssetDatabase.CreateAsset(ObjectData, fullPath);
            AssetDatabase.Refresh();

            Debug.Log("创建数据文件成功:" + dataFileName);
            return fullPath;
        }

        /// <summary>
        /// 获取对应路径下所有的序列化对象的AssetDatabase加载路径
        /// </summary>
        /// <param name="dataFilePath">数据路径</param>
        /// <returns>对应路径下所有序列化对象的AssetDatabase加载路径</returns>
        public static List<string> GetAllScriptableObjectPathFormPath(string dataFilePath)
        {
            List<string> ScriptableObjectList = new List<string>();
            try
            {
                DirectoryInfo dir = new DirectoryInfo(dataFilePath);
                FileSystemInfo[] fileinfo = dir.GetFileSystemInfos();  //返回目录中所有文件和子目录
                foreach (FileSystemInfo i in fileinfo)
                {
                    //Debug.Log(i.ToString());
                    if (!(i is DirectoryInfo) && i.ToString().EndsWith(".asset"))//当不是文件夹时
                    {

                        //Debug.Log(i.ToString().Replace("\\", "/").Replace(Application.dataPath, "Assets"));
                        string objectpath = (i.ToString().Replace("\\", "/").Replace(Application.dataPath, "Assets"));
                        ScriptableObjectList.Add(objectpath);
                        //Debug.Log(@object.name);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                throw;
            }
            if (ScriptableObjectList.Count <= 0)
            {
                Debug.LogError("当前路径下没有序列化对象:" + dataFilePath);
            }
            return ScriptableObjectList;
        }

        /// <summary>
        /// 删除指定路径下的所有文件
        /// 当路径指向文件夹时,会删除该文件夹下的所有子对象,但保留该文件夹
        /// 当路径指向一个文件时,会删除该文件
        /// </summary>
        /// <param name="srcPath">指定路径</param>
        public static void DelectDir(string srcPath)
        {
            try
            {
                if (Directory.Exists(srcPath) || File.Exists(srcPath))
                {
                    FileAttributes attr = File.GetAttributes(srcPath);//获取文件属性
                    if (attr == FileAttributes.Directory)//当为文件夹时,获取下面的所有文件和文件夹
                    {
                        DirectoryInfo dir = new DirectoryInfo(srcPath);
                        FileSystemInfo[] fileinfo = dir.GetFileSystemInfos();  //返回目录中所有文件和子目录
                        foreach (FileSystemInfo i in fileinfo)
                        {
                            if (i is DirectoryInfo)            //判断是否文件夹
                            {
                                DirectoryInfo subdir = new DirectoryInfo(i.FullName);
                                subdir.Delete(true);          //删除子目录和文件
                            }
                            else
                            {
                                File.Delete(i.FullName);      //删除指定文件
                            }
                        }
                    }
                    else
                    {
                        File.Delete(srcPath);
                    }
                    AssetDatabase.Refresh();
                    Debug.Log("删除指定路径下的文件成功:" + srcPath);
                }
                else
                {
                    Debug.Log("需要删除的文件夹或文件不存在:" + srcPath);
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                throw;
            }
        }



        #endregion
    }
}
#endif