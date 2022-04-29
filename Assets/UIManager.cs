using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using VIDE_Data;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    [SerializeField] private GameObject container_NPC;
    [SerializeField] private GameObject container_Player;
    [SerializeField] private TMP_Text text_NPC;
    [SerializeField] private TMP_Text[] buttons_text;
    [SerializeField] private Image NPC_Sprite;
    [SerializeField] VIDE_Assign asign;


    private GraphicRaycaster raycaster;
    // Start is called before the first frame update
    void Start()
    {
        raycaster = GetComponent<GraphicRaycaster>();
        container_NPC.SetActive(false);
        container_Player.SetActive(false);
        raycaster.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            if(!VD.isActive)
            {
                Begin();
            }
            else
            {
                VD.Next();
            }
        }
    }

    void Begin()
    {
        raycaster.enabled = true;
        PauseController.GamePaused = true;
        VD.OnNodeChange += UpdateUI;
        VD.OnEnd += End;
        VD.BeginDialogue(asign);
    }

    private void UpdateUI(VD.NodeData data)
    {
        container_NPC.SetActive(false);
        container_Player.SetActive(false);
        if(data.isPlayer)
        {
            container_Player.SetActive(true);

            for(int i=0; i<buttons_text.Length;i++)
            {
                if(i<data.comments.Length)
                {
                    buttons_text[i].transform.parent.gameObject.SetActive(true);
                    buttons_text[i].text = data.comments[i];
                }
                else
                {
                    buttons_text[i].transform.parent.gameObject.SetActive(false);
                }
            }
        }
        else
        {
            container_NPC.SetActive(true);
            NPC_Sprite.sprite = asign.defaultNPCSprite;
            text_NPC.text = data.comments[data.commentIndex];
        }
    }

    private void End(VD.NodeData data)
    {
        container_NPC.SetActive(false);
        container_Player.SetActive(false);
        VD.OnNodeChange -= UpdateUI;
        VD.OnEnd -= End;
        VD.EndDialogue();
        PauseController.GamePaused = false;
        raycaster.enabled = false;
    }

    private void OnDisable()
    {
        if (container_NPC != null)
            End(null);
    }

    public void SetPlayerChoice(int choice)
    {
        VD.nodeData.commentIndex = choice;
        if (Input.GetMouseButtonUp(0))
            VD.Next();
    }
}
