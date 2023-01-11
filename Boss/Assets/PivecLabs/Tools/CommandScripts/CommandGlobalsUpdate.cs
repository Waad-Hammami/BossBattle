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


	public class CommandGlobalsUpdate : ConsoleInput
    {
        public override string Command { get; protected set; }
        public override string Description { get; protected set; }
	    private string varType;
	    private string varName;
	    private string varValue;
	    
    
	    
	    public static CommandGlobalsUpdate CreateCommand()
	    {
		    return new CommandGlobalsUpdate();
	    }
	    
   
	    public CommandGlobalsUpdate()
        {
	        Command = "globalsU";
	        Description = string.Format("{0}{1}", Command.PadRight(15),"Updates value (string,bool,number) of a global variable");
	        
            AddCommandToConsole();
        }
        
	    public override void RunCommand()
	    {
		    Debug.Log ("Parameter Required");

	    }
	    
	    public override void RunCommandwithPar(string name)
	    {
		    Debug.Log ("Two Parameters Required");

	    }

	    public override void RunCommandwithPars(string name, string value)
	    {
	    	
	    	if (VariablesManager.ExistsGlobal(name) == true)
	    	
	    	{

		    	var variable = VariablesManager.GetGlobalType(name);
		    	switch (variable)
		    	{
		    	case Variable.DataType.String: 
			    		VariablesManager.SetGlobal(name, value); 
			    		break;
		    	case Variable.DataType.Number: 
			    		VariablesManager.SetGlobal(name, float.Parse(value)); 
			    		break;
		    	case Variable.DataType.Bool:
			    	VariablesManager.SetGlobal(name, Convert.ToBoolean(value)); 
			    		break; 
		    	}
		    	varName = name;
			    varValue = VariablesManager.GetGlobal(name).ToString();

	    		string output = string.Format("Name: {0}Value: {1}",
		            
		    		varName.PadRight(25),
		    		varValue.PadRight(20));
			            
		    	Debug.Log (output);
	    	}
	    	
	    	else
	    	{
	    		Debug.Log ("Variable name incorrect");
	    	}
		    
		 

        }

    
    }
}