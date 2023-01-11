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
	public class ActionChangeProceduralSkybox : IAction
	{

        // PROPERTIES: ----------------------------------------------------------------------------


		public NumberProperty changeSpeed = new NumberProperty(0.5f);

		public Material newSkyBox;
		
		// EXECUTABLE: ----------------------------------------------------------------------------
        
		public override bool InstantExecute(GameObject target, IAction[] actions, int index)
		{
			
			return false;
		}


		public override IEnumerator Execute(GameObject target, IAction[] actions, int index)
		{
 
 			Material defaultSky = RenderSettings.skybox;
			Material tempSky = new Material (Shader.Find ("Skybox/Procedural"));
			RenderSettings.skybox = tempSky;
			RenderSettings.skybox.Lerp (RenderSettings.skybox, defaultSky, 1);

		   
			float vMoveSpeed = changeSpeed.GetValue(target);

	        float initTime = Time.unscaledTime;

	        while (Time.unscaledTime - initTime < vMoveSpeed)
	        {
		        float t1 = (Time.unscaledTime - initTime) / vMoveSpeed;
	
		        RenderSettings.skybox.Lerp(RenderSettings.skybox, newSkyBox,t1);
		        
		        yield return null;
		        
		       }

			
	        yield return new WaitForSeconds(0.2f);
        }
		
		

        // +--------------------------------------------------------------------------------------+
        // | EDITOR                                                                               |
        // +--------------------------------------------------------------------------------------+

#if UNITY_EDITOR

		public static new string NAME = "ActionPack3/DayNight/Change Procedural Skybox";
		private const string NODE_TITLE = "Change Procedural Skybox";
		public const string CUSTOM_ICON_PATH = "Assets/PivecLabs/ActionPack3/Icons/";

		// PROPERTIES: ----------------------------------------------------------------------------
		private SerializedProperty spChangeSpeed;
		private SerializedProperty spnewSkyBox;

	
  
  // INSPECTOR METHODS: ---------------------------------------------------------------------

        public override string GetNodeTitle()
		{
			return string.Format(NODE_TITLE);
		}

		protected override void OnEnableEditorChild ()
		{
			this.spChangeSpeed = this.serializedObject.FindProperty("changeSpeed");
			this.spnewSkyBox = this.serializedObject.FindProperty("newSkyBox");


        }

        protected override void OnDisableEditorChild ()
		{
			this.spChangeSpeed = null;
			this.spnewSkyBox = null;
			    
		}

        public override void OnInspectorGUI()
		{
			this.serializedObject.Update();
			EditorGUILayout.PropertyField(this.spChangeSpeed, new GUIContent("Change Speed"));
			EditorGUILayout.Space();
			EditorGUILayout.PropertyField(this.spnewSkyBox, new GUIContent("New Sky Box"));
			EditorGUILayout.LabelField(" (Procedural)");

              EditorGUILayout.Space();
            this.serializedObject.ApplyModifiedProperties();
		}

		#endif
	}
}
