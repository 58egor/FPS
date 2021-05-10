using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayShooting : MonoBehaviour
{
    public float damage = 1f;//наносимый урон
    public int targets = 1;//количество поражаемых целей
    public float timeout = 0.2f;//задержка между выстрелами
    public int maxAmmo=30;//максимальное количество патронов в обоиме
    private bool isReload = false;//актива ли перезарядка
    public float reloadTimer;//время перезарядки
    public Transform cam;
    public float Radius;
    public int firstInTarget=5;
    private int curShoots;
    public float otdachaYmin = 0;//отдача по у
    public float otdachaYmax = 0;//отдача по у
    public float otdachaXmin = 1;//отдача по х
    public float otdachaXmax = 1;//отдача по х
    private float curReloadTimer;//текущий таймер перезарядки
    private int currentAmmo;//текущее количество патронов
    private float curTimeout;//текущая задержка между выстрелами
    private Camera camera;
    private Control contr;
    private bool isShooting=false;
    private Crosshair crosshair;
    public GameObject Object;
    private Animator animator;
    public int normal = 60;
    public int zoom = 30;
    public float smooth = 5;
    bool isZoomed=false;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInParent<Animator>();
        crosshair = GameObject.Find("Crosshair").GetComponent<Crosshair>();
        contr = GetComponentInParent<Control>();
        curTimeout = timeout;
        camera = Camera.main;
        currentAmmo = maxAmmo;
        curReloadTimer = reloadTimer;
        curShoots = firstInTarget;
        crosshair.UpdateCrosshairActive(Radius * 2, Radius * 2,timeout);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
    }
    void Update()
    {
        crosshair.UpdateCrosshairActive(Radius * 2, Radius * 2, timeout);
        if (Input.GetMouseButton(1) && !isReload)
        {
            if (!isZoomed)
            {
                isZoomed = true;
                animator.SetTrigger("PricelStart");
                animator.SetBool("Pricel", true);
            }
            camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, zoom, Time.deltaTime * smooth);
        }
        else
        {
            isZoomed = false;
            animator.SetTrigger("PricelStop");
            animator.SetBool("Pricel", false);
            camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, normal, Time.deltaTime * smooth);
        }
        if (Input.GetMouseButton(0) && curTimeout <= 0 && currentAmmo>0 && !isReload)
        {
            animator.SetBool("FireTest", true);
            currentAmmo--;//уменшьаем количество пуль
            otdacha(currentAmmo);//вызываем функцию отдачи
            Ray ray = camera.ScreenPointToRay(Razbros());//создаем луч, вызывая функцию,формирующая направление луча
            curTimeout = timeout;
            RaycastHit[] hit;
            hit = Physics.RaycastAll(ray);//делаем выстрел
            for (int i = 0; i < hit.Length; i++)
            {
                if (i != targets)
                {
                    Debug.Log("Damage:" + hit[i].collider.name);
                    Debug.DrawLine(ray.origin, hit[i].point, Color.green, 5);
                    Instantiate(Object,hit[i].point,Object.transform.rotation);
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
                curShoots = firstInTarget;
                animator.SetBool("FireTest", false);
                contr.otdachaY = 0;
                contr.otdachaX = 0;
                Debug.Log("stop sh");
                if (isShooting)//если мышка отпущена или не идет срельба
                {
                    Debug.Log("Sto shooting");
                    isShooting = false;
                    contr.otdachaY = 0;
                    contr.otdachaX = 0;
                }
            }
        }
        if (Input.GetKey(KeyCode.R) && !isReload && !(currentAmmo==maxAmmo))
        {
            animator.SetTrigger("Reload");
            animator.SetBool("FireTest", false);
            Debug.Log("ActiveReload");
            isReload = true;
        }
        CheckAmmo();//функция отвечающая за перезарядку
    }
    public Vector3 Razbros()//функция отвечающая за разброс
    {
        float sdvigX = 0;
        float sdvigY = 0;
        if (curShoots == 0 && !isZoomed)
        {
            sdvigX = Random.Range(-Radius, Radius);//радномим отклонение
            sdvigY = Random.Range(-Radius, Radius);//рандомим отклонение
            while ((sdvigX * sdvigX + sdvigY * sdvigY) > Radius * Radius)//проверяем точка в круге?
            {
                Debug.Log("Рандомим заново");//если нет
                sdvigX = Random.Range(-Radius, Radius);//то рандомим заново
                sdvigY = Random.Range(-Radius, Radius);
            }
        }
        Vector3 screenCneter = new Vector3(Screen.width / 2+ sdvigX, Screen.height / 2+ sdvigY, 0);//определяем центр камеры
        if (curShoots != 0)
        {
            curShoots--;
        }
        return screenCneter;
    }
    public void CheckAmmo() {//проверяем патроны
        
        if (currentAmmo <= 0 || isReload)//если патронов нету
        {
            if (!isReload)
            {
                animator.SetTrigger("Reload");
                isZoomed = false;
                isReload = true;
            }
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
    //public void CheckRazbros()//увеличение разброса
    //{
    //    if (curRazbros<=0) //если отстреляли нужное количество пуль
    //    {
    //        curRazbros = razbros;
    //        thisRazbros += step;//увеличиваем разброс на соответствующий шаг
          
    //        if (thisRazbros > maxRazbros)
    //        {
    //            thisRazbros = maxRazbros;
    //        }
    //        crosshair.UpdateCrosshair(thisRazbros * 2, thisRazbros * 2, false);
    //    }
    //}
    public void otdacha(int ammo)
    {
            isShooting = true;
            float otd= Random.Range(otdachaYmin, otdachaYmax);
            contr.otdachaY = otd;
            Debug.Log("Y:" + otd);
        if (!isZoomed)
        {
            otd = Random.Range(otdachaXmin, otdachaXmax);
            contr.otdachaX = otd;
            Debug.Log("X:" + otd);
        }
        else
        {
            contr.otdachaX = 0;
        }
        //Debug.Log("currentAmmo:"+ammo+"%3:"+ammo % 2);
        //if ((ammo % 2)==0)
        //{
        //    otdachaX = -otdachaX;
        //   Debug.Log("Sbros");
        //}
    }
    void OnDrawGizmosSelected() // подсветка, для визуальной настройки jumpDistance
    {
        Gizmos.color = Color.red;
        Vector3 screenCneter = new Vector3(Screen.width / 2, Screen.height / 2, 0);//определяем центр камеры
        Gizmos.DrawRay(camera.transform.position, camera.transform.forward);
        Gizmos.color = Color.red;

    }
}
