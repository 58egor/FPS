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
    
    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody>();
        body.freezeRotation = true;
        agent=GetComponent<NavMeshAgent>();
        player = GameObject.Find("Player").transform;
    }
    void look()
    {
        transform.LookAt(player);
    }
    void moving()
    {
        //Vector3 forward = transform.forward;
        //body.MovePosition(body.position + forward* speed * Time.fixedDeltaTime);
        agent.SetDestination(player.position);
    }
    void check()
    {
        RaycastHit hit;
        Physics.Raycast(transform.position,transform.forward, out hit,range);
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) *range, Color.yellow);
        if (hit.transform.gameObject.layer == 8)
        {
            Debug.Log("Player hit");
        }
    }
    // Update is called once per frame
    void Update()
    {
        look();
        check();
       //moving();
    }
    private void FixedUpdate()
    {
        moving();
    }
}
