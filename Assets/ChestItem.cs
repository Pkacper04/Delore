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
    private Vector3[] positions;

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
            transform.position = positions[PlayerPrefs.GetInt(gameObject.name+"pos")];
            ItemName = PlayerPrefs.GetString(gameObject.name + "name");
            ItemId = PlayerPrefs.GetInt(gameObject.name + "id");
            if(Opened == 1)
                ChestAnimation();
        }
        else
        {
            PlayerPrefs.SetInt(gameObject.name, 0);
            PlayerPrefs.SetInt(gameObject.name+"id", ItemId);
            PlayerPrefs.SetString(gameObject.name + "name", ItemName);
            Opened = 0;
            if (positions.Length != 0)
            {
                int pos = Random.Range(0, positions.Length);
                PlayerPrefs.SetInt(gameObject.name+"pos", pos);
                transform.position = positions[pos];
            }
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
        Debug.Log("animation");
        for (int i = 0; i < 90; i++)
        {
            chestLid.Rotate(-1, 0, 0);
            await Task.Delay(1);
        }
    }

    public void SaveChest()
    {
        PlayerPrefs.SetInt(gameObject.name, Opened);
    }




}
