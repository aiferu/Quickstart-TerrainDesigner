//
//  FindsShaderReferenceForMaterial.cs
//  AiferuFramework
//
//  Created by Aiferu on 2023/2/8.
//
//
#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Text;

namespace AiferuFramework.AssetsManagementTools
{
    /// <summary>
    /// 查找材质的Shader引用
    /// </summary>
    public class FindsShaderReferenceForMaterial
    {
        #region Field
        private static List<string> AllMaterialPaths = new List<string>();
        private static List<string> TargetMaterialPaths = new List<string>();
        private static string selectedShaderName = "";
        public static StringBuilder sb = new StringBuilder();
        #endregion

        #region Property

        #endregion

        #region UnityOriginalEvent
        [MenuItem("Assets/查找当前选中Shader文件的材质引用", true)]
        private static bool FindsShaderReferenceForMaterialSwitchFunc()
        {
            if (Selection.activeObject == null)
            {
                return false;
            }
            return Selection.activeObject.GetType() == typeof(Shader);
        }
        [MenuItem("Assets/查找当前选中Shader文件的材质引用")]
        private static void FindsShaderReferenceForMaterialFunc()
        {
            selectedShaderName = Selection.activeObject.name;
            Debug.Log("当前选中shader的名字:" + selectedShaderName);
            //获取所有材质路径
            AllMaterialPaths.Clear();
            AssetsTools.GetALLFilesMaterial("Assets", out AllMaterialPaths);

            //判断材质的Shader是否为我们选中的Shader
            foreach (var materialPath in AllMaterialPaths)
            {

                Material mat = AssetDatabase.LoadAssetAtPath<Material>(materialPath);
                //Debug.Log(materialPath);
                //Debug.Log(mat.name);
                if (mat.shader.name == selectedShaderName)
                {
                    TargetMaterialPaths.Add(materialPath);
                }


            }

            //存储结果
            PrintToTxt();
        }


        #endregion

        #region Function
        /// <summary>
        /// 将链表中的材质路径打印到文本中
        /// </summary>
        public static void PrintToTxt()
        {
            //加入shader的名字
            TargetMaterialPaths.Add(selectedShaderName);
            Debug.Log("开始打印");


            FileInfo fi = new FileInfo(Application.dataPath + "/Materials.txt");
            if (!fi.Exists)
            {
                fi.CreateText();
            }
            else
            {
                StreamWriter sw = new StreamWriter(Application.dataPath + "/Materials.txt");
                for (int i = 0; i < TargetMaterialPaths.Count - 1; i++)
                {
                    sb.Append(TargetMaterialPaths[i] + "\n");
                }
                string useNum = string.Format("共有 {0} 个Material用到：{1}", TargetMaterialPaths.Count - 1, selectedShaderName);
                sb.Append(useNum + "\n");
                sb.Append("用到的shader名字为：" + selectedShaderName);
                sw.Write(sb.ToString());

                sw.Flush();
                sw.Close();
                sw.Dispose();
            }
            TargetMaterialPaths.Clear();
            sb.Clear();
            AssetDatabase.Refresh();
        }
        #endregion
    }
}
#endif