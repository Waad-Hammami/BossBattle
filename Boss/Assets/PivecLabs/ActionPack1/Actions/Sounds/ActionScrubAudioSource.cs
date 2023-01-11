namespace PivecLabs.ActionPack
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.Events;
	using UnityEngine.UI;
	using UnityEngine.Audio;
	using GameCreator.Variables;


	using GameCreator.Core;


    #if UNITY_EDITOR
	using UnityEditor;
	using UnityEditor.UI;
	#endif

	[AddComponentMenu("")]
	public class ActionScrubAudioSource : IAction 
	{

		public GameObject audiosource;

		
		public GameObject sliderAudio;

		private AudioSource audioClip;

		public Text current;
		private bool audioset = false;

		// EXECUTABLE: ----------------------------------------------------------------------------

		public override bool InstantExecute(GameObject target, IAction[] actions, int index)
		{
			if (audiosource != null)
				audioClip = audiosource.GetComponent<AudioSource>();
			
			Slider slider = sliderAudio.GetComponent<Slider>();
			
			slider.maxValue = audioClip.clip.length;
			
			slider.onValueChanged.AddListener(delegate {ValueChangeCheck(); });
			audioset = true;
 
			return true;
		}

		public void ValueChangeCheck()
		{
			Slider slider = sliderAudio.GetComponent<Slider>();
			
			audioClip.time = slider.value;
			
			if (current != null)
			{
				int min = Mathf.FloorToInt(audioClip.time / 60);
				int sec = Mathf.FloorToInt(audioClip.time % 60);
				current.text = min + ":" + sec;
			}
	
		}
		
		void Update()
		{
			
			if (audioset == true && audioClip.isPlaying)
			{
				Slider slider = sliderAudio.GetComponent<Slider>();
				
				if (audiosource != null)
				{
					slider.value = audioClip.time;
				}
			
			}

			
		}

		// +--------------------------------------------------------------------------------------+
		// | EDITOR                                                                               |
		// +--------------------------------------------------------------------------------------+

		#if UNITY_EDITOR

		public static new string NAME = "ActionPack2/Audio/Scrub Audio with Slider";
		private const string NODE_TITLE = "Scrub Audio with Slider {0}";
		public const string CUSTOM_ICON_PATH = "Assets/PivecLabs/ActionPack2/Icons/";


		// INSPECTOR METHODS: ---------------------------------------------------------------------

		public override string GetNodeTitle()
		{
			return string.Format(
				NODE_TITLE,
				this.audioClip == null ? "none" : this.audioClip.name
				
			);
		}

		private SerializedProperty spAudioClip;
		private SerializedProperty spsliderAudio;
		private SerializedProperty spcurrent;

		protected override void OnEnableEditorChild()
		{
			this.spAudioClip = this.serializedObject.FindProperty("audiosource");
			this.spsliderAudio = this.serializedObject.FindProperty("sliderAudio");
			this.spcurrent = this.serializedObject.FindProperty("current");
		}

		public override void OnInspectorGUI()
		{
			this.serializedObject.Update();

			EditorGUILayout.PropertyField(this.spAudioClip);


			EditorGUILayout.PropertyField(this.spsliderAudio);

			EditorGUILayout.PropertyField(this.spcurrent);
			this.serializedObject.ApplyModifiedProperties();
		}

#endif
	}
}