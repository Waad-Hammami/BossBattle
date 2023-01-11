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
	public class ActionChangeLightSettings : IAction
	{

		public Light LightObject;

		public bool newColorBool = false;
		public bool newIntensityBool = false;
		public bool newRangeBool = false;

		public Color newColor;
		[Range (0,10)]
		public float newIntensity;
		[Range (0,100)]
		public float newRange;

		[VariableFilter(Variable.DataType.Color)]
		public VariableProperty newColorVar = new VariableProperty(Variable.VarType.GlobalVariable);

		[VariableFilter(Variable.DataType.Number)]
		public VariableProperty newIntensityVar = new VariableProperty(Variable.VarType.GlobalVariable);

		[VariableFilter(Variable.DataType.Number)]
		public VariableProperty newRangeVar = new VariableProperty(Variable.VarType.GlobalVariable);


        // EXECUTABLE: ----------------------------------------------------------------------------

        public override bool InstantExecute(GameObject target, IAction[] actions, int index)
        {
	         if (newColorBool == true)
	        {    
		         var newcolor = (Color)this.newColorVar.Get(target);
		         LightObject.color = newcolor;
	        }
		     else
	        {
			     LightObject.color = newColor;
			 }
        
	        if (newIntensityBool == true)
	        {    
		        var newIntensity = (float)this.newIntensityVar.Get(target);
		        LightObject.intensity = newIntensity;
	        }
	        else
	        {
		        LightObject.intensity = newIntensity;
	        }
	       
	        if (newRangeBool == true)
	        {    
		        var newRange = (float)this.newRangeVar.Get(target);
		        LightObject.range = newRange;
	        }
	        else
	        {
		        LightObject.range = newRange;
	        }
	       
	  
	        
            return true;
         
        }


        // +--------------------------------------------------------------------------------------+
        // | EDITOR                                                                               |
        // +--------------------------------------------------------------------------------------+

#if UNITY_EDITOR
      
		public static new string NAME = "ActionPack3/Lighting/Change Light Settings";
		private const string NODE_TITLE = "Change Light Settings";
		public const string CUSTOM_ICON_PATH = "Assets/PivecLabs/ActionPack3/Icons/";

        // PROPERTIES: ----------------------------------------------------------------------------
		private SerializedProperty spLightObject;
		
		private SerializedProperty spnewColorVar;
		private SerializedProperty spnewIntensityVar;
		private SerializedProperty spnewRangeVar;
		
		private SerializedProperty spnewColor;
		private SerializedProperty spnewIntensity;
		private SerializedProperty spnewRange;

		private SerializedProperty spnewColorBool;
		private SerializedProperty spnewIntensityBool;
		private SerializedProperty spnewRangeBool;
  
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
	        
	        this.spnewColorVar = this.serializedObject.FindProperty("newColorVar");
	        this.spnewIntensityVar = this.serializedObject.FindProperty("newIntensityVar");
	        this.spnewRangeVar = this.serializedObject.FindProperty("newRangeVar");
	        
	        this.spnewColor = this.serializedObject.FindProperty("newColor");
	        this.spnewIntensity = this.serializedObject.FindProperty("newIntensity");
	        this.spnewRange = this.serializedObject.FindProperty("newRange");

	        this.spnewColorBool = this.serializedObject.FindProperty("newColorBool");
	        this.spnewIntensityBool = this.serializedObject.FindProperty("newIntensityBool");
	        this.spnewRangeBool = this.serializedObject.FindProperty("newRangeBool");

        }

        protected override void OnDisableEditorChild()
        {
	        this.spLightObject = null;
	        
	        this.spnewColorVar = null;
	        this.spnewIntensityVar = null;
	        this.spnewRangeVar = null;
	        
	        this.spnewColor = null;
	        this.spnewIntensity = null;
	        this.spnewRange = null;
	        
	        this.spnewColorBool = null;
	        this.spnewIntensityBool = null;
	        this.spnewRangeBool = null;
        }

        public override void OnInspectorGUI()
        {
            this.serializedObject.Update();
	        EditorGUILayout.Space();
	        EditorGUILayout.PropertyField(this.spLightObject, new GUIContent("Light"));
	        EditorGUILayout.Space();
	        EditorGUILayout.LabelField("Color", EditorStyles.boldLabel);
	        EditorGUI.indentLevel++;
	        EditorGUILayout.PropertyField(this.spnewColorBool, new GUIContent("Value from Variable"));
	        
	        if (newColorBool == true)
	        {
		        EditorGUILayout.PropertyField(this.spnewColorVar, new GUIContent("New Color"));
	        }
	        else
	        {
		        EditorGUILayout.PropertyField(this.spnewColor, new GUIContent("New Color"));
	        }
	        EditorGUI.indentLevel--;
	        
	        EditorGUILayout.LabelField("Intensity", EditorStyles.boldLabel);
	        EditorGUI.indentLevel++;
	        EditorGUILayout.PropertyField(this.spnewIntensityBool, new GUIContent("Value from Variable"));
	        
	        if (newIntensityBool == true)
	        {
		        EditorGUILayout.PropertyField(this.spnewIntensityVar, new GUIContent("New Intensity"));
	        }
	        else
	        {
		        EditorGUILayout.PropertyField(this.spnewIntensity, new GUIContent("New Intensity"));
	        }
	        EditorGUI.indentLevel--;
	        
	        EditorGUILayout.LabelField("Range", EditorStyles.boldLabel);
	        EditorGUI.indentLevel++;
	        EditorGUILayout.PropertyField(this.spnewRangeBool, new GUIContent("Value from Variable"));
	        
	        if (newRangeBool == true)
	        {
		        EditorGUILayout.PropertyField(this.spnewRangeVar, new GUIContent("New Range"));
	        }
	        else
	        {
		        EditorGUILayout.PropertyField(this.spnewRange, new GUIContent("New Range"));
	        }
	        EditorGUILayout.Space();
	        EditorGUI.indentLevel--;
	        

	        
	       
	        
           this.serializedObject.ApplyModifiedProperties();
        }

#endif
    }
}
