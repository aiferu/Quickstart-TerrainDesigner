using System;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace AiferuFramework
{
    /// <summary>
    /// UnityEditor工具包
    /// </summary>
    public partial class EditorUtil
    {
        /// <summary>
        /// 使用文件浏览器打开URL对应的文件夹
        /// </summary>
        /// <param name="folderPath"></param>
        public static void OpenInFolder(string folderPath)
        {
            //file:/// URL加上这个就表示使用文件浏览器打开
            Application.OpenURL("file:///" + folderPath);
        }

#if UNITY_EDITOR
        /// <summary>
        /// 导出Package到文件
        /// </summary>
        /// <param name="assetPathName">需要导出为Package的文件路径</param>
        /// <param name="fileName">导出的Package文件的名称</param>
        public static void ExportPackage(string assetPathName, string fileName)
        {
            //ExportPackageOptions.Recurse 导出路径中的所有文件，但不包括依赖项
            AssetDatabase.ExportPackage(assetPathName, fileName, ExportPackageOptions.Recurse);
        }


        /// <summary>
        /// 将传入的字符串复制到剪贴板
        /// </summary>
        /// <param name="text">需要复制到剪贴板的字符串</param>
        public static void CopyText(string text)
        {
            //将传入的字符串复制到剪贴板
            GUIUtility.systemCopyBuffer = text;
            //调用其他编辑器菜单脚本
           // EditorApplication.ExecuteMenuItem("AiferuFramework/1.导出 UnityPackage %e");

        }
#endif
    }
}