using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCursros : MonoBehaviour
{
    [SerializeField] private Texture2D activeCursor;
    [SerializeField] private Texture2D[] declineCursors;
    [SerializeField] private Texture2D attackCursor;
    [SerializeField] private int numberOfCycles = 2;
    [SerializeField] private float animationTime = 1;

    

    static public ChangeCursros instance;

    private static Texture2D active;
    private static Texture2D[] decline;
    private static Texture2D attack;
    private static CursorMode cursorMode;
    private static Vector2 hotSpot;
    private static bool inAnimation = false;

    // Start is called before the first frame update
    void Start()
    {
        active = activeCursor;
        decline = declineCursors;
        attack = attackCursor;
        hotSpot = Vector2.zero;
        cursorMode = CursorMode.Auto;
        instance = this;
    }

    public static void DeclineCursor()
    {
        if(!inAnimation)
            instance.StartCoroutine("waitForChange");
    }

    public static void ActiveCursor()
    {
        Cursor.SetCursor(active, hotSpot, cursorMode);
    }

    public static void AttackCursor()
    {
        instance.StopCoroutine("waitForChange");
        inAnimation = false;
        Cursor.SetCursor(attack, hotSpot, cursorMode);
    }


    IEnumerator waitForChange()
    {
        inAnimation = true;
        for(int i=0; i< numberOfCycles;i++)
        {
            Cursor.SetCursor(decline[0], hotSpot, cursorMode);
            yield return new WaitForSeconds(animationTime);
            Cursor.SetCursor(decline[1], hotSpot, cursorMode);
            yield return new WaitForSeconds(animationTime);
            Cursor.SetCursor(decline[0], hotSpot, cursorMode);
            yield return new WaitForSeconds(animationTime);
            Cursor.SetCursor(decline[2], hotSpot, cursorMode);
        }

        ActiveCursor();
        inAnimation = false;
    }

}
