//
//  CreateAssetFolder.cs
//  AiferuFramework
//
//  Created by Aiferu on 2023/2/8.
//
//
#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
namespace AiferuFramework.AssetsManagementTools
{
    /// <summary>
    /// 
    /// </summary>
    public class CreateFolder : AssetsTools
    {
        #region Field
        private static string[] artResourcesFolderNames = {
            "Material",
            "Mesh",
            "Mesh/StaticMesh",
            "Mesh/Human",
            "Mesh/Human/Animation",
            "Mesh/NonHuman",
            "Mesh/NonHuman/Animation",
            "Texture",
            "UI",
            "Audio",
            "Audio/LongSound",
            "Audio/Sound",
            "Audio/Background",
            "Prefab",
            "Shader",
        };

        private static string[] ProjectFolderNames = {
            "00-ArtAssets",
            "00-ArtAssets/1stArtAssets",
            "00-ArtAssets/3rdArtAssets",
            "01-Scenes",
        };

        #endregion

        #region Property

        #endregion

        #region UnityOriginalEvent
        /// <summary>
        /// 创建美术资产文件夹
        /// </summary>
        [MenuItem("Assets/在当前选中文件夹下创建美术资产文件夹")]
        private static void CreateArtAssetsFolderFunc()
        {
            //获取当前选中文件的路径
            if (GetSelectedFolderPath() != null)
            {
                string respath = Application.dataPath + GetSelectedFolderPath().Remove(0, 6) + "/GameResources/";

                foreach (var resourcesFolderName in artResourcesFolderNames)
                {
                    string path = respath + resourcesFolderName;
                    //判断文件夹是否存在
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                        Debug.Log("文件夹创建成功:" + path);
                        AssetDatabase.Refresh();

                    }
                }
            }
        }

        /// <summary>
        /// 创建项目默认文件夹
        /// </summary>
        [MenuItem("AssetsTools/2001.创建项目美术资产目录文件夹", false, 2001)]
        private static void CreateProjectFolderFunc()
        {

            string respath = Application.dataPath;

            foreach (var resourcesFolderName in ProjectFolderNames)
            {
                string path = respath + "/" + resourcesFolderName;
                Debug.Log(path);
                //判断文件夹是否存在
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                    Debug.Log("文件夹创建成功:" + path);
                    AssetDatabase.Refresh();
                }
                else
                {
                    Debug.Log("hhh");
                }
            }

        }

        #endregion

        #region Function
        #endregion
    }
}
#endif