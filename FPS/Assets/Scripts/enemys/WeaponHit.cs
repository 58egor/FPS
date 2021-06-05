using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHit : MonoBehaviour
{
    public int damage = 25;
    public AudioClip audio;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            Debug.Log("Was Hiited");
            other.GetComponent<PlayerInfo>().GetDamage(damage,audio);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
