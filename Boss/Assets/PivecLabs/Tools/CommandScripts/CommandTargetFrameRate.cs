namespace PivecLabs.Tools
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.UI;


public class CommandTargetFrameRate : ConsoleInput 		
    {
        public override string Command { get; protected set; }
        public override string Description { get; protected set; }
		public static CommandTargetFrameRate CreateCommand()	
	    {
	return new CommandTargetFrameRate();					
	    }
	    
	    public CommandTargetFrameRate()					
        {
			Command = "frames";						
			
	        Description = string.Format("{0}{1}", Command.PadRight(15),"Set the Application target frame rate");
	        
	        AddCommandToConsole();
        }

        public override void RunCommand()
        {
			 Debug.Log ("Parameter Required");
	       
        }
	    public override void RunCommandwithPar(string name)
	    {
			 Application.targetFrameRate = int.Parse(name);
		    Debug.Log("Application Target Frame Rate set to "+ name);
	    }

	    public override void RunCommandwithPars(string name, string value)
	    {
			 Debug.Log ("Only one Parameter Required");
		}
	  
    }
}

