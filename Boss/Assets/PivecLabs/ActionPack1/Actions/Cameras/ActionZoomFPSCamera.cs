namespace PivecLabs.ActionPack
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
	using UnityEngine.Events;
	using GameCreator.Core;
	using GameCreator.Variables;
	using GameCreator.Core.Hooks;
	using GameCreator.Camera;
	using PivecLabs.MinMaxSliderInt;



#if UNITY_EDITOR
    using UnityEditor;
#endif

    [AddComponentMenu("")]
	public class ActionZoomFPSCamera : IAction
    {
	    public KeyCode selectedKey = KeyCode.None;

	    private bool keydown;
	    private bool keyup;

	    public NumberProperty sensitivity = new NumberProperty(10.0f);

	    [MinMaxRange(1, 80)]
	    public MinMaxInt MinMaxVal;
	    
	    
	    public GameObject maskPrefab;
	    
	    private GameObject mask;
	    
	    public bool zoomSave;

	    [VariableFilter(Variable.DataType.Number)]
	    public VariableProperty targetVariable = new VariableProperty(Variable.VarType.GlobalVariable);

       // EXECUTABLE: ----------------------------------------------------------------------------

        public override bool InstantExecute(GameObject target, IAction[] actions, int index)
        {


	        var controller = HookCamera.Instance.Get<GameCreator.Camera.CameraController>();
	        float value = (float)this.sensitivity.GetValue(target);

	 
	        if (keydown)
	        {

		        keyup = true;
		        
		        if (controller.currentCameraMotor != null && controller.currentCameraMotor.cameraMotorType.GetType() == typeof(GameCreator.Camera.CameraMotorTypeFirstPerson))
	    	  {
			        var zoom = controller.currentCameraMotor.GetComponent<GameCreator.Camera.CameraMotorTypeFirstPerson>();
				
			        
			        
			        if (mask == null) {
	        	
				        mask = Instantiate(maskPrefab);

			        }

			        zoom.positionOffset.z += Input.GetAxis("Mouse ScrollWheel") * value;
			        if (zoom.positionOffset.z > MinMaxVal.max) zoom.positionOffset.z = MinMaxVal.max;

			        if (zoom.positionOffset.z <= MinMaxVal.min) zoom.positionOffset.z = MinMaxVal.min;
		     
			        if (zoomSave)
			        {
				        this.targetVariable.Set(zoom.positionOffset.z, null);
			        }
	        	}
	        }
	        
	        if (!keydown && keyup)
	        {
	        	keyup = false;
		        var zoom = controller.currentCameraMotor.GetComponent<GameCreator.Camera.CameraMotorTypeFirstPerson>();

		        zoom.positionOffset.z = 0;
		        
		        Destroy(mask);
		        
		        if (zoomSave)
		        {
			        this.targetVariable.Set(zoom.positionOffset.z, null);
		        }
	        }
            return true;
        }


	    private void Update()
	    {
	    	
	    	if ((selectedKey != KeyCode.None))
	    	
	    	{
		    	if (Input.GetKeyDown(this.selectedKey))

		    	{
			    	keydown = true;
		    	}
		    
		    	if (Input.GetKeyUp(this.selectedKey))

		    	{
			    	keydown = false;
		    	}
	    		
	    	}

	    	
	    }
	    
	  
        // +--------------------------------------------------------------------------------------+
        // | EDITOR                                                                               |
        // +--------------------------------------------------------------------------------------+

#if UNITY_EDITOR

	    public static new string NAME = "ActionPack2/Camera/Zoom First Person Camera";
	    private const string NODE_TITLE = "Zoom First Person Camera";
	    public const string CUSTOM_ICON_PATH = "Assets/PivecLabs/ActionPack2/Icons/";

        // PROPERTIES: ----------------------------------------------------------------------------
	    private SerializedProperty spselectedKey;

	    private SerializedProperty spsensitivity;
	    private SerializedProperty spMaxVal;
	    private SerializedProperty spmaskPrefab;
	    private SerializedProperty spzoomSave;
	    private SerializedProperty spsVar;

        // INSPECTOR METHODS: ---------------------------------------------------------------------

        public override string GetNodeTitle()
        {
            return string.Format(
                 NODE_TITLE
             );
        }

        protected override void OnEnableEditorChild()
        {
	        this.spselectedKey = this.serializedObject.FindProperty("selectedKey");
	        this.spsensitivity = this.serializedObject.FindProperty("sensitivity");
	        this.spMaxVal = this.serializedObject.FindProperty("MinMaxVal");
	        this.spmaskPrefab = this.serializedObject.FindProperty("maskPrefab");
	        this.spzoomSave = this.serializedObject.FindProperty("zoomSave");
	        this.spsVar = this.serializedObject.FindProperty("targetVariable");

	    }

        protected override void OnDisableEditorChild()
        {
	        this.spselectedKey = null;
	        this.spsensitivity = null;
	        this.spMaxVal = null;
	        this.spmaskPrefab = null;
	        this.spzoomSave = null;
	        this.spsVar = null;
        }

        public override void OnInspectorGUI()
        {
            this.serializedObject.Update();
	        EditorGUILayout.Space();
	        EditorGUILayout.PropertyField(this.spselectedKey, new GUIContent("Use while KeyDown"));

	        EditorGUILayout.PropertyField(this.spsensitivity, new GUIContent("Scrollwheel sensitivity"));

	        EditorGUILayout.Space();
	        EditorGUILayout.PropertyField(this.spMaxVal, new GUIContent("Maximum Range"));
	        EditorGUILayout.Space();
	        EditorGUILayout.PropertyField(this.spzoomSave, new GUIContent("Save Current Zoom"));
	        if (zoomSave)
	        {
		        EditorGUILayout.PropertyField(this.spsVar, new GUIContent("Current Zoom Var"));

	        }

	        EditorGUILayout.Space();
	        EditorGUILayout.PropertyField(this.spmaskPrefab, new GUIContent("Mask Prefab"));

	        EditorGUILayout.Space();

	 
	        this.serializedObject.ApplyModifiedProperties();

        }
#endif
    }
}
