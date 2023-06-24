using UnityEngine;

namespace TimelineExtensions {
	public class DOTweenMixerPosition : DOTweenMixerBaseVector2 {
		protected override void ApplyValue(object playerData) {
			Transform transform = playerData as Transform;
			if (transform != null) transform.position = FinalValue;
		}
	}
}