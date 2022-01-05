using UnityEngine;

public class CameraController : MonoBehaviour
{

    [SerializeField] private float playerOffset = 7f;

    private Transform mainCamera;
    private Transform player;

    private Animator animator;

    internal int camNumber = 1;

    void Start()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").transform;
    }

    void Update()
    {
        if (!animator.IsInTransition(0))
        {
            if (CalculatePlayerOffset() >= playerOffset)
            {
                camNumber++;
                animator.SetInteger("CameraNumber", camNumber);

            }
            else if (CalculatePlayerOffset() <= -playerOffset)
            {
                camNumber--;
                animator.SetInteger("CameraNumber", camNumber);
            }
        }
    }

    private float CalculatePlayerOffset() => player.position.x - mainCamera.position.x;


    public void ChangeCamera(bool right)
    {
        if (!animator.IsInTransition(0))
        {
            camNumber = right ? camNumber + 1 : camNumber - 1;
            animator.SetInteger("CameraNumber", camNumber);
        }
    }

}
