using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public int damage=0;
    public AudioClip audio;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            other.GetComponent<PlayerInfo>().GetDamage(damage,audio);
            Destroy(gameObject);
        }
        else
        {
            if (other.gameObject.layer != 9 && other.gameObject.layer != 10)
            {
                Debug.Log("name:" + other.name);
                Destroy(gameObject);
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
