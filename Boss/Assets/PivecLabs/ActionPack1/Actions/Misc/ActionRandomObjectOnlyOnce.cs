namespace PivecLabs.ActionPack
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Events;
    using GameCreator.Core;

#if UNITY_EDITOR
    using UnityEditor;
#endif



    [AddComponentMenu("")]
	public class ActionRandomObjectOnlyOnce : IAction
    {

        [System.Serializable]
	    public class ActionGObject
        {
	        public TargetGameObject target = new TargetGameObject();
       
        }

        [SerializeField]
	    public List<ActionGObject> ListofObjects = new List<ActionGObject>();
	    
	    public List<ActionGObject> OriginalListofObjects;


	    public bool active = true;
	    public bool leaveactive = true;
	    public bool repeatatend = true;

	    private GameObject targetValue;
	    
	    private bool repeat = false;
	    private int rand;
	    private int randTotal;

	    private bool once = true;
	    private int listcount;
	    
        // EXECUTABLE: ----------------------------------------------------------------------------

	    public override bool InstantExecute(GameObject target, IAction[] actions, int index)
	    {
	
		    if (once)
	    	{
			    OriginalListofObjects = new List<ActionGObject>(ListofObjects);
	    		once = false;
	    		listcount = ListofObjects.Capacity;
	    	}
		    
		    if (repeatatend == true)
		    {
		    	if (listcount == 0)
		    	{
			    	ListofObjects = new List<ActionGObject>(OriginalListofObjects);
			    	repeat = false;
			    	listcount = ListofObjects.Capacity;

			    } 
		    }
		    
		    if (leaveactive == false)
		    {
			    if (targetValue != null) targetValue.SetActive(!this.active);

		    }
		    
	
		    if (repeat == false)
		    {
			    rand = Random.Range(0, ListofObjects.Capacity);
			    randTotal = ListofObjects.Capacity;
	
		    }

		    else if (repeat == true)

		    {
                
			    rand = Random.Range(0, randTotal);
			    
	
		    }
	    	

		    
		    if (randTotal > 0)
		    {
  

	        	targetValue = ListofObjects[rand].target.GetGameObject(target);
			    if (targetValue != null) targetValue.SetActive(this.active);
	
			    ListofObjects.RemoveAt(rand);
			    randTotal = (randTotal - 1);
			    repeat = true;
			    listcount = (listcount - 1);

		    }
		    

		  
	
		    return true;
		
        }


  
        // +--------------------------------------------------------------------------------------+
        // | EDITOR                                                                               |
        // +--------------------------------------------------------------------------------------+

#if UNITY_EDITOR
       
	    public static new string NAME = "ActionPack1/Misc/Set Active Random Object Only Once";
	    private const string NODE_TITLE = "Set {0} Random Object Only Once";
        public const string CUSTOM_ICON_PATH = "Assets/PivecLabs/ActionPack1/Icons/";

        // PROPERTIES: ----------------------------------------------------------------------------
	    
	    private SerializedProperty spActive;
	    private SerializedProperty spleaveActive;
	    private SerializedProperty sprepeatatEnd;


  
        // INSPECTOR METHODS: ---------------------------------------------------------------------

        public override string GetNodeTitle()
        {

	        return string.Format(NODE_TITLE, 
	        (this.active ? "Active" : "Inactive"));
        }


        protected override void OnEnableEditorChild()
        {
	        this.spActive = this.serializedObject.FindProperty("active");
	        this.spleaveActive = this.serializedObject.FindProperty("leaveactive");
	        this.sprepeatatEnd = this.serializedObject.FindProperty("repeatatend");


        }

        protected override void OnDisableEditorChild()
	    {
		    this.spActive = null;
		    this.spleaveActive = null;
		    this.sprepeatatEnd = null;

         }




        public override void OnInspectorGUI()
        {



            this.serializedObject.Update();
	        EditorGUILayout.LabelField("Set Active/Inactive 1 Random Object from List", EditorStyles.boldLabel);
	        EditorGUILayout.Space();
	        EditorGUI.indentLevel++;
	        EditorGUILayout.PropertyField(this.spActive, new GUIContent("Set In/Active"));
	        EditorGUILayout.PropertyField(this.spleaveActive, new GUIContent("Leave In/Active after next"));
	        EditorGUILayout.PropertyField(this.sprepeatatEnd, new GUIContent("Repeat after list empty"));

            EditorGUILayout.Space();
	        EditorGUILayout.LabelField("Object List");

	        SerializedProperty property = serializedObject.FindProperty("ListofObjects");
	        ArrayGUI(property, "Object ", true);
            EditorGUILayout.Space();
	        EditorGUI.indentLevel--;
               serializedObject.ApplyModifiedProperties();

        }



         private void ArrayGUI(SerializedProperty property, string itemType, bool visible)
            {

                 {

                    EditorGUI.indentLevel++;
                    SerializedProperty arraySizeProp = property.FindPropertyRelative("Array.size");
                    EditorGUILayout.PropertyField(arraySizeProp);
             
                for (int i = 0; i < arraySizeProp.intValue; i++)
                    {
                        EditorGUILayout.PropertyField(property.GetArrayElementAtIndex(i), new GUIContent(itemType + (i +1).ToString()), true);
                   
                    }
                     
                EditorGUI.indentLevel--;
                }
            }

      
  

#endif
    }
}