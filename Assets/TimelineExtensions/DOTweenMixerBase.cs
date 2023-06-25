using DG.Tweening;
using UnityEngine.Playables;

namespace TimelineExtensions {
	public class DOTweenMixerBase : PlayableBehaviour {
		static void ProcessInput(float progress, Tween tween) {
			tween.Goto(progress * tween.Duration());
		}
		
		public override void ProcessFrame(Playable playable, FrameData info, object playerData) {
			IterateInput(playable);
		}

		// Tool
		void IterateInput(Playable playable) {
			int inputCount = playable.GetInputCount();
			for (int i = 0; i < inputCount; i++) {
				if (playable.GetInputWeight(i) <= 0) continue;

				ScriptPlayable<DOTweenBehaviourBase> derivedPlayable = 
					(ScriptPlayable<DOTweenBehaviourBase>)playable.GetInput(i);
				DOTweenBehaviourBase behaviour = derivedPlayable.GetBehaviour();

				if (behaviour == null) continue;
				float progress = (float)(derivedPlayable.GetTime() / derivedPlayable.GetDuration());

				ProcessInput(progress, behaviour.tween);
			}
		}
	}
}