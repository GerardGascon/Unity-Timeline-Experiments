using UnityEditor;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine;

namespace TimelineExtensions {
	[TrackColor(0, 1, 0)]
	public abstract class DOTweenTrackBase<T> : TrackAsset where T : class {
		public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount) {
			T binding = go.GetComponent<PlayableDirector>().GetGenericBinding(this) as T;
 
			foreach (TimelineClip c in GetClips())
				((DOTweenClipBase<T>)c.asset).Target = binding;
			
			ScriptPlayable<DOTweenMixerBase> playable =
				ScriptPlayable<DOTweenMixerBase>.Create(graph, inputCount);
			
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