using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UnitAnimator : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Transform bulletProjectileTransform;
    [SerializeField] private Transform bulletSpawnPoint;

    private const string IS_WALKING = "IsWalking";
    private const string SHOOT = "Shoot";

    private void Awake()
    {
        if(TryGetComponent<MoveAction>(out MoveAction moveAction))
        {
            moveAction.OnMoveStart += MoveAction_OnMoveStart;
            moveAction.OnMoveEnd += MoveAction_OnMoveEnd;
        }

        if (TryGetComponent<ShootAction>(out ShootAction shootAction))
        {
            shootAction.OnShootStart += ShootAction_OnShootStart;
        }

    }

    private void ShootAction_OnShootStart(object sender, ShootAction.OnShootStartEventArgs e)
    {
        animator.SetTrigger(SHOOT);
        Transform bullet =Instantiate(bulletProjectileTransform, bulletSpawnPoint);
        BulletProjectile bulletProjectile = bullet.GetComponent<BulletProjectile>();

        Vector3 targetUnitShootAtPosition = e.targetUnit.GetWorldPosition();
        targetUnitShootAtPosition.y = bulletSpawnPoint.position.y;

        bulletProjectile.SetUp(targetUnitShootAtPosition);
    }

    private void MoveAction_OnMoveStart(object sender, System.EventArgs e)
    {
        animator.SetBool(IS_WALKING, true);
    }

    private void MoveAction_OnMoveEnd(object sender, System.EventArgs e)
    {
        animator.SetBool(IS_WALKING, false);
    }
}
