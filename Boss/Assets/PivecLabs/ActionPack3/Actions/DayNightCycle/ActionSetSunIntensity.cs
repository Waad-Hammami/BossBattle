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
	public class ActionSetSunIntensity : IAction
	{
		public bool newSunBool = false;
 
		public float newSunValue = 1.0f;

		[VariableFilter(Variable.DataType.Number)]
		public VariableProperty newSunVar = new VariableProperty(Variable.VarType.GlobalVariable);

        // EXECUTABLE: ----------------------------------------------------------------------------

        public override bool InstantExecute(GameObject target, IAction[] actions, int index)
        {
	        if (newSunBool == true)
	        {
		        DayNightManager.sunIntensity = (float)this.newSunVar.Get(target);
		        
	        }
	        else
	        {
		        DayNightManager.sunIntensity = (float)newSunValue;
		        
	        }
 			 
	        DayNightManager.sunCurveBool = true;
	        DayNightManager.colorsChanged = true;
	        
            return true;
         
        }


        // +--------------------------------------------------------------------------------------+
        // | EDITOR                                                                               |
        // +--------------------------------------------------------------------------------------+

#if UNITY_EDITOR
      
		public static new string NAME = "ActionPack3/DayNight/Set Sun Intensity";
		private const string NODE_TITLE = "Set Sun Intensity";
		public const string CUSTOM_ICON_PATH = "Assets/PivecLabs/ActionPack3/Icons/";

        // PROPERTIES: ----------------------------------------------------------------------------
		private SerializedProperty spnewSunVar;
		private SerializedProperty spnewSunVal;
		private SerializedProperty spnewSunBool;
  
        // INSPECTOR METHODS: ---------------------------------------------------------------------

        public override string GetNodeTitle()
        {

	        return string.Format(
		        NODE_TITLE
		       
	        );
        }

        protected override void OnEnableEditorChild()
        {
	        this.spnewSunVar = this.serializedObject.FindProperty("newSunVar");
	        this.spnewSunVal = this.serializedObject.FindProperty("newSunValue");
	        this.spnewSunBool = this.serializedObject.FindProperty("newSunBool");

        }

        protected override void OnDisableEditorChild()
        {
	        this.spnewSunVar = null;
	        this.spnewSunVar = null;
	        this.spnewSunBool = null;
        }

        public override void OnInspectorGUI()
        {
            this.serializedObject.Update();
            EditorGUILayout.Space();
	        EditorGUILayout.PropertyField(this.spnewSunBool, new GUIContent("Value from Variable"));
	        if (newSunBool == true)
	        {
		        EditorGUILayout.PropertyField(this.spnewSunVar, new GUIContent("New Intensity"));
	        }
	        else
	        {
		        EditorGUILayout.PropertyField(this.spnewSunVal, new GUIContent("New Intensity"));
	        }

	        EditorGUILayout.Space();
	  
           this.serializedObject.ApplyModifiedProperties();
        }

#endif
    }
}
