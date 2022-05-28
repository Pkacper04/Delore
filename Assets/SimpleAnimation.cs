using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimpleAnimation : MonoBehaviour
{
    [SerializeField]
    private List<Sprite> sprites;

    [SerializeField]
    private float speed;

    [SerializeField]
    private Image placeholder;

    public bool startAnimation = false;

    private int counter = 0;
    private float timeContainer = 0;

    private void Update()
    {
        if(startAnimation)
        {
            if(timeContainer + speed < Time.unscaledTime)
            {
                timeContainer = Time.unscaledTime;
                placeholder.sprite = sprites[counter];
                counter++;
                if (counter == sprites.Count)
                    counter = 0;
            }
        }
    }

}
