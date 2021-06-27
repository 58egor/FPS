using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MovingEnemy : MonoBehaviour
{
    Rigidbody body;
    Transform player;
    public float speed = 15f;
    NavMeshAgent agent;
    public float range = 10f;
    Animator animator;
    public int procentOfSound = 5;
    AudioManager audio;
    bool OnGround = false;
    private int layerMask;
    public float jumpDistance = 1.2f; // расстояние от центра объекта, до поверхности
    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody>();
        body.freezeRotation = true;
        agent=GetComponent<NavMeshAgent>();
        player = GameObject.Find("Player").transform;
        animator = transform.GetChild(0).GetComponent<Animator>();
        audio = GetComponent<AudioManager>();
        audio.Play("Charge");
        speed = Random.Range(4, 13);//рандомим скорость
        agent.speed = speed;
        layerMask = 1 << gameObject.layer | 1 << 2;
        layerMask = ~layerMask;
    }
    void look()
    {
        transform.LookAt(player);//смотрим на героя
    }
    void moving()
    {
        //Vector3 forward = transform.forward;
        //body.MovePosition(body.position + forward* speed * Time.fixedDeltaTime);
        float distance = Vector3.Distance(transform.position, player.position);// получаем дистанцию
        Debug.Log("Distance:" + distance);
        if (distance > 1.2f)//если не достигли
        {
            agent.SetDestination(player.position);//то идем на героя
        }
    }
    void check()//проверяем рядом ли герой
    {
        RaycastHit hit;
        Physics.Raycast(transform.position,transform.forward, out hit,range);//пускаем луч на определенную дистанцию
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) *range, Color.yellow);
        if (hit.collider != null)//если есть попадание
        {
            if (hit.transform.gameObject.layer == 8)//и попали по герою
            {
                if (Random.Range(0, 100) < procentOfSound)
                {
                    if (!audio.isPlaying("Hit"))
                    {
                        audio.Play("Hit");
                    }
                }
                animator.SetBool("Hit", true);//то запускаем анимацию ударов
                Debug.Log("Player hit");
            }
            else//если нет
            {
                animator.SetBool("Hit", false);//то выключаем
            }
            Debug.Log("Distance:" + hit.distance);
        }
        else
        {
            animator.SetBool("Hit", false);
        }

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
    // Update is called once per frame
    void Update()
    {
        look();
        check();
        CheckGround();
       //moving();
    }
    private void FixedUpdate()
    {
        
        Debug.Log("agent:" + agent.isOnNavMesh);
        if (agent.isOnNavMesh)
        {
            moving();
        }
        else
        {
            agent.enabled = false;
        }
    }
    private void OnDisable()
    {
        animator.SetBool("Hit", false);
    }
}
