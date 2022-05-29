using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupCore : MonoBehaviour
{
    public static bool Continued { get; set; }

    public List<ChestItem> chests = new List<ChestItem>();

    public List<string> itemsNames = new List<string>();

    public List<int> itemsId = new List<int>();

    [SerializeField]
    private PlayerStats stats;

    private void Awake()
    {
        if (!this.enabled)
            return;
        for(int i=0;i<chests.Count;i++)
        {
            chests[i].ItemId = itemsId[i];
            chests[i].ItemName = itemsNames[i];
        }
        
    }


    private void Start()
    {
        foreach (ChestItem chest in chests)
        {
            if (stats.PickedUpItems.ContainsKey(chest.ItemId))
            {
                chest.ChestOpened();
            }
            else if (stats.UsedItems.ContainsKey(chest.ItemId))
            {
                chest.ChestOpened();
            }
        }
    }



}
