using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
namespace AiferuFramework
{
    public class URPAssetsControl : MonoBehaviour
    {
        [SerializeField]
        private URPAssetsSettingData settingData;
        private void Start()
        {
            UniversalRenderPipelineAsset urpAssets = GraphicsSettings.currentRenderPipeline as UniversalRenderPipelineAsset;
            ApplyURPSetting(urpAssets);
        }
        private void ApplyURPSetting(UniversalRenderPipelineAsset universalRenderPipelineAsset)
        {
            universalRenderPipelineAsset.supportsCameraDepthTexture = settingData.EnableDepthTexture;
            universalRenderPipelineAsset.supportsCameraOpaqueTexture = settingData.EnableOpaqueTexture;
            universalRenderPipelineAsset.supportsHDR = settingData.EnableHDR;
            universalRenderPipelineAsset.useSRPBatcher = settingData.EnableSRPBatcher;
            universalRenderPipelineAsset.msaaSampleCount = (int)settingData.MSAASampleCount;
        }

    }
    [Serializable]
    public struct URPAssetsSettingData
    {
        public bool EnableDepthTexture;
        public bool EnableOpaqueTexture;
        public bool EnableHDR;
        public bool EnableSRPBatcher;
        public MsaaQuality MSAASampleCount;
    }

}


