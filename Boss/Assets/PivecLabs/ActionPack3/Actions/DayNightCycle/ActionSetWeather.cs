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
	public class ActionSetWeather : IAction
	{
		public bool newWeatherBool = false;
 
		public GameObject newWeatherValue;

		[VariableFilter(Variable.DataType.GameObject)]
		public VariableProperty newWeatherVar = new VariableProperty(Variable.VarType.GlobalVariable);

        // EXECUTABLE: ----------------------------------------------------------------------------

        public override bool InstantExecute(GameObject target, IAction[] actions, int index)
        {
	        if (HookCamera.Instance != null)
	        {

	         if (newWeatherBool == true)
	        {
		         Camera cam = HookCamera.Instance.Get<Camera>();
		         var effect = (GameObject)this.newWeatherVar.Get(target);
		         var go = Instantiate(effect) as GameObject;
		         go.transform.parent = cam.transform;
		         go.transform.position = cam.transform.position;
		         go.name = "weathereffect";
	    
	        }
		          else
	        {
			          Camera cam = HookCamera.Instance.Get<Camera>();
			          var go = Instantiate(newWeatherValue) as GameObject;
			          go.transform.parent = cam.transform;
			          go.transform.position = cam.transform.position;
			          go.name = "weathereffect";
	        
		        
 			 }
        }
	        
            return true;
         
        }


        // +--------------------------------------------------------------------------------------+
        // | EDITOR                                                                               |
        // +--------------------------------------------------------------------------------------+

#if UNITY_EDITOR
      
		public static new string NAME = "ActionPack3/DayNight/Set Weather Effects";
		private const string NODE_TITLE = "Set Weather Effects";
		public const string CUSTOM_ICON_PATH = "Assets/PivecLabs/ActionPack3/Icons/";

        // PROPERTIES: ----------------------------------------------------------------------------
		private SerializedProperty spnewWeatherVar;
		private SerializedProperty spnewWeatherVal;
		private SerializedProperty spnewWeatherBool;
  
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
	        this.spnewWeatherVar = this.serializedObject.FindProperty("newWeatherVar");
	        this.spnewWeatherVal = this.serializedObject.FindProperty("newWeatherValue");
	        this.spnewWeatherBool = this.serializedObject.FindProperty("newWeatherBool");

        }

        protected override void OnDisableEditorChild()
        {
  	        this.spnewWeatherVar = null;
	        this.spnewWeatherVar = null;
	        this.spnewWeatherBool = null;
        }

        public override void OnInspectorGUI()
        {
            this.serializedObject.Update();
            EditorGUILayout.Space();
	        EditorGUILayout.PropertyField(this.spnewWeatherBool, new GUIContent("Value from Variable"));
	        EditorGUI.indentLevel++;
	        if (newWeatherBool == true)
	        {
		        EditorGUILayout.PropertyField(this.spnewWeatherVar, new GUIContent("New Weather Effects"));
	        }
	        else
	        {
		        EditorGUILayout.PropertyField(this.spnewWeatherVal, new GUIContent("New Weather Effects"));
	        }
	        EditorGUI.indentLevel--;
	        EditorGUILayout.Space();
	  
           this.serializedObject.ApplyModifiedProperties();
        }

#endif
    }
}
