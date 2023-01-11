namespace PivecLabs.Tools
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.UI;


	public class CommandTimeScale : ConsoleInput 	
    {
        public override string Command { get; protected set; }
        public override string Description { get; protected set; }
		public static CommandTimeScale CreateCommand()	
	    {
	return new CommandTimeScale();				
	    }
	    
	    public CommandTimeScale()					
        {
	        Command = "time";					
			
	        Description = string.Format("{0}{1}", Command.PadRight(15),"Change the scale at which time passes ( 1 is realtime)");
	        
	        AddCommandToConsole();
        }

        public override void RunCommand()
	    {
   
		    Debug.Log ("Parameter Required - from 0.1 to 1 to slowdown , 1 to 100 to speedup");

        }
	    public override void RunCommandwithPar(string name)
	    {
	    	string output;
	    	float time = float.Parse(name);
	    	Time.timeScale = time;
		    
		    output = string.Format("{0}{1}", "Time Scale set to = ", Time.timeScale.ToString());
		    Debug.Log (output);

	    }

	    public override void RunCommandwithPars(string name, string value)
	    {
		    Debug.Log ("Only one Parameter Required  - from 0.1 to 1 to slowdown , 1 to 100 to speedup");

		}
	  
    }
}

