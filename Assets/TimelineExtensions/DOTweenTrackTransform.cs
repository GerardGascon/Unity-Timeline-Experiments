using UnityEditor;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine;

namespace TimelineExtensions {
	[TrackBindingType(typeof(Transform))]
	[TrackClipType(typeof(DOTweenClipPosition))]
	[TrackClipType(typeof(DOTweenClipRotation))]
	public class DOTweenTrackTransform : DOTweenTrackBase<Transform> {
		protected override void ProcessPlayable(Playable playable) {
			foreach (TimelineClip clip in GetClips()) {
				DOTweenClipBase<Transform> baseClip = clip.asset as DOTweenClipBase<Transform>;
				if (baseClip == null) continue;

				baseClip.start = clip.start;
				baseClip.end = clip.end;

#if UNITY_EDITOR
				SerializedObject clipObject = new(baseClip);

				clipObject.Update();
				clipObject.FindProperty("start").doubleValue = clip.start;
				clipObject.FindProperty("end").doubleValue = clip.end;
				clipObject.ApplyModifiedProperties();
#endif
			}
		}

		public override void GatherProperties(PlayableDirector director, IPropertyCollector driver) {
			Transform transform = director.GetGenericBinding(this) as Transform;
			if (transform != null) driver.AddFromComponent(transform.gameObject, transform);
		}
	}
}