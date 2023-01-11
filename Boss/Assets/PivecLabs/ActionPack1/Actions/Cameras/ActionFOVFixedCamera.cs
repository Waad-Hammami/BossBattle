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
	public class ActionFOVFixedCamera : IAction
    {
	    
	    public NumberProperty sensitivity = new NumberProperty(10.0f);

	    [MinMaxRange(1, 180)]
	    public MinMaxInt MinMaxVal;




       // EXECUTABLE: ----------------------------------------------------------------------------

        public override bool InstantExecute(GameObject target, IAction[] actions, int index)
        {


	        var controller = HookCamera.Instance.Get<GameCreator.Camera.CameraController>();
	        float value = (float)this.sensitivity.GetValue(target);

	        
	        if (controller.currentCameraMotor != null && controller.currentCameraMotor.cameraMotorType.GetType() == typeof(GameCreator.Camera.CameraMotorTypeFixed))
	        {
		        var orbit = controller.currentCameraMotor.GetComponent<GameCreator.Camera.CameraMotorTypeFixed>();
				
		        orbit.fieldOfView = orbit.fieldOfView += Input.GetAxis("Mouse ScrollWheel") * value;
		        if (orbit.fieldOfView < MinMaxVal.min) orbit.fieldOfView = MinMaxVal.min;
		        if (orbit.fieldOfView > MinMaxVal.max) orbit.fieldOfView = MinMaxVal.max;
		        
		        
		        
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

	    public static new string NAME = "ActionPack2/Camera/FOV Fixed Camera";
	    private const string NODE_TITLE = "FOV Fixed Camera";
	    public const string CUSTOM_ICON_PATH = "Assets/PivecLabs/ActionPack2/Icons/";

        // PROPERTIES: ----------------------------------------------------------------------------

	    private SerializedProperty spsensitivity;
	    private SerializedProperty spMinMax;

        // INSPECTOR METHODS: ---------------------------------------------------------------------

        public override string GetNodeTitle()
        {
            return string.Format(
                 NODE_TITLE
             );
        }

        protected override void OnEnableEditorChild()
        {
	        this.spsensitivity = this.serializedObject.FindProperty("sensitivity");
	        this.spMinMax = this.serializedObject.FindProperty("MinMaxVal");
 
	    }

        protected override void OnDisableEditorChild()
        {
	        this.spsensitivity = null;
	        this.spMinMax = null;
        }

        public override void OnInspectorGUI()
        {
            this.serializedObject.Update();
	        EditorGUILayout.Space();

	        EditorGUILayout.PropertyField(this.spsensitivity, new GUIContent("Scrollwheel sensitivity"));

	        EditorGUILayout.Space();
	        EditorGUILayout.PropertyField(this.spMinMax, new GUIContent("FOV Range"));

	        this.serializedObject.ApplyModifiedProperties();

        }
#endif
    }
}
