using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInfo : MonoBehaviour
{
    public int HP = 100;
    Animation animation;
    float timer = 3f;
    bool start = false;
    // Start is called before the first frame update
    void Start()
    {
        animation=transform.GetComponent<Animation>();
    }

    // Update is called once per frame
    void Update()
    {
        if (start)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
    public Color Damage(int hp)
    {
        if (!start)
        {
            HP -= hp;
            Debug.Log("Урон");
            if (HP <= 0)
            {
                gameObject.layer = 10;
                animation.Play();
                return Color.red;
            }
            else
            {
                return Color.white;
            }
        }
        return Color.white;
    }
    public void Dead()
    {
        start = true;
    }
}
