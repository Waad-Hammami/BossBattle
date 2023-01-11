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
	public class ActionSetSeason : IAction
	{
		public bool newSeasonBool = false;
 
		public float newSeasonValue = 1.0f;

		[VariableFilter(Variable.DataType.Number)]
		public VariableProperty newSeasonVar = new VariableProperty(Variable.VarType.GlobalVariable);

        // EXECUTABLE: ----------------------------------------------------------------------------

        public override bool InstantExecute(GameObject target, IAction[] actions, int index)
        {
	        if (newSeasonBool == true)
	        {
		        DayNightManager.newsunSeason = (int)this.newSeasonVar.Get(target);
		        
	        }
	        else
	        {
		        DayNightManager.newsunSeason = (int)newSeasonValue;
		        
	        }
 			 
	        DayNightManager.seasonChanged = true;

            return true;
         
        }


        // +--------------------------------------------------------------------------------------+
        // | EDITOR                                                                               |
        // +--------------------------------------------------------------------------------------+

#if UNITY_EDITOR
      
		public static new string NAME = "ActionPack3/DayNight/Set Season";
		private const string NODE_TITLE = "Set Season";
		public const string CUSTOM_ICON_PATH = "Assets/PivecLabs/ActionPack3/Icons/";

        // PROPERTIES: ----------------------------------------------------------------------------
		private SerializedProperty spnewSeasonVar;
		private SerializedProperty spnewSeasonVal;
		private SerializedProperty spnewSeasonBool;
  
        // INSPECTOR METHODS: ---------------------------------------------------------------------

        public override string GetNodeTitle()
        {

	        return string.Format(
		        NODE_TITLE
		       
	        );
        }

        protected override void OnEnableEditorChild()
        {
	        this.spnewSeasonVar = this.serializedObject.FindProperty("newSeasonVar");
	        this.spnewSeasonVal = this.serializedObject.FindProperty("newSeasonValue");
	        this.spnewSeasonBool = this.serializedObject.FindProperty("newSeasonBool");

        }

        protected override void OnDisableEditorChild()
        {
	        this.spnewSeasonVar = null;
	        this.spnewSeasonVar = null;
	        this.spnewSeasonBool = null;
        }

        public override void OnInspectorGUI()
        {
	        this.serializedObject.Update();
	        EditorGUILayout.Space();
	        EditorGUILayout.LabelField("Seasons: Full Summer = 1 / Full Winter = 60");

            EditorGUILayout.Space();
	        EditorGUILayout.PropertyField(this.spnewSeasonBool, new GUIContent("Value from Variable"));
	        if (newSeasonBool == true)
	        {
		        EditorGUILayout.PropertyField(this.spnewSeasonVar, new GUIContent("New Season Setting"));
	        }
	        else
	        {
		        EditorGUILayout.PropertyField(this.spnewSeasonVal, new GUIContent("New Season Setting"));
	        }

	        EditorGUILayout.Space();
	  
           this.serializedObject.ApplyModifiedProperties();
        }

#endif
    }
}
