using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class PlayerStats : MonoBehaviour
{
    public SortedList<int,string> PickedUpItems = new SortedList<int,string>();
    public SortedList<int, string> UsedItems = new SortedList<int, string>();
    public List<LockedDoor> doors = new List<LockedDoor>();
    private void Awake()
    {
        PlayerData data = SaveSystem.LoadPlayer();
        if (data != null)
        {
            PickedUpItems = data.pickedUpItems;
            UsedItems = data.usedItems;

            foreach (int keyId in PickedUpItems.Keys)
            {
                Debug.Log("Restored keys id: " + keyId);
            }

            foreach (string keyName in PickedUpItems.Values)
            {
                Debug.Log("Restored keys name: " + keyName);
            }

            foreach (LockedDoor door in doors)
            {
                if(UsedItems.ContainsKey(door.keyId))
                {
                    door.Locked = false;
                    door.Open();
                }
            }
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

    public void DeleteItem(int itemId)
    {
        UsedItems.Add(itemId, PickedUpItems[itemId]);
        PickedUpItems.Remove(itemId);
        
    }

    public bool CheckItem(int itemID)
    {
        return PickedUpItems.Keys.Contains(itemID);
    }

}
