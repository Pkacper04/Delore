using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public class cutscenes
{
    public List<Sprite> cutsceneImages;
    public string cutsceneDialogue;
}

[System.Serializable]
public class cutscnesList
{
    public List<cutscenes> numberOfCutscenes;
}
