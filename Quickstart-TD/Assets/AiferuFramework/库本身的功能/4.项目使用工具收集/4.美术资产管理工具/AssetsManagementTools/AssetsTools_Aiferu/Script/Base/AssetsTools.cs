#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections;
using System.Collections.Generic;

namespace AiferuFramework.AssetsManagementTools
{
    /// <summary>
    /// 该类中将存放用于资产操作的工具方法
    /// /// </summary>
    public class AssetsTools : EditorWindow
    {
        #region Field
        public static List<string> AllMaterialPath = new List<string>();
        public static List<string> AllFbxPath = new List<string>();
        public static List<string> AllFilesPath = new List<string>();
        private static string assetsToolsPath;
        #endregion

        #region Property
        public static string AssetsToolsPath
        {
            get
            {
                return AssetDatabase.GUIDToAssetPath(AssetDatabase.FindAssets("AssetTools_Aiferu")[0]);
            }
        }
        #endregion


        /// <summary>
        /// 获取当前选中的文件夹或资源的路径
        /// </summary>
        /// <returns></returns>
        public static string GetSelectedAssetPath()
        {
            //支持多选
            string[] guids = Selection.assetGUIDs;//获取当前选中的asset的GUID
            if (guids != null)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guids[0]);//通过GUID获取路径
                return assetPath;
            }
            return null;
        }

        /// <summary>
        /// 获取当前选中文件的文件夹路径
        /// </summary>
        /// <returns></returns>
        public static string GetSelectedFolderPath()
        {
            foreach (var obj in Selection.GetFiltered<Object>(SelectionMode.Assets))
            {
                var path = AssetDatabase.GetAssetPath(obj);
                if (string.IsNullOrEmpty(path))
                    continue;

                if (System.IO.Directory.Exists(path))
                    return path;
                else if (System.IO.File.Exists(path))
                    return System.IO.Path.GetDirectoryName(path);
            }

            return "Assets";
        }

        /// <summary>
        /// 递归遍历指定路径下的所有FBX文件
        /// </summary>
        /// <param name="path">文件夹路径</param>
        public static void GetALLFilesFBX(string path, out List<string> allFbxPath)
        {
            AllFilesPath.Clear();
            allFbxPath = AllFbxPath;
            List<string> allFilesPath = new List<string>();
            GetALLFiles(path, out allFilesPath);
            foreach (var filePath in allFilesPath)
            {
                if (filePath.ToLower().EndsWith(".fbx"))//提取fbx文件
                {
                    allFbxPath.Add(filePath);
                }
            }
        }

        /// <summary>
        /// 递归遍历指定路径下的所有material文件
        /// </summary>
        /// <param name="path">文件夹路径</param>
        public static void GetALLFilesMaterial(string path, out List<string> allMaterialPath)
        {

            AllFilesPath.Clear();
            allMaterialPath = AllMaterialPath;
            List<string> allFilesPath = new List<string>();
            GetALLFiles(path, out allFilesPath);
            foreach (var filePath in allFilesPath)
            {
                if (filePath.EndsWith(".mat"))//提取材质文件
                {
                    allMaterialPath.Add(filePath);
                }
            }
        }

        /// <summary>
        /// 递归遍历文件夹下的所有文件,获取所有文件的路径
        /// </summary>
        /// <param name="path"></param>
        /// <param name="allFilesPath"></param>
        public static void GetALLFiles(string path, out List<string> allFilesPath)
        {
            allFilesPath = AllFilesPath;
            DirectoryInfo direction = new DirectoryInfo(path);
            //文件夹下一层的所有子文件
            //SearchOption.TopDirectoryOnly：这个选项只取下一层的子文件
            //SearchOption.AllDirectories：这个选项会取其下所有的子文件
            FileInfo[] files = null;
            try
            {
               files = direction.GetFiles("*", SearchOption.TopDirectoryOnly);
            }
            catch (System.Exception)
            {
                allFilesPath.Add(path);
                return;
            }
            
            //文件夹下一层的所有文件夹
            DirectoryInfo[] folders = direction.GetDirectories("*", SearchOption.TopDirectoryOnly);
            for (int i = 0; i < folders.Length; i++)
            {
                //folders[i].FullName：硬盘上的完整路径名称
                //folders[i].Name：文件夹名称
                int folderAssetsIndex = folders[i].FullName.IndexOf("Assets");
                //从Assets开始取路径
                string folderPath = folders[i].FullName.Substring(folderAssetsIndex);
                GetALLFiles(folderPath, out allFilesPath);//递归遍历所有子文件夹
            }
            for (int i = 0; i < files.Length; i++)
            {
                //files[i].FullName：硬盘上的完整路径名称，包括文件名(D:\Project\Test\Assets\Scripts\Font\Test.cs)
                //files[i].Name：文件名称 Test.cs"
                //files[i].DirectoryName：文件的存放路径(D:\UnityProject\Test\Assets\Scripts\Font\)
                if (files[i].Name.EndsWith(".meta"))//过滤meta文件
                {
                    continue;
                }
                string filePath = files[i].FullName.Substring(files[i].FullName.IndexOf("Assets"));
                allFilesPath.Add(filePath);
            }


        }

    }



}

#endif



