using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathMoving : MonoBehaviour
{
    public float divider;
    void Update()
    {
        Vector3 _newPosition = transform.position;
        _newPosition.y += (Mathf.Sin(Time.time) * Time.deltaTime)/divider;
        transform.position = _newPosition;
    }
}
