using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
public class ChangeCursros : MonoBehaviour
{
    [SerializeField] private Texture2D activeCursor;
    [SerializeField] private Texture2D pickUpCursor;
    [SerializeField] private Texture2D[] declineCursors;


    [SerializeField] private int numberOfCycles = 4;
    [SerializeField] private float animationTime = 0.04f;





    static public ChangeCursros instance;

    private static Texture2D active;
    private static Texture2D pickUp;
    private static Texture2D[] decline;
    private static CursorMode cursorMode;
    private static Vector2 hotSpot;
    private static bool inAnimation = false;

    // Start is called before the first frame update
    void Start()
    {
        active = activeCursor;
        pickUp = pickUpCursor;
        decline = declineCursors;
        hotSpot =  new Vector2(200, 100);
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

    public static void PickUpCursor()
    {
        Cursor.SetCursor(pickUp, hotSpot, cursorMode);
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
