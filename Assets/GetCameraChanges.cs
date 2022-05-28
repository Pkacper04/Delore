using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetCameraChanges : MonoBehaviour
{
    public List<CameraChanging> camerasChanging = new List<CameraChanging>();


    private void Start()
    {
        PlayerData data = SaveSystem.LoadPlayer();

        if(data != null)
        {
            for(int i=0; i < camerasChanging.Count; i++) {
                camerasChanging[i].right = data.changesRight[i];
            }
        }
    }

    public List<bool> ReturnCameraRights()
    {
        List<bool> rights = new List<bool>();
        foreach(CameraChanging change in camerasChanging)
        {
            rights.Add(change.right);
        }
        return rights;
    }



}
