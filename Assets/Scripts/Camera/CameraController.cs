using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{

    [SerializeField] internal float playerOffset = 7f;



    private CinemachineStateDrivenCamera stateCamera;
    private Transform mainCamera;
    private Transform player;

    private Animator animator;

    internal int camNumber = 1;



    private void Awake()
    {
        animator = GetComponent<Animator>();
        stateCamera = GetComponent<CinemachineStateDrivenCamera>();
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").transform;
        player = GameObject.FindGameObjectWithTag("Player").transform;


        PlayerData data = SaveSystem.LoadPlayer();
        if (data != null)
        {
            camNumber = data.camNumber;

            CinemachineVirtualCameraBase tmpCamera = stateCamera.ChildCameras[0];
            stateCamera.ChildCameras[0] = stateCamera.ChildCameras[camNumber - 1];
            stateCamera.ChildCameras[camNumber - 1] = tmpCamera;

            mainCamera.position = stateCamera.ChildCameras[0].gameObject.transform.position;

            animator.SetInteger("CameraNumber", camNumber);
        }

    }


    void Update()
    {
       
       // Debug.Log(animator.IsInTransition(0));
        if (!animator.IsInTransition(0))
        {
            if (CalculatePlayerOffset() >= playerOffset)
            {
                camNumber++;
            }
            else if (CalculatePlayerOffset() <= -playerOffset)
            {
                camNumber--;
            }
            animator.SetInteger("CameraNumber", camNumber);
        }
    }

    internal float CalculatePlayerOffset() => player.position.x - mainCamera.position.x;


    public void ChangeCamera(bool right)
    {
        
        if (!animator.IsInTransition(0))
        {
            camNumber = right ? camNumber + 1 : camNumber - 1;
            animator.SetInteger("CameraNumber", camNumber);
        }
    }

}
