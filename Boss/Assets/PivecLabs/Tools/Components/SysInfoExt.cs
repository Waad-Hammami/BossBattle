namespace PivecLabs.Tools
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.UI;
	using UnityEngine.Profiling;

	public class SysInfoExt : MonoBehaviour
{
	
	public static bool sysinfo = false;
	public Text fps;
	public Text min;
	public Text max;
	public Text memalloc;
	public Text memtotal;
	public Text memgxf;
	public GameObject txtgxf;
	
	
	private float sys_accumulatedTime;
	private int sys_accumulatedFrames;
	private float sys_lastUpdateTime;
	private float sys_frameTime = 0.0f;
	private float sys_frameRate = 0.0f;
	private float sys_minFrameRate = 0.0f;
	private float sys_maxFrameRate = 0.0f;
	private float sys_minFrameTime = 0.0f;
	private float sys_maxFrameTime = 0.0f;
	public static float updateInterval = 0.5f;
	public static float minTime = 0.000000001f; 
	
	public Text os;
	public Text type;
	public Text graphics;
	public Text gfx;
	public Text window;

	public Text product;
	public Text version;
	public Text company;
	public Text build;
	public Text language;
	public Text platform;
	public Text genuine;



	// Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
	    if (sysinfo == true)
	    {
		    float deltaTime = Time.unscaledDeltaTime;

		    sys_accumulatedTime += deltaTime;
		    sys_accumulatedFrames++;

		    if (deltaTime < minTime) {
			    deltaTime = minTime;
		    }

		    if (deltaTime < sys_minFrameTime) {
			    sys_minFrameTime = deltaTime;
		    }

		    if (deltaTime > sys_maxFrameTime) {
			    sys_maxFrameTime = deltaTime;
		    }

		    float nowTime = Time.realtimeSinceStartup;
		    if (nowTime - sys_lastUpdateTime < updateInterval) {
			    return;
		    }

		    if (sys_accumulatedTime < minTime) {
			    sys_accumulatedTime = minTime;
		    }

		    if (sys_accumulatedFrames < 1) {
			    sys_accumulatedFrames = 1;
		    }

		    sys_frameTime = sys_accumulatedTime / sys_accumulatedFrames;
		    sys_frameRate = 1.0f / sys_frameTime;

		    sys_minFrameRate = 1.0f / sys_maxFrameTime;
		    sys_maxFrameRate = 1.0f / sys_minFrameTime;


		    UpdateDisplayContent();

		    ResetCounters();
	    
		    sys_lastUpdateTime = nowTime;
	    
	      	
	    	os.text = SystemInfo.operatingSystem;
		    type.text = SystemInfo.processorType;
		    graphics.text = SystemInfo.graphicsDeviceName;
		    gfx.text = string.Format("{0} - Shader: {1} - VRam: {2}MB", SystemInfo.graphicsDeviceVersion, SystemInfo.graphicsShaderLevel, SystemInfo.graphicsMemorySize.ToString());
		    window.text = string.Format("Window size: {1} x {2} - {3}dpi {0}Hz ",Screen.currentResolution.refreshRate, Screen.width, Screen.height, Screen.dpi);
		   
		    product.text = string.Format("{0}{1}", "Product name = ", Application.productName);
		    version.text = string.Format("{0}{1}", "Product version = ", Application.version);
		    company.text = string.Format("{0}{1}", "Company name = ", Application.companyName);
		    build.text = string.Format("{0}{1}", "Unity Build version = ", Application.unityVersion);
		    language.text = string.Format("{0}{1}", "Local Operating System Language = ", Application.systemLanguage);
		    platform.text = string.Format("{0}{1}", "Current Platform = ", Application.platform);
		    genuine.text = string.Format("{0}{1}", "Verified app integrity = ", Application.genuine);

	    }
	    
	    
    }
    
	private void UpdateDisplayContent() {
	
		fps.text = string.Format("{0} - {1} ms", sys_frameRate.ToString("F1"), (sys_frameTime * 1000.0f).ToString("F1")); 
		min.text = string.Format("{0} - {1} ms", sys_minFrameRate.ToString("F1"), (sys_maxFrameTime * 1000.0f).ToString("F1")); 
		max.text = string.Format("{0} - {1} ms", sys_maxFrameRate.ToString("F1"), (sys_minFrameTime * 1000.0f).ToString("F1")); 
	
		memtotal.text = string.Format("{0:0.0#} MB", ConvertBytesToMegabytes(Profiler.GetTotalReservedMemoryLong()));
		memalloc.text = string.Format("{0:0.0#} MB", ConvertBytesToMegabytes(Profiler.GetTotalAllocatedMemoryLong()));
		if (Debug.isDebugBuild)
		{
			memgxf.text = string.Format("{0:0.0#} MB", ConvertBytesToMegabytes(Profiler.GetAllocatedMemoryForGraphicsDriver()));
		}
		else
		{
			memgxf.text = "";
			txtgxf.SetActive(false);
		}
		
		
	}

	private void ResetCounters() {
		sys_minFrameTime = float.MaxValue;
		sys_maxFrameTime = float.MinValue;
		sys_accumulatedTime = 0.0f;
		sys_accumulatedFrames = 0;
	}
	
	static double ConvertBytesToMegabytes(long bytes)
	{
		return (bytes / 1024f) / 1024f;
	}
 }
}
