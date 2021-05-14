using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Control : MonoBehaviour
{
    public float speedMove = 1.5f;
    public float jumpForce=5f;
    public float jumpDistance = 1.2f; // расстояние от центра объекта, до поверхности
    public Transform player;
    Transform cam;
    Rigidbody body;
    Vector3 movement;
    public Transform point;
    public float rotationY;
    public float sensitivity = 5f; // чувствительность мыши
    public float headMinY = -40f; // ограничение угла для головы
    public float headMaxY = 40f;
    private int layerMask;
    private Animator animator;
    public float longOfPodkat = 1f;
    public float timerPodkat = 1f;
    float timer1, timer2;
    public float podkatSpeed = 15f;
    float speed;
    bool isSitDown = false;
    public int nJumps = 1;
    public int jumps = 0;
    public bool isShooting = false;
    public float otdachaX=0;
    public float otdachaY=0;
    public float DashTime = 0.5f;
    float CurrentDashTime = 0;
    float dashUp;
    public float DashSpeed = 5;
    public float dashCd=5;
    public int MaxDash = 2;
    int dashEnabled;
    bool DashActive = false;
    RigidbodyConstraints contains;
    RigidbodyConstraints contains2;
    AudioManager audio;
    Vector3 dashTraectory;
    // Start is called before the first frame update
    void Start()
    {

        timer1 = longOfPodkat;
        timer2 = timerPodkat;
        speed = speedMove;
        player = transform;
        body = GetComponent<Rigidbody>();
        body.freezeRotation = true;
        contains = body.constraints;
        contains2 = body.constraints | RigidbodyConstraints.FreezePositionY;
        layerMask = 1 << gameObject.layer | 1 << 2;
        animator = GetComponentInChildren<Animator>();
        layerMask = ~layerMask;
        dashEnabled = MaxDash;
        audio = GetComponent<AudioManager>();
        dashUp = dashCd;
        Debug.Log(layerMask);
    }
    public void Moving()
    {

        //Debug.Log(player.forward);
        Vector3 forward = player.forward * movement.z;//двигаемся вперед/назад относительно того куда смотрит игрок
        Vector3 right = player.right * movement.x;//двигаемся влево/вправо относительно того куда смотрит игрок
        if (!GlobalInfo.CheckWallRun())
        {
            if (GlobalInfo.CheckGround())
            {
                body.MovePosition(body.position + forward * speed * Time.fixedDeltaTime);//осуществялем передвижение вперед/назад
               
            }
            else
            {
                body.MovePosition(body.position + forward * speedMove / 2 * Time.fixedDeltaTime);//осуществялем передвижение вперед/назад
            }
            //Debug.Log("Move");
        }
        else
        {
            jumps = 0;
        }
        if (GlobalInfo.CheckGround())
        {
            body.MovePosition(body.position + right * speed * Time.fixedDeltaTime);//осуществялем передвижение влево/вправо
        }
        else
        {
            body.MovePosition(body.position + right * speedMove /2 * Time.fixedDeltaTime);//осуществялем передвижение влево/вправо
        }
    }
    void Dash()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && DashActive == false && dashEnabled!=0)
        {
            
            body.constraints = contains2;
            Debug.Log("Dash");
            Vector3 vec = new Vector3(0, 0, 0);
            float hor = Input.GetAxisRaw("Horizontal");
            float ver = Input.GetAxisRaw("Vertical");
                if (ver != 0)
                {
                    vec = transform.forward * ver;
                }
                if (hor != 0)
                {
                    vec += (transform.right * hor);
                }
                if (Mathf.Abs(hor) + Mathf.Abs(ver) != 0)
                {
                    vec = vec / (Mathf.Abs(hor) + Mathf.Abs(ver));
                }
                dashTraectory = vec;
                dashEnabled--;
            audio.Play("Dash");
            body.velocity = dashTraectory * DashSpeed;
            CurrentDashTime = DashTime;
            DashActive = true;
        }
        if (DashActive)
        {
            if (CurrentDashTime > 0)
            {
                CurrentDashTime -= Time.deltaTime;
            }
            else
            {
                body.velocity = new Vector3(0, 0, 0);
                body.constraints = contains;
                DashActive = false;
            }
        }
        if (dashEnabled < MaxDash)
        {
            if (dashUp > 0)
            {
                dashUp -= Time.deltaTime;
            }
            else
            {
                Debug.Log("DashAdd");
                dashUp = dashCd;
                dashEnabled++;
            }
        }
    }
    void Rotation()
    {//данная функция отвечает за поворот камеры(игрока). В данном случае камера повторяет координаты за точкой прикреплонной к игроку
        float rotationX = (player.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivity) + otdachaX;
        player.localEulerAngles = new Vector3(0, rotationX, 0);
        rotationY += ((Input.GetAxis("Mouse Y")) * sensitivity)+otdachaY;
        rotationY = Mathf.Clamp(rotationY, headMinY, headMaxY);
        point.localEulerAngles = new Vector3(-rotationY, 0, 0);
    }
    //определение направления движения
    void Dir()
    {
        movement.x = Input.GetAxisRaw("Horizontal");//влево и вправо
        movement.z = Input.GetAxisRaw("Vertical");//вперед и назад
        if (Input.GetAxisRaw("Vertical") > 0)
        {
            if (!isSitDown)
            {
                timer2 -= Time.deltaTime;
                if (timer2 <= 0)
                {
                    Debug.Log("True");
                    GlobalInfo.ChangePodkat(true);
                }
            }
        }
        else
        {
            GlobalInfo.ChangePodkat(false);
            timer2=timerPodkat;
        }
    }
    //функция прыжка
    void Jump()
    {
        Vector3 vec = new Vector3(0, jumpForce, 0);
        if (Input.GetAxisRaw("Vertical") > 0)
        {
            vec.x = player.forward.x * speedMove/2;
            vec.z = player.forward.z * speedMove/2;
        }
        else
        {
            if (Input.GetAxisRaw("Vertical") != 0)
            {
                vec.x = -player.forward.x * speedMove/2;
                vec.z = -player.forward.z * speedMove/2;
            }
        }
        if(Input.GetAxisRaw("Horizontal") > 0)
        {
            vec.x = (vec.x + player.right.x * speedMove/2) / 2;
            vec.z = (vec.z + player.right.z * speedMove/2) / 2;
        }
        else
        {
            if (Input.GetAxisRaw("Horizontal") != 0)
            {
                vec.x = (vec.x + (-player.right.x * speedMove/2)) / 2;
                vec.z = (vec.z + (-player.right.z * speedMove/2)) / 2;
            }
        }
        if (Input.GetKeyDown(KeyCode.Space) && (GlobalInfo.CheckGround() && !GlobalInfo.CheckWallRun()))//если нажали пробел и на земле
        {
        Debug.Log("Kick");
        body.velocity = vec;//пинаем вверх
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space) &&  jumps != nJumps && !GlobalInfo.CheckWallRun())
            {
                body.velocity = vec;//пинаем вверх
                jumps++;
            }
        }
    }
    //проверяем на земле ли
    void CheckGround()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, Vector3.down);//создаем луч направленный вниз
        if (Physics.Raycast(ray, out hit, jumpDistance, layerMask))//выпускаем луч определннеой длинны
        {
            GlobalInfo.ChangeGround(true);//если попали во что то то стоим на чем то
            jumps = 0;
        }
        else
        GlobalInfo.ChangeGround(false);//если нет то в воздухе
    }
    void SitDown()
    {
        if (Input.GetKey(KeyCode.C) && GlobalInfo.CheckGround())
        {
            if (GlobalInfo.ChecPodkat() && timer1>0)
            {
                timer1 -= Time.deltaTime;//подкат
                speed = podkatSpeed;
                Debug.Log("PodkatSpeed");
            }
            else
            {
                speed = speedMove/2;//сидим
                Debug.Log("MoveSpeed");
            }
            animator.SetBool("Sit", true);
            isSitDown = true;
        }
        else
        {
            bool upDown = false;
            if (isSitDown)
            {
                RaycastHit hit;
                Ray ray = new Ray(transform.position, Vector3.up*1);//создаем луч направленный вверх
                if (Physics.Raycast(ray, out hit, 1, layerMask))//выпускаем луч определннеой длинны
                {
                    upDown=true;
                }
                else
                    upDown = false;
            }
            if (!upDown)
            {
                timer1 = longOfPodkat;
                speed = speedMove;
                animator.SetBool("Sit", false);
                isSitDown = false;
            }
        }
    }
    // Update is called once per frame
    private void FixedUpdate()
    {
       if(!DashActive)
        Moving();//вызываем функцию передвижения
        Rotation();
    }

    void Update()
    {
        
        //otdachaY = GlobalInfo.otdachaY;
        Dir();//вызываем функцию определения направления
        Jump();//функция вызова прыжка
        CheckGround();
        SitDown();
        Dash();
    }
    void OnDrawGizmosSelected() // подсветка, для визуальной настройки jumpDistance
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, Vector3.down * jumpDistance);
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, Vector3.up * 1);
    }
}
