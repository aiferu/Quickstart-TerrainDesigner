using System.Text;
using Unity.Profiling;
using UnityEngine;

namespace AiferuFramework.GameStats
{
    /// <summary>
    /// ‰÷»æÕ≥º∆
    /// </summary>
    public class RenderStatsScript : MonoBehaviour
    {
        [SerializeField]
        private bool testMode = true;
        string statsText;
        ProfilerRecorder setPassCallsRecorder;
        ProfilerRecorder drawCallsRecorder;
        ProfilerRecorder verticesRecorder;
        ProfilerRecorder trisRecorder;


        void OnEnable()
        {
            setPassCallsRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "SetPass Calls Count");
            drawCallsRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Draw Calls Count");
            verticesRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Vertices Count");
            trisRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Triangles Count");
        }

        void OnDisable()
        {
            setPassCallsRecorder.Dispose();
            drawCallsRecorder.Dispose();
            verticesRecorder.Dispose();
            trisRecorder.Dispose();
        }

        void Update()
        {
            var sb = new StringBuilder(500);
            if (setPassCallsRecorder.Valid)
                sb.AppendLine($"SetPass Calls: {setPassCallsRecorder.LastValue}");
            if (drawCallsRecorder.Valid)
                sb.AppendLine($"Draw Calls: {drawCallsRecorder.LastValue}");
            if (verticesRecorder.Valid)
                sb.AppendLine($"Vertices: {verticesRecorder.LastValue}");
            if (trisRecorder.Valid)
                sb.AppendLine($"Tris: {trisRecorder.LastValue}");
            statsText = sb.ToString();
        }

        void OnGUI()
        {
            if (!testMode)
                return;
            GUIStyle fontsStyle = new GUIStyle();
            //fontsStyle.normal.background = nu;
            fontsStyle.normal.textColor = Color.white;
            fontsStyle.fontSize = 40;
            GUI.TextArea(new Rect(200, 300, 500, 100), statsText, fontsStyle);
        }
    }
}
