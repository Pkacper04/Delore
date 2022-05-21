using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using UnityEngine.VFX;
public class TriggerDialogue : MonoBehaviour
{
    [SerializeField]
    VIDE_Assign assign;

    [SerializeField]
    UIManager uiManager;

    [Tag]
    public string playerTag;

    [SerializeField]
    public VisualEffect effect;

    [SerializeField]
    private MeshRenderer meshRenderer;

    [SerializeField]
    private float duration = 5;

    [SerializeField]
    private AudioTrigger sfxTrigger;

    [SerializeField]
    private AudioClip deathVoice;

    [SerializeField]
    private AudioClip portalAudio;

    [SerializeField]
    private AudioClip deathAmbient;


    private void Start()
    {
        meshRenderer.material.SetFloat("DisolveValue_", 1);
        effect.Stop();
    }



    private bool visited = false;
    private void OnTriggerEnter(Collider other)
    {
        if (visited)
            return;
        if(other.tag == playerTag)
        {
            PauseController.BlockPauseMenu = true;
            visited = true;
            StartCoroutine(WaitForDialogue());
        }
    }

    private IEnumerator WaitForDialogue()
    {
        PauseController.GamePaused = true;
        effect.Play();
        sfxTrigger.playOneTime(portalAudio);
        sfxTrigger.PlayAmbientLoop(deathAmbient);
        yield return new WaitForSeconds(2);

        float disolve = 1;

        while (disolve > 0f)
        {
            disolve -= Time.deltaTime / duration;
            disolve = Mathf.Clamp01(disolve);
            meshRenderer.material.SetFloat("DisolveValue_", disolve);
            yield return null;
        }
        
        yield return new WaitForSeconds(1);
        uiManager.ActivateDialogue(assign);
        sfxTrigger.PlayLoop(deathVoice);
    }
}
