using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAction : BaseAction
{   
    public event EventHandler OnSwordSlashStarted;
    public event EventHandler OnSwordSlashCompleted;
    public static event EventHandler OnAnySwordHit;
    
    private enum State
    {
        SwingingBeforeHit,
        SwingingAfterHit
    }
    
    private State state;
    private int maxSwordDistance = 1;
    private float stateTimer;
    private Unit targetUnit;
    
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
            case State.SwingingBeforeHit:
                Vector3 slashDirection = (targetUnit.GetWorldPosition() - unit.GetWorldPosition()).normalized;
                float unitRotationSpeed = 10f;
                transform.forward = Vector3.Slerp(transform.forward, slashDirection, Time.deltaTime * unitRotationSpeed);
                break;
            
            case State.SwingingAfterHit:
                
                break;
        }
    }

    private void NextState()
    {
        switch (state)
        {
            case State.SwingingBeforeHit:
                state = State.SwingingAfterHit;
                float swingingAfterHitTime = .5f;
                stateTimer = swingingAfterHitTime;
                targetUnit.Damage(100);
                OnAnySwordHit?.Invoke(this, EventArgs.Empty);
                break;
            case State.SwingingAfterHit:
                OnSwordSlashCompleted?.Invoke(this, EventArgs.Empty);
                onActionComplete();
                break;
        }
    }

    public override string GetActionName()
    {
        return "Sword";
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {   
        targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);
        
        state = State.SwingingBeforeHit;
        float swingingBeforeHitTime = .7f;
        stateTimer = swingingBeforeHitTime;
        
        OnSwordSlashStarted?.Invoke(this, EventArgs.Empty);
        OnActionStart(onActionComplete);
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validActionGridPositionList = new List<GridPosition>();
        GridPosition gridPosition = unit.GetGridPosition();
        
        for (int x = -maxSwordDistance; x <= maxSwordDistance; x++)
        {
            for (int z = -maxSwordDistance; z <= maxSwordDistance; z++)
            {
                GridPosition offsetGrisPosition = new GridPosition(x, z);
                GridPosition targetGridPosition = gridPosition + offsetGrisPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(targetGridPosition))
                {
                    continue;
                }
                if (LevelGrid.Instance.GetUnitListAtGridPosition(targetGridPosition).Count <= 0)
                {
                    continue;
                }

                Unit unitAtGridPosition = LevelGrid.Instance.GetUnitAtGridPosition(targetGridPosition);

                if(unit.IsEnemy() == unitAtGridPosition.IsEnemy())
                {
                    continue;
                }
                
                validActionGridPositionList.Add(targetGridPosition);
            }
        }
        return validActionGridPositionList;
    }

    public override int GetActionPointsCost()
    {
        return 1;
    }

    public override EnemyAIActionScore GetEnemyAIActionScore(GridPosition gridPosition)
    {
        return new EnemyAIActionScore
        {
            gridPosition = gridPosition,
            actionScore = 200
        };
    }

    public int GetMaxSwordDistance()
    {
        return maxSwordDistance;
    }
    
}
