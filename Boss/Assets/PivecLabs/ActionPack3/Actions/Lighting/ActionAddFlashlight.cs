namespace PivecLabs.ActionPack
{
	using System.Collections;
	using System.Collections.Generic;
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
	public class ActionAddFlashlight : IAction
	{

		private Light LightObject;

		public bool newColorBool = false;
		public bool newIntensityBool = false;
		public bool newRangeBool = false;
		public bool useCookieBool = false;
		
		public bool activeBool = false;


		public Color newColor;
		[Range (0,10)]
		public float newIntensity;
		[Range (0,100)]
		public float newRange;

		[VariableFilter(Variable.DataType.Color)]
		public VariableProperty newColorVar = new VariableProperty(Variable.VarType.GlobalVariable);

		[VariableFilter(Variable.DataType.Number)]
		public VariableProperty newIntensityVar = new VariableProperty(Variable.VarType.GlobalVariable);

		[VariableFilter(Variable.DataType.Number)]
		public VariableProperty newRangeVar = new VariableProperty(Variable.VarType.GlobalVariable);

		public enum ATTACHTO
		{
			MainCamera,
			GameObject,
			Character
            
		}


		public ATTACHTO attachto = ATTACHTO.MainCamera;
		
		public TargetGameObject gameobject = new TargetGameObject(TargetGameObject.Target.GameObject);
		public TargetCharacter character = new TargetCharacter();
		public HumanBodyBones bone = HumanBodyBones.RightHand;
		public Texture2DProperty cookietexture = new Texture2DProperty();

		public Space space = Space.Self;
		public Vector3 position = Vector3.zero;
		public Vector3 rotation = Vector3.zero;

			
        // EXECUTABLE: ----------------------------------------------------------------------------

        public override bool InstantExecute(GameObject target, IAction[] actions, int index)
		{
        	
			GameObject go = GameObject.Find ("AP3FlashLight");
			if (go){
				Destroy (go.gameObject);
			}

			GameObject flashLight = new GameObject();

			switch (this.attachto)
			{
				
			case ATTACHTO.MainCamera:
			
				Camera cam = HookCamera.Instance.Get<Camera>();
				
				flashLight.transform.parent = cam.transform;
				flashLight.transform.rotation = cam.transform.rotation;
				LightObject = flashLight.gameObject.AddComponent<Light>();
				LightObject.transform.localPosition = new Vector3 (0,0,0);

				break;

			case ATTACHTO.GameObject:
			
				GameObject objectTarget = this.gameobject.GetGameObject(target);
				 
				flashLight.transform.parent = objectTarget.transform;
				flashLight.transform.rotation = objectTarget.transform.rotation;
				LightObject = flashLight.gameObject.AddComponent<Light>();
				LightObject.transform.localPosition = this.position;
				LightObject.transform.localRotation = Quaternion.Euler(this.rotation);

				break;

			case ATTACHTO.Character:
			
				Character charTarget = this.character.GetCharacter(target);
				
				CharacterAnimator animator = charTarget.GetCharacterAnimator();
				if (animator == null) return true;

				CharacterAttachments attachments = animator.GetCharacterAttachments();
				if (attachments == null) return true;
				
				
				attachments.Attach(
					this.bone,
					flashLight,
					this.position,
					Quaternion.Euler(this.rotation),
					this.space
				);

 		
				LightObject = flashLight.gameObject.AddComponent<Light>();
				
				break;
			}
        	
			if (HookPlayer.Instance != null)
			{


				LightObject.type = LightType.Spot;
				LightObject.shadows = LightShadows.Soft;
				LightObject.cullingMask &=  ~(1 << LayerMask.NameToLayer("Player"));
				LightObject.cookie =  this.cookietexture.GetValue(target);
				LightObject.renderMode = LightRenderMode.ForcePixel;
				LightObject.name = "AP3FlashLight";
					
	         if (newColorBool == true)
	        {    
		         var newcolor = (Color)this.newColorVar.Get(target);
		         LightObject.color = newcolor;
	        }
		     else
	        {
			     LightObject.color = newColor;
			 }
        
	        if (newIntensityBool == true)
	        {    
		        var newIntensity = (float)this.newIntensityVar.Get(target);
		        LightObject.intensity = newIntensity;
	        }
	        else
	        {
		        LightObject.intensity = newIntensity;
	        }
	       
	        if (newRangeBool == true)
	        {    
		        var newRange = (float)this.newRangeVar.Get(target);
		        LightObject.range = newRange;
	        }
	        else
	        {
		        LightObject.range = newRange;
	        }
	       
	  
			}
            return true;
         
        }


        // +--------------------------------------------------------------------------------------+
        // | EDITOR                                                                               |
        // +--------------------------------------------------------------------------------------+

#if UNITY_EDITOR
      
		public static new string NAME = "ActionPack3/Lighting/Add Flashlight";
		private const string NODE_TITLE = "Add Flashlight";
		public const string CUSTOM_ICON_PATH = "Assets/PivecLabs/ActionPack3/Icons/";

        // PROPERTIES: ----------------------------------------------------------------------------
		private SerializedProperty spAttachTo;
		private SerializedProperty spCharacter;
		private SerializedProperty spGameobject;
		private SerializedProperty spBone;
		private SerializedProperty spCookie;
		private SerializedProperty spTexture;
		private SerializedProperty spSpace;
		private SerializedProperty spPosition;
		private SerializedProperty spRotation;


		private SerializedProperty spnewColorVar;
		private SerializedProperty spnewIntensityVar;
		private SerializedProperty spnewRangeVar;
		
		private SerializedProperty spnewColor;
		private SerializedProperty spnewIntensity;
		private SerializedProperty spnewRange;

		private SerializedProperty spnewColorBool;
		private SerializedProperty spnewIntensityBool;
		private SerializedProperty spnewRangeBool;
  
        // INSPECTOR METHODS: ---------------------------------------------------------------------
        // INSPECTOR METHODS: ---------------------------------------------------------------------

        public override string GetNodeTitle()
        {

	        return string.Format(
		        NODE_TITLE
		       
	        );
        }

        protected override void OnEnableEditorChild()
        {
	        this.spAttachTo = this.serializedObject.FindProperty("attachto");
	        this.spCharacter = this.serializedObject.FindProperty("character");
	        this.spGameobject = this.serializedObject.FindProperty("gameobject");
	        this.spBone = this.serializedObject.FindProperty("bone");
	        this.spCookie = this.serializedObject.FindProperty("useCookieBool");
	        this.spTexture = this.serializedObject.FindProperty("cookietexture");
	        this.spSpace = serializedObject.FindProperty("space");
	        this.spPosition = this.serializedObject.FindProperty("position");
	        this.spRotation = this.serializedObject.FindProperty("rotation");

	        this.spnewColorVar = this.serializedObject.FindProperty("newColorVar");
	        this.spnewIntensityVar = this.serializedObject.FindProperty("newIntensityVar");
	        this.spnewRangeVar = this.serializedObject.FindProperty("newRangeVar");
	        
	        this.spnewColor = this.serializedObject.FindProperty("newColor");
	        this.spnewIntensity = this.serializedObject.FindProperty("newIntensity");
	        this.spnewRange = this.serializedObject.FindProperty("newRange");

	        this.spnewColorBool = this.serializedObject.FindProperty("newColorBool");
	        this.spnewIntensityBool = this.serializedObject.FindProperty("newIntensityBool");
	        this.spnewRangeBool = this.serializedObject.FindProperty("newRangeBool");

        }

        protected override void OnDisableEditorChild()
        {
	        this.spAttachTo = null;
	        this.spCharacter = null;
	        this.spGameobject = null;
	        this.spBone = null;
	        this.spCookie = null;
	        this.spTexture = null;
	        this.spSpace = null;
	        this.spPosition = null;
	        this.spRotation = null;

	        this.spnewColorVar = null;
	        this.spnewIntensityVar = null;
	        this.spnewRangeVar = null;
	        
	        this.spnewColor = null;
	        this.spnewIntensity = null;
	        this.spnewRange = null;
	        
	        this.spnewColorBool = null;
	        this.spnewIntensityBool = null;
	        this.spnewRangeBool = null;
        }

        public override void OnInspectorGUI()
        {
            this.serializedObject.Update();
	        EditorGUILayout.Space();
	        EditorGUILayout.PropertyField(this.spAttachTo, new GUIContent("Attach Flashlight to:"));
	        EditorGUILayout.Space();
	        
	        if (attachto == ATTACHTO.Character)
	        {
		        EditorGUI.indentLevel++;

	        	EditorGUILayout.PropertyField(this.spCharacter);
	        	
		        EditorGUILayout.PropertyField(this.spBone);

		        EditorGUILayout.PropertyField(this.spSpace);
		        
		        EditorGUI.indentLevel--;
	        }
	        
	        if (attachto == ATTACHTO.GameObject)
	        {
		        EditorGUI.indentLevel++;

	        	EditorGUILayout.PropertyField(this.spGameobject);
	        	
		        EditorGUI.indentLevel--;
	        }

	        if (attachto == ATTACHTO.GameObject || attachto == ATTACHTO.Character)
	        {
		        EditorGUI.indentLevel++;

		        EditorGUILayout.PropertyField(this.spPosition);
		        EditorGUILayout.PropertyField(this.spRotation);
		        EditorGUI.indentLevel--;

	        }

	        EditorGUILayout.Space();
	        EditorGUILayout.PropertyField(this.spCookie, new GUIContent("Use Cookie Texture"));
	        
	        if (useCookieBool == true)
	        {
		        EditorGUI.indentLevel++;

		        EditorGUILayout.PropertyField(this.spTexture, new GUIContent("Cookie Texture"));
		        EditorGUI.indentLevel--;
	        }

	        EditorGUILayout.Space();
	        
	        EditorGUILayout.Space();

	        var rect2 = EditorGUILayout.BeginHorizontal();
	        Handles.color = Color.gray;
	        Handles.DrawLine(new Vector2(rect2.x + 10, rect2.y), new Vector2(rect2.width + 15, rect2.y));
	        EditorGUILayout.EndHorizontal();

	        EditorGUILayout.Space();

	        EditorGUILayout.LabelField("Color", EditorStyles.boldLabel);
	        EditorGUI.indentLevel++;
	        EditorGUILayout.PropertyField(this.spnewColorBool, new GUIContent("Value from Variable"));
	        
	        if (newColorBool == true)
	        {
		        EditorGUILayout.PropertyField(this.spnewColorVar, new GUIContent("Light Color"));
	        }
	        else
	        {
		        EditorGUILayout.PropertyField(this.spnewColor, new GUIContent("Light Color"));
	        }
	        EditorGUI.indentLevel--;
	        
	        EditorGUILayout.LabelField("Intensity", EditorStyles.boldLabel);
	        EditorGUI.indentLevel++;
	        EditorGUILayout.PropertyField(this.spnewIntensityBool, new GUIContent("Value from Variable"));
	        
	        if (newIntensityBool == true)
	        {
		        EditorGUILayout.PropertyField(this.spnewIntensityVar, new GUIContent("Light Intensity"));
	        }
	        else
	        {
		        EditorGUILayout.PropertyField(this.spnewIntensity, new GUIContent("Light Intensity"));
	        }
	        EditorGUI.indentLevel--;
	        
	        EditorGUILayout.LabelField("Range", EditorStyles.boldLabel);
	        EditorGUI.indentLevel++;
	        EditorGUILayout.PropertyField(this.spnewRangeBool, new GUIContent("Value from Variable"));
	        
	        if (newRangeBool == true)
	        {
		        EditorGUILayout.PropertyField(this.spnewRangeVar, new GUIContent("Light Range"));
	        }
	        else
	        {
		        EditorGUILayout.PropertyField(this.spnewRange, new GUIContent("Light Range"));
	        }
	        EditorGUILayout.Space();
	        EditorGUILayout.Space();
	        EditorGUI.indentLevel--;
	        

	        
	       
	        
           this.serializedObject.ApplyModifiedProperties();
        }

#endif
    }
}
