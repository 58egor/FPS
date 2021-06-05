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
    Animator animator;
    public int procentOfSound = 5;
    AudioManager audio;
    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody>();
        body.freezeRotation = true;
        agent=GetComponent<NavMeshAgent>();
        player = GameObject.Find("Player").transform;
        animator = transform.GetChild(0).GetComponent<Animator>();
        audio = GetComponent<AudioManager>();
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
        if (hit.collider != null)
        {
            if (hit.transform.gameObject.layer == 8)
            {
                if (Random.Range(0, 100) < procentOfSound)
                {
                    if (!audio.isPlaying("Hit"))
                    {
                        audio.Play("Hit");
                    }
                }
                animator.SetBool("Hit", true);
                Debug.Log("Player hit");
            }
            else
            {
                animator.SetBool("Hit", false);
            }
        }
        else
        {
            animator.SetBool("Hit", false);
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
    private void OnDisable()
    {
        animator.SetBool("Hit", false);
    }
}
