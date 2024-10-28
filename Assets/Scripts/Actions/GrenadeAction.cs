using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeAction : BaseAction
{
    private int maxThrowDistance = 7;
    [SerializeField] private Transform grenadePrefab;
    private void Update()
    {
        if (!isActive)
        {
            return;
        }
    }
    
    public override string GetActionName()
    {
        return "Grenade";
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {   
        Transform grenadeTransform = Instantiate(grenadePrefab, unit.GetWorldPosition(), Quaternion.identity);
        GrenadeProjectile granadeProjectile = grenadeTransform.GetComponent<GrenadeProjectile>();
        granadeProjectile.Setup(gridPosition, OnGrenadeComplete);
        OnActionStart(onActionComplete);
    }

    private void OnGrenadeComplete()
    {
        OnActionComplete();
    }
    
    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validActionGridPositionList = new List<GridPosition>();
        GridPosition gridPosition = unit.GetGridPosition();
        for (int x = -maxThrowDistance; x <= maxThrowDistance; x++)
        {
            for (int z = -maxThrowDistance; z <= maxThrowDistance; z++)
            {
                GridPosition offsetGrisPosition = new GridPosition(x, z);
                GridPosition targetGridPositon = gridPosition + offsetGrisPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(targetGridPositon))
                {
                    continue;
                }

                Vector3 unitWorldPosition = LevelGrid.Instance.GetWorldPosition(gridPosition);
                Vector3 targetWorldPosition = LevelGrid.Instance.GetWorldPosition(targetGridPositon);

                if(Vector3.Distance(unitWorldPosition, targetWorldPosition) > 10.5)
                {
                    continue;
                }                            
                
                validActionGridPositionList.Add(targetGridPositon);
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
            actionScore = 0
        };
    }

    public int GetMaxThrowDistance()
    {
        return maxThrowDistance;
    }
}
