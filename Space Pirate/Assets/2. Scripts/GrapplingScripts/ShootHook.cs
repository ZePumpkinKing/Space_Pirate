using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootHook : MonoBehaviour
{
    Grappling grapp;
    Vector3 initPos;
    Quaternion initRot;
    public Transform returnPoint;
    public Vector3 hookPoint;
    public float timeStartedLerping;
    public float lerpTime;

    private void Awake()
    {
        grapp = FindObjectOfType<Grappling>();
    }
    public void Hookshot(RaycastHit hit)
    {
        hookPoint = hit.point;
        initRot = transform.rotation;
        initPos = transform.position;
        timeStartedLerping = Time.time;
    }
    public void MoveHook(Vector3 pos)
    {
        if (grapp.isObjGrappling)
        {
            hookPoint = pos;
            gameObject.layer = 0;
        }
        if (grapp.isPlayerGrappling)
        {
            gameObject.layer = 0;
        }
        //transform.position = Vector3.Lerp(initPos, hookPoint, Time.deltaTime * 10f);
        transform.position = Lerp(initPos, hookPoint, timeStartedLerping, lerpTime);
        transform.rotation = initRot;
    }

    public Vector3 Lerp(Vector3 start, Vector3 end, float timeStartedLerping, float lerpTime = 1)
    {
        float timeSinceStarted = Time.time - timeStartedLerping;

        float percentageComplete = timeSinceStarted / lerpTime;

        var result = Vector3.Lerp(start, end, percentageComplete);

        return result;
    }
    public void ReturnHook()
    {
        transform.localPosition = returnPoint.localPosition;
        transform.rotation = returnPoint.rotation;
        gameObject.layer = 14;
    }
}
