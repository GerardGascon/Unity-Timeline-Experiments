using UnityEngine.Playables;
using UnityEngine;

namespace TimelineExtensions {
	public class DOTweenBehaviourBase : PlayableBehaviour {
		public Vector4 endStatus;
		public AnimationCurve curve;
		public double start, end;
	}
}