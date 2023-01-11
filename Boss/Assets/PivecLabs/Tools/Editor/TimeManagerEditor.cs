namespace PivecLabs.Tools
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.UI;
	using UnityEditor;

	[CustomEditor(typeof(TimeManager))]
	[CanEditMultipleObjects]

	public class TimeManagerEditor : Editor 
{
	private int headerWidthCorrectionForScaling = 38;
	public string headerFlexibleStyle = "Box";

	private Texture2D header;

	public Color backgroundColorByDefault;


	
	SerializedProperty ntpHosts;
	SerializedProperty repeatPing;
	SerializedProperty pingInterval;
	SerializedProperty storedNtpTime;
	SerializedProperty storeVar;
	SerializedProperty startNtpTime;
	SerializedProperty startVar;
	SerializedProperty LastUpdateTime;
	SerializedProperty encryptKey;
	SerializedProperty encryptVar;
	SerializedProperty waitAtStart;



	void OnEnable()
	{
			backgroundColorByDefault = EditorGUIUtility.isProSkin
				? new Color(1, 1, 1, 255)
				: Color.white;

		header = Resources.Load ("piveclabs") as Texture2D;

		ntpHosts = serializedObject.FindProperty("ntpH");
		repeatPing = serializedObject.FindProperty("repeatPing");
		pingInterval = serializedObject.FindProperty("pingInterval");
		storeVar = serializedObject.FindProperty("storeVar");
		storedNtpTime = serializedObject.FindProperty("storedNtpTime");
		startVar = serializedObject.FindProperty("startVar");
		startNtpTime = serializedObject.FindProperty("startNtpTime");
		LastUpdateTime = serializedObject.FindProperty("LastUpdateTime");
		encryptVar = serializedObject.FindProperty("encryptVar");
		encryptKey = serializedObject.FindProperty("encryptKey");
		waitAtStart = serializedObject.FindProperty("waitAtStart");



}

	public override void OnInspectorGUI()
	{
		DrawEditorByDefaultWithHeaderAndHelpBox();

		serializedObject.Update();
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		EditorGUILayout.PropertyField(ntpHosts,new GUIContent("Ntp Host URL"));
		EditorGUILayout.Space();
		EditorGUILayout.PropertyField(encryptVar,new GUIContent("Encrypt Variables"));
		EditorGUILayout.Space();
		if (encryptVar.boolValue == true)
		{
			EditorGUI.indentLevel++;
			EditorGUILayout.PropertyField(encryptKey,new GUIContent("Encryption Key"));
			EditorGUILayout.LabelField(new GUIContent("Must be 32 characters/numbers"));

			EditorGUI.indentLevel--;
		}
		EditorGUILayout.Space();

		EditorGUILayout.PropertyField(startVar,new GUIContent("Store Initial Time"));

		EditorGUILayout.Space();
		if (startVar.boolValue == true)
		{
			EditorGUI.indentLevel++;
			EditorGUILayout.PropertyField(startNtpTime,new GUIContent("Initial Time Variable"));
			EditorGUI.indentLevel--;
		}

		EditorGUILayout.PropertyField(storeVar,new GUIContent("Store Current Time"));
		EditorGUILayout.Space();
		if (storeVar.boolValue == true)
		{
			EditorGUI.indentLevel++;
			EditorGUILayout.PropertyField(storedNtpTime,new GUIContent("Current Time Variable"));
			EditorGUI.indentLevel--;
		}

		EditorGUILayout.PropertyField(repeatPing,new GUIContent("Repeating Ntp Ping"));
		EditorGUILayout.Space();
		if (repeatPing.boolValue == true)
		{
			EditorGUI.indentLevel++;
			EditorGUILayout.PropertyField(pingInterval,new GUIContent("Ping Interval (sec)"));	
			EditorGUI.indentLevel--;
		}
		EditorGUILayout.Space();

		EditorGUILayout.Space();

		var rect2 = EditorGUILayout.BeginHorizontal();
		Handles.color = Color.gray;
		Handles.DrawLine(new Vector2(rect2.x - 1, rect2.y), new Vector2(rect2.width + 15, rect2.y));
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.Space();
		EditorGUILayout.LabelField(new GUIContent("Recurring Actions"));

		EditorGUI.indentLevel++;
		EditorGUILayout.PropertyField(LastUpdateTime,new GUIContent("Last Update Variable"));
		EditorGUILayout.PropertyField(waitAtStart,new GUIContent("Wait after Start (sec)"));
		EditorGUI.indentLevel--;
		
		EditorGUILayout.Space();

		EditorGUILayout.LabelField("Action Settings");

		EditorGUI.indentLevel++;


		SerializedProperty property = serializedObject.FindProperty("ListofActions");
		ArrayGUI(property, "Action ", true);
		EditorGUILayout.Space();
  
		EditorGUI.indentLevel--;


		serializedObject.ApplyModifiedProperties();
	}
	

	private void ArrayGUI(SerializedProperty property, string itemType, bool visible)
	{

		{

			EditorGUI.indentLevel++;
			SerializedProperty arraySizeProp = property.FindPropertyRelative("Array.size");
			EditorGUILayout.PropertyField(arraySizeProp);
             
			for (int i = 0; i < arraySizeProp.intValue; i++)
			{
				EditorGUILayout.PropertyField(property.GetArrayElementAtIndex(i), new GUIContent(itemType + (i +1).ToString()), true);
                   
			}
                    
	                 
			EditorGUI.indentLevel--;
		}
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