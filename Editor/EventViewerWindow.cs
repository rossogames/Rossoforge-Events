using RossoForge.Events.Service;
using RossoForge.Services.Locator;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace RossoForge.Events.Editor
{
    public class EventViewerWindow : EditorWindow
    {
        private Vector2 _scrollPos;
        private string _searchValue = "";
        private HashSet<Type> _expandedTypes = new();

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

            DrawSearch();
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

        private void DrawSearch()
        {
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField("Search Event: ", GUILayout.Width(100));
            _searchValue = EditorGUILayout.TextField(_searchValue);

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        }

        private void DrawGridHeader()
        {
            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
            EditorGUILayout.LabelField("Event", GUILayout.Width(200));
            EditorGUILayout.LabelField("|Listeners", GUILayout.Width(100));
            EditorGUILayout.LabelField("|Calls", GUILayout.Width(100));
            EditorGUILayout.LabelField("|Invoke");
            EditorGUILayout.EndHorizontal();
        }

        private void DrawGrid()
        {
            var busesInfo = GetAllBuses();
            foreach (var info in busesInfo)
            {
                if (!string.IsNullOrWhiteSpace(_searchValue) && !info.EventType.Name.ContainsInvariantCultureIgnoreCase(_searchValue))
                    continue;

                DrawGridRow(info);
            }
        }

        private void DrawGridRow(BusEditorInfo info)
        {
            EditorGUILayout.BeginHorizontal();

            bool expanded = _expandedTypes.Contains(info.EventType);
            if (GUILayout.Toggle(expanded, "", "foldout", GUILayout.Width(15)))
                _expandedTypes.Add(info.EventType);
            else
                _expandedTypes.Remove(info.EventType);

            EditorGUILayout.LabelField(info.EventType.Name, GUILayout.Width(185));
            EditorGUILayout.LabelField(info.ListenerCount.ToString(), GUILayout.Width(100));
            EditorGUILayout.LabelField(info.Calls.ToString(), GUILayout.Width(100));
            if (GUILayout.Button("Invoke", GUILayout.Width(80)))
            {
                info.EventBus.Raise(info.EventInstance);
            }

            DrawObjectFields(info.EventType, info.EventInstance);

            EditorGUILayout.EndHorizontal();

            if (expanded)
                DrawBus(info);
        }

        private void DrawBus(BusEditorInfo info)
        {
            EditorGUILayout.BeginVertical("box");

            foreach (var listener in info.ListenersType)
            {
                EditorGUILayout.LabelField("→ " + listener.Name);
            }

            EditorGUILayout.EndVertical();
        }

        private BusEditorInfo[] GetAllBuses()
        {
            var eventService = ServiceLocator.Get<IEventService>();
            var eventBuses = eventService.GetAllBuses();
            var eventBusesinfo = new BusEditorInfo[eventBuses.Length];

            for (int i = 0; i < eventBusesinfo.Length; i++)
            {
                var bus = eventBuses[i];
                eventBusesinfo[i] = bus.GetBusEditorInfo();
            }

            return eventBusesinfo;
        }

        void DrawObjectFields(Type type, object instance)
        {
            if (instance == null)
            {
                EditorGUILayout.LabelField("Null object");
                return;
            }

            var fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance);
            foreach (var field in fields)
            {
                Type fieldType = field.FieldType;
                object fieldValue = field.GetValue(instance);

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(field.Name, GUILayout.Width(150));

                object newValue = DrawFieldForType(fieldType, fieldValue);
                if (newValue != fieldValue)
                {
                    field.SetValue(instance, newValue);
                }

                EditorGUILayout.EndHorizontal();
            }
        }

        object DrawFieldForType(Type type, object value)
        {
            if (type == typeof(int))
                return EditorGUILayout.IntField((int)(value ?? 0));
            if (type == typeof(float))
                return EditorGUILayout.FloatField((float)(value ?? 0f));
            if (type == typeof(double))
                return EditorGUILayout.DoubleField((double)(value ?? 0f));
            if (type == typeof(string))
                return EditorGUILayout.TextField((string)(value ?? ""));
            if (type == typeof(bool))
                return EditorGUILayout.Toggle((bool)(value ?? false));
            if (type.IsEnum)
                return EditorGUILayout.EnumPopup((Enum)(value ?? Activator.CreateInstance(type)));

            // Para clases custom, dibujar sus campos recursivamente
            if (type.IsClass)
            {
                EditorGUILayout.LabelField(type.Name);
                EditorGUI.indentLevel++;
                DrawObjectFields(type, value);
                EditorGUI.indentLevel--;
                return value;
            }

            // Tipo no soportado
            EditorGUILayout.LabelField($"No drawer for {type.Name}");
            return value;
        }
    }
}
