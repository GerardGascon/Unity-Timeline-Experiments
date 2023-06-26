using UnityEngine.Playables;
using UnityEngine;
using DG.Tweening;
using UnityEditor;
using UnityEngine.Timeline;

namespace TimelineExtensions {
	public abstract class DOTweenClipBase<T> : PlayableAsset, ITimelineClipAsset {
		
		public ClipCaps clipCaps => ClipCaps.None;

		public Vector4 endStatus;
		public DOTweenBehaviourBase.PlaybackMode playbackMode;
		public double start, end;

		public T Target { set; get; }

		public override Playable CreatePlayable(PlayableGraph graph, GameObject owner) {
			ScriptPlayable<DOTweenBehaviourBase> playable = ScriptPlayable<DOTweenBehaviourBase>.Create(graph);

			ProcessPlayable(playable);

			return playable;
		}

		void ProcessPlayable(Playable playable) {
			ScriptPlayable<DOTweenBehaviourBase> basePlayable = (ScriptPlayable<DOTweenBehaviourBase>)playable;
			DOTweenBehaviourBase behaviour = basePlayable.GetBehaviour();

			if (behaviour == null) return;
			behaviour.tween = CreateTween(Target);
		}
		protected abstract Tween CreateTween(T target);
	}

#if UNITY_EDITOR
	public class DOTweenClipBaseEditor<T> : Editor {

		T editorResult;
		
		public override void OnInspectorGUI() {
			serializedObject.Update();

			SerializedProperty uniformEndValue = serializedObject.FindProperty("endStatus");
			SerializedProperty x = uniformEndValue.FindPropertyRelative("x");
			SerializedProperty y = uniformEndValue.FindPropertyRelative("y");
			SerializedProperty z = uniformEndValue.FindPropertyRelative("z");
			SerializedProperty w = uniformEndValue.FindPropertyRelative("w");

			SerializedProperty playback = serializedObject.FindProperty("playbackMode");

			const string dataField = "End Anchored Position";
			
			Vector4 result = editorResult switch {
				float => new Vector4(EditorGUILayout.FloatField(dataField, x.floatValue), 0, 0, 0),
				Vector2 => EditorGUILayout.Vector2Field(dataField,
					new Vector4(x.floatValue, y.floatValue, z.floatValue, w.floatValue)),
				Vector3 => EditorGUILayout.Vector3Field(dataField,
					new Vector4(x.floatValue, y.floatValue, z.floatValue, w.floatValue)),
				Vector4 => EditorGUILayout.Vector4Field(dataField,
					new Vector4(x.floatValue, y.floatValue, z.floatValue, w.floatValue)),
				_ => new Vector4()
			};

			x.floatValue = result.x;
			y.floatValue = result.y;
			z.floatValue = result.z;
			w.floatValue = result.w;

			EditorGUILayout.PropertyField(playback);

			serializedObject.ApplyModifiedProperties();
		}
	}
#endif
}