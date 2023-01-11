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
	public class ActionGetSeasonString : IAction
	{

		[VariableFilter(Variable.DataType.String)]
		public VariableProperty Season = new VariableProperty(Variable.VarType.GlobalVariable);
		
		public string seasons;
		public bool autumn;
		
        // EXECUTABLE: ----------------------------------------------------------------------------

        public override bool InstantExecute(GameObject target, IAction[] actions, int index)
        {
	       
	        int season = DayNightManager.seasons;
	        
	        if (season > 0)
	        {
		        
		        if (season <20)
		       
			        seasons = "Summer";
			        
		        if (season >19 && season <41 && autumn)
			        seasons = "Autumn";
			      
		        if (season >19 && season <41 && !autumn)
			        seasons = "Spring";

		        if (season >40)
			        seasons = "Winter";
			      
	   
	        
		        this.Season.Set(seasons);
	        }
  
	       
		        
	       
            return true;
         
        }


        // +--------------------------------------------------------------------------------------+
        // | EDITOR                                                                               |
        // +--------------------------------------------------------------------------------------+

#if UNITY_EDITOR
      
		public static new string NAME = "ActionPack3/DayNight/Get Season String";
		private const string NODE_TITLE = "Get Season String";
		public const string CUSTOM_ICON_PATH = "Assets/PivecLabs/ActionPack3/Icons/";

        // PROPERTIES: ----------------------------------------------------------------------------
		private SerializedProperty spSeason;
		private SerializedProperty spAutumn;
  
        // INSPECTOR METHODS: ---------------------------------------------------------------------

        public override string GetNodeTitle()
        {

	        return string.Format(
		        NODE_TITLE
		       
	        );
        }

        protected override void OnEnableEditorChild()
        {
	        this.spSeason = this.serializedObject.FindProperty("Season");
	        this.spAutumn = this.serializedObject.FindProperty("autumn");

        }

        protected override void OnDisableEditorChild()
        {
	        this.spSeason = null;
	        this.spAutumn = null;
        }

        public override void OnInspectorGUI()
        {
	        this.serializedObject.Update();
	        EditorGUILayout.Space();
	        EditorGUILayout.LabelField("Get Game Season and Store in String Variable");
	 
            EditorGUILayout.Space();
	        EditorGUILayout.PropertyField(this.spSeason, new GUIContent("Variable"));
	
	        EditorGUILayout.Space();
	        EditorGUILayout.PropertyField(this.spAutumn, new GUIContent("Autumn not Spring"));

	        EditorGUILayout.Space();
	  
           this.serializedObject.ApplyModifiedProperties();
        }

#endif
    }
}
