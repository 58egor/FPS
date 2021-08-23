using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Transform point;
    public bool pause = false;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("LockState:" + Cursor.lockState);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void FixedUpdate()
    {

    }
    // Update is called once per frame
    void Update()
    {
        if (!pause)
        {
            if (Cursor.visible)
            {
                Debug.Log("LockState:" + Cursor.lockState);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            Debug.Log("Pause1:" + pause);
        }
        else
        {
            if (!Cursor.visible)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                Debug.Log("LockState:" + Cursor.lockState);
            }
            Debug.Log("Pause2:" + pause);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pause = !pause;
            Debug.Log("Pause:"+pause);
        }
        transform.position = point.position;
        //Quaternion rot = transform.rotation;
        //rot.z = point.rotation.z;
        transform.rotation = point.rotation;
    }
}
