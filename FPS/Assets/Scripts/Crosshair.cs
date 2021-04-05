using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    public float xWidht=0;
    public float yHeight=0;
    public float speed = 20f;
    float speedActive = 20f;
    float xWidhtActive = 0;
    float yHeightActive = 0;
    public bool isAdaptive = false;
    RectTransform rect;
    // Start is called before the first frame update
    void Start()
    {
        speedActive = speed;
        rect = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (speedActive > 0 && xWidhtActive<xWidht || speedActive < 0 && xWidhtActive > xWidht /*|| !(xWidht - 1.5 <= xWidhtActive && xWidhtActive <= xWidht + 1.5)*/)
        {
            
            xWidhtActive = xWidhtActive + speedActive * Time.deltaTime;
            Debug.Log("xwidth active:" + xWidhtActive);
        }
        else
        {
        }
        if (speedActive > 0 && yHeightActive < yHeight || speedActive < 0 && yHeightActive > yHeight/*!(yHeight - 1.5 <= yHeightActive && yHeightActive <= yHeight + 1.5)*/)
        {
            yHeightActive = yHeightActive + speedActive * Time.deltaTime;
        }
        else
        {
        }
        rect.sizeDelta = new Vector2(xWidhtActive, yHeightActive);
    }
    public void UpdateCrosshair(float x, float y, bool znak)
    {
        if (isAdaptive)
        {
            xWidht = x;
            yHeight = y;
            if (!znak) { speedActive = speed; }
            else
            {
                Debug.Log("minus speed");
                speedActive = -speed*2.5f;
            }
        }
    }
    public void UpdateCrosshairActive(float x, float y)
    {
        if (isAdaptive)
        {
            xWidhtActive = x;
            yHeightActive = y;
            xWidht = x;
            yHeight = y;
        }
    }
}
