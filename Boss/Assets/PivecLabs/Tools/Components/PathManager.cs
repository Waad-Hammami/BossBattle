namespace PivecLabs.Tools
{
	/*	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

#if UNITY_EDITOR
 using UnityEditor;

	[ExecuteInEditMode]
	[AddComponentMenu("")]

 public class PathManager : MonoBehaviour
 {
	 

	 [SerializeField]
	 public bool activate;
	 [Space(20)] 

	 [SerializeField]

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
	         
		       
		       
						 go = GameObject.CreatePrimitive(PrimitiveType.Sphere);	
			
					 if (Physics.Raycast(ray, out hit))
					 {
						 offsetting = new Vector3(hit.point.x +offset.x,hit.point.y +offset.y,hit.point.z +offset.z );
						 go.transform.position = offsetting;
						 Debug.Log("Instantiated Primitive at Vector " + hit.point);
					 }

				 go.transform.localScale = new Vector3 (1, 1, 1);
 
				 }

				 e.Use();
			 }
		 }
	}

#endif*/
}