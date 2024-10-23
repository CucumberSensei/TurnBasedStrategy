using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MoveAction : BaseAction
{
    public EventHandler OnMoveStart;
    public EventHandler OnMoveEnd;

    private List<Vector3> targetDirectionList;
    private int pathIndex = 0;          
    [SerializeField] private int maxGridMovementDistance = 1;

   
    void Update()
    {
        if (!isActive)
        {
            return;
        }

        float moveSpeed = 4f;
        float stoppingDistance = .1f;

        if (Vector3.Distance(transform.position, targetDirectionList[pathIndex]) > stoppingDistance)
        {
            Vector3 moveDirection = (targetDirectionList[pathIndex] - transform.position).normalized;
            transform.position += moveDirection.normalized * moveSpeed * Time.deltaTime;

            OnMoveStart?.Invoke(this, EventArgs.Empty);

            float unitRotationSpeed = 30f;
            transform.forward = Vector3.Slerp(transform.forward, moveDirection, Time.deltaTime * unitRotationSpeed);          
        }
        else
        {
            pathIndex++;
            if(pathIndex >= targetDirectionList.Count)
            {
                OnMoveEnd?.Invoke(this, EventArgs.Empty);

                OnActionComplete();
            }                     
        }
    }



    public override void TakeAction(GridPosition targetGridPosition, Action onMoveComplete)
    {
        OnActionStart(onMoveComplete);
        pathIndex = 0;       
        targetDirectionList = new List<Vector3>();

        List<GridPosition> pathGridPositionList = 
            Pathfinding.Instance.FindPath(LevelGrid.Instance.GetGridPosition(transform.position), targetGridPosition,out int pathDistance);
       

        foreach(GridPosition gridPosition in pathGridPositionList)
        {
            targetDirectionList.Add(LevelGrid.Instance.GetWorldPosition(gridPosition));
        }
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validActionGridPositionList = new List<GridPosition>();

        for (int x = -maxGridMovementDistance; x <= maxGridMovementDistance; x++ )
        {
            for (int z = -maxGridMovementDistance; z <= maxGridMovementDistance; z++)
            {
                GridPosition offsetGrisPosition = new GridPosition(x,z);
                GridPosition targetGridPositon = unit.GetGridPosition() + offsetGrisPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(targetGridPositon))
                {
                    continue;
                }
                if(LevelGrid.Instance.GetUnitListAtGridPosition(targetGridPositon).Count > 0)
                {
                    continue;
                }
                if(targetGridPositon == unit.GetGridPosition())
                {
                    continue;
                }
                if (!Pathfinding.Instance.IsWalkableGridPosition(targetGridPositon))
                {
                    continue;
                }  
                if(!Pathfinding.Instance.HasPath(unit.GetGridPosition(), targetGridPositon))
                {
                    continue;
                }

                int pathDistanceMultiplier = 10;

                if(Pathfinding.Instance.GetPathDistance(unit.GetGridPosition(), targetGridPositon)> maxGridMovementDistance * pathDistanceMultiplier)
                {
                    continue;
                }

                validActionGridPositionList.Add(targetGridPositon);
            }
        }
        
        return validActionGridPositionList;
    }

    public override string GetActionName()
    {
        return "Move";
    }

    public override int GetActionPointsCost()
    {
        return 1;
    }

    public override EnemyAIActionScore GetEnemyAIActionScore(GridPosition gridPosition)
    {
        ShootAction shootAction = unit.GetAction<ShootAction>();

        List<GridPosition> validShootingGridPositonList = shootAction.GetValidActionGridPositionList(gridPosition);

        int posibleShootingTargets = validShootingGridPositonList.Count;        

        return new EnemyAIActionScore
        {
            gridPosition = gridPosition,
            actionScore = posibleShootingTargets * 10,
        };
    }
}
