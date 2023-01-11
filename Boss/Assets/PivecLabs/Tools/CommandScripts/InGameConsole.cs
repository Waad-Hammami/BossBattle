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

	public abstract class ConsoleInput
    {
        public abstract string Command { get; protected set; }
        public abstract string Description { get; protected set; }
  
        public void AddCommandToConsole()
        {
	        InGameConsole.AddCommandsToConsole(Command, this);
	        InGameConsole.AddDescToConsole(Description, this);
        }

	    public void ListCommands()
	    
	    {
	    
		    foreach (KeyValuePair<string, ConsoleInput> item in InGameConsole.CommandDesc)
		    {
			    Debug.Log(item.Key);
			    		     
		    }

    	
	    }
        public abstract void RunCommand();
	    public abstract void RunCommandwithPar(string name);
	    public abstract void RunCommandwithPars(string name,string value);
    }

	[AddComponentMenu("")]
	public class InGameConsole : MonoBehaviour
    {
        public static InGameConsole Instance { get; private set; }
	    public static Dictionary<string, ConsoleInput> Commands { get; private set; }
	    public static Dictionary<string, ConsoleInput> CommandDesc { get; private set; }
	    
	   	public bool adminMode;
	    public bool disableCommands;
	    public bool disableUpdates;
	    public bool disableLogs;
		
	   	public Canvas consoleCanvas;
    	public GameObject sysinfoPanel;
	    public Text consoleText;
        public Text inputText;
	    public InputField consoleInput;
	    
	    [SerializeField]
	    public KeyCode selectedKey = KeyCode.None;

	    [HideInInspector]
	    public bool showSysinfo;
	   	public bool displayCanvas = false;

	    [HideInInspector]
	    public bool showConsole;

        private void Awake()
        {
            if(Instance != null)
            {
                return;
            }
	        sysinfoPanel.SetActive(false);

            Instance = this;
	        Commands = new Dictionary<string, ConsoleInput>();
	        CommandDesc = new Dictionary<string, ConsoleInput>();
			
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
			    string _message = "[" + time + "] [" + type.ToString() + "] " + logMessage;
			    StartCoroutine(AddMessageToConsole(_message));  
		    }
		    else
		    {
		    	if(type == LogType.Log)
		    	{
			    	string time = string.Format("{0}:{1}:{2}",System.DateTime.Now.Hour,System.DateTime.Now.Minute,System.DateTime.Now.Second);
			    	string _message = "[" + time + "] [" + type.ToString() + "] " + logMessage;
			    	StartCoroutine(AddMessageToConsole(_message));  

		    	}

		    }
        }

        private void CreateCommands()
	    {
	    	CommandHelp.CreateCommand();
		    CommandGlobals.CreateCommand();
		    CommandGlobalsUpdate.CreateCommand();
		    CommandLocals.CreateCommand();
		    CommandLists.CreateCommand();
		    CommandEvent.CreateCommand();
		    CommandEvents.CreateCommand();
		    CommandTargetFrameRate.CreateCommand();
		    CommandVSync.CreateCommand();
		    CommandQuality.CreateCommand();
		    CommandIncreaseQuality.CreateCommand();
		    CommandDecreaseQuality.CreateCommand();
		    CommandTimeScale.CreateCommand();

		    CommandQuit.CreateCommand();
		    
		    //	Add any new commands here
		    //	CommandNAME.CreateCommand();  // change CommandName to your command

		    
        }

	    public static void AddCommandsToConsole(string _name, ConsoleInput _command)
        {
            if(!Commands.ContainsKey(_name))
            {
	            Commands.Add(_name, _command);
            }
        }

	    public static void AddDescToConsole(string _name, ConsoleInput _help)
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

	    public void ShowSysInfo()
	    
	    {
	    	if (consoleText.enabled == true)
	    	{
	    		
		    	consoleText.enabled = false;
		    	sysinfoPanel.SetActive(true);
		    	SysInfoExt.sysinfo = true;			
	    	}
		    	
		    else
		    {
			    consoleText.enabled = true;
			    sysinfoPanel.SetActive(false);
			    SysInfoExt.sysinfo = false;			
 	
		    }
	    	
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
            string[] _input = input.Split(null);
	
	        if (_input.Length == 0 || _input == null)
	        {
	             Debug.LogWarning("Input not Valid");
	            return;
	        }

	        if (!Commands.ContainsKey(_input[0]))
            {
	            Debug.LogWarning("Input not Valid");
	            Debug.LogWarning("Type ? for Valid commands");

            }
            else
	        {
	            if (_input.Length >1) 
	            {
			         if (disableUpdates == false)
			            {
				        	if (_input.Length <3)
				        	{
				        		Commands[_input[0]].RunCommandwithPar(_input[1]);
				        	}
				            if (_input.Length >2)
				            {
					            Commands[_input[0]].RunCommandwithPars(_input[1],_input[2]);
				            }
			            }
			            else
			            {
				            Debug.LogWarning("Updates Disabled");
			            }
				           
			           
		           }

	            else if (disableCommands == false)
	            {
		            Commands[_input[0]].RunCommand();
	            }
		         
	            else
		            
		            {
			           
		            	Debug.LogWarning("Commands Disabled");
		            }
 	            	            
	    	 }
           }
        }
   
   
}
