namespace PivecLabs.Tools
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.UI;
	using GameCreator.Core;
	using GameCreator.Variables;
	using GameCreator.Localization;
	using UnityEditor;
	
[ExecuteInEditMode]

[CustomEditor(typeof(ClicktoSpawnEditorMode))]
	public class ClicktoSpawnEditorModeEditor : Editor 
{

	private int headerWidthCorrectionForScaling = 38;
	public string headerFlexibleStyle = "Box";

	private Texture2D header;

	public Color backgroundColorByDefault;

	
	SerializedProperty activate;
	SerializedProperty useprefab;
	SerializedProperty prefabToUse;
	SerializedProperty prefabSize;
	SerializedProperty useprimitive;
	SerializedProperty primitiveType;
	SerializedProperty primitiveSize;
	SerializedProperty offset;
	SerializedProperty mouseButton;
    
	void OnEnable()
	{
		backgroundColorByDefault = EditorGUIUtility.isProSkin
			? new Color(1, 1, 1, 255)
			: Color.white;

		header = Resources.Load ("piveclabs") as Texture2D;

		activate = serializedObject.FindProperty("activate");
		useprefab = serializedObject.FindProperty("useprefab");
		prefabToUse = serializedObject.FindProperty("prefabToUse");
		prefabSize = serializedObject.FindProperty("prefabSize");
		useprimitive = serializedObject.FindProperty("useprimitive");
		primitiveType = serializedObject.FindProperty("primitiveType");
		primitiveSize = serializedObject.FindProperty("primitiveSize");
		offset = serializedObject.FindProperty("offset");
		mouseButton = serializedObject.FindProperty("mouseButton");
		EditorUtility.SetDirty(this);

}

	public override void OnInspectorGUI()
	{
		
		DrawEditorByDefaultWithHeaderAndHelpBox();

		serializedObject.Update();
		EditorGUILayout.Space();
		EditorGUILayout.PropertyField(activate,new GUIContent("Activate/Deactivate"));

		EditorGUILayout.PropertyField(mouseButton,new GUIContent("Mouse Button"));
		Rect position = EditorGUILayout.GetControlRect(false, 2 * EditorGUIUtility.singleLineHeight); 
		position.height *= 0.5f;
           
		position.y += position.height - 20;
		position.x += EditorGUIUtility.labelWidth -10;
		position.width -= EditorGUIUtility.labelWidth + 26; 
		GUIStyle style = GUI.skin.label;
		style.fontSize = 10;
		style.alignment = TextAnchor.UpperLeft; EditorGUI.LabelField(position, "Left", style);
		style.alignment = TextAnchor.UpperCenter; EditorGUI.LabelField(position, "Right", style);
		style.alignment = TextAnchor.UpperRight; EditorGUI.LabelField(position, "Middle", style);
		

		if (activate.boolValue == true)
		{
		
			EditorGUILayout.PropertyField(useprefab,new GUIContent("Use Prefab"));
			EditorGUILayout.Space();
			if (useprefab.boolValue == true)
			{
				EditorGUI.indentLevel++;
				EditorGUILayout.PropertyField(prefabToUse,new GUIContent("Prefab Object"));
				EditorGUILayout.PropertyField(prefabSize,new GUIContent("Prefab Size"));
				EditorGUI.indentLevel--;
				useprimitive.boolValue = false;
			}
			EditorGUILayout.LabelField("or...");
			EditorGUILayout.Space();
			EditorGUILayout.PropertyField(useprimitive,new GUIContent("Use Primitive"));
			if (useprimitive.boolValue == true)
			{
				EditorGUI.indentLevel++;
				EditorGUILayout.PropertyField(primitiveType,new GUIContent("Primitive Type"));
				EditorGUILayout.PropertyField(primitiveSize,new GUIContent("Primitive Size"));
				EditorGUILayout.PropertyField(offset,new GUIContent("Offset"));
				EditorGUI.indentLevel--;
				useprefab.boolValue = false;

			}

	
		}


		EditorGUILayout.Space();

		serializedObject.ApplyModifiedProperties();
		EditorUtility.SetDirty(this);

	}
	
	public void DrawEditorByDefaultWithHeaderAndHelpBox()
	{

		DrawHeaderFlexible(header, Color.black);

		GUI.backgroundColor = Color.black;
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
		GUI.backgroundColor = Color.yellow;

		LinkButton ("https://docs.piveclabs.com");
			
		GUI.backgroundColor = Color.white;
		
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