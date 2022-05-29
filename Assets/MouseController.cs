using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using NaughtyAttributes;
using UnityEngine.VFX;
using UnityEngine.SceneManagement;
namespace Delore.Player
{
    public class MouseController : MonoBehaviour
    {
        private PlayerStats playerStats;
        private PickupCore core;
        private Movement playerMovement;
        [SerializeField]
        private float appearDelay = 2f;
        [SerializeField]
        private float duration = 2f;
        [SerializeField]
        private VisualEffect effect;
        [SerializeField]
        private ParticleSystem light;
        [SerializeField]
        private SkinnedMeshRenderer[] playerRenderers;
        [SerializeField]
        private MeshRenderer[] playerMeshes;
        [SerializeField]
        private AudioTrigger audioSFX;
        [SerializeField]
        private AudioClip appearAudio;

        [SerializeField]
        private AudioSource audioUI;
        [SerializeField]
        private AudioClip SaveAudio;

        [SerializeField]
        private UIManager manager;
        [SerializeField]
        private Animator animator;
        [AnimatorParam("animator")]
        public string startParam;
        [AnimatorParam("animator")]
        public string blockStartParam;


        [SerializeField]
        private QuestSystem quests;
        [SerializeField]
        private NotebookScript notebook;


        [SerializeField]
        private AudioSource[] soundsToFade;

        [SerializeField]
        private float fadeDuration;

        [Scene]
        public string prologScene;
        [Scene]
        public string afterlifeScene;

        public bool MovingToChest { get; set; }

        private Coroutine coroutine = null;
        private ChangeColor objectScript;
        // Start is called before the first frame update

        private void Start()
        {
            effect.Stop();
            playerMovement = GetComponent<Movement>();
            playerStats = GetComponent<PlayerStats>();
            core = GetComponent<PickupCore>();
            PlayerData data = SaveSystem.LoadPlayer();
            if (data != null)
            {
                animator.SetBool(blockStartParam, true);
                return;
            }
            if (SceneManager.GetActiveScene().name == prologScene)
            {
                foreach (var item in playerRenderers)
                {
                    item.enabled = false;
                }
                foreach (var item in playerMeshes)
                {
                    item.enabled = false;
                }
                PauseController.GamePaused = true;
                PauseController.BlockPauseMenu = true;
                StartCoroutine(PlayerAppear());
            }
            else
            {
                animator.SetBool(blockStartParam, true);
                StartCoroutine(DelayAfterlife());
            }
        }

        private void Update()
        {
            #region DO WYRZUCENIA
            if (Input.GetKeyDown(KeyCode.M))
            {
                SaveSystem.SavePlayer(gameObject);
            }

            #endregion DO WYRZUCENIA

            if (PauseController.GamePaused)
                return;

            if (Input.GetMouseButtonDown(0))
                PickUpItem();
            else
            {
                MouseHover();
            }
        }


        private void MouseHover()
        {
            if (SceneManager.GetActiveScene().name == afterlifeScene)
                return;
            RaycastHit hit = GetMousePoint(false);

            Debug.Log(hit);

            if (hit.transform.tag == "Pickup" || hit.transform.tag == "Door" || hit.transform.tag == "CheckPoint")
            {
                ChangeObjColor(hit);
                ChangeCursros.PickUpCursor();
            }
            else
            {
                if (objectScript == null)
                    return;
                objectScript.OutHover();
                objectScript = null;
                ChangeCursros.ActiveCursor();

            }


        }

        private void PickUpItem()
        {
            RaycastHit hit = GetMousePoint();

            if (hit.collider == null)
                return;
            if (hit.collider.tag == "Pickup")
            {
                ChestItem item = hit.collider.GetComponent<ChestItem>();

                if (item.Opened == 1)
                    return;

                MovingToChest = true;
                playerMovement.PickUpMove(hit.transform.position);
                coroutine = StartCoroutine(WaitToOpenChest(hit.transform.position, item));
            }
            else if (hit.collider.tag == "Door")
            {
                LockedDoor door = hit.collider.GetComponent<LockedDoor>();

                if (!door.Locked)
                    return;

                playerMovement.PickUpMove(hit.transform.position);

                coroutine = StartCoroutine(WaitToOpenDoor(hit.transform.position, door));

            }
            else if(hit.collider.tag == "CheckPoint")
            {
                playerMovement.PickUpMove(hit.transform.position);
                coroutine = StartCoroutine(WaitToSaveGame(hit.transform.position));
            }

        }

        public RaycastHit GetMousePoint(bool stopCoroutine = false)
        {
            if (stopCoroutine && coroutine != null)
            {
                StopCoroutine(coroutine);
                coroutine = null;
            }
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            Physics.Raycast(ray, out hit);
            return hit;
        }

        private IEnumerator WaitToOpenChest(Vector3 position, ChestItem item)
        {
            yield return new WaitUntil(() => CheckChest(position));
            playerMovement.agent.isStopped = true;
            item.OpenChest();
            playerStats.AddItem(item.ItemId, item.ItemName);
            coroutine = null;

        }

        private bool CheckChest(Vector3 position)
        {
            if (Mathf.Abs(transform.position.x - position.x) > 1f)
                return false;
            else if (Mathf.Abs(transform.position.y - position.y) > .3f)
                return false;
            else if (Mathf.Abs(transform.position.z - position.z) > 1f)
                return false;
            else
                return true;
        }

        private IEnumerator WaitToOpenDoor(Vector3 position, LockedDoor door)
        {
            yield return new WaitUntil(() => Vector3.Distance(transform.position, position) < door.stoppingDistance);
            playerMovement.agent.isStopped = true;
            door.OpenDoor();
            coroutine = null;
        }


        private IEnumerator WaitToSaveGame(Vector3 position)
        {
            yield return new WaitUntil(() => Vector3.Distance(transform.position, position) < 2f);
            audioUI.PlayOneShot(SaveAudio);
            playerMovement.agent.isStopped = true;
            SaveSystem.SavePlayer(gameObject);
            coroutine = null;
        }
        IEnumerator PlayerAppear()
        {
            yield return new WaitForSeconds(appearDelay);
            effect.Play();
            light.Play();
            yield return new WaitForSeconds(.5f);
            audioSFX.playOneTime(appearAudio);
            yield return new WaitForSeconds(1f);
            animator.SetBool(startParam, true);
            foreach (SkinnedMeshRenderer renderer in playerRenderers)
            {
                renderer.enabled = true;
            }
            foreach (var item in playerMeshes)
            {
                item.enabled = true;
            }
            effect.Stop();
            light.Stop();


            StartCoroutine(WaitForEndAnimation());
        }

        public IEnumerator StartProlog()
        {
            yield return new WaitForSeconds(appearDelay);
            effect.Play();
            light.Play();
            yield return new WaitForSeconds(1f);
            foreach (SkinnedMeshRenderer renderer in playerRenderers)
            {
                renderer.enabled = false;
            }
            foreach (var item in playerMeshes)
            {
                item.enabled = false;
            }
            effect.Stop();
            light.Stop();
            yield return new WaitForSeconds(2);
            StartCoroutine(SoundFading.FadeInCoroutine(soundsToFade,fadeDuration,1,0));
            StartCoroutine(manager.SmoothEnding());

        }

        private IEnumerator WaitForEndAnimation()
        {
            yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("Movement"));
            PauseController.GamePaused = false;
            PauseController.BlockPauseMenu = false;
            quests.BuildTextQuest(0);
            notebook.UpdateNotebook("I have awakend", "I don't know what happened, iI died but im still alive? Wasn’t that just a dream? This place feels so familiar, yet so strange.");
        }

        private IEnumerator DelayAfterlife()
        {
            yield return new WaitForSecondsRealtime(4f);
            quests.BuildTextQuest(0);
        }

        private void ChangeObjColor(RaycastHit hit)
        {
            if (objectScript != null && objectScript.transform == hit.transform)
                return;

            if (objectScript != null)
                objectScript.OutHover();

            objectScript = hit.transform.GetComponent<ChangeColor>();
            objectScript.OnHover();
        }

    }

    

}
