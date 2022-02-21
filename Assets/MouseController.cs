using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Delore.Player
{
    public class MouseController : MonoBehaviour
    {
        private bool onEnemy = false;
        // Start is called before the first frame update


        private void Update()
        {
            CheckForAttack();

            if (Input.GetMouseButtonDown(0))
                PickUpItem();
        }

        private void PickUpItem()
        {
            RaycastHit hit = GetMousePoint();

            if (hit.collider.tag != "Pickup")
                return;

            Debug.Log("podniosles przedmiot: "+hit.collider.gameObject.name);
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


            if (hit.collider.tag == "Enemy")
            {
                ChangeCursros.AttackCursor();
                onEnemy = true;
                return;
            }
            else if (onEnemy)
            {
                ChangeCursros.ActiveCursor();
                onEnemy = false;
                return;
            }

            if(hit.collider.tag == "Pickup")
            {
                Debug.Log("na przedmiocie");
            }



        }





    }
}
