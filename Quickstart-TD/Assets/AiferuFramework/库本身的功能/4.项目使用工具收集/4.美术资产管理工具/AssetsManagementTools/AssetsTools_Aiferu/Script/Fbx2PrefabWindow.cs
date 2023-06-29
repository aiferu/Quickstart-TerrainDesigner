#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace AiferuFramework.AssetsManagementTools
{
    public class Fbx2PrefabWindows : AssetsTools
    {
        private static List<string> allFbxPath = new List<string>();
        /// <summary>
        /// fbx放在资源根目录的fbx文件夹中，脚步会遍历该目录，将fbx转成prefab放在子目录prefab中
        /// </summary>
        [MenuItem("AssetsTools/2004.FBX转Profab", false, 2004)]
        static void Fbx2Prefab()
        {
            EditorWindow.GetWindow(typeof(Fbx2PrefabWindows));
        }

        private void OnGUI()
        {
            GUILayout.Label("确定要将当前所选文件夹中的模型转换成预制体吗?");
            if (GUILayout.Button("确定"))
            {
                //AllFilesPath.Clear();
                allFbxPath.Clear();
                //遍历选定文件夹下的所有文件
                if (GetSelectedAssetPath() != null)
                {
                    Debug.Log(GetSelectedAssetPath());
                    GetALLFilesFBX(GetSelectedAssetPath(), out allFbxPath);
                    foreach (string fbxFilePath in allFbxPath)
                    {
                        //选择路径有.fbx,且没有animation的文件
                        if (fbxFilePath.EndsWith(".FBX") && !fbxFilePath.Contains("Animation") || fbxFilePath.EndsWith(".fbx") && !fbxFilePath.Contains("Animation"))
                        {

                            //创建Profab
                            GameObject pre = AssetDatabase.LoadAssetAtPath<GameObject>(fbxFilePath);
                            GameObject pre2 = MonoBehaviour.Instantiate(pre);
                            string PrefilePath1 = fbxFilePath.Substring(0, fbxFilePath.IndexOf(pre.name));
                            string PrefilePath2 = PrefilePath1.Replace("Assets", "Assets/000-Profabs");
                            Debug.Log(PrefilePath1);
                            Debug.Log(PrefilePath2);
                            if (!Directory.Exists(PrefilePath2))
                            {
                                //路径不存在创建路径
                                Directory.CreateDirectory(PrefilePath2);
                            }

                            //创建预制体文件
                            if (!File.Exists(PrefilePath2 + pre.name + ".prefab"))
                            {
                                PrefabUtility.SaveAsPrefabAsset(pre2, PrefilePath2 + pre.name + ".prefab");
                                // Debug.Log( PrefilePath2 + pre.name + ".prefab");
                            }
                            else
                            {
                                PrefabUtility.SaveAsPrefabAsset(pre2, PrefilePath2 + pre.name + "(Clone)" + ".prefab");
                                //  Debug.Log(PrefilePath2 + pre.name + "(Clone)" + ".prefab");
                            }
                            MonoBehaviour.DestroyImmediate(pre2);
                        }
                    }
                }
            }
        }

    }
}

#endif