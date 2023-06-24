using UnityEngine.Playables;
using UnityEngine;

namespace TimelineExtensions {
	public class DOTweenMixerBaseVector4 : DOTweenMixerBase {
		protected Vector4 InitValue;
		protected Vector4 FinalValue;

		public override void ProcessFrame(Playable playable, FrameData info, object playerData) {
			// From Track
			InitValue = new Vector4(
				InitStatus.x,
				InitStatus.y,
				InitStatus.z,
				InitStatus.w
			);

			// From Clips
			DOTweenBehaviourBase lastBehaviour = FindLastFinishedClip(playable);
			if (lastBehaviour != null) {
				InitValue = new Vector4(
					lastBehaviour.endStatus.x,
					lastBehaviour.endStatus.y,
					lastBehaviour.endStatus.z,
					lastBehaviour.endStatus.w
				);
			}

			FinalValue = new Vector4(InitValue.x, InitValue.y, InitValue.z, InitValue.w);

			// Calc Final Value
			IterateInput(playable);

			// Apply Final Value
			ApplyValue(playerData);
		}

		protected override void ProcessInput(DOTweenBehaviourBase behaviourBase, float progress) {
			base.ProcessInput(behaviourBase, progress);

			Vector4 endValue = new(
				behaviourBase.endStatus.x,
				behaviourBase.endStatus.y,
				behaviourBase.endStatus.z,
				behaviourBase.endStatus.w
			);

			FinalValue = Vector4.LerpUnclamped(InitValue, endValue, progress);
		}
	}
}