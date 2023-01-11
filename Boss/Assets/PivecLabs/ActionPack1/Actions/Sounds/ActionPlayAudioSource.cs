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
	public class ActionPlayAudioSource : IAction 
	{

		public GameObject audiosource;
		private AudioSource audioClip;

		[Range(0f, 1f)]
		public float volume = 1f;

		public Text duration;

		// EXECUTABLE: ----------------------------------------------------------------------------

		public override bool InstantExecute(GameObject target, IAction[] actions, int index)
		{
			if (audiosource != null)
			{
				audioClip = audiosource.GetComponent<AudioSource>();
				audioClip.Play(0);
				audioClip.volume = this.volume;
				
			}
	
			if (duration != null)
			{
				int min = Mathf.FloorToInt(audioClip.clip.length / 60);
				int sec = Mathf.FloorToInt(audioClip.clip.length % 60);
				duration.text = min + ":" + sec;
			}
	

			return true;
		}



		// +--------------------------------------------------------------------------------------+
		// | EDITOR                                                                               |
		// +--------------------------------------------------------------------------------------+

		#if UNITY_EDITOR

		public static new string NAME = "ActionPack2/Audio/Play AudioSource";
		private const string NODE_TITLE = "Play AudioSource";
		public const string CUSTOM_ICON_PATH = "Assets/PivecLabs/ActionPack2/Icons/";


		// INSPECTOR METHODS: ---------------------------------------------------------------------

		public override string GetNodeTitle()
		{
			return string.Format(
				NODE_TITLE
				
			);
		}

		private SerializedProperty spAudioClip;
		private SerializedProperty spVolume;
		private SerializedProperty spDuration;

		protected override void OnEnableEditorChild()
		{
			this.spAudioClip = this.serializedObject.FindProperty("audiosource");
			this.spVolume = this.serializedObject.FindProperty("volume");
			this.spDuration = this.serializedObject.FindProperty("duration");
 	}

		public override void OnInspectorGUI()
		{
			this.serializedObject.Update();

			EditorGUILayout.PropertyField(this.spAudioClip);

			EditorGUILayout.PropertyField(this.spVolume);
			
			EditorGUILayout.PropertyField(this.spDuration);
			this.serializedObject.ApplyModifiedProperties();
		}

#endif
	}
}