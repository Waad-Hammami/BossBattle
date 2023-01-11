namespace PivecLabs.Tools
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.Events;
	using UnityEngine.EventSystems;
	using UnityEngine.AI;
	using UnityEngine.SceneManagement;
	using System.ComponentModel;
	using System.Security.Cryptography;
	using System.Text;
	using System.Linq;


	using System;	
	using System.Net;
	using System.Net.Sockets;
	using GameCreator.Core;
	using GameCreator.Core.Hooks;
	using GameCreator.Characters;
	using GameCreator.Variables;

	[AddComponentMenu("Game Creator/Tools/Time Manager Component", 100)]

	public class TimeManager : MonoBehaviour

	{

		public static bool pingNtpHost;

		private static string ntphost;

	[SerializeField]
	 public enum ntpHosts
	 {
		 [InspectorName("pool.ntp.org")] poolntporg,
		 [InspectorName("europe.pool.ntp.org")] europepoolntporg,
		 [InspectorName("north-america.pool.ntp.org")] napoolntporg,
		 [InspectorName("asia.pool.ntp.org")] asiapoolntporg,
		 [InspectorName("us.pool.ntp.org")] uspoolntporg,
		 [InspectorName("uk.pool.ntp.org")] ukpoolntporg,
		 [InspectorName("de.pool.ntp.org")] depoolntporg
	 }

	 [SerializeField]
	 public ntpHosts ntpH = ntpHosts.poolntporg;
	 
		[SerializeField] 
		[VariableFilter(Variable.DataType.String)]
		public VariableProperty startNtpTime = new VariableProperty(Variable.VarType.GlobalVariable);

		[SerializeField] 
		[VariableFilter(Variable.DataType.String)]
		public VariableProperty storedNtpTime = new VariableProperty(Variable.VarType.GlobalVariable);


		[SerializeField] 
		[VariableFilter(Variable.DataType.String)]
		public VariableProperty LastUpdateTime = new VariableProperty(Variable.VarType.GlobalVariable);


		
		private bool processing;
		[SerializeField]
		public bool repeatPing;
		[SerializeField]
		[Range (1, 600)]
		public int pingInterval = 1;
		[SerializeField]
		public bool startVar;
		[SerializeField]
		public bool storeVar;
		[SerializeField]
		public bool encryptVar;
		[SerializeField]
		public string encryptKey = "abcdefghijklmnopqrstuvwxyz123456";


		[System.Serializable]
		public class ActionObject
		{
			public Actions actionToExecute;
			public int Every;
			public enum FREQUENCY
			{
				Minutes,
				Hours,
				Days,
				Weeks,
				Months
            
			}
			public FREQUENCY realTime = FREQUENCY.Hours;	
			
			[HideInInspector]
			public double actiontime;  
			
   
		}
		
		[SerializeField]
		[Range(1, 60)]
		public int waitAtStart;

		private double actionTime;

		[SerializeField]
		public List<ActionObject> ListofActions = new List<ActionObject>();

		Actions actionsToExecute = null;
	
		public bool executeAllActions = false;

		private bool actionsComplete = false;
		private double minute =  double.Parse("60000");
		private double hour =  double.Parse("3600000");
		private double day =  double.Parse("86400000");
		private double week =  double.Parse("604800000");
		private double month =  double.Parse("2629746000");
		
		System.Double lastexectime;	 
		Double localtime;

		private double[] actionsLastUpdate;
		private string exeVariable;
		private string started;
		private bool updatingActions = false;
		
		void Awake()
    {
	    
	    switch (this.ntpH)
	    {
	    case ntpHosts.poolntporg: 
		    ntphost = "pool.ntp.org";
		    break;
	    case ntpHosts.europepoolntporg: 
		    ntphost = "europe.pool.ntp.org";
		    break;
	    case ntpHosts.napoolntporg: 
		    ntphost = "north-america.pool.ntp.org";
		    break;
	    case ntpHosts.asiapoolntporg: 
		    ntphost = "asia.pool.ntp.org";
		    break;
	    case ntpHosts.uspoolntporg: 
		    ntphost = "us.pool.ntp.org";
		    break;
	    case ntpHosts.ukpoolntporg: 
		    ntphost = "uk.pool.ntp.org";
		    break;
	    case ntpHosts.depoolntporg: 
		    ntphost = "de.pool.ntp.org";
		    break;
	  
	    }    
	 
	   
	    
	    Double localtime = LocalTime();
	    System.Double ntptime = NtpTime();
	    
	    if (repeatPing == true)
	    {
		    InvokeRepeating("PingNtpServer", pingInterval, pingInterval);
	    }
	    
	    if (storeVar == true)
	    {	   
		    if (encryptVar == true)
		    {	
			    byte[] encrypted = encrypting(ntptime.ToString(), encryptKey);
			    string encryptedString = System.Convert.ToBase64String(encrypted);
			    this.storedNtpTime.Set(encryptedString);	 
		    }
		    else
		    {
			    this.storedNtpTime.Set(ntptime.ToString());	 
		    }
	    }

	    if (startVar == true)
	    {	 
	    	
		    started = (string)this.startNtpTime.Get();	    		
	    	if (started == "") 
	    	{
		    	started="0";

	    	}

	    	else
		        if (encryptVar == true)
		    	 {
			    
			      byte[] encryptedBytes = System.Convert.FromBase64String(started);
			       started = decrypting(encryptedBytes, encryptKey);
	    	
		    	}
	    	
	    	
	    	float initTime = float.Parse(started);
	    	
		    	
	    	if (initTime < 1)
	    	{
		    	if (encryptVar == true)
		    	{	
			    	byte[] encrypted = encrypting(ntptime.ToString(), encryptKey);
			    	string encryptedString = System.Convert.ToBase64String(encrypted);
			    	this.startNtpTime.Set(encryptedString);	 
			    	
			    	actionsLastUpdate = new double[ListofActions.Count];
			    	for (int i =0; i < ListofActions.Count; i++)
			    	{
				    	actionsLastUpdate[i] = ntptime;
				    	string result1 = string.Join(",", actionsLastUpdate);
				    	byte[] encrypted2 = encrypting(result1, encryptKey);
				    	string encryptedString2 = System.Convert.ToBase64String(encrypted2);
				    	this.LastUpdateTime.Set(encryptedString2);	
			    	}
			    	
			    	
		    	}
		    	
		    	else
		    	
		    	{
			    	this.startNtpTime.Set(ntptime.ToString());	
			    	
			    	actionsLastUpdate = new double[ListofActions.Count];

			    	for (int i =0; i < ListofActions.Count; i++)
			    	{
				    	actionsLastUpdate[i] = ntptime;
				    	string result1 = string.Join(",", actionsLastUpdate);
				    	this.LastUpdateTime.Set(result1);	
			    	}
		    	}
	    	}


	    }
	    
	    for (int i =0; i < ListofActions.Count; i++)
	    {
		    
		    switch (ListofActions[i].realTime)
		    {
		    case ActionObject.FREQUENCY.Minutes:
			    actionTime = (minute * ListofActions[i].Every);
			    break;
		    case ActionObject.FREQUENCY.Hours:
			    actionTime = (hour * ListofActions[i].Every);
		    	 break;
		    case ActionObject.FREQUENCY.Days:
			    actionTime = (day * ListofActions[i].Every);
		    	 break;
		    case ActionObject.FREQUENCY.Weeks:
			    actionTime = (week * ListofActions[i].Every);
		    	 break;
		    case ActionObject.FREQUENCY.Months:
			    actionTime = (month * ListofActions[i].Every);
		    	 break;
		    }
			
		    ListofActions[i].actiontime = (int)actionTime;

	    }

	    if	(ListofActions.Count > 0)
	    	InvokeRepeating("OnInvokeActions", waitAtStart, 60);

    }

  void Update()
		{
			if (pingNtpHost == true)
			{
				pingNtpHost = false;
				PingNtpServer();
			}
			
			
		}	

		public void OnInvokeActions()
		{
			if (encryptVar == true)
			{	
				byte[] encryptedBytes = System.Convert.FromBase64String(this.LastUpdateTime.ToStringValue(gameObject));
				exeVariable = decrypting(encryptedBytes, encryptKey);
			}
			else
			{
				exeVariable = this.LastUpdateTime.ToStringValue(gameObject);	 

			}
			
		
			actionsLastUpdate = exeVariable.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(double.Parse).ToArray();
			localtime = LocalTime();
			
			updatingActions = true;
				
				
			while (updatingActions == true)
			
			{
				
				updatingActions = false;

				for (int i =0; i < ListofActions.Count; i++)
				{

					
					if ((actionsLastUpdate[i] + ListofActions[i].actiontime) <= (localtime+10000))
				
					{
						updatingActions = true;

						StartCoroutine(execAction(i));
	
					}
				}
			}

		
			
		}


		IEnumerator execAction(int action) 
		{
			actionsToExecute = ListofActions[action].actionToExecute;

			if (actionsToExecute != null)
			{
				this.actionsComplete = false;
				
				actionsToExecute.actionsList.Execute(gameObject, this.OnCompleteActions);
					
				WaitUntil wait = new WaitUntil(() =>
					{
						if (actionsToExecute == null) return true;
					
						return this.actionsComplete;
					});

				actionsLastUpdate[action] = (actionsLastUpdate[action] + ListofActions[action].actiontime);
				string result1 = string.Join(",", actionsLastUpdate);
						
				if (encryptVar == true)
				{	
					byte[] encrypted3 = encrypting(result1, encryptKey);
					string encryptedString3 = System.Convert.ToBase64String(encrypted3);
					this.LastUpdateTime.Set(encryptedString3);	
				}
				else
				{
					this.LastUpdateTime.Set(result1);	
				}
						

					yield return wait;
			
			}

			yield return 0;

		}
		
		public void OnCompleteActions()
		{
			this.actionsComplete = true;
		}


		public	void PingNtpServer()
		{
			if(!processing)
			{
				processing = true;
				System.Double ntptime = NtpTime();

			
				if (storeVar == true)
				{	   
					if (encryptVar == true)
					{	
						byte[] encrypted = encrypting(ntptime.ToString(), encryptKey);
						string encryptedString = System.Convert.ToBase64String(encrypted);
						this.storedNtpTime.Set(encryptedString);	 
					}
					else
					{
						this.storedNtpTime.Set(ntptime.ToString());	 
					}
		      
				}

			
				processing = false;
			}
		}
    
	public double NtpTime ()
		{
		
	
			
		try
		{
			byte[] ntpData = new byte[48];
     
			ntpData[0] = 0x1B;
     
			IPAddress[] addresses = Dns.GetHostEntry (ntphost).AddressList;
			Socket socket = new Socket (AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
     
			socket.Connect (new IPEndPoint (addresses[0], 123));
			socket.ReceiveTimeout = 1000;
     
			socket.Send (ntpData);
			socket.Receive (ntpData);
			socket.Close ();
     
			ulong intc = (ulong) ntpData[40] << 24 | (ulong) ntpData[41] << 16 | (ulong) ntpData[42] << 8 | (ulong) ntpData[43];
			ulong frac = (ulong) ntpData[44] << 24 | (ulong) ntpData[45] << 16 | (ulong) ntpData[46] << 8 | (ulong) ntpData[47];
     
   
			return (double) ((intc * 1000) + ((frac * 1000) / 0x100000000L));
		}
			catch (Exception exception)
			{
				Debug.Log ("Could not get NTP time");
				Debug.Log (exception);
				return LocalTime ();
			}
			
		
	}
 
	public static double LocalTime ()
		{
			
			string rounded = (DateTime.UtcNow.Subtract (new DateTime (1900, 1, 1)).TotalMilliseconds).ToString("F0");
			double time1 = Convert.ToInt64(rounded);
			return time1;

		
	}
	
	public static byte[] encrypting(string toEncrypt, string key)
		{
			try
			{
				byte[] keyArray = Encoding.UTF8.GetBytes(key);
				byte[] toEncryptArray = Encoding.UTF8.GetBytes(toEncrypt);
				RijndaelManaged rManaged = new RijndaelManaged();
				rManaged.Key = keyArray;
				rManaged.Mode = CipherMode.ECB;
				rManaged.Padding = PaddingMode.ISO10126;
				ICryptoTransform cTransform = rManaged.CreateEncryptor();
				return cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
			}
				catch
				{
					Debug.Log("Encrypting Failed");
				}
			return null;
		}
		
		public static string decrypting(byte[] toEncryptArray, string key)
		{
			try
			{
            
				byte[] keyArray = Encoding.UTF8.GetBytes(key);
				RijndaelManaged rManaged = new RijndaelManaged();
				rManaged.Key = keyArray;
				rManaged.Mode = CipherMode.ECB;
				rManaged.Padding = PaddingMode.ISO10126;
				ICryptoTransform cTransform = rManaged.CreateDecryptor();
				byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
				return Encoding.UTF8.GetString(resultArray);
			}
				catch
            
				{
					Debug.Log("Decrypting Failed");
				}
			return null;
		}


	}


}