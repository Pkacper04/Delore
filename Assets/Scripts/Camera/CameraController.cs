using UnityEngine;
using System.Collections;
using System;
using Cinemachine;
using System.Collections.Generic;

public class CameraController : MonoBehaviour
{

    [SerializeField] internal float playerOffset = 7f;
    [SerializeField] CmeraSaveLoad cameraSaveSystem;
    [SerializeField] CinemachineStateDrivenCamera cinemachineCamera;
    
    


    private Transform player;
    private Animator animator;

    private bool inTransition = false;
    private int numberOfCameras = 0;

    internal int camNumberStorage;
    internal Transform mainCamera;
    internal int camNumber = 1;
    internal int lastCameraId = 0;
    public bool xAxis = true;


    private void Start()
    {
        animator = GetComponent<Animator>();
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").transform;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        numberOfCameras = cinemachineCamera.ChildCameras.Length;

        cameraSaveSystem.LoadData(this);

        if(xAxis)
            animator.SetInteger("CameraNumberX", camNumber);
        else
        {
            animator.SetInteger("CameraNumberZ", camNumber);
            WaitForTransition();
        }
    }



    void Update()
    {
        cameraSaveSystem.SetCameraId(this);

        if (!animator.IsInTransition(0) && !inTransition)
            AxisCamera();
        
    }

    private void AxisCamera()
    {
        if (CalculatePlayerOffset() >= playerOffset && camNumber + 1 <= numberOfCameras )
            camNumber++;
        else if (CalculatePlayerOffset() <= -playerOffset && camNumber - 1 > 0)
            camNumber--;

        UpdateAnimation();

    }

    private void UpdateAnimation()
    {
        if (xAxis)
            animator.SetInteger("CameraNumberX", camNumber);
        else
            animator.SetInteger("CameraNumberZ", camNumber);
    }

    private float CalculatePlayerOffset()
    {
        if(xAxis)
            return player.position.x - mainCamera.position.x;
        return player.position.z - mainCamera.position.z;
    }


    public void ChangeCamera(bool right)
    {
        
        if (!animator.IsInTransition(0))
        {
            camNumber = right ? camNumber + 1 : camNumber - 1;
            animator.SetInteger("CameraNumberX", camNumber);
        }
    }

    public void ChangeAxis(bool Axis, int cameraIdX, int cameraIdZ)
    {
        this.xAxis = Axis;

        if (!animator.IsInTransition(0))
        {
            if (Axis)
            {
                animator.SetInteger("CameraNumberZ", 0);
                animator.SetInteger("CameraNumberX", cameraIdX);
                camNumber = cameraIdX;
                StartCoroutine(WaitForTransition());
            }
            else
            {
                animator.SetInteger("CameraNumberZ", cameraIdZ);
                animator.SetInteger("CameraNumberX", 0);
                camNumber = cameraIdZ;
                StartCoroutine(WaitForTransition());
                
            }
        }
    }


    private IEnumerator WaitForTransition()
    {
        inTransition = true;
        yield return new WaitUntil(() => animator.IsInTransition(0));
        inTransition = false;
    }

}
