using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NotebookController
{
    public LockedDoor door1;
    public LockedDoor door2;
    public ChestItem chest;
    public string firstDoorText;
    public string firstChestText;
    public string secondDoorText;
    public bool activated;
}

/*[System.Serializable]
public class NotebookTexts
{
    public List<NotebookController> notebookTexts = new List<NotebookController>();
}*/
