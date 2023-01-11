namespace PivecLabs.Tools
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.Events;
	using UnityEngine.EventSystems;
	using UnityEngine.AI;
	using UnityEngine.SceneManagement;
	using System.ComponentModel;
	using System.Security.Cryptography;
	using System.Text;
	using UnityEngine.UI;

	using System;	
	using System.Net;
	using System.Net.Sockets;
	using GameCreator.Core;
	using GameCreator.Core.Hooks;
	using GameCreator.Characters;
	using GameCreator.Variables;

#if UNITY_EDITOR
	using UnityEditor;
	#endif

	[AddComponentMenu("")]
	public class ConditionTime : ConditionVariable
	{
		[VariableFilter(Variable.DataType.String)]
		public VariableProperty storedTime1 = new VariableProperty(Variable.VarType.GlobalVariable);
		[VariableFilter(Variable.DataType.String)]
		public VariableProperty storedTime2 = new VariableProperty(Variable.VarType.GlobalVariable);

		public bool encryptVar1;
		public string encryptKey1 = "abcdefghijklmnopqrstuvwxyz123456";
		public bool encryptVar2;
		public string encryptKey2 = "abcdefghijklmnopqrstuvwxyz123456";
		public bool satisfied = true;
		
		[Range(0,2)]
		public int tolerance = 0;
		private int tValue;
		
		// EXECUTABLE: ----------------------------------------------------------------------------
		
		protected override bool Compare(GameObject target)
		{
			
			string var1;
			string var2;
			
			switch (tolerance)
			{
			case 0:
				tValue = 0;
				break;
			case 1:
				tValue = 3;
				break;
			case 2:
				tValue = 4;
				break;
		
			}
			
			if (encryptVar1 == true)
			{
				byte[] encryptedBytes = System.Convert.FromBase64String(this.storedTime1.ToStringValue(target));
            
				var1 = decrypting(encryptedBytes, encryptKey1);
				
			}
			else
			{
				var1 = (string)this.storedTime1.Get(target);
			}
			
			if (encryptVar2 == true)
			{
				byte[] encryptedBytes = System.Convert.FromBase64String(this.storedTime2.ToStringValue(target));
            
				var2 = decrypting(encryptedBytes, encryptKey2);
				
			}
			else
			{
				var2 = (string)this.storedTime2.Get(target);
			}		
			
				
			var1 = var1.Substring(0, var1.Length - tValue);
			var2 = var2.Substring(0, var2.Length - tValue);

			double time1 = Convert.ToInt64(var1);
			double time2 = Convert.ToInt64(var2);
			
			if ((time2-time1) < 2)
				satisfied = true;
			else 
				satisfied = false;
		
			return this.satisfied;
		}


		public static string decrypting(byte[] toEncryptArray, string key)
		{
			try
			{
            
				byte[] keyArray = Encoding.UTF8.GetBytes(key);
				RijndaelManaged rManaged = new RijndaelManaged();
				rManaged.Key = keyArray;
				rManaged.Mode = CipherMode.ECB;
				rManaged.Padding = PaddingMode.ISO10126;
				ICryptoTransform cTransform = rManaged.CreateDecryptor();
				byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
				return Encoding.UTF8.GetString(resultArray);
			}
				catch
            
				{
					Debug.Log("Decrypting Failed");
				}
			return null;
		}
		// +--------------------------------------------------------------------------------------+
		// | EDITOR                                                                               |
		// +--------------------------------------------------------------------------------------+

		#if UNITY_EDITOR

		public static new string NAME = "Developer Tools/Time Comparison";
		public override string GetNodeTitle()
		{
			return string.Format(
				NODE_TITLE,
				this.storedTime1,
				this.storedTime2
			);
		}
		// PROPERTIES: ----------------------------------------------------------------------------

		private SerializedProperty spSatisfied;
		private SerializedProperty spvar1;
		private SerializedProperty spvar2;
		private SerializedProperty sptolerance;
		private SerializedProperty spencryptVar1;
		private SerializedProperty spencryptKey1;
		private SerializedProperty spencryptVar2;
		private SerializedProperty spencryptKey2;

		// INSPECTOR METHODS: ---------------------------------------------------------------------



		protected override void OnEnableEditorChild ()
		{
			this.spSatisfied = this.serializedObject.FindProperty("satisfied");
			this.spvar1 = this.serializedObject.FindProperty("storedTime1");
			this.spvar2 = this.serializedObject.FindProperty("storedTime2");
			this.sptolerance = this.serializedObject.FindProperty("tolerance");
			this.spencryptVar1 = this.serializedObject.FindProperty("encryptVar1");
			this.spencryptKey1 = this.serializedObject.FindProperty("encryptKey1");
			this.spencryptVar2 = this.serializedObject.FindProperty("encryptVar2");
			this.spencryptKey2 = this.serializedObject.FindProperty("encryptKey2");
		}
	

		public override void OnInspectorGUI()
		{
			this.serializedObject.Update();
			EditorGUILayout.PropertyField(this.spvar1, new GUIContent("Time Variable 1"));
			EditorGUILayout.Space();
			EditorGUILayout.PropertyField(spencryptVar1,new GUIContent("Encrypted Variable"));

			if (encryptVar1 == true)
			{
				EditorGUI.indentLevel++;
				EditorGUILayout.PropertyField(spencryptKey1,new GUIContent("Decryption Key"));
				EditorGUILayout.LabelField(new GUIContent("Must be identical to Encryption Key"));

				EditorGUI.indentLevel--;
			}
			EditorGUILayout.Space();

			EditorGUILayout.PropertyField(this.spvar2, new GUIContent("Time Variable 2"));
			EditorGUILayout.Space();
			EditorGUILayout.PropertyField(spencryptVar2,new GUIContent("Encrypted Variable"));

			if (encryptVar2 == true)
			{
				EditorGUI.indentLevel++;
				EditorGUILayout.PropertyField(spencryptKey2,new GUIContent("Decryption Key"));
				EditorGUILayout.LabelField(new GUIContent("Must be identical to Encryption Key"));

				EditorGUI.indentLevel--;
			}
			EditorGUILayout.Space();
			EditorGUILayout.PropertyField(this.sptolerance, new GUIContent("Tolerance"));
			Rect position = EditorGUILayout.GetControlRect(false, 2 * EditorGUIUtility.singleLineHeight); 
			position.height *= 0.5f;
 			position.y += position.height - 20;
			position.x += EditorGUIUtility.labelWidth -10;
			position.width -= EditorGUIUtility.labelWidth + 26; 
			GUIStyle style = GUI.skin.label;
			style.fontSize = 10;
			style.alignment = TextAnchor.UpperLeft; EditorGUI.LabelField(position, "Zero", style);
			style.alignment = TextAnchor.UpperCenter; EditorGUI.LabelField(position, "1 Sec", style);
			style.alignment = TextAnchor.UpperRight; EditorGUI.LabelField(position, "10 Sec", style);
			
		
			EditorGUILayout.PropertyField(this.spSatisfied, new GUIContent("Is Condition Satisfied?"));

			this.serializedObject.ApplyModifiedProperties();
		}

		#endif
	}
}