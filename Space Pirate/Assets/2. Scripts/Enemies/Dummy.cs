using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Dummy : MonoBehaviour
{
    NavMeshAgent agent;

    Transform player;

    [SerializeField] float safeDistance;
    [SerializeField] float chaseDistance;
    [SerializeField] float wanderRange;

    string state;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        state = "Wander";
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update() {
        Debug.DrawRay(transform.position, player.position, Color.blue);

        RaycastHit hit;
        Physics.Raycast(transform.position, player.position - transform.position, out hit);

        if (Physics.CheckSphere(transform.position, chaseDistance, LayerMask.GetMask("Player")) && hit.transform.CompareTag("Player")) {
            state = "Chase";
        } else {
            state = "Wander";
        }

        switch (state) {
            case "Fire":
                Fire();
                break;
            case "Melee":
                Melee();
                break;
            case "Chase":
                Chase();
                break;
            case "Wander":
            default:
                Wander();
                break;
        }
    }

    void Wander() {
        if (Time.frameCount % (60*5) == 0 && agent.isStopped) {
            agent.destination = transform.position + (new Vector3(Random.Range(-1,1), Random.Range(-1, 1), Random.Range(-1, 1)).normalized * wanderRange);
        }
    }

    void Chase()
    {
        if (!Physics.CheckSphere(transform.position, safeDistance, LayerMask.GetMask("Player")))
        {
            Move(player.position);
        }
        else
        {
            Move(transform.position);
        }
    }

    void Melee()
    {

    }

    void Fire()
    {

    }

    public void Move(Vector3 point) {
        agent.destination = point;
        agent.isStopped = false;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, safeDistance);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseDistance);
    }
}
