using Codice.CM.Common.Update.Partial;
using RossoForge.Events.Service;
using RossoForge.Services.Locator;
using UnityEditor;

namespace RossoForge.Events.Editor
{
    public class EventViewerWindow: EditorWindow
    {
        [MenuItem("RossoForge/Events/Viewer")]
        public static void ShowWindow()
        {
            var window = GetWindow<EventViewerWindow>("RossoForge - Event Viewer");
            window.Show();
        }

        private void OnGUI()
        {
            if (!CheckPlayMode())
                return;

            if (!CheckEventService())
                return;

            //_eventService = ServiceLocator.Get<IEventService>();
            UnityEngine.Debug.LogWarning("TEST");
        }

        private bool CheckPlayMode()
        {
            if (!EditorApplication.isPlaying)
            {
                EditorGUILayout.HelpBox("Press Play to see real-time event information.", MessageType.Info);
                return false;
            }

            return true;
        }

        private bool CheckEventService()
        {
            if (!ServiceLocator.TryGet<IEventService>(out _))
            {
                EditorGUILayout.HelpBox("Event service not found", MessageType.Warning);
                return false;
            }

            return true;
        }
    }
}
