using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform PlayerPos;
    [SerializeField] float Offset,CamSpeed,CamRotSpeed,AngleThreshold,ZoomSpeed,TimeSpeed;
    Vector3 startpos;
    Quaternion startrot;
    int FollowType;
    [SerializeField]Camera MainCam;
    Quaternion test;

    private void OnEnable()
    {
        startpos = transform.position;
        startrot = transform.rotation;
        FollowType = 1;
        StartCoroutine(StandardFollow());
        NearMissScript.OnMiss += Follow2;
        NearMissScript.OnMissExit += Follow1;
        NearMissScript.OnWallHit += Follow2;
        NearMissScript.OnWallHitExit += Follow1;
    }
    private void OnDisable()
    {
        NearMissScript.OnMiss -= Follow2;
        NearMissScript.OnMissExit -= Follow1;
        NearMissScript.OnWallHit -= Follow2;
        NearMissScript.OnWallHitExit -= Follow1;
    }
    IEnumerator StandardFollow()
    {
        
        float ti = 0;
        while (FollowType == 1)
        {
            ti += Time.deltaTime;
            if (MainCam.fieldOfView <60) MainCam.fieldOfView = Mathf.Lerp(MainCam.fieldOfView, 60, ti * ZoomSpeed);
            Vector3 target = new Vector3(PlayerPos.position.x - Offset, transform.position.y, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, target, CamSpeed);
            float angle = Mathf.Atan2(transform.position.y, PlayerPos.position.y) * Mathf.Rad2Deg;
            angle =50-angle;
            if (-angle < AngleThreshold)
            {
                test = Quaternion.Euler(-angle, 0, 0);
                test *= startrot;
                test = new Quaternion(test.x, test.y, 0, test.w); //makes the camera rotate less on z axis.
                transform.rotation = Quaternion.Lerp(transform.rotation, test, CamRotSpeed);
               

            }
            else transform.rotation = Quaternion.Lerp(transform.rotation, startrot, CamRotSpeed);
            yield return 0;
        }
    }
    IEnumerator ZoomFollow()
    {
        float ti = 0;
        while (FollowType == 2)
        {
            ti += Time.deltaTime;
            if (MainCam.fieldOfView > 40) MainCam.fieldOfView = Mathf.Lerp(MainCam.fieldOfView, 40, ti*ZoomSpeed);
            if (Time.timeScale > 0.5f) Time.timeScale = Mathf.Lerp(Time.timeScale, 0.5f, ti*TimeSpeed);
            Vector3 target = new Vector3(PlayerPos.position.x - Offset, transform.position.y, transform.position.z);
            //float angle = Mathf.Atan2(transform.position.x-transform.position.y, PlayerPos.position.x-PlayerPos.position.y) * Mathf.Rad2Deg;
            transform.position = Vector3.Lerp(transform.position, target, CamSpeed);
            //angle = 180+angle;
            //Quaternion test = Quaternion.Euler(-angle, 1, 1);
            transform.LookAt(PlayerPos);

            //transform.rotation = Quaternion.Slerp(transform.rotation, test, CamRotSpeed);

            yield return 0;
        }
        Invoke(nameof(ReturnToNormalScale), 1f);
    }
    void Follow1() //used for c# events
    {
        FollowType = 1;
        StartCoroutine(StandardFollow());
    }
    void Follow2()
    {
        FollowType = 2;
        CancelInvoke();
        StartCoroutine(ZoomFollow());
        
    }
    public void ChangeCameraMode(int mode)//used for external purposes
    {
        switch (mode)
        {
            case 1:
                FollowType = 1;
                StartCoroutine(StandardFollow());
                break;
            case 2:
                FollowType = 2;
                StartCoroutine(ZoomFollow());
                break;
            case 0:
                FollowType = 0;
                StopAllCoroutines();
                break;
        }
    }
    void ReturnToNormalScale()
    {
        Time.timeScale = 1;
    }
}
