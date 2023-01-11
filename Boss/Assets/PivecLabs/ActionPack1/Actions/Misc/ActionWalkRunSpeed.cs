namespace PivecLabs.ActionPack
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
	using UnityEngine.Events;
	using GameCreator.Core;
	using GameCreator.Characters;

	using GameCreator.Variables;
	using GameCreator.Core.Hooks;
	using GameCreator.Camera;
	using PivecLabs.MinMaxSliderFloat;



#if UNITY_EDITOR
    using UnityEditor;
#endif

    [AddComponentMenu("")]
	public class ActionWalkRunSpeed : IAction
    {
	    public KeyCode selectedKey = KeyCode.None;


	    public NumberProperty sensitivity = new NumberProperty(10.0f);
	    public NumberProperty walkrunswitch = new NumberProperty(5);

	    [MinMaxFloat(1, 15)]
	    public MinMaxfloat MinMaxVal;

	    private PlayerCharacter player;
	    private float Speed;
	    private bool keydown;
	    public bool speedSave;

	    [VariableFilter(Variable.DataType.Number)]
	    public VariableProperty targetVariable = new VariableProperty(Variable.VarType.GlobalVariable);

       // EXECUTABLE: ----------------------------------------------------------------------------

        public override bool InstantExecute(GameObject target, IAction[] actions, int index)
        {
	        player = HookPlayer.Instance.Get<PlayerCharacter>();

	        float value = (float)this.sensitivity.GetValue(target);
	        int switchvalue = (int)this.walkrunswitch.GetValue(target);
	        
	      
	        if (keydown  || (selectedKey == KeyCode.None))
	       {
	        	
		       
		        Speed = Speed += Input.GetAxis("Mouse ScrollWheel") * value;
		        if (Speed < MinMaxVal.min) Speed = MinMaxVal.min;
		        if (Speed > MinMaxVal.max) Speed = MinMaxVal.max;

		        if ((Speed > switchvalue ))
		        {    
			        player.characterLocomotion.canRun = true;
		        }
		        else
		        {
			        player.characterLocomotion.canRun = false;
		        }
            
		        player.characterLocomotion.runSpeed = Speed;
		        
		        
		        if (speedSave)
		        {
		        	this.targetVariable.Set(Speed, null);
		        }
	        }

	        
            return true;
        }

        public override IEnumerator Execute(GameObject target, IAction[] actions, int index)
        {

            return base.Execute(target, actions, index);
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

	    public static new string NAME = "ActionPack2/Misc/Walk Run Speed";
	    private const string NODE_TITLE = "Walk Run Speed";
	    public const string CUSTOM_ICON_PATH = "Assets/PivecLabs/ActionPack2/Icons/";

        // PROPERTIES: ----------------------------------------------------------------------------
	    private SerializedProperty spselectedKey;
	    private SerializedProperty spsensitivity;
	    private SerializedProperty spwalkrunswitch;
	    private SerializedProperty spMinMax;
	    private SerializedProperty spspeedSave;
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
	        this.spwalkrunswitch = this.serializedObject.FindProperty("walkrunswitch");
	        this.spMinMax = this.serializedObject.FindProperty("MinMaxVal");
	        this.spspeedSave = this.serializedObject.FindProperty("speedSave");
	        this.spsVar = this.serializedObject.FindProperty("targetVariable");

	    }

        protected override void OnDisableEditorChild()
        {
	        this.spselectedKey = null;
	        this.spsensitivity = null;
	        this.spwalkrunswitch = null;
	        this.spMinMax = null;
	        this.spspeedSave = null;
	        this.spsVar = null;
        }

        public override void OnInspectorGUI()
        {
            this.serializedObject.Update();
	        EditorGUILayout.Space();
	        EditorGUILayout.PropertyField(this.spselectedKey, new GUIContent("Use while KeyDown"));

	        EditorGUILayout.PropertyField(this.spsensitivity, new GUIContent("Scrollwheel sensitivity"));
	        EditorGUILayout.Space();
	        EditorGUILayout.PropertyField(this.spwalkrunswitch, new GUIContent("Min Speed to Run"));

	        EditorGUILayout.Space();
	        EditorGUILayout.PropertyField(this.spMinMax, new GUIContent("Move Speed Range"));
	        EditorGUILayout.Space();
	        EditorGUILayout.PropertyField(this.spspeedSave, new GUIContent("Save Current Speed"));
	        if (speedSave)
	        {
		        EditorGUILayout.PropertyField(this.spsVar, new GUIContent("Current Speed Var"));

	        }

	        this.serializedObject.ApplyModifiedProperties();

        }
#endif
    }
}
