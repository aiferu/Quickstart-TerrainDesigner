#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace AiferuFramework.AssetsManagementTools
{
    [CreateAssetMenu(fileName = "AudioData", menuName = "InputSettingAssets/AudioPreprocessorConfig", order = 4)]
    public class AudioPreprocessorConfig : BasePreprocessorConfig
    {
        private readonly InputFileType fileType = InputFileType.Audio;
        public override InputFileType FileType { get => fileType;}


        [Header("Default Audio Settings")]
        public bool Ambisonic = false;
        public bool loadInBackground = false;

        public AudioImporterSampleSettings AndroidSampleSettings = new AudioImporterSampleSettings();
        public AudioImporterSampleSettings IosSampleSettings = new AudioImporterSampleSettings();

        [Header("JudgeMentCondition")]
        public List<JudgeMentConditionItemConfig> judgeMentConditionItemConfigs = new List<JudgeMentConditionItemConfig>();

        [Header("Priority")]
        //”≈œ»º∂
        public int priority = 1;
    }
}

#endif
