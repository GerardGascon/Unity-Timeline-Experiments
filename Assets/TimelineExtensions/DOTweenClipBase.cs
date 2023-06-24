using UnityEngine.Playables;
using UnityEngine;
using DG.Tweening;
using UnityEditor;
using UnityEngine.Serialization;
using UnityEngine.Timeline;

namespace TimelineExtensions {
	public abstract class DOTweenClipBase : PlayableAsset, ITimelineClipAsset {
		
		public ClipCaps clipCaps => ClipCaps.None;

		public Vector4 endStatus;
		public AnimationCurve curve = AnimationCurve.Linear(0, 0, 1, 1);
		public double start, end;

		public override Playable CreatePlayable(PlayableGraph graph, GameObject owner) {
			ScriptPlayable<DOTweenBehaviourBase> playable = ScriptPlayable<DOTweenBehaviourBase>.Create(graph);

			ProcessPlayable(playable);

			return playable;
		}

		protected virtual void ProcessPlayable(Playable playable) {
			ScriptPlayable<DOTweenBehaviourBase> basePlayable = (ScriptPlayable<DOTweenBehaviourBase>)playable;
			DOTweenBehaviourBase behaviour = basePlayable.GetBehaviour();

			if (behaviour == null) return;
			behaviour.endStatus = endStatus;
			behaviour.curve = curve;
			behaviour.start = start;
			behaviour.end = end;
		}

		public abstract Tween CreateTween(Object target);
	}

#if UNITY_EDITOR

	[CustomEditor(typeof(DOTweenClipBase))]
	public class DOTweenClipBaseEditor : Editor {
		public override void OnInspectorGUI() {
			serializedObject.Update();

			SerializedProperty curve = serializedObject.FindProperty("curve");
			curve.animationCurveValue = UnityEditor.EditorGUILayout.CurveField("Curve", curve.animationCurveValue);

			serializedObject.ApplyModifiedProperties();
		}
	}

#endif
}