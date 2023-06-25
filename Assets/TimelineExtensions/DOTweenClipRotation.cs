using UnityEngine.Playables;
using UnityEngine;
using DG.Tweening;
using UnityEditor;

namespace TimelineExtensions {
	public sealed class DOTweenClipRotation : DOTweenClipBase<Transform> {
		protected override Tween CreateTween(Transform target) {
			if (target == null) return null;

			return target.DORotate(new Vector3(endStatus.x, endStatus.y, endStatus.z), (float)(end - start), RotateMode.WorldAxisAdd);
		}
	}

#if UNITY_EDITOR

	[CustomEditor(typeof(DOTweenClipRotation))]
	public class DOTweenClipRotationEditor : DOTweenClipBaseEditor<Vector3> { }

#endif
}