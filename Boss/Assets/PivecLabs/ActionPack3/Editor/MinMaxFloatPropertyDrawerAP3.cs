using UnityEngine;
using UnityEditor;

	namespace PivecLabs.MinMaxSliderFloatAP3 {
		
		[CustomPropertyDrawer(typeof(MinMaxFloatAttribute))]
	public class MinMaxSliderDrawer : PropertyDrawer {
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
			MinMaxFloatAttribute minMaxAttr = attribute as MinMaxFloatAttribute;

			var minProperty = property.FindPropertyRelative("min");
			var maxProperty = property.FindPropertyRelative("max");

			DrawMinMaxSlider(position, label, minMaxAttr, minProperty, maxProperty);
		}
			

		protected void DrawMinMaxSlider(Rect position, GUIContent label, MinMaxFloatAttribute minMaxAttr, SerializedProperty minProperty, SerializedProperty maxProperty) {
			float textWidth = 30f;
			float paddingWidth = 10f;
			float fieldWidth = (position.width - EditorGUIUtility.labelWidth) / 2 - textWidth - paddingWidth / 2;
			position.height = EditorGUIUtility.singleLineHeight;
			float minValue = 0;
			float maxValue = 0;
			minValue = minProperty.floatValue;
			maxValue = maxProperty.floatValue;					
			EditorGUI.BeginChangeCheck();
			EditorGUI.MinMaxSlider(position, label, ref minValue, ref maxValue, minMaxAttr.MinLimit, minMaxAttr.MaxLimit);
			position.y += EditorGUIUtility.singleLineHeight;
			position.x += EditorGUIUtility.labelWidth;
			position.width = textWidth;
			GUI.Label(position, new GUIContent("Min"));
			position.x += position.width;
			position.width = fieldWidth;
			minValue = Mathf.Clamp(EditorGUI.FloatField(position, minValue), minMaxAttr.MinLimit, maxValue);
			position.x += paddingWidth;
			position.x += position.width;
			position.width = textWidth;
			GUI.Label(position, new GUIContent("Max"));
			position.x += position.width;
			position.width = fieldWidth;
			maxValue = Mathf.Clamp(EditorGUI.FloatField(position, maxValue), minValue, minMaxAttr.MaxLimit);
			minProperty.floatValue = minValue;
			maxProperty.floatValue = maxValue;
		}
	
		public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
			MinMaxFloatAttribute minMax = attribute as MinMaxFloatAttribute;
			float size = EditorGUIUtility.singleLineHeight * 2;
			

			return size;
		}
	}
}