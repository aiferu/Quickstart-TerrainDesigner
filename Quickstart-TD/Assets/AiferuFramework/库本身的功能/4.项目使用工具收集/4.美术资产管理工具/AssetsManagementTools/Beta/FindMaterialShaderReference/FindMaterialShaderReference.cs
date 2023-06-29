#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;
/// <summary>
/// 查找Shader的材质引用
/// </summary>
public class FindMaterialShaderReference
{

    public static string FilePath = "Assets";
    public static string FileSavePath = "Assets/ShaderAssets";
    //搜索固定文件夹中的所有Material的路径
    public static List<string> listMatrials;
    //筛选出来的引用了当前shader文件的Material
    public static List<string> listTargetMaterial;
    //当前选中shader文件的名称
    public static string selectedShaderName = "hhh";

    public static StringBuilder sb;

    //只有当当前选中的文件为Shader时才会激发当前方法
    [MenuItem("Assets/查找当前选中Shader文件的材质引用", true)]
    private static bool OptionSelectAvailable()
    {
        if (Selection.activeObject == null)
        {
            return false;
        }
        return Selection.activeObject.GetType() == typeof(Shader);
    }

    [MenuItem("Assets/查找当前选中Shader文件的材质引用")]
    private static void SearchConstantShader()
    {
        Debug.Log("当前选中的Shader名字：" + Selection.activeObject.name);
        sb = new StringBuilder();

        selectedShaderName = Selection.activeObject.name;

        //初始化两个链表
        listMatrials = new List<string>();
        listMatrials.Clear();
        listTargetMaterial = new List<string>();
        listTargetMaterial.Clear();

        //项目路径 eg:projectPath = D:Project/Test/Assets
        string projectPath = Application.dataPath;

        //eg:projectPath = D:Project/Test
        projectPath = projectPath.Substring(0, projectPath.IndexOf("Assets"));

        try
        {
            //获取某一文件夹中的所有Matrial的Path信息
            GetMaterialsPath(projectPath, FilePath, "Material", ref listMatrials);
        }
        catch (System.Exception e)
        {
            Debug.LogError(e);
        }

        for (int i = 0; i < listMatrials.Count; i++)
        {
            EditorUtility.DisplayProgressBar("Check Materials", "Current Material :"
                + i + "/" + listMatrials.Count, (float)i / listMatrials.Count);

            try
            {
                //开始Check
                BegainCheckMaterials(listMatrials[i]);
            }
            catch (System.Exception e)
            {
                EditorUtility.ClearProgressBar();
                Debug.LogError(e);
            }
        }

        //将获得的链表存放起来
        //PrintToAssets();
        PrintToTxt();
        EditorUtility.ClearProgressBar();
        Debug.Log("Check Success");
    }


    /// <summary>
    /// 获取某一文件夹中的所有Matrial的Path信息
    /// </summary>
    /// <param name="projectPath"></param>
    /// <param name="targetFilePath"></param>
    /// <param name="searchType"></param>
    /// <param name="array"></param>
    public static void GetMaterialsPath(string projectPath, string targetFilePath, string searchType, ref List<string> array)
    {
        if (Directory.Exists(targetFilePath))
        {
            string[] guids;
            //搜索
            guids = AssetDatabase.FindAssets("t:" + searchType, new[] { targetFilePath });
            foreach (string guid in guids)
            {
                string source = AssetDatabase.GUIDToAssetPath(guid);
                listMatrials.Add(source);
            }
        }
    }


    /// <summary>
    /// 检查Material,当材质的shader为我们选中的shader时,加入 listTargetMaterial
    /// </summary>
    /// <param name="materialPath">材质的路径</param>
    public static void BegainCheckMaterials(string materialPath)
    {
        Material mat = AssetDatabase.LoadAssetAtPath<Material>(materialPath);
        if (mat.shader.name == selectedShaderName)
        {
            listTargetMaterial.Add(materialPath);
        }
    }

    /// <summary>
    /// 将链表中的材质路径打印到文本中
    /// </summary>
    public static void PrintToTxt()
    {
        //加入shader的名字
        listTargetMaterial.Add(selectedShaderName);

        FileInfo fi = new FileInfo(Application.dataPath + "/Materials.txt");
        if (!fi.Exists)
        {
            fi.CreateText();
        }
        else
        {
            StreamWriter sw = new StreamWriter(Application.dataPath + "/Materials.txt");
            for (int i = 0; i < listTargetMaterial.Count - 1; i++)
            {
                sb.Append(listTargetMaterial[i] + "\n");
            }
            string useNum = string.Format("共有 {0} 个Material用到：{1}", listTargetMaterial.Count - 1, selectedShaderName);
            sb.Append(useNum + "\n");
            sb.Append("用到的shader名字为：" + selectedShaderName);
            sw.Write(sb.ToString());

            sw.Flush();
            sw.Close();
            sw.Dispose();
        }
    }

    // [MenuItem("Assets/chuangjianhhh")]
    /// <summary>
    /// 将链表中的材质添加到资源中
    /// </summary>
    public static void PrintToAssets()
    {
        var level = ScriptableObject.CreateInstance<FindMaterialShaderReferenceAssets>();

        AssetDatabase.CreateAsset(level, "Assets/shaderAssets/" + selectedShaderName + ".asset");
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        // level
    }

}
#endif