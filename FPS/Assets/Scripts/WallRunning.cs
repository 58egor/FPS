using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRunning : MonoBehaviour
{
    Transform player;
    Rigidbody body;
    public float jumpForce = 5f;
    float timer = 0f;
    bool parkourAvailableRight = false;
    bool parkourAvailableLeft = false;
    public float wallDistance = 0.5f;
    private int layerMask;
    private Animator animator;
    public GameObject cam;
    float ret = 10f;
    // Start is called before the first frame update
    void Start()
    {
        player = transform;
        body = GetComponent<Rigidbody>();
        layerMask = 1 << gameObject.layer | 1 << 2;
        layerMask = ~layerMask;
        animator = GetComponentInChildren<Animator>();
    }
    //пускаем лучи что бы понять есть ли стена справа и слева
    void WallRun()
    {
        if(Input.GetAxisRaw("Vertical")>0 && (parkourAvailableRight || parkourAvailableLeft) && !GlobalInfo.ChechGround())//если бежим вперед, около стены и в вохдухе
        {
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
                    if (vec.x == 0)
                    {
                        vec.x = vec.x * jumpForce;//если лево/право на оси х
                    }
                    else
                    {
                        vec.z = vec.z * jumpForce;//если лево/право на оси у
                    }
                    vec.y = jumpForce;//прыжок не только в сторону но и вверх
                    Debug.Log("vec=" + vec);
                    body.velocity = vec;//делаем толчок
            }
            else
            {
                if (timer <= 0)
                {
                    if (parkourAvailableLeft)
                    {
                        animator.SetBool("WallLeft", true);
                    }
                    else
                    {
                        animator.SetBool("WallRight", true);
                    }
                    body.velocity = new Vector2(0, 0);//то значит бежим по стене
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
                Debug.Log("Справа стена");
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
                Debug.Log("Cлева стена");
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
     //   WallRun();
        
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
