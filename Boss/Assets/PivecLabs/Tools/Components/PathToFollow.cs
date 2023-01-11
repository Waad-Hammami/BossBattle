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
	using UnityEditorInternal;
#endif

[System.Serializable]
	public class Visuals
{
	public Color pathColor = Color.white;
    public Color inactivePathColor = Color.gray;
	public Color handleColor = Color.blue;
}

public enum CurveType
{
    EaseInAndOut,
    Linear,
    Custom
}

public enum AfterLoop
{
    Continue,
    Stop
}

[System.Serializable]
public class Point
{

    public Vector3 position;
    public Quaternion rotation;
    public Vector3 handleprev;
    public Vector3 handlenext;
    public CurveType curveTypeRotation;
    public AnimationCurve rotationCurve;
    public CurveType curveTypePosition;
    public AnimationCurve positionCurve;
    public bool chained;

    public Point(Vector3 pos, Quaternion rot)
    {
        position = pos;
        rotation = rot;
        handleprev = Vector3.back;
        handlenext = Vector3.forward;
        curveTypeRotation = CurveType.EaseInAndOut;
        rotationCurve = AnimationCurve.EaseInOut(0,0,1,1);
        curveTypePosition = CurveType.Linear;
        positionCurve = AnimationCurve.Linear(0,0,1,1);
        chained = true;
    }
}

	public class PathManager : MonoBehaviour
{

	public GameObject selectedGameObject;
    public bool lookAtTarget = false;
    public Transform target;
    public bool playOnAwake = false;
    public float playOnAwakeTime = 10;
	public List<Point> wayPoints = new List<Point>();
	public Visuals visualElements;
    public bool looped = false;
    public bool alwaysShow = true;
    public AfterLoop afterLoop = AfterLoop.Continue;

    private int currentWaypointIndex;
    private float currentTimeInWaypoint;
    private float timePerSegment;

    private bool paused = false;
    private bool playing = false;

    void Start ()
    {
        
  
	    if (lookAtTarget && target == null)
	    {
	        lookAtTarget = false;
            Debug.LogError("No target selected to look at, defaulting to normal rotation");
        }

	    foreach (var index in wayPoints)
	    {
            if (index.curveTypeRotation == CurveType.EaseInAndOut) index.rotationCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
            if (index.curveTypeRotation == CurveType.Linear) index.rotationCurve = AnimationCurve.Linear(0, 0, 1, 1);
            if (index.curveTypePosition == CurveType.EaseInAndOut) index.positionCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
            if (index.curveTypePosition == CurveType.Linear) index.positionCurve = AnimationCurve.Linear(0, 0, 1, 1);
        }

        if (playOnAwake)
            PlayPath(playOnAwakeTime);
    }

     public void PlayPath(float time)
    {
        if (time <= 0) time = 0.001f;
        paused = false;
        playing = true;
        StopAllCoroutines();
        StartCoroutine(FollowPath(time));
    }

    public void StopPath()
    {
        playing = false;
        paused = false;
        StopAllCoroutines();
    }

    public void UpdateTimeInSeconds(float seconds)
    {
        timePerSegment = seconds / ((looped) ? wayPoints.Count : wayPoints.Count - 1);
    }

     public void PausePath()
    {
        paused = true;
        playing = false;
    }

    public void ResumePath()
    {
        if (paused)
            playing = true;
        paused = false;
    }

    public bool IsPaused()
    {
        return paused;
    }

    public bool IsPlaying()
    {
        return playing;
    }

    public int GetCurrentWayPoint()
    {
        return currentWaypointIndex;
    }

    public float GetCurrentTimeInWaypoint()
    {
        return currentTimeInWaypoint;
    }

    public void SetCurrentWayPoint(int value)
    {
        currentWaypointIndex = value;
    }

    public void SetCurrentTimeInWaypoint(float value)
    {
        currentTimeInWaypoint = value;
    }

    public void RefreshTransform()
    {
        selectedGameObject.transform.position = GetBezierPosition(currentWaypointIndex, currentTimeInWaypoint);
        if (!lookAtTarget)
            selectedGameObject.transform.rotation = GetLerpRotation(currentWaypointIndex, currentTimeInWaypoint);
        else
            selectedGameObject.transform.rotation = Quaternion.LookRotation((target.transform.position - selectedGameObject.transform.position).normalized);
    }

    IEnumerator FollowPath(float time)
    {
        UpdateTimeInSeconds(time);
        currentWaypointIndex = 0;
        while (currentWaypointIndex < wayPoints.Count)
        {
            currentTimeInWaypoint = 0;
            while (currentTimeInWaypoint < 1)
            {
                if (!paused)
                {
                    currentTimeInWaypoint += Time.deltaTime / timePerSegment;
                    selectedGameObject.transform.position = GetBezierPosition(currentWaypointIndex, currentTimeInWaypoint);
                    if (!lookAtTarget)
                        selectedGameObject.transform.rotation = GetLerpRotation(currentWaypointIndex, currentTimeInWaypoint);
                    else
                        selectedGameObject.transform.rotation = Quaternion.LookRotation((target.transform.position - selectedGameObject.transform.position).normalized);
                }
                yield return 0;
            }
            ++currentWaypointIndex;
            if (currentWaypointIndex == wayPoints.Count - 1 && !looped) break;
            if (currentWaypointIndex == wayPoints.Count && afterLoop == AfterLoop.Continue) currentWaypointIndex = 0;
        }
        StopPath();
    }

    int GetNextIndex(int index)
    {
        if (index == wayPoints.Count-1)
            return 0;
        return index + 1;
    }

    Vector3 GetBezierPosition(int pointIndex, float time)
    {
        float t = wayPoints[pointIndex].positionCurve.Evaluate(time);
        int nextIndex = GetNextIndex(pointIndex);
        return
            Vector3.Lerp(
                Vector3.Lerp(
                    Vector3.Lerp(wayPoints[pointIndex].position,
                        wayPoints[pointIndex].position + wayPoints[pointIndex].handlenext, t),
                    Vector3.Lerp(wayPoints[pointIndex].position + wayPoints[pointIndex].handlenext,
                        wayPoints[nextIndex].position + wayPoints[nextIndex].handleprev, t), t),
                Vector3.Lerp(
                    Vector3.Lerp(wayPoints[pointIndex].position + wayPoints[pointIndex].handlenext,
                        wayPoints[nextIndex].position + wayPoints[nextIndex].handleprev, t),
                    Vector3.Lerp(wayPoints[nextIndex].position + wayPoints[nextIndex].handleprev,
                        wayPoints[nextIndex].position, t), t), t);
    }

    private Quaternion GetLerpRotation(int pointIndex, float time)
    {
        return Quaternion.LerpUnclamped(wayPoints[pointIndex].rotation, wayPoints[GetNextIndex(pointIndex)].rotation, wayPoints[pointIndex].rotationCurve.Evaluate(time));
    }

#if UNITY_EDITOR
    public void OnDrawGizmos()
    {
        if (UnityEditor.Selection.activeGameObject == gameObject || alwaysShow)
        {
            if (wayPoints.Count >= 2)
            {
                for (int i = 0; i < wayPoints.Count; i++)
                {
                    if (i < wayPoints.Count - 1)
                    {
                        var index = wayPoints[i];
                        var indexNext = wayPoints[i + 1];
                        UnityEditor.Handles.DrawBezier(index.position, indexNext.position, index.position + index.handlenext,
	                        indexNext.position + indexNext.handleprev,((UnityEditor.Selection.activeGameObject == gameObject) ? visualElements.pathColor : visualElements.inactivePathColor), null, 5);
                    }
                    else if (looped)
                    {
                        var index = wayPoints[i];
                        var indexNext = wayPoints[0];
                        UnityEditor.Handles.DrawBezier(index.position, indexNext.position, index.position + index.handlenext,
                            indexNext.position + indexNext.handleprev, ((UnityEditor.Selection.activeGameObject == gameObject) ? visualElements.pathColor : visualElements.inactivePathColor), null, 5);
                    }
                }
            }

        
        }
    }
#endif

}
}