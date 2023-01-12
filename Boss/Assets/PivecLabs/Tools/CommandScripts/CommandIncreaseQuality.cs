﻿namespace PivecLabs.Tools
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.UI;


	public class CommandIncreaseQuality : ConsoleInput 	
    {
        public override string Command { get; protected set; }
        public override string Description { get; protected set; }
		public static CommandIncreaseQuality CreateCommand()	
	    {
	return new CommandIncreaseQuality();				
	    }
	    
	    public CommandIncreaseQuality()					
        {
	        Command = "quality+";					
			
	        Description = string.Format("{0}{1}", Command.PadRight(15),"Increase Quality Level with/without expensive changes (true/false) ");
	        
	        AddCommandToConsole();
        }

        public override void RunCommand()
	    {
   
		    Debug.Log ("Parameter Required - true or false");

        }
	    public override void RunCommandwithPar(string name)
	    {
	    	string output;
	    	bool expensive = bool.Parse(name);
	    	QualitySettings.IncreaseLevel(expensive);
		    output = string.Format("{0}{1}", "Increase Quality Level with expensive changes - ", name);
		    Debug.Log (output);

		    output = string.Format("{0}{1}", "Quality Level now = ", QualitySettings.GetQualityLevel());
		    Debug.Log (output);

	    }

	    public override void RunCommandwithPars(string name, string value)
	    {
		    Debug.Log ("Only one Parameter Required  - true or false");

		}
	  
    }
}
