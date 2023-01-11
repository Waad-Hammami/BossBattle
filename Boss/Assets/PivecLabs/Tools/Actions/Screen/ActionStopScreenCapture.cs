namespace PivecLabs.Tools
{
        using System.Collections;
        using System.Collections.Generic;
        using UnityEngine;
        using UnityEngine.Events;
        using UnityEngine.UI;
        using UnityEngine.Video;
        using GameCreator.Core;
        using GameCreator.Variables;
        using GameCreator.Core.Hooks;
        using GameCreator.Characters;
		using System.Linq;
		
#if UNITY_EDITOR
    using UnityEditor;
#endif



	
    [AddComponentMenu("")]
	public class ActionStopScreenCapture : IAction
    {
      

	    public Actions actions;

        // EXECUTABLE: ----------------------------------------------------------------------------

        public override bool InstantExecute(GameObject target, IAction[] actions, int index)
        {
	        var references = this.actions.GetComponents<ActionScreenCapture>();

	         foreach (var reference in references)
	         {
		         reference.StopRepeating();
	         }
	          
	     

	          return true;

        }

        public override IEnumerator Execute(GameObject target, IAction[] actions, int index)
        {



            return base.Execute(target, actions, index);

        }

	   
	  
        // +--------------------------------------------------------------------------------------+
        // | EDITOR                                                                               |
        // +--------------------------------------------------------------------------------------+

#if UNITY_EDITOR

	    public static new string NAME = "Developer Tools/Capture/StopScreenCapture";
	    private const string NODE_TITLE = "Stop Screen Capture";
	    public const string CUSTOM_ICON_PATH = "Assets/PivecLabs/Tools/Icons/";

        // PROPERTIES: ----------------------------------------------------------------------------

        
	    private SerializedProperty spaction;


        // INSPECTOR METHODS: ---------------------------------------------------------------------

        public override string GetNodeTitle()
            {
                return string.Format(
	                NODE_TITLE
                 );
            }

        protected override void OnEnableEditorChild()
            {
       
	            this.spaction = this.serializedObject.FindProperty("actions");
 
        }


        protected override void OnDisableEditorChild()
            {
          
	            this.spaction = null;
 

        }

        public override void OnInspectorGUI()
            {
                this.serializedObject.Update();
 
	            EditorGUILayout.PropertyField(this.spaction, new GUIContent("Stop Repeating Action"));
	            
            this.serializedObject.ApplyModifiedProperties();
            }
	    

	 
#endif

        }
    }
