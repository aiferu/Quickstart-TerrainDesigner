#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace AiferuFramework.ArtBrushTool
{
    /// <summary>
    /// 美术刷草工具主类,用于调用美术刷草工具的功能,基础路径保存在这里
    /// </summary>
    public class ArtBrushToolMain
    {
        public static string ToolsPath 
        {
            get 
            {
                return AssetDatabase.GUIDToAssetPath(AssetDatabase.FindAssets("ArtBrushTool_Aiferu")[0]);
            }
        }
        public static string ToolsDataSavePath
        {
            get
            {
                return ToolsPath +"/Data/ArtBrushToolData.asset";           
            }
        }
    }
}
#endif
