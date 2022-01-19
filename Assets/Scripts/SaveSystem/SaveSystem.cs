using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveSystem : MonoBehaviour
{
    public static void SavePlayer(GameObject player)
    {
        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "/player.save";
        FileStream fs = new FileStream(path, FileMode.Create);

        PlayerData data = new PlayerData(player);

        formatter.Serialize(fs, data);
        fs.Close();

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
}
