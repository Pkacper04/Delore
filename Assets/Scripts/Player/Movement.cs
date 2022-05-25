using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using NaughtyAttributes;


namespace Delore.Player
{
    public class Movement : MonoBehaviour
    {

        public NavMeshAgent agent;
        [SerializeField] private CapsuleCollider capsuleCollider;
        [SerializeField] float slowingSpeed = 1.5f;
        [SerializeField] AudioSource playerSFX;
        [SerializeField] AudioClip steps;
        private float stepDelay = .4f;
        private float conDelay;

        public event Action Triggered;

        public bool crouch;

        private Animator animator;
        private float speed;
        private MouseController mouseController;
        private PlayerStats playerStats;
        private Rigidbody rigidbody;
        private Vector3 lastPosition;
        private bool isMoving = false;
        private float cooldown = 0;

        private void Awake()
        {
            conDelay = stepDelay;
            lastPosition = transform.position;
            PlayerData data = SaveSystem.LoadPlayer();
            if (data != null)
            {
                transform.position = new Vector3(data.position_x, data.position_y, data.position_z);
            }
        }

        void Start()
        {
            animator = GetComponent<Animator>();
            speed = agent.speed;
            mouseController = GetComponent<MouseController>();
            playerStats = GetComponent<PlayerStats>();
            rigidbody = GetComponent<Rigidbody>();
            isMoving = true;
        }

        void Update()
        {
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
            float speed = localVelocity.z;
            animator.SetFloat("Forward", speed / agent.speed);
            stepDelay = conDelay * ((conDelay*2) / (conDelay + ((speed / agent.speed) * conDelay)));
            if (speed / agent.speed > 0)
            {
                PlayStepSound();
            }
            lastPosition = transform.position;
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


        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.position.y > 4 && other.transform.position.y < 4.65f)
                Crouch();
        }


        private void Crouch()
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
        }

        private void PlayStepSound()
        {

            if(cooldown < Time.time)
            {
                playerSFX.PlayOneShot(steps);
                cooldown = Time.time + stepDelay;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (animator.GetBool("Crouch"))
                if (other.transform.position.y > 4 && other.transform.position.y < 4.65f)
                    Crouch();
        }




        

        private void OnCollisionEnter(Collision collision)
        {

            if(collision.transform.tag == "Enemy")
            {
                Triggered?.Invoke();
            }

            /*if (collision.transform.tag == "Pickup")
            {
                playerStats.AddItem(collision.gameObject);
                collision.gameObject.SetActive(false);
            }*/


        }


    }
}
