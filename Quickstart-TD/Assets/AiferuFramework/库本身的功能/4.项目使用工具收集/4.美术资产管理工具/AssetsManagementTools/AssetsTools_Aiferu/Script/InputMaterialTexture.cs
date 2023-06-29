#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace AiferuFramework.AssetsManagementTools
{
    /// <summary>
    /// 遍历当前选中文件夹中的所有材质,为其添加对应的贴图
    /// </summary>
    public class InputMaterialTexture
    {
        private static InputMaterialTextureConfig configData = null;
        /// <summary>
        /// 颜色贴图 -贴图的命名规则（材质_目标贴图后缀）
        /// </summary>
        private static string[] TextureNameKeyword_Albedo = new string[] { "_BassColor", "_BaseColor", "_Basecolor", "_BascColor", "_BaseCoor", "_BaseColo" };
        /// <summary>
        /// MAR贴图 金属度 环境光遮蔽 光滑度
        /// </summary>
        private static string[] TextureNameKeyword_MAR = new string[] { "_MAR" };
        /// <summary>
        /// 法线贴图
        /// </summary>
        private static string[] TextureNameKeyword_Normal = new string[] { "_Normal", "_normal" };
        /// <summary>
        /// 自发光贴图
        /// </summary>
        private static string[] TextureNameKeyword_Emissive = new string[] { "_Emissive" };
        /// <summary>
        /// 环境光遮蔽贴图
        /// </summary>
        private static string[] TextureNameKeyword_AmbientOcclusion = new string[] { "_AO", "_AmbientOcclusionTexture" };
        /// <summary>
        /// 光滑度/粗糙度贴图
        /// </summary>
        private static string[] TextureNameKeyword_Roughness = new string[] { "_Roughness", "_Smoothness" };
        /// <summary>
        /// 金属度贴图
        /// </summary>
        private static string[] TextureNameKeyword_Metallic = new string[] { "_Metallic" };

        private static string ShaderName_Albedo = "_BaseMap";
        private static string ShaderName_MAR = "_MARBlendMap";
        private static string ShaderName_Normal = "_BumpMap";
        private static string ShaderName_Emissive = "_EmissionMap";
        private static string ShaderName_AmbientOcclusion = "_OcclusionMap";
        private static string ShaderName_Roughness = "_SmoothnessMap";
        private static string ShaderName_MetallicTexture = "_MetallicGlossMap";


        [MenuItem("AssetsTools/2003.导入材质贴图", false, 2003)]
        private static void InputTextureToMaterial()
        {

            EditorWindow.GetWindow(typeof(InputTextureWindow));
        }

        private static void Initialize(InputMaterialTextureConfig configData)
        {
            TextureNameKeyword_Albedo = configData.TextureNameKeyword_Albedo;
            TextureNameKeyword_AmbientOcclusion = configData.TextureNameKeyword_AmbientOcclusion;
            TextureNameKeyword_Emissive = configData.TextureNameKeyword_Emissive;
            TextureNameKeyword_MAR = configData.TextureNameKeyword_MAR;
            TextureNameKeyword_Metallic = configData.TextureNameKeyword_Metallic;
            TextureNameKeyword_Normal = configData.TextureNameKeyword_Normal;
            TextureNameKeyword_Roughness = configData.TextureNameKeyword_Roughness;

            ShaderName_Albedo = configData.ShaderName_Albedo;
            ShaderName_AmbientOcclusion = configData.ShaderName_AmbientOcclusion;
            ShaderName_Emissive = configData.ShaderName_Emissive;
            ShaderName_MAR = configData.ShaderName_MAR;
            ShaderName_MetallicTexture = configData.ShaderName_MetallicTexture;
            ShaderName_Normal = configData.ShaderName_Normal;
            ShaderName_Roughness = configData.ShaderName_Roughness;


        }


        public class InputTextureWindow : EditorWindow
        {
            private static string configPath = "";
            string modelURL = "";
            string textureURL = "";

            private void OnGUI()
            {
                GUILayout.Label("模型路径:");
                modelURL = EditorGUILayout.TextField("", modelURL);
                GUILayout.Label("贴图路径:");
                textureURL = EditorGUILayout.TextField("", textureURL);
                GUILayout.Label("配置文件");
                configData = (InputMaterialTextureConfig)EditorGUILayout.ObjectField(configData, typeof(InputMaterialTextureConfig), true);
                GUILayout.Space(10);


                if (GUILayout.Button("赋值"))
                {
                    Initialize(configData);
                    InputTextureMain(modelURL, textureURL);
                }
            }
        }


        /// <summary>
        /// 遍历提供的路径中的所有文件,对材质和贴图做匹配
        /// </summary>
        /// <param name="modelURL">模型材质路径</param>
        /// <param name="textureURL">模型对应贴图路径</param>
        public static void InputTextureMain(string modelURL, string textureURL)
        {


            //获取所有的贴图
            string[] TextureFilesURL = AssetDatabase.FindAssets("t:Texture", new string[] { textureURL });

            //获取所有贴图URL
            for (int j = 0; j < TextureFilesURL.Length; j++)
            {
                TextureFilesURL[j] = AssetDatabase.GUIDToAssetPath(TextureFilesURL[j]);
                Debug.Log(TextureFilesURL[j]);
            }

            //获取所有材质
            string[] MaterialFilesURL = AssetDatabase.FindAssets("t:Material", new string[] { modelURL });

            //获取所有材质的URL
            for (int i = 0; i < MaterialFilesURL.Length; i++)
            {
                MaterialFilesURL[i] = AssetDatabase.GUIDToAssetPath(MaterialFilesURL[i]);
            }

            //开始遍历
            for (int i = 0; i < MaterialFilesURL.Length; i++)
            {
                //Debug.Log(MaterialFilesURL[i]);
                //获取材质编号
                //获取材质名称中的数字,并且只取前4个
                Material material = AssetDatabase.LoadAssetAtPath<Material>(MaterialFilesURL[i]);
                string ID = System.Text.RegularExpressions.Regex.Replace(material.name, @"[^0-9]+", "");
                if (ID.Length > 4)
                {
                    ID = ID.Substring(0, 4);
                }

                //遍历所有贴图,找到编号对应的贴图
                for (int j = 0; j < TextureFilesURL.Length; j++)
                {
                    //载入贴图
                    Texture texture = AssetDatabase.LoadAssetAtPath<Texture>(TextureFilesURL[j]);

                    //遍历贴图的类型
                    TextureApply(material, ID, texture, TextureNameKeyword_Albedo, ShaderName_Albedo);
                    TextureApply(material, ID, texture, TextureNameKeyword_AmbientOcclusion, ShaderName_AmbientOcclusion);
                    TextureApply(material, ID, texture, TextureNameKeyword_MAR, ShaderName_MAR);
                    TextureApply(material, ID, texture, TextureNameKeyword_Roughness, ShaderName_Roughness);
                    TextureApply(material, ID, texture, TextureNameKeyword_Normal, ShaderName_Normal);
                    TextureApply(material, ID, texture, TextureNameKeyword_Emissive, ShaderName_Emissive);
                    TextureApply(material, ID, texture, TextureNameKeyword_Metallic, ShaderName_MetallicTexture);
                }
            }
        }

        /// <summary>
        /// 将对应的贴图应用到材质上
        /// </summary>
        /// <param name="material">要替换贴图的材质</param>
        /// <param name="ID">材质的ID,由美术确定</param>
        /// <param name="texture">要替换进材质的贴图</param>
        /// <param name="textureNameKeyword">贴图对应的名称关键字</param>
        /// <param name="shaderFieldName">材质shader对应的字段名称</param>
        private static void TextureApply(Material material, string ID, Texture texture, string[] textureNameKeyword, string shaderFieldName)
        {
            for (int r = 0; r < textureNameKeyword.Length; r++)
            {
                if (texture.name.Contains(ID) && texture.name.Contains(textureNameKeyword[r]))
                {
                    material.SetTexture(shaderFieldName, texture);
                    Debug.Log(texture.name);
                    continue;
                }

            }
        }



    }
}
#endif
