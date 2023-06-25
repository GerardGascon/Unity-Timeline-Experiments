using UnityEngine.Playables;
using UnityEngine;
using DG.Tweening;
using UnityEditor;

namespace TimelineExtensions {
	public sealed class DOTweenClipPosition : DOTweenClipBase<Transform> {
		protected override Tween CreateTween(Transform target) {
			if (target == null) return null;

			return target.DOMove(
				new Vector3(endStatus.x, endStatus.y),
				(float)(end - start));
		}
	}

#if UNITY_EDITOR

	[CustomEditor(typeof(DOTweenClipPosition))]
	public class DOTweenClipPositionEditor : DOTweenClipBaseEditor<Vector2> { }

#endif
}