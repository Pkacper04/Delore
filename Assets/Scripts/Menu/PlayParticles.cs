using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayParticles : MonoBehaviour
{

    private Animator animator;
    [SerializeField] ParticleSystem particles;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!particles.isPlaying && animator.GetCurrentAnimatorClipInfoCount(0) == 0)
            particles.Play();
    }
}
