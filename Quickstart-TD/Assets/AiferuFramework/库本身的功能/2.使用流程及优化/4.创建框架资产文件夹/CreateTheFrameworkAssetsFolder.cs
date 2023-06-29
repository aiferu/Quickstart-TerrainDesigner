using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace AiferuFramework
{
#if UNITY_EDITOR
    /// <summary>
    /// 创建框架所需资产文件夹
    /// </summary>
    public class CreateTheFrameWorkAssetsFolder : EditorWindow
    {
        MonoScript scriptObj = null;
        int loopCount = 0;

        [MenuItem("AiferuFramework/库本身的功能/2.使用流程及优化/2.获取脚本路径到剪贴板", false, 2002)]
        public static void ShowWindow()
        {
            EditorWindow.GetWindow(typeof(GetFilePath));

        }

        public void OnGUI()
        {

        }


    }
#endif
}

