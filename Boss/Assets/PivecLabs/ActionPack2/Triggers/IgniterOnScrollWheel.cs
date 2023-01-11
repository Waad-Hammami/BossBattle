namespace PivecLabs.ActionPack
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using GameCreator.Core;
	using GameCreator.Variables;
	using GameCreator.Core.Hooks;
	using GameCreator.Camera;

    [AddComponentMenu("")]
    public class IgniterOnScrollWheel : Igniter 
	{
		#if UNITY_EDITOR
		public new static string NAME = "Input/On Scroll Wheel";
        #endif


        private void Update()
		{
			if(Input.GetAxisRaw("Mouse ScrollWheel") != 0)
			{
		        this.ExecuteTrigger(gameObject);		
        	}
        }
	}
}