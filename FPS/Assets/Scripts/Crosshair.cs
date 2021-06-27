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
        rect.sizeDelta = new Vector2(xWidhtActive, yHeightActive);
    }

    public void UpdateCrosshairActive(float x, float y,float speed)
    {
        if (isAdaptive)
        {
            xWidhtActive = x;
            yHeightActive = y;
            xWidht = x;
            yHeight = y;
            this.speed = speed;
        }
    }
    public void DisableOrActive(bool action)
    {
        gameObject.SetActive(action);
    }
}
