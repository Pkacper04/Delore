using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupCore : MonoBehaviour
{
    public static bool Continued { get; set; }

    public List<ChestItem> chests = new List<ChestItem>();

    public List<string> itemsNames = new List<string>();

    public List<int> itemsId = new List<int>();

    private void Awake()
    {
        if (!this.enabled)
            return;
        PlayerData data = SaveSystem.LoadPlayer();
        if (data != null)
        {
            Continued = true;
        }
        else
        {
            Continued = false;
            for(int i=0;i<chests.Count;i++)
            {
                chests[i].ItemId = itemsId[i];
                chests[i].ItemName = itemsNames[i];
            }
        }
    }




    public void SaveChests()
    {
        foreach (ChestItem chest in chests)
        {
            chest.SaveChest();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
