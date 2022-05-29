using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


[System.Serializable]
public class PlayerData
{
    public float position_x;
    public float position_y;
    public float position_z;

    public int camNumber;
    public bool xAxis;
    public int lastCameraId;
    public int storedCamNumber;
    public List<bool> changesRight = new List<bool>();

    public int levelID;

    public SortedList<int, string> pickedUpItems = new SortedList<int, string>();
    public SortedList<int, string> usedItems = new SortedList<int, string>();


    public List<bool> interactions = new List<bool>();
    public string notebookText;
    public int temporaryDoorid;

    public string questText;
    public List<bool> activated = new List<bool>();
    public List<bool> finished = new List<bool>();
    public PlayerData(GameObject player)
    {
        Debug.Log(player.name);
        CameraController controller = GameObject.FindGameObjectWithTag("CameraController").GetComponent<CameraController>();

        changesRight = GameObject.FindObjectOfType<GetCameraChanges>().ReturnCameraRights();

        position_x = player.transform.position.x;
        position_y = player.transform.position.y;
        position_z = player.transform.position.z;

        camNumber = controller.camNumber;
        xAxis = controller.xAxis;
        lastCameraId = controller.lastCameraId;
        storedCamNumber = controller.camNumberStorage;

        levelID = SceneManager.GetActiveScene().buildIndex;

        PlayerStats stats = player.GetComponent<PlayerStats>();
        pickedUpItems = stats.PickedUpItems;
        usedItems = stats.UsedItems;

        NotebookScript notebook = GameObject.FindObjectOfType<NotebookScript>();

        foreach(NotebookController notebookController in notebook.notebooks)
        {
            interactions.Add(notebookController.activated);
        }
        notebookText = notebook.notebookText.text;
        temporaryDoorid = notebook.temporaryDoorId;

        QuestSystem quests = GameObject.FindObjectOfType<QuestSystem>();

        questText = quests.questsContainer;

       for(int i=0; i<quests.quests.Count;i++)
        {
            activated.Add(quests.quests[i].activated);
            finished.Add(quests.quests[i].finished);
        }

    }


}
