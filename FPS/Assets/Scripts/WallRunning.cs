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
                     vec = -player.transform.right;//если надо отпрыгнуть влело
                }
                vec = vec * jumpForce;
                vec.y = jumpForce;//прыжок не только в сторону но и вверх
                Debug.Log("vec=" + vec);
                body.velocity = vec;//делаем толчок
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
            //Debug.Log("Бежим по стене");
        }
        else
        {
            GlobalInfo.ChangeWallRun(false);
            animator.SetBool("WallLeft", false);
            animator.SetBool("WallRight", false);
        }
    }
    void WallRunningRay()
    {
        //луч вправо от персонажа
        RaycastHit hit;
        Ray ray = new Ray(transform.position, player.transform.right);//создаем луч направленный вниз
        if (Physics.Raycast(ray, out hit, wallDistance, layerMask))//выпускаем луч определннеой длинны
        {
            Debug.Log(hit.collider.gameObject.layer);
            if (hit.collider.gameObject.layer == 0)//если это стена
            {
                Debug.Log("vec=" + hit.collider.transform.forward);
                
                wallRunVec = transform.forward;
                if (wallRunVec.x < 0)
                {
                    wallRunVec.x = -hit.transform.forward.z;
                }
                else
                {
                    wallRunVec.x = hit.transform.forward.z;
                }
                if (wallRunVec.z < 0)
                {
                    wallRunVec.z = -hit.transform.forward.x;
                }
                else
                {
                    wallRunVec.z = hit.transform.forward.x;
                }
                parkourAvailableRight = true;//то бег по стенам возможен
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
        //луч влево от персонажа
        ray = new Ray(transform.position,-player.transform.right);//создаем луч направленный вниз
        if (Physics.Raycast(ray, out hit, wallDistance, layerMask))//выпускаем луч определннеой длинны
        {
            Debug.Log(hit.collider.gameObject.layer);
            if (hit.collider.gameObject.layer == 0)//если это стена
            {
                Debug.Log("vec="+ hit.transform.forward);
                wallRunVec = transform.forward;
                if (wallRunVec.x < 0)
                {
                    wallRunVec.x = -hit.transform.forward.z;
                }
                else
                {
                    wallRunVec.x = hit.transform.forward.z;
                }
                if (wallRunVec.z < 0)
                {
                    wallRunVec.z = -hit.transform.forward.x;
                }
                else
                {
                    wallRunVec.z = hit.transform.forward.x;
                }
                parkourAvailableLeft = true;//то бег по стенам возможен
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
    }
    // Update is called once per frame
    private void FixedUpdate()
    {
        
    }
    void Update()
    {
        WallRun();
        WallRunningRay();

    }
    void OnDrawGizmosSelected() // подсветка, для визуальной настройки jumpDistance
    {
        Gizmos.color = Color.red;
       // Gizmos.DrawRay(transform.position, player.transform.right * wallDistance);
       // Gizmos.DrawRay(transform.position, player.transform.right * -wallDistance);
    }
}
