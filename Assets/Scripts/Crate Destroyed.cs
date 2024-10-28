using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateDestroyed : MonoBehaviour
{
    [SerializeField] private Transform rootTransform;
    void Start()
    {   
        Vector3 randomDir = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
        ApplyExplosionToParts(rootTransform, 300f, transform.position + randomDir, 10f);
    }
    
    
    private void ApplyExplosionToParts(Transform root, float explosionForce, Vector3 explosionPosition, float explosionRadius)
    {
        foreach (Transform child in root)
        {
            if(child.TryGetComponent<Rigidbody>(out Rigidbody rigidbody))
            {
                rigidbody.AddExplosionForce(explosionForce, explosionPosition, explosionRadius);
                
            }

            ApplyExplosionToParts(child,  explosionForce,  explosionPosition, explosionRadius);
        }
    }
}
