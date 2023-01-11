namespace PivecLabs.Tools
{
	using System;
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
		
	using UnityEditor;
	using UnityEditorInternal;


public enum ManipulationModes
{
    Free
}

public enum NewWaypointMode
{
    SceneCamera,
    LastWaypoint,
    WaypointIndex,
    WorldCenter
}

	[CustomEditor(typeof(PathManager))]
	public class PathManagerEditor : Editor
{
    private PathManager t;
    private ReorderableList pointReorderableList;

    //Editor variables
    private bool visualFoldout;
    private bool manipulationFoldout;
    private bool showRawValues;
    private float time;
    private ManipulationModes cameraTranslateMode;
    private ManipulationModes cameraRotationMode;
    private ManipulationModes handlePositionMode;
    private NewWaypointMode waypointMode;
    private int waypointIndex = 1;
    private CurveType allCurveType = CurveType.Custom;
    private AnimationCurve allAnimationCurve = AnimationCurve.EaseInOut(0,0,1,1);

    //GUIContents
    private GUIContent addPointContent = new GUIContent("Add Point", "Adds a waypoint at the scene view camera's position/rotation");
    private GUIContent testButtonContent = new GUIContent("Test", "Only available in play mode");
    private GUIContent pauseButtonContent = new GUIContent("Pause", "Paused Camera at current Position");
    private GUIContent continueButtonContent = new GUIContent("Continue", "Continues Path at current position");
    private GUIContent stopButtonContent = new GUIContent("Stop", "Stops the playback");
    private GUIContent deletePointContent = new GUIContent("X", "Deletes this waypoint");
    private GUIContent gotoPointContent = new GUIContent("Goto", "Teleports the scene camera to the specified waypoint");
    private GUIContent relocateContent = new GUIContent("Relocate", "Relocates the specified camera to the current view camera's position/rotation");
    private GUIContent alwaysShowContent = new GUIContent("Always show", "When true, shows the curve even when the GameObject is not selected - \"Inactive cath color\" will be used as path color instead");
    private GUIContent chainedContent = new GUIContent("o───o", "Toggles if the handles of the specified waypoint should be chained (mirrored) or not");
    private GUIContent unchainedContent = new GUIContent("o─x─o", "Toggles if the handles of the specified waypoint should be chained (mirrored) or not");
    private GUIContent replaceAllPositionContent = new GUIContent("Replace all position lerps", "Replaces curve types (and curves when set to \"Custom\") of all the waypoint position lerp types with the specified values");
    private GUIContent replaceAllRotationContent = new GUIContent("Replace all rotation lerps", "Replaces curve types (and curves when set to \"Custom\") of all the waypoint rotation lerp types with the specified values");

    //Serialized Properties
    private SerializedObject serializedObjectTarget;
	private SerializedProperty selectedObjectProperty;
    private SerializedProperty lookAtTargetProperty;
    private SerializedProperty lookAtTargetTransformProperty;
    private SerializedProperty playOnAwakeProperty;
    private SerializedProperty playOnAwakeTimeProperty;
    private SerializedProperty visualPathProperty;
    private SerializedProperty visualInactivePathProperty;
    private SerializedProperty visualFrustumProperty;
    private SerializedProperty visualHandleProperty;
    private SerializedProperty loopedProperty;
    private SerializedProperty alwaysShowProperty;
    private SerializedProperty afterLoopProperty;

    private int selectedIndex = -1;

    private float currentTime;
    private float previousTime;

    private bool hasScrollBar = false;

    void OnEnable()
    {
        EditorApplication.update += Update;
        
        t = (PathManager) target;
        if (t == null) return;

        SetupEditorVariables();
        GetVariableProperties();
        SetupReorderableList();
    }

    void OnDisable()
    {
        EditorApplication.update -= Update;
    }

    void Update()
    {
        if (t == null) return;
        currentTime = t.GetCurrentWayPoint() + t.GetCurrentTimeInWaypoint();
	    if (Math.Abs(currentTime - previousTime) > 0.0001f)
        {
            Repaint();
            previousTime = currentTime;
        }
    }

    public override void OnInspectorGUI()
    {
        serializedObjectTarget.Update();
        DrawPlaybackWindow();
        Rect scale = GUILayoutUtility.GetLastRect();
        hasScrollBar = (Screen.width - scale.width <= 12);
        GUILayout.Space(5);
        GUILayout.Box("", GUILayout.Width(Screen.width - 20), GUILayout.Height(3));
        GUILayout.Space(5);
        DrawBasicSettings();
        GUILayout.Space(5);
        GUILayout.Box("", GUILayout.Width(Screen.width-20), GUILayout.Height(3));
        DrawVisualDropdown();
        GUILayout.Box("", GUILayout.Width(Screen.width - 20), GUILayout.Height(3));
        DrawManipulationDropdown();
        GUILayout.Box("", GUILayout.Width(Screen.width - 20), GUILayout.Height(3));
        GUILayout.Space(10);
        DrawWaypointList();
        GUILayout.Space(10);
        DrawRawValues();
        serializedObjectTarget.ApplyModifiedProperties();
    }

    void OnSceneGUI()
    {
	    if (t.wayPoints.Count >= 2)
        {
            for (int i = 0; i < t.wayPoints.Count; i++)
            {
                DrawHandles(i);
                Handles.color = Color.white;
            }
        }
    }

    void SelectIndex(int index)
    {
        selectedIndex = index;
        pointReorderableList.index = index;
        Repaint();
    }

    void SetupEditorVariables()
    {
        cameraTranslateMode = (ManipulationModes)PlayerPrefs.GetInt("cameraTranslateMode", 1);
        cameraRotationMode = (ManipulationModes)PlayerPrefs.GetInt("cameraRotationMode", 1);
        handlePositionMode = (ManipulationModes)PlayerPrefs.GetInt("handlePositionMode", 0);
        waypointMode = (NewWaypointMode) PlayerPrefs.GetInt("waypointMode", 0);
        time = PlayerPrefs.GetFloat("time", 10);
    }

    void GetVariableProperties()
    {
        serializedObjectTarget = new SerializedObject(t);
	    selectedObjectProperty = serializedObjectTarget.FindProperty("selectedGameObject");
        lookAtTargetProperty = serializedObjectTarget.FindProperty("lookAtTarget");
        lookAtTargetTransformProperty = serializedObjectTarget.FindProperty("target");
	    visualPathProperty = serializedObjectTarget.FindProperty("visualElements.pathColor");
        visualInactivePathProperty = serializedObjectTarget.FindProperty("visualElements.inactivePathColor");
         visualHandleProperty = serializedObjectTarget.FindProperty("visualElements.handleColor");
        loopedProperty = serializedObjectTarget.FindProperty("looped");
        alwaysShowProperty = serializedObjectTarget.FindProperty("alwaysShow");
        afterLoopProperty = serializedObjectTarget.FindProperty("afterLoop");
        playOnAwakeProperty = serializedObjectTarget.FindProperty("playOnAwake");
        playOnAwakeTimeProperty = serializedObjectTarget.FindProperty("playOnAwakeTime");
    }

    void SetupReorderableList()
    {
	       pointReorderableList = new ReorderableList(serializedObject, serializedObject.FindProperty("wayPoints"), true, true, false, false);

	      pointReorderableList.elementHeight *= 2;
	
	    /*	    pointReorderableList.drawElementCallback = (rect, index, active, focused) =>
        {
            float startRectY = rect.y;
            if (index > t.wayPoints.Count - 1) return;
            rect.height -= 2;
            float fullWidth = rect.width - 16 * (hasScrollBar ? 1 : 0);
            rect.width = 40;
            fullWidth -= 40;
            rect.height /= 2;
            GUI.Label(rect, "#" + (index + 1));
            rect.y += rect.height-3;
            rect.x -= 14;
            rect.width += 12;
            if (GUI.Button(rect, t.wayPoints[index].chained ? chainedContent : unchainedContent))
            {
                Undo.RecordObject(t, "Changed chain type");
                t.wayPoints[index].chained = !t.wayPoints[index].chained;
            }
            rect.x += rect.width+2;
            rect.y = startRectY;
            //Position
            rect.width = (fullWidth - 22) / 3 - 1;
            EditorGUI.BeginChangeCheck();
            CurveType tempP = (CurveType)EditorGUI.EnumPopup(rect, t.wayPoints[index].curveTypePosition);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(t, "Changed enum value");
	            t.wayPoints[index].curveTypePosition = tempP;
            }
            rect.y += pointReorderableList.elementHeight / 2 - 4;
            //rect.x += rect.width + 2;
            EditorGUI.BeginChangeCheck();
            GUI.enabled = t.wayPoints[index].curveTypePosition == CurveType.Custom;
            AnimationCurve tempACP = EditorGUI.CurveField(rect, t.wayPoints[index].positionCurve);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(t, "Changed curve");
                t.wayPoints[index].positionCurve = tempACP;
            }
            GUI.enabled = true;
            rect.x += rect.width + 2;
            rect.y = startRectY;

            //Rotation

            rect.width = (fullWidth - 22) / 3 - 1;
            EditorGUI.BeginChangeCheck();
            CurveType temp = (CurveType)EditorGUI.EnumPopup(rect, t.wayPoints[index].curveTypeRotation);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(t, "Changed enum value");
                t.wayPoints[index].curveTypeRotation = temp;
            }
            rect.y += pointReorderableList.elementHeight / 2 - 4;
            //rect.height /= 2;
            //rect.x += rect.width + 2;
            EditorGUI.BeginChangeCheck();
            GUI.enabled = t.wayPoints[index].curveTypeRotation == CurveType.Custom;
            AnimationCurve tempAC = EditorGUI.CurveField(rect, t.wayPoints[index].rotationCurve);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(t, "Changed curve");
                t.wayPoints[index].rotationCurve = tempAC;
            }
            GUI.enabled = true;

            rect.y = startRectY;
            rect.height *= 2;
            rect.x += rect.width + 2;
            rect.width = (fullWidth - 22) / 3;
            rect.height = rect.height / 2 - 1;
            if (GUI.Button(rect, gotoPointContent))
            {
                pointReorderableList.index = index;
                selectedIndex = index;
                SceneView.lastActiveSceneView.pivot = t.wayPoints[pointReorderableList.index].position;
                SceneView.lastActiveSceneView.size = 3;
                SceneView.lastActiveSceneView.Repaint();
            }
            rect.y += rect.height + 2;
            if (GUI.Button(rect, relocateContent))
            {
                Undo.RecordObject(t, "Relocated waypoint");
                pointReorderableList.index = index;
                selectedIndex = index;
                t.wayPoints[pointReorderableList.index].position = SceneView.lastActiveSceneView.camera.transform.position;
                t.wayPoints[pointReorderableList.index].rotation = SceneView.lastActiveSceneView.camera.transform.rotation;
                SceneView.lastActiveSceneView.Repaint();
            }
            rect.height = (rect.height + 1) * 2;
            rect.y = startRectY;
            rect.x += rect.width + 2;
	        rect.width = 20;

            if (GUI.Button(rect, deletePointContent))
            {
                Undo.RecordObject(t, "Deleted a waypoint");
	            t.wayPoints.Remove(t.wayPoints[index]);
                SceneView.RepaintAll();
            }
        };

        pointReorderableList.drawHeaderCallback = rect =>
        {
            float fullWidth = rect.width;
            rect.width = 56;
            GUI.Label(rect, "Sum: " + t.wayPoints.Count);
            rect.x += rect.width;
            rect.width = (fullWidth - 78) / 3;
            GUI.Label(rect, "Position Lerp");
            rect.x += rect.width;
            GUI.Label(rect, "Rotation Lerp");
            //rect.x += rect.width*2;
            //GUI.Label(rect, "Del.");
        };

        pointReorderableList.onSelectCallback = l =>
        {
            selectedIndex = l.index;
            SceneView.RepaintAll();
	    };*/
	    }

    void DrawPlaybackWindow()
    {
        GUI.enabled = Application.isPlaying;
        GUILayout.BeginVertical("Box");
        GUILayout.BeginHorizontal();
        if (GUILayout.Button(testButtonContent))
        {
            t.PlayPath(time);
        }

        if (!t.IsPaused())
        {
            if (Application.isPlaying && !t.IsPlaying()) GUI.enabled = false;
            if (GUILayout.Button(pauseButtonContent))
            {
                t.PausePath();
            }
        }
        else if (GUILayout.Button(continueButtonContent))
        {
            t.ResumePath();
        }

        if (GUILayout.Button(stopButtonContent))
        {
            t.StopPath();
        }
        GUI.enabled = true;
        EditorGUI.BeginChangeCheck();
        GUILayout.Label("Time (seconds)");
        time = EditorGUILayout.FloatField("", time, GUILayout.MinWidth(5), GUILayout.MaxWidth(50));
        if (EditorGUI.EndChangeCheck())
        {
            time = Mathf.Clamp(time, 0.001f, Mathf.Infinity);
            t.UpdateTimeInSeconds(time);
            PlayerPrefs.SetFloat("CPC_time", time);
        }
        GUILayout.EndHorizontal();
        GUI.enabled = Application.isPlaying;
        EditorGUI.BeginChangeCheck();
        currentTime = EditorGUILayout.Slider(currentTime, 0, t.wayPoints.Count - ((t.looped) ? 0.01f : 1.01f));
        if (EditorGUI.EndChangeCheck())
        {
            t.SetCurrentWayPoint(Mathf.FloorToInt(currentTime));
            t.SetCurrentTimeInWaypoint(currentTime % 1);
            t.RefreshTransform();
        }
        GUI.enabled = false;
        Rect rr = GUILayoutUtility.GetRect(4, 8);
        float endWidth = rr.width - 60;
        rr.y -= 4;
        rr.width = 4;
        int c = t.wayPoints.Count + ((t.looped) ? 1 : 0);
        for (int i = 0; i < c; ++i)
        {
            GUI.Box(rr, "");
            rr.x += endWidth / (c - 1);
        }
        GUILayout.EndVertical();
        GUI.enabled = true;
    }

    void DrawBasicSettings()
    {
        GUILayout.BeginHorizontal();
	    //  GUI.enabled = !useMainCameraProperty.boolValue;
	    selectedObjectProperty.objectReferenceValue = (GameObject)EditorGUILayout.ObjectField(selectedObjectProperty.objectReferenceValue, typeof(GameObject), true);
        GUI.enabled = true;
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        lookAtTargetProperty.boolValue = GUILayout.Toggle(lookAtTargetProperty.boolValue, "Look at target", GUILayout.Width(Screen.width / 3f));
        GUI.enabled = lookAtTargetProperty.boolValue;
        lookAtTargetTransformProperty.objectReferenceValue = (Transform)EditorGUILayout.ObjectField(lookAtTargetTransformProperty.objectReferenceValue, typeof(Transform), true);
        GUI.enabled = true;
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        loopedProperty.boolValue = GUILayout.Toggle(loopedProperty.boolValue, "Looped", GUILayout.Width(Screen.width/3f));
        GUI.enabled = loopedProperty.boolValue;
        GUILayout.Label("After loop:", GUILayout.Width(Screen.width / 4f));
        afterLoopProperty.enumValueIndex = Convert.ToInt32(EditorGUILayout.EnumPopup((AfterLoop)afterLoopProperty.intValue));
        GUI.enabled = true;
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        playOnAwakeProperty.boolValue = GUILayout.Toggle(playOnAwakeProperty.boolValue, "Play on awake", GUILayout.Width(Screen.width / 3f));
        GUI.enabled = playOnAwakeProperty.boolValue;
        GUILayout.Label("Time: ", GUILayout.Width(Screen.width / 4f));
        playOnAwakeTimeProperty.floatValue = EditorGUILayout.FloatField(playOnAwakeTimeProperty.floatValue);
        GUI.enabled = true;
        GUILayout.EndHorizontal();
    }

    void DrawVisualDropdown()
    {
        EditorGUI.BeginChangeCheck();
        GUILayout.BeginHorizontal();
        visualFoldout = EditorGUILayout.Foldout(visualFoldout, "Visual");
        alwaysShowProperty.boolValue = GUILayout.Toggle(alwaysShowProperty.boolValue, alwaysShowContent);
        GUILayout.EndHorizontal();
        if (visualFoldout)
        {
            GUILayout.BeginVertical("Box");
            visualPathProperty.colorValue = EditorGUILayout.ColorField("Path color", visualPathProperty.colorValue);
            visualInactivePathProperty.colorValue = EditorGUILayout.ColorField("Inactive path color", visualInactivePathProperty.colorValue);
             visualHandleProperty.colorValue = EditorGUILayout.ColorField("Handle color", visualHandleProperty.colorValue);
            if (GUILayout.Button("Default colors"))
            {
                Undo.RecordObject(t, "Reset to default color values");
	            t.visualElements = new Visuals();
            }
            GUILayout.EndVertical();
        }
        if (EditorGUI.EndChangeCheck())
        {
            SceneView.RepaintAll();
        }
    }

    void DrawManipulationDropdown()
    {
	    /*    manipulationFoldout = EditorGUILayout.Foldout(manipulationFoldout, "Transform manipulation modes");
        EditorGUI.BeginChangeCheck();
        if (manipulationFoldout)
        {
            GUILayout.BeginVertical("Box");
            cameraTranslateMode = (ManipulationModes)EditorGUILayout.EnumPopup("Waypoint Translation", cameraTranslateMode);
            cameraRotationMode = (ManipulationModes)EditorGUILayout.EnumPopup("Waypoint Rotation", cameraRotationMode);
            handlePositionMode = (ManipulationModes)EditorGUILayout.EnumPopup("Handle Translation", handlePositionMode);
            GUILayout.EndVertical();
        }
        if (EditorGUI.EndChangeCheck())
        {
            PlayerPrefs.SetInt("CPC_cameraTranslateMode", (int)cameraTranslateMode);
            PlayerPrefs.SetInt("CPC_cameraRotationMode", (int)cameraRotationMode);
            PlayerPrefs.SetInt("CPC_handlePositionMode", (int)handlePositionMode);
            SceneView.RepaintAll();
	    }*/
    }

    void DrawWaypointList()
    {
        GUILayout.Label("Replace all lerp types");
        GUILayout.BeginVertical("Box");
        GUILayout.BeginHorizontal();
        allCurveType = (CurveType)EditorGUILayout.EnumPopup(allCurveType, GUILayout.Width(Screen.width / 3f));
        if (GUILayout.Button(replaceAllPositionContent))
        {
            Undo.RecordObject(t, "Applied new position");
            foreach (var index in t.wayPoints)
            {
                index.curveTypePosition = allCurveType;
                if (allCurveType == CurveType.Custom)
                    index.positionCurve.keys = allAnimationCurve.keys;
            }
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        GUI.enabled = allCurveType == CurveType.Custom;
        allAnimationCurve = EditorGUILayout.CurveField(allAnimationCurve, GUILayout.Width(Screen.width / 3f));
        GUI.enabled = true;
        if (GUILayout.Button(replaceAllRotationContent))
        {
            Undo.RecordObject(t, "Applied new rotation");
            foreach (var index in t.wayPoints)
            {
                index.curveTypeRotation = allCurveType;
                if (allCurveType == CurveType.Custom)
                    index.rotationCurve = allAnimationCurve;
            }
        }
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
        GUILayout.BeginHorizontal();
        GUILayout.Space(Screen.width/2f-20);
        GUILayout.Label("↓");
        GUILayout.EndHorizontal();
        serializedObject.Update();
        pointReorderableList.DoLayoutList();
        serializedObject.ApplyModifiedProperties();
        Rect r = GUILayoutUtility.GetRect(Screen.width - 16, 18);
        //r.height = 18;
        r.y -= 10;
        GUILayout.Space(-30);
        GUILayout.BeginHorizontal();
        if (GUILayout.Button(addPointContent))
        {
            Undo.RecordObject(t, "Added camera path point");
            switch (waypointMode)
            {
                case NewWaypointMode.SceneCamera:
                    t.wayPoints.Add(new Point(SceneView.lastActiveSceneView.camera.transform.position, SceneView.lastActiveSceneView.camera.transform.rotation));
                    break;
                case NewWaypointMode.LastWaypoint:
                    if (t.wayPoints.Count > 0)
	                    t.wayPoints.Add(new Point(t.wayPoints[t.wayPoints.Count - 1].position, t.wayPoints[t.wayPoints.Count - 1].rotation) { handlenext = t.wayPoints[t.wayPoints.Count - 1].handlenext, handleprev = t.wayPoints[t.wayPoints.Count - 1].handleprev });
                    else
                    {
                        t.wayPoints.Add(new Point(Vector3.zero, Quaternion.identity));
                        Debug.LogWarning("No previous waypoint found to place this waypoint, defaulting position to world center");
                    }
                    break;
                case NewWaypointMode.WaypointIndex:
                    if (t.wayPoints.Count > waypointIndex-1 && waypointIndex > 0)
                        t.wayPoints.Add(new Point(t.wayPoints[waypointIndex-1].position, t.wayPoints[waypointIndex-1].rotation) { handlenext = t.wayPoints[waypointIndex-1].handlenext, handleprev = t.wayPoints[waypointIndex-1].handleprev });
                    else
                    {
                        t.wayPoints.Add(new Point(Vector3.zero, Quaternion.identity));
                        Debug.LogWarning("Waypoint index "+waypointIndex+" does not exist, defaulting position to world center");
                    }
                    break;
                case NewWaypointMode.WorldCenter:
                    t.wayPoints.Add(new Point(Vector3.zero, Quaternion.identity));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            selectedIndex = t.wayPoints.Count - 1;
            SceneView.RepaintAll();
        }
        GUILayout.Label("at", GUILayout.Width(20));
        EditorGUI.BeginChangeCheck();
        waypointMode = (NewWaypointMode) EditorGUILayout.EnumPopup(waypointMode, waypointMode==NewWaypointMode.WaypointIndex ? GUILayout.Width(Screen.width/4) : GUILayout.Width(Screen.width/2));
        if (waypointMode == NewWaypointMode.WaypointIndex)
        {
            waypointIndex = EditorGUILayout.IntField(waypointIndex, GUILayout.Width(Screen.width / 4));
        }
        if (EditorGUI.EndChangeCheck())
        {
            PlayerPrefs.SetInt("CPC_waypointMode", (int)waypointMode);
        }
        GUILayout.EndHorizontal();
    }

    void DrawHandles(int i)
    {
        DrawHandleLines(i);
        Handles.color = t.visualElements.handleColor;
        DrawNextHandle(i);
        DrawPrevHandle(i);
        DrawWaypointHandles(i);
        DrawSelectionHandles(i);
    }

    void DrawHandleLines(int i)
    {
	    Handles.color = t.visualElements.handleColor;
        if (i < t.wayPoints.Count - 1 || t.looped == true)
            Handles.DrawLine(t.wayPoints[i].position, t.wayPoints[i].position + t.wayPoints[i].handlenext);
        if (i > 0 || t.looped == true)
            Handles.DrawLine(t.wayPoints[i].position, t.wayPoints[i].position + t.wayPoints[i].handleprev);
        Handles.color = Color.white;
    }

    void DrawNextHandle(int i)
    {
        if (i < t.wayPoints.Count - 1 || loopedProperty.boolValue)
        {
            EditorGUI.BeginChangeCheck();
            Vector3 posNext = Vector3.zero;
            float size = HandleUtility.GetHandleSize(t.wayPoints[i].position + t.wayPoints[i].handlenext) * 0.1f;
            if (handlePositionMode == ManipulationModes.Free)
            {
                posNext = Handles.FreeMoveHandle(t.wayPoints[i].position + t.wayPoints[i].handlenext, Quaternion.identity, size, Vector3.zero, Handles.SphereHandleCap);
            }
            else
            {
                if (selectedIndex == i)
                {
                    Handles.SphereHandleCap(0, t.wayPoints[i].position + t.wayPoints[i].handlenext, Quaternion.identity, size, EventType.Repaint);
                    posNext = Handles.PositionHandle(t.wayPoints[i].position + t.wayPoints[i].handlenext, Quaternion.identity);
                }
                else if (Event.current.button != 1)
                {
                    if (Handles.Button(t.wayPoints[i].position + t.wayPoints[i].handlenext, Quaternion.identity, size, size, Handles.SphereHandleCap))
                    {
                        SelectIndex(i);
                    }
                }
            }
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(target, "Changed Handle Position");
                t.wayPoints[i].handlenext = posNext - t.wayPoints[i].position;
                if (t.wayPoints[i].chained)
                    t.wayPoints[i].handleprev = t.wayPoints[i].handlenext * -1;
            }
        }

    }

    void DrawPrevHandle(int i)
    {
        if (i > 0 || loopedProperty.boolValue)
        {
            EditorGUI.BeginChangeCheck();
            Vector3 posPrev = Vector3.zero;
            float size = HandleUtility.GetHandleSize(t.wayPoints[i].position + t.wayPoints[i].handleprev) * 0.1f;
            if (handlePositionMode == ManipulationModes.Free)
            {
                posPrev = Handles.FreeMoveHandle(t.wayPoints[i].position + t.wayPoints[i].handleprev, Quaternion.identity, 0.1f * HandleUtility.GetHandleSize(t.wayPoints[i].position + t.wayPoints[i].handleprev), Vector3.zero, Handles.SphereHandleCap);

            }
            else
            {
                if (selectedIndex == i)
                {
                    Handles.SphereHandleCap(0, t.wayPoints[i].position + t.wayPoints[i].handleprev, Quaternion.identity, 0.1f * HandleUtility.GetHandleSize(t.wayPoints[i].position + t.wayPoints[i].handlenext), EventType.Repaint);

                    posPrev = Handles.PositionHandle(t.wayPoints[i].position + t.wayPoints[i].handleprev, Quaternion.identity);
                }
                else if (Event.current.button != 1)
                {
                    if (Handles.Button(t.wayPoints[i].position + t.wayPoints[i].handleprev, Quaternion.identity, size, size, Handles.SphereHandleCap))
                    {
                        SelectIndex(i);
                    }

                }
            }
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(target, "Changed Handle Position");
                t.wayPoints[i].handleprev = posPrev - t.wayPoints[i].position;
                if (t.wayPoints[i].chained)
                    t.wayPoints[i].handlenext = t.wayPoints[i].handleprev * -1;
            }
        }
    }

    void DrawWaypointHandles(int i)
    {
        if (Tools.current == Tool.Move)
        {
            EditorGUI.BeginChangeCheck();
            Vector3 pos = Vector3.zero;
	        if (cameraTranslateMode == ManipulationModes.Free)
            {
                if (i == selectedIndex) pos = Handles.PositionHandle(t.wayPoints[i].position, (Tools.pivotRotation == PivotRotation.Local) ? t.wayPoints[i].rotation : Quaternion.identity);
            }
            else
            {
                pos = Handles.FreeMoveHandle(t.wayPoints[i].position, (Tools.pivotRotation == PivotRotation.Local) ? t.wayPoints[i].rotation : Quaternion.identity, HandleUtility.GetHandleSize(t.wayPoints[i].position) * 0.2f, Vector3.zero, Handles.RectangleHandleCap);

            }
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(target, "Moved Waypoint");
                t.wayPoints[i].position = pos;
            }
        }
        else if (Tools.current == Tool.Rotate)
        {

            EditorGUI.BeginChangeCheck();
            Quaternion rot = Quaternion.identity;
	        if (cameraRotationMode == ManipulationModes.Free)
            {
                if (i == selectedIndex) rot = Handles.RotationHandle(t.wayPoints[i].rotation, t.wayPoints[i].position);
            }
            else
            {
                rot = Handles.FreeRotateHandle(t.wayPoints[i].rotation, t.wayPoints[i].position, HandleUtility.GetHandleSize(t.wayPoints[i].position) * 0.2f);
            }
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(target, "Rotated Waypoint");
                t.wayPoints[i].rotation = rot;
            }
        }
    }

    void DrawSelectionHandles(int i)
    {
        if (Event.current.button != 1 && selectedIndex != i)
        {
	         if (cameraTranslateMode == ManipulationModes.Free && Tools.current == Tool.Move
	                || cameraRotationMode == ManipulationModes.Free && Tools.current == Tool.Rotate)
            {
                float size = HandleUtility.GetHandleSize(t.wayPoints[i].position) * 0.2f;
                if (Handles.Button(t.wayPoints[i].position, Quaternion.identity, size, size, Handles.SphereHandleCap))
                {
                
                    SelectIndex(i);
                }
            }
        }
    }

    void DrawRawValues()
    {
        if (GUILayout.Button(showRawValues ? "Hide raw values" : "Show raw values"))
            showRawValues = !showRawValues;

        if (showRawValues)
        {
	        foreach (var i in t.wayPoints)
            {
                EditorGUI.BeginChangeCheck();
                GUILayout.BeginVertical("Box");
                Vector3 pos = EditorGUILayout.Vector3Field("Waypoint Position", i.position);
                Quaternion rot = Quaternion.Euler(EditorGUILayout.Vector3Field("Waypoint Rotation", i.rotation.eulerAngles));
                Vector3 posp = EditorGUILayout.Vector3Field("Previous Handle Offset", i.handleprev);
                Vector3 posn = EditorGUILayout.Vector3Field("Next Handle Offset", i.handlenext);
                GUILayout.EndVertical();
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(t, "Changed waypoint transform");
                    i.position = pos;
                    i.rotation = rot;
                    i.handleprev = posp;
                    i.handlenext = posn;
                    SceneView.RepaintAll();
                }
            }
        }
    }
 }
}