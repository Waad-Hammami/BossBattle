namespace PivecLabs.Tools
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.UI;
	using UnityEditor;

[CustomEditor(typeof(InGameConsole))]
[CanEditMultipleObjects]

public class InGameConsoleEditor : Editor 
{
	private int headerWidthCorrectionForScaling = 38;
	public string headerFlexibleStyle = "Box";

	private Texture2D header;

	public Color backgroundColorByDefault;

	
	SerializedProperty adminMode;
	SerializedProperty consoleCanvas;
	SerializedProperty sysinfoPanel;
	SerializedProperty consoleText;
	SerializedProperty inputText;
	SerializedProperty consoleInput;
	SerializedProperty disableUpdates;
	SerializedProperty disableCommands;
	SerializedProperty disableLogs;
	SerializedProperty selectedKey;

	void OnEnable()
	{
			backgroundColorByDefault = EditorGUIUtility.isProSkin
				? new Color(1, 1, 1, 255)
				: Color.white;

		header = Resources.Load ("piveclabs") as Texture2D;

		adminMode = serializedObject.FindProperty("adminMode");
		disableUpdates = serializedObject.FindProperty("disableUpdates");
		disableCommands = serializedObject.FindProperty("disableCommands");
		disableLogs = serializedObject.FindProperty("disableLogs");
		
		consoleCanvas = serializedObject.FindProperty("consoleCanvas");
		sysinfoPanel = serializedObject.FindProperty("sysinfoPanel");
		consoleText = serializedObject.FindProperty("consoleText");
		inputText = serializedObject.FindProperty("inputText");
		consoleInput = serializedObject.FindProperty("consoleInput");
		
		selectedKey = serializedObject.FindProperty("selectedKey");

}

	public override void OnInspectorGUI()
	{
		DrawEditorByDefaultWithHeaderAndHelpBox();

		serializedObject.Update();
		EditorGUILayout.Space();
		EditorGUILayout.PropertyField(selectedKey,new GUIContent("Toggle Console Key"));
		EditorGUILayout.Space();

		EditorGUILayout.PropertyField(adminMode,new GUIContent("Select Admin mode"));
		EditorGUILayout.Space();
		serializedObject.ApplyModifiedProperties();
		if (adminMode.boolValue == true)
		{
			EditorGUI.indentLevel++;
			EditorGUILayout.PropertyField(disableCommands,new GUIContent("Disable Commands"));
			EditorGUILayout.PropertyField(disableUpdates,new GUIContent("Disable Updates"));
			EditorGUILayout.PropertyField(disableLogs,new GUIContent("Disable Error Logs"));
			EditorGUI.indentLevel--;
		}
		EditorGUILayout.Space();
		EditorGUILayout.LabelField("UI Configuration");
		EditorGUILayout.Space();

		EditorGUI.indentLevel++;
		EditorGUILayout.PropertyField(consoleCanvas);
		EditorGUILayout.PropertyField(sysinfoPanel);
		EditorGUILayout.PropertyField(consoleText);
		EditorGUILayout.PropertyField(inputText);
		EditorGUILayout.PropertyField(consoleInput);
		EditorGUI.indentLevel--;
		EditorGUILayout.Space();

		serializedObject.ApplyModifiedProperties();
	}
	public void DrawEditorByDefaultWithHeaderAndHelpBox()
	{

		DrawHeaderFlexible(header, Color.black);

		GUI.backgroundColor = backgroundColorByDefault;

		DrawHelpBox();

	}

	public void DrawHeaderFlexible(Texture2D header, Color backgroundColor)
	{
		if (header)
		{
			GUI.backgroundColor = backgroundColor;

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
		GUI.backgroundColor = backgroundColorByDefault;

		LinkButton ("https://docs.piveclabs.com");
			
				
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