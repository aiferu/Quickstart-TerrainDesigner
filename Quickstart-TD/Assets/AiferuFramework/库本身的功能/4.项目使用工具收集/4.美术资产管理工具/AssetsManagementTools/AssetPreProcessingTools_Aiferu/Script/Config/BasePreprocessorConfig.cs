#if UNITY_EDITOR
using UnityEngine;
namespace AiferuFramework.AssetsManagementTools
{
    [CreateAssetMenu(fileName = "BaseData", menuName = "InputSettingAssets/BasePreprocessorConfig", order = 1)]
    public class BasePreprocessorConfig : ScriptableObject
    {
        public enum InputFileType
        {
            Model,
            Texture,
            Audio
        };
        private InputFileType fileType = InputFileType.Model;

        public virtual InputFileType FileType { get => fileType; set => fileType = value; }
    }
}
#endif
