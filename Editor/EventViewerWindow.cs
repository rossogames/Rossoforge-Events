using RossoForge.Events.Service;
using RossoForge.Services.Locator;
using UnityEditor;
using UnityEngine;

namespace RossoForge.Events.Editor
{
    public class EventViewerWindow : EditorWindow
    {
        private Vector2 _scrollPos;

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

            _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);

            DrawGridHeader();
            DrawGrid();

            EditorGUILayout.EndScrollView();
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

        private BusInfo[] GetAllBuses()
        {
            var eventService = ServiceLocator.Get<IEventService>();
            var eventBuses = eventService.GetAllBuses();
            var eventBusesinfo = new BusInfo[eventBuses.Length];

            for (int i = 0; i < eventBusesinfo.Length; i++)
            {
                var bus = eventBuses[i];
                eventBusesinfo[i] = new BusInfo
                {
                    EventType = bus.GetType().GetGenericArguments()[0],
                    ListenersType = bus.GetListenersType(),
                    Calls = bus.Calls
                };
            }

            return eventBusesinfo;
        }

        private void DrawGridHeader()
        {
            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
            GUILayout.Label("Event", GUILayout.Width(200));
            GUILayout.Label("Listeners", GUILayout.Width(100));
            GUILayout.Label("Calls", GUILayout.Width(100));
            EditorGUILayout.EndHorizontal();
        }

        private void DrawGrid()
        {
            var busesInfo = GetAllBuses();
            foreach (var info in busesInfo)
            {
                DrawGridRow(info);
            }
        }

        private void DrawGridRow(BusInfo info)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label(info.EventType.Name, GUILayout.Width(200));
            GUILayout.Label(info.ListenerCount.ToString(), GUILayout.Width(100));
            GUILayout.Label(info.Calls.ToString(), GUILayout.Width(100));
            EditorGUILayout.EndHorizontal();
        }

        private void DrawBus(BusInfo info)
        {
            EditorGUILayout.BeginVertical("box");

            EditorGUILayout.LabelField("Event Type", info.EventType.Name, EditorStyles.boldLabel);
            EditorGUILayout.LabelField("Listeners", info.ListenerCount.ToString());

            foreach (var listener in info.ListenersType)
            {
                EditorGUILayout.LabelField("→ " + listener.Name);
            }

            EditorGUILayout.EndVertical();
        }
    }
}
