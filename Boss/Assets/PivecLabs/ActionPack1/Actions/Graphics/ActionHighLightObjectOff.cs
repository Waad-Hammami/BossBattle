namespace PivecLabs.ActionPack
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using UnityEngine;

	using UnityEngine.Events;
    using GameCreator.Core;
    using GameCreator.Core.Hooks;
    using GameCreator.Variables;

#if UNITY_EDITOR
    using UnityEditor;
#endif

  
    [AddComponentMenu("")]
	public class ActionHighLightObjectOff : IAction
	{

        // PROPERTIES: ----------------------------------------------------------------------------


		public TargetGameObject target = new TargetGameObject();
		 private Renderer[] renderers;

   
        // EXECUTABLE: ----------------------------------------------------------------------------

        public override bool InstantExecute(GameObject target, IAction[] actions, int index)
        {


	        GameObject TargetObject = this.target.GetGameObject(target);
	        if (TargetObject != null) {
	   
	        renderers = TargetObject.GetComponentsInChildren<Renderer>();

	   	         
	        foreach (var renderer in renderers) {

		        var materials = renderer.sharedMaterials.ToList();
	
		        materials.RemoveAll(x=>x.name=="FillObject (Instance)");
		        materials.RemoveAll(x=>x.name=="MaskObject (Instance)");
		               
		        renderer.materials = materials.ToArray();
		       
	

	        }

	        }
	      

            return true;
        }
	

        // +--------------------------------------------------------------------------------------+
        // | EDITOR                                                                               |
        // +--------------------------------------------------------------------------------------+

#if UNITY_EDITOR

		public static new string NAME = "ActionPack2/Graphics/HighLight Object Off";
		private const string NODE_TITLE = "HighLight Object Off";
		public const string CUSTOM_ICON_PATH = "Assets/PivecLabs/ActionPack2/Icons/";

		// PROPERTIES: ----------------------------------------------------------------------------
		private SerializedProperty spTargetObject;

	
  
  // INSPECTOR METHODS: ---------------------------------------------------------------------

        public override string GetNodeTitle()
		{
			return string.Format(NODE_TITLE);
		}

		protected override void OnEnableEditorChild ()
		{
			this.spTargetObject = this.serializedObject.FindProperty("target");
	

        }

        protected override void OnDisableEditorChild ()
		{
			this.spTargetObject = null;
			    
		}

        public override void OnInspectorGUI()
		{
			this.serializedObject.Update();
			EditorGUILayout.PropertyField(this.spTargetObject, new GUIContent("HighLighted Object"));
			EditorGUILayout.Space();
   

              EditorGUILayout.Space();
            this.serializedObject.ApplyModifiedProperties();
		}

		#endif
	}
}
