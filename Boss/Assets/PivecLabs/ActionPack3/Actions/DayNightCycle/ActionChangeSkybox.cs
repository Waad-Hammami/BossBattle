namespace PivecLabs.ActionPack
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.Events;
	using GameCreator.Core;
    using GameCreator.Variables;

#if UNITY_EDITOR
    using UnityEditor;
	#endif
  
    [AddComponentMenu("")]
	public class ActionChangeSkybox : IAction
	{

        // PROPERTIES: ----------------------------------------------------------------------------



		public Material newSkyBox;
		
		// EXECUTABLE: ----------------------------------------------------------------------------
        
		public override bool InstantExecute(GameObject target, IAction[] actions, int index)
		{
		
			RenderSettings.skybox = newSkyBox;
			
			return true;
		}


		

        // +--------------------------------------------------------------------------------------+
        // | EDITOR                                                                               |
        // +--------------------------------------------------------------------------------------+

#if UNITY_EDITOR

		public static new string NAME = "ActionPack3/DayNight/Change Skybox";
		private const string NODE_TITLE = "Change Skybox";
		public const string CUSTOM_ICON_PATH = "Assets/PivecLabs/ActionPack3/Icons/";

		// PROPERTIES: ----------------------------------------------------------------------------
		private SerializedProperty spnewSkyBox;

	
  
  // INSPECTOR METHODS: ---------------------------------------------------------------------

        public override string GetNodeTitle()
		{
			return string.Format(NODE_TITLE);
		}

		protected override void OnEnableEditorChild ()
		{
			this.spnewSkyBox = this.serializedObject.FindProperty("newSkyBox");


        }

        protected override void OnDisableEditorChild ()
		{
			this.spnewSkyBox = null;
			    
		}

        public override void OnInspectorGUI()
		{
			this.serializedObject.Update();
			EditorGUILayout.Space();
			EditorGUILayout.PropertyField(this.spnewSkyBox, new GUIContent("New Sky Box"));
			EditorGUILayout.LabelField(" (Non-Procedural)");


              EditorGUILayout.Space();
            this.serializedObject.ApplyModifiedProperties();
		}

		#endif
	}
}
