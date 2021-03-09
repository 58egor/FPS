using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayShooting : MonoBehaviour
{
    public GameObject gunPoint;
    public float damage = 1f;
    public int targets = 1;
    public float timeout = 0.2f;
    private float curTimeout;
    public LineRenderer lineRenderer;
    // Start is called before the first frame update
    void Start()
    {
        //lineRenderer = GetComponent<LineRenderer>();
        curTimeout = timeout;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit[] hit;
        hit = Physics.RaycastAll(gunPoint.transform.position, gunPoint.transform.forward);
        Vector3[] points = new Vector3[2];
        lineRenderer.positionCount = points.Length;
        points[0] = gunPoint.transform.position;
        points[1] = hit[0].point;
        lineRenderer.SetPositions(points);
        if (Input.GetMouseButton(0) && curTimeout <= 0)
        {
            curTimeout = timeout;
            //RaycastHit[] hit;
            //hit = Physics.RaycastAll(gunPoint.transform.position, gunPoint.transform.forward);
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
    }
}
