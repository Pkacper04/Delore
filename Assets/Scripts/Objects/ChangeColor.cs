using System.Collections.Generic;
using UnityEngine;

    public class ChangeColor : MonoBehaviour
    {
        private Dictionary<Renderer, Color> objectRenderer = new Dictionary<Renderer, Color>();
    // Start is called before the first frame update
    
        void Start()
        {
            foreach (var item in GetComponentsInChildren<Renderer>())
            {
                objectRenderer.Add(item, item.material.color);
            }
        }

        public void OnHover()
        {
                foreach (var item in objectRenderer.Keys)
                {

                    item.material.color += new Color(1f, 1f, 1f);
                }
        }


        public void OutHover()
        {
            foreach (var item in objectRenderer)
            {
                item.Key.material.color = item.Value;
            }
        }

    }

