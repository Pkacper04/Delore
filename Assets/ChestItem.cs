using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ChestItem : MonoBehaviour
{

    public string ItemName { get; set; }
    public int ItemId { get; set; }

    public int Opened { get; set; }

    [SerializeField]
    private AudioTrigger trigger;

    [SerializeField]
    private AudioClip openSound;

    [SerializeField]
    private Transform chestLid;

    [SerializeField]
    private NotebookScript notebook;

    public int enableQuestId;
    public int disableQuestId;


    public void OpenChest()
    {
        if (Opened == 1)
            return;
        Opened = 1;
        notebook.UpdateNotebook(this);
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
    }

    public void ChestOpened()
    {
        Opened = 1;
        chestLid.Rotate(-90,0,0);
    }




}
