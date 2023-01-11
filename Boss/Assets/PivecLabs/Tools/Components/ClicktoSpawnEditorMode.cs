namespace PivecLabs.Tools
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

#if UNITY_EDITOR
 using UnityEditor;

	[ExecuteInEditMode]
	[AddComponentMenu("")]

 public class ClicktoSpawnEditorMode : MonoBehaviour
 {
	 
	 [SerializeField]
	 public enum primitive
	 {
		 Cube,
		 Sphere,
		 Capsule,
		 Cylinder,
		 Plane,
		 Quad
	 }
	 [Space(10)] 
	 
	 [SerializeField]
	 public bool activate;
	 [Space(20)] 

	[SerializeField]
	 public bool useprefab;

	 [SerializeField]
	 public GameObject prefabToUse;
	 [Range(0.3f,10f)]
	 
	 [SerializeField]
	 public float prefabSize = 1;

	 [SerializeField]
	 public bool useprimitive;
	 
	 [SerializeField]
	 public primitive primitiveType = primitive.Cube;
	 
	 [SerializeField]
	 [Range(0.1f,10f)]
	 public float primitiveSize = 1;
	 [SerializeField]
	 public Vector3 offset;
	 private Vector3 offsetting;

	 [Space(10)] 
	 [SerializeField]
	 [Range(0,2)]
	 public int mouseButton = 1;

	 private GameObject go;
	 
	 
	 private void OnEnable()
     {
	     EditorUtility.SetDirty(this);
	     offset = new Vector3(0,0,0);

	     if (!Application.isEditor)
         {
             Destroy(this);
         }
	     SceneView.duringSceneGui += OnScene;
     }
 
     void OnScene(SceneView scene)
     {
         Event e = Event.current;
	     if (activate == true)
	     {
	     	
	     	
         if (e.type == EventType.MouseDown && e.button == mouseButton)
         {
  
             Vector3 mousePos = e.mousePosition;
             float ppp = EditorGUIUtility.pixelsPerPoint;
             mousePos.y = scene.camera.pixelHeight - mousePos.y * ppp;
             mousePos.x *= ppp;
 
             Ray ray = scene.camera.ScreenPointToRay(mousePos);
	         RaycastHit hit;
	         
		       
		       
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
            e.Use();
         }
	     }
     }
 }
#endif
}