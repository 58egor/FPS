using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Climbing : MonoBehaviour
{
    Transform player;
    public float headRay = 1f;
    public float rayDistance = 1f;
    private int layerMask;
    bool bodySee = false;
    bool headSee = false;
    Vector3 newPos=new Vector3(0,0,0);
    Vector3 upPos = new Vector3(0, 0, 0);
    Vector3 lastPos= new Vector3(0, 0, 0);
    bool isUp = false;
    Rigidbody body;
    bool isClimbing = false;
    public float upSpeed = 12;
    float difX = 0;
    float difZ = 0;
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("коснулись стены");
    }
    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody>();
        player = transform;
        layerMask = 1 << gameObject.layer | 1 << 2;
        layerMask = ~layerMask;
    }
    void CheckClimbing()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, player.transform.forward);//создаем луч направленный вперед
        if (Physics.Raycast(ray, out hit, rayDistance, layerMask))//выпускаем луч определннеой длинны
        {
            Debug.Log(hit.collider.gameObject.name);
            if (hit.collider.gameObject.layer == 0)
            {
                bodySee = true;
                if (isClimbing)
                {
                    newPos.x = hit.point.x;
                    newPos.z = hit.point.z;
                }
            }
        }
        else 
        {
            bodySee = false;
        }
        Vector3 vec = transform.position;
        vec.y += headRay;
        ray = new Ray(vec, player.transform.forward);//создаем луч направленный вперед
        if (Physics.Raycast(ray, out hit, rayDistance, layerMask))//выпускаем луч определннеой длинны
        {
            Debug.Log(hit.collider.gameObject.name);
            if (hit.collider.gameObject.layer == 0)
            {
                headSee = true;
               
            }
                
        }
        else
        {
            if (!isClimbing)
            {
                newPos.y = vec.y + 2;
            }
            headSee = false;
        }
    }
    void StartClimbing()
    {
        if (!isUp)
        {
            if (player.position.y < newPos.y)
            {
                body.MovePosition(body.position + player.up * upSpeed * Time.fixedDeltaTime);//осуществялем передвижение вверх
                Debug.Log("Вверх."+ "Pos player:" + player.position.y + ",Y pos:" + newPos.y);

            }
            else
            {
                Debug.Log("Pos player:" + player.position.y + ",Y pos:"+newPos.y);
                Debug.Log("вперед готовимся");
                isUp = true;
                lastPos = body.position;
                difX = Vector3.Distance(newPos, body.position);
                Debug.Log("Difx:" + difX);
            }
        }
        else
        {
            Debug.Log("vpered");
            if (Vector3.Distance(newPos, body.position)>0.1)
            {
                Debug.Log("Dif:" + Vector3.Distance(newPos, body.position));
                lastPos = body.position;
                body.position=Vector3.MoveTowards(body.position,newPos, Time.fixedDeltaTime *upSpeed);//осуществялем передвижение вперед
            }
            else
            {
                isUp = false;
                isClimbing = false;//закончил вскарабкиваться 
                player.GetComponent<Control>().enabled = true;//возвращаем управление
                body.useGravity = true;//включаем гравитацию
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        CheckClimbing();
        if(bodySee && !headSee)
        {
            Debug.Log("Можем подниматься");
        }
        if(bodySee && !headSee && !GlobalInfo.CheckGround() && Input.GetAxisRaw("Vertical") > 0 && !isClimbing)//если в вохдухе и двигаемся вперед
        {//голова не видит стену,а тело видит то начинаем взбираться
            Debug.Log("Поднимаемся");
            upPos = player.position;
            upPos.y = newPos.y;
            isClimbing = true;//помечаем это
            body.useGravity = false;//отрубаем гравитаюци.
            player.GetComponent<Control>().enabled = false;//отбираем управление у игрока
            Debug.Log("NewPos:" + newPos);
        }
        
    }
    private void FixedUpdate()
    {
        if (isClimbing)
        {
            StartClimbing();//начинаем взбираться
        }
    }
    void OnDrawGizmosSelected() // подсветка, для визуальной настройки jumpDistance
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, player.transform.forward * rayDistance);
        Gizmos.color = Color.red;
        Vector3 vec = transform.position;
        vec.y += headRay;
        Gizmos.DrawRay(vec, player.transform.forward * rayDistance);
    }
}
