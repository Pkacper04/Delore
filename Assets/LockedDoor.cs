using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using NaughtyAttributes;
public class LockedDoor : MonoBehaviour
{
    [SerializeField] int keyId;
    [SerializeField] float rotateValue;
    [SerializeField] float rotateSize = 120;
    public float stoppingDistance = 2;
    public bool Locked = true;

    [SerializeField]
    private bool doubleDoor = false;

    [SerializeField, ShowIf("doubleDoor")]
    private LockedDoor secondDoor;
    [SerializeField, ShowIf("doubleDoor")]
    private MeshCollider secondDoorCollider;

    private PlayerStats stats;
    private PickupCore core;
    private MeshCollider meshCollider;
    private void Start()
    {
        meshCollider = GetComponent<MeshCollider>();
        meshCollider.enabled = true;
        if(secondDoor)
            secondDoorCollider.enabled = true;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        stats = player.GetComponent<PlayerStats>();
        core = player.GetComponent<PickupCore>();
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
                Opening();
                secondDoor.Opening();
            }
            else
            {
                Locked = false;
                Opening();
            }
        }
        else
        {
            Debug.Log("Potrzebujesz klucza o nazwie: "+core.itemsNames[keyId]);
        }
    }

    public async void Opening()
    {
        for (int i = 0; i < Mathf.Abs(rotateSize/rotateValue); i++)
        {
            transform.Rotate(0, rotateValue, 0);
            await Task.Delay(1);
        }
    
    }
}
