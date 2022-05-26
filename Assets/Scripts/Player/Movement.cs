using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using NaughtyAttributes;
using Delore.AI;

namespace Delore.Player
{
    public class Movement : MonoBehaviour
    {

        public NavMeshAgent agent;
        public bool dead = false;
        [SerializeField] private CapsuleCollider capsuleCollider;
        [SerializeField] float slowingSpeed = 1.5f;
        [SerializeField] AudioSource playerSFX;
        [SerializeField] AudioClip steps;
        [SerializeField] AudioClip death;
        [SerializeField] float duration;
        [SerializeField] CameraController controller;
        private float stepDelay = .4f;
        private float conDelay;

        public event Action Triggered;
        [SerializeField]
        private Animator animator;
        public float speed;
        private MouseController mouseController;
        private PlayerStats playerStats;
        private Rigidbody rigidbody;
        private float cooldown = 0;

        [AnimatorParam("animator")]
        public string deathParam;

        private void Awake()
        {
            conDelay = stepDelay;
            PlayerData data = SaveSystem.LoadPlayer();
            if (data != null)
            {
                transform.position = new Vector3(data.position_x, data.position_y, data.position_z);
            }
        }

        void Start()
        {
            speed = agent.speed;
            mouseController = GetComponent<MouseController>();
            playerStats = GetComponent<PlayerStats>();
            rigidbody = GetComponent<Rigidbody>();
        }

        void Update()
        {
            if (dead)
            {
                rigidbody.velocity = new Vector3(0, -.2f, 0);
                rigidbody.angularVelocity = new Vector3(0, -.2f, 0);
                return;
            }

            rigidbody.velocity = new Vector3(0,0,0);
            rigidbody.angularVelocity = new Vector3(0, 0, 0);
            if (Mathf.Abs(rigidbody.velocity.x) > 1 || Mathf.Abs(rigidbody.velocity.y) > 1)
                rigidbody.velocity = new Vector3(0,0,0);
            if (Input.GetMouseButton(1) && !PauseController.GamePaused)
            {
                MoveToCursor();
            }

            UpdateAnimations();
        }

        
        public void PickUpMove(Vector3 targetPosition)
        {
            agent.destination = targetPosition;
        }

        private void MoveToCursor()
        {
            if (agent.isStopped)
                agent.isStopped = false;
            RaycastHit hit = mouseController.GetMousePoint();

            if (CanMove(hit.point))
            {
                agent.destination = hit.point;
                mouseController.MovingToChest = false;
            }
            else
                ChangeCursros.DeclineCursor();
        }

        private void UpdateAnimations()
        {



            Vector3 velocity = agent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            speed = localVelocity.z;
            speed = speed / agent.speed;
            animator.SetFloat("Forward", speed);
            stepDelay = conDelay * ((conDelay*2) / (conDelay + (speed * conDelay)));
            if (speed / agent.speed > 0)
            {
                PlayStepSound();
            }
        }


        private bool CanMove(Vector3 destination)
        {
            NavMeshPath path = new NavMeshPath();
            bool hasPath = NavMesh.CalculatePath(transform.position, destination, NavMesh.AllAreas, path);

            agent.CalculatePath(destination, path);

            if (path.status == NavMeshPathStatus.PathPartial)
            {
                ChangeCursros.DeclineCursor();
                return false;
            }
            return hasPath;
        }


/*        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.position.y > 4 && other.transform.position.y < 4.65f)
                Crouch();
        }*/


        /*private void Crouch()
        {
            crouch = !animator.GetBool("Crouch");

            animator.SetBool("Crouch", crouch);

            float pos_y;
            float height;
            float agentHeight;

            if (crouch)
            {
                pos_y = 0.44f;
                height = 0.88f;
                agentHeight = 0.8f;
                agent.speed = 1.5f;
            }
            else
            {
                pos_y = 0.79f;
                height = 1.58f;
                agentHeight = 1.5f;
                agent.speed = speed;
            }

            capsuleCollider.center = new Vector3(capsuleCollider.center.x, pos_y, capsuleCollider.center.z);
            capsuleCollider.height = height;
            agent.height = agentHeight;
        }*/

        private void PlayStepSound()
        {

            if(cooldown < Time.time)
            {
                playerSFX.PlayOneShot(steps);
                cooldown = Time.time + stepDelay;
            }
        }

/*        private void OnTriggerExit(Collider other)
        {
            if (animator.GetBool("Crouch"))
                if (other.transform.position.y > 4 && other.transform.position.y < 4.65f)
                    Crouch();
        }*/




        

        private void OnCollisionEnter(Collision collision)
        {
            if(collision.transform.tag == "Enemy")
            {
                collision.transform.GetComponent<AIMovement>().waiting = false;
                dead = true;
                playerSFX.Stop();
                playerSFX.clip = death;
                playerSFX.Play();
                StartCoroutine(DeathAnimation());            
            }

            /*if (collision.transform.tag == "Pickup")
            {
                playerStats.AddItem(collision.gameObject);
                collision.gameObject.SetActive(false);
            }*/


        }

        private IEnumerator DeathAnimation()
        {
            animator.SetBool(deathParam,true);
            controller.DisableLooking();
            rigidbody.isKinematic = true;
            capsuleCollider.enabled = false;
            agent.enabled = false;

            yield return new WaitForSeconds(3f);
            
            yield return new WaitForSeconds(2f);
            
            Triggered?.Invoke();
        }

    }
}
