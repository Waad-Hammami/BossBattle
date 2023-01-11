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
	public class ActionChangeColliderSize : IAction
	{

		public GameObject colliderObject;

		public bool newRadiusBool = false;

		[Range (0,100)]
		public float newRadius;


		[VariableFilter(Variable.DataType.Number)]
		public VariableProperty newRadiusVar = new VariableProperty(Variable.VarType.GlobalVariable);

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

			SphereCollider collider = colliderObject.GetComponent<SphereCollider>();
	       
			if (newRadiusBool == true)
	        {    
		        newRadius = (float)this.newRadiusVar.Get(target);
		    
	        }
	       
			if (FadeBool == true)
			{

				float oldSize = collider.radius;
				float vFadeSpeed = (float)this.newFade;
				float initTime = Time.time;
            
				if (vFadeSpeed > 0)
	        
				{
					while (Time.time - initTime < vFadeSpeed)
					{
						float t = (Time.time - initTime) / vFadeSpeed;
						float easeValue = Easing.GetEase(easing, 0.0f, 1.0f, t);

						collider.radius = Mathf.Lerp(oldSize, newRadius, easeValue);
					        

						yield return null;
					}
				}
			}
	    		 
			else
			{
				
				collider.radius = newRadius;
			}
	      
	        
	        
			yield return 0;
		}
        // +--------------------------------------------------------------------------------------+
        // | EDITOR                                                                               |
        // +--------------------------------------------------------------------------------------+

#if UNITY_EDITOR
      
		public static new string NAME = "ActionPack3/Lighting/Change Collider Size";
		private const string NODE_TITLE = "Change Collider Size";
		public const string CUSTOM_ICON_PATH = "Assets/PivecLabs/ActionPack3/Icons/";

        // PROPERTIES: ----------------------------------------------------------------------------
		private SerializedProperty spGameObject;
		
		private SerializedProperty spnewRadiusVar;
		
		private SerializedProperty spnewRadius;

		private SerializedProperty spnewRadiusBool;
		
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
	        this.spGameObject = this.serializedObject.FindProperty("colliderObject");
	        
	        this.spnewRadiusVar = this.serializedObject.FindProperty("newRadiusVar");
	        
	        this.spnewRadius = this.serializedObject.FindProperty("newRadius");

	        this.spnewRadiusBool = this.serializedObject.FindProperty("newRadiusBool");

	        this.spFadeBool = this.serializedObject.FindProperty("FadeBool");
	        
	        this.spnewFadeVar = this.serializedObject.FindProperty("newFadeVar");
	        
	        this.spnewFade = this.serializedObject.FindProperty("newFade");
	        
	        this.spnewFadeBool = this.serializedObject.FindProperty("newFadeBool");

        }

        protected override void OnDisableEditorChild()
        {
	        this.spGameObject = null;
	        
	        this.spnewRadiusVar = null;
	        
	        this.spnewRadius = null;
	        
	        this.spnewRadiusBool = null;
	   
	        this.spFadeBool = null;
 	        
	        this.spnewFadeVar = null;
	        
	        this.spnewFade = null;
	        
	        this.spnewFadeBool = null;
        }

        public override void OnInspectorGUI()
        {
            this.serializedObject.Update();
	        EditorGUILayout.Space();
	        EditorGUILayout.PropertyField(this.spGameObject, new GUIContent("Collider Object"));
	        EditorGUILayout.Space();
	        
	        EditorGUILayout.LabelField("Radius", EditorStyles.boldLabel);
	        EditorGUI.indentLevel++;
	        EditorGUILayout.PropertyField(this.spnewRadiusBool, new GUIContent("Value from Variable"));
	        
	        if (newRadiusBool == true)
	        {
		        EditorGUILayout.PropertyField(this.spnewRadiusVar, new GUIContent("New Radius"));
	        }
	        else
	        {
		        EditorGUILayout.PropertyField(this.spnewRadius, new GUIContent("New Radius"));
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
