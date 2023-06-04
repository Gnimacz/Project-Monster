using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float speed = 1f;
    [SerializeField] private float delay = 0.1f;
    [SerializeField] private float followDistance = 2f;
    private bool isFollowing = false;

    public bool IsFollowing { get => isFollowing; set => isFollowing = value; }

    private void Update()
    {
        if(isFollowing && target is not null) MoveTowardsTarget();
    }

    //move towards the target with a small delay and a given speed
    public void MoveTowardsTarget()
    {
        Vector3 LerpXY = Vector3.Lerp(transform.position, target.position - (target.transform.right * followDistance), speed * Time.deltaTime);
        transform.position = new Vector3(LerpXY.x, LerpXY.y, transform.position.z);
    }


    public void SetTarget(Transform target)
    {
        this.target = target;
    }
    #region overloads
    public void SetTarget(GameObject target)
    {
        this.target = target.transform;
    }

    public void SetTarget(Vector3 target)
    {
        this.target.position = target;
    }

    public void SetTarget(float x, float y, float z)
    {
        this.target.position = new Vector3(x, y, z);
    }

    public void SetTarget(float x, float y)
    {
        this.target.position = new Vector3(x, y, 0);
    }
    #endregion
}
