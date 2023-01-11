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
	public class ActionReSetWeather : IAction
	{
	

        // EXECUTABLE: ----------------------------------------------------------------------------

        public override bool InstantExecute(GameObject target, IAction[] actions, int index)
        {
	        if (HookCamera.Instance != null)
	        {

	        
			          GameObject go = GameObject.Find ("weathereffect");
	      
			          if (go){
				          Destroy (go.gameObject);
			          }

			      
	        }
	        
            return true;
         
        }


        // +--------------------------------------------------------------------------------------+
        // | EDITOR                                                                               |
        // +--------------------------------------------------------------------------------------+

#if UNITY_EDITOR
      
		public static new string NAME = "ActionPack3/DayNight/Weather Effects Reset";
		private const string NODE_TITLE = "Weather Effects Reset";
		public const string CUSTOM_ICON_PATH = "Assets/PivecLabs/ActionPack3/Icons/";

        // PROPERTIES: ----------------------------------------------------------------------------

 
        public override string GetNodeTitle()
        {

	        return string.Format(
		        NODE_TITLE
		       
	        );
        }

        protected override void OnEnableEditorChild()
        {

        }

        protected override void OnDisableEditorChild()
        {
        }

        public override void OnInspectorGUI()
        {
            this.serializedObject.Update();
	        EditorGUILayout.Space();
            
	        EditorGUILayout.LabelField("Remove Weather Effects");        
            
	        EditorGUILayout.Space();
	  
           this.serializedObject.ApplyModifiedProperties();
        }

#endif
    }
}
