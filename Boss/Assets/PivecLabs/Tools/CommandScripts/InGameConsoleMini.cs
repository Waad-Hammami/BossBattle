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
	
	#if UNITY_EDITOR
	using UnityEditor;
	#endif

	public abstract class ConsoleInputMini
    {
        public abstract string Command { get; protected set; }
        public abstract string Description { get; protected set; }
  
        public void AddCommandToConsole()
        {
	        InGameConsoleMini.AddCommandsToConsole(Command, this);
	        InGameConsoleMini.AddDescToConsole(Description, this);
        }

	    public void ListCommands()
	    
	    {
	    
		    foreach (KeyValuePair<string, ConsoleInputMini> item in InGameConsoleMini.CommandDesc)
		    {
			    Debug.Log(item.Key);
			    		     
		    }

    	
	    }
        public abstract void RunCommand();
	    public abstract void RunCommandwithPar(string name);
	    public abstract void RunCommandwithPars(string name,string value);
    }

	[AddComponentMenu("")]
	public class InGameConsoleMini : MonoBehaviour
    {
	    public static InGameConsoleMini Instance { get; private set; }
	    public static Dictionary<string, ConsoleInputMini> Commands { get; private set; }
	    public static Dictionary<string, ConsoleInputMini> CommandDesc { get; private set; }
	    
	   	public bool adminMode;
	    public bool disableCommands;
	    public bool disableUpdates;
	    public bool disableLogs;
		
	   	public Canvas consoleCanvas;
 	    public Text consoleText;
        public Text inputText;
	    public InputField consoleInput;
	    
	    [SerializeField]
	    public KeyCode selectedKey = KeyCode.None;

	    [HideInInspector]
	   	public bool displayCanvas = false;

	    [HideInInspector]
	    public bool showConsole;

        private void Awake()
        {
            if(Instance != null)
            {
                return;
            }

            Instance = this;
	        Commands = new Dictionary<string, ConsoleInputMini>();
	        CommandDesc = new Dictionary<string, ConsoleInputMini>();
			
        }

        private void Start()
        {


	        consoleCanvas.gameObject.SetActive(false);
	        CreateCommands();
	        
	    
	        
 		  }

	    private void Update()
	    {
		    if (Input.GetKeyDown(selectedKey))
		    {
			    if (showConsole == false)
			    {
				    showConsole = true;
			    }
			    
			    else 
			    {
				    showConsole = false;
			    }
				   
	    	}
	    	
		    if (showConsole == true)
		    {
			   
				    displayCanvas = true;
				    ShowConsole();
			  
		    }
			   
		    else   if (showConsole == false)
			   
			    {
				    displayCanvas = false;
				    HideConsole();
			    }
	    
				   
	    	
	    	
	    	
	    }
	    
        private void OnEnable()
        {
	        Application.logMessageReceived += HandleLog;
	    }

        private void OnDisable()
        {
            Application.logMessageReceived -= HandleLog;
        }

        private void HandleLog(string logMessage, string stackTrace, LogType type)
	    {
		    if (disableLogs == false)
        	
		    {
			    string time = string.Format("{0}:{1}:{2}",System.DateTime.Now.Hour,System.DateTime.Now.Minute,System.DateTime.Now.Second);
			    string _message = "[" + time + "]" + logMessage;
			    StartCoroutine(AddMessageToConsole(_message));  
		    }
		    else
		    {
		    	if(type == LogType.Log)
		    	{
			    	string time = string.Format("{0}:{1}:{2}",System.DateTime.Now.Hour,System.DateTime.Now.Minute,System.DateTime.Now.Second);
			    	string _message = "[" + time + "]" + logMessage;
			    	StartCoroutine(AddMessageToConsole(_message));  

		    	}

		    }
        }

        private void CreateCommands()
	    {
	    	CommandMini.CreateCommand();
		    CommandHelpMini.CreateCommand();

		    //	Add any new commands here
		    //	CommandNAME.CreateCommand();  // change CommandName to your command

		    
        }

	    public static void AddCommandsToConsole(string _name, ConsoleInputMini _command)
        {
            if(!Commands.ContainsKey(_name))
            {
	            Commands.Add(_name, _command);
            }
        }

	    public static void AddDescToConsole(string _name, ConsoleInputMini _help)
	    {
		    if(!CommandDesc.ContainsKey(_name))
		    {
			    CommandDesc.Add(_name, _help);
		    }
	    }
	  

	    IEnumerator AddMessageToConsole(string msg)
	    {
		    yield return new WaitForEndOfFrame();
		    consoleText.text += msg + "\n";
		    ClearInput();
		   
	    }


	    public void ClearMessages()
	    {
		    consoleText.text = "";
 
	    }

	    public void EndEdit()
	    {
		    
			    if(inputText.text != "")
				    {
				    StartCoroutine(AddMessageToConsole(inputText.text));  
				    ParseInput(inputText.text);
				    }  
		    		 
	    }

	    public void ClearInput()
	    {
		    
		    consoleInput.text = "";
			 
	    }



	    public void ShowConsole()
	    
	    {
		    consoleCanvas.gameObject.SetActive(true);
    	
	    }

	    public void HideConsole()
	    
	    {
		    consoleCanvas.gameObject.SetActive(false);
    	
	    }


        private void ParseInput(string input)
	    {
        	
        	
        	
		    bool notfound = false;
	        string _input = input.ToString();
	
		    string eventname = _input.Remove(0,1);
		   
		    if (_input.Equals("?"))
		    
			    Commands[_input[0].ToString()].RunCommand();
		    
		         
		    else if (eventname.Length<1)
		    {
			    Debug.Log ("Event Name Required");

		    }
		    else
		    {
		    	
			    string[] keys = EventDispatchManager.Instance.GetSubscribedKeys();

			    foreach (string name in keys)
		
			    {
				    if (name.Equals(eventname))
				    {
					    Commands[_input[0].ToString()].RunCommandwithPar(eventname);
					    notfound = false;
					    break;
				    }
				    
				    notfound = true;
				     
					   
			    }
	       
			    if (notfound == true)
				    Debug.Log ("Event Name not found");
	        
		    }
	    
	    
			
         
        }
   
    }
}
