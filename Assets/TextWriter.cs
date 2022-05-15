using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextWriter : MonoBehaviour
{
    [SerializeField]
    private TMP_Text dialogue;

    public bool TextIsBuilding { get; private set; }
    public void BuildText(string text, float displayTime)
    {
        TextIsBuilding = true;
        StartCoroutine(displayText(text,displayTime));
    }

    public void ClearDialogue()
    {
        dialogue.text = "";
    }

    public void StopBuildingText()
    {
        StopAllCoroutines();
        TextIsBuilding = false;
    }
    private IEnumerator displayText(string text, float displayTime)
    {
        for(int i=0;i<text.Length;i++)
        {
            dialogue.text = string.Concat(dialogue.text, text[i]);
            yield return new WaitForSeconds(displayTime);
        }
        TextIsBuilding = false;
    }
}

