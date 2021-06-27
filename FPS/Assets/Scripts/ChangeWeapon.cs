using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeWeapon : MonoBehaviour
{
    public GameObject[] guns;
    int id = 0;
    // Start is called before the first frame update
    void Start()
    {
        guns[id].SetActive(true);
        Debug.Log("Length:" + guns.Length);
    }

    // Update is called once per frame
    void Update()
    {
        float mouse = Input.GetAxis("Mouse ScrollWheel");
        if (mouse > 0)
        {
            guns[id].SetActive(false);
            id++;
            if (id > guns.Length-1)
            {
                id = 0;
            }
            guns[id].SetActive(true);

        }
        if (mouse < 0)
        {
            guns[id].SetActive(false);
            id--;
            if (id <0)
            {
                id = guns.Length-1;
            }
            guns[id].SetActive(true);
        }
    }
}
