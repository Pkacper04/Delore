using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
public class TriggerDialogue : MonoBehaviour
{
    [SerializeField]
    VIDE_Assign assign;

    [SerializeField]
    UIManager uiManager;

    [Tag]
    public string playerTag;


    private bool visited = false;
    private void OnTriggerEnter(Collider other)
    {
        if (visited)
            return;
        if(other.tag == playerTag)
        {
            uiManager.ActivateDialogue(assign);
            visited = true;
        }
    }
}
