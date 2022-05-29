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
    [SerializeField] private Image Player_Sprite;
    [SerializeField] private GameObject GameOverScreen;
    [SerializeField] private Movement gameOverTrigger;
    [SerializeField] private float dialogueSpeed;
    [SerializeField] private float duration;
    [SerializeField] private float deathduration;
    [SerializeField] private float startingDuration;
    [SerializeField] private float dialogueDuration;
    [SerializeField] private CanvasGroup blackScreen;
    [SerializeField] private CanvasGroup thanksForPlaying;
    [SerializeField] private SimpleAnimation simpleAnimation;
    [SerializeField] private MouseController mouseController;

    [Scene]
    public string afterLifeScene;
    [Scene]
    public string loadingScene;
    [Scene]
    public string restartScene;
    [Scene]
    public string mainMenuScene;

    private bool GameFinished = false;

    private VIDE_Assign asign;
    private GraphicRaycaster raycaster;
    private bool firstDialogue = true;
    // Start is called before the first frame update
    void Start()
    {
        thanksForPlaying.alpha = 0;
        thanksForPlaying.blocksRaycasts = false;
        thanksForPlaying.interactable = false;
        blackScreen.alpha = 1;
        blackScreen.blocksRaycasts = true;
        StartCoroutine(SmoothStarting());
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
        if(GameFinished)
        {
            if(Input.anyKeyDown)
            {
                SceneManager.LoadScene(mainMenuScene);
            }
        }

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
                Player_Sprite.sprite = asign.defaultPlayerSprite;
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
            StartCoroutine(mouseController.StartProlog());
            
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
        StartCoroutine(SmoothEnding(false));
    }

    public IEnumerator SmoothStarting()
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

    public IEnumerator SmoothEnding(bool changescene = true)
    {
        float blackAlpha = 0;
        while (blackScreen.alpha < 1)
        {
            if(changescene)
                blackAlpha += Time.unscaledDeltaTime / startingDuration;
            else
                blackAlpha += Time.unscaledDeltaTime / deathduration;
            blackAlpha = Mathf.Clamp01(blackAlpha);
            blackScreen.alpha = blackAlpha;
            yield return null;
        }
        blackScreen.blocksRaycasts = true;
        if(changescene)
            SceneManager.LoadScene(loadingScene);
        else
        {
            GameOverScreen.SetActive(true);
            Time.timeScale = 0;
            PauseController.GameEnded = true;
        }
    }


    public IEnumerator SmoothEndingGame()
    {
        float blackAlpha = 0;
        while (blackScreen.alpha < 1)
        {
            blackAlpha += Time.unscaledDeltaTime / deathduration;
            blackAlpha = Mathf.Clamp01(blackAlpha);
            blackScreen.alpha = blackAlpha;
            yield return null;
        }
        blackAlpha = 0;
        simpleAnimation.startAnimation = true;
        Debug.Log("ending");
        while (thanksForPlaying.alpha < 1)
        {
            blackAlpha += Time.unscaledDeltaTime / deathduration;
            blackAlpha = Mathf.Clamp01(blackAlpha);
            thanksForPlaying.alpha = blackAlpha;
            Debug.Log("alpha: "+thanksForPlaying.alpha);
            yield return null;
        }
        thanksForPlaying.blocksRaycasts = true;
        GameFinished = true;
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
