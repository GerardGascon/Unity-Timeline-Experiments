using UnityEngine.Playables;
using UnityEngine;

namespace TimelineExtensions {
	public class DOTweenMixerBaseVector3 : DOTweenMixerBase {
		protected Vector3 InitValue;
		protected Vector3 FinalValue;

		public override void ProcessFrame(Playable playable, FrameData info, object playerData) {
			// From Track
			InitValue = new Vector3(
				InitStatus.x,
				InitStatus.y,
				InitStatus.z
			);

			// From Clips
			DOTweenBehaviourBase lastBehaviour = FindLastFinishedClip(playable);
			if (lastBehaviour != null) {
				InitValue = new Vector3(
					lastBehaviour.endStatus.x,
					lastBehaviour.endStatus.y,
					lastBehaviour.endStatus.z
				);
			}

			FinalValue = new Vector3(InitValue.x, InitValue.y, InitValue.z);

			// Calc Final Value
			IterateInput(playable);

			// Apply Final Value
			ApplyValue(playerData);
		}

		protected override void ProcessInput(DOTweenBehaviourBase behaviourBase, float progress) {
			base.ProcessInput(behaviourBase, progress);

			Vector3 endValue = new(
				behaviourBase.endStatus.x,
				behaviourBase.endStatus.y,
				behaviourBase.endStatus.z
			);

			FinalValue = Vector3.LerpUnclamped(InitValue, endValue, progress);
		}
	}
}