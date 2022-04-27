using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Delore.AI
{
    public class AIDetection : MonoBehaviour
    {
        private LayerMask playerMask;
        private LayerMask obstructionMask;

        internal bool playerInRange = false;




        [Range(0, 50)]
        public float radius = 10;

        [Range(0, 360)]
        public float angle = 120;




        void Start()
        {
            playerMask = LayerMask.GetMask("Player");
            obstructionMask = LayerMask.GetMask("Default");

            SphereCollider playerInDistance = gameObject.AddComponent<SphereCollider>();
            playerInDistance.isTrigger = true;

        }

        #region FieldOfView
        public bool FieldOfView()
        {
            Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, playerMask);
            if (rangeChecks.Length != 0)
            {
                Transform target = rangeChecks[0].transform;
                Vector3 directionToTarget = (target.position - transform.position).normalized;
                if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
                {
                    float distanceToTarget = Vector3.Distance(target.position, transform.position);
                    if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                        return true;

                    return false;
                }
                return false;
            }
            return false;
        }
        #endregion

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
                playerInRange = true;

        }

        private void OnTriggerExit(Collider other)
        {
            if (other.tag == "Player")
                playerInRange = false;
        }


        #region DrawHearingDistance
        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
        }
        #endregion

    }
}