using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShootAction : BaseAction
{
    public static EventHandler<OnShootStartEventArgs> OnShootCameraStart;
    public static EventHandler OnShootCameraEnd;

    public EventHandler<OnShootStartEventArgs> OnShootStart;

    public class OnShootStartEventArgs : EventArgs
    {
        public Unit targetUnit;
        public Unit shootingUnit;
    }


    private enum State
    {
        Aiming,
        Shooting,
        CoolOff,
    }

    private State state;       
    private int maxShootDistance = 5;
    private float stateTimer;
    private Unit targetUnit;
    private bool canShoot = true;
    

   

    private void Update()
    {
        if (!isActive)
        {
            return;
        }

        stateTimer -= Time.deltaTime;

        if(stateTimer < 0)
        {
            NextState();
        }

        switch (state)
        {
            case State.Aiming:
                Vector3 shootDirection = (targetUnit.GetWorldPosition() - unit.GetWorldPosition()).normalized;

                float unitRotationSpeed = 10f;
                transform.forward = Vector3.Slerp(transform.forward, shootDirection, Time.deltaTime * unitRotationSpeed);
                break;

            case State.Shooting:
                if (canShoot)
                {
                    Shoot();
                    canShoot = false;
                }
                
                break;

            case State.CoolOff:
                //
                break;
        }
    }

    private void NextState()
    {
        switch (state)
        {
            case State.Aiming:
                if (stateTimer < 0)
                {
                    state = State.Shooting;
                    float shootingStateTime = .1f;
                    stateTimer = shootingStateTime;
                }
                break;
            case State.Shooting:
                if (stateTimer < 0)
                {
                    state = State.CoolOff;
                    float coolOffStateTime = .5f;
                    stateTimer = coolOffStateTime;
                }
                break;
            case State.CoolOff:
                if (stateTimer < 0)
                {
                    OnActionComplete();

                    OnShootCameraEnd?.Invoke(this, EventArgs.Empty);
                }
                break;
        }

        Debug.Log(transform.ToString() + state);
    }

    private void Shoot()
    {
        Debug.Log("POO!!");
        OnShootStart?.Invoke(this, new OnShootStartEventArgs{
            targetUnit = targetUnit,
            shootingUnit = unit,
        });
        targetUnit.Damage(20);
    }

    public override string GetActionName()
    {
        return "Shoot";
    }

    public override int GetActionPointsCost()
    {
        return 1;
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        GridPosition unitGridPosition = unit.GetGridPosition();

        return GetValidActionGridPositionList(unitGridPosition);
    }

    public List<GridPosition> GetValidActionGridPositionList(GridPosition gridPosition)
    {
        List<GridPosition> validActionGridPositionList = new List<GridPosition>();

        for (int x = -maxShootDistance; x <= maxShootDistance; x++)
        {
            for (int z = -maxShootDistance; z <= maxShootDistance; z++)
            {
                GridPosition offsetGrisPosition = new GridPosition(x, z);
                GridPosition targetGridPositon = gridPosition + offsetGrisPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(targetGridPositon))
                {
                    continue;
                }

                Vector3 unitWordlPosition = LevelGrid.Instance.GetWorldPosition(gridPosition);
                Vector3 targetWorldPosition = LevelGrid.Instance.GetWorldPosition(targetGridPositon);

                if(Vector3.Distance(unitWordlPosition, targetWorldPosition) > 10.5)
                {
                    continue;
                }                            

                if (LevelGrid.Instance.GetUnitListAtGridPosition(targetGridPositon).Count <= 0)
                {
                    continue;
                }

                Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(targetGridPositon);

                if(unit.IsEnemy() == targetUnit.IsEnemy())
                {
                    continue;
                }

                validActionGridPositionList.Add(targetGridPositon);
            }
        }
        return validActionGridPositionList;
    }

    public override void TakeAction(GridPosition targetGridPosition, Action onActionComplete)
    {
        OnActionStart(onActionComplete);

        canShoot = true;
        targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(targetGridPosition);
        state = State.Aiming;
        float aimingStateTime = 1f;
        stateTimer = aimingStateTime;
        Debug.Log(transform + "Aiming");

        OnShootCameraStart?.Invoke(this, new OnShootStartEventArgs
        {
            targetUnit = targetUnit,
            shootingUnit = unit,
        });
    }

    public int GetMaxShootDistance()
    {
        return maxShootDistance;
    }

    public override EnemyAIActionScore GetEnemyAIActionScore(GridPosition gridPosition)
    {
        Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);

        return new EnemyAIActionScore
        {
            gridPosition = gridPosition,
            actionScore = 100 + Mathf.RoundToInt((1 - targetUnit.GetHealthNormalized()) * 100f),
        };
    }
}
