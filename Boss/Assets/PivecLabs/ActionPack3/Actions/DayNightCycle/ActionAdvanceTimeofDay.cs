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
	public class ActionAdvanceTimeofDay : IAction
	{

		
		public NumberProperty advTOD = new NumberProperty(6.0f);

		public bool seconds;

        // EXECUTABLE: ----------------------------------------------------------------------------

        public override bool InstantExecute(GameObject target, IAction[] actions, int index)
        {
	       
	        DayNightManager.newadvTOD = (float)advTOD.GetValue(gameObject);
	        DayNightManager.advTOD = true;
    
	       
            return true;
         
        }


        // +--------------------------------------------------------------------------------------+
        // | EDITOR                                                                               |
        // +--------------------------------------------------------------------------------------+

#if UNITY_EDITOR
      
		public static new string NAME = "ActionPack3/DayNight/Advance Time of Day";
		private const string NODE_TITLE = "Advance Time of Day";
		public const string CUSTOM_ICON_PATH = "Assets/PivecLabs/ActionPack3/Icons/";

        // PROPERTIES: ----------------------------------------------------------------------------
		private SerializedProperty sptimeofDay;
  
        // INSPECTOR METHODS: ---------------------------------------------------------------------

        public override string GetNodeTitle()
        {

	        return string.Format(
		        NODE_TITLE
		       
	        );
        }

        protected override void OnEnableEditorChild()
        {
	        this.sptimeofDay = this.serializedObject.FindProperty("advTOD");

        }

        protected override void OnDisableEditorChild()
        {
	        this.sptimeofDay = null;
       }

        public override void OnInspectorGUI()
        {
	        this.serializedObject.Update();
	        EditorGUILayout.Space();
	        EditorGUILayout.LabelField("Advance Time of Day by:");

            EditorGUILayout.Space();
	        EditorGUILayout.PropertyField(this.sptimeofDay, new GUIContent("Variable"));
	

	        EditorGUILayout.Space();
	  
           this.serializedObject.ApplyModifiedProperties();
        }

#endif
    }
}
