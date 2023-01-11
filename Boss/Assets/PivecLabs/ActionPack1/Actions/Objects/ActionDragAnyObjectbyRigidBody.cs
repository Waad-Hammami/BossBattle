namespace PivecLabs.ActionPack
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

	public class ActionDragAnyObjectbyRigidBody : IAction
	{

		public GameObject objectToDrag;
		public bool allowRBDragging = false;
		public bool allowRBDrag = false;
		public float forceAmount = 500;
		private Rigidbody selectedRigidbody;
		private Vector3 originalScreenTargetPosition;
		private Vector3 originalRigidbodyPos;
		private float selectionDistance;

		[SerializeField]
		[Range(0, 2)]
		public int mouseButton = 0;

		public override bool InstantExecute(GameObject target, IAction[] actions, int index)
		{
			if (allowRBDragging == true)
			{
				allowRBDrag = true;
			}
				
			else 
			{
				allowRBDrag = false;
				selectedRigidbody = null;
			}
			
			Debug.Log(allowRBDrag);
				
			return true;

		}


		Rigidbody GetRigidbodyFromMouseClick()
		{
			RaycastHit hitInfo = new RaycastHit();
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			bool hit = Physics.Raycast(ray, out hitInfo);
			if (hit)
			{
				if (hitInfo.collider.gameObject.GetComponent<Rigidbody>())
				{
					selectionDistance = Vector3.Distance(ray.origin, hitInfo.point);
					originalScreenTargetPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, selectionDistance));
					originalRigidbodyPos = hitInfo.collider.transform.position;
					return hitInfo.collider.gameObject.GetComponent<Rigidbody>();
				}
			}

			return null;
		}

		


		void Update()
		{
		
			if (allowRBDrag == true)
			{
				if (Input.GetMouseButtonDown(mouseButton))
				{
					selectedRigidbody = GetRigidbodyFromMouseClick();
				}

				if (Input.GetMouseButtonUp(mouseButton) && selectedRigidbody)
				{

					selectedRigidbody = null;

				}

			}
		


		}

		void FixedUpdate()
		{
			

			if (selectedRigidbody && allowRBDrag)
			{
				Vector3 mousePositionOffset = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, selectionDistance)) - originalScreenTargetPosition;
				selectedRigidbody.velocity = (originalRigidbodyPos + mousePositionOffset - selectedRigidbody.transform.position) * forceAmount * Time.deltaTime;
			}
		}

		// +--------------------------------------------------------------------------------------+
		// | EDITOR                                                                               |
		// +--------------------------------------------------------------------------------------+

#if UNITY_EDITOR

		public static new string NAME = "ActionPack1/Objects/Drag Any Object by Rigid Body";
		private const string NODE_TITLE = "Drag Any Object by Rigid Body";
		public const string CUSTOM_ICON_PATH = "Assets/PivecLabs/ActionPack1/Icons/";

		// PROPERTIES: ----------------------------------------------------------------------------
		private SerializedProperty spallowRBDragging;

		private SerializedProperty spmouseButton;

		// INSPECTOR METHODS: ---------------------------------------------------------------------

		public override string GetNodeTitle()
		{

			return string.Format(NODE_TITLE);
		}


		protected override void OnEnableEditorChild()
		{
			this.spallowRBDragging = this.serializedObject.FindProperty("allowRBDragging");
			this.spmouseButton = this.serializedObject.FindProperty("mouseButton");

		}

		protected override void OnDisableEditorChild()
		{
			this.spmouseButton = null;
			this.spallowRBDragging = null;


		}

		public override void OnInspectorGUI()
		{
			this.serializedObject.Update();
			EditorGUILayout.Space();
			EditorGUILayout.PropertyField(this.spallowRBDragging, new GUIContent("Allow RB Dragging"));
			EditorGUILayout.Space();
			if (allowRBDragging == true)
			{
				EditorGUILayout.Space();
				EditorGUI.indentLevel++;
				
				EditorGUILayout.PropertyField(spmouseButton, new GUIContent("Mouse Button"));
				Rect position = EditorGUILayout.GetControlRect(false, 2 * EditorGUIUtility.singleLineHeight);
				position.height *= 0.5f;

				position.y += position.height - 10;
				position.x += EditorGUIUtility.labelWidth - 10;
				position.width -= EditorGUIUtility.labelWidth + 26;
				GUIStyle style = GUI.skin.label;
				style.fontSize = 10;
				style.alignment = TextAnchor.UpperLeft; EditorGUI.LabelField(position, "Left", style);
				style.alignment = TextAnchor.UpperCenter; EditorGUI.LabelField(position, "Right", style);
				style.alignment = TextAnchor.UpperRight; EditorGUI.LabelField(position, "Middle", style);
				EditorGUI.indentLevel--;
			}
			this.serializedObject.ApplyModifiedProperties();
		}

#endif
	}
}