#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
namespace AiferuFramework.AssetsManagementTools
{
    [CreateAssetMenu(fileName = "TextureData", menuName = "InputSettingAssets/TexturePreprocessorConfig", order = 2)]
    public class TexturePreprocessorConfig : BasePreprocessorConfig
    {
        private readonly InputFileType fileType = InputFileType.Texture;
        public override InputFileType FileType { get => fileType;}

        [Header("Default Texture Settings")]
        public bool enableTextureType = true;
        public TextureImporterType TextureType;
        public int MaxTextureSize = 4096;
        public bool EnableReadWrite = false;
        public bool EnableMipmap = false;
        public bool EnableMipMapStreaming = false;
        public TextureImporterAlphaSource AlphaSource = TextureImporterAlphaSource.FromInput;
        public bool AlphaISTransParency = false;

        [Header("Filtering Settings")]
        public bool ForceFilterMode;
        public FilterMode FilterMode = FilterMode.Bilinear;
        public int AnisoLevel = 1;

        [Header("OverrideSettings/Ios&Android")]
        public bool ForceOverride = false;
        public TextureImporterFormat Format = TextureImporterFormat.ASTC_8x8;

        [Header("OverrideSettings/Windows&Mac&Linux")]
        public TextureImporterFormat PCFormat = TextureImporterFormat.DXT5Crunched;

        [Header("JudgeMentCondition")]
        public List<JudgeMentConditionItemConfig> judgeMentConditionItemConfigs = new List<JudgeMentConditionItemConfig>();

        [Header("Priority")]
        //”≈œ»º∂
        public int priority = 1;

        
    }
}
#endif