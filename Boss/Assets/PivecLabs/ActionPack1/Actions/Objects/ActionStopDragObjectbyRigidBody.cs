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
	public class ActionStopDragObjectbyRigidBody : IAction
	{

		public Actions actions;


		public override bool InstantExecute(GameObject target, IAction[] actions, int index)
		{

			var references = this.actions.gameObject.GetComponents<ActionDragObjectbyRigidBody>();
			
			foreach (var reference in references)
			{
				reference.StopRBDragging();
			}
			return true;

		}

	


		// +--------------------------------------------------------------------------------------+
		// | EDITOR                                                                               |
		// +--------------------------------------------------------------------------------------+

#if UNITY_EDITOR

		public static new string NAME = "ActionPack1/Objects/Stop Drag Object with RigidBody";
		private const string NODE_TITLE = "Stop Drag Object with RigidBody";
		public const string CUSTOM_ICON_PATH = "Assets/PivecLabs/ActionPack1/Icons/";

		// PROPERTIES: ----------------------------------------------------------------------------

		private SerializedProperty spallowDragging;

		// INSPECTOR METHODS: ---------------------------------------------------------------------

		public override string GetNodeTitle()
		{

			return string.Format(NODE_TITLE);
		}


		protected override void OnEnableEditorChild()
		{
				this.spallowDragging = this.serializedObject.FindProperty("actions");

		}

		protected override void OnDisableEditorChild()
		{
			//		this.spobjectToDrag = null;
			this.spallowDragging = null;


		}

		public override void OnInspectorGUI()
		{
			this.serializedObject.Update();
			EditorGUILayout.Space();
			EditorGUILayout.PropertyField(this.spallowDragging, new GUIContent("Allow RigidBody Dragging Action"));
			EditorGUILayout.Space();

			this.serializedObject.ApplyModifiedProperties();
		}

#endif
	}
}