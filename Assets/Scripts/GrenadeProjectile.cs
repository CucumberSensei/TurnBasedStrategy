using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeProjectile : MonoBehaviour
{   
    private Vector3 targetWorldPosition;
    private float moveSpeed = 10f;
    private float stoppingDistance = .2f;
    private Action onGranadeComplete;
    private float radius = 4f;

    private void Update()
    {
        Vector3 moveDirection = (targetWorldPosition - transform.position).normalized;
        
        if (Vector3.Distance(transform.position, targetWorldPosition) > stoppingDistance )
        {
            transform.position += moveDirection * (moveSpeed * Time.deltaTime);
        }
        else
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
            foreach (Collider collider in colliders)
            {
                if (collider.TryGetComponent<Unit>(out Unit targetUnit))
                {
                    targetUnit.Damage(40);
                }
            }
            Destroy(this.gameObject);
            onGranadeComplete();
        }
    }
    
    public void Setup(GridPosition targetGridPosition, Action onGranadeComplete)
    {
        targetWorldPosition = LevelGrid.Instance.GetWorldPosition(targetGridPosition);
        this.onGranadeComplete = onGranadeComplete;
    }
}
