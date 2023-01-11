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
	public class ActionHighLightObjectOnByTag : IAction
	{

        // PROPERTIES: ----------------------------------------------------------------------------

		private GameObject[] targetObject;
		private Renderer[] renderers;
		 private Material highlightMaskMaterial;
		 private Material highlightFillMaterial;

        [VariableFilter(Variable.DataType.Color)]
		public VariableProperty HighLightColorVar = new VariableProperty(Variable.VarType.GlobalVariable);
 
		[Range(0.0f, 6.0f)] public float highlightWidth = 1.0f;

        [VariableFilter(Variable.DataType.Number)]
		public VariableProperty HighLightWidth = new VariableProperty(Variable.VarType.GlobalVariable);
 
		public bool colourVar = false;
		public bool widthVar = false;
 
		public Color highlightColour = Color.green;

		private static HashSet<Mesh> registeredMeshes = new HashSet<Mesh>();

		public string tagStr = "";


  
        // EXECUTABLE: ----------------------------------------------------------------------------

        public override bool InstantExecute(GameObject target, IAction[] actions, int index)
        {

	        targetObject= GameObject.FindGameObjectsWithTag(tagStr);
	        
	        if (targetObject != null) {
	   
	   		        
		        for (int i = 0; i < targetObject.Length; i++)

	        	{

	   
			        renderers = targetObject[i].GetComponentsInChildren<Renderer>();

	        
			        foreach (var skinnedMeshRenderer in targetObject[i].GetComponentsInChildren<SkinnedMeshRenderer>()) {
		       	if (registeredMeshes.Add(skinnedMeshRenderer.sharedMesh)) {
			       	skinnedMeshRenderer.sharedMesh.uv4 = new Vector2[skinnedMeshRenderer.sharedMesh.vertexCount];
		       	}
	       	}
	        
			        foreach (var meshFilter in targetObject[i].GetComponentsInChildren<MeshFilter>()) {


		         meshFilter.sharedMesh.SetUVs(3, new Vector2[meshFilter.sharedMesh.vertexCount]);
	        }
	        
	        highlightMaskMaterial = Instantiate(Resources.Load<Material>(@"MaskObject"));
	        highlightFillMaterial = Instantiate(Resources.Load<Material>(@"FillObject"));

	        highlightMaskMaterial.name = "MaskObject (Instance)";
	        highlightFillMaterial.name = "FillObject (Instance)";

	        if (colourVar == true)
	        {
		        highlightColour = (Color)this.HighLightColorVar.Get(target);
	        }
	         

	        if (widthVar == true)
	        {
		        highlightWidth = (float)this.HighLightWidth.Get(target);
	        }
	         
	        
	        foreach (var renderer in renderers) {

		        var materials = renderer.sharedMaterials.ToList();

		        materials.Add(highlightMaskMaterial);
		        materials.Add(highlightFillMaterial);

		        renderer.materials = materials.ToArray();
	        }
                     
	        highlightFillMaterial.SetColor("_HighLightColor", highlightColour);
	        highlightMaskMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.Always);
	        highlightFillMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.LessEqual);
	        highlightFillMaterial.SetFloat("_HighLightWidth", highlightWidth);

	        	}
	        }

            return true;
        }
	

        // +--------------------------------------------------------------------------------------+
        // | EDITOR                                                                               |
        // +--------------------------------------------------------------------------------------+

#if UNITY_EDITOR


		public static new string NAME = "ActionPack2/Graphics/HighLight Object On By Tag";
		private const string NODE_TITLE = "HighLight Object On By Tag";
		public const string CUSTOM_ICON_PATH = "Assets/PivecLabs/ActionPack2/Icons/";

		// PROPERTIES: ----------------------------------------------------------------------------

		private SerializedProperty spHighLightColour;
        private SerializedProperty spcolourVar;
		private SerializedProperty spHighLightColourVar;

        private SerializedProperty spToggleWidth;
 
		private SerializedProperty spHighLightWidthSlider;
		private SerializedProperty spHighLightOnlySLider;

		private SerializedProperty spHighLightWidth;
  
  // INSPECTOR METHODS: ---------------------------------------------------------------------

        public override string GetNodeTitle()
		{
			return string.Format(NODE_TITLE);
		}

		protected override void OnEnableEditorChild ()
		{
			this.spcolourVar = this.serializedObject.FindProperty("colourVar");
			this.spHighLightColour = this.serializedObject.FindProperty("highlightColour");
			this.spHighLightColourVar = this.serializedObject.FindProperty("HighLightColorVar");
            this.spToggleWidth = this.serializedObject.FindProperty("widthVar");
 			this.spHighLightWidthSlider = this.serializedObject.FindProperty("highlightWidth");
			this.spHighLightWidth = this.serializedObject.FindProperty("HighLightWidth");
 

        }

        protected override void OnDisableEditorChild ()
		{
			this.spHighLightColour = null;
            this.spcolourVar = null;
            this.spHighLightColourVar = null;
            this.spToggleWidth = null; 
            this.spHighLightWidthSlider = null;
            this.spHighLightWidth = null;
			    
		}

        public override void OnInspectorGUI()
		{
			this.serializedObject.Update();
			
			tagStr = EditorGUILayout.TagField("HighLighted Object Tag", tagStr);

			EditorGUILayout.Space();
            EditorGUILayout.PropertyField(this.spcolourVar, new GUIContent("Value from Variable"));
            if (colourVar == true)
            {
	            EditorGUILayout.PropertyField(this.spHighLightColourVar, new GUIContent("HighLight Colour"));
            }
            else
            {
                EditorGUILayout.PropertyField(this.spHighLightColour, new GUIContent("HighLight Colour"));
            }


            EditorGUILayout.Space();
            EditorGUILayout.Space();


            EditorGUILayout.PropertyField(this.spToggleWidth, new GUIContent("Value from Variable"));
            if (widthVar == true)
            {
                EditorGUILayout.PropertyField(this.spHighLightWidth, new GUIContent("HighLight Width"));
            }
            else
            {
                EditorGUILayout.PropertyField(this.spHighLightWidthSlider, new GUIContent("HighLight Width"));
            }

            EditorGUILayout.Space();
            EditorGUILayout.Space();

              EditorGUILayout.Space();
            this.serializedObject.ApplyModifiedProperties();
		}

		#endif
	}
}
