using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] enemy;
    public float timer = 3f;
    float time;
    float xCenter,zCenter;
    float x, z;
    public int spawnEnemys = 3;
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
            int j = Random.Range(0, enemy.Length);
            Debug.Log("Lenght:" + enemy.Length);
            Instantiate(enemy[j], new Vector3(Random.Range(xCenter - x, xCenter + x), 5, Random.Range(zCenter - z, zCenter + z)), enemy[j].transform.rotation);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (time > 0)
        {
            time -= Time.deltaTime;
        }
        else
        {
            for (int i = 0; i < spawnEnemys; i++)
            {
                int j = Random.Range(0, enemy.Length);
                
                Instantiate(enemy[j], new Vector3(Random.Range(xCenter - x, xCenter + x), 5, Random.Range(zCenter - z, zCenter + z)), enemy[j].transform.rotation);
            }
            time = timer;
        }
    }
}
