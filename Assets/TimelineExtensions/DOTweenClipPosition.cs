using UnityEngine.Playables;
using UnityEngine;
using DG.Tweening;
using UnityEditor;

namespace TimelineExtensions {
	public class DOTweenClipPosition : DOTweenClipBase {
		public override Playable CreatePlayable(PlayableGraph graph, GameObject owner) {
			ScriptPlayable<DOTweenBehaviourPosition> playable =
				ScriptPlayable<DOTweenBehaviourPosition>.Create(graph);

			ProcessPlayable(playable);

			return playable;
		}

		public override Tween CreateTween(Object target) {
			Transform transform = target as Transform;
			if (transform == null) return null;

			return transform.DOMove(
				new Vector3(endStatus.x, endStatus.y),
				(float)(end - start));
		}
	}

#if UNITY_EDITOR

	[CustomEditor(typeof(DOTweenClipPosition))]
	public class DOTweenClipPositionEditor : DOTweenClipBaseEditor {
		public override void OnInspectorGUI() {
			base.OnInspectorGUI();

			serializedObject.Update();

			SerializedProperty uniformEndValue = serializedObject.FindProperty("endStatus");
			SerializedProperty x = uniformEndValue.FindPropertyRelative("x");
			SerializedProperty y = uniformEndValue.FindPropertyRelative("y");

			Vector2 editorResult = EditorGUILayout.Vector2Field(
				"End Anchored Position",
				new Vector2(x.floatValue, y.floatValue)
			);

			x.floatValue = editorResult.x;
			y.floatValue = editorResult.y;

			serializedObject.ApplyModifiedProperties();
		}
	}

#endif
}