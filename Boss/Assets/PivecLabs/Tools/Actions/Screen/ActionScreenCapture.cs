namespace PivecLabs.Tools
{
        using System.Collections;
        using System.Collections.Generic;
        using UnityEngine;
        using UnityEngine.Events;
        using UnityEngine.UI;
        using UnityEngine.Video;
        using GameCreator.Core;
        using GameCreator.Variables;
        using GameCreator.Core.Hooks;
        using GameCreator.Characters;
		using System.Linq;
		
#if UNITY_EDITOR
    using UnityEditor;
#endif



	
    [AddComponentMenu("")]
	public class ActionScreenCapture : IAction
    {

	    public enum CAPTURE
	    {
		    LowRes,
		    HighRes,
		    Thumbnail
		   
	    }

	    public CAPTURE imageMode = CAPTURE.LowRes;

	    public enum ENCODE
	    {
		    png,
		    jpg,
		    tga
	    }
	    
	    public ENCODE imageformat = ENCODE.png;
	    public string imagePath;
	    public bool logdata;
	    [Range(0.0f,10f)]
	    [SerializeField]
	    public float timer = 0;
	    [Range(0,60)]
	    [SerializeField]
	    public float repeat = 0;
	    [Range(1,20)]
	    [SerializeField]
	    public int imagesize = 1;
	    
	    public TargetGameObject target = new TargetGameObject();
	    public Actions actions;
	    private byte[] imageBytes;
	    private string screenShot;
	    
	    int resWidth = Screen.width*4; 
	    int resHeight = Screen.height*4;
	    public Camera captureCamera;
	    int scale = 1;
	    RenderTexture renderTexture;

	    private bool resizeimage;
	    
        // EXECUTABLE: ----------------------------------------------------------------------------

        public override bool InstantExecute(GameObject target, IAction[] actions, int index)
        {
	        
	        switch (this.imageMode)
	        {
	        case CAPTURE.LowRes:
		        resizeimage = false;

		        if (repeat > 0)
		        {
			        InvokeRepeating("captureLoRes", 0.0f, repeat);
		        
		        }
		        else
		        {
		        	captureLoRes();
		        }
		        
		        break;
	        case CAPTURE.HighRes:
		        resizeimage = false;

	         if (repeat > 0)
		        {
			        InvokeRepeating("captureHiRes", 0.0f, repeat);
		        
		        }
		        else
		        {
		        	captureHiRes();
		        }
		        
		        break;
	        case CAPTURE.Thumbnail:
	        	
	        	resizeimage = true;

		        if (repeat > 0)
		        {
			        InvokeRepeating("captureLoRes", 0.0f, repeat);
		        
		        }
		        else
		        {
		        	captureLoRes();
		        }
		        break;

	        }
	        
	      

	        return true;

        }

        public override IEnumerator Execute(GameObject target, IAction[] actions, int index)
        {
	      
	        return base.Execute(target, actions, index);
	       
        }
        
	    private void captureLoRes()
	    {
	    	StartCoroutine(captureScreenshot());
	    }
	    
	    private void captureHiRes()
	    {
		    StartCoroutine(captureHiResScreenshot());
	    }
	    


	    public void StopRepeating()
	    {
		    CancelInvoke("captureLoRes");
		    CancelInvoke("captureHiRes");
		    CancelInvoke("captureThumbnail");
	    }


	    IEnumerator captureScreenshot()
	    {
		    yield return new WaitForSeconds(timer);
		    yield return new WaitForEndOfFrame();
		    	        
		    screenShot = string.Format("{0}{1}{2}{3}", imagePath,System.DateTime.Now.Ticks,".",imageformat);

		    Texture2D screenImage = new Texture2D(Screen.width, Screen.height);
		    screenImage.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
		    screenImage.Apply();
		    
		    
		    if (resizeimage == true)
		    {
			    screenImage = Resize(screenImage, screenImage.width/imagesize, screenImage.height/imagesize);
		    }
		    

		    
		    switch (this.imageformat)
		    {
		    case ENCODE.png:
			    imageBytes = screenImage.EncodeToPNG();
			    break;
		    case ENCODE.jpg:
			    imageBytes = screenImage.EncodeToJPG();
			    break;
		    case ENCODE.tga:
			    imageBytes = screenImage.EncodeToTGA();
			    break;
		    }
		    
		    if(Application.isMobilePlatform)
		    {
			    System.IO.File.WriteAllBytes(Application.persistentDataPath+"/"+screenShot, imageBytes);

		    }
		    else
		    {
		    	
			    System.IO.File.WriteAllBytes(screenShot, imageBytes);

		    }
		    if (logdata == true)
		    {
			    Debug.Log("screenImage.width" + screenImage.width);		
			    Debug.Log("imagesBytes=" + imageBytes.Length);
			    Debug.Log("imagePath=" + screenShot);
		    }
	    }
	    
	    IEnumerator captureHiResScreenshot()
	    {

		    yield return new WaitForSeconds(timer);
		    yield return new WaitForEndOfFrame();
	        
		    screenShot = string.Format("{0}{1}{2}{3}", imagePath,System.DateTime.Now.Ticks,".",imageformat);

	    	int resWidthN = resWidth*scale;
	    	int resHeightN = resHeight*scale;
	    	RenderTexture rt = new RenderTexture(resWidthN, resHeightN, 24);
	    	captureCamera.targetTexture = rt;

	    	Texture2D screenImage = new Texture2D(resWidthN, resHeightN, TextureFormat.RGB24,false);
	    	captureCamera.Render();
		    RenderTexture.active = rt;
	    	screenImage.ReadPixels(new Rect(0, 0, resWidthN, resHeightN), 0, 0);
	    	captureCamera.targetTexture = null;
		    RenderTexture.active = null; 

		    switch (this.imageformat)
		    {
		    case ENCODE.png:
			    imageBytes = screenImage.EncodeToPNG();
			    break;
		    case ENCODE.jpg:
			    imageBytes = screenImage.EncodeToJPG();
			    break;
		    case ENCODE.tga:
			    imageBytes = screenImage.EncodeToTGA();
			    break;
		    }
		    
		    if(Application.isMobilePlatform)
		    {
			    System.IO.File.WriteAllBytes(Application.persistentDataPath+"/"+screenShot, imageBytes);

		    }
		    else
		    {
		    	
			    System.IO.File.WriteAllBytes(screenShot, imageBytes);

		    }
		    if (logdata == true)
		    {
			    Debug.Log("HiResImage.width" + screenImage.width);		
			    Debug.Log("imagesBytes=" + imageBytes.Length);
			    Debug.Log("imagePath=" + screenShot);
		    }
	    
	   
    	}
	    
	    public static Texture2D Resize(Texture2D source, int newWidth, int newHeight)
	    {
		    source.filterMode = FilterMode.Point;
		    RenderTexture rt = RenderTexture.GetTemporary(newWidth, newHeight);
		    rt.filterMode = FilterMode.Point;
		    RenderTexture.active = rt;
		    Graphics.Blit(source, rt);
		    Texture2D nTex = new Texture2D(newWidth, newHeight);
		    nTex.ReadPixels(new Rect(0, 0, newWidth, newHeight), 0,0);
		    nTex.Apply();
		    RenderTexture.active = null;
		    RenderTexture.ReleaseTemporary(rt);
		    return nTex;
	    }
	    
	
        // +--------------------------------------------------------------------------------------+
        // | EDITOR                                                                               |
        // +--------------------------------------------------------------------------------------+

#if UNITY_EDITOR

	    public static new string NAME = "Developer Tools/Capture/ScreenCapture";
	    private const string NODE_TITLE = "Screen Capture";
	    public const string CUSTOM_ICON_PATH = "Assets/PivecLabs/Tools/Icons/";

        // PROPERTIES: ----------------------------------------------------------------------------

	    private SerializedProperty spimagemode;
	    private SerializedProperty spcamera;
      
	    private SerializedProperty spimagePath;
	    private SerializedProperty spimageData;
	    private SerializedProperty spimageType;
	    private SerializedProperty spimageSize;
	    private SerializedProperty sphiRes;
	    private SerializedProperty sptimer;
	    private SerializedProperty sprepeat;


        // INSPECTOR METHODS: ---------------------------------------------------------------------

        public override string GetNodeTitle()
            {
                return string.Format(
	                NODE_TITLE,
	                this.target
                 );
            }

        protected override void OnEnableEditorChild()
            {
	            this.spimagemode = this.serializedObject.FindProperty("imageMode");
	            
	            this.spcamera = this.serializedObject.FindProperty("camera");
     
	            this.spimagePath = this.serializedObject.FindProperty("imagePath");
	            this.spimageData = this.serializedObject.FindProperty("logdata");
	            this.spimageType = this.serializedObject.FindProperty("imageformat");
	            this.spimageSize = this.serializedObject.FindProperty("imagesize");
	            this.sphiRes = this.serializedObject.FindProperty("hires");
	            this.sptimer = this.serializedObject.FindProperty("timer");
	            this.sprepeat = this.serializedObject.FindProperty("repeat");

        }


        protected override void OnDisableEditorChild()
            {
          
	            this.spimagemode = null;
	            this.spcamera = null;
	            this.spimagePath = null;
	            this.spimageData = null;
	            this.spimageType = null;
	            this.spimageSize = null;
	            this.sphiRes = null;
	            this.sptimer = null;
	            this.sprepeat = null;

        }

        public override void OnInspectorGUI()
            {
                this.serializedObject.Update();
            	EditorGUILayout.LabelField("ScreenShot", EditorStyles.boldLabel);
	            EditorGUILayout.Space();

	            EditorGUILayout.PropertyField(this.spimagemode, new GUIContent("Image Mode"));
	            EditorGUILayout.Space();
	            switch ((CAPTURE)this.spimagemode.intValue)
	            {
	            case CAPTURE.LowRes:
		            EditorGUI.indentLevel++;
		            EditorGUILayout.PropertyField(this.spimagePath, new GUIContent("Image Prefix"));
		            EditorGUILayout.PropertyField(this.spimageType, new GUIContent("Image Format"));
		            EditorGUI.indentLevel--;
		            break;
	            case CAPTURE.HighRes:
		            EditorGUI.indentLevel++;
		            EditorGUILayout.PropertyField(this.spimagePath, new GUIContent("Image Prefix"));
		            EditorGUILayout.PropertyField(this.spimageType, new GUIContent("Image Format"));
		            EditorGUILayout.PropertyField(this.spcamera, new GUIContent("HiRes Camera"));
		            EditorGUILayout.PropertyField(this.sptimer, new GUIContent("Delay Start (sec)"));
		            EditorGUILayout.PropertyField(this.sprepeat, new GUIContent("Repeat Every (sec)"));

		            EditorGUI.indentLevel--;

		            break;
	            case CAPTURE.Thumbnail:
		            EditorGUILayout.PropertyField(this.spimagePath, new GUIContent("Image Prefix"));
		            EditorGUILayout.PropertyField(this.spimageType, new GUIContent("Image Format"));
		            EditorGUILayout.PropertyField(this.spimageSize, new GUIContent("Reduce Size by "));

		            break;
		            
	           

	            }
	            EditorGUILayout.Space();

	            EditorGUILayout.PropertyField(this.spimageData, new GUIContent("Log Data"));
	            EditorGUILayout.Space();

	            
            this.serializedObject.ApplyModifiedProperties();
            }
	    

	 
#endif

        }
    }
