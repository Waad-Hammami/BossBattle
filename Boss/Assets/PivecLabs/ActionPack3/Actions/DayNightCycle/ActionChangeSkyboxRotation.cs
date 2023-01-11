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
	public class ActionChangeSkyboxRotation : IAction
	{

        // PROPERTIES: ----------------------------------------------------------------------------

		public bool rotationBool = false;
		public NumberProperty changeSpeed = new NumberProperty(1.0f);


		public Material newSkyBox;
		
		// EXECUTABLE: ----------------------------------------------------------------------------
        
		public override bool InstantExecute(GameObject target, IAction[] actions, int index)
		{
		
			if (rotationBool == true)
			{
				DayNightManager.sbRotation = true;
		        
			}
			else
			{
				DayNightManager.sbRotation = false;
		        
			}
			DayNightManager.newskyboxRotateSpeed = (int)changeSpeed.GetValue(target);
			DayNightManager.sbRotationChanged = true;			
			return true;
		}


		

        // +--------------------------------------------------------------------------------------+
        // | EDITOR                                                                               |
        // +--------------------------------------------------------------------------------------+

#if UNITY_EDITOR

		public static new string NAME = "ActionPack3/DayNight/Change Skybox Rotation";
		private const string NODE_TITLE = "Change Skybox Rotation";
		public const string CUSTOM_ICON_PATH = "Assets/PivecLabs/ActionPack3/Icons/";

		// PROPERTIES: ----------------------------------------------------------------------------
		private SerializedProperty spnewSkyBoxRotation;
		private SerializedProperty spnewSkyBoxRotationSpeed;

	
  
  // INSPECTOR METHODS: ---------------------------------------------------------------------

        public override string GetNodeTitle()
		{
			return string.Format(NODE_TITLE);
		}

		protected override void OnEnableEditorChild ()
		{
			this.spnewSkyBoxRotation = this.serializedObject.FindProperty("rotationBool");
			this.spnewSkyBoxRotationSpeed = this.serializedObject.FindProperty("changeSpeed");


        }

        protected override void OnDisableEditorChild ()
		{
			this.spnewSkyBoxRotation = null;
			this.spnewSkyBoxRotationSpeed = null;
			    
		}

        public override void OnInspectorGUI()
		{
			this.serializedObject.Update();
			EditorGUILayout.Space();
			EditorGUILayout.PropertyField(this.spnewSkyBoxRotation, new GUIContent("Rotate SkyBox"));
			EditorGUILayout.Space();
			EditorGUILayout.PropertyField(this.spnewSkyBoxRotationSpeed, new GUIContent("Rotate Speed"));


              EditorGUILayout.Space();
            this.serializedObject.ApplyModifiedProperties();
		}

		#endif
	}
}
