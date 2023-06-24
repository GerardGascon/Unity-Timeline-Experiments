using UnityEngine;
using UnityEngine.Playables;

namespace TimelineExtensions {
	public class DOTweenMixerBase : PlayableBehaviour {
		public Vector4 InitStatus;

		// Interface
		protected virtual void ProcessInput(DOTweenBehaviourBase behaviourBase, float progress) { }

		// Interface
		protected virtual void ApplyValue(object playerData) { }

		// Tool
		protected void IterateInput(Playable playable) {
			int inputCount = playable.GetInputCount();
			for (int i = 0; i < inputCount; i++) {
				if (playable.GetInputWeight(i) <= 0) continue;

				ScriptPlayable<DOTweenBehaviourBase> derivedPlayable = 
					(ScriptPlayable<DOTweenBehaviourBase>)playable.GetInput(i);
				DOTweenBehaviourBase behaviour = derivedPlayable.GetBehaviour();

				if (behaviour == null) continue;
				float progress = (float)(derivedPlayable.GetTime() / derivedPlayable.GetDuration());

				if (behaviour.curve != null) {
					progress = behaviour.curve.Evaluate(progress);
				}

				ProcessInput(behaviour, progress);
			}
		}

		// Tool
		protected DOTweenBehaviourBase FindLastFinishedClip(Playable playable) {
			DOTweenBehaviourBase result = null;

			int inputCount = playable.GetInputCount();
			double maxEnd = 0.0;

			// Update Init Value
			for (int i = 0; i < inputCount; i++) {
				ScriptPlayable<DOTweenBehaviourBase> derivedPlayable = 
					(ScriptPlayable<DOTweenBehaviourBase>)playable.GetInput(i);
				DOTweenBehaviourBase behaviour = derivedPlayable.GetBehaviour();

				// Clip is Finished
				if (behaviour == null || playable.GetTime() + 1e-2 < behaviour.end) 
					continue;
				
				// Find Last Clip
				if (!(behaviour.end > maxEnd)) continue;
				maxEnd = behaviour.end;
				result = behaviour;
			}

			return result;
		}
	}
}