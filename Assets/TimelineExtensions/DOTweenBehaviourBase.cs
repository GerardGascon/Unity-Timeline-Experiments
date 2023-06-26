using DG.Tweening;
using UnityEngine;
using UnityEngine.Playables;

namespace TimelineExtensions {
	public class DOTweenBehaviourBase : PlayableBehaviour {
		public Tween tween;

		[System.Serializable]
		public enum PlaybackMode {
			Default,
			Looped
		}
		public PlaybackMode playbackMode;

		public override void OnBehaviourPause(Playable playable, FrameData info) {
			base.OnBehaviourPause(playable, info);
			if(info.deltaTime > 0) tween.Goto(tween.Duration());
		}
	}
}