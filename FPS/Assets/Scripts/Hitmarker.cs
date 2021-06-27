using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hitmarker : MonoBehaviour
{
    float timeout = 0;
    float timer = 0;
    RectTransform rect;
    public Image[] parts;
    float radius;
    float speed = 0;

    // Start is called before the first frame update
    void Start()
    {
        rect = GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (rect.sizeDelta.x < radius * 2)
        {
            rect.sizeDelta = new Vector2(rect.sizeDelta.x + speed * Time.deltaTime, rect.sizeDelta.x + speed * Time.deltaTime);//изменяем размер 
        }
        if (timer < (timeout/4+timeout/2))//проверяем затраченное время
        {
            timer += Time.deltaTime;
        }
        else//если время вышло то вырубаем хитмаркер
        {
            for (int i = 0; i < parts.Length; i++)//когда достигли цели
            {
                parts[i].gameObject.SetActive(false);//вырубаем хитмаркер
            }
        }
        
    }
    public void UpdateHitmarher(float rad, float time)//изменяем хитмаркер в соответсвии с оружием
    {
        radius = rad;
        timeout = time;
        speed = (radius*2) / (timeout / 2);
    }
    public void Active(Color color)//активируем с полученным цветом
    {
        timer = 0;
        rect.sizeDelta = new Vector2(0, 0);
        for (int i = 0; i < parts.Length;i++) {
            parts[i].color = color;
            parts[i].gameObject.SetActive(true);
        }
    }
}
