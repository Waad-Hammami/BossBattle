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
	public class ActionTelePhotoAdvCameraOff : IAction
    {

	    [Range(1, 80)]
	    public float MaxVal;
	    
	    public bool zoomallow = true;
	    public bool zoomOrbit = true;
	    
	    public GameObject maskPrefab;
	    
	    private GameObject mask;
       // EXECUTABLE: ----------------------------------------------------------------------------

        public override bool InstantExecute(GameObject target, IAction[] actions, int index)
        {


	        var controller = HookCamera.Instance.Get<GameCreator.Camera.CameraController>();
		        
		        if (controller.currentCameraMotor != null && controller.currentCameraMotor.cameraMotorType.GetType() == typeof(GameCreator.Camera.CameraMotorTypeAdventure))
	    	  {
		        var zoom = controller.currentCameraMotor.GetComponent<GameCreator.Camera.CameraMotorTypeAdventure>();
				
			        zoom.targetOffset.z = 0;
				    zoom.allowZoom = zoomallow;
			        zoom.allowOrbitInput = zoomOrbit;
			        
			        mask = GameObject.Find("AP2TelephotoMask");
			        
			        if (mask  != null)
			        {
			        	
				        Destroy(mask);
			        }
	    
	        
	        }
	        
	    
            return true;
        }


	  
	    
	  
        // +--------------------------------------------------------------------------------------+
        // | EDITOR                                                                               |
        // +--------------------------------------------------------------------------------------+

#if UNITY_EDITOR

	    public static new string NAME = "ActionPack2/Camera/Telephoto Adventure Camera Off";
	    private const string NODE_TITLE = "Telephoto Adventure Camera Off";
	    public const string CUSTOM_ICON_PATH = "Assets/PivecLabs/ActionPack2/Icons/";

        // PROPERTIES: ----------------------------------------------------------------------------
	    private SerializedProperty spzoomallow;
	    private SerializedProperty spzoomOrbit;

        // INSPECTOR METHODS: ---------------------------------------------------------------------

        public override string GetNodeTitle()
        {
            return string.Format(
                 NODE_TITLE
             );
        }

        protected override void OnEnableEditorChild()
        {
	        this.spzoomallow = this.serializedObject.FindProperty("zoomallow");
	        this.spzoomOrbit = this.serializedObject.FindProperty("zoomOrbit");

	    }

        protected override void OnDisableEditorChild()
        {
	        this.spzoomallow = null;
	        this.spzoomOrbit = null;
       }

        public override void OnInspectorGUI()
        {
            this.serializedObject.Update();
	        EditorGUILayout.Space();
	        EditorGUILayout.LabelField("Reset Camera Values", EditorStyles.boldLabel);
	        EditorGUILayout.Space();
	        EditorGUILayout.PropertyField(this.spzoomallow, new GUIContent("Allow Zoom"));

	        EditorGUILayout.PropertyField(this.spzoomOrbit, new GUIContent("Allow Orbit"));


	        EditorGUILayout.Space();

	        this.serializedObject.ApplyModifiedProperties();

        }
#endif
    }
}
