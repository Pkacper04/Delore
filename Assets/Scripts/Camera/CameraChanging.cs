using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraChanging : MonoBehaviour
{
    [SerializeField] private bool changeAxis = false;
    
    private CameraController controller;
    private bool right = false;

    void Start()
    {
        controller = GameObject.FindGameObjectWithTag("CameraController").GetComponent<CameraController>();
        GameObject Player = GameObject.FindWithTag("Player");
        if (!changeAxis)
        {
            if (Player.transform.position.x > transform.position.x)
                right = true;
        }
        else
        {
            if (Player.transform.position.z < transform.position.z)
                right = true;

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            right = !right;
            if(!changeAxis)
                controller.ChangeCamera(right);
            else
                controller.ChangeAxis(right);
        }
    }
}
