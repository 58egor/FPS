using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Transform point;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = point.position;
        //Quaternion rot = transform.rotation;
        //rot.z = point.rotation.z;
        transform.rotation = point.rotation;
    }
}
