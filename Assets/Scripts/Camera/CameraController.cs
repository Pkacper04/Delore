using UnityEngine;
using System.Collections;


public class CameraController : MonoBehaviour
{

    [SerializeField] internal float playerOffset = 7f;
    [SerializeField] CmeraSaveLoad cameraSaveSystem;


    private Transform player;
    private Animator animator;
    private bool inTransition = false;


    internal Transform mainCamera;
    internal int camNumber = 1;
    internal int camNumberStorage = 0;
    internal int lastCameraId = 0;
    internal bool xAxis = true;


    private void Start()
    {
        animator = GetComponent<Animator>();
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").transform;
        player = GameObject.FindGameObjectWithTag("Player").transform;

        cameraSaveSystem.LoadData(this);

        if(xAxis)
            animator.SetInteger("CameraNumberX", camNumber);
        else
        {
            animator.SetInteger("CameraNumberZ", camNumber);
            WaitForTransition();
        }
    }



    void Update()
    {

        cameraSaveSystem.SetCameraId(this);

        if (xAxis)
            XAxisCamera();
        else
            ZAxisCamera();

    }

    private void ZAxisCamera()
    {
        
        if(!animator.IsInTransition(0) && !inTransition)
        {
            if (CalculatePlayerOffsetZ() >= playerOffset)
                camNumber++;
            else if (CalculatePlayerOffsetZ() <= -playerOffset)
                camNumber--;
            animator.SetInteger("CameraNumberZ", camNumber);
        }
        
        
    }

    private void XAxisCamera()
    {
        
        if (!animator.IsInTransition(0) && !inTransition)
        {
            if (CalculatePlayerOffsetX() >= playerOffset)
                camNumber++;
            else if (CalculatePlayerOffsetX() <= -playerOffset)
                camNumber--;
            animator.SetInteger("CameraNumberX", camNumber);

        }
    }

    private float CalculatePlayerOffsetX() => player.position.x - mainCamera.position.x;
    private float CalculatePlayerOffsetZ() => player.position.z - mainCamera.position.z;


    public void ChangeCamera(bool right)
    {
        
        if (!animator.IsInTransition(0))
        {
            camNumber = right ? camNumber + 1 : camNumber - 1;
            animator.SetInteger("CameraNumberX", camNumber);
        }
    }

    public void ChangeAxis(bool Axis)
    {
        this.xAxis = Axis;

        if (!animator.IsInTransition(0))
        {
            if (Axis)
            {
                camNumber = camNumberStorage;
                animator.SetInteger("CameraNumberZ", 0);
                animator.SetInteger("CameraNumberX", camNumber);
            }
            else
            {
                animator.SetInteger("CameraNumberZ", 1);
                camNumberStorage = camNumber;
                camNumber = 1;
                StartCoroutine(WaitForTransition());
                
            }
        }
    }


    private IEnumerator WaitForTransition()
    {
        inTransition = true;
        yield return new WaitUntil(() => animator.IsInTransition(0));
        inTransition = false;
    }

}
