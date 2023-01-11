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
	public class ActionChangeLightHalo : IAction
	{

		public Light LightObject;

		public bool newHaloBool = false;

		public bool newHalo;

		[VariableFilter(Variable.DataType.Bool)]
		public VariableProperty newHaloVar = new VariableProperty(Variable.VarType.GlobalVariable);

        // EXECUTABLE: ----------------------------------------------------------------------------

        public override bool InstantExecute(GameObject target, IAction[] actions, int index)
		{
        	
			var haloComponent = LightObject.GetComponent("Halo");
			
			if (haloComponent != null)
			
			{
				var haloEnabledProperty = haloComponent.GetType().GetProperty("enabled");
				
				if (newHaloBool == true)
				{    
					var newHalo = (bool)this.newHaloVar.Get(target);
					haloEnabledProperty.SetValue(haloComponent, newHalo, null);
				}
				else
				{
	        	
					haloEnabledProperty.SetValue(haloComponent, newHalo, null);
					
				}
	         
			}
	       
	        
            return true;
         
        }


        // +--------------------------------------------------------------------------------------+
        // | EDITOR                                                                               |
        // +--------------------------------------------------------------------------------------+

#if UNITY_EDITOR
      
		public static new string NAME = "ActionPack3/Lighting/Change Light Halo";
		private const string NODE_TITLE = "Change Light Halo";
		public const string CUSTOM_ICON_PATH = "Assets/PivecLabs/ActionPack3/Icons/";

        // PROPERTIES: ----------------------------------------------------------------------------
		private SerializedProperty spLightObject;
		
		private SerializedProperty spnewHaloVar;
		
		private SerializedProperty spnewHalo;

		private SerializedProperty spnewHaloBool;
  
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
	        
	        this.spnewHaloVar = this.serializedObject.FindProperty("newHaloVar");
	        
	        this.spnewHalo = this.serializedObject.FindProperty("newHalo");

	        this.spnewHaloBool = this.serializedObject.FindProperty("newHaloBool");

        }

        protected override void OnDisableEditorChild()
        {
	        this.spLightObject = null;
	        
	        this.spnewHaloVar = null;
	        
	        this.spnewHalo = null;
	        
	        this.spnewHaloBool = null;
        }

        public override void OnInspectorGUI()
        {
            this.serializedObject.Update();
	        EditorGUILayout.PropertyField(this.spLightObject, new GUIContent("Light with Halo"));
	        EditorGUILayout.Space();
	        
	        EditorGUILayout.LabelField("Halo", EditorStyles.boldLabel);
	        EditorGUI.indentLevel++;
	        EditorGUILayout.PropertyField(this.spnewHaloBool, new GUIContent("Value from Variable"));
	        
	        if (newHaloBool == true)
	        {
		        EditorGUILayout.PropertyField(this.spnewHaloVar, new GUIContent("Enable/Disable Halo"));
	        }
	        else
	        {
		        EditorGUILayout.PropertyField(this.spnewHalo, new GUIContent("Enable/Disable Halo"));
	        }
	        EditorGUILayout.Space();
	        EditorGUI.indentLevel--;
	        
	       
	        
           this.serializedObject.ApplyModifiedProperties();
        }

#endif
    }
}
