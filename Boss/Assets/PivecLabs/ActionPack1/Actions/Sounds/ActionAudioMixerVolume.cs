namespace PivecLabs.ActionPack
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
	using UnityEngine.Events;
	using UnityEngine.Audio;
	using GameCreator.Variables;


    using GameCreator.Core;

#if UNITY_EDITOR
    using UnityEditor;
#endif



    [AddComponentMenu("")]
	public class ActionAudioMixerVolume : IAction
    {

        [System.Serializable]
	    public class SoundObject
	    {
		    public AudioMixerGroup mixerChildGroup;

	        public NumberProperty volume = new NumberProperty(1.0f);

          }

	    [SerializeField]
	    public List<SoundObject> AudioGroups = new List<SoundObject>();

	    public AudioMixerGroup mixerGroup;
	    private float vol;


        // EXECUTABLE: ----------------------------------------------------------------------------


	    public override bool InstantExecute(GameObject target, IAction[] actions, int index)
	    {
		    AudioMixerGroup mixer = null;
  
		    mixer = this.mixerGroup;
		   
		    for (int i = 0; i < AudioGroups.Capacity; i++)
		    {
			    float value = AudioGroups[i].volume.GetValue(target);

			    mixer.audioMixer.SetFloat(AudioGroups[i].mixerChildGroup.name, value);
			    
		    }



      
		    return true;
        }


   



        // +--------------------------------------------------------------------------------------+
        // | EDITOR                                                                               |
        // +--------------------------------------------------------------------------------------+

#if UNITY_EDITOR
       
	    public static new string NAME = "ActionPack2/Audio/Custom Mixer Volumes";
	    private const string NODE_TITLE = "Custom Mixer Volumes";
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
  
	        this.spAudioMixer = this.serializedObject.FindProperty("mixerGroup");


        }

        protected override void OnDisableEditorChild()
        {
          
			this.spAudioMixer = null;

         }




        public override void OnInspectorGUI()
        {



            this.serializedObject.Update();
 
	        EditorGUILayout.PropertyField(this.spAudioMixer,new GUIContent("Custom Mixer Group"));
	        
	 
            EditorGUILayout.Space();
	        EditorGUILayout.LabelField("Child Groups");
	        SerializedProperty property = serializedObject.FindProperty("AudioGroups");
	        ArrayGUI(property, "Audio Group ", true);
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