using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RayShooting : MonoBehaviour
{
    public int bullets = 1;//количество пуль
    public int damage = 1;//наносимый урон
    public int targets = 1;//количество поражаемых целей
    public float timeout = 0.2f;//задержка между выстрелами
    public int maxAmmo=30;//максимальное количество патронов в обоиме
    public bool singleReload = false;
    bool isSingleReload = false;
    private bool isReload = false;//актива ли перезарядка
    public float reloadTimer;//время перезарядки
    public Transform cam;
    public float Radius;//радиус разброса
    public int firstInTarget=5;//количество пуль которые попадают при самом начале выстрела
    private int curShoots;
    public float otdachaYmin = 0;//отдача по у
    public float otdachaYmax = 0;//отдача по у
    public float otdachaXmin = 1;//отдача по х
    public float otdachaXmax = 1;//отдача по х
    private float curReloadTimer;//текущий таймер перезарядки
    private int currentAmmo;//текущее количество патронов
    private float curTimeout;//текущая задержка между выстрелами
    private Camera camera;
    private Control contr;//доступ к скрпиту для передачи отдачи
    private bool isShooting=false;
    private Crosshair crosshair;//доступ к прицелу
    private Hitmarker hitmarker;//доступ к хитмаркеру
    private Animator animator;
    public int normal = 60;//нормальый фов
    public int zoom = 30;//при зуме
    public float smooth = 5;
    bool isZoomed=false;
    public bool zoomExist = true;
    public float zoomBuff = 2;
    public bool singleShoot=false;
    int layer;
    Text text;//доступк счетчику патронов
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInParent<Animator>();
        crosshair = GameObject.Find("Crosshair").GetComponent<Crosshair>();//получаем доступ к прицелу
        hitmarker= GameObject.Find("Hitmarker").GetComponent<Hitmarker>();//получаем доступ к хитмаркеру
        contr = GetComponentInParent<Control>();//получаем доступ к скрипту
        curTimeout = timeout;
        camera = Camera.main;
        currentAmmo = maxAmmo;
        curReloadTimer = reloadTimer;
        curShoots = firstInTarget;
        crosshair.UpdateCrosshairActive(Radius * 2, Radius * 2,timeout);//передаем данные для прицела
        hitmarker.UpdateHitmarher(Radius, timeout);//передаем данные хитмаркеру
        text = GameObject.Find("Ammo").GetComponent<Text>();
        text.text = currentAmmo.ToString();
        layer = 1 << 11;
        layer = ~layer;
        Debug.Log("Lyer:" + layer);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
    }
    void Update()
    {
        crosshair.UpdateCrosshairActive(Radius * 2, Radius * 2, timeout);
        if (Input.GetMouseButton(1) && !isReload && zoomExist)//если нажали правую кнопку мыши то прицелились
        {
            if (!isZoomed)
            {
                isZoomed = true;
                animator.SetTrigger("PricelStart");//активируем анимацию
                animator.SetBool("Pricel", true);
            }
            camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, zoom, Time.deltaTime * smooth);//изменяем фов
            crosshair.DisableOrActive(false);//выключаем прицел
        }
        else//если отжали
        {
            crosshair.DisableOrActive(true);//велючаем прицел
            isZoomed = false;
            animator.SetTrigger("PricelStop");
            animator.SetBool("Pricel", false);
            camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, normal, Time.deltaTime * smooth);//вернули фов в норму
        }
        if (((Input.GetMouseButton(0) && !singleShoot) || (Input.GetMouseButtonDown(0) && singleShoot)) && curTimeout <= 0 && !isReload)//если нажали кнопку выстрела
        {
            if (singleReload)
            {
                animator.SetBool("ReloadStop", false);
                isSingleReload = false;
            }
            currentAmmo--;//уменшьаем количество пуль
            otdacha();//вызываем функцию отдачи
            curTimeout = timeout;
            for (int i = 0; i < bullets; i++)
            {
                Shoot();//вызываем функцию выстрела
            }
        }
        else//если не стреляем или кд
        {
            curTimeout -= Time.deltaTime;//уменьшаем кд
            if (!singleShoot)//проверям одиночный выстрел или нет
            {//нет
                if ((!Input.GetMouseButton(0)) || isReload || currentAmmo <= 0)//если мышь отпущена или перезарядка
                {
                    curShoots = firstInTarget;//первые н пуль снова бьют в точку
                    animator.SetBool("FireTest", false);
                    contr.otdachaY = 0;//выключаем отдачу
                    contr.otdachaX = 0;
                    Debug.Log("stop sh");
                    CheckAmmo();//функция отвечающая за перезарядку
                }
            }
            else
            {//да
                curShoots = firstInTarget;
               // animator.SetBool("FireTest", false);
                if (curTimeout <= 0)//если кд кончилось и на мышь не нажали то выстрела нету
                {
                    animator.SetBool("FireTest", false);
                    contr.otdachaY = 0;
                    contr.otdachaX = 0;
                    Debug.Log("stop sh");
                    CheckAmmo();//функция отвечающая за перезарядку
                }
            }
                
        }
        if (Input.GetKey(KeyCode.R) && !isReload && !(currentAmmo==maxAmmo) && curTimeout <= 0)//активриуем перезарядку
        {
            animator.SetTrigger("Reload");
            animator.SetBool("FireTest", false);
            Debug.Log("ActiveReload");
            isReload = true;
            isZoomed = false;
            if (singleReload)
            {
                animator.SetBool("ReloadStop", true);
            }
        }
        text.text = currentAmmo.ToString();
    }
    void Shoot()//функция выстрела
    {
        animator.SetBool("FireTest", true);
        Ray ray = camera.ScreenPointToRay(Razbros());//создаем луч, вызывая функцию,формирующая направление луча
        RaycastHit[] hit;
        hit = Physics.RaycastAll(ray, Mathf.Infinity, layer);//делаем выстрел
        for (int i = 0; i < hit.Length; i++)//соритуем объекты в которые попали по увеличению дистанции
        {

            for (int j = i; j < hit.Length; j++)
            {
                if (hit[i].distance > hit[j].distance)
                {
                    RaycastHit copy = hit[i];
                    hit[i] = hit[j];
                    hit[j] = copy;
                }
            }
        }
        Debug.Log("hit lenght:" + hit.Length);
        for (int i = 0; i < hit.Length; i++)
        {
            Debug.Log("hit:" + hit[i].collider.name);
        }
        for (int i = 0; i < hit.Length; i++)
        {

            if (i < targets)//передаем урон определенному количество протинвиков
            {
                Debug.Log("Damage:" + hit[i].collider.name);
                if (hit[i].collider.gameObject.layer == 9)
                {
                    Color color = hit[i].collider.GetComponent<EnemyInfo>().Damage(damage);//передаем урон и получаем инфомрацию о полученном уроне противником
                    hitmarker.Active(color);//активируем хитмаркер(белый-попал,красный-убил)

                }
                else
                {

                }
            }
            else
            {
                break;
            }
        }
    }
    public Vector3 Razbros()//функция отвечающая за разброс
    {
        float sdvigX = 0;
        float sdvigY = 0;
        float rad = Radius;
        if (isZoomed) {
            if (zoomBuff == 0)
            {
                rad = 0;
            }
            else
            {
                rad = rad / zoomBuff;
            }
        }
        if (curShoots == 0)
        {
            sdvigX = Random.Range(-rad, rad);//радномим отклонение
            sdvigY = Random.Range(-rad, rad);//рандомим отклонение
            while ((sdvigX * sdvigX + sdvigY * sdvigY) > rad * rad)//проверяем точка в круге?
            {
                Debug.Log("Рандомим заново");//если нет
                sdvigX = Random.Range(-rad, rad);//то рандомим заново
                sdvigY = Random.Range(-rad, rad);
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
        
        if (currentAmmo <= 0 || isReload || isSingleReload)//если патронов нету
        {
            if (!isReload && !isSingleReload)
            {
                animator.SetTrigger("Reload");
                if (singleReload)
                {
                    animator.SetBool("ReloadStop", true);
                }
                isZoomed = false;
                isReload = true;
            }
            Debug.Log("Reload:"+curReloadTimer);
            if (curReloadTimer <= 0)
            {

                if (!singleReload)
                {
                    currentAmmo = maxAmmo;
                    isReload = false;
                }
                else
                {
                    currentAmmo++;
                    isReload = false;
                    if (currentAmmo != maxAmmo) { 
                        isSingleReload = true;
                    }
                    else
                    {
                        animator.SetBool("ReloadStop", false);
                        isSingleReload = false;

                    }
                }
                curReloadTimer = reloadTimer;
            }
            else
            {
                curReloadTimer -= Time.deltaTime;
            }
        }
    }
    public void otdacha()
    {
        isShooting = true;
        float otd;
        if (isZoomed)//если прицелились то отдача уменьшена в 2 раза
        {
            otd = Random.Range(otdachaYmin/2, otdachaYmax/2);//рандомим отдачу по координате у
        }
        else
        {
            otd = Random.Range(otdachaYmin, otdachaYmax);//рандомим отдачу по координате у
        }
            contr.otdachaY = otd;//передаем эту отдачу
            Debug.Log("Y:" + otd);
        if (!isZoomed)
        {
            otd = Random.Range(otdachaXmin, otdachaXmax);//рандомим отдачу по координате х

        }
        else
        {
            otd = Random.Range(otdachaXmin/2, otdachaXmax/2);//рандомим отдачу по координате х
        }
        contr.otdachaX = otd;//передаем отдачу
        Debug.Log("X:" + otd);
    }
    void OnDrawGizmosSelected() // подсветка, для визуальной настройки jumpDistance
    {
        Gizmos.color = Color.red;
        Vector3 screenCneter = new Vector3(Screen.width / 2, Screen.height / 2, 0);//определяем центр камеры
        Gizmos.DrawRay(camera.transform.position, camera.transform.forward);
        Gizmos.color = Color.red;

    }
    private void OnDisable()
    {
        curReloadTimer = reloadTimer;
        animator.Rebind();
    }
}
