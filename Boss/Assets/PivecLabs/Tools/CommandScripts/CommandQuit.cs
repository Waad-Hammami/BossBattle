namespace PivecLabs.Tools
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.UI;
	using GameCreator.Core;
	using GameCreator.Variables;
	using GameCreator.Localization;

	public class CommandQuit : ConsoleInput
    {
        public override string Command { get; protected set; }
        public override string Description { get; protected set; }
	    public static CommandQuit CreateCommand()
	    {
		    return new CommandQuit();
	    }
	    
        public CommandQuit()
        {
            Command = "quit";
	        Description = string.Format("{0}{1}", Command.PadRight(15),"Quits the application");
 
            AddCommandToConsole();
        }

	    public override void RunCommandwithPar(string name)
	    {
		    RunCommand();
	    }
	    public override void RunCommandwithPars(string name, string value)
	    {
		    RunCommand();
	    }
        public override void RunCommand()
        {
            if (Application.isEditor)
            {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#endif
            }
            else
            {
                Application.Quit();
            }
        }

       
    }
}