using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AiferuFramework.GameStats
{
    /// <summary>
    /// Ö¡ÂÊÏÞÖÆ
    /// </summary>
    public class FrameRateLimit : MonoBehaviour
    {
        [SerializeField]
        bool enable = true;
        [SerializeField]
        private int MaxFPS = 45;

        private void Awake()
        {
            if (enable)
            {
                Application.targetFrameRate = MaxFPS;
            }

        }
    }
}

