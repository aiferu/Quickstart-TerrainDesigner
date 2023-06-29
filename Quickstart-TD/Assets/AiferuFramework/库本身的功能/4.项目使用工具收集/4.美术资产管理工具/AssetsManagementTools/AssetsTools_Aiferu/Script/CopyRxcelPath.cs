#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace AiferuFramework.AssetsManagementTools
{
    public class CopyRxcelPath : AssetsTools
    {
        //Project窗口的右键菜单
        [MenuItem("Assets/复制当前选中资产的资产表路径到剪贴板")]
        static void CopyExcelPath()
        {
            //获取当前选中文件的路径
            if (GetSelectedAssetPath() != null)
            {
                string path = GetSelectedAssetPath();
                path = path.Replace("Assets/AssetBundle/Lobby/", "");
                path = path.Replace(".prefab", "");
                //赋值到剪贴板
                GUIUtility.systemCopyBuffer = path;
                Debug.Log(path);
            }
        }
    }
}
#endif
