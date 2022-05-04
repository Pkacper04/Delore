using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestItem : MonoBehaviour
{

    public string ItemName { get; set; }
    public int ItemId { get; set; }

    public int Opened { get; set; }

    [SerializeField]
    private Vector3[] positions;

    void Start()
    {
        if (PickupCore.Continued)
        {
            Opened = PlayerPrefs.GetInt(gameObject.name); //wywo³ujemy metode do otworzenia skrzyni
            transform.position = positions[PlayerPrefs.GetInt(gameObject.name+"pos")];
            ItemName = PlayerPrefs.GetString(gameObject.name + "name");
            ItemId = PlayerPrefs.GetInt(gameObject.name + "id");
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

        // Animacja otwierania skrzyni w osobnej metodzie
    }


    public void SaveChest()
    {
        PlayerPrefs.SetInt(gameObject.name, Opened);
    }
}
