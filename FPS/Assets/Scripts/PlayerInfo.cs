using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfo : MonoBehaviour
{
    public int HP=100;
    AudioSource audio;
    AudioManager audioManager;
    Slider slider;
    public GameObject dead;
    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
        slider = GameObject.Find("HP").GetComponent<Slider>();
        slider.maxValue = HP;
        slider.value = HP;
        audioManager = GetComponent<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        slider.value = HP;
    }
    public void GetDamage(int hp,AudioClip clip)
    {
        HP -= hp;
        audio.PlayOneShot(clip);

        if (HP <= 0)
        {
            dead.SetActive(true);
            Cursor.visible = true;
            if (!audioManager.isPlaying("Dead"))
            {
                audioManager.Play("Dead");
            }
        }
        else
        {
            if (!audioManager.isPlaying("Hit"))
            {
                audioManager.Play("Hit");
            }
        }
    }
}
