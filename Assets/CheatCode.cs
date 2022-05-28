using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Delore.AI;
public class CheatCode : MonoBehaviour
{
    public List<AIDetection> Enemies;

    private bool turnedOn = true;
    private float angle;
    private List<CapsuleCollider> colliders = new List<CapsuleCollider>();

    private void Start()
    {
        angle = Enemies[0].angle;
        foreach (AIDetection enemy in Enemies)
        {
            colliders.Add(enemy.transform.GetComponent<CapsuleCollider>());
        }
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            turnedOn = !turnedOn;
            for(int i=0;i<Enemies.Count;i++)
            {
                if (turnedOn)
                {
                    Enemies[i].angle = angle;
                    colliders[i].enabled = true;
                }
                else
                {
                    Enemies[i].angle = 0;
                    colliders[i].enabled = false;
                }
            }
        }
    }

}
