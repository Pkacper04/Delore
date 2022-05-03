using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using VIDE_Data;
using UnityEngine.UI;
using Delore.Player;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{

    [SerializeField] private GameObject container_NPC;
    [SerializeField] private GameObject container_Player;
    [SerializeField] private TMP_Text text_NPC;
    [SerializeField] private TMP_Text[] buttons_text;
    [SerializeField] private Image NPC_Sprite;
    [SerializeField] VIDE_Assign asign;
    [SerializeField] private GameObject GameOverScreen;
    [SerializeField] private Movement gameOverTrigger;


    private GraphicRaycaster raycaster;
    // Start is called before the first frame update
    void Start()
    {
        GameOverScreen.SetActive(false);
        container_NPC.SetActive(false);
        container_Player.SetActive(false);
        raycaster = GetComponent<GraphicRaycaster>();
    }

    private void OnEnable()
    {
        gameOverTrigger.Triggered += GameOver;
    }



    

    // Update is called once per frame
    void Update()
    {

        if(PauseController.GamePaused)
            raycaster.enabled = false;
        else
            raycaster.enabled = true;

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
    }

    private void OnDisable()
    {
        if (container_NPC != null)
            End(null);


        gameOverTrigger.Triggered -= GameOver;
    }

    public void SetPlayerChoice(int choice)
    {
        VD.nodeData.commentIndex = choice;
        if (Input.GetMouseButtonUp(0))
            VD.Next();
    }


    public void RestartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
    }


    private void GameOver()
    {
        GameOverScreen.SetActive(true);
        Time.timeScale = 0;
        PauseController.GameEnded = true;
    }


}
