using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCircle : MonoBehaviour
{
    [SerializeField]
    private List<CameraChanging> changers;
    public void ChangeRight()
    {
        foreach(CameraChanging change in changers)
        {
            change.right = !change.right;
        }
    }
}
