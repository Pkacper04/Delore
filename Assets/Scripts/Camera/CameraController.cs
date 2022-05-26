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
    private OverrideOffset newOffset;

    private bool inTransition = false;
    private int numberOfCameras = 0;
    private float storageOffset;
    private bool blockOffseIncrement;
    private bool blockOffsetDectrement;

    internal int camNumberStorage;
    internal Transform mainCamera;
    internal int camNumber = 1;
    internal int lastCameraId = 0;
    public bool xAxis = true;


    private void Start()
    {
        storageOffset = playerOffset;
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
        else
            OverrideOffset();

    }

    private void AxisCamera()
    {
        if (!blockOffseIncrement && CalculatePlayerOffset() >= playerOffset && camNumber + 1 <= numberOfCameras)
        {
            camNumber++;
        }
        else if (!blockOffsetDectrement && CalculatePlayerOffset() <= -playerOffset)
        {
            camNumber--;
        }

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
            OverrideOffset();
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

private void OverrideOffset()
{
    
    if (cinemachineCamera.LiveChild.VirtualCameraGameObject.TryGetComponent<OverrideOffset>(out newOffset))
    {
        if(newOffset.newOffset != -1)
            playerOffset = newOffset.newOffset;
        blockOffseIncrement = newOffset.blockOffseIncrement;
        blockOffsetDectrement = newOffset.blockOffsetDectrement;
    }
    else
    {
        playerOffset = storageOffset;
        blockOffseIncrement = false;
        blockOffsetDectrement = false;
    }
}


    public void DisableLooking()
    {
        cinemachineCamera.LookAt = null;
        cinemachineCamera.LiveChild.LookAt = null;
        blockOffseIncrement = true;
        blockOffsetDectrement = true;
    }
}
