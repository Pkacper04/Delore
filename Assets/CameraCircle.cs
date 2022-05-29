using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCircle : MonoBehaviour
{
    [SerializeField]
    private List<CameraChanging> changers;
    [SerializeField]
    private List<CameraChanging> secondChangers;
    public void ChangeRight()
    {
        foreach(CameraChanging change in changers)
        {
            change.right = !change.right;
        }

 
    }


    public void SetCamerasToZero()
    {
        foreach (CameraChanging change in secondChangers)
        {
            change.right = false;
        }
    }

    public void SetCamerasToOne()
    {
        foreach (CameraChanging change in secondChangers)
        {
            change.right = true;
        }
    }
}
