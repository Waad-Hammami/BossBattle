namespace PivecLabs.Tools
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.UI;
	using UnityEngine.Events;
	using UnityEngine.EventSystems;

	using GameCreator.Core;
	using GameCreator.Variables;
	using GameCreator.Localization;


	public class CommandMini : ConsoleInputMini
    {
        public override string Command { get; protected set; }
        public override string Description { get; protected set; }
    	public static CommandMini CreateCommand()
	    {
		    return new CommandMini();
	    }
	    

	    public CommandMini()
        {
	        Command = "/";
	        Description = string.Format("{0}{1}", Command.PadRight(15),"Dispatches a Game Creator Event");
	        AddCommandToConsole();
        }

	    public override void RunCommandwithPar(string name)
	    {
	   
		       EventDispatchManager.Instance.Dispatch(name, InGameConsoleMini.Instance.consoleCanvas.gameObject);
		       Debug.Log (name +" Event Dispatched");
        }
	    public override void RunCommand()
	    {
		    Debug.Log ("Event Name Required");

	    }

	    public override void RunCommandwithPars(string name, string value)
	    {
		    Debug.Log ("Event Name Required");

	    }

	  
    }
}