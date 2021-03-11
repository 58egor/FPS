using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayShooting : MonoBehaviour
{
    public float damage = 1f;
    public int targets = 1;
    public float timeout = 0.2f;
    public int maxAmmo=30;
    private bool isReload = false;
    public float reloadTimer;
    private float curReloadTimer;
    private int currentAmmo;
    private float curTimeout;
    private Camera camera;
    // Start is called before the first frame update
    void Start()
    {
        curTimeout = timeout;
        camera = Camera.main;
        currentAmmo = maxAmmo;
        curReloadTimer = reloadTimer;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0) && curTimeout <= 0 && currentAmmo>0)
        {
            currentAmmo--;
            Vector3 screenCneter = new Vector3(Screen.width / 2, Screen.height / 2, 0);//определяем центр камеры
            Ray ray = camera.ScreenPointToRay(screenCneter);
            curTimeout = timeout;
            RaycastHit[] hit;
            hit = Physics.RaycastAll(ray);
            for (int i = 0; i < hit.Length; i++)
            {
                if (i != targets)
                {
                    Debug.Log("Damage:" + hit[i].collider.name);
                }
                else
                {
                    break;
                }
            }
        }
        else
        {
            curTimeout -= Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.R) && !isReload && !(currentAmmo==maxAmmo))
        {
            Debug.Log("ActiveReload");
            isReload = true;
        }
        CheckAmmo();//функция отвечающая за перезарядку

    }
    public void CheckAmmo() {
        
        if (currentAmmo <= 0 || isReload)//если патронов нету
        {Debug.Log("Reload:"+curReloadTimer);
            if (curReloadTimer < 0)
            {
                isReload = false;
                currentAmmo = maxAmmo;
                curReloadTimer = reloadTimer;
            }
            else
            {
                curReloadTimer -= Time.deltaTime;
            }
        }
    }
    void OnDrawGizmosSelected() // подсветка, для визуальной настройки jumpDistance
    {
        Gizmos.color = Color.red;
        Vector3 screenCneter = new Vector3(Screen.width / 2, Screen.height / 2, 0);//определяем центр камеры
        Gizmos.DrawRay(camera.transform.position, camera.transform.forward);
        Gizmos.color = Color.red;

    }
}
