namespace PivecLabs.ActionPack
{
        using System.Collections;
        using System.Collections.Generic;
        using UnityEngine;
        using UnityEngine.Events;
        using UnityEngine.UI;
        using UnityEngine.Video;
        using GameCreator.Core;
        using GameCreator.Variables;

#if UNITY_EDITOR
    using UnityEditor;
#endif

        [AddComponentMenu("")]
        public class ActionDisplayCameraOnObject : IAction
        {
	        public TargetGameObject target1 = new TargetGameObject();
	        public TargetGameObject target2 = new TargetGameObject();

	        
	        public Vector3Property variable = new Vector3Property(new Vector3(-90.0f,0f,0f));
	        public NumberProperty variableFOV = new NumberProperty(60.0f);


	    	private Camera targetCamera;
 	        private RenderTexture targetRenderTexture;
	        private Quaternion currentRotation;
	        private Vector3 currentEulerAngles;


        // EXECUTABLE: ----------------------------------------------------------------------------

        public override bool InstantExecute(GameObject target, IAction[] actions, int index)
	        {
            	
            	
		        GameObject targetObject1 = this.target1.GetGameObject(target);
		        GameObject targetObject2 = this.target2.GetGameObject(target);
		        if (targetObject1 != null && targetObject2 != null) {

		        targetRenderTexture = new RenderTexture (Screen.width, Screen.height, 24, RenderTextureFormat.ARGB32);
		        targetRenderTexture.Create();


		        GameObject camera3d = new GameObject();
		        targetCamera = camera3d.AddComponent<Camera>();
		        targetCamera.transform.SetParent(targetObject1.transform);
		        targetCamera.transform.localPosition = new Vector3 (0,0,0);

	
		            targetCamera.enabled = true;
		            targetCamera.allowHDR = true;
		        	targetCamera.orthographic = false;
		        	targetCamera.fieldOfView = 	variableFOV.GetValue(target);		
		            targetCamera.name = "MirrorCamera";

		    		targetCamera.clearFlags = CameraClearFlags.SolidColor;
		            targetCamera.backgroundColor = Color.clear;
	         
		        targetCamera.targetTexture = targetRenderTexture;
		        targetCamera.Render();

		        
		        Renderer render = targetObject2.GetComponent<Renderer>();
		        if (render != null) render.material.mainTexture = targetRenderTexture;

		        currentEulerAngles = variable.GetValue(target);
		        currentRotation.eulerAngles = currentEulerAngles;
		        targetCamera.transform.localRotation = currentRotation;
 
		        }
            return true;
                
            }

            public override IEnumerator Execute(GameObject target, IAction[] actions, int index)
            {

            

            return base.Execute(target, actions, index);

        }

            // +--------------------------------------------------------------------------------------+
            // | EDITOR                                                                               |
            // +--------------------------------------------------------------------------------------+

#if UNITY_EDITOR

	        public static new string NAME = "ActionPack2/Camera/Add Camera to Object";
	        private const string NODE_TITLE = "Place Camera on {0}";
	        public const string CUSTOM_ICON_PATH = "Assets/PivecLabs/ActionPack2/Icons/";

        // PROPERTIES: ----------------------------------------------------------------------------

	        private SerializedProperty sptargetObject1;
	        private SerializedProperty sptargetObject2;
	        private SerializedProperty spVariable;
	        	private SerializedProperty spVariableFOV;

        // INSPECTOR METHODS: ---------------------------------------------------------------------

        public override string GetNodeTitle()
            {
                return string.Format(
                     NODE_TITLE,
	                this.target1
                 );
            }

            protected override void OnEnableEditorChild()
            {
	            this.sptargetObject1 = this.serializedObject.FindProperty("target1");
	            this.sptargetObject2 = this.serializedObject.FindProperty("target2");
	            this.spVariable = this.serializedObject.FindProperty("variable");
	            this.spVariableFOV = this.serializedObject.FindProperty("variableFOV");
 
         }

        protected override void OnDisableEditorChild()
            {
	            this.sptargetObject1 = null;
	            this.sptargetObject2 = null;
	            this.spVariable = null;
	            this.spVariableFOV = null;
  
        }

        public override void OnInspectorGUI()
            {
                this.serializedObject.Update();
        
            	EditorGUILayout.Space();

	            EditorGUILayout.PropertyField(this.sptargetObject1, new GUIContent("Object for Camera"));
	            EditorGUILayout.Space();
	            EditorGUILayout.PropertyField(this.sptargetObject2, new GUIContent("Object for Image"));
	            EditorGUILayout.Space();
	            EditorGUILayout.PropertyField(this.spVariable, new GUIContent("Rotation for Camera"));
	            EditorGUILayout.Space();
	            EditorGUILayout.PropertyField(this.spVariableFOV, new GUIContent("FOV for Camera"));
        
            this.serializedObject.ApplyModifiedProperties();
            }

#endif
        }
    }
