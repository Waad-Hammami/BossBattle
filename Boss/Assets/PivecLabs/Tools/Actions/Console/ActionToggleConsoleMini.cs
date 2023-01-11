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
	public class ActionToggleConsoleMini : IAction
    {
      
	    public bool showConsole;
	    public GameObject consoleManager;


        // EXECUTABLE: ----------------------------------------------------------------------------

        public override bool InstantExecute(GameObject target, IAction[] actions, int index)
        {
	        
	        if (showConsole == true)
	        {	        	
	        	consoleManager.GetComponent<InGameConsoleMini>().showConsole = true;
	        }
	        else if (showConsole == false)
	        {
		        consoleManager.GetComponent<InGameConsoleMini>().showConsole = false;

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

	    public static new string NAME = "Developer Tools/Console/Toggle Console Mini";
	    private const string NODE_TITLE = "Toggle In-Game Console Mini";
	    public const string CUSTOM_ICON_PATH = "Assets/PivecLabs/Tools/Icons/";

        // PROPERTIES: ----------------------------------------------------------------------------

	    private SerializedProperty spconsoleManager;

	    private SerializedProperty sptoggle;


        // INSPECTOR METHODS: ---------------------------------------------------------------------

        public override string GetNodeTitle()
            {
                return string.Format(
	                NODE_TITLE
                 );
            }

        protected override void OnEnableEditorChild()
            {
       
	            this.spconsoleManager = this.serializedObject.FindProperty("consoleManager");
	            this.sptoggle = this.serializedObject.FindProperty("showConsole");
 
        }


        protected override void OnDisableEditorChild()
            {
          
	            this.spconsoleManager = null;
            	this.sptoggle = null;
 

        }

        public override void OnInspectorGUI()
            {
                this.serializedObject.Update();
 
	            EditorGUILayout.PropertyField(this.spconsoleManager, new GUIContent("Console Manager Mini"));
	            EditorGUILayout.PropertyField(this.sptoggle, new GUIContent("Toggle Console"));
	            
            this.serializedObject.ApplyModifiedProperties();
            }
	    

	 
#endif

        }
    }
