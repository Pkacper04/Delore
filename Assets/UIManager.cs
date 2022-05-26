using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using VIDE_Data;
using UnityEngine.UI;
using Delore.Player;
using UnityEngine.SceneManagement;
using NaughtyAttributes;

public class UIManager : MonoBehaviour
{

    [SerializeField] private CanvasGroup container_NPC;
    [SerializeField] private CanvasGroup container_Player;
    [SerializeField] private TextWriter text_NPC;
    [SerializeField] private TextWriter text_player;
    [SerializeField] private Image NPC_Sprite;
    [SerializeField] private GameObject GameOverScreen;
    [SerializeField] private Movement gameOverTrigger;
    [SerializeField] private float dialogueSpeed;
    [SerializeField] private Material playerDissolveMaterial;
    [SerializeField] private float duration;
    [SerializeField] private float startingDuration;
    [SerializeField] private float dialogueDuration;
    [SerializeField] private CanvasGroup blackScreen;

    [Scene]
    public string afterLifeScene;
    [Scene]
    public string loadingScene;
    [Scene]
    public string restartScene;
    [Scene]
    public string mainMenuScene;

    private VIDE_Assign asign;
    private GraphicRaycaster raycaster;
    private bool firstDialogue = true;
    // Start is called before the first frame update
    void Start()
    {
        blackScreen.alpha = 1;
        blackScreen.blocksRaycasts = true;
        StartCoroutine(SmoothStarting());
        playerDissolveMaterial.SetFloat("DisolveValue_",0);
        GameOverScreen.SetActive(false);
        DisableCanvas(container_NPC);
        DisableCanvas(container_Player);
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

        if(PauseController.GamePaused)
            raycaster.enabled = false;
        else
            raycaster.enabled = true;

        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
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

        if (firstDialogue)
            StartCoroutine(EnableDialogue(data));
        else
        {

            if (data.isPlayer)
            {
                ActivateCanvas(container_Player);
                if (text_player.TextIsBuilding)
                    text_player.StopBuildingText();
                text_player.ClearDialogue();
                text_player.BuildText(data.comments[data.commentIndex], dialogueSpeed);
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
    }

    private void End(VD.NodeData data)
    {
        DisableCanvas(container_NPC);
        DisableCanvas(container_Player);
        VD.OnNodeChange -= UpdateUI;
        VD.OnEnd -= End;
        VD.EndDialogue();
        if (SceneManager.GetActiveScene().name == afterLifeScene)
            StartCoroutine(PlayerDissolver());
    }

    private void OnDisable()
    {
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
        SceneManager.LoadScene(restartScene);
    }

    public void ExitGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(mainMenuScene);
    }


    private void GameOver()
    {
        GameOverScreen.SetActive(true);
        Time.timeScale = 0;
        PauseController.GameEnded = true;
    }

    private IEnumerator PlayerDissolver()
    {
        yield return new WaitForSeconds(.5f);
        float disolve = 0;

        while (disolve < 1f)
        {
            disolve += Time.deltaTime / duration;
            disolve = Mathf.Clamp01(disolve); 
            playerDissolveMaterial.SetFloat("DisolveValue_", disolve);
            yield return null;
        }

        yield return new WaitForSeconds(1);
        
        if (SceneManager.GetActiveScene().name == afterLifeScene)
        {
            gameOverTrigger.gameObject.SetActive(false);
            playerDissolveMaterial.SetFloat("DisolveValue_", 0);
            PauseController.GamePaused = false;
            PauseController.BlockPauseMenu = false;
            StartCoroutine(SmoothEnding());
        }
        else
        {
            GameOver();
        }
    }

    private IEnumerator SmoothStarting()
    {
        float blackAlpha = 1;
        while(blackScreen.alpha > 0)
        {
            blackAlpha -= Time.unscaledDeltaTime / startingDuration;
            blackAlpha = Mathf.Clamp01(blackAlpha);
            blackScreen.alpha = blackAlpha;
            yield return null;
        }
        blackScreen.blocksRaycasts = false;
    }

    private IEnumerator SmoothEnding()
    {
        float blackAlpha = 0;
        while (blackScreen.alpha < 1)
        {
            blackAlpha += Time.unscaledDeltaTime / startingDuration;
            blackAlpha = Mathf.Clamp01(blackAlpha);
            blackScreen.alpha = blackAlpha;
            yield return null;
        }
        blackScreen.blocksRaycasts = true;
        SceneManager.LoadScene(loadingScene);
    }

    private IEnumerator EnableDialogue(VD.NodeData data)
    {
        firstDialogue = false;
        float alpha = 0;

        if (data.isPlayer)
        {
            while(container_Player.alpha < 1)
            {
                alpha += Time.deltaTime / dialogueDuration;
                alpha = Mathf.Clamp01(alpha);
                container_Player.alpha = alpha;
                yield return null;
            }

            ActivateCanvas(container_Player);
            if (text_player.TextIsBuilding)
                text_player.StopBuildingText();
            text_player.ClearDialogue();
            text_player.BuildText(data.comments[data.commentIndex], dialogueSpeed);
        }
        else
        {
            while (container_NPC.alpha < 1)
            {
                alpha += Time.deltaTime / dialogueDuration;
                alpha = Mathf.Clamp01(alpha);
                container_NPC.alpha = alpha;
                yield return null;
            }
            ActivateCanvas(container_NPC);
            if (text_NPC.TextIsBuilding)
                text_NPC.StopBuildingText();
            text_NPC.ClearDialogue();
            NPC_Sprite.sprite = asign.defaultNPCSprite;
            text_NPC.BuildText(data.comments[data.commentIndex], dialogueSpeed);
        }
    }
}
