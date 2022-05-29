using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using NaughtyAttributes;
using System;

public class QuestSystem : MonoBehaviour
{

    [SerializeField]
    private TMP_Text questPlace;

    public string questsContainer;

    [SerializeField]
    private TextWriter textWriter;


    public float questBuildingTime;

    public float questLineThroughtSpeed;


    public float dissaperTime;

    public float dissaperDelay;

    [SerializeField]
    private Animator questAnimator;

    [SerializeField, AnimatorParam("questAnimator")]
    private string showQuestPanel;

    public List<QuestData> quests = new List<QuestData>();

    private Coroutine showWithDelay;
    private bool questPanelActive = false;
    private bool textDeleting = false;

    private void Start()
    {
        PlayerData data = SaveSystem.LoadPlayer();
        if(data != null)
        {
            questsContainer = data.questText;
            questPlace.text = questsContainer;
            for(int i = 0; i < quests.Count; i++)
            {
                quests[i].activated = data.activated[i];
                quests[i].finished = data.finished[i];
            }
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            questPanelActive = !questPanelActive;
            ShowQuestPanel(questPanelActive);
        }
    }
    public void BuildTextQuest(int questID = -1)
    {
        Debug.Log("build text");
        if (quests[questID].activated)
            return;
        if (!questPanelActive)
        {
            questPanelActive = !questPanelActive;
            ShowQuestPanel(questPanelActive);
            StartCoroutine(WaitToBuildText(questID));
        }
        else
        {
            if (textDeleting)
            {
                StartCoroutine(WaitToDeleteText(questID, BuildTextQuest,1f));
            }
            else
            {
                if(showWithDelay != null)
                {
                    StopCoroutine(showWithDelay);
                    showWithDelay = null;
                }
                questsContainer = textWriter.BuildText(quests[questID].questName,true);
                quests[questID].activated = true;
                textWriter.BuildText(quests[questID].questName, questBuildingTime, true);
                questPanelActive = !questPanelActive;
                ShowQuestPanel(false, quests[questID].questName.Length * questBuildingTime + 2f);
            }
            
        }
    }

    public void SetQuestWithDelay(int questID,float delay)
    {
        StartCoroutine(WaitToDeleteText(questID, BuildTextQuest, delay));
    }
    public void DeleteQuest(int questID = -1)
    {
        textDeleting = true;
        if (!questPanelActive)
        {
            questPanelActive = !questPanelActive;
            ShowQuestPanel(questPanelActive);
            StartCoroutine(WaitToDeleteText(questID, DeleteQuest,.7f));
            return;
        }
        else
        {
            if (quests[questID].activated)
            {
                if (showWithDelay != null)
                {
                    StopCoroutine(showWithDelay);
                    showWithDelay = null;
                }
                questsContainer = textWriter.TextDissapear(quests[questID].questName);
                quests[questID].finished = true;
                textWriter.AnimatedLineThrought(quests[questID].questName, questLineThroughtSpeed);
                StartCoroutine(WaitForTextDissapear(quests[questID].questName));

                questPanelActive = !questPanelActive;
                int textLength = quests[questID].questName.Length;
                ShowQuestPanel(false, (textLength * questLineThroughtSpeed + textLength * dissaperTime + 2f));
                return;
            }
            else
            {
                textDeleting = false;
                StartCoroutine(WaitToEndQuest(questID));
                return;
            }
        }
    }

    private void ShowQuestPanel(bool show)
    {
        questAnimator.SetBool(showQuestPanel, show);
    }

    private void ShowQuestPanel(bool show, float time)
    {
        showWithDelay = StartCoroutine(ChangePanelAfter(show,time));

    }

    private IEnumerator WaitToEndQuest(int questID)
    {
        BuildTextQuest(questID);
        yield return new WaitUntil(() => textWriter.TextIsBuilding == false);
        DeleteQuest(questID);
    }

    private IEnumerator ChangePanelAfter(bool show, float time)
    {
        yield return new WaitForSecondsRealtime(time);
        questAnimator.SetBool(showQuestPanel, show);
        questPanelActive = show;
        showWithDelay = null;
    }

    private IEnumerator WaitToBuildText(int questID)
    {
        yield return new WaitForSecondsRealtime(.7f);
        BuildTextQuest(questID);
    }

    private IEnumerator WaitToDeleteText(int questID, Action<int> method, float duration)
    {
        yield return new WaitForSecondsRealtime(duration);
        method.Invoke(questID);
    }

    private IEnumerator WaitForTextDissapear(string text)
    {
        yield return new WaitUntil(() => textWriter.TextIsDestroying == false);
        yield return new WaitForSecondsRealtime(dissaperDelay);
        textWriter.TextDissapear(text,dissaperTime);
        yield return new WaitUntil(() => textWriter.TextIsVanishing == false);
        textDeleting = false;
    }
}
