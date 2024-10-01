using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpinAction : BaseAction
{   

    private float totalSpinAmount;   

    private void Update()
    {   
        if (!isActive)
        {
            return;
        }

        float spiningAmount = 360f * Time.deltaTime;

        transform.eulerAngles += new Vector3(0, spiningAmount, 0);
        totalSpinAmount += spiningAmount;

        if (totalSpinAmount >= 360) 
        { 
            OnActionComplete();
        }
    }

    public override void TakeAction(GridPosition targetGridPosition, Action onSpinComplete)
    {
        OnActionStart(onSpinComplete);

        totalSpinAmount = 0;
                                    
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();
        validGridPositionList.Add(unit.GetGridPosition());

        return validGridPositionList;
    }

    public override string GetActionName()
    {
        return "Spin";
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
            actionScore = 0,
        };
    }
}
