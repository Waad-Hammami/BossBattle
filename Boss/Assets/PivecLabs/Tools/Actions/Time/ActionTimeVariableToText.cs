namespace PivecLabs.Tools
{
	using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
	using UnityEngine.UI;
	using System.ComponentModel;

    using System.Security.Cryptography;
    using System.Text;
    using UnityEngine.Events;
    using GameCreator.Core;
    using GameCreator.Variables;

#if UNITY_EDITOR
    using UnityEditor;
	#endif

	[AddComponentMenu("")]
    public class ActionTimeVariableToText : IAction
    {
    
        [VariableFilter(Variable.DataType.String)]
        public VariableProperty targetVariable = new VariableProperty(Variable.VarType.GlobalVariable);
        public Text text;
        public string content = "{0}";
        public string decryptKey = "abcdefghijklmnopqrstuvwxyz123456";

	    public bool encryptVar;
	    public bool formatVar;
	    private string formattedTime;
	    
	    public enum timeFormat
	    {
		    Date,
		    HoursMinutes,
		    HoursMinutesSeconds
		   
	    }

	    public timeFormat formatTime = timeFormat.Date;

        // EXECUTABLE: ----------------------------------------------------------------------------
        public override bool InstantExecute(GameObject target, IAction[] actions, int index)
        {
	        if (encryptVar == true)
	        {	

                 byte[] encryptedBytes = System.Convert.FromBase64String(this.targetVariable.ToStringValue(target));
            
               string decrypted = decrypting(encryptedBytes, decryptKey);

	           System.Double storedtime = Double.Parse(decrypted);	 
		        DateTime dateTime2 = new DateTime(1900, 1, 1).AddMilliseconds(storedtime).ToLocalTime();	    
	           
		        if (formatVar)
		        {
		        	
			        switch (this.formatTime)
			        {
			        case timeFormat.Date:
			      
				        formattedTime = dateTime2.ToString("dd-MM-yyyy");
				        break;
			        case timeFormat.HoursMinutes:
				       
				        formattedTime = dateTime2.ToString("HH:mm");
				        break;
			       
			        case timeFormat.HoursMinutesSeconds:
			       
				        formattedTime = dateTime2.ToString("HH:mm:ss");
				        break;
			        }
		        
			        this.text.text = formattedTime;
		        }
		        
		        else
		        {
		        	this.text.text = dateTime2.ToString();
		        }

		      
	           
            }
	        else
	        {
		        System.Double storedtime = Double.Parse(this.targetVariable.ToStringValue(target));	 

		        DateTime dateTime2 = new DateTime(1900, 1, 1).AddMilliseconds(storedtime).ToLocalTime();
		        
		        if (formatVar)
		        {
		        	
			        switch (this.formatTime)
			        {
			        case timeFormat.Date:
			      
				        formattedTime = dateTime2.ToString("dd-MM-yyyy");
				        break;
			        case timeFormat.HoursMinutes:
				       
				        formattedTime = dateTime2.ToString("HH:mm");
				        break;
			       
			        case timeFormat.HoursMinutesSeconds:
			       
				        formattedTime = dateTime2.ToString("HH:mm:ss");
				        break;
			        }
		        
			        this.text.text = formattedTime;
		        }
		        
		        else
		        {
		        	this.text.text = dateTime2.ToString();
		        }

	        	
	        }



            return true;
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

	    public const string CUSTOM_ICON_PATH = "Assets/PivecLabs/Tools/Icons/";
	    public static new string NAME = "Developer Tools/Time/Time Variable to Text ";
        private const string NODE_TITLE = "Time Variable to Text ";

        // PROPERTIES: ----------------------------------------------------------------------------

        private SerializedProperty sptext;
        private SerializedProperty sptargetVariable;
        private SerializedProperty spKey;
	    private SerializedProperty spencryptVar;
	    private SerializedProperty spformatVar;
	    private SerializedProperty spformatTime;

	    // INSPECTOR METHODS: ---------------------------------------------------------------------

        public override string GetNodeTitle()
        {
            return string.Format(NODE_TITLE);
        }

        protected override void OnEnableEditorChild()
        {
            this.sptext = this.serializedObject.FindProperty("text");
            this.sptargetVariable = this.serializedObject.FindProperty("targetVariable");
            this.spKey = this.serializedObject.FindProperty("decryptKey");
	        this.spencryptVar = this.serializedObject.FindProperty("encryptVar");
	        this.spformatVar = this.serializedObject.FindProperty("formatVar");
	        this.spformatTime = this.serializedObject.FindProperty("formatTime");
        }

        protected override void OnDisableEditorChild()
        {
            this.sptext = null;
            this.sptargetVariable = null;
            this.spKey = null;
	        this.spencryptVar = null;
	        this.spformatVar = null;
	        this.spformatTime = null;
       }

        public override void OnInspectorGUI()
        {
            this.serializedObject.Update();
	        EditorGUILayout.PropertyField(this.sptargetVariable, new GUIContent("Time Variable"));

     
	        EditorGUILayout.Space();
	        EditorGUILayout.PropertyField(spencryptVar,new GUIContent("Encrypted Variable"));
	        if (encryptVar == true)
	        {
		        EditorGUI.indentLevel++;
        		 EditorGUILayout.PropertyField(this.spKey, new GUIContent("Decryption Key "));
			      EditorGUILayout.LabelField(new GUIContent("Must be identical to Encryption Key"));

		        EditorGUI.indentLevel--;
	        }
	        EditorGUILayout.Space();
	        EditorGUILayout.PropertyField(this.sptext, new GUIContent("UI Text Field"));
	        EditorGUILayout.PropertyField(this.spformatVar, new GUIContent("Format Output"));
	        if (formatVar == true)
	        {
		        EditorGUI.indentLevel++;
		        EditorGUILayout.PropertyField(this.spformatTime, new GUIContent("Format"));
	
		        EditorGUI.indentLevel--;
	        }

  

            EditorGUILayout.Space();

            this.serializedObject.ApplyModifiedProperties();
        }

#endif
    }
}