namespace PivecLabs.ActionPack
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.Events;
	using UnityEngine.UI;
	using GameCreator.Core;
	using PivecLabs.MinMaxSliderFloat;


	public class AudioVisualizer : MonoBehaviour {
	public AudioSource audioSource;
	[Space(10)] 
	public Color visualizerColour = Color.gray;
	[Space(10)] 
	
	[MinMaxFloat(0, 20)]
	public MinMaxfloat MinMaxHieght;
	
	private float minimumHeight = 0.1f;
	private float maximumHeight = 5.0f;
	[Space(10)] 
	[Range(0, 20)]
	public float updateSensitivity = 10.0f;

		AudioVisualizerObject[] visualizerObjects;

	void Start () {
		visualizerObjects = GetComponentsInChildren<AudioVisualizerObject> ();
		minimumHeight = MinMaxHieght.min;
		maximumHeight = MinMaxHieght.max;

	}
	
		void FixedUpdate () {
		 float[] spectrum = new float[256];
		audioSource.GetSpectrumData (spectrum, 0, FFTWindow.Rectangular);
		for (int i = 0; i < visualizerObjects.Length; i++) {
			Vector2 newSize = visualizerObjects [i].GetComponent<RectTransform> ().rect.size;
			newSize.y = Mathf.Clamp (Mathf.Lerp (newSize.y, minimumHeight + (spectrum [i] * (maximumHeight - minimumHeight) * 5.0f), updateSensitivity * 0.5f), minimumHeight, maximumHeight);
			visualizerObjects [i].GetComponent<RectTransform> ().sizeDelta = newSize;
			visualizerObjects [i].GetComponent<Image> ().color = visualizerColour;
			}
		}
	}


}