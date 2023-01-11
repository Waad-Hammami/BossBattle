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


	public class CommandEvents : ConsoleInput
	{
		private int count;
		private int target;
		private string method;
		public UnityEvent onList = new UnityEvent();

        public override string Command { get; protected set; }
        public override string Description { get; protected set; }
    	public static CommandEvents CreateCommand()
	    {
		    return new CommandEvents();
	    }
	    

	    public CommandEvents()
        {
	        Command = "events";
	        Description = string.Format("{0}{1}", Command.PadRight(15),"Lists all Game Creator Events");
	        AddCommandToConsole();
        }

	    public override void RunCommand()
		{
			string[] keys = EventDispatchManager.Instance.GetSubscribedKeys();

			Debug.Log ("Events in Total = " + keys.Length );
			
			foreach (string name in keys)
		
			{
				
				Debug.Log ("source = " + name);
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