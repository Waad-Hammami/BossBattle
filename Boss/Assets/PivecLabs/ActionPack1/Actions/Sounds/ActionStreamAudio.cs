namespace PivecLabs.ActionPack
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.Events;
	using UnityEngine.Audio;
	using UnityEngine.Networking;
	using GameCreator.Core;
	using GameCreator.Variables;

    #if UNITY_EDITOR
    using UnityEditor;
	#endif

	[AddComponentMenu("")]
	public class ActionStreamAudio : IAction 
	{
        public enum AudioMixerType
        {
            None,
            Custom,
            DefaultMusicMixer
        }
        
		public enum AUDIOTYPE
		{
			MP3,
			OGG,
			AIFF,
			WAV,
			XMA
			
		}

		public AUDIOTYPE audiotype = AUDIOTYPE.OGG;
		private AudioType audioTYPE;
		
        public AudioClip audioClip;
        public AudioMixerType audioMixer = AudioMixerType.DefaultMusicMixer;
        [Indent] public AudioMixerGroup mixerGroup;

        [Space]
        public float fadeIn = 1f;

        [Range(0f, 1f)]
		public float volume = 1f;
        
		private AudioMixerGroup mixer = null;

		public StringProperty url = new StringProperty();

		private string streamingURL;
        // EXECUTABLE: ----------------------------------------------------------------------------

        public override bool InstantExecute(GameObject target, IAction[] actions, int index)
        {
            mixer = null;
            switch (this.audioMixer)
            {
                case AudioMixerType.DefaultMusicMixer:
                    mixer = DatabaseGeneral.Load().musicAudioMixer;
                    break;

                case AudioMixerType.Custom:
                    mixer = this.mixerGroup;
                    break;
            }
            
	        switch (this.audiotype)
	        {
	        case AUDIOTYPE.MP3:
		        audioTYPE = AudioType.MPEG;
		        break;
	        case AUDIOTYPE.OGG:
		        audioTYPE = AudioType.OGGVORBIS;
		        break;
	        case AUDIOTYPE.AIFF:
		        audioTYPE = AudioType.AIFF;
		        break;
	        case AUDIOTYPE.WAV:
		        audioTYPE = AudioType.WAV;
		        break;
	        case AUDIOTYPE.XMA:
		        audioTYPE = AudioType.XMA;
		        break;
	        }
            
	        streamingURL = this.url.GetValue(target);
	        StartCoroutine(GetAudioClip());
            return true;
        }


		IEnumerator GetAudioClip() {
			using (var uwr = UnityWebRequestMultimedia.GetAudioClip(streamingURL, audioTYPE)) {
				yield return uwr.SendWebRequest();
				if (uwr.result == UnityWebRequest.Result.ConnectionError) 
					{
					Debug.LogError(uwr.error);
					yield break;
				}

				AudioClip clip = DownloadHandlerAudioClip.GetContent(uwr);
				AudioManager.Instance.PlayMusic(clip, this.fadeIn, this.volume, mixer);
			}
		}
		// +--------------------------------------------------------------------------------------+
		// | EDITOR                                                                               |
		// +--------------------------------------------------------------------------------------+

		#if UNITY_EDITOR

		public static new string NAME = "ActionPack2/Audio/Stream Audio from Web";
		private const string NODE_TITLE = "Stream Audio from Web";
		public const string CUSTOM_ICON_PATH = "Assets/PivecLabs/ActionPack2/Icons/";

		// INSPECTOR METHODS: ---------------------------------------------------------------------

		public override string GetNodeTitle()
		{
			return string.Format(
				NODE_TITLE
			);
		}

		private SerializedProperty spURL;
		private SerializedProperty spAudioType;
		private SerializedProperty spAudioMixer;
        private SerializedProperty spMixerGroup;

        private SerializedProperty spFadeIn;
        private SerializedProperty spVolume;

        protected override void OnEnableEditorChild()
        {
	        this.spURL = this.serializedObject.FindProperty("url");
	        this.spAudioType = this.serializedObject.FindProperty("audiotype");
	        this.spAudioMixer = this.serializedObject.FindProperty("audioMixer");
            this.spMixerGroup = this.serializedObject.FindProperty("mixerGroup");
            this.spFadeIn = this.serializedObject.FindProperty("fadeIn");
            this.spVolume = this.serializedObject.FindProperty("volume");
        }
        
		protected override void OnDisableEditorChild()
		{
			this.spURL = null;
			this.spAudioType = null;
			this.spAudioMixer = null;
			this.spMixerGroup = null;
			this.spFadeIn = null;
			this.spVolume = null;

		}

        public override void OnInspectorGUI()
        {
            this.serializedObject.Update();

	        EditorGUILayout.PropertyField(this.spURL, new GUIContent("WebServer URL"));
	        EditorGUILayout.Space();

	        EditorGUILayout.PropertyField(this.spAudioType, new GUIContent("Audio Type"));
	        EditorGUILayout.Space();

	        EditorGUILayout.PropertyField(this.spAudioMixer);
            if (this.spAudioMixer.enumValueIndex == (int)AudioMixerType.Custom)
            {
                EditorGUILayout.PropertyField(this.spMixerGroup);
            }

            EditorGUILayout.PropertyField(this.spFadeIn);
            EditorGUILayout.PropertyField(this.spVolume);

            this.serializedObject.ApplyModifiedProperties();
        }

#endif
    }
}