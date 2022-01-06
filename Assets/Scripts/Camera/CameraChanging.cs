using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraChanging : MonoBehaviour
{
    private CameraController controller;
    private bool right = false;

    void Start()
    {
        controller = GameObject.FindGameObjectWithTag("CameraController").GetComponent<CameraController>();
        GameObject Player = GameObject.FindWithTag("Player");
        if (Player.transform.position.x > transform.position.x)
            right = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            right = !right;
            controller.ChangeCamera(right);
        }
    }
}
