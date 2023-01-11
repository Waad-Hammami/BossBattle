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
	public class ActionSetAmbientColorNight : IAction
	{
		public bool newColourBool = false;
 
		public Color newColourValue = Color.white;

		[VariableFilter(Variable.DataType.Color)]
		public VariableProperty newColourVar = new VariableProperty(Variable.VarType.GlobalVariable);

        // EXECUTABLE: ----------------------------------------------------------------------------

        public override bool InstantExecute(GameObject target, IAction[] actions, int index)
        {
	        if (newColourBool == true)
	        {
		        DayNightManager.newAmbientNight = (Color)this.newColourVar.Get(target);
		        
	        }
	        else
	        {
		        DayNightManager.newAmbientNight = (Color)newColourValue;
		        
 			 }

	        DayNightManager.colorsChanged = true;
	        
            return true;
         
        }


        // +--------------------------------------------------------------------------------------+
        // | EDITOR                                                                               |
        // +--------------------------------------------------------------------------------------+

#if UNITY_EDITOR
      
		public static new string NAME = "ActionPack3/DayNight/Set Ambient Color Nighttime";
		private const string NODE_TITLE = "Set Ambient Color Nighttime";
		public const string CUSTOM_ICON_PATH = "Assets/PivecLabs/ActionPack3/Icons/";

        // PROPERTIES: ----------------------------------------------------------------------------
		private SerializedProperty spnewColourVar;
		private SerializedProperty spnewColourVal;
		private SerializedProperty spnewColourBool;
  
        // INSPECTOR METHODS: ---------------------------------------------------------------------

        public override string GetNodeTitle()
        {

	        return string.Format(
		        NODE_TITLE
		       
	        );
        }

        protected override void OnEnableEditorChild()
        {
	        this.spnewColourVar = this.serializedObject.FindProperty("newColourVar");
	        this.spnewColourVal = this.serializedObject.FindProperty("newColourValue");
	        this.spnewColourBool = this.serializedObject.FindProperty("newColourBool");

        }

        protected override void OnDisableEditorChild()
        {
  	        this.spnewColourVar = null;
	        this.spnewColourVar = null;
	        this.spnewColourBool = null;
        }

        public override void OnInspectorGUI()
        {
            this.serializedObject.Update();
            EditorGUILayout.Space();
	        EditorGUILayout.PropertyField(this.spnewColourBool, new GUIContent("Value from Variable"));
	        if (newColourBool == true)
	        {
		        EditorGUILayout.PropertyField(this.spnewColourVar, new GUIContent("New Colour"));
	        }
	        else
	        {
		        EditorGUILayout.PropertyField(this.spnewColourVal, new GUIContent("New Colour"));
	        }

	        EditorGUILayout.Space();
	  
           this.serializedObject.ApplyModifiedProperties();
        }

#endif
    }
}
