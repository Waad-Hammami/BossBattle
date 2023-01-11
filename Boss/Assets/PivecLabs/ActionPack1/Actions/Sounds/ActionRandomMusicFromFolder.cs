namespace PivecLabs.ActionPack
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
	using UnityEngine.Events;
	using UnityEngine.Audio;


    using GameCreator.Core;

#if UNITY_EDITOR
    using UnityEditor;
#endif



    [AddComponentMenu("")]
	public class ActionRandomMusicFromFolder : IAction
    {

  
	    public float fadeIn;
	     
	    [Range(0f, 1f)]
	    public float volume = 1.0f;

     

	    public enum AudioMixerType
	    {
		    None,
		    Custom,
		    DefaultMusicMixer
	    }
    
    	
	    public AudioMixerType audioMixer = AudioMixerType.DefaultMusicMixer;
	    [Indent] public AudioMixerGroup mixerGroup;

	    private AudioClip audioclip;
	    private float fadein = 0.5f;
	    private float vol;

	    public string FolderName = "Music";
	    private Object[] soundClips;
	    
        // EXECUTABLE: ----------------------------------------------------------------------------


	    public override bool InstantExecute(GameObject target, IAction[] actions, int index)
	    {

		    if (soundClips == null)
		    	soundClips = Resources.LoadAll(FolderName, typeof(AudioClip));  
		   
		    AudioMixerGroup mixer = null;
	        
	        switch (this.audioMixer)
	        {
	        case AudioMixerType.DefaultMusicMixer:
		        mixer = DatabaseGeneral.Load().soundAudioMixer;
		        break;

	        case AudioMixerType.Custom:
		        mixer = this.mixerGroup;
		        break;
	        }

		    
		    
		    audioclip = (AudioClip)soundClips[Random.Range(0, soundClips.Length)];
	
		    AudioManager.Instance.PlayMusic(audioclip, fadein, 1.0f, mixer);

      
		    return true;
        }


   



        // +--------------------------------------------------------------------------------------+
        // | EDITOR                                                                               |
        // +--------------------------------------------------------------------------------------+

#if UNITY_EDITOR
       
	    public static new string NAME = "ActionPack2/Audio/Random Music From Folder";
	    private const string NODE_TITLE = "Play Random Music from Folder";
	    //    public const string CUSTOM_ICON_PATH = "Assets/PivecLabs/ActionPack2/Icons/";

        // PROPERTIES: ----------------------------------------------------------------------------

 
	    private SerializedProperty spAudioMixer;
	    private SerializedProperty spMixerGroup;
	    private SerializedProperty spFolderName;
	    private SerializedProperty spFadeIn;
	    private SerializedProperty spVolume;




        // INSPECTOR METHODS: ---------------------------------------------------------------------

        public override string GetNodeTitle()
        {

            return string.Format(NODE_TITLE);
        }


        protected override void OnEnableEditorChild()
        {
  
	        this.spAudioMixer = this.serializedObject.FindProperty("audioMixer");
	        this.spMixerGroup = this.serializedObject.FindProperty("mixerGroup");
	        this.spFolderName = this.serializedObject.FindProperty("FolderName");
	        this.spFadeIn = this.serializedObject.FindProperty("fadeIn");
	        this.spVolume = this.serializedObject.FindProperty("volume");


        }

        protected override void OnDisableEditorChild()
        {
          
	        this.spAudioMixer = null;
	        this.spMixerGroup = null;
	        this.spFolderName = null;
	        this.spFadeIn = null;
	        this.spVolume = null;

         }




        public override void OnInspectorGUI()
        {



            this.serializedObject.Update();
	        EditorGUILayout.LabelField("Play Random Music from Folder", EditorStyles.boldLabel);
	        EditorGUILayout.Space();
	        
	        EditorGUILayout.PropertyField(this.spFolderName,new GUIContent("Music Clips Folder"));
	        EditorGUILayout.Space();


	        EditorGUILayout.PropertyField(this.spAudioMixer);
	        if (this.spAudioMixer.enumValueIndex == (int)AudioMixerType.Custom)
	        {
		        EditorGUILayout.PropertyField(this.spMixerGroup);
	        }

	        EditorGUILayout.Space();

	        EditorGUILayout.PropertyField(this.spFadeIn);
	        EditorGUILayout.PropertyField(this.spVolume);

            EditorGUILayout.Space();
  
  
               serializedObject.ApplyModifiedProperties();

        }



           

      


#endif
    }
}