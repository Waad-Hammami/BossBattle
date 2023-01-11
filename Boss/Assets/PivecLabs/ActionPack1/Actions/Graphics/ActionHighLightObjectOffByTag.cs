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
	public class ActionHighLightObjectOffByTag
	: IAction
	{

        // PROPERTIES: ----------------------------------------------------------------------------


		private GameObject[] targetObject;
		private Renderer[] renderers;

		public string tagStr = "";

        // EXECUTABLE: ----------------------------------------------------------------------------

        public override bool InstantExecute(GameObject target, IAction[] actions, int index)
        {

	        targetObject= GameObject.FindGameObjectsWithTag(tagStr);

	        if (targetObject != null)
	        {
	        	
		        
		        
		        for (int i = 0; i < targetObject.Length; i++)

	        	{
			        renderers = targetObject[i].GetComponentsInChildren<Renderer>();
			              	
			        foreach (var renderer in renderers) {

				        var materials = renderer.sharedMaterials.ToList();
	
				        materials.RemoveAll(x=>x.name=="FillObject (Instance)");
				        materials.RemoveAll(x=>x.name=="MaskObject (Instance)");
		               
				        renderer.materials = materials.ToArray();
		       
		        	
	        		}

	        	}
		  
	        		
	        	}
	        
	      

            return true;
        }
	

        // +--------------------------------------------------------------------------------------+
        // | EDITOR                                                                               |
        // +--------------------------------------------------------------------------------------+

#if UNITY_EDITOR

		public static new string NAME = "ActionPack2/Graphics/HighLight Object Off By Tag";
		private const string NODE_TITLE = "HighLight Object Off By Tag";
		public const string CUSTOM_ICON_PATH = "Assets/PivecLabs/ActionPack2/Icons/";

		// PROPERTIES: ----------------------------------------------------------------------------

		private SerializedProperty sptagStr;

	
  
  // INSPECTOR METHODS: ---------------------------------------------------------------------

        public override string GetNodeTitle()
		{
			return string.Format(NODE_TITLE);
		}

		protected override void OnEnableEditorChild ()
		{


        }

        protected override void OnDisableEditorChild ()
		{
			    
		}

        public override void OnInspectorGUI()
		{
			this.serializedObject.Update();
			EditorGUILayout.Space();
 
			tagStr = EditorGUILayout.TagField("HighLighted Object Tag", tagStr);


              EditorGUILayout.Space();
            this.serializedObject.ApplyModifiedProperties();
		}

		#endif
	}
}
