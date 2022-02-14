using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Movement : MonoBehaviour
{

    [SerializeField] private NavMeshAgent agent;
    private Animator animator;
    [SerializeField] private CapsuleCollider capsuleCollider;

    float speed;

    private void Awake()
    {
        PlayerData data = SaveSystem.LoadPlayer();
        if(data != null)
        {
            transform.position = new Vector3(data.position_x, data.position_y, data.position_z);
        }
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        speed = agent.speed;
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(1) && !PauseController.GamePaused)
        {
            MoveToCursor();
        }
        UpdateAnimations();
    }

    private void MoveToCursor()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        
        Physics.Raycast(ray, out hit);
        if (CanMove(hit.point))
            agent.destination = hit.point;
    }

    private void UpdateAnimations()
    {
        Vector3 velocity = agent.velocity;
        Vector3 localVelocity = transform.InverseTransformDirection(velocity);
        float speed = localVelocity.z;
        animator.SetFloat("Forward", speed / agent.speed);
    }


    private bool CanMove(Vector3 destination)
    {
        NavMeshPath path = new NavMeshPath();
        bool hasPath = NavMesh.CalculatePath(transform.position, destination, NavMesh.AllAreas, path);

        agent.CalculatePath(destination, path);

        Debug.Log(path.status == NavMeshPathStatus.PathPartial);

       if(path.status == NavMeshPathStatus.PathPartial)
            return false;

        return hasPath;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.position.y > 4 && other.transform.position.y < 4.65f)
            Crouch();
    }


    private void Crouch()
    {
        bool crouch = !animator.GetBool("Crouch");

        animator.SetBool("Crouch", crouch);

        float pos_y;
        float height;
        float agentHeight;

        if(crouch)
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

        

        Debug.Log(capsuleCollider.center);

        capsuleCollider.center = new Vector3(capsuleCollider.center.x, pos_y, capsuleCollider.center.z);
        capsuleCollider.height = height;
        agent.height = agentHeight;
    }

    private void OnTriggerExit(Collider other)
    {
        if (animator.GetBool("Crouch"))
            Crouch();
    }
       
    

}
