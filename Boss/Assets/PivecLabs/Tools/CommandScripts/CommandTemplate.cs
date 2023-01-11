/*  // Delete this Line

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


public class CommandNAME : ConsoleInput 		// Change CommandNAME to your CommandName
    {
        public override string Command { get; protected set; }
        public override string Description { get; protected set; }
	public static CommandNAME CreateCommand()	// Change CommandNAME to your CommandName
	    {
	return new CommandNAME();					// Change CommandNAME to your CommandName
	    }
	    
	    public CommandNAME()					// Change CommandNAME to your CommandName
        {
			Command = "?";						// Change ? to your command
			
		Description = string.Format("{0}{1}", Command.PadRight(15),"Your Command Description goes here");
	        
	        AddCommandToConsole();
        }

        public override void RunCommand()
        {
			// Add your Command code here if there are no Parameters
			
			// else uncomment the following line
			
			// Debug.Log ("Parameter Required");
	       
        }
	    public override void RunCommandwithPar(string name)
	    {
			// Add your Command code here if there is one Parameter
			
			// else uncomment the following line
			
			// RunCommand();
	    }

	    public override void RunCommandwithPars(string name, string value)
	    {
			// Add your Command code here if there are two Parameters
			
			// else uncomment the following line
				
			// RunCommand();
		}
	  
    }
}

		// You must also add your command to InGameConsole.c (approx Line 130).
		
*/  	// Delete this Line.