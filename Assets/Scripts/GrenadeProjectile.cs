using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GrenadeProjectile : MonoBehaviour
{
    public static event EventHandler OnAnyGrenadeExploded;
    
    [SerializeField] private Transform GrenadeExplodeVFX;
    [SerializeField] private TrailRenderer grenadeTrailRenderer;
    [SerializeField] private AnimationCurve positionYCurve;
    
    private Vector3 targetWorldPosition;
    private float moveSpeed = 10f;
    private float stoppingDistance = .2f;
    private Action onGrenadeComplete;
    private float radius = 4f;
    private float totalDistance;
    private Vector3 positionXZ;

    private void Update()
    {
        Vector3 moveDirection = (targetWorldPosition - positionXZ).normalized;
        
        if (Vector3.Distance(positionXZ, targetWorldPosition) > stoppingDistance )
        {   
            positionXZ += moveDirection * (moveSpeed * Time.deltaTime);

            float currentDistance = Vector3.Distance(positionXZ, targetWorldPosition);
            float currentDistanceNormalized = 1 - currentDistance / totalDistance;
            float maxHeight = totalDistance / 3f; 
            float positionY = positionYCurve.Evaluate(currentDistanceNormalized) * maxHeight;
            
            transform.position = new Vector3(positionXZ.x, positionY, positionXZ.z);
        }
        else
        {
            // ReSharper disable once Unity.PreferNonAllocApi
            Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
            foreach (Collider collider in colliders)
            {
                if (collider.TryGetComponent<Unit>(out Unit targetUnit))
                {
                    targetUnit.Damage(40);
                }

                if (collider.TryGetComponent<DestructibleCrate>(out DestructibleCrate destructibleCrate))
                {
                    destructibleCrate.Damage();
                }
            }
            
            OnAnyGrenadeExploded?.Invoke(this, EventArgs.Empty);
            grenadeTrailRenderer.transform.parent = null; 
            Instantiate(GrenadeExplodeVFX,transform.position + Vector3.up * 1f, Quaternion.identity);
            Destroy(this.gameObject);
            onGrenadeComplete();
        }
    }
    
    public void Setup(GridPosition targetGridPosition, Action onGrenadeComplete)
    {
        targetWorldPosition = LevelGrid.Instance.GetWorldPosition(targetGridPosition);
        this.onGrenadeComplete = onGrenadeComplete;
        
        positionXZ = transform.position;
        positionXZ.y = 0f;
        totalDistance = Vector3.Distance(positionXZ, targetWorldPosition);
    }
}
