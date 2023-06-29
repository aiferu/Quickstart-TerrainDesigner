#if UNITY_EDITOR
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEditor;
using System.Collections.Generic;
using System.Numerics;

namespace AiferuFramework.ArtBrushTool
{
    /// <summary>
    /// ����ˢ�ݹ���
    /// </summary>
    public class ArtBrushToolEW : EditorWindow
    {
        public static ArtBrushToolEW ins;

        #region ��������
        /// <summary>
        /// �����Ƿ�����
        /// </summary>
        public bool Enable;
        /// <summary>
        /// �Ƿ�����ˢ
        /// </summary>
        public bool BrushEnable;
        /// <summary>
        /// ��ӵĶ���
        /// </summary>
        private GameObject AddObject;
        /// <summary>
        /// ��ǰѡ��Ķ���
        /// </summary>
        public Transform CurrentSelect;
        /// <summary>
        /// �ݶ�������ͼ����
        /// </summary>
        private Texture[] TexObjects;
        /// <summary>
        /// ���ݴ洢
        /// </summary>
        public ArtBrushToolData data;
        #endregion

        [MenuItem("AiferuFramework/�Ȿ��Ĺ���/4.��Ŀʹ�ù����ռ�/8.��������ˢ�ݹ���Bata %g", false, 4008)]
        static void Open()
        {
            #region ���ڳ�ʼ��
            if (ins == null)
            {
                ins = (ArtBrushToolEW)EditorWindow.GetWindowWithRect(typeof(ArtBrushToolEW), new Rect(0, 0, 700, 400), false, "ProfabPainter");
            }else
            {
                ins.Focus();
            }
            ins.Show();


            #endregion
            Debug.Log("ArtBrushTool��ʼ���ɹ�");
        }
        private void Awake()
        {
            LoadData();
            #region ��ʼ��
            TexObjects = new Texture[ArtBrushToolData.PlantCount];
            Enable = true;
            Debug.Log(Enable);
            #endregion
        }
        private void OnDisable()
        {
            SaveData();
            Enable = false;
            Debug.Log("ArtBrushTool�ر�");
        }

        /// <summary>
        /// ÿ֡�ػ��ƴ���
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
            //��ȡ��ǰѡ��
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
                if (GUILayout.Button("��", GUILayout.Width(52)))
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
            if (GUILayout.Button("������ͼ���ݶ���"))
            {
                data.MapDataObject = new GameObject("MapData");
            }
            if (GUILayout.Button("����Ϊ��ͼ���ݶ���"))
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


        #region �洢����
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