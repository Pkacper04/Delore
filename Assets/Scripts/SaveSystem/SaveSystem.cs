using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

public class SaveSystem : MonoBehaviour
{

    private static Animator saveIcon;
    private void Start()
    {
        saveIcon = GameObject.Find("SaveIcon").GetComponent<Animator>();
    }

    public static void SavePlayer(GameObject player)
    {
        PlayerData data = new PlayerData(player);

        SaveGame(data);
    }



    public static PlayerData LoadPlayer()
    {
        string path = Application.persistentDataPath + "/player.save";
        if (!File.Exists(path))
            return null;

        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream fs = new FileStream(path, FileMode.Open);
        PlayerData data = binaryFormatter.Deserialize(fs) as PlayerData;
        fs.Close();

        return data;
    }

    public static void DeletePlayerSave()
    {
        string path = Application.persistentDataPath + "/player.save";
        File.Delete(path);
    }


    public static async void SaveGame(PlayerData data)
    {

        saveIcon.SetBool("Saving", true);
        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "/player.save";
        FileStream fs = new FileStream(path, FileMode.Create);


        formatter.Serialize(fs, data);
        fs.Close();

        await Task.Delay(5000);
        saveIcon.SetBool("Saving", false);

    }



}
