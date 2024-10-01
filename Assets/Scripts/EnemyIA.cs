using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyIA : MonoBehaviour
{
    private enum State
    {
        WaitingForEnemyTurn,
        TakingTurn,
        Busy,
    }

    private State state;
    private float timer;


    private void Awake()
    {
        state = State.WaitingForEnemyTurn;
    }

    void Start()
    {
        TurnSystem.Instance.OnTurnChange += TurnSystem_OnTurnChange;
    }

    private void TurnSystem_OnTurnChange(object sender, System.EventArgs e)
    {
        if (!TurnSystem.Instance.IsPlayerTurn())
        {
            state = State.TakingTurn;
            timer = 2f;
        }
        
    }

    void Update()
    {
        if (TurnSystem.Instance.IsPlayerTurn())
        {
            return;
        }

        switch (state)
        {
            case State.WaitingForEnemyTurn:
                break;
            case State.TakingTurn:

                timer -= Time.deltaTime;

                if (timer <= 0)
                {

                    if (TryTakingEnemyAIAction(SetTakingTurn))
                    {
                        state = State.Busy;
                    }
                    else
                    {   
                        state = State.WaitingForEnemyTurn;
                        TurnSystem.Instance.NextTurn();
                    }
                }
                break;
            case State.Busy:
                break;
        }            
    }

    private bool TryTakingEnemyAIAction(Action onEnemyAIActionComplete)
    {
        foreach (Unit enemyUnit in UnitManager.Instance.GetEnemyUnitList())
        {
            if(TryTakeEnemyAIAction(enemyUnit, onEnemyAIActionComplete))
            {
                return true;
            }           
        }
        return false;
    }

    private bool TryTakeEnemyAIAction(Unit enemyUnit, Action onActionComplete)
    {   
        BaseAction bestAction = null;
        EnemyAIActionScore bestEnemyAIActionScore = null;

        foreach(BaseAction baseAction in enemyUnit.GetActionsArray())
        {
            if (!enemyUnit.CanSpendActionPointsToTakeAction(baseAction))
            {
                continue;
            }


            EnemyAIActionScore enemyAIActionScore = baseAction.GetBestEnemyAIActionScore();

            if (bestEnemyAIActionScore == null)
            {
                bestAction = baseAction;
                bestEnemyAIActionScore = enemyAIActionScore;
            }
            else
            {
                if(enemyAIActionScore != null && enemyAIActionScore.actionScore > bestEnemyAIActionScore.actionScore)
                {
                    bestAction = baseAction;
                    bestEnemyAIActionScore = enemyAIActionScore;
                }
            }
        }

        if(bestAction != null && enemyUnit.TrySpendAcionPoints(bestAction))
        {
            Debug.Log(bestEnemyAIActionScore.gridPosition + ": " + bestEnemyAIActionScore.actionScore);
            bestAction.TakeAction(bestEnemyAIActionScore.gridPosition, onActionComplete);
            return true;
        }
        else
        {
            return false;
        }     
    }

    private void SetTakingTurn()
    {
        timer = .5f;
        state = State.TakingTurn;
    }
}
