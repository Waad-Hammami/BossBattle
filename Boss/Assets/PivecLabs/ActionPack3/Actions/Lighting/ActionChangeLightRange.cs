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
	public class ActionChangeLightRange : IAction
	{

		public Light LightObject;

		public bool newRangeBool = false;

		[Range (0,100)]
		public float newRange;


		[VariableFilter(Variable.DataType.Number)]
		public VariableProperty newRangeVar = new VariableProperty(Variable.VarType.GlobalVariable);

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

	      
	       
	        if (newRangeBool == true)
	        {    
		        newRange = (float)this.newRangeVar.Get(target);
		    
	        }
	        
			if (FadeBool == true)
			{

				float oldRange = LightObject.range;
				float vFadeSpeed = (float)this.newFade;
				float initTime = Time.time;
            
				if (vFadeSpeed > 0)
	        
				{
					while (Time.time - initTime < vFadeSpeed)
					{
						float t = (Time.time - initTime) / vFadeSpeed;
						float easeValue = Easing.GetEase(easing, 0.0f, 1.0f, t);

						LightObject.range = Mathf.Lerp(oldRange, newRange, easeValue);
					        

						yield return null;
					}
				}
			}
	    		 
			else
			{
				
				LightObject.range = newRange;
			}
	      
	        
	        
			yield return 0;
		}
        // +--------------------------------------------------------------------------------------+
        // | EDITOR                                                                               |
        // +--------------------------------------------------------------------------------------+

#if UNITY_EDITOR
      
		public static new string NAME = "ActionPack3/Lighting/Change Light Range";
		private const string NODE_TITLE = "Change Light Range";
		public const string CUSTOM_ICON_PATH = "Assets/PivecLabs/ActionPack3/Icons/";

        // PROPERTIES: ----------------------------------------------------------------------------
		private SerializedProperty spLightObject;
		
		private SerializedProperty spnewRangeVar;
		
		private SerializedProperty spnewRange;

		private SerializedProperty spnewRangeBool;
		
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
	        
	        this.spnewRangeVar = this.serializedObject.FindProperty("newRangeVar");
	        
	        this.spnewRange = this.serializedObject.FindProperty("newRange");

	        this.spnewRangeBool = this.serializedObject.FindProperty("newRangeBool");

	        this.spFadeBool = this.serializedObject.FindProperty("FadeBool");
	        
	        this.spnewFadeVar = this.serializedObject.FindProperty("newFadeVar");
	        
	        this.spnewFade = this.serializedObject.FindProperty("newFade");
	        
	        this.spnewFadeBool = this.serializedObject.FindProperty("newFadeBool");

        }

        protected override void OnDisableEditorChild()
        {
	        this.spLightObject = null;
	        
	        this.spnewRangeVar = null;
	        
	        this.spnewRange = null;
	        
	        this.spnewRangeBool = null;
	   
	        this.spFadeBool = null;
 	        
	        this.spnewFadeVar = null;
	        
	        this.spnewFade = null;
	        
	        this.spnewFadeBool = null;
        }

        public override void OnInspectorGUI()
        {
            this.serializedObject.Update();
	        EditorGUILayout.Space();
	        EditorGUILayout.PropertyField(this.spLightObject, new GUIContent("Light"));
	        EditorGUILayout.Space();
	        
	        EditorGUILayout.LabelField("Range", EditorStyles.boldLabel);
	        EditorGUI.indentLevel++;
	        EditorGUILayout.PropertyField(this.spnewRangeBool, new GUIContent("Value from Variable"));
	        
	        if (newRangeBool == true)
	        {
		        EditorGUILayout.PropertyField(this.spnewRangeVar, new GUIContent("New Range"));
	        }
	        else
	        {
		        EditorGUILayout.PropertyField(this.spnewRange, new GUIContent("New Range"));
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
