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

    [SerializeField] private CanvasGroup container_NPC;
    [SerializeField] private CanvasGroup container_Player;
    [SerializeField] private TextWriter text_NPC;
    [SerializeField] private TextWriter text_player;
    [SerializeField] private Image NPC_Sprite;
<<<<<<< HEAD
    private VIDE_Assign asign;
    [SerializeField] private GameObject GameOverScreen;
    [SerializeField] private Movement gameOverTrigger;
    [SerializeField] private float dialogueSpeed;
=======
    [SerializeField] VIDE_Assign asign;
>>>>>>> LevelDesign


    private GraphicRaycaster raycaster;
    // Start is called before the first frame update
    void Start()
    {
<<<<<<< HEAD
        GameOverScreen.SetActive(false);
        DisableCanvas(container_NPC);
        DisableCanvas(container_Player);
=======
        container_NPC.SetActive(false);
        container_Player.SetActive(false);
>>>>>>> LevelDesign
        raycaster = GetComponent<GraphicRaycaster>();
    }

    private void OnEnable()
    {
        gameOverTrigger.Triggered += GameOver;
    }

    private void ActivateCanvas(CanvasGroup canvas)
    {
        canvas.alpha = 1;
        canvas.blocksRaycasts = true;
        canvas.interactable = true;
    }

    private void DisableCanvas(CanvasGroup canvas)
    {
        canvas.alpha = 0;
        canvas.blocksRaycasts = false;
        canvas.interactable = false;
    }

    

    // Update is called once per frame
    void Update()
    {
<<<<<<< HEAD

=======
>>>>>>> LevelDesign
        if(PauseController.GamePaused)
            raycaster.enabled = false;
        else
            raycaster.enabled = true;

<<<<<<< HEAD
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
=======
        if(Input.GetKeyDown(KeyCode.Return))
>>>>>>> LevelDesign
        {
            if (VD.isActive)
            {
                VD.Next();
            }
        }
    }

    public void ActivateDialogue(VIDE_Assign videAssign)
    {
        asign = videAssign;
        if (!VD.isActive)
            Begin();
        
    }

    void Begin()
    {
        VD.OnNodeChange += UpdateUI;
        VD.OnEnd += End;
        VD.BeginDialogue(asign);
    }

    private void UpdateUI(VD.NodeData data)
    {
        DisableCanvas(container_NPC);
        DisableCanvas(container_Player);



        if (data.isPlayer)
        {
            ActivateCanvas(container_Player);
            text_player.ClearDialogue();
            text_player.BuildText(data.comments[data.commentIndex],dialogueSpeed);
        }
        else
        {
            ActivateCanvas(container_NPC);
            if (text_NPC.TextIsBuilding)
                text_NPC.StopBuildingText();
            text_NPC.ClearDialogue();
            NPC_Sprite.sprite = asign.defaultNPCSprite;
            text_NPC.BuildText(data.comments[data.commentIndex], dialogueSpeed);
        }
    }

    private void End(VD.NodeData data)
    {
        DisableCanvas(container_NPC);
        DisableCanvas(container_Player);
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
