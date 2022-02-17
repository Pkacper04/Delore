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
            }
            else if (onEnemy)
            {
                ChangeCursros.ActiveCursor();
                onEnemy = false;
            }

        }



    }
}
