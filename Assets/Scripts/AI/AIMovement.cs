using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using NaughtyAttributes;
using Delore.Player;

namespace Delore.AI
{
    public class AIMovement : MonoBehaviour
    {
        NavMeshAgent agent;
        GameObject player;
        AIDetection detection;
        float speed;
        int patrolPointsNumber;
        int currentPoint = 0;

        float timer;
        bool isChasing = false;
        public bool waiting = false;
        Movement playerMovement;
       

        public AIType aiType;



        [SerializeField] List<Vector3> patrolPoints = new List<Vector3>();
        [SerializeField] List<float> patrolRotations= new List<float>();
        [SerializeField] float rotationSpeed = 10f;
        [SerializeField] float chasingSpeed = 3f;
        [SerializeField] float patrolSpeed = 2f;
        [SerializeField] Animator animator;

        [AnimatorParam("animator")]
        public string speedAnimator;

        [SerializeField]
        float timeOfChasing = 3f;

        [SerializeField,MinMaxSlider(1f,30f)]
        private Vector2 patrolTime = new Vector2(1f,1f);




        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            player = GameObject.FindGameObjectWithTag("Player");
            detection = GetComponent<AIDetection>();
            speed = agent.speed;
            patrolPointsNumber = patrolPoints.Count;
            playerMovement = player.GetComponent<Movement>();
        }

        void Update()
        {
            if(playerMovement.dead && !waiting)
            {
                Debug.Log("dziala");
                agent.isStopped = true;
                isChasing = false;
                StartCoroutine(Patrol());
                UpdateAnimation();
                return;
            }
            else if (playerMovement.dead)
            {
                isChasing = false;
                UpdateAnimation();
                return;
            }
            

            if (detection.FieldOfView() || isChasing)
                Mover();
                
            if (!isChasing && !waiting)
                StartCoroutine(Patrol());

            UpdateAnimation();
        }

        private void Mover()
        {
            agent.speed = chasingSpeed;
            agent.updateRotation = true;
            if (detection.FieldOfView())
            {
                agent.isStopped = false;
                timer = timeOfChasing;
                agent.destination = AIType.Aggressive == aiType ? player.transform.position : RunningDirection();
                isChasing = true;
            }
            else if (timer > 0)
            {
                agent.destination = AIType.Aggressive == aiType ? player.transform.position : RunningDirection();
            }
            else
            {
                if (isChasing)
                    agent.isStopped = true;
                isChasing = false;
            }
            timer -= Time.deltaTime;
        }


        private void UpdateAnimation()
        {
            Vector3 velocity = agent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            float speed = localVelocity.z;
            float endSpeed = speed / agent.speed;
            if (!isChasing)
                endSpeed /= 2;
            animator.SetFloat(speedAnimator, endSpeed);
        }
        private IEnumerator Patrol()
        {
            agent.speed = patrolSpeed;
            agent.updateRotation = true;
            waiting = true;
            if (agent.isStopped)
                agent.isStopped = false;

            agent.destination = patrolPoints[currentPoint];
            Debug.Log("new destination: "+agent.destination);

            yield return new WaitUntil(() => Vector3.Distance(transform.position,patrolPoints[currentPoint]) <= 1f);


            agent.updateRotation = false;

            if (patrolRotations[currentPoint] > 0)
            {
                while (Mathf.Abs(transform.rotation.eulerAngles.y - patrolRotations[currentPoint]) >= .1f)
                {
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(new Vector3(0,patrolRotations[currentPoint],0)), rotationSpeed * Time.deltaTime);
                    yield return null;
                }
            }


            
            currentPoint++;
            if (currentPoint == patrolPointsNumber)
            {
                currentPoint = 0;
            }

            

            yield return new WaitForSeconds(Random.Range(patrolTime.x,patrolTime.y));
            waiting = false;
        }

        private Vector3 RunningDirection()
        {
            float pos_x = player.transform.position.x;
            float pos_y = player.transform.position.y;
            float pos_z = player.transform.position.z;
            float runningDistance = detection.radius * 2f;
            if (transform.position.x > pos_x)
                return pos_z > transform.position.z ? new Vector3(pos_x + runningDistance, pos_y, pos_z - runningDistance) : new Vector3(pos_x + runningDistance, pos_y, pos_z + runningDistance);
            else
                return pos_z > transform.position.z ? new Vector3(pos_x - runningDistance, pos_y, pos_z - runningDistance) : new Vector3(pos_x - runningDistance, pos_y, pos_z + runningDistance);
        }

    }

    public enum AIType
    {
        Aggressive,
        Passive
    }

}
