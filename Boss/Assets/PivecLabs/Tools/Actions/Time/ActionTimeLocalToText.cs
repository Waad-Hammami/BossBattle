namespace PivecLabs.Tools
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.Events;
	using UnityEngine.EventSystems;
	using UnityEngine.AI;
	using UnityEngine.SceneManagement;
	using System.ComponentModel;
	using System.Security.Cryptography;
	using System.Text;
	using UnityEngine.UI;

	using System;	
	using System.Net;
	using System.Net.Sockets;
	using GameCreator.Core;
	using GameCreator.Core.Hooks;
	using GameCreator.Characters;
	using GameCreator.Variables;


#if UNITY_EDITOR
    using UnityEditor;
	#endif

	[AddComponentMenu("")]
	public class ActionTimeLocalToText : IAction
    {
    
         public Text text;
  

        // EXECUTABLE: ----------------------------------------------------------------------------
        public override bool InstantExecute(GameObject target, IAction[] actions, int index)
        {
	       if (this.text != null)
            {
		       
		       Double localtime = LocalTime();
		       DateTime dateTime1 = new DateTime(1900, 1, 1).AddMilliseconds(localtime);
		       
		       
		       this.text.text = dateTime1.ToString();

	           
            }


            return true;
       
        }
        
	    public static double LocalTime ()
	    {
		    return DateTime.Now.Subtract (new DateTime (1900, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
		
	    }


        // +--------------------------------------------------------------------------------------+
        // | EDITOR                                                                               |
        // +--------------------------------------------------------------------------------------+

#if UNITY_EDITOR

	    public const string CUSTOM_ICON_PATH = "Assets/PivecLabs/Tools/Icons/";
	    public static new string NAME = "Developer Tools/Time/Time Local to Text ";
	    private const string NODE_TITLE = "Time Local to Text ";

        // PROPERTIES: ----------------------------------------------------------------------------

        private SerializedProperty sptext;
   
        // INSPECTOR METHODS: ---------------------------------------------------------------------

        public override string GetNodeTitle()
        {
            return string.Format(NODE_TITLE);
        }

        protected override void OnEnableEditorChild()
        {
            this.sptext = this.serializedObject.FindProperty("text");
           }

        protected override void OnDisableEditorChild()
        {
            this.sptext = null;
         }

        public override void OnInspectorGUI()
        {
            this.serializedObject.Update();
           EditorGUILayout.Space();
             EditorGUILayout.PropertyField(this.sptext, new GUIContent("UI Text Field"));

            EditorGUILayout.Space();
    

            EditorGUILayout.Space();

            this.serializedObject.ApplyModifiedProperties();
        }

#endif
    }
}