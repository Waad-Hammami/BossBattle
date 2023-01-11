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
    public class ActionOrbitFollowCamera : IAction
    {

	    public enum DIRECTION
	    {
		    Left,
		    Right
	    }

	    public DIRECTION direction = DIRECTION.Left;
	    
	    public NumberProperty speed = new NumberProperty(10.0f);
  
	
	    private GameObject orbiter;
 
       // EXECUTABLE: ----------------------------------------------------------------------------

        public override bool InstantExecute(GameObject target, IAction[] actions, int index)
        {



	        var controller = HookCamera.Instance.Get<GameCreator.Camera.CameraController>();
	        float value = (float)this.speed.GetValue(target);
	        switch (this.direction)
	        {
	        case DIRECTION.Left:
		        value = +value;
		        break;
	        case DIRECTION.Right:
		        value = -value;
		        break;
	        }
	        
	        if (controller.currentCameraMotor != null && controller.currentCameraMotor.cameraMotorType.GetType() == typeof(GameCreator.Camera.CameraMotorTypeFollow))
	        {
		        var orbit = controller.currentCameraMotor.GetComponent<GameCreator.Camera.CameraMotorTypeFollow>();
				
		        GameObject anchor = orbit.anchor.GetGameObject(gameObject);
	        	
		        this.orbiter = new GameObject(gameObject.name + " orbiter");
		        this.orbiter.transform.SetParent(anchor.transform);

		        Vector3 orbiterPosition = orbit.anchorOffset;
		        this.orbiter.transform.localPosition = orbiterPosition;
		   	        
		        this.orbiter.transform.RotateAround(anchor.transform.position, Vector3.up, value * Time.deltaTime);
			    orbit.anchorOffset = this.orbiter.transform.localPosition;
		      
		        Destroy(this.orbiter);
		        
		        
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

	    public static new string NAME = "ActionPack2/Camera/Orbit Follow Camera";
        private const string NODE_TITLE = "Orbit Follow Camera";
	    public const string CUSTOM_ICON_PATH = "Assets/PivecLabs/ActionPack2/Icons/";

        // PROPERTIES: ----------------------------------------------------------------------------

	    private SerializedProperty spdirection;
	    private SerializedProperty spspeed;

        // INSPECTOR METHODS: ---------------------------------------------------------------------

        public override string GetNodeTitle()
        {
            return string.Format(
                 NODE_TITLE
             );
        }

        protected override void OnEnableEditorChild()
        {
	        this.spdirection = this.serializedObject.FindProperty("direction");
	        this.spspeed = this.serializedObject.FindProperty("speed");
        }

        protected override void OnDisableEditorChild()
        {
	        this.spdirection = null;
	        this.spspeed = null;
        }

        public override void OnInspectorGUI()
        {
            this.serializedObject.Update();

	        EditorGUILayout.PropertyField(this.spdirection, new GUIContent("Orbit Direction"));
	        EditorGUILayout.PropertyField(this.spspeed, new GUIContent("Orbit Speed"));

            this.serializedObject.ApplyModifiedProperties();
        }

#endif
    }
}
