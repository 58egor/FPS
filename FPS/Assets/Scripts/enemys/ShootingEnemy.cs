using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ShootingEnemy : MonoBehaviour
{
    Rigidbody body;
    Transform player;
    public int speedMin = 15;
    public int speedMax = 15;
    int speed = 0;
    NavMeshAgent agent;
    public int rangeMin = 10;
    public int rangeMax = 10;
   int range = 0;
    private int layerMask;
    public float jumpDistance = 1.2f; // расстояние от центра объекта, до поверхности
    bool OnGround = false;
    public GameObject bullet;
    public GameObject gunPoint;
    public float timeout = 1f;
    public float ocheredTimeout = 0.5f;
    float curOcheredTimeout=0;
    float curTimeout=0;
    public int numberOfBulletsMin = 1;
    public int numberOfBulletsMax = 4;
    int numberOfBullets=1;
    int number;
    public int bulletSpeedMin = 25;
    public int bulletSpeedMax = 40;
    int bulletSpeed = 20;
    bool Shoot = false;
    AudioManager audio;

    // Start is called before the first frame update
    void Start()
    {
        speed = Random.Range(speedMin, speedMax);
        range = Random.Range(rangeMin, rangeMax);
        bulletSpeed = Random.Range(bulletSpeedMin, bulletSpeedMax);
        numberOfBullets = Random.Range(numberOfBulletsMin, numberOfBulletsMax);
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
        transform.LookAt(player);
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
        float distance = Vector3.Distance(transform.position, player.position);
        Debug.Log("Distance:" + distance);
        if (distance > range)
        {
            agent.isStopped = false;
            agent.SetDestination(player.position);
        }
        else
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
                if (curOcheredTimeout <= 0)
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
        look();
        CheckGround();
    }
    private void FixedUpdate()
    {
        moving();
    }
    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, range);
    }
}
