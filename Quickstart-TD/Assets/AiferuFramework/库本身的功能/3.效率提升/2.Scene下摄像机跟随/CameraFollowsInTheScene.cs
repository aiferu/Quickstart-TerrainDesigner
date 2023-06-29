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
    /// Scene�����������
    /// </summary>
    [InitializeOnLoad]
    public class CameraFollowsInTheScene 
    {
        #region Field
        private static bool CameraisFollowEnable;
        private static bool[] CameraisFollow;
        //��ǰ�������������
        private static Camera[] AllCameras;
        //��ǰ��Ҫ����������λ��
        private static Transform TargetCameraTransform;
        private static Vector3 Cam_position = Vector3.zero;
        private static Vector3 Cam_rotation = Vector3.zero;
        #endregion

        #region Property

        #endregion

        #region UnityOriginalEvent

        static CameraFollowsInTheScene()
        {
            //�󶨳���GUI��Ⱦ�ص�
            SceneView.duringSceneGui += OnSceneGUI;


            //��ʼ������
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
            //��ȡ��ǰ�������������
            AllCameras = Camera.allCameras;

            Handles.BeginGUI();
            GUILayout.BeginArea(new Rect(0, 0, 150, 2000)); // �涨��ʾ����Ϊ��С

            // -----------------------------------------------
            //�ڷָ����ڣ���Ӵ���

            //������ť
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
        /// ��Ⱦ��ť
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
        /// ���������
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