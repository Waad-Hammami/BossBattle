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
	public class ActionGetDayofWeek : IAction
	{

		[VariableFilter(Variable.DataType.Number)]
		public VariableProperty DayofWeek = new VariableProperty(Variable.VarType.GlobalVariable);
		

        // EXECUTABLE: ----------------------------------------------------------------------------

        public override bool InstantExecute(GameObject target, IAction[] actions, int index)
        {
	       
	        int dow = DayNightManager.dayofWeek;
	        this.DayofWeek.Set(dow);
		        
	       
            return true;
         
        }


        // +--------------------------------------------------------------------------------------+
        // | EDITOR                                                                               |
        // +--------------------------------------------------------------------------------------+

#if UNITY_EDITOR
      
		public static new string NAME = "ActionPack3/DayNight/Get Day of Week";
		private const string NODE_TITLE = "Get Day of Week";
		public const string CUSTOM_ICON_PATH = "Assets/PivecLabs/ActionPack3/Icons/";

        // PROPERTIES: ----------------------------------------------------------------------------
		private SerializedProperty spDayofWeek;
  
        // INSPECTOR METHODS: ---------------------------------------------------------------------

        public override string GetNodeTitle()
        {

	        return string.Format(
		        NODE_TITLE
		       
	        );
        }

        protected override void OnEnableEditorChild()
        {
	        this.spDayofWeek = this.serializedObject.FindProperty("DayofWeek");

        }

        protected override void OnDisableEditorChild()
        {
	        this.spDayofWeek = null;
       }

        public override void OnInspectorGUI()
        {
	        this.serializedObject.Update();
	        EditorGUILayout.Space();
	        EditorGUILayout.LabelField("Get Day and Store in Variable");
	        EditorGUILayout.LabelField("Sunday is 0, Saturday is 6");

            EditorGUILayout.Space();
	        EditorGUILayout.PropertyField(this.spDayofWeek, new GUIContent("Variable"));
	

	        EditorGUILayout.Space();
	  
           this.serializedObject.ApplyModifiedProperties();
        }

#endif
    }
}
