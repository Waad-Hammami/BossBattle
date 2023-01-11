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
	public class ActionStopDragAnyObjectbyTag : IAction
	{

		public Actions actions;


		public override bool InstantExecute(GameObject target, IAction[] actions, int index)
		{

			var references = this.actions.gameObject.GetComponents<ActionDragAnyObjectbyTag>();
			
			foreach (var reference in references)
			{
				reference.StopDragging();
			}
			return true;

		}

	


		// +--------------------------------------------------------------------------------------+
		// | EDITOR                                                                               |
		// +--------------------------------------------------------------------------------------+

#if UNITY_EDITOR

		public static new string NAME = "ActionPack1/Objects/Stop Drag Any Object by Tag";
		private const string NODE_TITLE = "Stop Drag Any Object by Tag";
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
			this.spallowDragging = null;


		}

		public override void OnInspectorGUI()
		{
			this.serializedObject.Update();
			EditorGUILayout.Space();
			EditorGUILayout.PropertyField(this.spallowDragging, new GUIContent("Allow Dragging Any by Tag"));
			EditorGUILayout.Space();

			this.serializedObject.ApplyModifiedProperties();
		}

#endif
	}
}