//
//  InputMaterialTextureConfig.cs
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
    /// 存储贴图导入材质工具的设置
    /// </summary>
    [CreateAssetMenu(fileName = "InputMaterialTextureData", menuName = "InputSettingAssets/InputMaterialTextureConfig", order = 7)]
    public class InputMaterialTextureConfig : ScriptableObject
    {
        #region Field
        [Header("TextureNameKeyword")]
        /// <summary>
        /// 颜色贴图 -贴图的命名规则（材质_目标贴图后缀）
        /// </summary>
        public string[] TextureNameKeyword_Albedo = new string[] { "_BassColor", "_BaseColor", "_Basecolor", "_BascColor", "_BaseCoor", "_BaseColo" };
        /// <summary>
        /// MAR贴图 金属度 环境光遮蔽 光滑度
        /// </summary>
        public string[] TextureNameKeyword_MAR = new string[] { "_MAR" };
        /// <summary>
        /// 法线贴图
        /// </summary>
        public string[] TextureNameKeyword_Normal = new string[] { "_Normal", "_normal" };
        /// <summary>
        /// 自发光贴图
        /// </summary>
        public string[] TextureNameKeyword_Emissive = new string[] { "_Emissive" };
        /// <summary>
        /// 环境光遮蔽贴图
        /// </summary>
        public string[] TextureNameKeyword_AmbientOcclusion = new string[] { "_AO", "_AmbientOcclusionTexture" };
        /// <summary>
        /// 光滑度/粗糙度贴图
        /// </summary>
        public string[] TextureNameKeyword_Roughness = new string[] { "_Roughness", "_Smoothness" };
        /// <summary>
        /// 金属度贴图
        /// </summary>
        public string[] TextureNameKeyword_Metallic = new string[] { "_Metallic" };

        [Header("ShaderName")]
        public string ShaderName_Albedo = "_BaseMap";
        public string ShaderName_MAR = "_MARBlendMap";
        public string ShaderName_Normal = "_BumpMap";
        public string ShaderName_Emissive = "_EmissionMap";
        public string ShaderName_AmbientOcclusion = "_OcclusionMap";
        public string ShaderName_Roughness = "_SmoothnessMap";
        public string ShaderName_MetallicTexture = "_MetallicGlossMap";
        #endregion

        #region Property

        #endregion

        #region UnityOriginalEvent

        #endregion

        #region Function

        #endregion
    }
}
#endif