using DG.Tweening;
using UnityEditor;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine;

namespace TimelineExtensions {
	[TrackColor(0, 1, 0)]
	public abstract class DOTweenTrackBase<T> : TrackAsset where T : class {
		protected class DOTweenMixer : PlayableBehaviour {
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
		
		public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount) {
			T binding = go.GetComponent<PlayableDirector>().GetGenericBinding(this) as T;
 
			foreach (TimelineClip c in GetClips())
				((DOTweenClipBase<T>)c.asset).Target = binding;
			
			ScriptPlayable<DOTweenMixer> playable =
				ScriptPlayable<DOTweenMixer>.Create(graph, inputCount);
			
			ProcessPlayable(playable);
 
			return playable;
		}

		protected abstract void ProcessPlayable(Playable playable);
	}

#if UNITY_EDITOR
	public abstract class DOTweenTrackBaseEditor : Editor {
		public abstract override void OnInspectorGUI();
	}

#endif
}