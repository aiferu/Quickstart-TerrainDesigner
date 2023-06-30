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
using UnityEditor.SceneManagement;
using UnityEditor.SearchService;
using AiferuFramework.ArtBrushTool;

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
            "00-ArtAssets/Assets_Scene_Profab",
            "01-Scenes",
        };

        private static string[] SceneProfabFolderNames = {
            "00-ArtAssets/Assets_Scene_Profab/[SceneName]",
            "00-ArtAssets/Assets_Scene_Profab/[SceneName]/Object",
            "00-ArtAssets/Assets_Scene_Profab/[SceneName]/Obsolete",
            "00-ArtAssets/Assets_Scene_Profab/[SceneName]/SceneArchitecture",
            "00-ArtAssets/Assets_Scene_Profab/[SceneName]/SceneArchitecture/Base",
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



        [MenuItem("Assets/创建场景预制体存放路径", true)]
        private static bool ValidateCreateSceneProfabFolder()
        {
            if (Selection.activeObject == null)
            {
                return false;
            }
            return Selection.activeObject.GetType() == typeof(SceneAsset);
        }
        /// <summary>
        /// 创建场景预制体存放路径,会根据当前选中场景进行改变
        /// </summary>
        [MenuItem("Assets/创建场景预制体存放路径")]
        private static void CreateSceneProfabFolder()
        {
            //获取当前选中文件的路径
            if (GetSelectedFolderPath() != null)
            {
                string respath = Application.dataPath;
                AlternateText(ref SceneProfabFolderNames, "[SceneName]", Selection.activeObject.name);
                foreach (var resourcesFolderName in SceneProfabFolderNames)
                {
                    string path = respath + "/"+ resourcesFolderName;
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

        public static void AlternateText(ref string[]TextList,string oldText,string newText)
        {
            for (int i = 0; i < TextList.Length; i++)
            {
                TextList[i] = TextList[i].Replace(oldText, newText);
            }
        }


        


        public class CreateSceneArchitectureObjectEW : EditorWindow
        {
            private static string layerName = "[LayerName]";
            [MenuItem("GameObject/AFramework/创建Scene架构对象")]
            static void Open()
            {
                #region 窗口初始化
                CreateSceneArchitectureObjectEW ewIns = (CreateSceneArchitectureObjectEW)EditorWindow.GetWindowWithRect(typeof(CreateSceneArchitectureObjectEW), new Rect(0, 0, 300, 100), false, "SetLayerName");
                #endregion
            }

            private void OnGUI()
            {
                GUILayout.Space(20);
                GUILayout.BeginHorizontal();
                GUILayout.Space(20);
                GUILayout.Label("LayerName:");
                layerName = GUILayout.TextField(layerName);
                GUILayout.Space(20);
                GUILayout.EndHorizontal();
                GUILayout.Space(20);
                GUILayout.BeginHorizontal();
                GUILayout.Space(20);
                if (GUILayout.Button("Creation"))
                {
                    CreateSceneArchitectureObject(layerName);
                    Close();
                }
                GUILayout.Space(20);
                GUILayout.EndHorizontal() ;
                GUILayout.Space(20);
            }

            private static void CreateSceneArchitectureObject(string layerName)
            {
                GameObject go1 = new GameObject("---------------"+layerName+"---------------");
                GameObject go2 = new GameObject("======"+layerName+"_ART======");
                GameObject go3 = new GameObject("======"+layerName+"_Interaction======");
                GameObject go2_1 = new GameObject("====Enviroment====");
                go2_1.transform.parent = go2.transform;
                GameObject go2_2 = new GameObject("====Render====");
                go2_2.transform.parent = go2.transform;
                GameObject go4 = new GameObject("======"+layerName+"_Audio======");
            }

        }
        #endregion

        #region Function
        #endregion
    }
}
#endif