#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;


[InitializeOnLoad]
public class EditorHotkeysTracker
{
    static EditorHotkeysTracker()
    {
        SceneView.duringSceneGui += view =>
    {
        if (Event.current.type == EventType.KeyDown)
        {
            if (Event.current.keyCode == KeyCode.Space)
            {
                Debug.Log("¿Õ¸ñ°´ÏÂ");
            }
        }
    };
    }
}

#endif