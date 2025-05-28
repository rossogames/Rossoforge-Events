using Codice.CM.Common.Update.Partial;
using RossoForge.Events.Bus;
using RossoForge.Events.Service;
using RossoForge.Services.Locator;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RossoForge.Events.Editor
{
    public class EventViewerWindow: EditorWindow
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

            UnityEngine.Debug.LogWarning("TEST");

            _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);

            var eventBuses = GetAllBuses();
            //foreach (var busInfo in _eventService.GetAllBuses())
            //{
            //    DrawBus(busInfo);
            //}

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

        public BusInfo[] GetAllBuses()
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
                    ListenersType = bus.GetListenersType()
                };
            }

            return eventBusesinfo;
        }
    }

    public class BusInfo
    {
        public Type EventType;
        public Type[] ListenersType;
    }
}
