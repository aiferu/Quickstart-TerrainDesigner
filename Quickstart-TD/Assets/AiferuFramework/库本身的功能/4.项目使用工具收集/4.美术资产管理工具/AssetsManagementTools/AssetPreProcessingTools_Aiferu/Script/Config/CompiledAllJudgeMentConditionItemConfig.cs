#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;
namespace AiferuFramework.AssetsManagementTools
{
    [CreateAssetMenu(fileName = "CompiledAllJudgeMentConditionItemConfig", menuName = "InputSettingAssets/CompiledAllJudgeMentConditionItemConfig", order = 4)]
    public class CompiledAllJudgeMentConditionItemConfig : ScriptableObject
    {
        public List<TexturePreprocessorConfig> TexturteJudgeMent;
        public List<ModelPreprocessorConfig> MeshJudgeMent;
        public List<AudioPreprocessorConfig> AudioJudgeMent;

        public List<string> loopAnimationName;
    }
}
#endif
