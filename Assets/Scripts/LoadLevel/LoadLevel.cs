using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
public class LoadLevel : MonoBehaviour
{
    private bool loaded = false;
    [SerializeField] private Image loadingBar;
    [SerializeField] private TMP_Text loadingText;
    void Start()
    {
        loadingBar.fillAmount = 0;
        loadingText.enabled = false;
        StartCoroutine(LoadScene());
    }


    IEnumerator LoadScene()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync("Prolog");
        operation.allowSceneActivation = false;

        while (!loaded)
        {
            loadingBar.fillAmount = Mathf.Clamp01(operation.progress/.9f);

            if(operation.progress == .9f)
                loaded = true;
            yield return null;
        }
        while(loaded)
        {
            loadingBar.color = new Color(255, 255, 255, 0);
            loadingText.enabled = true;

            if (Input.anyKeyDown)
                operation.allowSceneActivation = true;
            yield return null;
        }


        

    }
}
