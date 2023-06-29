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
    /// 实现获取文件的路径到剪贴板
    /// </summary>
    public class GetFilePath : EditorWindow
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
            GUILayout.Label("脚本文件名称");
            scriptObj = (MonoScript)EditorGUILayout.ObjectField(scriptObj, typeof(MonoScript), true);
            if (GUILayout.Button("GetFilePath"))
            {
                //复制传入的string到剪贴板
                EditorUtil.CopyText(GetPath(scriptObj.name));
            }
        }




        /// <summary>
        /// 获取脚本的文件夹路径
        /// </summary>
        /// <param name="_scriptName">脚本的名字</param>
        /// <returns></returns>
        static string GetPath(string _scriptName)
        {
            //AssetDatabase.FindAssets 按照文件名称，返回所有查找到的文件的文件编号
            string[] path = AssetDatabase.FindAssets(_scriptName);
            if (path.Length > 1)
            {
                Debug.LogError("有同名文件" + _scriptName + "获取路径失败");
                return null;
            }
            //将字符串中的脚本名字和后缀统统去除掉
            //AssetDatabase.GUIDToAssetPath 根据传入的文件编号，查找文件路径，在Unity中，所有的文件都有一个文件编号
            //string.Replace(a,b) 将字符串中的a替换成b;
            string _path = (AssetDatabase.GUIDToAssetPath(path[0]).Replace((@"/" + _scriptName + ".cs"), "")).Replace("Assets/", "");
            return _path;

        }

    }
#endif
}

