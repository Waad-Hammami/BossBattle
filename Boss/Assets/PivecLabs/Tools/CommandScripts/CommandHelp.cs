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


	public class CommandHelp : ConsoleInput
    {
        public override string Command { get; protected set; }
        public override string Description { get; protected set; }
    	public static CommandHelp CreateCommand()
	    {
		    return new CommandHelp();
	    }
	    
	    public CommandHelp()
        {
	        Command = "?";
	        Description = string.Format("{0}{1}", Command.PadRight(15),"Displays all available commands");
	        AddCommandToConsole();
        }

        public override void RunCommand()
        {
	        
	        
	        ListCommands();
	       
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