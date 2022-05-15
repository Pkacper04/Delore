using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class PlayerStats : MonoBehaviour
{
    public SortedList<int,string> PickedUpItems = new SortedList<int,string>();
    public SortedList<int, string> UsedItems = new SortedList<int, string>();

    private void Awake()
    {
        PlayerData data = SaveSystem.LoadPlayer();
        if (data != null)
        {
            PickedUpItems = data.pickedUpItems;
            UsedItems = data.usedItems;
        }
    }

    private void Update()
    {
        foreach (var item in PickedUpItems.Values)
        {
            Debug.Log(item);
        }
    }

    public void AddItem(int itemId, string itemName)
    {
        PickedUpItems.Add(itemId,itemName);
    }

    public void DeleteItem(int itemId, string itemName)
    {
        PickedUpItems.Remove(itemId);
        UsedItems.Add(itemId,itemName);
    }
}
