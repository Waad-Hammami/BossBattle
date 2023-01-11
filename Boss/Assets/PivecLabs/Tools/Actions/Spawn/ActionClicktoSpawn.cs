namespace PivecLabs.Tools

{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.UI;
	using UnityEngine.Events;

	using GameCreator.Core;
	using GameCreator.Core.Hooks;
	using GameCreator.Variables;

#if UNITY_EDITOR
	using UnityEditor;
#endif

	[AddComponentMenu("")]

	public class ActionClicktoSpawn : IAction

 {
	 
	  public enum primitive
	 {
		 Cube,
		 Sphere,
		 Capsule,
		 Cylinder,
		 Plane,
		 Quad
	 }
	 
	 public bool activate;

	 public bool useprefab;

	 public GameObject prefabToUse;
	 
	 [Range(0.1f,10f)]
	 public float prefabSize = 1;
	 public bool useprimitive;
	 public primitive primitiveType = primitive.Cube;
	 
	 [Range(0.1f,10f)]
	 public float primitiveSize = 1;
	 public Vector3 offset;
	 private Vector3 offsetting;

	 [Range(0,2)]
	 public int mouseButton = 1;

	 private GameObject go;
	 
	 private float mZCoord;


	
	 public override bool InstantExecute(GameObject target, IAction[] actions, int index)
		{

				if (activate == true)
			{
	     	
	     	
				{
 
					RaycastHit hit;
					Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		         
		       
		       
					if (useprefab == false)
	         
	        
					{
	         	
								switch (this.primitiveType)
						{
						case primitive.Cube: 
							go = GameObject.CreatePrimitive(PrimitiveType.Cube);
							break;
						case primitive.Sphere: 
							go = GameObject.CreatePrimitive(PrimitiveType.Sphere);	
							break;
						case primitive.Capsule: 
							go = GameObject.CreatePrimitive(PrimitiveType.Capsule);
							break;
						case primitive.Cylinder: 
							go = GameObject.CreatePrimitive(PrimitiveType.Cylinder);	
							break;
						case primitive.Plane: 
							go = GameObject.CreatePrimitive(PrimitiveType.Plane);
							break;
						case primitive.Quad: 
							go = GameObject.CreatePrimitive(PrimitiveType.Quad);
							break;
						}  
						
						
									if (Physics.Raycast(ray, out hit))
						{
									offsetting = new Vector3(hit.point.x +offset.x,hit.point.y +offset.y,hit.point.z +offset.z );
									go.transform.position = offsetting;
									Debug.Log("Instantiated Primitive at Vector " + hit.point);
						}

							go.transform.localScale = new Vector3 (primitiveSize, primitiveSize, primitiveSize);
 
					}

						else if (useprefab == true)
	         
					{
						if (prefabToUse != null)
						{
							go = prefabToUse;
	         		
							if (Physics.Raycast(ray, out hit))
							{
                 
								go.transform.position = hit.point;
								Debug.Log("Instantiated Prefab at Vector " + hit.point);
							}

							go.transform.localScale = new Vector3 (prefabSize, prefabSize, prefabSize);
							Instantiate(go, (hit.point), Quaternion.identity);
						}
					}
					
				}
				
					Debug.Log("Name = " + go.name);
					Debug.Log("Position = " + go.transform.position);
					Debug.Log("Rotation = " + go.transform.rotation);
			}
			
		
			return true;

		}



 
		// +--------------------------------------------------------------------------------------+
		// | EDITOR                                                                               |
		// +--------------------------------------------------------------------------------------+

#if UNITY_EDITOR

		public static new string NAME = "Developer Tools/Spawn/Click to Spawn";
		private const string NODE_TITLE = "Click to Spawn";
		public const string CUSTOM_ICON_PATH = "Assets/PivecLabs/Tools/Icons/";

		// PROPERTIES: ----------------------------------------------------------------------------
	 private SerializedProperty spactivate;
	 private SerializedProperty spuseprefab;
	 private SerializedProperty spprefabToUse;
	 private SerializedProperty spprefabSize;
	 private SerializedProperty spuseprimitive;
	 private SerializedProperty spprimitiveType;
	 private SerializedProperty spprimitiveSize;
	 private SerializedProperty spoffset;

		// INSPECTOR METHODS: ---------------------------------------------------------------------

		public override string GetNodeTitle()
		{

			return string.Format(NODE_TITLE);
		}


		protected override void OnEnableEditorChild()
		{
			this.spactivate = serializedObject.FindProperty("activate");
			this.spuseprefab = serializedObject.FindProperty("useprefab");
			this.spprefabToUse = serializedObject.FindProperty("prefabToUse");
			this.spprefabSize = serializedObject.FindProperty("prefabSize");
			this.spuseprimitive = serializedObject.FindProperty("useprimitive");
			this.spprimitiveType = serializedObject.FindProperty("primitiveType");
			this.spprimitiveSize = serializedObject.FindProperty("primitiveSize");
			this.spoffset = serializedObject.FindProperty("offset");

		}

		protected override void OnDisableEditorChild()
		{
			this.spactivate = null;
			this.spuseprefab = null;
			this.spprefabToUse = null;
			this.spprefabSize = null;
			this.spuseprimitive = null;
			this.spprimitiveType = null;
			this.spprimitiveSize = null;
			this.spoffset = null;


		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();
			EditorGUILayout.Space();

			EditorGUILayout.PropertyField(spactivate,new GUIContent("Activate Click to Spawn"));
			if (activate == true)
			{
		
				EditorGUILayout.Space();
				useprefab = EditorGUILayout.Toggle("Use Prefab", useprefab);
				if (useprefab == true)
				{
					EditorGUI.indentLevel++;
					EditorGUILayout.PropertyField(spprefabToUse,new GUIContent("Prefab Object"));
					EditorGUILayout.PropertyField(spprefabSize,new GUIContent("Prefab Size"));
					EditorGUI.indentLevel--;
					useprimitive = false;
				}
				EditorGUILayout.LabelField("or");
				EditorGUILayout.Space();
				useprimitive = EditorGUILayout.Toggle("Use Primitive", useprimitive);
				if (useprimitive == true)
				{
					EditorGUI.indentLevel++;
					EditorGUILayout.PropertyField(spprimitiveType,new GUIContent("Primitive Type"));
					EditorGUILayout.PropertyField(spprimitiveSize,new GUIContent("Primitive Size"));
					EditorGUILayout.PropertyField(spoffset,new GUIContent("Offset"));
					EditorGUI.indentLevel--;
					useprefab = false;

				}

	
			}



			EditorGUILayout.Space();

			this.serializedObject.ApplyModifiedProperties();	
		}
	
#endif
	}
}