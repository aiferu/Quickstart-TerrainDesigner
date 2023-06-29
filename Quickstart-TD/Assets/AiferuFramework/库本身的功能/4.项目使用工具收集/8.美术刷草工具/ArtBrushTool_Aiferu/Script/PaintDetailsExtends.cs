#if UNITY_EDITOR
using SVTXPainterEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace AiferuFramework.ArtBrushTool
{
    [InitializeOnLoad]
    public class PaintDetailsEXtends
    {
        private static GameObject PreviewModel;

        private static int layerMask =0;
        #region �󶨳���GUI��Ⱦ�ص�
        static PaintDetailsEXtends()
        {
            //�󶨳���GUI��Ⱦ��
            SceneView.duringSceneGui += OnSceneGUI;
        }

        //~PaintDetailsEXtends()
        //{
        //    SceneView.duringSceneGui -= OnSceneGUI;
        //}
        #endregion

        //Scene���ص�����
        static void OnSceneGUI(SceneView view)
        {
            Event e = Event.current;

            if (ArtBrushToolEW.ins == null)
                return;
            if (e.alt)
            {
                return;
            }
            //�ж��Ƿ���������ˢ�ݹ���
            if (ArtBrushToolEW.ins.Enable)
            {
                Planting();
            }
        }


        static void Planting()
        {
            //ʹ������ȡ���潻��
            Event e = Event.current;
            //�����������ʱ
            RaycastHit raycastHit = new RaycastHit();
            Ray terrainRay = HandleUtility.GUIPointToWorldRay(e.mousePosition);
            //Debug.DrawLine(terrainRay.origin, terrainRay.GetPoint(100), Color.red);\
            bool isHit;
            if (ArtBrushToolEW.ins.data.NeedCollider)
            {
                isHit = Physics.Raycast(terrainRay, out raycastHit, Mathf.Infinity);
            }else
            {
                GameObject seletedObject = Selection.activeObject as GameObject;
                MeshFilter meshFilter = FindMeshFilter(seletedObject);
                if (meshFilter == null) return;
                isHit =RXLookingGlass.IntersectRayMesh(terrainRay,meshFilter ,out raycastHit);
            }
            //����Ԥ��ģ��
            //GameObject target = ArtBrushToolEW.ins.data.Plants[ArtBrushToolEW.ins.data.PlantSelect];
            //target.transform.position = raycastHit.point;
            if (isHit)
            {
                //������껮��λ�úͱ༭��������õ��ܶȵȲ���ʵ����ֲ�� �����ϱ��
                //ʵ����ֲ��
                if (e.type == EventType.MouseDown && e.button == 0 && ArtBrushToolEW.ins.BrushEnable)
                {
                    if (ArtBrushToolEW.ins.data.BrushIsAddMode)
                    {
                        //���ģʽ
                        AddBrush(raycastHit);
                    }else
                    {
                        //ɾ��ģʽ
                        SubBrush(raycastHit);
                    }

                }
            }
            //���Ʊ�ˢ
            DrawBrush(raycastHit);
        }

        /// <summary>
        /// ɾ����ˢ
        /// </summary>
        private static void SubBrush(RaycastHit hit)
        {
            Debug.Log("ɾ��ģʽ��tmûд,��Ϲ��");
            //��������
            for (int i = 0; i < ArtBrushToolEW.ins.data.Density * ArtBrushToolEW.ins.data.BrushSize; i++)
            {
                Vector2 randomPoint = UnityEngine.Random.insideUnitCircle * (ArtBrushToolEW.ins.data.BrushSize / 2);
                Vector3 randomPoint3 = new Vector3(randomPoint.x, 0, randomPoint.y) + hit.point;
                Ray ray = new Ray(randomPoint3, hit.normal);
                Handles.DrawLine(randomPoint3, randomPoint3 + (hit.normal * ArtBrushToolEW.ins.data.BrushSize / 2));
                //Debug.Log(randomPoint3);
            }
        }

        /// <summary>
        /// ��ӱ�ˢ
        /// </summary>
        private static void AddBrush(RaycastHit hit)
        {
            //���ؿ�ʼ
            Undo.IncrementCurrentGroup();
            //��������
            for (int i = 0; i <Mathf.Clamp(ArtBrushToolEW.ins.data.Density * Mathf.Pow(ArtBrushToolEW.ins.data.BrushSize/2,2)*Mathf.PI,1f,30520f); i++)
            {
                //������������
                Vector2 randomPoint = UnityEngine.Random.insideUnitCircle * (ArtBrushToolEW.ins.data.BrushSize / 2);
                Vector3 randomPoint3 = new Vector3(randomPoint.x, 0, randomPoint.y);

                //�Ʒ�������ת����
                //���㷨����������y�����ת
                Quaternion rotation = Quaternion.FromToRotation(Vector3.up,hit.normal);
                Vector3 newPos = rotation* randomPoint3+hit.point;

                Ray ray = new Ray(newPos + hit.normal*Mathf.Clamp(ArtBrushToolEW.ins.data.BrushSize/36,0.1f,10f), -hit.normal);

                RaycastHit targetHit = new RaycastHit();
                if (ArtBrushToolEW.ins.data.NeedCollider)
                {
                    Physics.Raycast(ray, out targetHit, Mathf.Infinity);
                }
                else
                {
                    GameObject seletedObject = Selection.activeObject as GameObject;
                    MeshFilter meshFilter = FindMeshFilter(seletedObject);
                    if (meshFilter == null) return;
                    RXLookingGlass.IntersectRayMesh(ray, meshFilter, out targetHit);
                }
                
                Debug.DrawRay(ray.origin, ray.direction, Color.blue, 1f);
                //Debug.Log(randomPoint3);
                //Debug.Log(targetHit.point);
                InsProfab(targetHit);
            }

            //���ؽ���
        }

        private static void InsProfab(RaycastHit hit)
        {
            GameObject target = ArtBrushToolEW.ins.data.Plants[ArtBrushToolEW.ins.data.PlantSelect];
            if (hit.point == Vector3.zero) return;
            if (target == null) return;
            if (ArtBrushToolEW.ins.data.MapDataObject == null)
            {
                Debug.Log("���,�ȴ�����ͼ���ݶ���!!!!");
                return;
            }
            GameObject go = PrefabUtility.InstantiatePrefab(target,ArtBrushToolEW.ins.data.MapDataObject.transform) as GameObject;
            go.transform.position = hit.point;
            go.transform.up = hit.normal;
            float scale = UnityEngine.Random.Range(ArtBrushToolEW.ins.data.ScaleRandomMin, ArtBrushToolEW.ins.data.ScaleRandomMax);
            go.transform.localScale = go.transform.localScale * scale;
            Vector3 minRange = ArtBrushToolEW.ins.data.RotationMin; // ��С��Χ
            Vector3 maxRange = ArtBrushToolEW.ins.data.RotationMax; // ���Χ
            // �������3ά����
            Vector3 randomVector = new Vector3(
                UnityEngine.Random.Range(minRange.x, maxRange.x),
                UnityEngine.Random.Range(minRange.y, maxRange.y),
                UnityEngine.Random.Range(minRange.z, maxRange.z)
            );
            go.transform.localRotation = Quaternion.Euler(randomVector);

            //���ؽ���
            Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
        }

        /// <summary>
        /// ���Ʊ�ˢ
        /// </summary>
        private static void DrawBrush(RaycastHit hit)
        {
            if (ArtBrushToolEW.ins.BrushEnable)
            {
                //���ñ༭ģʽΪ��,����ˢ��ˢ��ʱ����޷�ʹ���ƶ���ת��
                Tools.current = Tool.None;
                //����Ĭ�ϵ�ѡ��,�����Ͳ���ѡ������
                HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));

                //���Ʊ�ˢ��ʽ
                Handles.color = new Color(1,1,1,0.1f);
                Handles.DrawSolidDisc(hit.point, hit.normal, ArtBrushToolEW.ins.data.BrushSize/2);
                Handles.color = Color.red;
                Handles.DrawWireDisc(hit.point, hit.normal, ArtBrushToolEW.ins.data.BrushSize/2);
                Handles.DrawLine(hit.point, hit.point+(hit.normal* ArtBrushToolEW.ins.data.BrushSize / 2));




            }

        }

        private static MeshFilter FindMeshFilter(GameObject selectedObject)
        {
            if (selectedObject != null)
            {
                var meshFilter = FindMeshFilterInChildren(selectedObject);
                if (meshFilter != null)
                {
                    // �ҵ��� MeshFilter ���
                    //Debug.Log("Found MeshFilter on: " + meshFilter.name);
                    return meshFilter;
                }
                else
                {
                    // û���ҵ� MeshFilter ���
                    //Debug.Log("No MeshFilter found in the selected object or its children.");
                    return null;
                }
            }
            else
            {
                // û��ѡ�ж���
                //Debug.Log("No object selected.");
                return null;
            }
        }

        private static MeshFilter FindMeshFilterInChildren(GameObject obj)
        {
            MeshFilter meshFilter = obj.GetComponent<MeshFilter>();
            if (meshFilter != null)
            {
                return meshFilter;
            }

            // �����Ӷ���
            foreach (Transform child in obj.transform)
            {
                meshFilter = FindMeshFilterInChildren(child.gameObject);
                if (meshFilter != null)
                {
                    return meshFilter;
                }
            }

            return null;
        }
    }
}
#endif
