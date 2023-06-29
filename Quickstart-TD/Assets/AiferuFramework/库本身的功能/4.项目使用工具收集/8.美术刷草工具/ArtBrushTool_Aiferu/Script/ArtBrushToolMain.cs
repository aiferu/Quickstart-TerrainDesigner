#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace AiferuFramework.ArtBrushTool
{
    /// <summary>
    /// ����ˢ�ݹ�������,���ڵ�������ˢ�ݹ��ߵĹ���,����·������������
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
