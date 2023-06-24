using UnityEngine.Playables;
using UnityEngine;

namespace TimelineExtensions {
	public class DOTweenMixerBaseVector2 : DOTweenMixerBase {
		protected Vector2 InitValue;
		protected Vector2 FinalValue;

		public override void ProcessFrame(Playable playable, FrameData info, object playerData) {
			// From Track
			InitValue = new Vector2(
				InitStatus.x,
				InitStatus.y
			);

			// From Clips
			DOTweenBehaviourBase lastBehaviour = FindLastFinishedClip(playable);
			if (lastBehaviour != null) {
				InitValue = new Vector2(
					lastBehaviour.endStatus.x,
					lastBehaviour.endStatus.y
				);
			}

			FinalValue = new Vector2(InitValue.x, InitValue.y);

			// Calc Final Value
			IterateInput(playable);

			// Apply Final Value
			ApplyValue(playerData);
		}

		protected override void ProcessInput(DOTweenBehaviourBase behaviourBase, float progress) {
			base.ProcessInput(behaviourBase, progress);

			Vector2 endValue = new(
				behaviourBase.endStatus.x,
				behaviourBase.endStatus.y
			);

			FinalValue = Vector2.LerpUnclamped(InitValue, endValue, progress);
		}
	}
}