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
	public class ActionLoopAudioSource : IAction 
	{

		public GameObject audiosource;
		private AudioSource audioClip;
		public bool loopAudio;

		// EXECUTABLE: ----------------------------------------------------------------------------

		public override bool InstantExecute(GameObject target, IAction[] actions, int index)
		{
			if (audiosource != null)
			{
				audioClip = audiosource.GetComponent<AudioSource>();
				
				if (loopAudio)
				{
					audioClip.loop = true;
				}else
				{
					audioClip.loop = false;
				}
				
			}
			

			return true;
		}


		// +--------------------------------------------------------------------------------------+
		// | EDITOR                                                                               |
		// +--------------------------------------------------------------------------------------+

		#if UNITY_EDITOR

		public static new string NAME = "ActionPack2/Audio/Loop AudioSource";
		private const string NODE_TITLE = "Loop AudioSource";
		public const string CUSTOM_ICON_PATH = "Assets/PivecLabs/ActionPack2/Icons/";


		// INSPECTOR METHODS: ---------------------------------------------------------------------

		public override string GetNodeTitle()
		{
			return string.Format(
				NODE_TITLE
				
			);
		}

		private SerializedProperty spAudioClip;
		private SerializedProperty spAudioLoop;

		protected override void OnEnableEditorChild()
		{
			this.spAudioClip = this.serializedObject.FindProperty("audiosource");
			this.spAudioLoop = this.serializedObject.FindProperty("loopAudio");
		}

		public override void OnInspectorGUI()
		{
			this.serializedObject.Update();

			EditorGUILayout.PropertyField(this.spAudioClip);
			EditorGUILayout.PropertyField(this.spAudioLoop);


			this.serializedObject.ApplyModifiedProperties();
		}

#endif
	}
}