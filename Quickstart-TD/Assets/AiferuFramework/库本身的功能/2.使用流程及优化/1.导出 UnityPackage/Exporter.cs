using System;
using System.IO;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace AiferuFramework
{
    public class Exporter
    {
        private static string GeneratePackageName()
        {
            return "AFramework/AiferuFramework_" + DateTime.Now.ToString("yyyyMMdd_HH_mm");
        }

#if UNITY_EDITOR
        //%e 这项菜单的快捷键为ctrl+e -----------------------------------------------↓
        //false 暂时用不着 ----------------------------------------------------------------↓
        //菜单项排序用，序号越小越靠前 ----------------------------------------------------------↓
        [MenuItem("AiferuFramework/库本身的功能/2.使用流程及优化/1.导出 AllUnityPackage %e", false, 2001)]
        static void AllExport()
        {
            string path = Path.Combine(Application.dataPath, "../AFramework/");
            Debug.Log(path);
            if (Directory.Exists(path))
            {
                Debug.Log("文件夹存在");
            }else
            {
                Directory.CreateDirectory(path);
                Debug.Log("文件夹创建成功");
            }
            EditorUtil.ExportPackage("Assets/AiferuFramework", GeneratePackageName() + ".unitypackage");
            EditorUtil.OpenInFolder(path);
        }

        [MenuItem("AiferuFramework/库本身的功能/2.使用流程及优化/1.导出 SimpUnityPackage %w", false, 2001)]
        static void SimpExport()
        {
            string path = Path.Combine(Application.dataPath, "../AFramework/");
            Debug.Log(path);
            if (Directory.Exists(path))
            {
                Debug.Log("文件夹存在");
            }
            else
            {
                Directory.CreateDirectory(path);
                Debug.Log("文件夹创建成功");
            }
            EditorUtil.ExportPackage("Assets/AiferuFramework/库本身的功能", GeneratePackageName() + ".unitypackage");
            EditorUtil.OpenInFolder(path);
        }
#endif

    }
}