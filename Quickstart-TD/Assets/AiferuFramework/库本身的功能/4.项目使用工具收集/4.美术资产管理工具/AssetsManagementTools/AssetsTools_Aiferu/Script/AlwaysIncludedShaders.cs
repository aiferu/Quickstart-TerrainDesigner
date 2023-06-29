#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public class AlwaysIncludedShaderTools
{
    /// <summary>
    /// 将文件夹下的shader添加到编译所有着色器变体列表
    /// </summary>
    [MenuItem("AssetsTools/2005将文件夹下的shader添加到编译所有着色器变体列表", false, 2005)]
    public static void AlwaysIncludedShaders()
    {
        //if (folder == null)
        //	return;

        ////获取文件夹路径
        //string folder_path = AssetDatabase.GetAssetPath(folder);
        ////当输入文件夹路径不存在时
        //if (!AssetDatabase.IsValidFolder(folder_path))
        //{
        //	EditorUtility.DisplayDialog("Error", "请选中Shader所在文件夹", "OK");
        //	return;
        //}

        //在用户所选中的文件夹下搜索所有Shader
        Shader[] shaders = Selection.GetFiltered<Shader>(SelectionMode.DeepAssets);
        if (shaders == null || shaders.Length == 0)
        {
            EditorUtility.DisplayDialog("Error", "未搜索到Shader文件", "OK");
            return;
        }

        //修改GraphicsSettings
        //加载GraphSetting文件
        SerializedObject graphicsSettings = new SerializedObject(AssetDatabase.LoadAssetAtPath<Object>("ProjectSettings/GraphicsSettings.asset"));
        //读取出GraphSetting中的AlwaysIncludedShaders属性
        SerializedProperty m_AlwaysIncludedShaders = graphicsSettings.FindProperty("m_AlwaysIncludedShaders");
        //移除所有元素（这个操作太危险了）
        //m_AlwaysIncludedShaders.ClearArray();
        //移除序号11之后的所有元素（难搞，不移除，手动去移除）
        Debug.Log(m_AlwaysIncludedShaders.CountInProperty());//打印一下
        Debug.Log(shaders.Length);



        //新建一个新的序列化属性
        SerializedProperty element;
        for (int i = 0; i < shaders.Length; i++)
        {
            //在AlwaysIncludedShaders中插入序列号
            m_AlwaysIncludedShaders.InsertArrayElementAtIndex(i);
            //拿到对应序列号的索引
            element = m_AlwaysIncludedShaders.GetArrayElementAtIndex(i);

            //将索引指对应的对象赋值为当前Shader
            element.objectReferenceValue = shaders[i];
            Debug.Log(shaders[i]);

        }
        //应用更改
        Debug.Log(m_AlwaysIncludedShaders.CountInProperty());
        //graphicsSettings.ApplyModifiedProperties();

    }
}
#endif