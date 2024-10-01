using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Compilation;
using UnityEngine;

public abstract class BaseAction : MonoBehaviour
{
    protected Unit unit;
    protected bool isActive = false;
    protected Action onActionComplete;

    protected virtual void Awake()
    {
        unit = GetComponent<Unit>();
    }

    public abstract string GetActionName();

    public abstract void TakeAction(GridPosition gridPosition, Action onActionComplete);

    public virtual bool IsValidActionGridPosition(GridPosition gridPosition)
    {
        return GetValidActionGridPositionList().Contains(gridPosition);
    }

    public abstract List<GridPosition> GetValidActionGridPositionList();

    public abstract int GetActionPointsCost();

    protected void OnActionStart(Action onActionComplete)
    {
        this.onActionComplete = onActionComplete;
        isActive = true;
    }

    protected void OnActionComplete()
    {
        onActionComplete();
        isActive = false;
    }

    public EnemyAIActionScore GetBestEnemyAIActionScore()
    {
        List<EnemyAIActionScore> enemyAIActionScoreList = new List<EnemyAIActionScore>();

        List<GridPosition> validActionGridPositionList = GetValidActionGridPositionList();

        foreach(GridPosition gridPosition in validActionGridPositionList)
        {
            EnemyAIActionScore enemyAIActionScore = GetEnemyAIActionScore(gridPosition);
            enemyAIActionScoreList.Add(enemyAIActionScore);
        }


        if(enemyAIActionScoreList.Count > 0)
        {
            enemyAIActionScoreList.Sort((EnemyAIActionScore a, EnemyAIActionScore b) => b.actionScore - a.actionScore);
            return enemyAIActionScoreList[0];
        }
        else
        {   
            //No posible Action to take
            return null;
        }
        
    }

    public abstract EnemyAIActionScore GetEnemyAIActionScore(GridPosition gridPosition);
    

}
