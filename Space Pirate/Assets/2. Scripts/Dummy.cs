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

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update() {
        Debug.DrawRay(transform.position, player.position, Color.blue);
        if (Physics.CheckSphere(transform.position, chaseDistance, LayerMask.GetMask("Player")) && !Physics.Raycast(transform.position, player.position, LayerMask.GetMask("Ground"))) {
            if (!Physics.CheckSphere(transform.position, safeDistance, LayerMask.GetMask("Player"))) {
                Move(player.position);
            } else {
                Move(transform.position);
            }
        }
    }

    Vector3 offsetOfPlayer()
    {
        return player.position + new Vector3(Random.Range(-1, 1), 0, Random.Range(-1, 1)).normalized * safeDistance;
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
