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


	public class CommandLists : ConsoleInput
    {
        public override string Command { get; protected set; }
        public override string Description { get; protected set; }
	    private string varType;
	    private string varName;
	    private string varValue;
	    public static CommandLists CreateCommand()
	    {
		    return new CommandLists();
	    }
	    
    
	    public CommandLists()
        {
	        Command = "lists";
	        Description = string.Format("{0}{1}", Command.PadRight(15),"Displays all List variables");
	        
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
		    
	        foreach(GameObject gameObj in GameObject.FindObjectsOfType<GameObject>())
	        {
		        
        		ListVariables listVariables = gameObj.GetComponent<ListVariables>();
		        if (listVariables != null) 
		        {
        			for (int i = 0; i < listVariables.references.Length; ++i)
        			{
	        			varName = gameObj.transform.parent.name;
	        			int type = listVariables.references[i].variable.type;
	        			
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
		        			varType =  "String";
	        			}
	        			if (type == (int)Variable.DataType.Bool)
	        			{
		        			varType =  "Number";
	        			}if (type == (int)Variable.DataType.Color)
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
	        			
	        			varValue = listVariables.references[i].variable.Get().ToString();
	        		
	        			string output = string.Format("GameObject: {0}Type: {1}Value: {2}",
		            
		        			varName.PadRight(20),
		        			varType.PadRight(10), 
		        			varValue.PadRight(20));
			            
	        			Debug.Log (output);
        			}
		        
		        }
				
			}
	 
        }

    
    }
}