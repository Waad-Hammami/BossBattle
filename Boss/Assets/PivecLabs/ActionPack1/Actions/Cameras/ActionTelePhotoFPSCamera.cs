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



#if UNITY_EDITOR
    using UnityEditor;
#endif

    [AddComponentMenu("")]
	public class ActionTelePhotoFPSCamera : IAction
    {

	    [Range(1, 80)]
	    public float MaxVal;
	    
	    
	    public GameObject maskPrefab;
	    
	    private GameObject mask;
       // EXECUTABLE: ----------------------------------------------------------------------------

        public override bool InstantExecute(GameObject target, IAction[] actions, int index)
        {


	        var controller = HookCamera.Instance.Get<GameCreator.Camera.CameraController>();
		        
	        if (controller.currentCameraMotor != null && controller.currentCameraMotor.cameraMotorType.GetType() == typeof(GameCreator.Camera.CameraMotorTypeFirstPerson))
	    	  {
			        var zoom = controller.currentCameraMotor.GetComponent<GameCreator.Camera.CameraMotorTypeFirstPerson>();
				
		        
			        
			        if (mask == null) {
	        	
				        mask = Instantiate(maskPrefab);
				        mask.name = "AP2TelephotoMask";

			        }

		        zoom.positionOffset.z = MaxVal;

	        
	        }
	        
	    
            return true;
        }


	  
	    
	  
        // +--------------------------------------------------------------------------------------+
        // | EDITOR                                                                               |
        // +--------------------------------------------------------------------------------------+

#if UNITY_EDITOR

	    public static new string NAME = "ActionPack2/Camera/Telephoto First Person Camera";
	    private const string NODE_TITLE = "Telephoto First Person Camera";
	    public const string CUSTOM_ICON_PATH = "Assets/PivecLabs/ActionPack2/Icons/";

        // PROPERTIES: ----------------------------------------------------------------------------
	    private SerializedProperty spMaxVal;
	    private SerializedProperty spmaskPrefab;

        // INSPECTOR METHODS: ---------------------------------------------------------------------

        public override string GetNodeTitle()
        {
            return string.Format(
                 NODE_TITLE
             );
        }

        protected override void OnEnableEditorChild()
        {
	        this.spMaxVal = this.serializedObject.FindProperty("MaxVal");
	        this.spmaskPrefab = this.serializedObject.FindProperty("maskPrefab");

	    }

        protected override void OnDisableEditorChild()
        {
	        this.spMaxVal = null;
	        this.spmaskPrefab = null;
        }

        public override void OnInspectorGUI()
        {
            this.serializedObject.Update();
	        EditorGUILayout.Space();
	        EditorGUILayout.PropertyField(this.spMaxVal, new GUIContent("Fixed Range"));
	        EditorGUILayout.Space();
	        EditorGUILayout.PropertyField(this.spmaskPrefab, new GUIContent("Mask Prefab"));

	        EditorGUILayout.Space();

	 
	        this.serializedObject.ApplyModifiedProperties();

        }
#endif
    }
}
