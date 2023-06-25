using System;
using System.Net;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Timeline.Sample {
	[TrackColor(.6f, .5f, .6f)]
	[TrackBindingType(typeof(Light))]
	[TrackClipType(typeof(LightFXClipBase))]
	public class LightFXTrack : TrackAsset, ILayerable {
		public bool Additive;

		class LayerMixer : PlayableBehaviour {
			public Color defaultColor { get; set; }
			public float defaultIntensity { get; set; }

			Light m_Binding;

			public override void ProcessFrame(Playable playable, FrameData info, object playerData) {
				Color color = defaultColor;
				float intensity = defaultIntensity;

				int inputCount = playable.GetInputCount();
				for (int i = 0; i < inputCount; i++) {
					Playable input = playable.GetInput(i);
					TrackMixer trackMixer = ((ScriptPlayable<TrackMixer>)input).GetBehaviour();
					if (trackMixer.ColorWeight <= float.Epsilon && trackMixer.IntensityWeight <= 0)
						continue;

					if (trackMixer.Additive) {
						color += trackMixer.Color * trackMixer.ColorWeight;
						intensity += trackMixer.Intensity * trackMixer.IntensityWeight;
					}else {
						color = Color.Lerp(color, trackMixer.Color, trackMixer.ColorWeight);
						intensity = Mathf.Lerp(intensity, trackMixer.Intensity, trackMixer.IntensityWeight);
					}
				}

				color.a = 1;

				Light light = playerData as Light;
				if (light != null) {
					light.color = color;
					light.intensity = intensity;
				}

				m_Binding = light;
			}

			public override void OnPlayableDestroy(Playable playable) {
				if (m_Binding == null) return;
				m_Binding.color = defaultColor;
				m_Binding.intensity = defaultIntensity;
			}
		}


		class TrackMixer : PlayableBehaviour {
			public bool Additive;

			public float ColorWeight { get; private set; }
			public float IntensityWeight { get; private set; }
			public Color Color { get; private set; }
			public float Intensity { get; private set; }


			public override void ProcessFrame(Playable playable, FrameData info, object playerData) {
				ColorWeight = 0;
				IntensityWeight = 0;
				Color = Color.clear;
				Intensity = 0;

				int inputCount = playable.GetInputCount();
				for (int i = 0; i < inputCount; i++) {
					float weight = playable.GetInputWeight(i);
					if (weight <= float.Epsilon)
						continue;
					Playable input = playable.GetInput(i);
					if (input.GetPlayState() != PlayState.Playing)
						continue;

					Type type = input.GetPlayableType();
					if (!typeof(LightFXBehaviourBase).IsAssignableFrom(type))
						continue;

					ScriptPlayable<LightFXBehaviourBase> scriptPlayable = (ScriptPlayable<LightFXBehaviourBase>)input;
					LightFXBehaviourBase behaviour = scriptPlayable.GetBehaviour();

					switch (behaviour) {
						case IColorModifier colorModifier:
							ColorWeight += weight;
							Color += colorModifier.Color * weight;
							break;
						case IIntensityModifier intensityModifier:
							IntensityWeight += weight;
							Intensity += intensityModifier.Intensity * weight;
							break;
					}
				}
			}
		}


		Playable ILayerable.CreateLayerMixer(PlayableGraph graph, GameObject go, int inputCount) {
			Color defaultColor = Color.white;
			float defaultIntensity = 1.0f;

			if (go != null) {
				PlayableDirector director = go.GetComponent<PlayableDirector>();
				if (director != null) {
					Light light = director.GetGenericBinding(this) as Light;
					if (light != null) {
						defaultColor = light.color;
						defaultIntensity = light.intensity;
					}
				}
			}

			ScriptPlayable<LayerMixer> playable = ScriptPlayable<LayerMixer>.Create(graph, inputCount);
			LayerMixer behaviour = playable.GetBehaviour();
			behaviour.defaultColor = defaultColor;
			behaviour.defaultIntensity = defaultIntensity;
			return playable;
		}

		public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount) {
			ScriptPlayable<TrackMixer> playable = ScriptPlayable<TrackMixer>.Create(graph, inputCount);
			TrackMixer behaviour = playable.GetBehaviour();
			behaviour.Additive = Additive;
			return playable;
		}
	}
}