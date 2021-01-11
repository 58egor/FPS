using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRunning : MonoBehaviour
{
    Transform player;
    Rigidbody body;
    Vector3 wallRunVec;
    public float jumpForce = 5f;
    float timer = 0f;
    bool parkourAvailableRight = false;
    bool parkourAvailableLeft = false;
    public float wallDistance = 0.5f;
    private int layerMask;
    private Animator animator;
    public GameObject cam;
    float ret = 10f;
    float speedMove;
    public float degree = 45;
    // Start is called before the first frame update
    void Start()
    {
        player = transform;
        body = GetComponent<Rigidbody>();
        layerMask = 1 << gameObject.layer | 1 << 2;
        layerMask = ~layerMask;
        animator = GetComponentInChildren<Animator>();
        speedMove = player.GetComponent<Control>().speedMove;
        Debug.Log("Speed=" + speedMove);
    }
    //пускаем лучи что бы понять есть ли стена справа и слева
    void WallRun()
    {
        if(Input.GetAxisRaw("Vertical")>0 && (parkourAvailableRight || parkourAvailableLeft) && !GlobalInfo.CheckGround())//если бежим вперед, около стены и в вохдухе
        {
            GlobalInfo.ChangePodkat(true);
            if (Input.GetKeyDown(KeyCode.Space))//если нам надо оттолкнуться от стены во время
            {//бега по стенам
                Vector3 vec;//вектор прыжка
                timer = 0.2f;//отключаем бег по стена на 0.2
                Debug.Log(player.transform.right);
                if (parkourAvailableLeft)
                {
                    vec = player.transform.right;//если надо отпрыгнуть врпаво
                }
                else
                {
                    vec = -player.transform.right;//если надо отпрыгнуть влево
                }
                vec = vec * jumpForce;
                //vec.y = jumpForce;//прыжок не только в сторону но и вверх
                Debug.Log("vec=" + vec);
                Vector3 vec2 = new Vector3(0, jumpForce, 0);
                if (Input.GetAxisRaw("Vertical") > 0)
                {
                    vec2.x = player.forward.x * speedMove / 2;
                    vec2.z = player.forward.z * speedMove / 2;
                }
                else
                {
                    if (Input.GetAxisRaw("Vertical") != 0)
                    {
                        vec2.x = -player.forward.x * speedMove / 2;
                        vec2.z = -player.forward.z * speedMove / 2;
                    }
                }
                body.velocity = vec+vec2;//делаем толчок
            }
            else
            {
                if (timer <= 0)
                {
                    GlobalInfo.ChangeWallRun(true);
                    if (parkourAvailableLeft)
                    {
                        animator.SetBool("WallLeft", true);
                    }
                    else
                    {
                        animator.SetBool("WallRight", true);
                    }
                    body.velocity = new Vector2(0, 0);//то значит бежим по стене
                    body.MovePosition(body.position + wallRunVec * speedMove * Time.deltaTime);//осуществялем передвижение
                }
                else
                {
                    timer -= Time.deltaTime;
                }
            }
            Debug.Log("Бежим по стене");
        }
        else
        {
            GlobalInfo.ChangeWallRun(false);
            animator.SetBool("WallLeft", false);
            animator.SetBool("WallRight", false);
        }
    }
    void RayRight()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, player.transform.right);//создаем луч направленный вниз
        if (Physics.Raycast(ray, out hit, wallDistance, layerMask))//выпускаем луч определннеой длинны
        {
            Debug.Log(hit.collider.gameObject.layer);
            if (hit.collider.gameObject.layer == 0)//если это стена
            {
                if (!parkourAvailableRight)
                {
                    wallRunVec = player.forward;
                }
                parkourAvailableRight = true;//то бег по стенам возможен
                Debug.Log("Right1");
            }
            else
            {
                parkourAvailableRight = false;//иначе нет
            }

        }
        else
        {
            parkourAvailableRight = false;//иначе нет
        }
        if (!parkourAvailableRight)
        {
            Vector3 vec = player.transform.right * wallDistance;
            Vector3 vec2 = new Vector3(0, 0, 0);
            vec2.z = vec.z * Mathf.Cos(degree) - vec.x * Mathf.Sin(degree);
            vec2.x = vec.x * Mathf.Cos(degree) + vec.z * Mathf.Sin(degree);
            ray = new Ray(transform.position, vec2);//создаем луч направленный вниз
            if (Physics.Raycast(ray, out hit, wallDistance, layerMask))//выпускаем луч определннеой длинны
            {
                Debug.Log(hit.collider.gameObject.layer);
                if (hit.collider.gameObject.layer == 0)//если это стена
                {
                    Debug.Log("vec2 is working=" + hit.collider.transform.forward);
                    if (!parkourAvailableRight)
                    {
                        wallRunVec = player.forward;
                    }
                    parkourAvailableRight = true;//то бег по стенам возможен
                    Debug.Log("Right2");
                }
                else
                {
                    parkourAvailableRight = false;//иначе нет
                }

            }
        }
    }
    void WallRunningRay()
    {
        //луч влево от персонажа
        RaycastHit hit;
        Ray ray = new Ray(transform.position,-player.transform.right);//создаем луч направленный вниз
        if (Physics.Raycast(ray, out hit, wallDistance, layerMask))//выпускаем луч определннеой длинны
        {
            Debug.Log(hit.collider.gameObject.layer);
            if (hit.collider.gameObject.layer == 0)//если это стена
            {
                Debug.Log("vec="+ hit.transform.forward);
                if (!parkourAvailableLeft)
                {
                    wallRunVec = player.forward;
                }
                parkourAvailableLeft = true;//то бег по стенам возможен
                Debug.Log("Left1");
            }
            else
            {
                parkourAvailableLeft = false;
            }

        }
        else
        {
            parkourAvailableLeft = false;
        }
        if (!parkourAvailableLeft)
        {
            Vector3 vec = player.transform.right * -wallDistance;
            Vector3 vec5 = new Vector3(0, 0, 0);
            vec5.x = vec.x * Mathf.Cos(degree) - vec.z * Mathf.Sin(degree);
            vec5.z = vec.z * Mathf.Cos(degree) + vec.x * Mathf.Sin(degree);
            ray = new Ray(transform.position, vec5);//создаем луч направленный вниз
            if (Physics.Raycast(ray, out hit, wallDistance, layerMask))//выпускаем луч определннеой длинны
            {
                Debug.Log(hit.collider.gameObject.layer);
                if (hit.collider.gameObject.layer == 0)//если это стена
                {
                    Debug.Log("vec2 is working=" + hit.collider.transform.forward);

                    if (!parkourAvailableLeft)
                    {
                        wallRunVec = player.forward;
                    }
                    parkourAvailableRight = true;//то бег по стенам возможен
                    Debug.Log("Left2");
                }
                else
                {
                    parkourAvailableRight = false;//иначе нет
                }

            }
        }
    }
    // Update is called once per frame
    private void FixedUpdate()
    {
        
    }
    void Update()
    {
        RayRight();
        WallRun();
        WallRunningRay();

    }
    void OnDrawGizmosSelected() // подсветка, для визуальной настройки jumpDistance
    {
        Gizmos.color = Color.red;
        Vector3 vec = player.transform.right*wallDistance;
        Vector3 vec2=new Vector3(0,0,0);
        vec2.z = vec.z * Mathf.Cos(degree) - vec.x * Mathf.Sin(degree);
        vec2.x = vec.x * Mathf.Cos(degree) + vec.z * Mathf.Sin(degree);
       // Vector3 vec3 = new Vector3(0, 0, 0);
       // vec3.x = vec.x * Mathf.Cos(degree) - vec.z * Mathf.Sin(degree);
       // vec3.z = vec.z * Mathf.Cos(degree) + vec.x * Mathf.Sin(degree);
        vec = player.transform.right * -wallDistance;
        Vector3 vec4 = new Vector3(0, 0, 0);
        vec4.z = vec.z * Mathf.Cos(degree) - vec.x * Mathf.Sin(degree);
        vec4.x = vec.x * Mathf.Cos(degree) + vec.z * Mathf.Sin(degree);
        Vector3 vec5 = new Vector3(0, 0, 0);
        vec5.x = vec.x * Mathf.Cos(degree) - vec.z * Mathf.Sin(degree);
        vec5.z = vec.z * Mathf.Cos(degree) + vec.x * Mathf.Sin(degree);
        Gizmos.DrawRay(transform.position, (vec2));
       // //Gizmos.DrawRay(transform.position, (vec3));
        //Gizmos.DrawRay(transform.position, (vec4));
        Gizmos.DrawRay(transform.position, (vec5));
       // Gizmos.DrawRay(transform.position, player.transform.right * wallDistance);
       // Gizmos.DrawRay(transform.position, player.transform.right * -wallDistance);
    }
}
