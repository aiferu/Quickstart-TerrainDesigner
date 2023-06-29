using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;

namespace AiferuFramework.AssetsManagementTools
{
    /// <summary>
    /// 变量：
    /// assetlmporter // 对资源导入器的引用
    /// assetPath     // 要导入资源导入之后的路径名称，从Assets目录开始
    /// context       // 导入上下文
    /// 编写完成后记得编译成dll脚本
    /// </summary>
    public class AssetsPreProcessing : AssetPostprocessor
    {
        private string AssetPreprocessingToolsFilePath = "Assets/Scipts/Editor/AssetsManagementTools/AssetPreProcessingTools";
        private string configAssetFilePath;
        private string configAssetPath;

        #region 读取插件的所有配置文件
        public CompiledAllJudgeMentConditionItemConfig LoadConfig()
        {
            //文件处理
            AssetPreprocessingToolsFilePath = AssetPreprocessingTools.GetAssetPreprocessingToolsFilePath();
            configAssetFilePath = AssetPreprocessingToolsFilePath + "/Data/CompiledAllJudgeMentConditionItemConfigData/";
            configAssetPath = configAssetFilePath + "CompiledAllJudgeMentConditionItemConfig.asset";
            if (!Directory.Exists(configAssetFilePath))
            {
                Directory.CreateDirectory(configAssetFilePath);
            }
            //当配置文件不存在时创建文件
            //if (!File.Exists(configAssetPath))
            //{
            //    Debug.Log("创建配置文件");
            //    CompiledAllJudgeMentConditionItemConfig compiledAllJudgeMentConditionItemConfig = ScriptableObject.CreateInstance<CompiledAllJudgeMentConditionItemConfig>();
            //    //删除原有文件,生成新文件
            //    AssetDatabase.DeleteAsset(configAssetPath);
            //    Debug.Log(configAssetPath);
            //    AssetDatabase.CreateAsset(compiledAllJudgeMentConditionItemConfig, configAssetPath);
            //    AssetDatabase.Refresh();
            //    Debug.Log("创建判断条件成功");
            //}

            //读取数据
            Debug.Log("读取配置文件");
            CompiledAllJudgeMentConditionItemConfig configAsset = AssetDatabase.LoadAssetAtPath<CompiledAllJudgeMentConditionItemConfig>(configAssetPath);
            return configAsset;
        }
        #endregion

        #region 网格导入器
        /// <summary>
        /// 模型导入之前调用，在这可以修改模型的导入设置
        /// </summary>
        public void OnPreprocessModel()
        {

            //获取导入器
            ModelImporter modelImporter = assetImporter as ModelImporter;
            if (modelImporter == null) return;

            //遍历对应类型的所有配置文件
            ApplyConfig(modelImporter);



        }
        /// <summary>
        /// 模型导入之后调用
        /// </summary>
        /// <param name="go"></param>
        public void OnPostprocessModel(GameObject go)
        {
            Debug.Log("OnPostprocessModel=" + go.name);
            //获取导入器
            ModelImporter modelImporter = assetImporter as ModelImporter;
            CompiledAllJudgeMentConditionItemConfig ConfigAsset = LoadConfig();
            //动画添加循环
            //当为动画文件时
            if (modelImporter.importAnimation == true)
            {
                //保证所有的动画片段的名称与其模型统一
                //List<ModelImporterClipAnimation> actions = new List<ModelImporterClipAnimation>();
                //ModelImporterClipAnimation anim = modelImporter.defaultClipAnimations[0];
                //anim.name = go.name;

                ////当动画类型为需要循环时
                //for (int i = 0; i < ConfigAsset.loopAnimationName.Count; i++)
                //{
                //    Debug.Log(anim.name);
                //    if (go.name.ToUpper().Contains(ConfigAsset.loopAnimationName[i].ToUpper()))
                //    {
                //        Debug.Log("循环动画:" + go.name);
                //        anim.loopTime = true;
                //        break;

                //    }
                //}
                //Debug.Log("保存循环");
                //actions.Add(anim);
                //modelImporter.clipAnimations = actions.ToArray();
                //AssetDatabase.Refresh();
                //AssetDatabase.SaveAssets();
                //modelImporter.SaveAndReimport();




            }
        }
        #endregion

        #region 纹理导入器
        /// <summary>
        /// 纹理导入之前调用，针对导入到的纹理进行设置
        /// </summary>
        public void OnPreprocessTexture()
        {
            //获取导入器
            TextureImporter textureImporter = assetImporter as TextureImporter;
            if (textureImporter == null) return;
            ApplyConfig(textureImporter);
        }
        /// <summary>
        /// 在纹理导入成功之后调用
        /// </summary>
        /// <param name="tex"></param>
        public void OnPostprocessTexture(Texture2D tex)
        {
            Debug.Log("OnPostProcessTexture=" + this.assetPath);
        }
        #endregion

        #region 音频导入器
        /// <summary>
        /// 在音频导入之前调用，针对导入的音频进行设置
        /// </summary>
        public void OnPreprocessAudio()
        {

            //获取音频导入器
            AudioImporter audioIpmorter = this.assetImporter as AudioImporter;
            if (audioIpmorter == null) return;
            ApplyConfig(audioIpmorter);


        }
        /// <summary>
        /// 在音频导入成功之后调用
        /// </summary>
        /// <param name="clip"></param>
        public void OnPostprocessAudio(AudioClip clip)
        {
            Debug.Log(clip.name);
        }
        #endregion

        #region 解析设置配置文件
        /// <summary>
        /// 纹理配置文件解析
        /// </summary>
        /// <param name="TextureConfig"></param>
        /// <param name="impor"></param>
        private static void TextureSetting(TexturePreprocessorConfig TextureConfig, TextureImporter impor)
        {
            if (TextureConfig.enableTextureType)
            {
                impor.textureType = TextureConfig.TextureType;
            }
            impor.maxTextureSize = TextureConfig.MaxTextureSize;
            impor.mipmapEnabled = TextureConfig.EnableMipmap;
            impor.streamingMipmaps = TextureConfig.EnableMipMapStreaming;

            //不再控制贴图的alpha导入设置 2022.10.11 
            // impor.alphaSource = TextureConfig.AlphaSource;
            //  impor.alphaIsTransparency = TextureConfig.AlphaISTransParency;



            //纹理读写
            // if (TextureConfig.EnableReadWrite && !impor.isReadable)
            //   {
            //    Debug.Log("Enabling Read/Write." + impor.name);
            impor.isReadable = TextureConfig.EnableReadWrite;
            //   }


            if (TextureConfig.ForceFilterMode)
            {
                impor.anisoLevel = TextureConfig.AnisoLevel;
                impor.filterMode = TextureConfig.FilterMode;
            }

            //设置特定平台的纹理设置

            if (TextureConfig.ForceOverride)
            {
                //impor.SetPlatformTextureSettings(TextureImporterPlatformSettings)
                //这里的已弃用请不要管他 ,不要注释掉 tnnd
                impor.SetPlatformTextureSettings("Android", TextureConfig.MaxTextureSize, TextureConfig.Format, false);
                impor.SetPlatformTextureSettings("Iphone", TextureConfig.MaxTextureSize, TextureConfig.Format, false);
                impor.SetPlatformTextureSettings("Standalone", TextureConfig.MaxTextureSize*2,TextureConfig.PCFormat,false);
            }

        }
        /// <summary>
        /// 音频配置文件解析
        /// </summary>
        /// <param name="audioPreprocessor"></param>
        /// <param name="audioIpmorter"></param>
        private static void AudioSetting(AudioPreprocessorConfig audioPreprocessor, AudioImporter audioIpmorter)
        {
            audioIpmorter.ambisonic = audioPreprocessor.Ambisonic;
            audioIpmorter.loadInBackground = audioPreprocessor.loadInBackground;
            audioIpmorter.SetOverrideSampleSettings("Android", audioPreprocessor.AndroidSampleSettings);
            audioIpmorter.SetOverrideSampleSettings("iOS", audioPreprocessor.IosSampleSettings);
        }
        /// <summary>
        /// 网格配置文件解析
        /// </summary>
        /// <param name="modelPreprocessorConfig"></param>
        /// <param name="modelImporter"></param>
        private static void ModelSetting(ModelPreprocessorConfig modelPreprocessorConfig, ModelImporter modelImporter)
        {
            modelImporter.bakeAxisConversion = modelPreprocessorConfig.BakeAxisConversion;
            modelImporter.importBlendShapes = modelPreprocessorConfig.ImportBlendShapes;
            modelImporter.importVisibility = modelPreprocessorConfig.ImportVisibility;
            modelImporter.importCameras = modelPreprocessorConfig.ImportCameras;
            modelImporter.importLights = modelPreprocessorConfig.ImportLight;

            //网格读写
            //   if (modelPreprocessorConfig.ReadWriteEnable && !modelImporter.isReadable)
            //  {
            //   Debug.Log("Enabling Read/Write." + modelImporter.name);
            modelImporter.isReadable = modelPreprocessorConfig.ReadWriteEnable;
            //   }

            modelImporter.swapUVChannels = modelPreprocessorConfig.SwapUVs;
            modelImporter.generateSecondaryUV = modelPreprocessorConfig.GenerateLightmapUVs;

            modelImporter.animationType = modelPreprocessorConfig.ModelImporterAnimationType;



            modelImporter.importAnimation = modelPreprocessorConfig.InputAnimation;

            modelImporter.materialImportMode = modelPreprocessorConfig.ModelImporterMaterialImportMode;



        }

        #endregion

        #region 应用配置文件
        void ApplyConfig(ModelImporter Importer)
        {
            CompiledAllJudgeMentConditionItemConfig ConfigAsset = LoadConfig();
            foreach (var item in ConfigAsset.MeshJudgeMent)
            {
                bool isok = false;
                //遍历所有的判断条件
                foreach (var m in item.judgeMentConditionItemConfigs)
                {
                    //进行判断
                    switch (m.judgmentType)
                    {
                        case JudgeMentConditionItemConfig.JudgmentType.Equal:
                            if (assetPath.Contains(m.keyword))
                            {
                                isok = true;
                            }
                            else
                            {
                                isok = false;

                            }
                            break;
                        case JudgeMentConditionItemConfig.JudgmentType.NotEqual:
                            if (!assetPath.Contains(m.keyword))
                            {
                                isok = true;
                            }
                            else
                            {
                                isok = false;

                            }
                            break;
                        default:
                            break;
                    }
                    if (!isok)
                    {
                        break;
                    }
                }
                //Debug.Log("设置:" + item.name);
                if (isok)
                {
                    //应用设置
                    Debug.Log("应用设置:" + item.name);
                    ModelSetting(item, Importer);
                    break;
                }
            }
        }
        void ApplyConfig(AudioImporter Importer)
        {
            CompiledAllJudgeMentConditionItemConfig ConfigAsset = LoadConfig();
            foreach (var item in ConfigAsset.AudioJudgeMent)
            {
                bool isok = false;
                //遍历所有的判断条件
                foreach (var m in item.judgeMentConditionItemConfigs)
                {
                    //进行判断
                    switch (m.judgmentType)
                    {
                        case JudgeMentConditionItemConfig.JudgmentType.Equal:
                            if (assetPath.Contains(m.keyword))
                            {
                                isok = true;
                            }
                            else
                            {
                                isok = false;

                            }
                            break;
                        case JudgeMentConditionItemConfig.JudgmentType.NotEqual:
                            if (!assetPath.Contains(m.keyword))
                            {
                                isok = true;
                            }
                            else
                            {
                                isok = false;

                            }
                            break;
                        default:
                            break;
                    }
                    if (!isok)
                    {
                        break;
                    }
                }
                //Debug.Log("设置:" + item.name);
                if (isok)
                {
                    //应用设置
                    Debug.Log("应用设置:" + item.name);
                    AudioSetting(item, Importer);
                    break;
                }
            }
        }
        void ApplyConfig(TextureImporter Importer)
        {
            CompiledAllJudgeMentConditionItemConfig ConfigAsset = LoadConfig();
            foreach (var item in ConfigAsset.TexturteJudgeMent)
            {
                bool isok = false;
                //遍历所有的判断条件
                foreach (var m in item.judgeMentConditionItemConfigs)
                {
                    //进行判断
                    switch (m.judgmentType)
                    {
                        case JudgeMentConditionItemConfig.JudgmentType.Equal:
                            if (assetPath.Contains(m.keyword))
                            {
                                isok = true;
                            }
                            else
                            {
                                isok = false;

                            }
                            break;
                        case JudgeMentConditionItemConfig.JudgmentType.NotEqual:
                            if (!assetPath.Contains(m.keyword))
                            {
                                isok = true;
                            }
                            else
                            {
                                isok = false;

                            }
                            break;
                        default:
                            break;
                    }
                    if (!isok)
                    {
                        break;
                    }
                }
                //Debug.Log("设置:" + item.name);
                if (isok)
                {
                    //应用设置
                    Debug.Log("应用设置:" + item.name);
                    TextureSetting(item, Importer);
                    break;
                }
            }
        }
        #endregion

        ////所有的资源的导入，删除，移动，都会调用此方法，注意，这个方法是static的
        //public static void OnPostprocessAllAssets(string[] importedAsset, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        //{
        //    //  Debug.Log("OnPostprocessAllAssets");
        //    foreach (string str in importedAsset)
        //    {
        //        Debug.Log("importedAsset = " + str);
        //    }
        //    foreach (string str in deletedAssets)
        //    {
        //        Debug.Log("deletedAssets = " + str);
        //    }
        //    foreach (string str in movedAssets)
        //    {
        //        Debug.Log("movedAssets = " + str);
        //    }
        //    foreach (string str in movedFromAssetPaths)
        //    {
        //        Debug.Log("movedFromAssetPaths = " + str);
        //    }
        //}

    }
}

#endif