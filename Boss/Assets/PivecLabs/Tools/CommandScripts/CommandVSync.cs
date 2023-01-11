namespace PivecLabs.Tools
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.UI;


	public class CommandVSync : ConsoleInput 		
    {
        public override string Command { get; protected set; }
        public override string Description { get; protected set; }
		public static CommandVSync CreateCommand()	
	    {
	return new CommandVSync();					
	    }
	    
	    public CommandVSync()					
        {
	        Command = "vsync";						
			
	        Description = string.Format("{0}{1}", Command.PadRight(15),"Set vSync to 0 (dont sync), 1,2,3 or 4");
	        
	        AddCommandToConsole();
        }

        public override void RunCommand()
        {
			 Debug.Log ("Parameter Required");
	       
        }
	    public override void RunCommandwithPar(string name)
	    {
		    QualitySettings.vSyncCount = int.Parse(name);
		    Debug.Log("QualitySettings vSync set to "+ name);
	    }

	    public override void RunCommandwithPars(string name, string value)
	    {
			 Debug.Log ("Only one Parameter Required");
		}
	  
    }
}

