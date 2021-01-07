using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShaker : MonoBehaviour
{
    Vector3 originalPos;
    public float speed = 1;
    public float scaleX = 1;
    public float scaleY = 1;
    int pos = 0;
    float cornerAngle = 0f;
    // Start is called before the first frame update
    void Start()
    {
        originalPos = transform.localPosition;
        Debug.Log(originalPos.y);
    }
    void CameraMove()
    {
        if (Input.GetAxisRaw("Vertical") != 0  && !GlobalInfo.CheckWallRun())
        {
            cornerAngle += Time.deltaTime * speed;
            this.gameObject.transform.localPosition = new Vector3(Mathf.Cos(cornerAngle) * scaleX, originalPos.y+ Mathf.Sin(cornerAngle) * Mathf.Cos(cornerAngle) * scaleY, 0);//двигаем камеру по Лемниската Бернулли
        }
        else
        {
            transform.localPosition = originalPos;
        }
    }

    // Update is called once per frame
    void Update()
    {
        CameraMove();
        //if (Input.GetAxisRaw("Vertical")!=0)
        //{
        //    float x = Random.Range(-0.5f, 0.5f);
        //    float y = Random.Range(-0.5f, 0.5f);
        //    transform.localPosition = new Vector3(x, y, transform.localPosition.z);
        //}
        //else
        //{
        //    transform.localPosition = new Vector3(0, 0, transform.localPosition.z);
        //}

    }
}