using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PivecLabs.MinMaxSliderFloatAP3 {


	[System.Serializable]
	public class MinMaxType<T> {
		public T min;
		public T max;
	}
	
	[System.Serializable]
	public class MinMaxfloat : MinMaxType<float> { }
	
	
	[System.AttributeUsage(System.AttributeTargets.Field)]
	public class MinMaxFloatAttribute : PropertyAttribute {
		public readonly float MinLimit = 0;
		public readonly float MaxLimit = 1;

		public MinMaxFloatAttribute(float min, float max) {
			MinLimit = min;
			MaxLimit = max;
		}
	}
	

}
