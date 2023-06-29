using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AiferuFramework.GameStats
{
    /// <summary>
    /// ¶¯Ì¬·Ö±æÂÊ
    /// </summary>
    public class DynamicResolution : MonoBehaviour
    {
        [SerializeField]
        private bool testMode = true;
        string screenText;
        [Range(0.1f, 1f)]
        public float scale = 1f;

        FrameTiming[] frameTimings = new FrameTiming[3];


        float m_widthScale = 1.0f;
        float m_heightScale = 1.0f;

        // Variables for dynamic resolution algorithm that persist across frames
        uint m_frameCount = 0;

        const uint kNumFrameTimings = 2;

        double m_gpuFrameTime;
        double m_cpuFrameTime;

        // Use this for initialization
        void Start()
        {
            int rezWidth = (int)Mathf.Ceil(ScalableBufferManager.widthScaleFactor * Screen.currentResolution.width);
            int rezHeight = (int)Mathf.Ceil(ScalableBufferManager.heightScaleFactor * Screen.currentResolution.height);
            screenText = string.Format("Scale: {0:F3}x{1:F3}\nResolution: {2}x{3}\n",
                m_widthScale,
                m_heightScale,
                rezWidth,
                rezHeight);
        }

        // Update is called once per frame
        void Update()
        {
            float oldWidthScale = m_widthScale;
            float oldHeightScale = m_heightScale;

            m_heightScale = scale;
            m_widthScale = scale;

            if (m_widthScale != oldWidthScale || m_heightScale != oldHeightScale)
            {
                ScalableBufferManager.ResizeBuffers(m_widthScale, m_heightScale);
            }
            
            DetermineResolution();
            int rezWidth = (int)Mathf.Ceil(ScalableBufferManager.widthScaleFactor * Screen.currentResolution.width);
            int rezHeight = (int)Mathf.Ceil(ScalableBufferManager.heightScaleFactor * Screen.currentResolution.height);
            screenText = string.Format("Scale: {0:F3}x{1:F3}\nResolution: {2}x{3}\nScaleFactor: {4:F3}x{5:F3}\nGPU: {6:F3} CPU: {7:F3}",
                m_widthScale,
                m_heightScale,
                rezWidth,
                rezHeight,
                ScalableBufferManager.widthScaleFactor,
                ScalableBufferManager.heightScaleFactor,
                m_gpuFrameTime,
                m_cpuFrameTime);
        }

        // Estimate the next frame time and update the resolution scale if necessary.
        private void DetermineResolution()
        {
            ++m_frameCount;
            if (m_frameCount <= kNumFrameTimings)
            {
                return;
            }
            FrameTimingManager.CaptureFrameTimings();
            FrameTimingManager.GetLatestTimings(kNumFrameTimings, frameTimings);
            if (frameTimings.Length < kNumFrameTimings)
            {
                Debug.LogFormat("Skipping frame {0}, didn't get enough frame timings.",
                    m_frameCount);

                return;
            }

            m_gpuFrameTime = (double)frameTimings[0].gpuFrameTime;
            m_cpuFrameTime = (double)frameTimings[0].cpuFrameTime;
        }

        private void OnGUI()
        {
            if (testMode)
            {
                GUIStyle fontsStyle = new GUIStyle();
                //fontsStyle.normal.background = nu;
                fontsStyle.normal.textColor = Color.white;
                fontsStyle.fontSize = 40;
                GUI.TextArea(new Rect(200, 600, 500, 100), screenText, fontsStyle);
            }
        }
    }
}