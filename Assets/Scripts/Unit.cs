using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private const int MAX_ACTION_POINTS = 3;

    public static event EventHandler OnAnyActionPointChange;

    public static event EventHandler OnAnyUnitSpawn;
    public static event EventHandler OnAnyUnitDie;

    
    private GridPosition gridPosition;  
    private HealthSystem healthSystem;
    [SerializeField]private int actionPoints = MAX_ACTION_POINTS;
    [SerializeField] private Transform actoinCameraSpot;
    [SerializeField] private BaseAction[] actionsArray;
    [SerializeField] private bool isEnemy;

    private void Awake()
    {        
        actionsArray = GetComponents<BaseAction>();
        healthSystem = GetComponent<HealthSystem>();
    }

    private void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(this, gridPosition);

        TurnSystem.Instance.OnTurnChange += TurnSystem_OnTurnChange;
        healthSystem.OnDeath += HealthSystem_OnDeath;

        OnAnyUnitSpawn?.Invoke(this, EventArgs.Empty);
    }

    private void HealthSystem_OnDeath(object sender, EventArgs e)
    {
        LevelGrid.Instance.RemoveUnitAtGridPosition(this, gridPosition);
        Destroy(gameObject);
    }

    private void TurnSystem_OnTurnChange(object sender, EventArgs e)
    {
        ReloadActionPoints();
        OnAnyActionPointChange?.Invoke(this, new EventArgs());       
    }

    private void Update()
    {                    
        GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);

        if(newGridPosition != gridPosition)
        {
            GridPosition oldPosition = gridPosition;
            gridPosition = newGridPosition;

            LevelGrid.Instance.UnitMove(this, oldPosition, newGridPosition);
        }
    }

    public bool TrySpendAcionPoints(BaseAction baseAction)
    {
        if (CanSpendActionPointsToTakeAction(baseAction))
        {
            SpendActionPoints(baseAction);
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool CanSpendActionPointsToTakeAction(BaseAction baseAction)
    {
        if (actionPoints >= baseAction.GetActionPointsCost())
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void SpendActionPoints(BaseAction baseAction)
    {
        actionPoints = actionPoints - baseAction.GetActionPointsCost();
        OnAnyActionPointChange?.Invoke(this, new EventArgs());
    }

    public T GetAction<T>() where T : BaseAction
    {
        foreach (BaseAction baseAction in actionsArray)
        {
            if(baseAction is T)
            {
                return (T)baseAction;
            }
        }
        return null;
    }

    public GridPosition GetGridPosition()
    {
        return gridPosition;
    }

    public Vector3 GetWorldPosition()
    {
        return transform.position;
    }

    public BaseAction[] GetActionsArray()
    {
        return actionsArray;
    }

    public int GetActionPoints()
    {
        return actionPoints;
    }

    private void ReloadActionPoints()
    {
        if (!IsEnemy() && TurnSystem.Instance.IsPlayerTurn() ||
             IsEnemy() && !TurnSystem.Instance.IsPlayerTurn())
        {
            actionPoints = MAX_ACTION_POINTS;
        }      
    }
    
    public bool IsEnemy()
    {
        return isEnemy;
    }

    public void Damage(int damageAmount)
    {
        healthSystem.Damage(damageAmount);
    }

    private void OnDestroy()
    {
        TurnSystem.Instance.OnTurnChange -= TurnSystem_OnTurnChange;
        healthSystem.OnDeath -= HealthSystem_OnDeath;
        OnAnyUnitDie?.Invoke(this, EventArgs.Empty);
    }

    public Transform GetActionCameraSpot()
    {
        return actoinCameraSpot;
    }

    public float GetHealthNormalized()
    {
        return healthSystem.GetHealthNormalized();
    }
}
