using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
namespace AiferuFramework
{
    [CustomEditor(typeof(StepLoading))]
    public class StepLoadingEW : Editor
    {
        //隐藏所有的预制体对象
        //显示所有的预制体对象
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            StepLoading stepLoading = (StepLoading)target;
            if (GUILayout.Button("显示所有对象"))
            {
                stepLoading.ShowAllProfab();
                Debug.Log("显示所有对象");
            }
            if (GUILayout.Button("关闭所有对象"))
            {
                stepLoading.HideAllProfab();
                Debug.Log("隐藏所有对象");
            }
        }
    }
    
}


