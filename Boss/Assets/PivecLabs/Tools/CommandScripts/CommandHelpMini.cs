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


	public class CommandHelpMini : ConsoleInputMini
    {
        public override string Command { get; protected set; }
        public override string Description { get; protected set; }
    	public static CommandHelpMini CreateCommand()
	    {
		    return new CommandHelpMini();
	    }
	    
	    public CommandHelpMini()
        {
	        Command = "?";
	        Description = string.Format("{0}{1}", Command.PadRight(15),"Displays all available events");
	        AddCommandToConsole();
        }

        public override void RunCommand()
        {
	        
	        string[] keys = EventDispatchManager.Instance.GetSubscribedKeys();

	        Debug.Log ("Events in Total = " + keys.Length );
			
	        foreach (string name in keys)
		
	        {
				
		        Debug.Log ("Event Commands = \"/" + name + "\"");
	        }
	       
        }
	    public override void RunCommandwithPar(string name)
	    {
		    RunCommand();
	    }

	    public override void RunCommandwithPars(string name, string value)
	    {
		    RunCommand();
	    }
	  
    }
}