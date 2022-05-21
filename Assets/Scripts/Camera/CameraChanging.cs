using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class CameraChanging : MonoBehaviour
{
    [SerializeField] private bool changeAxis = false;
    [SerializeField, ShowIf("changeAxis")] private int cameraIdX;
    [SerializeField, ShowIf("changeAxis")] private int cameraIdZ;
    
    private CameraController controller;
    private bool right = false;
    private GameObject player;

    void Start()
    {
        controller = GameObject.FindGameObjectWithTag("CameraController").GetComponent<CameraController>();
        player = GameObject.FindWithTag("Player");
        if (!changeAxis)
        {
            if (player.transform.position.x > transform.position.x)
                right = true;
        }
        else
        {
            if (player.transform.position.z > transform.position.z)
                right = true;

        }
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Player" && !other.isTrigger)
        {
            if (!changeAxis)
            {
                if (controller.xAxis)
                {
                    right = player.transform.position.x > transform.position.x ? false : true;
                    controller.ChangeCamera(right);
                }
                else
                { 
                    right = player.transform.position.z > transform.position.z ? false : true;
                    controller.ChangeCamera(right);
                }
            }
            else
            {
                controller.ChangeAxis(!controller.xAxis, cameraIdX, cameraIdZ);
            }
        }
    }
}
