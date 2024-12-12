using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ragdoll : MonoBehaviour
{
    [SerializeField] private Transform ragdollRootBone;

    public void SetUp(Transform originalRootBone)
    {
        MatchAllChildTransform(originalRootBone, ragdollRootBone);
        Vector3 randomDir = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
        ApplyExplosionToRagdoll(ragdollRootBone, 200f, transform.position + randomDir, 10f);
    }

    private void MatchAllChildTransform(Transform root, Transform clone)
    {
        foreach (Transform child in root)
        {
            Transform cloneChild = clone.Find(child.name);

            if(cloneChild != null)
            {
                cloneChild.transform.position = child.position;
                cloneChild.transform.rotation = child.rotation;
            }

            if (cloneChild != null)
            {
                MatchAllChildTransform(child, cloneChild);
            }
            
        }
    }

    private void ApplyExplosionToRagdoll(Transform root, float explosionForce, Vector3 explosionPosition, float explosionRadius)
    {
        foreach (Transform child in root)
        {
            if(child.TryGetComponent<Rigidbody>(out Rigidbody rigidbody))
            {
                rigidbody.AddExplosionForce(explosionForce, explosionPosition, explosionRadius);
                
            }

            ApplyExplosionToRagdoll(child,  explosionForce,  explosionPosition, explosionRadius);
        }
    }
}
