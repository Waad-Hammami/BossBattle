namespace PivecLabs.ActionPack
{
	using System.Collections;
	using System.Collections.Generic;
	using System;
	using System.Linq;
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
	public class ActionSetDefaultTerrainTexture : IAction
	{
		public bool newTextureBool = false;
 
		public int newTextureValue = 0;

		[VariableFilter(Variable.DataType.Number)]
		public VariableProperty newTextureVar = new VariableProperty(Variable.VarType.GlobalVariable);
		public Terrain terrain;

		private int indexOfDefaultTexture;
		private TerrainData terrainData;

        // EXECUTABLE: ----------------------------------------------------------------------------

		public override IEnumerator Execute(GameObject target, IAction[] actions, int index)
		{
	        if (newTextureBool == true)
	        {
		        indexOfDefaultTexture = (int)this.newTextureVar.Get(target);
		        
	        }
	        else
	        {
		        indexOfDefaultTexture = (int)newTextureValue;
		        
	        }
 			 
	        terrainData = terrain.terrainData;      
	        int nbTextures = terrainData.alphamapLayers;
	        float[, ,] splatmapData = new float[terrainData.alphamapWidth, terrainData.alphamapHeight, terrainData.alphamapLayers];

	
 	
	        for (int y = 0; y < terrainData.alphamapHeight; y++)
	        {
		        for (int x = 0; x < terrainData.alphamapWidth; x++)
		        {
			        float[] splatWeights = new float[terrainData.alphamapLayers];
 
			
			        float z = splatWeights.Sum();
 
			        if(Mathf.Approximately(z, 0.0f)){
				        splatWeights [indexOfDefaultTexture] = 1.0f;
			        }
 
			        for(int i = 0; i<terrainData.alphamapLayers; i++){
 
				        splatWeights[i] /= z;
 
				        splatmapData[x, y, i] = splatWeights[i];
			        }
		        }
	        }
		
	        terrainData.SetAlphamaps(0, 0, splatmapData);
      
	        
			yield return new WaitForSeconds(.1f);
         
        }


        // +--------------------------------------------------------------------------------------+
        // | EDITOR                                                                               |
        // +--------------------------------------------------------------------------------------+

#if UNITY_EDITOR
      
		public static new string NAME = "ActionPack3/DayNight/Set Default Terrain Texture";
		private const string NODE_TITLE = "Set Default Terrain Texture";
		public const string CUSTOM_ICON_PATH = "Assets/PivecLabs/ActionPack3/Icons/";

        // PROPERTIES: ----------------------------------------------------------------------------
		private SerializedProperty spnewTextureVar;
		private SerializedProperty spnewTextureVal;
		private SerializedProperty spnewTextureBool;
		private SerializedProperty spnewTerrain;
  
        // INSPECTOR METHODS: ---------------------------------------------------------------------

        public override string GetNodeTitle()
        {

	        return string.Format(
		        NODE_TITLE
		       
	        );
        }

        protected override void OnEnableEditorChild()
        {
	        this.spnewTextureVar = this.serializedObject.FindProperty("newTextureVar");
	        this.spnewTextureVal = this.serializedObject.FindProperty("newTextureValue");
	        this.spnewTextureBool = this.serializedObject.FindProperty("newTextureBool");
	        this.spnewTerrain = this.serializedObject.FindProperty("terrain");

        }

        protected override void OnDisableEditorChild()
        {
	        this.spnewTextureVar = null;
	        this.spnewTextureVar = null;
	        this.spnewTextureBool = null;
	        this.spnewTerrain = null;
        }

        public override void OnInspectorGUI()
        {
	        this.serializedObject.Update();
	        EditorGUILayout.Space();
	        EditorGUILayout.LabelField("NOTE: Only use with single Textured terrain", EditorStyles.boldLabel);

            EditorGUILayout.Space();
	        EditorGUILayout.PropertyField(this.spnewTerrain, new GUIContent("Terrain"));
	        EditorGUILayout.Space();
	        EditorGUILayout.PropertyField(this.spnewTextureBool, new GUIContent("Value from Variable"));
	        if (newTextureBool == true)
	        {
		        EditorGUILayout.PropertyField(this.spnewTextureVar, new GUIContent("New Layer Index"));
	        }
	        else
	        {
		        EditorGUILayout.PropertyField(this.spnewTextureVal, new GUIContent("New Layer Index"));
	        }

	        EditorGUILayout.Space();
	  
           this.serializedObject.ApplyModifiedProperties();
        }

#endif
    }
}
