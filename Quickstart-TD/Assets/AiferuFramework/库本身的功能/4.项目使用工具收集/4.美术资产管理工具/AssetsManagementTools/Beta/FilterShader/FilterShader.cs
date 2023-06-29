#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 遍历所有Shader，筛选出你要的shader，将路径导出到txt文件中
/// 排查特定文件夹外是否有其他shader
/// 这是一个临时工具
/// </summary>
public class FilterShader
{
    public static string ShaderFilterURL = "Assets/";
    public static List<string> ShaderURL = new List<string>();

    ///// <summary>
    ///// 目录筛选条件
    ///// </summary>
    //readonly private static string[] ShaderFileURLScreeningCondition = new string[] { "AmplifyShaderEditor" , "Shader_zhengli" , "Assets/Scipts/Editor/Tools" , "Assets/Spine" };

    [MenuItem("AssetsTools/FilterShader")]
    private static void FilterShaderFunc()
    {
        ShaderURL.Clear();
        //获取所有的shaderGUID
        string[] ShaderFilesURL = AssetDatabase.FindAssets("t:Shader", new string[] { ShaderFilterURL });

        //获取所有shaderURL
        for (int j = 0; j < ShaderFilesURL.Length; j++)
        {
         //   Debug.Log(ShaderFilesURL[j]);
            ShaderFilesURL[j] = AssetDatabase.GUIDToAssetPath(ShaderFilesURL[j]);
            //  Debug.Log(ShaderFilesURL[j]);

            //筛选是否符合条件

            //// 遍历贴图的类型
            // for (int r = 0; r < ShaderFileURLScreeningCondition.Length; r++)
            // {
            //   //  当类型和编号都对应时,写入材质
            //     if (ShaderFilesURL[j].Contains(ShaderFileURLScreeningCondition[r]))
            //     {
            //         break;
            //     }

            // }



            //不在AmplifyShaderEditor路径下
            if (ShaderFilesURL[j].Contains("AmplifyShaderEditor"))
            {
                continue;
            }
            //不在测试文件夹Shader_zhengli路径下
            if (ShaderFilesURL[j].Contains("Shader_zhengli"))
            {
                continue;
            }
            //不在美术工具目录下
            if (ShaderFilesURL[j].Contains("Assets/Scipts/Editor/Tools"))
            {
                continue;
            }
            //不在Spine插件下
            if (ShaderFilesURL[j].Contains("Assets/Spine"))
            {
                continue;
            }

            //排除VFX
            if (ShaderFilesURL[j].Contains(".vfx"))
            {
                continue;
            }

            //载入
            ShaderURL.Add(ShaderFilesURL[j]);
        }
        foreach (var item in ShaderURL)
        {
            Debug.Log(item);
        }
       
    }


}
#endif