namespace PivecLabs.ActionPack
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.UI;
	using GameCreator.Core;
	using GameCreator.Localization;
	using PivecLabs.MinMaxSliderFloat;

	using UnityEditor;


	[CustomEditor(typeof(AudioVisualizer))]
	public class AudioVisualizerEditor : Editor
	{

		private int headerWidthCorrectionForScaling = 38;
		public string headerFlexibleStyle = "Box";
		private Texture2D header;
		public Color backgroundColorByDefault;


		SerializedProperty audioSource;
		SerializedProperty visualizerColour;
		SerializedProperty MinMaxHieght;
		SerializedProperty updateSensitivity;


		void OnEnable()
		{
			header = Resources.Load("piveclabs") as Texture2D;

			audioSource = serializedObject.FindProperty("audioSource");
			visualizerColour = serializedObject.FindProperty("visualizerColour");
			MinMaxHieght = serializedObject.FindProperty("MinMaxHieght");
			updateSensitivity = serializedObject.FindProperty("updateSensitivity");
		}

		public override void OnInspectorGUI()
		{
			DrawEditorByDefaultWithHeaderAndHelpBox();


			serializedObject.Update();
			EditorGUILayout.Space();

	
				EditorGUILayout.PropertyField(audioSource, new GUIContent("Audio Source"));
				EditorGUILayout.PropertyField(visualizerColour, new GUIContent("Visual Object Color"));
				EditorGUILayout.PropertyField(MinMaxHieght);
				EditorGUILayout.PropertyField(updateSensitivity);

			serializedObject.ApplyModifiedProperties();


		}
		public void DrawEditorByDefaultWithHeaderAndHelpBox()
		{

			DrawHeaderFlexible(header, Color.black);
			DrawHelpBox();

		}

		public void DrawHeaderFlexible(Texture2D header, Color backgroundColor)
		{
			if (header)
			{
				if (header.width + headerWidthCorrectionForScaling < EditorGUIUtility.currentViewWidth)
				{
					EditorGUILayout.BeginVertical(headerFlexibleStyle);

					DrawHeader(header);

					EditorGUILayout.EndVertical();
				}
				else
				{
					DrawHeaderIfScrollbar(header);
				}
			}
		}

		public void DrawHeaderIfScrollbar(Texture2D header)
		{
			EditorGUI.DrawTextureTransparent(
				GUILayoutUtility.GetRect(
				EditorGUIUtility.currentViewWidth - headerWidthCorrectionForScaling,
				header.height),
				header,
				ScaleMode.ScaleToFit);
		}

		public void DrawHeader(Texture2D header)
		{
			EditorGUI.DrawTextureTransparent(
				GUILayoutUtility.GetRect(
				header.width,
				header.height),
				header,
				ScaleMode.ScaleToFit);
		}


		public void DrawHelpBox()
		{
			LinkButton("https://docs.piveclabs.com");


		}

		private void LinkButton(string url)
		{
			var style = GUI.skin.GetStyle("HelpBox");
			style.richText = true;
			style.alignment = TextAnchor.MiddleCenter;

			bool bClicked = GUILayout.Button("<b>Online Documentation can be found at https://docs.piveclabs.com</b>", style);

			var rect = GUILayoutUtility.GetLastRect();
			EditorGUIUtility.AddCursorRect(rect, MouseCursor.Link);
			if (bClicked)
				Application.OpenURL(url);
		}

	}
}