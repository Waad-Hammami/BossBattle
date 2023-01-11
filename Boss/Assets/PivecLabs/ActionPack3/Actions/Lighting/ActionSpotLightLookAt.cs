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
	public class ActionSpotLightLookAt : IAction
	{

		public Light LightObject;

		public TargetPosition lookAtPosition = new TargetPosition();


        // EXECUTABLE: ----------------------------------------------------------------------------

		public override IEnumerator Execute(GameObject target, IAction[] actions, int index)
		{

	      
			if (LightObject.type == LightType.Spot)
			
			{
				
			
				LightObject.transform.LookAt(lookAtPosition.GetPosition(target), Vector3.up);

				
				
			}
	
	        
	        
			yield return 0;
		}
        // +--------------------------------------------------------------------------------------+
        // | EDITOR                                                                               |
        // +--------------------------------------------------------------------------------------+

#if UNITY_EDITOR
      
		public static new string NAME = "ActionPack3/Lighting/SpotLight Look at Object";
		private const string NODE_TITLE = "SpotLight Look at Object";
		public const string CUSTOM_ICON_PATH = "Assets/PivecLabs/ActionPack3/Icons/";

        // PROPERTIES: ----------------------------------------------------------------------------
		private SerializedProperty spLightObject;
		
		private SerializedProperty splookAtPosition;
		


  
        // INSPECTOR METHODS: ---------------------------------------------------------------------

        public override string GetNodeTitle()
        {

	        return string.Format(
		        NODE_TITLE
		       
	        );
        }

        protected override void OnEnableEditorChild()
        {
	        this.spLightObject = this.serializedObject.FindProperty("LightObject");
	        
	        this.splookAtPosition = this.serializedObject.FindProperty("lookAtPosition");
	        
	
        }

        protected override void OnDisableEditorChild()
        {
	        this.spLightObject = null;
	        
	        this.splookAtPosition = null;
	        
	      }

        public override void OnInspectorGUI()
        {
            this.serializedObject.Update();
	        EditorGUILayout.Space();
	        EditorGUILayout.PropertyField(this.spLightObject, new GUIContent("Spot Light"));
	        EditorGUILayout.Space();
	        
	        EditorGUILayout.LabelField("Object to Look at", EditorStyles.boldLabel);
	      
	        EditorGUILayout.PropertyField(this.splookAtPosition, new GUIContent("Look at"));
	     
	  	        EditorGUILayout.Space();
	  


	        EditorGUI.indentLevel--;	        
	       
	        
           this.serializedObject.ApplyModifiedProperties();
        }

#endif
    }
}
