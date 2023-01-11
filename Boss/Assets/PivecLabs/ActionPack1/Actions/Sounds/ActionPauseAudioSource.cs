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
	public class ActionPauseAudioSource : IAction 
	{

		public GameObject audiosource;
		private AudioSource audioClip;


		// EXECUTABLE: ----------------------------------------------------------------------------

		public override bool InstantExecute(GameObject target, IAction[] actions, int index)
		{
			if (audiosource != null)
			{
				audioClip = audiosource.GetComponent<AudioSource>();
				audioClip.Pause();
				
			}
			

			return true;
		}


		// +--------------------------------------------------------------------------------------+
		// | EDITOR                                                                               |
		// +--------------------------------------------------------------------------------------+

		#if UNITY_EDITOR

		public static new string NAME = "ActionPack2/Audio/Pause AudioSource";
		private const string NODE_TITLE = "Pause AudioSource";
		public const string CUSTOM_ICON_PATH = "Assets/PivecLabs/ActionPack2/Icons/";


		// INSPECTOR METHODS: ---------------------------------------------------------------------

		public override string GetNodeTitle()
		{
			return string.Format(
				NODE_TITLE
				
			);
		}

		private SerializedProperty spAudioClip;

		protected override void OnEnableEditorChild()
		{
			this.spAudioClip = this.serializedObject.FindProperty("audiosource");
	}

		public override void OnInspectorGUI()
		{
			this.serializedObject.Update();

			EditorGUILayout.PropertyField(this.spAudioClip);


			this.serializedObject.ApplyModifiedProperties();
		}

#endif
	}
}