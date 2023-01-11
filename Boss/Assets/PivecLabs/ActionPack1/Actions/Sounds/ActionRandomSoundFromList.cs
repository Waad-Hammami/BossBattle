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
	public class ActionRandomSoundFromList : IAction
    {

        [System.Serializable]
	    public class SoundObject
        {
	        public AudioClip audioClip;
	        [Space] public float fadeIn;
	        [Range(0f, 1f)] public float volume = 1.0f;

          }

	    [SerializeField]
	    public List<SoundObject> ListofSounds = new List<SoundObject>();


	    public enum AudioMixerType
	    {
		    None,
		    Custom,
		    DefaultSoundMixer
	    }
    
    	
	    public AudioMixerType audioMixer = AudioMixerType.DefaultSoundMixer;
	    [Indent] public AudioMixerGroup mixerGroup;

	    private AudioClip audioclip;
	    private float fadein;
	    private float vol;




        // EXECUTABLE: ----------------------------------------------------------------------------


	    public override bool InstantExecute(GameObject target, IAction[] actions, int index)
	    {

              
	        AudioMixerGroup mixer = null;
	        switch (this.audioMixer)
	        {
	        case AudioMixerType.DefaultSoundMixer:
		        mixer = DatabaseGeneral.Load().soundAudioMixer;
		        break;

	        case AudioMixerType.Custom:
		        mixer = this.mixerGroup;
		        break;
	        }

		    
		    int randVal = Random.Range(0, ListofSounds.Capacity);
		    
		    audioclip = ListofSounds[randVal].audioClip;
		    fadein = ListofSounds[randVal].fadeIn;
		    vol = ListofSounds[randVal].volume;

		    AudioManager.Instance.PlaySound2D(audioclip, fadein, vol, mixer);

      
		    return true;
        }


   



        // +--------------------------------------------------------------------------------------+
        // | EDITOR                                                                               |
        // +--------------------------------------------------------------------------------------+

#if UNITY_EDITOR
       
	    public static new string NAME = "ActionPack2/Audio/Random Sound From List";
	    private const string NODE_TITLE = "Play Random Sound from List";
	    public const string CUSTOM_ICON_PATH = "Assets/PivecLabs/ActionPack2/Icons/";

        // PROPERTIES: ----------------------------------------------------------------------------

 
	    private SerializedProperty spAudioMixer;
	    private SerializedProperty spMixerGroup;




        // INSPECTOR METHODS: ---------------------------------------------------------------------

        public override string GetNodeTitle()
        {

            return string.Format(NODE_TITLE);
        }


        protected override void OnEnableEditorChild()
        {
  
	        this.spAudioMixer = this.serializedObject.FindProperty("audioMixer");
	        this.spMixerGroup = this.serializedObject.FindProperty("mixerGroup");


        }

        protected override void OnDisableEditorChild()
        {
          
	        this.spAudioMixer = null;
	        this.spMixerGroup = null;

         }




        public override void OnInspectorGUI()
        {



            this.serializedObject.Update();
	        EditorGUILayout.LabelField("Play Random Sound from List", EditorStyles.boldLabel);

	        EditorGUILayout.PropertyField(this.spAudioMixer);
	        if (this.spAudioMixer.enumValueIndex == (int)AudioMixerType.Custom)
	        {
		        EditorGUILayout.PropertyField(this.spMixerGroup);
	        }

 
            EditorGUILayout.Space();
	        EditorGUILayout.LabelField("Sound List");
	        SerializedProperty property = serializedObject.FindProperty("ListofSounds");
 	        ArrayGUI(property, "Sound ", true);
	        EditorGUILayout.Space();
  
  
               serializedObject.ApplyModifiedProperties();

        }



         private void ArrayGUI(SerializedProperty property, string itemType, bool visible)
            {

                 {

                    EditorGUI.indentLevel++;
                    SerializedProperty arraySizeProp = property.FindPropertyRelative("Array.size");
                    EditorGUILayout.PropertyField(arraySizeProp);
             
                for (int i = 0; i < arraySizeProp.intValue; i++)
                    {
                        EditorGUILayout.PropertyField(property.GetArrayElementAtIndex(i), new GUIContent(itemType + (i +1).ToString()), true);
                   
                    }
                    
		                 
                EditorGUI.indentLevel--;
                }
            }

      


#endif
    }
}