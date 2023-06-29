using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
namespace AiferuFramework
{
    [CustomEditor(typeof(StepLoading))]
    public class StepLoadingEW : Editor
    {
        //�������е�Ԥ�������
        //��ʾ���е�Ԥ�������
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            StepLoading stepLoading = (StepLoading)target;
            if (GUILayout.Button("��ʾ���ж���"))
            {
                stepLoading.ShowAllProfab();
                Debug.Log("��ʾ���ж���");
            }
            if (GUILayout.Button("�ر����ж���"))
            {
                stepLoading.HideAllProfab();
                Debug.Log("�������ж���");
            }
        }
    }
    
}


