using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupCore : MonoBehaviour
{
    public static bool Continued { get; set; }

    [SerializeField]
    private List<ChestItem> chests = new List<ChestItem>();
    [SerializeField]
    private List<string> itemsNames = new List<string>();
    [SerializeField]
    private List<int> itemsId = new List<int>();

    private void Awake()
    {
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
                int item = Random.Range(0, itemsId.Count);
                chests[i].ItemId = itemsId[item];
                chests[i].ItemName = itemsNames[item];
                itemsId.RemoveAt(item);
                itemsNames.RemoveAt(item);
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
