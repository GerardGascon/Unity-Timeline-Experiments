using UnityEngine.Playables;
using UnityEngine;

namespace TimelineExtensions {
	public class DOTweenMixerBaseFloat : DOTweenMixerBase {
		protected float InitValue;
		protected float FinalValue;

		public override void ProcessFrame(Playable playable, FrameData info, object playerData) {
			// From Track
			InitValue = InitStatus.x;

			// From Clips
			DOTweenBehaviourBase lastBehaviour = FindLastFinishedClip(playable);
			if (lastBehaviour != null) {
				InitValue = lastBehaviour.endStatus.x;
			}

			FinalValue = InitValue;

			// Calc Final Value
			IterateInput(playable);

			// Apply Final Value
			ApplyValue(playerData);
		}

		protected override void ProcessInput(DOTweenBehaviourBase behaviourBase, float progress) {
			base.ProcessInput(behaviourBase, progress);

			float endValue = behaviourBase.endStatus.x;

			FinalValue = Mathf.LerpUnclamped(InitValue, endValue, progress);
		}
	}
}