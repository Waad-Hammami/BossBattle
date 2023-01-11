namespace PivecLabs.Tools
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.UI;


public class CommandQuality : ConsoleInput 	
    {
        public override string Command { get; protected set; }
        public override string Description { get; protected set; }
		public static CommandQuality CreateCommand()	
	    {
	return new CommandQuality();				
	    }
	    
	    public CommandQuality()					
        {
	        Command = "quality";					
			
	        Description = string.Format("{0}{1}", Command.PadRight(15),"Display Application Quality Level");
	        
	        AddCommandToConsole();
        }

        public override void RunCommand()
	    {
        	
		    string output = string.Format("{0}{1}", "Current Quality Level = ", QualitySettings.GetQualityLevel());
		    Debug.Log (output);
	       
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

