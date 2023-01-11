namespace PivecLabs.Tools
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.Events;
	using GameCreator.Core;
	using GameCreator.Core.Hooks;
	using GameCreator.Variables;

	#if UNITY_EDITOR
	using UnityEditor;
	#endif

	[AddComponentMenu("")]
	public class ActionWaitFromVariable : IAction 
	{
		public NumberProperty waitTime = new NumberProperty(0.0f);
        private bool forceStop = false;

		// EXECUTABLE: ----------------------------------------------------------------------------
		
        public override IEnumerator Execute(GameObject target, IAction[] actions, int index)
		{
            this.forceStop = false;
            float waitTimeValue = this.waitTime.GetValue(target);
            float stopTime = Time.time + waitTimeValue;
            WaitUntil waitUntil = new WaitUntil(() => Time.time > stopTime || this.forceStop);
            yield return waitUntil;
			yield return 0;
		}

        public override void Stop()
        {
            this.forceStop = true;
        }

        // +--------------------------------------------------------------------------------------+
        // | EDITOR                                                                               |
        // +--------------------------------------------------------------------------------------+

        #if UNITY_EDITOR
		public const string CUSTOM_ICON_PATH = "Assets/Plugins/GameCreator/Transitions/Icons/Actions/";


		public static new string NAME = "Developer Tools/Time/Wait Time from Variable";
		private const string NODE_TITLE = "Wait Time from Variable";

		// PROPERTIES: ----------------------------------------------------------------------------

		private SerializedProperty spWaitTime;

		// INSPECTOR METHODS: ---------------------------------------------------------------------

		public override string GetNodeTitle()
		{
			return NODE_TITLE;
		}

		protected override void OnEnableEditorChild()
		{
			this.spWaitTime = this.serializedObject.FindProperty("waitTime");
		}

		protected override void OnDisableEditorChild()
		{
			this.spWaitTime = null;
		}

		public override void OnInspectorGUI()
		{
			this.serializedObject.Update();

			EditorGUILayout.PropertyField(this.spWaitTime, new GUIContent("Wait Time Variable"));

			this.serializedObject.ApplyModifiedProperties();
		}

		#endif
	}
}