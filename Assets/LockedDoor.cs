using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using NaughtyAttributes;
public class LockedDoor : MonoBehaviour
{
    public int keyId;
    [SerializeField] float rotateValue;
    [SerializeField] float rotateSize = 120;
    public float stoppingDistance = 2;
    public bool Locked = true;
    public int enableQuestId;
    public int disableQuestId;

    [SerializeField]
    private NotebookScript notebook;

    [SerializeField]
    private bool doubleDoor = false;

    [SerializeField]
    private bool safeOpen = false;

    [SerializeField, ShowIf("doubleDoor")]
    private LockedDoor secondDoor;
    [SerializeField, ShowIf("doubleDoor")]
    private MeshCollider secondDoorCollider;

    [SerializeField, ShowIf("safeOpen")]
    private string checkPosition;



    public bool LastDoor = false;

    [SerializeField, ShowIf("LastDoor")]
    private EndGame endGame;

    [SerializeField]
    private AudioTrigger trigger;

    [SerializeField]
    private AudioClip openSound;

    private PlayerStats stats;
    private MeshCollider meshCollider;

    private void Start()
    {
        meshCollider = GetComponent<MeshCollider>();
        meshCollider.enabled = true;
        if(secondDoor)
            secondDoorCollider.enabled = true;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        stats = player.GetComponent<PlayerStats>();
    }
    public void OpenDoor()
    {
        if (!Locked)
            return;

        
        if (stats.CheckItem(keyId))
        {
            
            stats.DeleteItem(keyId);
            if (doubleDoor)
            {

                Locked = false;
                secondDoor.Locked = false;
                if(safeOpen)
                {
                    if(checkPosition == "z")
                    {
                        if(transform.position.z < stats.transform.position.z)
                        {
                            rotateValue *= -1;
                            secondDoor.rotateValue *= -1;
                        }
                    }
                    else if(checkPosition == "x")
                    {
                        if (transform.position.x < stats.transform.position.x)
                        {
                            rotateValue *= -1;
                            secondDoor.rotateValue *= -1;
                        }
                    }
                }

                Opening();
                secondDoor.Opening();
            }
            else
            {
                Locked = false;
                Opening();
            }
        }
        notebook.UpdateNotebook(this);
    }

    public async void Opening()
    {
        trigger.playOneTime(openSound);
        for (int i = 0; i < Mathf.Abs(rotateSize/rotateValue); i++)
        {
            transform.Rotate(0, rotateValue, 0);
            await Task.Delay(1);
        }

        if (LastDoor)
            endGame.GameEnd();
    
    }

    public void Open()
    {
        transform.Rotate(0, Mathf.Abs(rotateSize / rotateValue)*rotateValue,0);
    }
}
