#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

/// <summary>
/// ��������Shader��ɸѡ����Ҫ��shader����·��������txt�ļ���
/// �Ų��ض��ļ������Ƿ�������shader
/// ����һ����ʱ����
/// </summary>
public class FilterShader
{
    public static string ShaderFilterURL = "Assets/";
    public static List<string> ShaderURL = new List<string>();

    ///// <summary>
    ///// Ŀ¼ɸѡ����
    ///// </summary>
    //readonly private static string[] ShaderFileURLScreeningCondition = new string[] { "AmplifyShaderEditor" , "Shader_zhengli" , "Assets/Scipts/Editor/Tools" , "Assets/Spine" };

    [MenuItem("AssetsTools/FilterShader")]
    private static void FilterShaderFunc()
    {
        ShaderURL.Clear();
        //��ȡ���е�shaderGUID
        string[] ShaderFilesURL = AssetDatabase.FindAssets("t:Shader", new string[] { ShaderFilterURL });

        //��ȡ����shaderURL
        for (int j = 0; j < ShaderFilesURL.Length; j++)
        {
         //   Debug.Log(ShaderFilesURL[j]);
            ShaderFilesURL[j] = AssetDatabase.GUIDToAssetPath(ShaderFilesURL[j]);
            //  Debug.Log(ShaderFilesURL[j]);

            //ɸѡ�Ƿ��������

            //// ������ͼ������
            // for (int r = 0; r < ShaderFileURLScreeningCondition.Length; r++)
            // {
            //   //  �����ͺͱ�Ŷ���Ӧʱ,д�����
            //     if (ShaderFilesURL[j].Contains(ShaderFileURLScreeningCondition[r]))
            //     {
            //         break;
            //     }

            // }



            //����AmplifyShaderEditor·����
            if (ShaderFilesURL[j].Contains("AmplifyShaderEditor"))
            {
                continue;
            }
            //���ڲ����ļ���Shader_zhengli·����
            if (ShaderFilesURL[j].Contains("Shader_zhengli"))
            {
                continue;
            }
            //������������Ŀ¼��
            if (ShaderFilesURL[j].Contains("Assets/Scipts/Editor/Tools"))
            {
                continue;
            }
            //����Spine�����
            if (ShaderFilesURL[j].Contains("Assets/Spine"))
            {
                continue;
            }

            //�ų�VFX
            if (ShaderFilesURL[j].Contains(".vfx"))
            {
                continue;
            }

            //����
            ShaderURL.Add(ShaderFilesURL[j]);
        }
        foreach (var item in ShaderURL)
        {
            Debug.Log(item);
        }
       
    }


}
#endif