using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text;

public class TextWriter : MonoBehaviour
{
    [SerializeField]
    private TMP_Text dialogue;

    public bool TextIsBuilding { get; private set; }
    public bool TextIsDestroying { get; private set; }
    public bool TextIsVanishing { get; private set; }
    public void BuildText(string text, float displayTime, bool onEndIndent = false)
    {
        TextIsBuilding = true;
        StartCoroutine(displayText(text,displayTime,onEndIndent));
    }

    public string BuildText(string text, bool OnEndIndent = false)
    {
        if (OnEndIndent)
            return dialogue.text + text + "\n";
        return dialogue.text + text;
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

    public void LineThrought(string textToChange)
    {
        int startPostion = dialogue.text.IndexOf(textToChange);
        int endPostion = startPostion + textToChange.Length;
        dialogue.text = dialogue.text.Insert(startPostion, "<s>");
        dialogue.text = dialogue.text.Insert(endPostion+3, "</s>");
    }


    public void AnimatedLineThrought(string textToChange, float speed)
    {
        TextIsDestroying = true;
        int startPosition = dialogue.text.IndexOf(textToChange);
        dialogue.text = dialogue.text.Insert(startPosition, "<s>");

        StartCoroutine(AnimateLine(startPosition, textToChange.Length, speed));
    }


    public string TextDissapear(string textToDelete)
    {
        int startPosition = dialogue.text.IndexOf(textToDelete);
        string endtext;
        endtext = dialogue.text.Remove(startPosition, textToDelete.Length);
        Debug.Log("end text: "+endtext);
        endtext = endtext.Remove(startPosition-1,1);
        Debug.Log("after end text: " + endtext);
        return endtext;
    }
    public void TextDissapear(string textToDelete,float speed)
    {
        StartCoroutine(AnimatedDissapear(textToDelete,speed));

    }
    private IEnumerator displayText(string text, float displayTime, bool onEndIndent)
    {
        for(int i=0;i<text.Length;i++)
        {
            dialogue.text = string.Concat(dialogue.text, text[i]);
            yield return new WaitForSeconds(displayTime);
        }
        if (onEndIndent)
            dialogue.text += "\n";
        TextIsBuilding = false;
    }

    private IEnumerator AnimateLine(int startPosition, int length, float speed)
    {
        for(int i = 1; i <= length; i++)
        {
            dialogue.text = dialogue.text.Insert(startPosition+i+3, "</s>");
            yield return new WaitForSecondsRealtime(speed);
            dialogue.text = dialogue.text.Remove(startPosition + i + 3,4);
        }
        dialogue.text = dialogue.text.Insert(startPosition + length + 3, "</s>");
        TextIsDestroying = false;
    }

    private IEnumerator AnimatedDissapear(string text, float speed)
    {
        TextIsVanishing = true;
        Debug.Log("animacja");
        int startPosition = dialogue.text.IndexOf(text);
        int endPosition = startPosition + text.Length - 1;
        Debug.Log("start postion: "+startPosition);
        for (int i=0;i<text.Length;i++)
        {
            dialogue.text = dialogue.text.Remove(endPosition - i , 1);
            yield return new WaitForSecondsRealtime(speed);
        }
        dialogue.text = dialogue.text.Remove(startPosition-4,4);
        dialogue.text = dialogue.text.Remove(startPosition - 4, 4);
        TextIsVanishing = false;
    }
}

