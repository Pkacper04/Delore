using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using NaughtyAttributes;
using System;

public class NotebookScript : MonoBehaviour
{


    public TMP_Text notebookText;
    [SerializeField]
    private Animator notificationAnimator;

    [SerializeField, AnimatorParam("notificationAnimator")]
    private string showNotification;

    [SerializeField]
    private float notificationTime;

    [SerializeField]
    private AudioSource UiAudio;

    [SerializeField]
    private AudioClip notificationClip;

    [SerializeField]
    private QuestSystem quests;

    public List<NotebookController> notebooks = new List<NotebookController>();

    public int temporaryDoorId = -1;

    private string header;
    private string content;
    private string textToEdit;

    private void Start()
    {
        temporaryDoorId = -1;
        PlayerData data = SaveSystem.LoadPlayer();

        if(data != null)
        {
            temporaryDoorId = data.temporaryDoorid;
            notebookText.text = data.notebookText;
            for(int i=0;i<notebooks.Count;i++)
            {
                notebooks[i].activated = data.interactions[i];
            }
        }
    }

    public void UpdateNotebook(LockedDoor door = null)
    {
        if (IsActivated(door))
        {
            Debug.Log("Door locked: "+door.Locked);
            if (!door.Locked)
            {
                if (door.LastDoor)
                    return;
                quests.DeleteQuest(door.disableQuestId);
                content = SetContent(door);
                if (content == null)
                {
                    Debug.LogError("Text or values were not set");
                    return;
                }

                content += "\n\n";
                textToEdit = header + content + notebookText.text;
                notebookText.text = textToEdit;
                StartCoroutine(ShowNotification());
            }
                return;
        }
        temporaryDoorId = door.keyId;


        content = SetContent(door);
        if(content == null)
        {
            Debug.LogError("Text or values were not set");
            return;
        }

        content += "\n\n";
        textToEdit = header + content + notebookText.text;
        notebookText.text = textToEdit;
        StartCoroutine(ShowNotification());

        if (!door.Locked)
        {
            quests.DeleteQuest(door.disableQuestId);
        }
        else
            quests.BuildTextQuest(door.enableQuestId);

    }

    public void UpdateNotebook(ChestItem chest = null)
    {
        quests.DeleteQuest(chest.disableQuestId);
        if (IsActivated(chest))
            quests.BuildTextQuest(chest.enableQuestId);
        else
        {
            int textLength = quests.quests[chest.enableQuestId].questName.Length;
            quests.SetQuestWithDelay(chest.enableQuestId, (textLength * quests.questBuildingTime + textLength * quests.questLineThroughtSpeed + textLength * quests.dissaperTime + 2f));
        }


        header = "<uppercase><b>" + "Chest Interaction" + "</b></uppercase>" + "\n";

        content = SetContent(chest);
        if (content == null)
        {
            Debug.LogError("Text or values were not set");
            return;
        }

        content += "\n\n";
        textToEdit = header + content + notebookText.text;
        notebookText.text = textToEdit;
        StartCoroutine(ShowNotification());
        
    }

    public void UpdateNotebook(string header, string content)
    {
        this.header = "<uppercase><b>" + header + "</b></uppercase>" +"\n";
        this.content = content + "\n\n";
        textToEdit = this.header + this.content + notebookText.text;
        notebookText.text = textToEdit;
        StartCoroutine(ShowNotification());
    }

    private string SetContent(LockedDoor door)
    {
        foreach (NotebookController controller in notebooks)
        {
            if (controller.door1.keyId == door.keyId || controller.door2.keyId == door.keyId)
            {
                if (controller.activated)
                {
                    header = "<uppercase><b>" + "Opened "+controller.door1.name + "</b></uppercase>" + "\n";
                    return controller.secondDoorText;
                }
                else
                {
                    header = "<uppercase><b>" + "Checked Closed " + controller.door1.name + "</b></uppercase>" + "\n";
                    controller.activated = true;
                    return controller.firstDoorText;
                }
            }
        }
        return null;
    }

    private string SetContent(ChestItem chest)
    {
        foreach (NotebookController controller in notebooks)
        {
            if (controller.chest.ItemId == chest.ItemId)
            {
                header = "<uppercase><b>" + "Found the " + controller.chest.ItemName + "</b></uppercase>" + "\n";
                controller.activated = true;
                return controller.firstChestText;
                
            }
        }
        return null;
    }

    private bool IsActivated(ChestItem chest)
    {
        foreach (NotebookController controller in notebooks)
        {
            if (controller.chest.ItemId == chest.ItemId)
            {
                return controller.activated;
            }
        }
        return false;
    }

    private bool IsActivated(LockedDoor door)
    {
        foreach (NotebookController controller in notebooks)
        {
            if (controller.door1.keyId == door.keyId)
            {
                return controller.activated;
            }
            else if(controller.door2.keyId == door.keyId)
            {
                return controller.activated;
            }
        }
        return false;
    }


    private IEnumerator ShowNotification()
    {
        UiAudio.PlayOneShot(notificationClip);
        notificationAnimator.SetBool(showNotification, true);
        yield return new WaitForSecondsRealtime(notificationTime);
        notificationAnimator.SetBool(showNotification, false);
    }


}
