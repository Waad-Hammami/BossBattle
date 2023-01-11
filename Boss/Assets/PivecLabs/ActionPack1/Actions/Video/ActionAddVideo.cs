namespace PivecLabs.ActionPack
    {
        using System.Collections;
        using System.Collections.Generic;
        using UnityEngine;
        using UnityEngine.Events;
        using GameCreator.Core;
        using GameCreator.Variables;

#if UNITY_EDITOR
    using UnityEditor;
#endif

        [AddComponentMenu("")]
        public class ActionAddVideo : IAction
        {
        public enum VIDEOORIGIN
        {
            Local,
            URL
        }

        public VIDEOORIGIN videoOrigin = VIDEOORIGIN.Local;

        public UnityEngine.Video.VideoClip localVideo;

        public StringProperty url = new StringProperty();

	    public TargetGameObject target = new TargetGameObject();


     // EXECUTABLE: ----------------------------------------------------------------------------

        public override bool InstantExecute(GameObject target, IAction[] actions, int index)
            {

	            GameObject targetObject = this.target.GetGameObject(target);
	            if (targetObject != null) {
	            	
	           
                var videoPlayer = targetObject.AddComponent<UnityEngine.Video.VideoPlayer>();
                var audioSource = targetObject.AddComponent<AudioSource>();
                videoPlayer.playOnAwake = false;

            switch (this.videoOrigin)
            {
                case VIDEOORIGIN.Local:
                   videoPlayer.clip = localVideo;
                    break;
                case VIDEOORIGIN.URL:
                   videoPlayer.url = this.url.GetValue(target);
                     break;
            }

                 
                videoPlayer.renderMode = UnityEngine.Video.VideoRenderMode.MaterialOverride;
	            videoPlayer.playOnAwake = false;
  
                videoPlayer.targetMaterialRenderer = targetObject.GetComponent<Renderer>();
                videoPlayer.targetMaterialProperty = "_MainTex";
                videoPlayer.audioOutputMode = UnityEngine.Video.VideoAudioOutputMode.AudioSource;
                videoPlayer.SetTargetAudioSource(0, audioSource);

	            }

                return true;
            }

            public override IEnumerator Execute(GameObject target, IAction[] actions, int index)
            {

                return base.Execute(target, actions, index);
            }

            // +--------------------------------------------------------------------------------------+
            // | EDITOR                                                                               |
            // +--------------------------------------------------------------------------------------+

#if UNITY_EDITOR

	        public static new string NAME = "ActionPack2/Video/Add Video to Object";
            private const string NODE_TITLE = "Place Video on {0}";
	        public const string CUSTOM_ICON_PATH = "Assets/PivecLabs/ActionPack2/Icons/";

            // PROPERTIES: ----------------------------------------------------------------------------

             private SerializedProperty spvideoOrigin;
             private SerializedProperty spvideoClip;
             private SerializedProperty spvideoURL;
	         private SerializedProperty sptargetObject;

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
                this.spvideoOrigin = this.serializedObject.FindProperty("videoOrigin");
                this.spvideoClip = this.serializedObject.FindProperty("localVideo");
                this.spvideoURL = this.serializedObject.FindProperty("url");
	            this.sptargetObject = this.serializedObject.FindProperty("target");
            }

            protected override void OnDisableEditorChild()
            {
                this.spvideoOrigin = null;
                this.spvideoClip = null;
                this.spvideoURL = null;
	            this.sptargetObject = null;

        }

        public override void OnInspectorGUI()
            {
                this.serializedObject.Update();

                EditorGUILayout.PropertyField(this.spvideoOrigin, new GUIContent("Video Origin"));
         
            switch ((VIDEOORIGIN)this.spvideoOrigin.intValue)
            {
                case VIDEOORIGIN.Local:
                    EditorGUILayout.PropertyField(this.spvideoClip, new GUIContent("Video Clip"));
                    break;
                case VIDEOORIGIN.URL:
                     EditorGUILayout.PropertyField(this.spvideoURL, new GUIContent("URL"));
                    break;
               
            }
            EditorGUILayout.Space();

	            EditorGUILayout.PropertyField(this.sptargetObject, new GUIContent("Target Object"));

            this.serializedObject.ApplyModifiedProperties();
            }

#endif
        }
    }
