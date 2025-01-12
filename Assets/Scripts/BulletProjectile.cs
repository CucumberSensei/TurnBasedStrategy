using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletProjectile : MonoBehaviour
{
    private Vector3 targetPosition;
    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private Transform bulletHitFVX;

    public void SetUp(Vector3 targetPosition)
    {
        this.targetPosition = targetPosition;       
    }

    private void Update()
    {   
        Vector3 moveDirection = (targetPosition - transform.position).normalized;


        float distanceBeforeMoving = Vector3.Distance(transform.position, targetPosition);

        float bulletSpeed = 200f;
        transform.position += moveDirection * Time.deltaTime * bulletSpeed;
        
        float distanceAfterMoving = Vector3.Distance(transform.position, targetPosition);

        if(distanceAfterMoving > distanceBeforeMoving)
        {
            transform.position = targetPosition;

            trailRenderer.transform.parent = null;

            Destroy(this.gameObject);
            Instantiate(bulletHitFVX, targetPosition, Quaternion.identity);
        }
    }
}
