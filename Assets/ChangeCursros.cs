using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ChangeCursros : MonoBehaviour
{
    [SerializeField] private Image cursor;
    private static Animator animator;


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        Debug.Log(animator);
    }

    private void Update()
    {
        cursor.rectTransform.position = Input.mousePosition;
    }

    public static void ActiveCursor()
    {
        animator.SetBool("attack", false);
        animator.SetBool("decline", false);
    }

    public static void AttackCursor()
    {
        animator.SetBool("decline", false);
        animator.SetBool("attack", true);
    }

    public static void DeclineCursor()
    {
        animator.SetBool("decline", true);
    }


    public void EndDecline()
    {
        animator.SetBool("decline", false);
    }



}
