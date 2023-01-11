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
	public class ActionChangeLightColor : IAction
	{

		public Light LightObject;

		public bool newColorBool = false;
		public bool RandomizeBool = false;

		public Color newColor;
		[Range (0,10)]

		[VariableFilter(Variable.DataType.Color)]
		public VariableProperty newColorVar = new VariableProperty(Variable.VarType.GlobalVariable);


        // EXECUTABLE: ----------------------------------------------------------------------------

        public override bool InstantExecute(GameObject target, IAction[] actions, int index)
        {
	      
	        
	        if (RandomizeBool == true)
	        {
	        	
	        	LightObject.color = RandomColor();
	        }
 
	        else{
	        	
		        if (newColorBool == true)
		        {    
		         var newcolor = (Color)this.newColorVar.Get(target);
		         LightObject.color = newcolor;
		        }
		        else
		        {
			        LightObject.color = newColor;
		        }
	        }
	        
	        
	        return true;
        }


		public static Color RandomColor()
		{
			var hue = Random.Range(0f, 1f);
			return Color.HSVToRGB( hue, 1f, 1f);
		}
		
		
        // +--------------------------------------------------------------------------------------+
        // | EDITOR                                                                               |
        // +--------------------------------------------------------------------------------------+

#if UNITY_EDITOR
      
		public static new string NAME = "ActionPack3/Lighting/Change Light Color";
		private const string NODE_TITLE = "Change Light Color";
		public const string CUSTOM_ICON_PATH = "Assets/PivecLabs/ActionPack3/Icons/";

        // PROPERTIES: ----------------------------------------------------------------------------
		private SerializedProperty spLightObject;
		
		private SerializedProperty spnewColorVar;
		
		private SerializedProperty spnewColor;

		private SerializedProperty spnewColorBool;
		private SerializedProperty spRandomizeBool;
 
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
	        
	        this.spnewColor = this.serializedObject.FindProperty("newColor");
	        
	        this.spnewColorBool = this.serializedObject.FindProperty("newColorBool");
 
	        this.spRandomizeBool = this.serializedObject.FindProperty("RandomizeBool");
        }

        protected override void OnDisableEditorChild()
        {
	        this.spLightObject = null;
	        
	        this.spnewColorVar = null;
	        
	        this.spnewColor = null;
	        
	        this.spnewColorBool = null;
	        
	        this.spRandomizeBool = null;
       }

        public override void OnInspectorGUI()
        {
            this.serializedObject.Update();
	        EditorGUILayout.PropertyField(this.spLightObject, new GUIContent("Light"));
	        EditorGUILayout.Space();
	        EditorGUILayout.LabelField("Color", EditorStyles.boldLabel);
	        EditorGUI.indentLevel++;
	        
	        EditorGUILayout.PropertyField(this.spRandomizeBool, new GUIContent("Random Color"));

	        if (RandomizeBool == false)
	        {

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
	        
	        }
	       
	        
           this.serializedObject.ApplyModifiedProperties();
        }

#endif
    }
}
