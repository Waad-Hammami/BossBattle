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
	public class ActionRandomObjectFromList : IAction
    {

        [System.Serializable]
	    public class ActionGObject
        {
	        public TargetGameObject target = new TargetGameObject();
           
            [Range(1f,100)]
	        public int Probability;
            [HideInInspector]
	        public int actionweight;
        }

        [SerializeField]
	    public List<ActionGObject> ListofObjects = new List<ActionGObject>();

	    public bool active = true;
	    public bool leaveactive = true;

	    private GameObject targetValue;
    
        // EXECUTABLE: ----------------------------------------------------------------------------

	    public override bool InstantExecute(GameObject target, IAction[] actions, int index)
	    {
		    if (leaveactive == false)
		    {
			    if (ListofObjects.Capacity > 0)
			    {
				    for (int i = 0; i < ListofObjects.Capacity; i++)
				    {
					    targetValue = ListofObjects[i].target.GetGameObject(target);
					    if (targetValue != null) targetValue.SetActive(!this.active);

				    }
			    }
		    }
		   
			
	        int rand = RandomProbability();
                
	        targetValue = ListofObjects[rand].target.GetGameObject(target);
	        if (targetValue != null) targetValue.SetActive(this.active);


		    return true;

        }


        public int RandomProbability()
        {

	        int weightTotal = 0;
            if (ListofObjects.Capacity > 0)
            {
                for (int i = 0; i < ListofObjects.Capacity; i++)
                {
                    weightTotal += ListofObjects[i].Probability;

                }

                int result = 0, total = 0;
                int randVal = Random.Range(0, weightTotal);

                for (result = 0; result < ListofObjects.Capacity; result++)
                {
                    total += ListofObjects[result].Probability;
                    if (total > randVal) break;
                }

                return result;

            }
            return 0;
        }

      





        // +--------------------------------------------------------------------------------------+
        // | EDITOR                                                                               |
        // +--------------------------------------------------------------------------------------+

#if UNITY_EDITOR
       
	    public static new string NAME = "ActionPack1/Misc/Set Active Random Object From List";
	    private const string NODE_TITLE = "Set {0} Random Object from List";
        public const string CUSTOM_ICON_PATH = "Assets/PivecLabs/ActionPack1/Icons/";

        // PROPERTIES: ----------------------------------------------------------------------------
	    
	    private SerializedProperty spActive;
	    private SerializedProperty spleaveActive;

  
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


        }

        protected override void OnDisableEditorChild()
	    {
		    this.spActive = null;
		    this.spleaveActive = null;

         }




        public override void OnInspectorGUI()
        {



            this.serializedObject.Update();
	        EditorGUILayout.LabelField("Set Active/Inactive 1 Random Object from List", EditorStyles.boldLabel);
	        EditorGUILayout.Space();
	        EditorGUI.indentLevel++;
	        EditorGUILayout.PropertyField(this.spActive, new GUIContent("Set In/Active"));
	        EditorGUILayout.PropertyField(this.spleaveActive, new GUIContent("Leave In/Active after next"));

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
                    
	                SetProbabilityForList();
	                 
                EditorGUI.indentLevel--;
                }
            }

      
     private void SetProbabilityForList()
	    {
		    float tempCount = 0;
		    foreach (ActionGObject target in ListofObjects)
		    {
			    if (target.Probability == 0)
			    {
				    target.Probability = 100;
			    }
			    tempCount += target.Probability;
		    }

          

	    }

#endif
    }
}