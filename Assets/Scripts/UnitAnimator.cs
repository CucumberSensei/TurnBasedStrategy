using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UnitAnimator : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Transform bulletProjectileTransform;
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private Transform rifleTransform;
    [SerializeField] private Transform swordTransform;
    
    private const string IS_WALKING = "IsWalking";
    private const string SHOOT = "Shoot";
    private const string SWORD_SLASH = "SwordSlash";

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
        if (TryGetComponent<SwordAction>(out SwordAction swordAction))
        {
            swordAction.OnSwordSlashStarted += SwordAction_OnSwordSlashStarted;
            swordAction.OnSwordSlashCompleted += SwordAction_OnSwordSlashCompleted;
        }
    }

    private void Start()
    {
        EquipRifle();
    }
    

    private void SwordAction_OnSwordSlashCompleted(object sender, EventArgs e)
    {
        EquipRifle();
    }

    private void SwordAction_OnSwordSlashStarted(object sender, EventArgs e)
    {   
        EquipSword();
        animator.SetTrigger(SWORD_SLASH);
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
    
    private void EquipRifle()
    {
        swordTransform.gameObject.SetActive(false);
        rifleTransform.gameObject.SetActive(true);
    }

    private void EquipSword()
    {   
        rifleTransform.gameObject.SetActive(false);
        swordTransform.gameObject.SetActive(true);
    }
    
}
