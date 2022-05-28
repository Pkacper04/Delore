using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class CameraChanging : MonoBehaviour
{
    [SerializeField] private bool changeAxis = false;
    [SerializeField, ShowIf("changeAxis")] private int cameraIdX;
    [SerializeField, ShowIf("changeAxis")] private int cameraIdZ;

    [SerializeField]
    private bool circle = false;
    [SerializeField, ShowIf("circle")]
    private CameraCircle cameraCircle;

    
    private CameraController controller;

    public bool right = false;
    private GameObject player;
    private Vector3 enterPosition;
    private float rotation;


    void Start()
    {
        controller = GameObject.FindGameObjectWithTag("CameraController").GetComponent<CameraController>();
        player = GameObject.FindWithTag("Player");
        rotation = Mathf.Round(transform.eulerAngles.y);
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Player" && !other.isTrigger)
        {
           
            enterPosition = other.transform.position;
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" && !other.isTrigger)
        {
            Debug.Log("otation: "+rotation);

            if (controller.xAxis)
            {
                if (changeAxis && rotation == 90)
                {
                    Debug.Log("xAxis and change: "+Mathf.Abs((enterPosition.z - other.transform.position.z)));
                    if (Mathf.Abs((enterPosition.z - other.transform.position.z)) < 0.2)
                        return;
                    if (circle)
                        cameraCircle.ChangeRight();
                    controller.ChangeAxis(!controller.xAxis, cameraIdX, cameraIdZ);
                }
                else {
                    Debug.Log("xAxis: " + Mathf.Abs((enterPosition.x - other.transform.position.x)));
                    if (Mathf.Abs((enterPosition.x - other.transform.position.x)) < 0.2)
                    {
                        return;
                    }
                    else
                    {
                        right = !right;
                        if (changeAxis)
                        {
                            if (circle)
                                cameraCircle.ChangeRight();
                            controller.ChangeAxis(!controller.xAxis, cameraIdX, cameraIdZ);
                        }
                        else
                            controller.ChangeCamera(right);
                    }
                }

            }
            else
            {
                if (changeAxis && rotation == 0)
                {
                    Debug.Log("zAxis and change: " + Mathf.Abs((enterPosition.x - other.transform.position.x)));
                    if (Mathf.Abs((enterPosition.x - other.transform.position.x)) < 0.2)
                        return;
                    if (circle)
                        cameraCircle.ChangeRight();
                    controller.ChangeAxis(!controller.xAxis, cameraIdX, cameraIdZ);
                }
                else
                {
                    Debug.Log("zAxis: " + Mathf.Abs((enterPosition.z - other.transform.position.z)));
                    if (Mathf.Abs((enterPosition.z - other.transform.position.z)) < 0.2)
                    {
                        return;
                    }
                    else
                    {
                        right = !right;
                        if (changeAxis)
                        {
                            if (circle)
                                cameraCircle.ChangeRight();
                            controller.ChangeAxis(!controller.xAxis, cameraIdX, cameraIdZ);
                        }
                        else
                            controller.ChangeCamera(right);
                    }
                }
            }
        }
    }



}
