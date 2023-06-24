using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine;

namespace TimelineExtensions
{
    [TrackBindingType(typeof(Transform))]
    [TrackClipType(typeof(DOTweenClipPosition))]
    public class DOTweenTrackPosition : DOTweenTrackBase
    {
        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            ScriptPlayable<DOTweenMixerPosition> playable = ScriptPlayable<DOTweenMixerPosition>.Create(graph, inputCount);

            ProcessPlayable(playable);

            return playable;
        }

        public override void InitProperty(Object target, Vector4 initStatus)
        {
            base.InitProperty(target, initStatus);

            Transform transform = target as Transform;
            if (transform != null)
            {
                transform.position = new Vector2(
                    initStatus.x,
                    initStatus.y
                );
            }
        }

        public override void GatherProperties(PlayableDirector director, IPropertyCollector driver)
        {
            Transform transform = director.GetGenericBinding(this) as Transform;
            if (transform != null) driver.AddFromName(transform, "m_LocalPosition");
        }
    }

#if UNITY_EDITOR

    [CustomEditor(typeof(DOTweenTrackPosition))]
    public class DOTweenTrackPositionEditor : DOTweenTrackBaseEditor
    {
        public override void OnInspectorGUI()
        {
            SerializedProperty initValue = serializedObject.FindProperty("initStatus");
            SerializedProperty uniformValue = initValue.FindPropertyRelative("uniformValue");

            GUI.color = Color.gray;
            EditorGUILayout.Vector2Field("Init Anchored Position",
                new Vector2(
                    uniformValue.FindPropertyRelative("x").floatValue,
                    uniformValue.FindPropertyRelative("y").floatValue
                )
            );
            GUI.color = Color.white;
        }
    }

#endif
}
