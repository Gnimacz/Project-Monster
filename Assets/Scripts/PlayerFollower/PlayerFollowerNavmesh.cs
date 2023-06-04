using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerFollowerNavmesh : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float speed = 1f;
    [SerializeField] private float delay = 0.1f;
    [SerializeField] private float followDistance = 2f;
    private SphereCollider targetCollider;
    private bool isFollowing = false;
    [SerializeField] private NavMeshAgent agent;

    public bool IsFollowing { get => isFollowing; set => isFollowing = value; }
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        targetCollider = GetComponent<SphereCollider>();
        agent.speed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        if(isFollowing && target is not null) MoveTowardsTarget();
    }

    void MoveTowardsTarget()
    {
        agent.SetDestination(target.position - (target.transform.right * followDistance) + (target.transform.up * targetCollider.bounds.extents.y/2));
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }
}
