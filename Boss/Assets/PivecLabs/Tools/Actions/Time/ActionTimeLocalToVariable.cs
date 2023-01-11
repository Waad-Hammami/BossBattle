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
	public class ActionTimeLocalToVariable : IAction
    {
    
    
	    [VariableFilter(Variable.DataType.String)]
	    public VariableProperty storedLocalTime = new VariableProperty(Variable.VarType.GlobalVariable);

	    public bool encryptVar;
	    public string encryptKey = "abcdefghijklmnopqrstuvwxyz123456";

        // EXECUTABLE: ----------------------------------------------------------------------------
        public override bool InstantExecute(GameObject target, IAction[] actions, int index)
        {
		       
	        if (encryptVar == true)
	        {	
		        Double localtime = LocalTime();
		        byte[] encrypted = encrypting(localtime.ToString(), encryptKey);
		        string encryptedString = System.Convert.ToBase64String(encrypted);
		        this.storedLocalTime.Set(encryptedString);	 
	        }
	        else
	        {

		        Double localtime = LocalTime();
		        DateTime dateTime1 = new DateTime(1900, 1, 1).AddMilliseconds(localtime);
		        this.storedLocalTime.Set(localtime.ToString());	 
	        }
       

            return true;
       
        }
        
	    public static double LocalTime ()
	    {
	
				    
		    string rounded = (DateTime.UtcNow.Subtract (new DateTime (1900, 1, 1)).TotalMilliseconds).ToString("F0");
		    double time1 = Convert.ToInt64(rounded);
		    return time1;
		   

	    }

	    public static byte[] encrypting(string toEncrypt, string key)
	    {
		    try
		    {
			    byte[] keyArray = Encoding.UTF8.GetBytes(key);
			    byte[] toEncryptArray = Encoding.UTF8.GetBytes(toEncrypt);
			    RijndaelManaged rManaged = new RijndaelManaged();
			    rManaged.Key = keyArray;
			    rManaged.Mode = CipherMode.ECB;
			    rManaged.Padding = PaddingMode.ISO10126;
			    ICryptoTransform cTransform = rManaged.CreateEncryptor();
			    return cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
		    }
			    catch
			    {
				    Debug.Log("Encrypting Failed");
			    }
		    return null;
	    }

        // +--------------------------------------------------------------------------------------+
        // | EDITOR                                                                               |
        // +--------------------------------------------------------------------------------------+

#if UNITY_EDITOR

	    public const string CUSTOM_ICON_PATH = "Assets/PivecLabs/Tools/Icons/";
	    public static new string NAME = "Developer Tools/Time/Time Local to Variable";
	    private const string NODE_TITLE = "Time Local to Variable";

        // PROPERTIES: ----------------------------------------------------------------------------

        private SerializedProperty spstoredLocalTime;
	    private SerializedProperty spencryptVar;
	    private SerializedProperty spencryptKey;

	    // INSPECTOR METHODS: ---------------------------------------------------------------------

        public override string GetNodeTitle()
        {
            return string.Format(NODE_TITLE);
        }

        protected override void OnEnableEditorChild()
        {
            this.spstoredLocalTime = this.serializedObject.FindProperty("storedLocalTime");
	        this.spencryptVar = this.serializedObject.FindProperty("encryptVar");
	        this.spencryptKey = this.serializedObject.FindProperty("encryptKey");
        }

        protected override void OnDisableEditorChild()
        {
            this.spstoredLocalTime = null;
	        this.spencryptVar = null;
	        this.spencryptKey = null;
        }

        public override void OnInspectorGUI()
        {
            this.serializedObject.Update();
           EditorGUILayout.Space();
	        EditorGUILayout.PropertyField(spencryptVar,new GUIContent("Encrypt Local Time Variable"));
	        if (encryptVar == true)
	        {
		        EditorGUI.indentLevel++;
		        EditorGUILayout.PropertyField(spencryptKey,new GUIContent("Encryption Key"));
		        EditorGUILayout.LabelField(new GUIContent("Must be 32 characters/numbers"));

		        EditorGUI.indentLevel--;
	        }
	        EditorGUILayout.Space();
	        EditorGUILayout.PropertyField(spstoredLocalTime,new GUIContent("Local Time Variable"));

            EditorGUILayout.Space();
    

            this.serializedObject.ApplyModifiedProperties();
        }

#endif
    }
}