using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class PlayerStats : MonoBehaviour
{
     public List<GameObject> PickedUpItems = new List<GameObject>();


    public void AddItem(GameObject item)
    {
        PickedUpItems.Add(item);
    }

    public void DeleteItem(GameObject item)
    {
        PickedUpItems.Remove(item);
    }
}
