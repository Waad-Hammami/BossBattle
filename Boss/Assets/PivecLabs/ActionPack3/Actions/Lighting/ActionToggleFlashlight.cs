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
	public class ActionToggleFlashlight : IAction 
	{
  
        // EXECUTABLE: ----------------------------------------------------------------------------

        public override bool InstantExecute(GameObject target, IAction[] actions, int index)
        {
                	
	        Light go = GameObject.Find ("AP3FlashLight").GetComponent<Light>();
		        
	        if (go != null) 
	        {
	        	if (go.enabled == true) 
		        	go.enabled = false;
		        else
			        go.enabled = true;
	        }
		        


	        return true;
        }

		// +--------------------------------------------------------------------------------------+
		// | EDITOR                                                                               |
		// +--------------------------------------------------------------------------------------+

		#if UNITY_EDITOR

		public static new string NAME = "ActionPack3/Lighting/Toggle FlashLight Active";
		private const string NODE_TITLE = "Toggle Flashlight active state";
		public const string CUSTOM_ICON_PATH = "Assets/PivecLabs/ActionPack3/Icons/";

		// INSPECTOR METHODS: ---------------------------------------------------------------------

		public override string GetNodeTitle()
		{
			return string.Format(
				NODE_TITLE
			);
		}
		
		public override void OnInspectorGUI()
		{
			this.serializedObject.Update();
			EditorGUILayout.Space();

			EditorGUILayout.LabelField("Toggle Flashlight On/Off", EditorStyles.boldLabel);
			EditorGUILayout.Space();

			this.serializedObject.ApplyModifiedProperties();
		}
		#endif
	}
}