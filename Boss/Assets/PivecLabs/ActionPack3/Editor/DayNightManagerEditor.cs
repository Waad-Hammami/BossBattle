namespace PivecLabs.ActionPack
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.UI;
	using GameCreator.Core;
	using GameCreator.Localization;
	using UnityEditor;


	[CustomEditor(typeof(DayNightManager))]
	public class DayNightManagerEditor : Editor
	{

		private int headerWidthCorrectionForScaling = 38;
		public string headerFlexibleStyle = "Box";
		private Texture2D header;
		public Color backgroundColorByDefault;


		SerializedProperty usingSystemTime;
		SerializedProperty usingRealTimeSpeed;
		SerializedProperty randomizeStartTime;
		SerializedProperty setStartTimeString;
		SerializedProperty setStartTime;
		SerializedProperty setStartDay;
		SerializedProperty varFormatTime;
		SerializedProperty varFormatDay;
		SerializedProperty setStartDayString;
		SerializedProperty dayAsMinutes;
		SerializedProperty timeCurve;
		SerializedProperty ambientNight;
		SerializedProperty ambientDay;
		SerializedProperty sunColor;
		SerializedProperty sunObject;
		SerializedProperty sunSeason;
		SerializedProperty sunCurve;
		SerializedProperty moonObject;
		SerializedProperty skyboxRotate;
		SerializedProperty skyboxRotateSpeed;
		SerializedProperty savetotalDays;
		SerializedProperty totaldaysVariable;
		
		void OnEnable()
		{
			header = Resources.Load("piveclabs") as Texture2D;

			usingSystemTime = serializedObject.FindProperty("usingSystemTime");
			usingRealTimeSpeed = serializedObject.FindProperty("usingRealTimeSpeed");
			randomizeStartTime = serializedObject.FindProperty("randomizeStartTime");
			varFormatTime = serializedObject.FindProperty("varFormatTime");
			varFormatDay = serializedObject.FindProperty("varFormatDay");
			setStartTime = serializedObject.FindProperty("setStartTime");
			setStartDay = serializedObject.FindProperty("setStartDay");
			setStartTimeString = serializedObject.FindProperty("setStartTimeString");
			setStartDayString = serializedObject.FindProperty("setStartDayString");
			dayAsMinutes = serializedObject.FindProperty("dayAsMinutes");
			timeCurve = serializedObject.FindProperty("timeCurve");
			sunObject = serializedObject.FindProperty("sunObject");
			sunColor = serializedObject.FindProperty("sunColor");
			ambientDay = serializedObject.FindProperty("ambientDay");
			ambientNight = serializedObject.FindProperty("ambientNight");
			sunSeason = serializedObject.FindProperty("sunSeason");
			sunCurve = serializedObject.FindProperty("sunCurve");
			moonObject = serializedObject.FindProperty("moonObject");
			skyboxRotate = serializedObject.FindProperty("skyboxRotate");
			skyboxRotateSpeed = serializedObject.FindProperty("skyboxRotateSpeed");
			savetotalDays = serializedObject.FindProperty("savetotalDays");
			totaldaysVariable = serializedObject.FindProperty("totaldaysVariable");
		}
		public override void OnInspectorGUI()
		{
			DrawEditorByDefaultWithHeaderAndHelpBox();


			serializedObject.Update();
			EditorGUILayout.Space();

			EditorGUILayout.LabelField("Time Settings", EditorStyles.boldLabel);
			EditorGUI.indentLevel++;
			EditorGUILayout.PropertyField(usingSystemTime, new GUIContent("Use System Time"));
			if (usingSystemTime.boolValue == false)
			{
				EditorGUI.indentLevel++;
				EditorGUILayout.PropertyField(randomizeStartTime, new GUIContent("Random Start Time"));
				EditorGUILayout.Space();

				if (randomizeStartTime.boolValue == false)
				{
					EditorGUI.indentLevel--;
					EditorGUILayout.LabelField("Set Start Time", EditorStyles.boldLabel);
					EditorGUI.indentLevel++;
					EditorGUILayout.PropertyField(varFormatTime,new GUIContent("String or Number"));

					switch (varFormatTime.enumValueIndex)
					{
				
					case 0:
						EditorGUILayout.PropertyField(setStartTimeString, new GUIContent("Start Time String"));
						break;
					case 1:
						EditorGUILayout.PropertyField(setStartTime, new GUIContent("Start Time Number"));
						break;
					}
					
					EditorGUI.indentLevel--;
					EditorGUILayout.LabelField("Set Start Day", EditorStyles.boldLabel);
					EditorGUI.indentLevel++;
					EditorGUILayout.PropertyField(varFormatDay,new GUIContent("String or Number"));

					switch (varFormatDay.enumValueIndex)
					{
				
					case 0:
						EditorGUILayout.PropertyField(setStartDayString, new GUIContent("Start Day String"));
						break;
					case 1:
						EditorGUILayout.PropertyField(setStartDay, new GUIContent("Start Day Number"));
						break;
					}
					
	
				
				}
				EditorGUI.indentLevel--;
			}
			EditorGUILayout.Space();

			EditorGUILayout.PropertyField(usingRealTimeSpeed, new GUIContent("Use Real Time Speed"));
			if (usingRealTimeSpeed.boolValue == false)
			{
				EditorGUI.indentLevel++;
				EditorGUILayout.PropertyField(dayAsMinutes, new GUIContent("Minutes per Day"));
				EditorGUI.indentLevel--;
			}
			EditorGUILayout.Space();
			EditorGUILayout.PropertyField(sunCurve, new GUIContent("Use Day/Night Curve"));		

			if (sunCurve.boolValue == true)
			{
				EditorGUI.indentLevel++;
				EditorGUILayout.Space();
				EditorGUILayout.PropertyField(timeCurve, new GUIContent("Sun Curve"));

				EditorGUI.indentLevel--;
			}
			EditorGUILayout.Space();
			EditorGUILayout.Space();
			EditorGUILayout.PropertyField(sunSeason, new GUIContent("Season"));
			EditorGUI.indentLevel--;
			
			Rect position = EditorGUILayout.GetControlRect(false, 2 * EditorGUIUtility.singleLineHeight);
			position.height *= 0.5f;

			position.y += position.height - 20;
			position.x += EditorGUIUtility.labelWidth - 10;
			position.width -= EditorGUIUtility.labelWidth + 26;
			GUIStyle style = GUI.skin.label;
			style.fontSize = 10;
			style.alignment = TextAnchor.UpperLeft; EditorGUI.LabelField(position, "Summer", style);
			style.alignment = TextAnchor.UpperCenter; EditorGUI.LabelField(position, "Spring/Autumn", style);
			style.alignment = TextAnchor.UpperRight; EditorGUI.LabelField(position, "Winter", style);
			EditorGUI.indentLevel++;
			EditorGUILayout.PropertyField(savetotalDays, new GUIContent("Save Day Count"));
			EditorGUI.indentLevel++;
			if (savetotalDays.boolValue == true)
			{
				EditorGUILayout.PropertyField(totaldaysVariable, new GUIContent("Total Days Variable"));

			}
			EditorGUI.indentLevel--;
			EditorGUI.indentLevel--;
			EditorGUILayout.Space();
			EditorGUI.indentLevel++;
			EditorGUILayout.PropertyField(skyboxRotate, new GUIContent("Rotate Skybox"));
			EditorGUI.indentLevel++;
			if (skyboxRotate.boolValue == true)
			{
				EditorGUILayout.PropertyField(skyboxRotateSpeed, new GUIContent("Rotate Speed"));

			}
			EditorGUI.indentLevel--;
			EditorGUI.indentLevel--;
			EditorGUILayout.Space();

			var rect1 = EditorGUILayout.BeginHorizontal();
			Handles.color = Color.gray;
			Handles.DrawLine(new Vector2(rect1.x - 1, rect1.y), new Vector2(rect1.width + 15, rect1.y));
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.Space();
			EditorGUILayout.LabelField("Visual Settings", EditorStyles.boldLabel);
			EditorGUILayout.Space();

			EditorGUI.indentLevel++;
			EditorGUILayout.PropertyField(sunObject, new GUIContent("Sun Prefab"));
			EditorGUILayout.PropertyField(sunColor, new GUIContent("Sun Color"));
			EditorGUILayout.PropertyField(ambientDay, new GUIContent("Day Ambient Color"));
			EditorGUILayout.PropertyField(ambientNight, new GUIContent("Night Ambient Color"));
			EditorGUILayout.PropertyField(moonObject, new GUIContent("Optional Moon Prefab"));

			EditorGUI.indentLevel--;
			EditorGUILayout.Space();

			var rect2 = EditorGUILayout.BeginHorizontal();
			Handles.color = Color.gray;
			Handles.DrawLine(new Vector2(rect2.x - 1, rect2.y), new Vector2(rect2.width + 15, rect2.y));
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.Space();
			EditorGUILayout.LabelField("Action Settings", EditorStyles.boldLabel);			EditorGUILayout.Space();

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