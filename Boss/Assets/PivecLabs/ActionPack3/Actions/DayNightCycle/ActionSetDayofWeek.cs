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
	public class ActionSetDayofWeek : IAction
	{

		

			public Actions actionToExecute;
		
			public enum DAY
			{
				Sunday,
				Monday,
				Tuesday,
				Wednesday,
				Thursday,
				Friday,
				Saturday
			
			}
			public DAY Day = DAY.Sunday;

        // EXECUTABLE: ----------------------------------------------------------------------------

        public override bool InstantExecute(GameObject target, IAction[] actions, int index)
        {
	       
	        int setDay = (int)Day;

	        DayNightManager.dayofWeek = setDay;
	        DayNightManager.setDOW = true;    
	       
            return true;
         
        }


        // +--------------------------------------------------------------------------------------+
        // | EDITOR                                                                               |
        // +--------------------------------------------------------------------------------------+

#if UNITY_EDITOR
      
		public static new string NAME = "ActionPack3/DayNight/Set Day of Week";
		private const string NODE_TITLE = "Set Day of Week";
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
	        this.spDayofWeek = this.serializedObject.FindProperty("Day");

        }

        protected override void OnDisableEditorChild()
        {
	        this.spDayofWeek = null;
       }

        public override void OnInspectorGUI()
        {
	        this.serializedObject.Update();
	        EditorGUILayout.Space();
	 
            EditorGUILayout.Space();
	        EditorGUILayout.PropertyField(this.spDayofWeek, new GUIContent("Set Day to:"));
	

	        EditorGUILayout.Space();
	  
           this.serializedObject.ApplyModifiedProperties();
        }

#endif
    }
}
