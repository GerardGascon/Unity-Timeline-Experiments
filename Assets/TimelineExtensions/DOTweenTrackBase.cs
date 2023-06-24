using UnityEditor;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine;

namespace TimelineExtensions {
	[TrackColor(0, 1, 0)]
	public abstract class DOTweenTrackBase : TrackAsset {
		public Vector4 initStatus;

		public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount) {
			ScriptPlayable<DOTweenMixerBase> playable = ScriptPlayable<DOTweenMixerBase>.Create(graph, inputCount);

			ProcessPlayable(playable);

			return playable;
		}

		protected virtual void ProcessPlayable(Playable playable) {
			ScriptPlayable<DOTweenMixerBase> basePlayable = (ScriptPlayable<DOTweenMixerBase>)playable;
			DOTweenMixerBase mixer = basePlayable.GetBehaviour();
			if (mixer != null) {
				mixer.InitStatus = initStatus;
			}

			foreach (TimelineClip clip in GetClips()) {
				DOTweenClipBase baseClip = clip.asset as DOTweenClipBase;
				if (baseClip == null) continue;
				
				baseClip.start = clip.start;
				baseClip.end = clip.end;

#if UNITY_EDITOR
				SerializedObject clipObject = new(baseClip);

				clipObject.Update();
				clipObject.FindProperty("start").doubleValue = clip.start;
				clipObject.FindProperty("end").doubleValue = clip.end;
				clipObject.ApplyModifiedProperties();
#endif
			}
		}

		public virtual void InitProperty(Object target, Vector4 initStatus) { }
	}

#if UNITY_EDITOR
	[CustomEditor(typeof(DOTweenTrackBase))]
	public abstract class DOTweenTrackBaseEditor : Editor {
		public abstract override void OnInspectorGUI();
	}

#endif
}