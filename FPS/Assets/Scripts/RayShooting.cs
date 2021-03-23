using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayShooting : MonoBehaviour
{
    public float damage = 1f;
    public int targets = 1;
    public float timeout = 0.2f;
    public int maxAmmo=30;
    private bool isReload = false;
    public float reloadTimer;
    public Transform cam;
    public float maxRazbros;//максимальное значение рандома по коорлинате 
    public float minRazbros;//минимальное значение рандома по коорлинате
    private float thisRazbros;//текущее значение разброса
    public float step;//шаг увелиивающий диапзоно выстрела
    public int razbros;//количество выстрелов увеличивающий разброс
    private int curRazbros;//текущее количество выстрелов для увеличеня разброса
    public float razbrosTimer;//таймер уменьшающий диапозон разброса при остановки стрельбы
    public float otdachaY = 0;
    public float otdachaX = 1;
    private float curRazbrosTimeout;
    private float curReloadTimer;
    private int currentAmmo;
    private float curTimeout;
    private Camera camera;
    private Control contr;
    private bool isShooting=false;
    // Start is called before the first frame update
    void Start()
    {
        contr = GetComponentInParent<Control>();
        curTimeout = timeout;
        camera = Camera.main;
        currentAmmo = maxAmmo;
        curReloadTimer = reloadTimer;
        curRazbrosTimeout = razbrosTimer;
        thisRazbros = minRazbros;
        curRazbros = razbros;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
    }
    void Update()
    {
        if (Input.GetMouseButton(0) && curTimeout <= 0 && currentAmmo>0 && !isReload)
        {
            curRazbros--;
            curRazbrosTimeout = razbrosTimer;
            float sdvigX = Random.Range(-thisRazbros, thisRazbros);//радномим отклонение
            float sdvigY = Random.Range(-thisRazbros, thisRazbros);//рандомим отклонение
            while((sdvigX* sdvigX+ sdvigY* sdvigY) > thisRazbros * thisRazbros)//проверяем точка в круге?
            {
                Debug.Log("Рандомим заново");//если нет
                sdvigX = Random.Range(-thisRazbros, thisRazbros);//то рандомим заново
                sdvigY = Random.Range(-thisRazbros, thisRazbros);
            }
            currentAmmo--;
            otdacha();
            Vector3 screenCneter = new Vector3(Screen.width / 2, Screen.height / 2, 0);//определяем центр камеры
            screenCneter.x += sdvigX;//сдвигаем коорлинату
            screenCneter.y += sdvigY;//сдвигаем координату
            Ray ray = camera.ScreenPointToRay(screenCneter);
            curTimeout = timeout;
            RaycastHit[] hit;
            hit = Physics.RaycastAll(ray);
            for (int i = 0; i < hit.Length; i++)
            {
                if (i != targets)
                {
                    Debug.Log("Damage:" + hit[i].collider.name);
                    Debug.DrawLine(ray.origin, hit[i].point, Color.green, 5);
                }
                else
                {
                    break;
                }
            }
        }
        else
        {
            curTimeout -= Time.deltaTime;
            if (!Input.GetMouseButton(0) || isReload || currentAmmo <= 0)
            {
                Debug.Log("stop sh");
                if (isShooting)
                {
                    Debug.Log("Sto shooting");
                    isShooting = false;
                    contr.otdachaY = 0;
                    contr.otdachaX = 0;
                }
                curRazbrosTimeout -= Time.deltaTime;
                if (curRazbrosTimeout <= 0)
                {
                    thisRazbros -= step;
                    if (thisRazbros < minRazbros)
                    {
                        thisRazbros = minRazbros;
                    }
                }
            }
        }
        if (Input.GetKey(KeyCode.R) && !isReload && !(currentAmmo==maxAmmo))
        {
            Debug.Log("ActiveReload");
            isReload = true;
        }
        CheckAmmo();//функция отвечающая за перезарядку
        CheckRazbros();//функция отвечающая за увеличение радиуса разброса

    }
    public void CheckAmmo() {
        
        if (currentAmmo <= 0 || isReload)//если патронов нету
        {
            Debug.Log("Reload:"+curReloadTimer);
            if (curReloadTimer <= 0)
            {
                isReload = false;
                currentAmmo = maxAmmo;
                curReloadTimer = reloadTimer;
            }
            else
            {
                curReloadTimer -= Time.deltaTime;
            }
        }
    }
    public void CheckRazbros()
    {
        if (curRazbros<=0) 
        {
            curRazbros = razbros;
            thisRazbros += step;
            if (thisRazbros > maxRazbros)
            {
                thisRazbros = maxRazbros;
            }
        }
    }
    public void otdacha()
    {
        if (!isShooting)
        {
            isShooting = true;
            contr.otdachaY = otdachaY;
            contr.otdachaX = otdachaX;
        }
    }
    void OnDrawGizmosSelected() // подсветка, для визуальной настройки jumpDistance
    {
        Gizmos.color = Color.red;
        Vector3 screenCneter = new Vector3(Screen.width / 2, Screen.height / 2, 0);//определяем центр камеры
        Gizmos.DrawRay(camera.transform.position, camera.transform.forward);
        Gizmos.color = Color.red;

    }
}
