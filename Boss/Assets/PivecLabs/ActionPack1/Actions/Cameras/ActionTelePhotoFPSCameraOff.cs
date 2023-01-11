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
	public class ActionTelePhotoFPSCameraOff : IAction
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
				
		        zoom.positionOffset.z = 0;
			        
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

	    public static new string NAME = "ActionPack2/Camera/Telephoto First Person Camera Off";
	    private const string NODE_TITLE = "Telephoto First Person Camera Off";
	    public const string CUSTOM_ICON_PATH = "Assets/PivecLabs/ActionPack2/Icons/";

        // PROPERTIES: ----------------------------------------------------------------------------

        // INSPECTOR METHODS: ---------------------------------------------------------------------

        public override string GetNodeTitle()
        {
            return string.Format(
                 NODE_TITLE
             );
        }

        protected override void OnEnableEditorChild()
        {

	    }

        protected override void OnDisableEditorChild()
        {
      }

        public override void OnInspectorGUI()
        {
            this.serializedObject.Update();
	        EditorGUILayout.Space();
	        EditorGUILayout.LabelField("Reset Camera Values", EditorStyles.boldLabel);
	        EditorGUILayout.Space();

	        this.serializedObject.ApplyModifiedProperties();

        }
#endif
    }
}
