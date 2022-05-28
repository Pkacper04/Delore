using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


[System.Serializable]
public class PlayerData
{

    public float position_x;
    public float position_y;
    public float position_z;
    public int camNumber;
    public bool xAxis;
    public int lastCameraId;
    public int storedCamNumber;
    public int levelID;
    public SortedList<int, string> pickedUpItems = new SortedList<int, string>();
    public SortedList<int, string> usedItems = new SortedList<int, string>();
    public List<bool> changesRight = new List<bool>();
    
    public PlayerData(GameObject player)
    {
        Debug.Log(player.name);
        CameraController controller = GameObject.FindGameObjectWithTag("CameraController").GetComponent<CameraController>();

        changesRight = GameObject.FindObjectOfType<GetCameraChanges>().ReturnCameraRights();

        position_x = player.transform.position.x;
        position_y = player.transform.position.y;
        position_z = player.transform.position.z;

        Debug.Log("pos x: "+position_x);
        Debug.Log("pos y: "+position_y);
        Debug.Log("pos z: "+position_z);

        camNumber = controller.camNumber;
        xAxis = controller.xAxis;
        lastCameraId = controller.lastCameraId;
        storedCamNumber = controller.camNumberStorage;

        levelID = SceneManager.GetActiveScene().buildIndex;

        PlayerStats stats = player.GetComponent<PlayerStats>();
        pickedUpItems = stats.PickedUpItems;
        foreach(int keyId in pickedUpItems.Keys)
        {
            Debug.Log("Saved keys id: "+keyId);
        }

        foreach (string keyName in pickedUpItems.Values)
        {
            Debug.Log("Saved keys name: " + keyName);
        }
        usedItems = stats.UsedItems;
    }


}
