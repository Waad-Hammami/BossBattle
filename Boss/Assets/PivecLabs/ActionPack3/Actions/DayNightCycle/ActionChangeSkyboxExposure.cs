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
	public class ActionChangeSkyboxExposure : IAction
	{

        // PROPERTIES: ----------------------------------------------------------------------------

		public bool exposureBool = false;
		public NumberProperty changeExposure = new NumberProperty(1.0f);
		public NumberProperty changeSpeed = new NumberProperty(1.0f);

		public bool waitTillComplete = false;

		// EXECUTABLE: ----------------------------------------------------------------------------
        
		public override IEnumerator Execute(GameObject target, IAction[] actions, int index)
		{
		

			if (exposureBool == true)
			{
				DayNightManager.newskyboxExposureSetting = (float)changeExposure.GetValue(target);
				DayNightManager.newskyboxExposureSpeed = (float)changeSpeed.GetValue(target);
				DayNightManager.sbExposureChanged = true;	
			


			}
			else
			{
				DayNightManager.sbExposureChanged = false;						
		        
			}
			
			if (this.waitTillComplete)
			{

				yield return new WaitForSeconds((float)changeSpeed.GetValue(target));
			}

	        
			yield return 0;
		}


		

        // +--------------------------------------------------------------------------------------+
        // | EDITOR                                                                               |
        // +--------------------------------------------------------------------------------------+

#if UNITY_EDITOR

		public static new string NAME = "ActionPack3/DayNight/Change Skybox Exposure";
		private const string NODE_TITLE = "Change Skybox Exposure";
		public const string CUSTOM_ICON_PATH = "Assets/PivecLabs/ActionPack3/Icons/";

		// PROPERTIES: ----------------------------------------------------------------------------
		private SerializedProperty spnewSkyBoxExposure;
		private SerializedProperty spnewSkyBoxExposureSetting;
		private SerializedProperty spnewSkyBoxExposureSpeed;
		private SerializedProperty spWaitTillComplete;
 
	
  
  // INSPECTOR METHODS: ---------------------------------------------------------------------

		public override string GetNodeTitle()
		{
			return string.Format(NODE_TITLE);
		}

		protected override void OnEnableEditorChild ()
		{
			this.spnewSkyBoxExposure = this.serializedObject.FindProperty("exposureBool");
			this.spnewSkyBoxExposureSetting = this.serializedObject.FindProperty("changeExposure");
			this.spnewSkyBoxExposureSpeed = this.serializedObject.FindProperty("changeSpeed");
			this.spWaitTillComplete = this.serializedObject.FindProperty("waitTillComplete");

		}

		protected override void OnDisableEditorChild ()
		{
			this.spnewSkyBoxExposure = null;
			this.spnewSkyBoxExposureSetting = null;
			this.spnewSkyBoxExposureSpeed = null;
			this.spWaitTillComplete = null;
		    
		}

		public override void OnInspectorGUI()
		{
			this.serializedObject.Update();
			EditorGUILayout.Space();
			EditorGUILayout.PropertyField(this.spnewSkyBoxExposure, new GUIContent("SkyBox Exposure"));
			EditorGUILayout.Space();
			EditorGUILayout.PropertyField(this.spnewSkyBoxExposureSetting, new GUIContent("Exposure Setting"));
			EditorGUILayout.Space();
			EditorGUILayout.PropertyField(this.spnewSkyBoxExposureSpeed, new GUIContent("Change Speed"));
			EditorGUILayout.Space();
			EditorGUILayout.PropertyField(this.spWaitTillComplete);
 

			EditorGUILayout.Space();
			this.serializedObject.ApplyModifiedProperties();
		}

		#endif
	}
}
