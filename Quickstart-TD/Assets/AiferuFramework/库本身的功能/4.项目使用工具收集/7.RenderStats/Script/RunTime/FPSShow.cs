using System;
using UnityEngine;

namespace AiferuFramework.GameStats
{
    /// <summary>
    /// 帧数显示器
    /// </summary>
    public class FPSShow : MonoBehaviour
    {
        [SerializeField]
        private bool testMode = true;
        /// <summary>
        /// 单位统计时间
        /// </summary>
        private const float fpsMeasureTime = 1f;

        /// <summary>
        /// 单位帧数统计时间
        /// </summary>
        private const int fpsMeasureFrame = 30;

        /// <summary>
        /// 帧数统计
        /// </summary>
        private int fpsCount;

        /// <summary>
        /// 计时器
        /// </summary>
        private float timer;

        /// <summary>
        /// 单位时间帧数
        /// </summary>
        private int fps;

        /// <summary>
        /// 单位帧数耗时
        /// </summary>
        private float timeUse;

        private string result;
        private GUIStyle style;
        private Rect rect;

        /// <summary>
        /// FPS统计类型
        /// </summary>
        public enum FPSType
        {
            FixedTime,
            FixedFrame
        }
        /// <summary>
        /// FPS在屏幕中的位置
        /// </summary>
        public enum RectType
        {
            UpLeft,
            UpMiddle,
            UpRight,
            DownLeft,
            DownMiddle,
            DownRight,
            Middle,
            MiddleLeft,
            MiddleRight
        }

        public FPSType fpsType;
        public RectType rectType;

        private void Start()
        {
            timer = 0f;
            fpsCount = 0;
        }

        private void Update()
        {
            if (fpsType == FPSType.FixedTime)
            {
                //固定时间帧数法
                FixedTimeFPS();
            }
            else if (fpsType == FPSType.FixedFrame)
            {
                //固定帧数时间法
                FixedFPSTime();
            }
        }

        /// <summary>
        /// 固定帧数时间法
        /// </summary>
        private void FixedFPSTime()
        {
            timer += Time.deltaTime;
            fpsCount += 1;

            if (timer >= fpsMeasureTime)
            {
                fps = Mathf.RoundToInt(fpsCount / timer);

                result = "FPS：" + fps.ToString();

                fpsCount = 0;
                timer = 0f;
            }

        }

        /// <summary>
        /// 固定时间帧数法
        /// </summary>
        private void FixedTimeFPS()
        {
            timer += Time.deltaTime;
            fpsCount += 1;

            if (fpsCount >= fpsMeasureFrame)
            {
                timeUse = timer / fpsCount;

                result = "TPF：" + Math.Round(timeUse, 2).ToString();

                fpsCount = 0;
                timer = 0f;
            }
        }

        /// <summary>
        /// GUI可视化窗口
        /// </summary>
        private void OnGUI()
        {
            if (!testMode)
                return;
            switch (rectType)
            {
                case RectType.UpLeft:
                    rect = new Rect(0, 0, 200, 200);
                    break;
                case RectType.UpMiddle:
                    rect = new Rect(Screen.width / 2, 0, 200, 200);
                    break;
                case RectType.UpRight:
                    rect = new Rect(Screen.width - 70, 0, 200, 200);
                    break;
                case RectType.Middle:
                    rect = new Rect(Screen.width / 2, Screen.height / 2, 200, 200);
                    break;
                case RectType.MiddleLeft:
                    rect = new Rect(0, Screen.height / 2, 200, 200);
                    break;
                case RectType.MiddleRight:
                    rect = new Rect(Screen.width - 70, Screen.height / 2, 200, 200);
                    break;
                case RectType.DownLeft:
                    rect = new Rect(0, Screen.height - 20, 200, 200);
                    break;
                case RectType.DownMiddle:
                    rect = new Rect(Screen.width / 2, Screen.height - 20, 200, 200);
                    break;
                case RectType.DownRight:
                    rect = new Rect(Screen.width - 70, Screen.height - 20, 200, 200);
                    break;
                default:
                    break;
            }
            GUIStyle fontsStyle = new GUIStyle();
            //fontsStyle.normal.background = nu;
            fontsStyle.normal.textColor = Color.white;
            fontsStyle.fontSize = 40;

            GUI.Label(rect, result, fontsStyle);
        }
    }
}

