//
//  CameraFollowsInTheScene.cs
//  AiferuFramework
//
//  Created by Aiferu on 2023/5/4.
//
//
#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

namespace AiferuFramework
{
    /// <summary>
    /// Scene下摄像机跟随
    /// </summary>
    [InitializeOnLoad]
    public class CameraFollowsInTheScene 
    {
        #region Field
        private static bool CameraisFollowEnable;
        private static bool[] CameraisFollow;
        //当前场景所有摄像机
        private static Camera[] AllCameras;
        //当前需要跟随的摄像机位置
        private static Transform TargetCameraTransform;
        private static Vector3 Cam_position = Vector3.zero;
        private static Vector3 Cam_rotation = Vector3.zero;
        #endregion

        #region Property

        #endregion

        #region UnityOriginalEvent

        static CameraFollowsInTheScene()
        {
            //绑定场景GUI渲染回调
            SceneView.duringSceneGui += OnSceneGUI;


            //初始化数组
            CameraisFollow = new bool[20];
        }

        ~CameraFollowsInTheScene ()
        {
            SceneView.duringSceneGui -= OnSceneGUI;
        }


        #endregion

        #region Function

        private static void OnSceneGUI(SceneView view)
        {
            //获取当前场景所有摄像机
            AllCameras = Camera.allCameras;

            Handles.BeginGUI();
            GUILayout.BeginArea(new Rect(0, 0, 150, 2000)); // 规定显示区域为大小

            // -----------------------------------------------
            //在分割线内，添加代码

            //启动按钮
            CameraisFollowEnable = GUILayout.Toggle(CameraisFollowEnable, "CameraisFollowEnable");

            if (CameraisFollowEnable)
            {
                DrawButton();
                CameraFollow();
            }else
            {
                TargetCameraTransform = null;
            }
            // -----------------------------------------------

            GUILayout.EndArea();
            Handles.EndGUI();

        }

        /// <summary>
        /// 渲染按钮
        /// </summary>
        private static void DrawButton()
        {
            GUILayout.BeginVertical("box");

            for (int i = 0; i < AllCameras.Length; i++)
            {
                GUILayout.BeginHorizontal("box");

                CameraisFollow[i] = GUILayout.Toggle(CameraisFollow[i], "");

                if (GUILayout.Button(AllCameras[i].name))
                {
                    if (CameraisFollow[i])
                    {
                        TargetCameraTransform = AllCameras[i].transform;
                    }else
                    {
                        TargetCameraTransform = null;
                    }
                    Selection.activeObject = AllCameras[i];
                }else if (TargetCameraTransform == AllCameras[i].transform && !CameraisFollow[i])
                {
                    TargetCameraTransform = null;
                }

                GUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();
        }

        /// <summary>
        /// 摄像机跟随
        /// </summary>
        private static void CameraFollow()
        {
            Cam_position = SceneView.lastActiveSceneView.camera.transform.position;
            Cam_rotation = new Vector3(
            SceneView.lastActiveSceneView.camera.transform.rotation.eulerAngles.x,
            SceneView.lastActiveSceneView.camera.transform.rotation.eulerAngles.y, 0);
            if (TargetCameraTransform!=null)
            {
                TargetCameraTransform.position = Cam_position;
                TargetCameraTransform.rotation = Quaternion.Euler(Cam_rotation);
            }
 
        }


        #endregion
    }
}
#endif