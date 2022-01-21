using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class LoadLevel : MonoBehaviour
{
    private bool loaded = false;
    private Image loadingBar;
    void Start()
    {
        loadingBar = GetComponent<Image>();
        loadingBar.fillAmount = 0;
        
        StartCoroutine(LoadScene());
    }


    IEnumerator LoadScene()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync("Prolog");
        operation.allowSceneActivation = false;

        while (!loaded)
        {
            loadingBar.fillAmount = Mathf.Clamp01(operation.progress/.9f);
            Debug.Log(loadingBar.fillAmount);
            if(operation.progress == .9f)
                loaded = true;
            yield return null;
        }
        while(loaded)
        {
            if (Input.anyKeyDown)
                operation.allowSceneActivation = true;
            yield return null;
        }


        

    }
}
