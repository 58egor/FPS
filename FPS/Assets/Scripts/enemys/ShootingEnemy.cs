using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ShootingEnemy : MonoBehaviour
{
    Rigidbody body;
    Transform player;
    public int speedMin = 15;//мин скорость
    public int speedMax = 15;//макс скорость
    int speed = 0;
    NavMeshAgent agent;
    public int rangeMin = 10;//радиус при котором начинает стрелять минимальый
    public int rangeMax = 10;//максимальный
   int range = 0;
    private int layerMask;
    public float jumpDistance = 1.2f; // расстояние от центра объекта, до поверхности
    bool OnGround = false;
    public GameObject bullet;
    public GameObject gunPoint;
    public float timeout = 1f;//кд между выстрелами
    public float ocheredTimeout = 0.5f;//кд между очередью выстрелов
    float curOcheredTimeout=0;
    float curTimeout=0;
    public int numberOfBulletsMin = 1;//минимальное количество выпускаем пуль
    public int numberOfBulletsMax = 4;//максимальное количество
    int numberOfBullets=1;
    int number;
    public int bulletSpeedMin = 25;//скорость пули мин
    public int bulletSpeedMax = 40;//макс
    int bulletSpeed = 20;
    bool Shoot = false;
    AudioManager audio;

    // Start is called before the first frame update
    void Start()
    {
        speed = Random.Range(speedMin, speedMax);//ранодомим скорость
        range = Random.Range(rangeMin, rangeMax);//рандомим радиус
        bulletSpeed = Random.Range(bulletSpeedMin, bulletSpeedMax);//рандомим скорость пули
        numberOfBullets = Random.Range(numberOfBulletsMin, numberOfBulletsMax);//рандомим количество пуль
        number = numberOfBullets;
        curOcheredTimeout = ocheredTimeout;
        body = GetComponent<Rigidbody>();
        body.freezeRotation = true;
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.Find("Player").transform;
        agent.speed = speed;
        layerMask = 1 << gameObject.layer | 1 << 2;
        layerMask = ~layerMask;
        audio = GetComponent<AudioManager>();
        audio.Play("Spawn");

    }
    void look()
    {
        transform.LookAt(player);//смотрим на противника
    }
    void CheckGround()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, Vector3.down);//создаем луч направленный вниз
        if (Physics.Raycast(ray, out hit, jumpDistance, layerMask))//выпускаем луч определннеой длинны
        {
            agent.enabled = true;
            OnGround = true;
        }
        else
        {
            OnGround = false;
            agent.enabled = false;
        }
    }
    void moving()
    {
        float distance = Vector3.Distance(transform.position, player.position);//определяем дистанцию до героя
        Debug.Log("Distance:" + distance);
        if (distance > range)//если больше радиуса
        {
            agent.isStopped = false;
            agent.SetDestination(player.position);//то идем к нему с помощью нав меш агента
        }
        else//иначе
        {
            if (!agent.isStopped)
            {
                audio.Play("Find");
            }
            agent.isStopped = true;

            if (curTimeout <=0 || Shoot)
            {
                
                Shoot = true;
                curTimeout = timeout;
                if (curOcheredTimeout <= 0)//стреляем очереью в героя
                {
                    audio.Play("Fire");
                    GameObject spawn = Instantiate(bullet, gunPoint.transform.position, Quaternion.identity);
                    Rigidbody bulletInstance = spawn.GetComponent<Rigidbody>();
                    bulletInstance.velocity = gunPoint.transform.forward * bulletSpeed;
                    curOcheredTimeout = ocheredTimeout;
                    number--;

                }
                else
                {
                    curOcheredTimeout -= Time.deltaTime;
                }
                if (number <= 0)
                {
                    Shoot = false;
                }
            }
            else
            {
                curOcheredTimeout = 0;
                curTimeout -= Time.deltaTime;
                number = numberOfBullets;
            }

        }
    }
    // Update is called once per frame
    void Update()
    {
        look();//смотрим на героя
        CheckGround();//проверяем на земле ли
    }
    private void FixedUpdate()
    {
        moving();//двигаемся
    }
    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, range);
    }
}
