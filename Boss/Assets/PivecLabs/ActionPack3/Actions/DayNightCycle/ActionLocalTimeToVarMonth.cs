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
	public class ActionLocalTimeToVarMonth: IAction
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
				
	        case FORMAT.Number:
			
		        this.storedNumber.Set(dateTime1.Month);
		        break;
	        case FORMAT.String:
	        
		        switch (dateTime1.Month.ToString())
		        {
		        case "1":
			        this.storedString.Set("January");
			        break;
		        case "2":
			        this.storedString.Set("Febuary");
			        break;
		        case "3":
			        this.storedString.Set("March");
			        break;
		        case "4":
			        this.storedString.Set("April");
			        break;
		        case "5":
			        this.storedString.Set("May");
			        break;
		        case "6":
			        this.storedString.Set("June");
			        break;
		        case "7":
			        this.storedString.Set("July");
			        break;
		        case "8":
			        this.storedString.Set("August");
			        break;
		        case "9":
			        this.storedString.Set("September");
			        break;
		        case "10":
			        this.storedString.Set("October");
			        break;
		        case "11":
			        this.storedString.Set("November");
			        break;
		        case "12":
			        this.storedString.Set("December");
			        break;
	       
		        }
	        
		      
		        break;
	        }

	        
	       	

            return true;
       
        }
        
	    public static double LocalTime ()
	    {
	
				    
		    string rounded = (DateTime.UtcNow.Subtract (new DateTime (1900, 1, 1)).TotalMilliseconds).ToString("F0");
		    double time1 = Convert.ToInt64(rounded);
		    return time1;
		   

	    }

	

        // +--------------------------------------------------------------------------------------+
        // | EDITOR                                                                               |
        // +--------------------------------------------------------------------------------------+

#if UNITY_EDITOR

	    public const string CUSTOM_ICON_PATH = "Assets/PivecLabs/ActionPack3/Icons/";
	    public static new string NAME = "ActionPack3/DayNight/Local Month to Variable";
	    private const string NODE_TITLE = "Local Month to Variable";

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