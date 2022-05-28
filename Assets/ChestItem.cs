using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ChestItem : MonoBehaviour
{
    private Transform chestLid;

    public string ItemName { get; set; }
    public int ItemId { get; set; }

    public int Opened { get; set; }


    [SerializeField]
    private AudioTrigger trigger;

    [SerializeField]
    private AudioClip openSound;

    void Start()
    {
       foreach(Transform item in GetComponentsInChildren<Transform>())
        {
            if(item.name == "Chest_Lid")
                chestLid = item;
        }
        Debug.Log(chestLid.name);
        if (PickupCore.Continued)
        {
            Opened = PlayerPrefs.GetInt(gameObject.name);
            ItemName = PlayerPrefs.GetString(gameObject.name + "name");
            ItemId = PlayerPrefs.GetInt(gameObject.name + "id");
            if (Opened == 1)
                ChestOpened();
        }
        else
        {
            PlayerPrefs.SetInt(gameObject.name, 0);
            PlayerPrefs.SetInt(gameObject.name+"id", ItemId);
            PlayerPrefs.SetString(gameObject.name + "name", ItemName);
            Opened = 0;
        }

    }

    public void OpenChest()
    {
        if (Opened == 1)
            return;
        Opened = 1;

        ChestAnimation();
    }

    private async void ChestAnimation()
    {
        trigger.playOneTime(openSound);
        Debug.Log("animation");
        for (int i = 0; i < 45; i++)
        {
            chestLid.Rotate(-2, 0, 0);
            await Task.Delay(1);
        }
        SaveChest();
    }

    private void ChestOpened()
    {
        chestLid.Rotate(-90,0,0);
    }
    public void SaveChest()
    {
        PlayerPrefs.SetInt(gameObject.name, Opened);
    }




}
