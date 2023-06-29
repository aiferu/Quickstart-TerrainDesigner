//
//  ReplacesTheShaderForTheSelectedMaterial.cs
//  AiferuFramework
//
//  Created by Aiferu on 2023/2/7.
//
//
#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
namespace AiferuFramework.AssetsManagementTools
{
    /// <summary>
    /// 
    /// </summary>
    public class ReplacesTheShaderForTheSelectedMaterial : AssetsTools
    {
        #region Field
        Shader shader = null;
        List<string> allMaterialPath = new List<string>();
        #endregion

        #region Property

        #endregion
        [MenuItem("AssetsTools/2002.将当前选中文件夹中所有材质的Shader替换为指定的Shader", false, 2002)]
        static void Fmt()
        {
            EditorWindow.GetWindow(typeof(ReplacesTheShaderForTheSelectedMaterial));
        }

        private void OnGUI()
        {
            GUILayout.Label("将当前选中文件夹中的所有材质的shder替换为指定的shader");
            GUILayout.Label("需要替换的着色器:");
            shader = (Shader)EditorGUILayout.ObjectField(shader, typeof(Shader), true);
            allMaterialPath.Clear();
            if (GUILayout.Button("替换着色器"))
            {
                if (GetSelectedFolderPath() != null)
                {
                    GetALLFilesMaterial(GetSelectedFolderPath(), out allMaterialPath);
                }
                foreach (var materialPath in allMaterialPath)
                {
                    //选择路径有.mat的文件
                    if (materialPath.EndsWith(".mat"))
                    {
                        Material material = AssetDatabase.LoadAssetAtPath<Material>(materialPath);
                        material.shader = shader;
                        Debug.Log(material.name);
                    }

                }


            }
        }

        #region Function


        #endregion
    }
}
#endif