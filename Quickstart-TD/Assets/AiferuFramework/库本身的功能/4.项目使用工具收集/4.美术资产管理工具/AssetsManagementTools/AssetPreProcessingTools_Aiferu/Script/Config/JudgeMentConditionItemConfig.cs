#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace AiferuFramework.AssetsManagementTools
{
    [CreateAssetMenu(fileName = "JudgeMentConditionItemConfig", menuName = "InputSettingAssets/JudgeMentConditionItemConfig", order = 6)]
    public class JudgeMentConditionItemConfig : ScriptableObject
    {


        public enum JudgmentType
        {
            Equal,
            NotEqual,
        }

        public string RuleName = "规则0";
        //判断类型
        public JudgmentType judgmentType = JudgmentType.Equal;
        //关键字
        public string keyword = "";
        //优先级
        public int priority = 1;

        public string filePath = "";
    }
}
#endif