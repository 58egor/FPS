using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Control : MonoBehaviour
{
    public float speedMove = 1.5f;
    public float jumpForce=5f;
    public float jumpDistance = 1.2f; // расстояние от центра объекта, до поверхности
    Transform player;
    Rigidbody body;
    Vector3 movement;
    public Transform cam;
    private float rotationY;
    public float sensitivity = 5f; // чувствительность мыши
    public float headMinY = -40f; // ограничение угла для головы
    public float headMaxY = 40f;
    private int layerMask;
    private Animator animator;
    public float timerPodkat = 1f;
    public float podkatSpeed = 15f;
    float speed;
    // Start is called before the first frame update
    void Start()
    {
        speed = speedMove;
        player = transform;
        body = GetComponent<Rigidbody>();
        body.freezeRotation = true;
        layerMask = 1 << gameObject.layer | 1 << 2;
        animator = GetComponentInChildren<Animator>();
        layerMask = ~layerMask;
        Debug.Log(layerMask);
    }
    public void Moving()
    {
        Debug.Log(player.forward);
        Vector3 forward = player.forward * movement.z;//двигаемся вперед/назад относительно того куда смотрит игрок
        Vector3 right = player.right * movement.x;//двигаемся влево/вправо относительно того куда смотрит игрок
        if (!GlobalInfo.CheckWallRun())
        {
            body.MovePosition(body.position + forward * speed * Time.fixedDeltaTime);//осуществялем передвижение
            Debug.Log("Move");
        }
        if (true/*GlobalInfo.CheckGround()*/) {
            body.MovePosition(body.position + right * speed * Time.fixedDeltaTime);//осуществялем передвижение
        }
        //body.AddForce(movement * speedMove);
    }
    void Rotation()
    {//данная функция отвечает за поворот камеры(игрока)
        float rotationX = player.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivity;
        player.localEulerAngles = new Vector3(0, rotationX, 0);
        rotationY += Input.GetAxis("Mouse Y") * sensitivity;
        rotationY = Mathf.Clamp(rotationY, headMinY, headMaxY);
        cam.localEulerAngles = new Vector3(-rotationY, 0, 0);
    }
    //определение направления движения
    void Dir()
    {
        movement.x = Input.GetAxisRaw("Horizontal");//влево и вправо
        movement.z = Input.GetAxisRaw("Vertical");//вперед и назад
        if (Input.GetAxisRaw("Vertical") > 0)
        {
            timerPodkat -= Time.deltaTime;
            if (timerPodkat <= 0)
            {
                Debug.Log("True");
                GlobalInfo.ChangePodkat(true);
            }
        }
        else
        {
            GlobalInfo.ChangePodkat(false);
            timerPodkat = 1f;
        }
    }
    //функция прыжка
    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && GlobalInfo.CheckGround())//если нажали пробел и на земле
        {
        body.velocity = new Vector2(0, jumpForce);//пинаем вверх
        //    if (Input.GetAxisRaw("Horizontal") ==0) {
        //        body.velocity = new Vector2(0, jumpForce);//пинаем вверх
        //    }
        //    else
        //    {
        //        if (Input.GetAxisRaw("Horizontal") > 0)
        //        {
        //            Vector3 vec = player.right;
        //            vec = new Vector3(vec.x * speedMove, jumpForce, vec.z * speedMove);
        //            body.velocity = vec;
        //        }
        //        else
        //        {
        //            Vector3 vec = -player.right;
        //            vec = new Vector3(vec.x * speedMove, jumpForce, vec.z * speedMove);
        //            body.velocity = vec;
        //        }

        //    }
            //GlobalInfo.ChangeGround(false);
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
        }
        else
        GlobalInfo.ChangeGround(false);//если нет то в воздухе
    }
    void SitDown()
    {
        if (Input.GetKey(KeyCode.C) && GlobalInfo.CheckGround())
        {
            if (GlobalInfo.ChecPodkat())
            {
                speed = podkatSpeed;
                Debug.Log("PodkatSpeed");
            }
            else
            {
                speed = speedMove;
                Debug.Log("MoveSpeed");
            }
            animator.SetBool("Sit", true);
        }
        else
        {
            speed = speedMove;
            animator.SetBool("Sit", false);
        }
    }
    // Update is called once per frame
    private void FixedUpdate()
    {
        Moving();//вызываем функцию передвижения
        Rotation();
    }

    void Update()
    {
        Dir();//вызываем функцию определения направления
        Jump();//функция вызова прыжка
        CheckGround();
        SitDown();
    }
    void OnDrawGizmosSelected() // подсветка, для визуальной настройки jumpDistance
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, Vector3.down * jumpDistance);
    }
}
