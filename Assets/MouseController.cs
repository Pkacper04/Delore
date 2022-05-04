using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Delore.Player
{
    public class MouseController : MonoBehaviour
    {
        private PlayerStats playerStats;
        private PickupCore core;
        // Start is called before the first frame update

        private void Start()
        {
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

            if (hit.collider.tag != "Pickup")
                return;

            ChestItem item = hit.collider.GetComponent<ChestItem>();

            if (item.Opened == 1)
                return;

            item.OpenChest();

            playerStats.AddItem(item.ItemId, item.ItemName);
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





    }
}
