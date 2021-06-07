using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject enemy;
    public float timer = 3f;
    float time;
    float xCenter,zCenter;
    float x, z;
    void Start()
    {
        time = timer;
        xCenter = transform.position.x;
        zCenter = transform.position.z;
        x = transform.localScale.x/2;
        z = transform.localScale.z/2;
        Debug.Log("Scale:" + x);
        for (int i = 0; i < 10; i++)
        {
            Instantiate(enemy, new Vector3(Random.Range(xCenter - x, xCenter + x), 5, Random.Range(zCenter - z, zCenter + z)), enemy.transform.rotation);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //if (time > 0)
        //{
        //    time -= Time.deltaTime;
        //}
        //else
        //{
        //    for (int i = 0; i < 3; i++)
        //    {
        //        Instantiate(enemy, new Vector3(Random.Range(xCenter - x, xCenter + x), 5, Random.Range(zCenter - z, zCenter + z)), enemy.transform.rotation);
        //    }
        //    time = timer;
        //}
    }
}
