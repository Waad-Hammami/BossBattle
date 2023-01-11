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
	public class ActionGetTimeofDay : IAction
	{

		[VariableFilter(Variable.DataType.String)]
		public VariableProperty timeofDay = new VariableProperty(Variable.VarType.GlobalVariable);
		
		public bool seconds;
		public bool hours;

        // EXECUTABLE: ----------------------------------------------------------------------------

        public override bool InstantExecute(GameObject target, IAction[] actions, int index)
        {
	       
	        string tod = (string)DayNightManager.FormatTime(seconds,hours);
	        this.timeofDay.Set(tod);
		        
	       
            return true;
         
        }


        // +--------------------------------------------------------------------------------------+
        // | EDITOR                                                                               |
        // +--------------------------------------------------------------------------------------+

#if UNITY_EDITOR
      
		public static new string NAME = "ActionPack3/DayNight/Get Time of Day";
		private const string NODE_TITLE = "Get Time of Day";
		public const string CUSTOM_ICON_PATH = "Assets/PivecLabs/ActionPack3/Icons/";

        // PROPERTIES: ----------------------------------------------------------------------------
		private SerializedProperty sptimeofDay;
		private SerializedProperty spseconds;
		private SerializedProperty sphours;
 
        // INSPECTOR METHODS: ---------------------------------------------------------------------

        public override string GetNodeTitle()
        {

	        return string.Format(
		        NODE_TITLE
		       
	        );
        }

        protected override void OnEnableEditorChild()
        {
	        this.sptimeofDay = this.serializedObject.FindProperty("timeofDay");
	        this.spseconds = this.serializedObject.FindProperty("seconds");
	        this.sphours = this.serializedObject.FindProperty("hours");

        }

        protected override void OnDisableEditorChild()
        {
	        this.sptimeofDay = null;
	        this.spseconds = null;
	        this.sphours = null;
        }

        public override void OnInspectorGUI()
        {
	        this.serializedObject.Update();
	        EditorGUILayout.Space();
	        EditorGUILayout.LabelField("Get Time and store in Variable");

            EditorGUILayout.Space();
	        EditorGUILayout.PropertyField(this.sptimeofDay, new GUIContent("Variable"));
	
	        EditorGUILayout.Space();
	        EditorGUILayout.PropertyField(this.sphours, new GUIContent("12 hour clock"));
	        EditorGUILayout.Space();
	        
	        EditorGUILayout.PropertyField(this.spseconds, new GUIContent("Include Seconds"));

	        EditorGUILayout.Space();
	  
           this.serializedObject.ApplyModifiedProperties();
        }

#endif
    }
}
