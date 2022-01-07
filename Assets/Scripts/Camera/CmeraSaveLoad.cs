using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

public class CmeraSaveLoad : MonoBehaviour
{
    private CinemachineStateDrivenCamera stateCamera;
    private PlayerData data;
    // Start is called before the first frame update

    private void Awake()
    {
        data = SaveSystem.LoadPlayer();
        stateCamera = GetComponent<CinemachineStateDrivenCamera>();
    }

    public void LoadData(CameraController controller)
    {
        data = SaveSystem.LoadPlayer();
        if (data != null)
        {

            controller.camNumber = data.camNumber;
            controller.xAxis = data.xAxis;
            controller.camNumberStorage = data.storedCamNumber;

            CinemachineVirtualCameraBase tmpCamera = stateCamera.ChildCameras[0];

            stateCamera.ChildCameras[0] = stateCamera.ChildCameras[data.lastCameraId];
            stateCamera.ChildCameras[data.lastCameraId] = tmpCamera;
            controller.mainCamera.position = stateCamera.ChildCameras[0].gameObject.transform.position;

        }
    }
    
    public void SetCameraId(CameraController controller)
    {
        controller.lastCameraId = Array.IndexOf(stateCamera.ChildCameras, stateCamera.LiveChild);
        if (data != null && data.lastCameraId == controller.lastCameraId)
            controller.lastCameraId = 0;
    }


}
