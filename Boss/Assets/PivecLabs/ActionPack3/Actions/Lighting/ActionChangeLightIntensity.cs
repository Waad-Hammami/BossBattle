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
	public class ActionChangeLightIntensity : IAction
	{

		public Light LightObject;

		public bool newIntensityBool = false;
		public bool RandomizeBool = false;

		[Range (0,10)]
		public float newIntensity;

		[VariableFilter(Variable.DataType.Number)]
		public VariableProperty newIntensityVar = new VariableProperty(Variable.VarType.GlobalVariable);

		public bool FadeBool = false;
		public bool newFadeBool = false;

		[Range (0,5)]
		public float newFade;

		[VariableFilter(Variable.DataType.Number)]
		public VariableProperty newFadeVar = new VariableProperty(Variable.VarType.GlobalVariable);
		private Easing.EaseType easing = Easing.EaseType.QuadInOut;


        // EXECUTABLE: ----------------------------------------------------------------------------

		public override IEnumerator Execute(GameObject target, IAction[] actions, int index)

			{
	      
	        
	          if (RandomizeBool == true)
	        {
	        	
	        	newIntensity = RandomIntensity();
	        }
 
	        else
	        {
	        	
		        if (newIntensityBool == true)
		        {    
		         newIntensity = (float)this.newIntensityVar.Get(target);
		    
		        }
	    	
	        }
		     
			if (FadeBool == true)
				{

			        float oldIntensity = LightObject.intensity;
			        float vFadeSpeed = (float)this.newFade;
			        float initTime = Time.time;
            
			        if (vFadeSpeed > 0)
	        
			        {
				        while (Time.time - initTime < vFadeSpeed)
				        {
					        float t = (Time.time - initTime) / vFadeSpeed;
					        float easeValue = Easing.GetEase(easing, 0.0f, 1.0f, t);

					        LightObject.intensity = Mathf.Lerp(oldIntensity, newIntensity, easeValue);
					        

					              yield return null;
				        }
			        }
				}
	    		 
			else
			{
				
				LightObject.intensity = newIntensity;
			}
	      
	        
	        
				yield return 0;
        }


		public static float RandomIntensity()
		{
			var intensity = Random.Range(0f, 10f);
			return intensity;
		}
		
		
        // +--------------------------------------------------------------------------------------+
        // | EDITOR                                                                               |
        // +--------------------------------------------------------------------------------------+

#if UNITY_EDITOR
      
		public static new string NAME = "ActionPack3/Lighting/Change Light Intensity";
		private const string NODE_TITLE = "Change Light Intensity";
		public const string CUSTOM_ICON_PATH = "Assets/PivecLabs/ActionPack3/Icons/";

        // PROPERTIES: ----------------------------------------------------------------------------
		private SerializedProperty spLightObject;
		
		private SerializedProperty spnewIntensityVar;
		private SerializedProperty spnewIntensity;
		private SerializedProperty spnewIntensityBool;
		private SerializedProperty spRandomizeBool;
		
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
	        
	        this.spnewIntensityVar = this.serializedObject.FindProperty("newIntensityVar");
	        
	        this.spnewIntensity = this.serializedObject.FindProperty("newIntensity");
	        
	        this.spnewIntensityBool = this.serializedObject.FindProperty("newIntensityBool");
 
	        this.spRandomizeBool = this.serializedObject.FindProperty("RandomizeBool");
 
	        this.spFadeBool = this.serializedObject.FindProperty("FadeBool");
	        
	        this.spnewFadeVar = this.serializedObject.FindProperty("newFadeVar");
	        
	        this.spnewFade = this.serializedObject.FindProperty("newFade");
	        
	        this.spnewFadeBool = this.serializedObject.FindProperty("newFadeBool");

        }

        protected override void OnDisableEditorChild()
        {
	        this.spLightObject = null;
	        
	        this.spnewIntensityVar = null;
	        
	        this.spnewIntensity = null;
	        
	        this.spnewIntensityBool = null;
	        
	        this.spRandomizeBool = null;
	        
	        this.spFadeBool = null;
 	        
	        this.spnewFadeVar = null;
	        
	        this.spnewFade = null;
	        
	        this.spnewFadeBool = null;
        }

        public override void OnInspectorGUI()
        {
            this.serializedObject.Update();
	        EditorGUILayout.PropertyField(this.spLightObject, new GUIContent("Light"));
	        EditorGUILayout.Space();
	        EditorGUILayout.LabelField("Intensity", EditorStyles.boldLabel);
	        EditorGUI.indentLevel++;
	        
	        EditorGUILayout.PropertyField(this.spRandomizeBool, new GUIContent("Random Intensity"));

	        if (RandomizeBool == false)
	        {

	        EditorGUILayout.PropertyField(this.spnewIntensityBool, new GUIContent("Value from Variable"));
	        
	        if (newIntensityBool == true)
	        {
		        EditorGUILayout.PropertyField(this.spnewIntensityVar, new GUIContent("New Intensity"));
	        }
	        else
	        {
		        EditorGUILayout.PropertyField(this.spnewIntensity, new GUIContent("New Intensity"));
	        }
	        
	        }
	       
	        EditorGUILayout.PropertyField(this.spFadeBool, new GUIContent("Fade over Time"));
	        
	        if (FadeBool == true)
	        {

		        EditorGUILayout.PropertyField(this.spnewFadeBool, new GUIContent("Value from Variable"));
	        
		        if (newFadeBool == true)
		        {
			        EditorGUILayout.PropertyField(this.spnewFadeVar, new GUIContent("Fade Time"));
		        }
		        else
		        {
			        EditorGUILayout.PropertyField(this.spnewFade, new GUIContent("Fade Time"));
		        }
	        
	        }


	        EditorGUI.indentLevel--;

           this.serializedObject.ApplyModifiedProperties();
        }

#endif
    }
}
