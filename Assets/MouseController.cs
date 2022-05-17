using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Delore.Player
{
    public class MouseController : MonoBehaviour
    {
        private PlayerStats playerStats;
        private PickupCore core;
        private Movement playerMovement;

        public bool MovingToChest { get; set; }
        // Start is called before the first frame update

        private void Start()
        {
            playerMovement = GetComponent<Movement>();
            playerStats = GetComponent<PlayerStats>();
            core = GetComponent<PickupCore>();
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
            CheckForAttack();

            if (Input.GetMouseButtonDown(0))
                PickUpItem();
        }

        private void PickUpItem()
        {
            RaycastHit hit = GetMousePoint();

            if (hit.collider == null || hit.collider.tag != "Pickup")
                return;

            ChestItem item = hit.collider.GetComponent<ChestItem>();

            if (item.Opened == 1)
                return;

            NavMeshPath path = new NavMeshPath();

            NavMeshHit navHit;
            NavMesh.SamplePosition(hit.transform.position, out navHit, 20f, -1);

            MovingToChest = true;
            playerMovement.PickUpMove(navHit.position);

            item.OpenChest();

            playerStats.AddItem(item.ItemId, item.ItemName);
            /*object[] parms = new object[2] { navHit.position, item };
            StartCoroutine("WaitToOpenChest",parms);*/

        }

        public RaycastHit GetMousePoint()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            Physics.Raycast(ray, out hit);
            return hit;
        }

        public void CheckForAttack()
        {
            RaycastHit hit = GetMousePoint();
            if (hit.collider == null)
                return;

            if(hit.collider.tag == "Pickup")
            {
                Debug.Log("na przedmiocie");
            }

        }

        private IEnumerator WaitToOpenChest(object[] parms)
        {

            Vector3 targetPosition = (Vector3)parms[0];

            Debug.Log("test");
            yield return new WaitUntil(() => MovingToChest == false);
            Debug.Log(playerMovement.agent.destination);
            Debug.Log((Vector3)parms[0]);
            if (playerMovement.agent.destination == (Vector3)parms[0])
            {
                ChestItem item = (ChestItem)parms[1];

                item.OpenChest();

                playerStats.AddItem(item.ItemId, item.ItemName);
            }
        }




    }
}
