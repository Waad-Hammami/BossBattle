namespace PivecLabs.Tools
{
	using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
	using UnityEngine.UI;
	using System.ComponentModel;

    using System.Security.Cryptography;
    using System.Text;
    using UnityEngine.Events;
    using GameCreator.Core;
    using GameCreator.Variables;

#if UNITY_EDITOR
    using UnityEditor;
	#endif

	[AddComponentMenu("")]
    public class ActionTimeGetCurrentNTP : IAction
    {
    
   
        // EXECUTABLE: ----------------------------------------------------------------------------
        public override bool InstantExecute(GameObject target, IAction[] actions, int index)
        {
	        
	        TimeManager.pingNtpHost = true;
	 

            return true;
        }

       
        // +--------------------------------------------------------------------------------------+
        // | EDITOR                                                                               |
        // +--------------------------------------------------------------------------------------+

#if UNITY_EDITOR

	    public const string CUSTOM_ICON_PATH = "Assets/PivecLabs/Tools/Icons/";
	    public static new string NAME = "Developer Tools/Time/Get Current NTP Time now";
	    private const string NODE_TITLE = "Get Current NTP Time now";

        // PROPERTIES: ----------------------------------------------------------------------------

 
	    // INSPECTOR METHODS: ---------------------------------------------------------------------

        public override string GetNodeTitle()
        {
            return string.Format(NODE_TITLE);
        }

        protected override void OnEnableEditorChild()
        {
        }

        protected override void OnDisableEditorChild()
        {
        }

        public override void OnInspectorGUI()
        {
            this.serializedObject.Update();
	        EditorGUILayout.Space();
        		
			      EditorGUILayout.LabelField(new GUIContent("Stores in Current Time Variable"));
			      EditorGUILayout.LabelField(new GUIContent("as set in the Time Manager component"));

	

            EditorGUILayout.Space();

            this.serializedObject.ApplyModifiedProperties();
        }

#endif
    }
}