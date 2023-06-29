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
        #region 绑定场景GUI渲染回调
        static PaintDetailsEXtends()
        {
            //绑定场景GUI渲染回
            SceneView.duringSceneGui += OnSceneGUI;
        }

        //~PaintDetailsEXtends()
        //{
        //    SceneView.duringSceneGui -= OnSceneGUI;
        //}
        #endregion

        //Scene面板回调函数
        static void OnSceneGUI(SceneView view)
        {
            Event e = Event.current;

            if (ArtBrushToolEW.ins == null)
                return;
            if (e.alt)
            {
                return;
            }
            //判断是否开启了美术刷草工具
            if (ArtBrushToolEW.ins.Enable)
            {
                Planting();
            }
        }


        static void Planting()
        {
            //使用射线取地面交点
            Event e = Event.current;
            //当鼠标左键点击时
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
            //生成预览模型
            //GameObject target = ArtBrushToolEW.ins.data.Plants[ArtBrushToolEW.ins.data.PlantSelect];
            //target.transform.position = raycastHit.point;
            if (isHit)
            {
                //根据鼠标划过位置和编辑器面板设置的密度等参数实例化植被 并打上标记
                //实例化植被
                if (e.type == EventType.MouseDown && e.button == 0 && ArtBrushToolEW.ins.BrushEnable)
                {
                    if (ArtBrushToolEW.ins.data.BrushIsAddMode)
                    {
                        //添加模式
                        AddBrush(raycastHit);
                    }else
                    {
                        //删除模式
                        SubBrush(raycastHit);
                    }

                }
            }
            //绘制笔刷
            DrawBrush(raycastHit);
        }

        /// <summary>
        /// 删除笔刷
        /// </summary>
        private static void SubBrush(RaycastHit hit)
        {
            Debug.Log("删除模式还tm没写,别瞎点");
            //区域射线
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
        /// 添加笔刷
        /// </summary>
        private static void AddBrush(RaycastHit hit)
        {
            //撤回开始
            Undo.IncrementCurrentGroup();
            //区域射线
            for (int i = 0; i <Mathf.Clamp(ArtBrushToolEW.ins.data.Density * Mathf.Pow(ArtBrushToolEW.ins.data.BrushSize/2,2)*Mathf.PI,1f,30520f); i++)
            {
                //计算射线坐标
                Vector2 randomPoint = UnityEngine.Random.insideUnitCircle * (ArtBrushToolEW.ins.data.BrushSize / 2);
                Vector3 randomPoint3 = new Vector3(randomPoint.x, 0, randomPoint.y);

                //绕法向量旋转坐标
                //计算法向量与世界y轴的旋转
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

            //撤回结束
        }

        private static void InsProfab(RaycastHit hit)
        {
            GameObject target = ArtBrushToolEW.ins.data.Plants[ArtBrushToolEW.ins.data.PlantSelect];
            if (hit.point == Vector3.zero) return;
            if (target == null) return;
            if (ArtBrushToolEW.ins.data.MapDataObject == null)
            {
                Debug.Log("大哥,先创建地图数据对象!!!!");
                return;
            }
            GameObject go = PrefabUtility.InstantiatePrefab(target,ArtBrushToolEW.ins.data.MapDataObject.transform) as GameObject;
            go.transform.position = hit.point;
            go.transform.up = hit.normal;
            float scale = UnityEngine.Random.Range(ArtBrushToolEW.ins.data.ScaleRandomMin, ArtBrushToolEW.ins.data.ScaleRandomMax);
            go.transform.localScale = go.transform.localScale * scale;
            Vector3 minRange = ArtBrushToolEW.ins.data.RotationMin; // 最小范围
            Vector3 maxRange = ArtBrushToolEW.ins.data.RotationMax; // 最大范围
            // 生成随机3维向量
            Vector3 randomVector = new Vector3(
                UnityEngine.Random.Range(minRange.x, maxRange.x),
                UnityEngine.Random.Range(minRange.y, maxRange.y),
                UnityEngine.Random.Range(minRange.z, maxRange.z)
            );
            go.transform.localRotation = Quaternion.Euler(randomVector);

            //撤回结束
            Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
        }

        /// <summary>
        /// 绘制笔刷
        /// </summary>
        private static void DrawBrush(RaycastHit hit)
        {
            if (ArtBrushToolEW.ins.BrushEnable)
            {
                //设置编辑模式为无,这样刷笔刷的时候就无法使用移动旋转等
                Tools.current = Tool.None;
                //禁用默认的选择,这样就不会选中物体
                HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));

                //绘制笔刷样式
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
                    // 找到了 MeshFilter 组件
                    //Debug.Log("Found MeshFilter on: " + meshFilter.name);
                    return meshFilter;
                }
                else
                {
                    // 没有找到 MeshFilter 组件
                    //Debug.Log("No MeshFilter found in the selected object or its children.");
                    return null;
                }
            }
            else
            {
                // 没有选中对象
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

            // 遍历子对象
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
