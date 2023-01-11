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


	public class CommandGlobals : ConsoleInput
    {
        public override string Command { get; protected set; }
        public override string Description { get; protected set; }
	    private string varType;
	    private string varName;
	    private string varValue;
	    public static CommandGlobals CreateCommand()
	    {
		    return new CommandGlobals();
	    }
	    
    
	    public CommandGlobals()
        {
	        Command = "globals";
	        Description = string.Format("{0}{1}", Command.PadRight(15),"Displays all global variables");
	        
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
		        DatabaseVariables database = DatabaseVariables.Load();
	            
	            GlobalVariables globalVariables = database.GetGlobalVariables();
            if (globalVariables != null)
            {

           
	            for (int i = 0; i < globalVariables.references.Length; ++i)
	            {
	            	
		            int type = globalVariables.references[i].variable.type;
		            if (type == (int)Variable.DataType.String)
		            {
			            varType =  "String";
		            }
		            if (type == (int)Variable.DataType.Bool)
		            {
			            varType =  "Bool";
		            }
		            if (type == (int)Variable.DataType.Number)
		            {
			            varType =  "Number";
		            }
		            if (type == (int)Variable.DataType.Color)
		            {
			            varType =  "Color";
		            }
		            if (type == (int)Variable.DataType.GameObject)
		            {
			            varType =  "GameObject";
		            }
		            if (type == (int)Variable.DataType.Sprite)
		            {
			            varType =  "Sprite";
		            }
		            if (type == (int)Variable.DataType.Texture2D)
		            {
			            varType =  "Texture2D";
		            }
		            if (type == (int)Variable.DataType.Vector2)
		            {
			            varType =  "Vector2";
		            }
		            if (type == (int)Variable.DataType.Vector3)
		            {
			            varType =  "Vector3";
		            }
		       
		            varName = string.Format("{0}", globalVariables.references[i].variable.name);
		            varValue = VariablesManager.GetGlobal(globalVariables.references[i].variable.name).ToString();
		            string output = string.Format("Name: {0}Type: {1}Value: {2}",
		            
		            	varName.PadRight(25),
			            varType.PadRight(10), 
			            varValue.PadRight(20));
			            
		            Debug.Log (output);
	            }
			}
            else Debug.Log("No Global Variables Defined");

		}

    
    }
}