using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

#if UNITY_EDITOR
/// <summary>
/// 盛放脚本的代码
/// 查找结点及其所有子节点中，是否有指定的脚本组件
/// </summary>
public class MonoFinder : EditorWindow
{
    Transform root = null;
    MonoScript scriptObj = null;
    int loopCount = 0;

    List<Transform> results = new List<Transform>();

    [MenuItem("AiferuFramework/库本身的功能/4.项目使用工具收集/2.查找选定脚本是否在选定物体上",false,4002)]
    static void Init()
    {
        EditorWindow.GetWindow(typeof(MonoFinder));
    }

    void OnGUI()
    {
        
        GUILayout.Label("节点:");
        root = (Transform)EditorGUILayout.ObjectField(root, typeof(Transform), true);
        GUILayout.Label("脚本类型:");
        scriptObj = (MonoScript)EditorGUILayout.ObjectField(scriptObj, typeof(MonoScript), true);
        if (GUILayout.Button("Find"))
        {
            results.Clear();
            loopCount = 0;
            Debug.Log("开始查找.");
            FindScript(root);
        }
        if (results.Count > 0)
        {
            foreach (Transform t in results)
            {
                EditorGUILayout.ObjectField(t, typeof(Transform), false);
            }
        }
        else
        {
            GUILayout.Label("无数据");
        }
    }

    void FindScript(Transform root)
    {
        if (root != null && scriptObj != null)
        {
            loopCount++;
            Debug.Log(".." + loopCount + ":" + root.gameObject.name);
            if (root.GetComponent(scriptObj.GetClass()) != null)
            {
                results.Add(root);
            }
            foreach (Transform t in root)
            {
                FindScript(t);
            }
        }
    }
}
#endif
