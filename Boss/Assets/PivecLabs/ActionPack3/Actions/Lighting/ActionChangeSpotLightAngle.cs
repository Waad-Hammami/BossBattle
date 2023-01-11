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
	public class ActionChangeSpotLightAngle : IAction
	{

		public Light LightObject;

		public bool newAngleBool = false;

		[Range (0,100)]
		public float newAngle;


		[VariableFilter(Variable.DataType.Number)]
		public VariableProperty newAngleVar = new VariableProperty(Variable.VarType.GlobalVariable);

		public bool FadeBool = false;
		public bool newFadeBool = false;

		[Range (0,10)]
		public float newFade;

		[VariableFilter(Variable.DataType.Number)]
		public VariableProperty newFadeVar = new VariableProperty(Variable.VarType.GlobalVariable);
		private Easing.EaseType easing = Easing.EaseType.QuadInOut;


        // EXECUTABLE: ----------------------------------------------------------------------------

		public override IEnumerator Execute(GameObject target, IAction[] actions, int index)
		{

	      
			if (LightObject.type == LightType.Spot)
			
			{
				
				if (newAngleBool == true)
				{    
					newAngle = (float)this.newAngleVar.Get(target);
		    
				}
				
				if (FadeBool == true)
				{

					float oldAngle = LightObject.spotAngle;
					float vFadeSpeed = (float)this.newFade;
					float initTime = Time.time;
            
					if (vFadeSpeed > 0)
	        
					{
						while (Time.time - initTime < vFadeSpeed)
						{
							float t = (Time.time - initTime) / vFadeSpeed;
							float easeValue = Easing.GetEase(easing, 0.0f, 1.0f, t);

							LightObject.spotAngle = Mathf.Lerp(oldAngle, newAngle, easeValue);
					        

							yield return null;
						}
					}
				}
	    		 
				else
				{
				
					LightObject.spotAngle = newAngle;
				}
	      
				
			}
	
	        
	        
			yield return 0;
		}
        // +--------------------------------------------------------------------------------------+
        // | EDITOR                                                                               |
        // +--------------------------------------------------------------------------------------+

#if UNITY_EDITOR
      
		public static new string NAME = "ActionPack3/Lighting/Change SpotLight Angle";
		private const string NODE_TITLE = "Change SpotLight Angle";
		public const string CUSTOM_ICON_PATH = "Assets/PivecLabs/ActionPack3/Icons/";

        // PROPERTIES: ----------------------------------------------------------------------------
		private SerializedProperty spLightObject;
		
		private SerializedProperty spnewAngleVar;
		
		private SerializedProperty spnewAngle;

		private SerializedProperty spnewAngleBool;
		
		private SerializedProperty spFadeBool;
		private SerializedProperty spnewFadeVar;
		private SerializedProperty spnewFade;
		private SerializedProperty spnewFadeBool;


  
        // INSPECTOR METHODS: ---------------------------------------------------------------------
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
	        
	        this.spnewAngleVar = this.serializedObject.FindProperty("newAngleVar");
	        
	        this.spnewAngle = this.serializedObject.FindProperty("newAngle");

	        this.spnewAngleBool = this.serializedObject.FindProperty("newAngleBool");

	        this.spFadeBool = this.serializedObject.FindProperty("FadeBool");
	        
	        this.spnewFadeVar = this.serializedObject.FindProperty("newFadeVar");
	        
	        this.spnewFade = this.serializedObject.FindProperty("newFade");
	        
	        this.spnewFadeBool = this.serializedObject.FindProperty("newFadeBool");

        }

        protected override void OnDisableEditorChild()
        {
	        this.spLightObject = null;
	        
	        this.spnewAngleVar = null;
	        
	        this.spnewAngle = null;
	        
	        this.spnewAngleBool = null;
	   
	        this.spFadeBool = null;
 	        
	        this.spnewFadeVar = null;
	        
	        this.spnewFade = null;
	        
	        this.spnewFadeBool = null;
        }

        public override void OnInspectorGUI()
        {
            this.serializedObject.Update();
	        EditorGUILayout.Space();
	        EditorGUILayout.PropertyField(this.spLightObject, new GUIContent("Spot Light"));
	        EditorGUILayout.Space();
	        
	        EditorGUILayout.LabelField("Angle", EditorStyles.boldLabel);
	        EditorGUI.indentLevel++;
	        EditorGUILayout.PropertyField(this.spnewAngleBool, new GUIContent("Value from Variable"));
	        
	        if (newAngleBool == true)
	        {
		        EditorGUILayout.PropertyField(this.spnewAngleVar, new GUIContent("New Angle"));
	        }
	        else
	        {
		        EditorGUILayout.PropertyField(this.spnewAngle, new GUIContent("New Angle"));
	        }
	        EditorGUILayout.Space();
	        EditorGUILayout.PropertyField(this.spFadeBool, new GUIContent("Change over Time"));
	        
	        if (FadeBool == true)
	        {

		        EditorGUILayout.PropertyField(this.spnewFadeBool, new GUIContent("Value from Variable"));
	        
		        if (newFadeBool == true)
		        {
			        EditorGUILayout.PropertyField(this.spnewFadeVar, new GUIContent("Change Time"));
		        }
		        else
		        {
			        EditorGUILayout.PropertyField(this.spnewFade, new GUIContent("Change Time"));
		        }
	        
	        }


	        EditorGUI.indentLevel--;	        
	       
	        
           this.serializedObject.ApplyModifiedProperties();
        }

#endif
    }
}
