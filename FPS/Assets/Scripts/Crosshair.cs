using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    public float xWidht=0;
    public float yHeight=0;
    public bool isAdaptive = false;
    RectTransform rect;
    // Start is called before the first frame update
    void Start()
    {
        rect = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        rect.sizeDelta = new Vector2(xWidht, yHeight);
    }
    public void UpdateCrosshair(float x,float y)
    {
        if (isAdaptive)
        {
            xWidht = x;
            yHeight = y;
        }
    }
}
