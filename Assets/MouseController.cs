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
        private UIManager manager;
        [SerializeField]
        private Animator animator;
        [AnimatorParam("animator")]
        public string startParam;
        [AnimatorParam("animator")]
        public string blockStartParam;

        [Scene]
        public string prologScene;
        [Scene]
        public string afterlifeScene;

        public bool MovingToChest { get; set; }

        private Coroutine coroutine = null;
        // Start is called before the first frame update

        private void Start()
        {
            effect.Stop();
            playerMovement = GetComponent<Movement>();
            playerStats = GetComponent<PlayerStats>();
            core = GetComponent<PickupCore>();
            
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
                animator.SetBool(blockStartParam,true);
            }
        }

        private void Update()
        {
            #region DO WYRZUCENIA
            if (Input.GetKeyDown(KeyCode.M))
            {
                core.SaveChests();
                SaveSystem.SavePlayer(gameObject);
            }

            #endregion DO WYRZUCENIA

            if (PauseController.GamePaused)
                return;

            if (Input.GetMouseButtonDown(0))
                PickUpItem();
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

                NavMeshPath path = new NavMeshPath();

                NavMeshHit navHit;
                NavMesh.SamplePosition(hit.transform.position, out navHit, 20f, -1);

                MovingToChest = true;
                playerMovement.PickUpMove(navHit.position);

                coroutine = StartCoroutine(WaitToOpenChest(navHit.position, item));
            }
            else if(hit.collider.tag == "Door")
            {
                LockedDoor door = hit.collider.GetComponent<LockedDoor>();

                playerMovement.PickUpMove(hit.transform.position);

                coroutine = StartCoroutine(WaitToOpenDoor(hit.transform.position, door));
                
            }

        }

        public RaycastHit GetMousePoint()
        {
            if(coroutine != null)
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
            
            yield return new WaitUntil(() => Vector3.Distance(transform.position, position) < .3f);
            playerMovement.agent.isStopped = true;
            item.OpenChest();
            playerStats.AddItem(item.ItemId, item.ItemName);
            coroutine = null;
            
        }

        private IEnumerator WaitToOpenDoor(Vector3 position, LockedDoor door)
        {
            yield return new WaitUntil(() => Vector3.Distance(transform.position, position) < door.stoppingDistance);
            playerMovement.agent.isStopped = true;
            door.OpenDoor();
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
            foreach(SkinnedMeshRenderer renderer in playerRenderers)
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
            StartCoroutine(manager.SmoothEnding());

        }

        private IEnumerator WaitForEndAnimation()
        {
            yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("Movement"));
            PauseController.GamePaused = false;
            PauseController.BlockPauseMenu = false;
        }
    }
}
