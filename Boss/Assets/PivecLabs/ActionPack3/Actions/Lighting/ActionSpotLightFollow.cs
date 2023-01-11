namespace PivecLabs.ActionPack
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
    using UnityEngine.UI;
	using UnityEngine.Events;
	using GameCreator.Core;
    using GameCreator.Characters;
    using GameCreator.Core.Hooks;
	using GameCreator.Variables;

#if UNITY_EDITOR
    using UnityEditor;
	#endif

	[AddComponentMenu("")]
	public class ActionSpotLightFollow : IAction
	{

		public Light LightObject;

		public bool newTimeBool = false;

		[Range (0.1f,2.0f)]
		public float newTime;

		[VariableFilter(Variable.DataType.Number)]
		public VariableProperty newTimeVar = new VariableProperty(Variable.VarType.GlobalVariable);

		public TargetPosition lookAtPosition = new TargetPosition();

		private float vSpeed;
		
		// EXECUTABLE: ----------------------------------------------------------------------------
        
        
		public override bool InstantExecute(GameObject target, IAction[] actions, int index)
		{

			if (LightObject.type == LightType.Spot)
			
			{
 				vSpeed = (float)this.newTime;

				CancelInvoke("SpotlightFollow"); 

				InvokeRepeating("SpotlightFollow", 0.0f, vSpeed); 
			}

			return true;
		}

	
		
		void SpotlightFollow()
		{
		
			LightObject.transform.LookAt(lookAtPosition.GetPosition(gameObject), Vector3.left);
			
		}
	
	
		 public void StopRepeating()
		{
			CancelInvoke("SpotlightFollow");
		}

        // +--------------------------------------------------------------------------------------+
        // | EDITOR                                                                               |
        // +--------------------------------------------------------------------------------------+

#if UNITY_EDITOR
      
		public static new string NAME = "ActionPack3/Lighting/SpotLight Follow Object";
		private const string NODE_TITLE = "SpotLight Follow Object";
		public const string CUSTOM_ICON_PATH = "Assets/PivecLabs/ActionPack3/Icons/";

        // PROPERTIES: ----------------------------------------------------------------------------
		private SerializedProperty spLightObject;
		
		private SerializedProperty splookAtPosition;
		
		private SerializedProperty spnewTimeVar;
		private SerializedProperty spnewTime;
		private SerializedProperty spnewTimeBool;


  
        // INSPECTOR METHODS: ---------------------------------------------------------------------
        // INSPECTOR METHODS: ---------------------------------------------------------------------

        public override string GetNodeTitle()
        {

	        return string.Format(
		        NODE_TITLE
		       
	        );
        }

        protected override void OnEnableEditorChild()
        {
	        this.spLightObject = this.serializedObject.FindProperty("LightObject");
	        
	        this.splookAtPosition = this.serializedObject.FindProperty("lookAtPosition");
	        	        
	        this.spnewTimeVar = this.serializedObject.FindProperty("newTimeVar");
	        
	        this.spnewTime = this.serializedObject.FindProperty("newTime");
	        
	        this.spnewTimeBool = this.serializedObject.FindProperty("newTimeBool");

        }

        protected override void OnDisableEditorChild()
        {
	        this.spLightObject = null;
	        
	        this.splookAtPosition = null;
	        
 	        
	        this.spnewTimeVar = null;
	        
	        this.spnewTime = null;
	        
	        this.spnewTimeBool = null;
        }

        public override void OnInspectorGUI()
        {
            this.serializedObject.Update();
	        EditorGUILayout.Space();
	        EditorGUILayout.PropertyField(this.spLightObject, new GUIContent("Spot Light"));
	        EditorGUILayout.Space();
	        
	        EditorGUILayout.LabelField("Object to Follow", EditorStyles.boldLabel);
	      
		        EditorGUILayout.PropertyField(this.splookAtPosition, new GUIContent("Look At"));
	     
	  	        EditorGUILayout.Space();
	        
	        EditorGUILayout.LabelField("Update Speed", EditorStyles.boldLabel);
	        EditorGUI.indentLevel++;
	        EditorGUILayout.PropertyField(this.spnewTimeBool, new GUIContent("Value from Variable"));
	        
		        if (newTimeBool == true)
		        {
			        EditorGUILayout.PropertyField(this.spnewTimeVar, new GUIContent("Update Time"));
		        }
		        else
		        {
			        EditorGUILayout.PropertyField(this.spnewTime, new GUIContent("Update Time"));
		        }
	       


	        EditorGUI.indentLevel--;	        
	       
	        
           this.serializedObject.ApplyModifiedProperties();
        }

#endif
    }
}
