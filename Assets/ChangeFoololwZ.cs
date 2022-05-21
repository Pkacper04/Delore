using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class ChangeFoololwZ : MonoBehaviour
{
    [SerializeField]
    private CinemachineVirtualCamera customCamera;
    [SerializeField]
    private CinemachineStateDrivenCamera cameraController;
    [SerializeField]
    private Transform baseCamera;
    [SerializeField]
    private float changeOffset;

    private bool changed = false;

    private CinemachineTransposer cameraOffset;

    private void Start()
    {
        cameraOffset = customCamera.GetCinemachineComponent<CinemachineTransposer>();
        cameraOffset.m_FollowOffset.z = 1;
    }

    void Update()
    {
        if(cameraController.IsLiveChild(customCamera))
        {
            Debug.Log("offset: "+ (customCamera.transform.position.x - baseCamera.transform.position.x));
            if (customCamera.transform.position.x - baseCamera.transform.position.x >= changeOffset)
            {
                cameraOffset.m_FollowOffset.z = -1;
            }
                
        }
        else
        {
            cameraOffset.m_FollowOffset.z = 1;
        }
    }
}
