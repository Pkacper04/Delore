using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Movement : MonoBehaviour
{

    private NavMeshAgent agent;
    private Animator animator;

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
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();    
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0) && !PauseController.GamePaused)
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

        if (path.status == NavMeshPathStatus.PathPartial)
            return false;

        return hasPath;
    }
}
