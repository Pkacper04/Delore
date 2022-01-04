using UnityEngine;

public class CameraController : MonoBehaviour
{

    [SerializeField] private Transform[] playerCameras;

    private Animator animator;
    private int camNumber = 1;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        if (playerCameras[camNumber - 1].rotation.y >= 0.40f)
        {
            camNumber++;
            animator.SetInteger("CameraNumber", camNumber);
        }
        if (playerCameras[camNumber - 1].rotation.y <= -0.40f)
        {
            camNumber--;
            animator.SetInteger("CameraNumber", camNumber);
        }
    }
}
