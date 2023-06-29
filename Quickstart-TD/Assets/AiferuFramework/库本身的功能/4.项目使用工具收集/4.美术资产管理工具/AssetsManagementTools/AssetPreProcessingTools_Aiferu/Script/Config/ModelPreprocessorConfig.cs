#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace AiferuFramework.AssetsManagementTools
{
    [CreateAssetMenu(fileName = "ModelData", menuName = "InputSettingAssets/ModelPreprocessorConfig", order = 3)]
    public class ModelPreprocessorConfig : BasePreprocessorConfig
    {
        private readonly InputFileType fileType = InputFileType.Model;
        public override InputFileType FileType { get => fileType; }

        [Header("Scene Settings")]
        public bool ConvertUnits = true;
        public bool BakeAxisConversion = false;
        public bool ImportBlendShapes = false;
        public bool ImportVisibility = false;
        public bool ImportCameras = false;
        public bool ImportLight = false;


        [Header("Meshes Settings")]

        public bool ReadWriteEnable = false;//需要使用动画时才打开

        [Header("Geometry Settings")]

        public bool SwapUVs = false;//切换导入模型UV的主副UV
        public bool GenerateLightmapUVs = false;//生成光照贴图UV

        [Header("Rig Setting")]
        public ModelImporterAnimationType ModelImporterAnimationType = ModelImporterAnimationType.None;


        [Header("Animation Setting")]
        public bool InputAnimation = false;

        [Header("Materials Setting")]
        public ModelImporterMaterialImportMode ModelImporterMaterialImportMode = ModelImporterMaterialImportMode.ImportViaMaterialDescription;
        public ModelImporterMaterialLocation materialLocation = ModelImporterMaterialLocation.InPrefab;

        [Header("JudgeMentCondition")]
        public List<JudgeMentConditionItemConfig> judgeMentConditionItemConfigs = new List<JudgeMentConditionItemConfig>();

        [Header("Priority")]
        //优先级
        public int priority = 1;
    }
}
#endif
