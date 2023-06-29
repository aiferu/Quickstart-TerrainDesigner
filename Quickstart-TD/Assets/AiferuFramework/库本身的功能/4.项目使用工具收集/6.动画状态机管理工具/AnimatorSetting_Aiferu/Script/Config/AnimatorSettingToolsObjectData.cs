#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace AiferuFramework.AnimatorSettingTools
{
    /// <summary>
    /// 存储蒙皮预制体的信息
    /// </summary>
    [Serializable]
    public class AnimatorSettingToolsObjectData : ScriptableObject
    {
        public string ObjectDataSavePath;
        //名称
        public string SkinProfabName;
        //预制体的引用
        public GameObject SkinProfab;

        //动画状态机模板
        public AnimatorSettingToolsBaseData.AnimatorTemplate AnimatorTemplate = AnimatorSettingToolsBaseData.AnimatorTemplate.Other;
        //动画分组
        public AnimatorSettingToolsBaseData.AnimationGroup AnimationGroup = AnimatorSettingToolsBaseData.AnimationGroup.Other;

        public string BuildAnimatorSavePath;


    }
}

#endif