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
    /// 存储动画状态机工具的信息
    /// </summary>
    [CreateAssetMenu(fileName = "AnimatorSettingToolsBaseData", menuName = "AnimatorSettingTools/BaseData", order = 1)]
    [Serializable]
    public class AnimatorSettingToolsBaseData : ScriptableObject
    {
        //动画分组

        public List<string> AnimationClipNames = new List<string> {
        "BufferB",
        "BufferF",
        "BufferL",
        "BufferR",
        "EmoDown",
        "EmoStay",
        "EmoUp",
        "Idle",
        "ImmortalDown",
        "ImmortalStay",
        "ImmortalUp",
        "LaunchDown",
        "LaunchStay",
        "LaunchUp",
        "SitDown",
        "SitStay",
        "SitUp",
        "SleepDownB",
        "SleepDownF",
        "SleepDownL",
        "SleepDownR",
        "SleepStayB",
        "SleepStayF",
        "SleepStayL",
        "SleepStayR",
        "SleepUpB",
        "SleepUpF",
        "SleepUpL",
        "SleepUpR",
        "SoakFeet",
        "WalkB",
        "WalkF",
        "WalkL",
        "WalkR",
        "Wave",
        "Dig",
        "Hold3",
        "hold1",
        "Hold2",
        "Jump",
        "Pick",
        "Run",
        "Write",
        "Walk",
        "SleepDown",
        "SleepStay",
        "SleepUP",
        "举牌1",
    };

        public List<string> animatorTemplatePath = new List<string>()
        {
            "Assets/测试用工程素材/AAA_Animators/Template/FansTemplate.controller",
            "Assets/测试用工程素材/AAA_Animators/Template/PlayTemplate.controller",
        };

        public List<string> animationGroup = new List<string>()
        {
            "C_L_CPFF_0194_AviationTeenager",
            "C_L_NINA_0204_Angell",
            "C_L_Unicorn_0200_BluishViolet",
            "C_L_LowPoly",
            "C_L_Fans_0254_GirlBumpPling",
            "C_L_Fans_0263_Swordsman",
            "C_L_Fans_0264_Assassin",
            "C_L_Fans_0272_YiYi",
            "C_L_Fans_0274_YaoYao",
            "C_L_Fans_0281_Elves",
            "C_L_Fans_0289_RedPanda",
            "C_L_Fans_0290_Fox",
            "C_L_Universe_0206_Moon",
            "C_L_SINDY_0222_TiredTiger",
            "C_Constellation_0231_Aries",
            "C_L_Fans_0290_Fox",
        };

        public enum AnimatorTemplate
        {
            Other = 0,
            FansTemplate,
            PlayTemplate,
            //Count
        }

        public enum AnimationGroup
        {
            Other = 0,
            CPFF,
            NINA,
            Unicorn,
            Lowpoly,
            fan254,
            fan263,
            fan264,
            fan272,
            fan274,
            fan281,
            fan289,
            fan290,
            Universe,
            SINDY,
            Constellation,
            Dreamer,
            Count
        }
    }
}
#endif
