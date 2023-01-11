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
	public class ActionGetDayofWeekString : IAction
	{

		[VariableFilter(Variable.DataType.String)]
		public VariableProperty DayofWeek = new VariableProperty(Variable.VarType.GlobalVariable);
		
		public string day;
		
        // EXECUTABLE: ----------------------------------------------------------------------------

        public override bool InstantExecute(GameObject target, IAction[] actions, int index)
        {
	       
	        int dow = DayNightManager.dayofWeek;
  
	        switch (dow)
	        {
	        case 0:
		        day = "Sunday";
		        break;
	        case 1:
		        day = "Monday";
		        break;
	        case 2:
		        day = "Tuesday";
		        break;
	        case 3:
		        day = "Wednesday";
		        break;
	        case 4:
		        day = "Thursday";
		        break;
	        case 5:
		        day = "Friday";
		        break;
	        case 6:
		        day = "Saturday";
		        break;
	        }
	        
	        this.DayofWeek.Set(day);
		        
	       
            return true;
         
        }


        // +--------------------------------------------------------------------------------------+
        // | EDITOR                                                                               |
        // +--------------------------------------------------------------------------------------+

#if UNITY_EDITOR
      
		public static new string NAME = "ActionPack3/DayNight/Get Day of Week String";
		private const string NODE_TITLE = "Get Day of Week String";
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
	        EditorGUILayout.LabelField("Get Game Day and Store in String Variable");
	 
            EditorGUILayout.Space();
	        EditorGUILayout.PropertyField(this.spDayofWeek, new GUIContent("Variable"));
	

	        EditorGUILayout.Space();
	  
           this.serializedObject.ApplyModifiedProperties();
        }

#endif
    }
}
