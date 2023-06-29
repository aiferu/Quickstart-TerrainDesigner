#if UNITY_EDITOR
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEditor;
using System.Collections.Generic;
using System.Numerics;

namespace AiferuFramework.ArtBrushTool
{
    /// <summary>
    /// 美术刷草工具
    /// </summary>
    public class ArtBrushToolEW : EditorWindow
    {
        public static ArtBrushToolEW ins;

        #region 基础数据
        /// <summary>
        /// 工具是否启用
        /// </summary>
        public bool Enable;
        /// <summary>
        /// 是否开启笔刷
        /// </summary>
        public bool BrushEnable;
        /// <summary>
        /// 添加的对象
        /// </summary>
        private GameObject AddObject;
        /// <summary>
        /// 当前选择的对象
        /// </summary>
        public Transform CurrentSelect;
        /// <summary>
        /// 草对象缩略图数组
        /// </summary>
        private Texture[] TexObjects;
        /// <summary>
        /// 数据存储
        /// </summary>
        public ArtBrushToolData data;
        #endregion

        [MenuItem("AiferuFramework/库本身的功能/4.项目使用工具收集/8.美术射线刷草工具Bata %g", false, 4008)]
        static void Open()
        {
            #region 窗口初始化
            if (ins == null)
            {
                ins = (ArtBrushToolEW)EditorWindow.GetWindowWithRect(typeof(ArtBrushToolEW), new Rect(0, 0, 700, 400), false, "ProfabPainter");
            }else
            {
                ins.Focus();
            }
            ins.Show();


            #endregion
            Debug.Log("ArtBrushTool初始化成功");
        }
        private void Awake()
        {
            LoadData();
            #region 初始化
            TexObjects = new Texture[ArtBrushToolData.PlantCount];
            Enable = true;
            Debug.Log(Enable);
            #endregion
        }
        private void OnDisable()
        {
            SaveData();
            Enable = false;
            Debug.Log("ArtBrushTool关闭");
        }

        /// <summary>
        /// 每帧重绘制窗口
        /// </summary>
        void OnInspectorUpdate()
        {
            Repaint();
        }

        /// <summary>
        /// Draw
        /// </summary>
        void OnGUI()
        {
            //获取当前选择
            CurrentSelect = Selection.activeTransform;
            //Debug.Log(CurrentSelect.name);

            GUILayout.Space(20);
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.BeginVertical("box", GUILayout.Width(668));
            GUILayout.BeginHorizontal();
            GUILayout.Label("Add Assets", GUILayout.Width(125));

            AddObject = (GameObject)EditorGUILayout.ObjectField("", AddObject, typeof(GameObject), false, GUILayout.Width(480));

            if (GUILayout.Button("+", GUILayout.Width(40)))
            {
                for (int i = 0; i < ArtBrushToolData.PlantCount; i++)
                {
                    if (data.Plants[i] == null)
                    {
                        data.Plants[i] = AddObject;
                        break;
                    }
                }
            }

            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            for (int i = 0; i < ArtBrushToolData.PlantCount; i++)
            {
                if (data.Plants[i] != null)
                    TexObjects[i] = AssetPreview.GetAssetPreview(data.Plants[i]) as Texture;
                else TexObjects[i] = null;
            }

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.BeginVertical("box", GUILayout.Width(660));
            data.PlantSelect = GUILayout.SelectionGrid(data.PlantSelect, TexObjects, ArtBrushToolData.PlantCount, "gridlist", GUILayout.Width(660), GUILayout.Height(55));

            GUILayout.BeginHorizontal();
            for (int i = 0; i < ArtBrushToolData.PlantCount; i++)
            {
                if (data.Plants[i]!=null)
                {
                     GUILayout.Button(data.Plants[i].name.Substring(Mathf.Max(data.Plants[i].name.Length-7,0),Mathf.Min(7, data.Plants[i].name.Length)), GUILayout.Width(52));
                }else
                {
                    GUILayout.Button("null", GUILayout.Width(52));
                }
                
                
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();

            for (int i = 0; i < ArtBrushToolData.PlantCount; i++)
            {
                if (GUILayout.Button("—", GUILayout.Width(52)))
                {
                    data.Plants[i] = null;
                }
            }

            GUILayout.EndHorizontal();

            GUILayout.EndVertical();
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.BeginVertical("box", GUILayout.Width(668));
            GUILayout.BeginHorizontal();
            GUILayout.Label("Setting", GUILayout.Width(70));
            BrushEnable = GUILayout.Toggle(BrushEnable, "BrushEnable");
            data.BrushIsAddMode = GUILayout.Toggle(data.BrushIsAddMode, "+/-");
            data.NeedCollider = GUILayout.Toggle(data.NeedCollider, "NeedCollider");
            GUILayout.EndHorizontal();
            data.BrushSize = EditorGUILayout.Slider("Brush Size", data.BrushSize, 0.1f, 36f);
            GUILayout.Label("ScaleRandom", GUILayout.Width(145));
            data.ScaleRandomMin = EditorGUILayout.Slider("Scale RandomMin", data.ScaleRandomMin, 0.05f, 1.5f);
            data.ScaleRandomMax = EditorGUILayout.Slider("Scale RandomMax", data.ScaleRandomMax, 0.05f, 1.5f);
            GUILayout.BeginHorizontal();
            GUILayout.Label("RotationRandom", GUILayout.Width(145));
            GUILayout.BeginVertical();
            ArtBrushToolEW.ins.data.RotationMin = EditorGUILayout.Vector3Field("", ArtBrushToolEW.ins.data.RotationMin, GUILayout.Width(145));
            ArtBrushToolEW.ins.data.RotationMax = EditorGUILayout.Vector3Field("",ArtBrushToolEW.ins.data.RotationMax, GUILayout.Width(145));
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
            data.Density = EditorGUILayout.Slider("Density", data.Density, 0.1f, 30f);
            if (GUILayout.Button("创建地图数据对象"))
            {
                data.MapDataObject = new GameObject("MapData");
            }
            if (GUILayout.Button("设置为地图数据对象"))
            {
                data.MapDataObject = Selection.activeGameObject;
            }
            GUILayout.EndVertical();
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.BeginVertical(GUILayout.Width(347));


        }


        #region 存储数据
        private void SaveData()
        {
            if (AssetDatabase.LoadAssetAtPath<ArtBrushToolData>(ArtBrushToolMain.ToolsDataSavePath) == null)
            {
                AssetDatabase.CreateAsset(data, ArtBrushToolMain.ToolsDataSavePath);
            }
            EditorUtility.SetDirty(data);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private void LoadData()
        {
            data = AssetDatabase.LoadAssetAtPath<ArtBrushToolData>(ArtBrushToolMain.ToolsDataSavePath);
            if (data == null)
            {
                data = ScriptableObject.CreateInstance<ArtBrushToolData>();
            }

            #endregion
        }
    }
}
#endif