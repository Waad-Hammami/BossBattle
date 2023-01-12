﻿namespace PivecLabs.ActionPack
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
	public class ActionLocalTimeToVarHour : IAction
    {
    
    
	    [VariableFilter(Variable.DataType.String)]
	    public VariableProperty storedString = new VariableProperty(Variable.VarType.GlobalVariable);


	    [VariableFilter(Variable.DataType.Number)]
	    public VariableProperty storedNumber = new VariableProperty(Variable.VarType.GlobalVariable);
	    

	    public enum FORMAT
	    {
		    String,
		    Number
			
	    }
	    public FORMAT varFormat = FORMAT.String;


        // EXECUTABLE: ----------------------------------------------------------------------------
        public override bool InstantExecute(GameObject target, IAction[] actions, int index)
        {

		    Double localtime = LocalTime();
		    DateTime dateTime1 = new DateTime(1900, 1, 1).AddMilliseconds(localtime);
	        switch (varFormat)
	        {
				
	        case FORMAT.String:
			
		        this.storedString.Set(dateTime1.Hour.ToString());
		        break;
	        case FORMAT.Number:
			
		        this.storedNumber.Set(dateTime1.Hour);
		        break;
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

	    public const string CUSTOM_ICON_PATH = "Assets/PivecLabs/ActionPack3/Icons/";
	    public static new string NAME = "ActionPack3/DayNight/Local Hour to Variable";
	    private const string NODE_TITLE = "Local Hour to Variable";

        // PROPERTIES: ----------------------------------------------------------------------------

        private SerializedProperty spstoredString;
	    private SerializedProperty spstoredNumber;
	    private SerializedProperty spformat;

	    // INSPECTOR METHODS: ---------------------------------------------------------------------

        public override string GetNodeTitle()
        {
            return string.Format(NODE_TITLE);
        }

        protected override void OnEnableEditorChild()
        {
	        this.spstoredString = this.serializedObject.FindProperty("storedString");
	        this.spstoredNumber= this.serializedObject.FindProperty("storedNumber");
	        this.spformat = this.serializedObject.FindProperty("varFormat");
        }

        protected override void OnDisableEditorChild()
        {
            this.spstoredString = null;
	        this.spstoredString = null;
	        this.spformat = null;
        }

        public override void OnInspectorGUI()
        {
            this.serializedObject.Update();
	        EditorGUILayout.Space();
	        EditorGUILayout.PropertyField(spformat,new GUIContent("String or Number"));
	        switch (varFormat)
	        {
				
	        case FORMAT.String:
			
		        EditorGUILayout.PropertyField(spstoredString,new GUIContent("String Variable"));
		        break;
	        case FORMAT.Number:
			
		        EditorGUILayout.PropertyField(spstoredNumber,new GUIContent("Number Variable"));
		        break;
	        }

            EditorGUILayout.Space();
    

            this.serializedObject.ApplyModifiedProperties();
        }

#endif
    }
}