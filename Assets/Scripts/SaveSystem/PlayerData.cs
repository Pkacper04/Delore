using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class PlayerData
{

    public float position_x;
    public float position_y;
    public float position_z;
    public int camNumber;

    
    public PlayerData(GameObject player)
    {
        position_x = player.transform.position.x;
        position_y = player.transform.position.y;
        position_z = player.transform.position.z;
        camNumber = GameObject.FindGameObjectWithTag("CameraController").GetComponent<CameraController>().camNumber;
    }


}
