namespace PivecLabs.ActionPack
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.Audio;
	using GameCreator.Core;
	using GameCreator.Variables;
	using UnityEngine.UI;
	using UnityEngine.Events;

	public class DayNightManager : MonoBehaviour
{

	public bool usingSystemTime = false;
	public bool usingRealTimeSpeed = false;
	public bool randomizeStartTime = false;
	
	private static bool usingST = false;
	private static bool usingRT = false;
	
	[SerializeField]
	public enum FORMATTIME
	{
		String,
		Number
			
	}
	[SerializeField]
	public FORMATTIME varFormatTime = FORMATTIME.Number;

	[SerializeField]
	public enum FORMATDAY
	{
		String,
		Number
			
	}
	[SerializeField]
	public FORMATDAY varFormatDay = FORMATDAY.Number;


	[SerializeField]
	public NumberProperty setStartTime = new NumberProperty(6.0f);
	[SerializeField]
	public StringProperty setStartTimeString = new StringProperty("6");

	[SerializeField]
	public NumberProperty setStartDay = new NumberProperty(0.0f);
	[SerializeField]
	public StringProperty setStartDayString = new StringProperty("Monday");
	public NumberProperty dayAsMinutes = new NumberProperty(15.0f);

	public bool savetotalDays;

	[VariableFilter(Variable.DataType.Number)]
	public VariableProperty totaldaysVariable = new VariableProperty(Variable.VarType.GlobalVariable);

	private Vector3 sunAngle;
	private static float time = 0.0f;
	private float actionTime = 0.0f;
	private float day = 0.0f;
	public static int dayofWeek;
	private int dayofweek;
	public static bool setDOW;

	public float maxSun = 5;
	[SerializeField]
	public AnimationCurve timeCurve;
	private float currentCurveValue = 0;
	private int daysPassed = 0;
	
	public static float sunIntensity = 1.0f;
	public static bool sunCurveBool;
	public bool sunCurve;
	
	public  Color sunColor;
	public  Color ambientDay;
	public  Color ambientNight;
	
	public  static bool colorsChanged;
	public  static Color newSunColor;
	public  static Color newAmbientDay;
	public  static Color newAmbientNight;
	
	private static DayNightManager Instance;

	public Light sunObject;
	private float shaderlightvalue;
	
	public Light moonObject;
	private static float moonXAxis;

	public  static bool sbRotation;
	public  static bool sbRotationChanged;

	public bool skyboxRotate;
	[Range(0, 10)]
	public float skyboxRotateSpeed;
	public static float newskyboxRotateSpeed;


	public  static bool sbExposureChanged;

	public bool skyboxExposure;
	[Range(0, 1)]
	public static float newskyboxExposureSetting;
	public static float oldskyboxExposureSetting = 1.0f;

	[Range(0, 10)]
	public static float newskyboxExposureSpeed;

	public static bool setTOD;
	public static float newTOD;

	public static bool advTOD;
	public static float newadvTOD;


	[Range(1, 60)]
	public int sunSeason;
	public static int seasons = 0;
	public static bool seasonChanged;
	public static int newsunSeason;

	private static float sunXAxis;
	private bool yAxis;
	private float sunYAxis;
	private float sunZAxis;
	private float tempFloat;
	private float roundedTime;
	
	[System.Serializable]
	public class ActionObject
	{
		public Actions actionToExecute;
		
		public enum DAY
		{
			Sunday,
			Monday,
			Tuesday,
			Wednesday,
			Thursday,
			Friday,
			Saturday,
			EveryDay
			
		}
		public DAY Day = DAY.EveryDay;
		
		[Range(0,23)]
		public int hour;
		[Range(0,59)]
		public int minute;
		[HideInInspector]
		public float actiontime;  
   
	}


	[SerializeField]
	public List<ActionObject> ListofActions = new List<ActionObject>();
	private List<ActionObject> ListofActionsCopy = new List<ActionObject>();

	Actions actionsToExecute = null;
	
	public bool executeAllActions = false;


	void Awake ()
	{
		if (usingSystemTime == true)
		{
			randomizeStartTime = false;
		}
		

		if (randomizeStartTime == false) {
			if (usingSystemTime == false) {
				
				switch (varFormatTime)
				{
				
				case FORMATTIME.String:
			
					time = int.Parse(setStartTimeString.GetValue(gameObject));
					break;
				case FORMATTIME.Number:
					time = (int)setStartTime.GetValue(gameObject);
					break;
				}
				
				
				switch (varFormatDay)
				{
				
				case FORMATDAY.String:
			
					switch (setStartDayString.GetValue(gameObject))
					{
					case "Sunday":
						dayofweek = (int)0;
						break;
					case "Monday":
						dayofweek = (int)1;
						break;
					case "Tuesday":
						dayofweek = (int)2;
						break;
					case "Wednesday":
						dayofweek = (int)3;
						break;
					case "Thursday":
						dayofweek = (int)4;
						break;
					case "Friday":
						dayofweek = (int)5;
						break;
					case "Saturday":
						dayofweek = (int)6;
						break;
					}
					
					
					break;
					
				case FORMATDAY.Number:
					dayofweek = (int)setStartDay.GetValue(gameObject);
					break;
				}

			
			}
			
			
			
			
			if (usingSystemTime == true) {
				usingST = true;
				time = System.DateTime.Now.TimeOfDay.Hours;
				time += ((System.DateTime.Now.TimeOfDay.Minutes) / 60.0f);
				dayofweek = (int)System.DateTime.Now.DayOfWeek;
			}
			
		}

		if (randomizeStartTime == true) {
			time = UnityEngine.Random.Range (0.0f, 24.0f);
			dayofweek = (int)UnityEngine.Random.Range (0.0f, 6.0f);
			
		}
		

		if (sunObject != null)
		{
			sunXAxis = -100 + (360 * (time / 24));
			sunObject.transform.localEulerAngles = new Vector3 (sunXAxis, 96f, 0);
			currentCurveValue = timeCurve.Evaluate (time);
			RenderSettings.ambientLight = Color.Lerp (ambientNight, ambientDay, (currentCurveValue));
			RenderSettings.reflectionIntensity = Mathf.Lerp (0f, 1f, currentCurveValue);

			sunObject.bounceIntensity = 1f;
			sunObject.shadowBias = 0.05f;
			sunObject.shadowNormalBias = 0f;
			sunObject.shadowNearPlane = 0.2f;
			sunObject.shadows = LightShadows.Soft;

			newSunColor = (Color)sunColor;
			newAmbientDay = (Color)ambientDay;
			newAmbientNight = (Color)ambientNight;
	
		}
		
		if (moonObject != null)
		{
			moonXAxis = -100 + (360 * (time / 24));
			moonObject.transform.localEulerAngles = new Vector3 (moonXAxis, 96f, 0);
			currentCurveValue = timeCurve.Evaluate (time);
		}
		
		
		for (int i =0; i < ListofActions.Count; i++)
		{
			actionTime = ListofActions[i].hour;
			actionTime += (ListofActions[i].minute / 60.0f);
			


			if (usingRealTimeSpeed == false) {
				actionTime = actionTime *10;
			}
			
				ListofActions[i].actiontime = actionTime;
			
		}
		
		dayofWeek = dayofweek;

		ListofActionsCopy = new List<ActionObject>(ListofActions);

	}

	void LateUpdate ()
	{
		if (usingRealTimeSpeed == false) {
			day = dayAsMinutes.GetValue(gameObject);
			time += (Time.smoothDeltaTime) / (day * 2.5f);
						
		}
		if (usingRealTimeSpeed == true) {
			usingRT = true;
			time += (Time.smoothDeltaTime) / (3600.0f);
			

		}
		if (time >= 24) {
			time = 0;
			daysPassed += 1;
			if (savetotalDays)
			{
				float current = (float)(this.totaldaysVariable.Get(null) ?? 0f);
				this.totaldaysVariable.Set(current + daysPassed, null);

			}
			
			dayofweek +=1;
			dayofWeek = dayofweek;
			if (dayofweek > 6) 
			{
				dayofweek = 0;
			}
			ListofActionsCopy = new List<ActionObject>(ListofActions);
		}
		
		
		if (colorsChanged == true)
		{
			StartCoroutine(OnColorsChanged());
		}

		if (seasonChanged == true)
		{
			StartCoroutine(OnSeasonChanged());
		}

		if (setTOD == true)
		{
			StartCoroutine(OnSetTOD());
		}



		if (advTOD == true)
		{
			StartCoroutine(OnAdvTOD());
		}

		if (setDOW == true)
		{
			StartCoroutine(OnSetDOW());
		}



		if (sbRotationChanged == true)
		{
			StartCoroutine(OnSBRotationChanged());
		}
		if (sbExposureChanged == true)
		{
			StartCoroutine(OnSBExposureChanged());
		}


		if (sunObject != null)
		{
			if (sunCurve == true)
			{
			
				currentCurveValue = timeCurve.Evaluate (time);

				RenderSettings.ambientLight = Color.Lerp (ambientNight, ambientDay, (currentCurveValue));
				RenderSettings.reflectionIntensity = Mathf.Lerp (0f, 1f, currentCurveValue);
				
				tempFloat = (maxSun * (currentCurveValue));
				if (tempFloat > maxSun) {
					tempFloat = maxSun;
				}
				if (tempFloat < 0) {
					tempFloat = 0;
				}
		
				sunObject.intensity = tempFloat;
				sunObject.shadowStrength = tempFloat;

			}

			if (sunCurve == false)
			{
				sunObject.intensity = sunIntensity;
				sunObject.shadowStrength = sunIntensity;

			}
	
		
			sunXAxis = -100 + (360 * (time / 24));
			sunYAxis =  sunSeason;
			seasons = sunSeason;
			Quaternion RotationOfDay = Quaternion.Euler(sunXAxis, 0, 0);
			Quaternion RotationOfSeason = Quaternion.Euler(0, sunYAxis, 0);
		
		
			sunObject.transform.rotation = RotationOfDay * RotationOfSeason;
			
		}

		if (moonObject != null)
		{
			moonXAxis = 100 + (360 * (time / 24));
		
			Quaternion RotationOfDay = Quaternion.Euler(moonXAxis, 0, 0);
		
		
			moonObject.transform.rotation = RotationOfDay;
			
		}
		
		if (skyboxRotate)
		{
			RenderSettings.skybox.SetFloat("_Rotation", Time.time * skyboxRotateSpeed); 
	
		}
	
		if (usingRealTimeSpeed == false)	
		{
			roundedTime = time *10f;
		
			for (int i =0; i < ListofActionsCopy.Count; i++)
			{
				actionTime = ListofActionsCopy[i].actiontime;
				int actionDay = (int)ListofActionsCopy[i].Day;
		
				if (Mathf.Approximately((int)actionTime, (int)roundedTime))
				{

					if (dayofweek == actionDay || actionDay == 7)
					{
						StartCoroutine(execAction(i));
					}
				
					ListofActionsCopy.RemoveAt(i);
				}
			}
		}
		else
			if (usingRealTimeSpeed == true)	
			{
				roundedTime = System.DateTime.Now.TimeOfDay.Hours;
				roundedTime += ((System.DateTime.Now.TimeOfDay.Minutes) / 60.0f);


				for (int i =0; i < ListofActionsCopy.Count; i++)
				{
					actionTime = ListofActionsCopy[i].actiontime;
					int actionDay = (int)ListofActionsCopy[i].Day;


					if (actionTime == roundedTime)
					{
						if (dayofweek == actionDay || actionDay == 7)
						{
							StartCoroutine(execAction(i));
						}
						ListofActionsCopy.RemoveAt(i);
					}
				}
				
				
			}
		
	}
	
	public static string FormatTime(bool showSeconds, bool showHours)
	{
		string timeDisplay;
		string  amPmDesignator = "AM";
		
		
		if (usingST && usingRT)
		{
			int systemHours = System.DateTime.Now.Hour;
			int systemMinutes = System.DateTime.Now.Minute;
			int systemSeconds = System.DateTime.Now.Second;

			if (showSeconds == true)
			{
				if (showHours == true)
				{
					if (systemHours == 0)
						systemHours = 12;
					else if (systemHours == 12)
						amPmDesignator = "PM";
					else if (systemHours > 12) {
						systemHours -= 12;
						amPmDesignator = "PM";
					}
					timeDisplay =	string.Format("{0}:{1:00}:{2:00} {3}", systemHours, systemMinutes, systemSeconds, amPmDesignator);
				}
				
				else
				{
					timeDisplay = string.Format ("{0:D2}:{1:D2}:{2:D2}", (int) System.DateTime.Now.Hour, System.DateTime.Now.Minute, System.DateTime.Now.Second);

				}

			}
			
			else {
				
				if (showHours == true)
				{
					if (systemHours == 0)
						systemHours = 12;
					else if (systemHours == 12)
						amPmDesignator = "PM";
					else if (systemHours > 12) {
						systemHours -= 12;
						amPmDesignator = "PM";
					}
					timeDisplay =	string.Format("{0}:{1:00} {2}", systemHours, systemMinutes, amPmDesignator);
				}
				
				else
				{
					timeDisplay = string.Format ("{0:D2}:{1:D2}", (int) System.DateTime.Now.Hour, System.DateTime.Now.Minute);
				}
				
				

			}
		}
		else
		{
			var timespan = TimeSpan.FromHours ((float)time);
			int hours = timespan.Hours;
			int minutes = timespan.Minutes;
			int seconds = timespan.Seconds;

			if (showSeconds == true)
			{
				if (showHours == true)
				{
					if (hours == 0)
						hours = 12;
					else if (hours == 12)
						amPmDesignator = "PM";
					else if (hours > 12) {
						hours -= 12;
						amPmDesignator = "PM";
					}
					timeDisplay = string.Format("{0}:{1:00}:{2:00} {3}", hours, minutes, seconds, amPmDesignator);
				}
			
				
				else
				{
			
					timeDisplay = string.Format ("{0:D2}:{1:D2}:{2:D2}", (int) timespan.Hours, timespan.Minutes, timespan.Seconds);
				
				}
			}
			
			else
			{
				
				if (showHours == true)
				{
					if (hours == 0)
						hours = 12;
					else if (hours == 12)
						amPmDesignator = "PM";
					else if (hours > 12) {
						hours -= 12;
						amPmDesignator = "PM";
					}
					timeDisplay = string.Format("{0}:{1:00} {2}", hours, minutes, amPmDesignator);
				}
				else
				{
					timeDisplay = string.Format ("{0:D2}:{1:D2}", (int) timespan.Hours, timespan.Minutes);
				}
			

			}
		}
				
		
		return timeDisplay;
	}
 
 
 
	IEnumerator execAction(int action) 
	{
		actionsToExecute = ListofActionsCopy[action].actionToExecute;

		if (actionsToExecute != null)
		{
			actionsToExecute.actionsList.Execute(gameObject, this.OnCompleteActions);

			
		}

		yield return 0;

	}

	IEnumerator OnColorsChanged()
	{
		sunColor = (Color)newSunColor;
		ambientDay = (Color)newAmbientDay;
		ambientNight = (Color)newAmbientNight;
		if (sunCurveBool == true)
		{
			sunCurve = false;
		}
		else 
		{
			sunCurve = true;
		}
		colorsChanged = false;
		yield return 0;
	}
	
	IEnumerator OnSeasonChanged()
	{
		seasonChanged = false;
		sunSeason = (int)newsunSeason;
		yield return 0;
	}
	
	IEnumerator OnSBRotationChanged()
	{
		sbRotationChanged = false;
		skyboxRotate = sbRotation;
		skyboxRotateSpeed = newskyboxRotateSpeed;
		yield return 0;
	}
	
	IEnumerator OnSBExposureChanged()
	{
		sbExposureChanged = false;
		float lerpDuration = newskyboxExposureSpeed; 
		float startValue = oldskyboxExposureSetting; 
		float endValue = newskyboxExposureSetting; 
		float valueToLerp;
		float timeElapsed = 0;
		
		while (timeElapsed < lerpDuration)
		{
			valueToLerp = Mathf.Lerp(startValue, endValue, timeElapsed / lerpDuration);
			timeElapsed += Time.deltaTime;
			RenderSettings.skybox.SetFloat("_Exposure", valueToLerp); 
			yield return null;
		}

		oldskyboxExposureSetting = newskyboxExposureSetting;
	
		yield return 0;
	}
	
	
	IEnumerator OnSetTOD()
	{
		setTOD = false;
		if (usingRealTimeSpeed == false) {
			time = newTOD;
		}
		
		yield return 0;
	}
	
	IEnumerator OnAdvTOD()
	{
		advTOD = false;
		if (usingRealTimeSpeed == false) {
			time += newadvTOD;
		}

		yield return 0;
	}
	IEnumerator OnSetDOW()
	{
		setDOW = false;
		if (usingRealTimeSpeed == false) {
			dayofweek = dayofWeek;
		}

		yield return 0;
	}
	
	
	private void OnCompleteActions()
	{	
           
	}

	}
}
 
