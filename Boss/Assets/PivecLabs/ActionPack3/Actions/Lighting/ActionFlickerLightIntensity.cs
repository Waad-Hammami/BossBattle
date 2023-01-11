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
	using PivecLabs.MinMaxSliderFloatAP3;

#if UNITY_EDITOR
    using UnityEditor;
	#endif

	[AddComponentMenu("")]
	public class ActionFlickerLightIntensity : IAction
	{

		public Light LightObject;

		public bool newIntensityBool = false;
		public bool RandomizeBool = false;

		[MinMaxFloat (0,10)]
		public MinMaxfloat intensityRange;

		[MinMaxFloat (0,1)]
		public MinMaxfloat timeRange;


		public bool newTimeBool = false;

		[Range (0,300)]
		public int newTime;

		[VariableFilter(Variable.DataType.Number)]
		public VariableProperty newTimeVar = new VariableProperty(Variable.VarType.GlobalVariable);


        // EXECUTABLE: ----------------------------------------------------------------------------

		public override IEnumerator Execute(GameObject target, IAction[] actions, int index)

		{
				
			if (newTimeBool == true)
			{    
				newTime = (int)this.newTimeVar.Get(target);
		    
			}
	
	      
				
				while (newTime > 0){
					LightObject.intensity = intensityRange.min;
					
					yield return new WaitForSeconds(Random.Range(timeRange.min, timeRange.max));
    				
					LightObject.intensity = intensityRange.max;
					
					yield return new WaitForSeconds(Random.Range(timeRange.min, timeRange.max));
					  
    				newTime--; 
				}
		
				LightObject.intensity = intensityRange.max;
				yield return 0;
        }



		
		
        // +--------------------------------------------------------------------------------------+
        // | EDITOR                                                                               |
        // +--------------------------------------------------------------------------------------+

#if UNITY_EDITOR
      
		public static new string NAME = "ActionPack3/Lighting/Flicker Light Intensity";
		private const string NODE_TITLE = "Flicker Light Intensity";
		public const string CUSTOM_ICON_PATH = "Assets/PivecLabs/ActionPack3/Icons/";

        // PROPERTIES: ----------------------------------------------------------------------------
		private SerializedProperty spLightObject;
		
		
		private SerializedProperty spnewTimeVar;
		private SerializedProperty spnewTime;
		private SerializedProperty spnewTimeBool;
		private SerializedProperty spintensityRange;
		private SerializedProperty sptimeRange;

		
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
	        
	        
	        this.spnewTimeVar = this.serializedObject.FindProperty("newTimeVar");
	        
	        this.spnewTime = this.serializedObject.FindProperty("newTime");
	        
	        this.spnewTimeBool = this.serializedObject.FindProperty("newTimeBool");

	        this.spintensityRange = this.serializedObject.FindProperty("intensityRange");

	        this.sptimeRange = this.serializedObject.FindProperty("timeRange");
        }

        protected override void OnDisableEditorChild()
        {
	        this.spLightObject = null;
	        
	        
	        
	        this.spnewTimeVar = null;
	        
	        this.spnewTime = null;
	        
	        this.spnewTimeBool = null;
	        
	        this.spintensityRange = null;
 	        
	        this.sptimeRange = null;
        }

        public override void OnInspectorGUI()
        {
            this.serializedObject.Update();
	        EditorGUILayout.PropertyField(this.spLightObject, new GUIContent("Light Object"));
	        EditorGUILayout.Space();
	        EditorGUI.indentLevel++;
	        
	        EditorGUILayout.PropertyField(this.spintensityRange, new GUIContent("Min/Max intensity"));
	        EditorGUILayout.PropertyField(this.sptimeRange, new GUIContent("Min/Max Flicker"));
    
	        {

		        EditorGUILayout.PropertyField(this.spnewTimeBool, new GUIContent("Value from Variable"));
	        
		        if (newTimeBool == true)
		        {
			        EditorGUILayout.PropertyField(this.spnewTimeVar, new GUIContent("Flicker Time"));
		        }
		        else
		        {
			        EditorGUILayout.PropertyField(this.spnewTime, new GUIContent("Flicker Time"));
		        }
	        
	        }


	        EditorGUI.indentLevel--;

           this.serializedObject.ApplyModifiedProperties();
        }

#endif
    }
}
