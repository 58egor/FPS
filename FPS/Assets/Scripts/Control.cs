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
    // Start is called before the first frame update
    void Start()
    {
        player = transform;
        body = GetComponent<Rigidbody>();
        body.freezeRotation = true;
        layerMask = 1 << gameObject.layer | 1 << 2;
        layerMask = ~layerMask;
        Debug.Log(layerMask);
    }
    public void Moving()
    {
        Vector3 forward = player.forward * movement.z;//двигаемся вперед/назад относительно того куда смотрит игрок
        Vector3 right = player.right * movement.x;//двигаемся влево/вправо относительно того куда смотрит игрок
        body.MovePosition(body.position + forward * speedMove * Time.fixedDeltaTime);//осуществялем передвижение
        body.MovePosition(body.position + right * speedMove * Time.fixedDeltaTime);//осуществялем передвижение
        //body.AddForce(movement * speedMove);
    }
    void Rotation()
    {//данная функция отвечает за поворот камеры(игрока)
        float rotationX = player.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivity;
        rotationY += Input.GetAxis("Mouse Y") * sensitivity;
        rotationY = Mathf.Clamp(rotationY, headMinY, headMaxY);
        player.localEulerAngles = new Vector3(0, rotationX, 0);
        cam.localEulerAngles = new Vector3(-rotationY, 0, 0);


    }
    //определение направления движения
    void Dir()
    {
        movement.x = Input.GetAxisRaw("Horizontal");//вперед и назад
        movement.z = Input.GetAxisRaw("Vertical");//влево и вправо
    }
    //функция прыжка
    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && GlobalInfo.ChechGround())//если нажали пробел и на земле
        {
            body.velocity = new Vector2(0, jumpForce);//пинаем вверх
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
    }
}
