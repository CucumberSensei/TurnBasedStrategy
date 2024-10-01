using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UnitManager : MonoBehaviour
{
    public static UnitManager Instance { get; private set; }

    [SerializeField] private List<Unit> unitList;
    [SerializeField] private List<Unit> enemyUnitList;
    [SerializeField] private List <Unit> friendlyUnitList;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one UnitManager system" + transform);
        }

        Instance = this;

        unitList = new List<Unit>();
        enemyUnitList = new List<Unit>();
        friendlyUnitList = new List<Unit>();
    }

    void Start()
    {
        Unit.OnAnyUnitSpawn += Unit_OnAnyUnitSpawn;
        Unit.OnAnyUnitDie += Unit_OnAnyUnitDie;
    }

    private void Unit_OnAnyUnitDie(object sender, EventArgs e)
    {
        Unit unit = sender as Unit;

        unitList.Remove(unit);

        if (unit.IsEnemy())
        {
            enemyUnitList.Remove(unit);
        }
        else
        {
            friendlyUnitList.Remove(unit);
        }
    }

    private void Unit_OnAnyUnitSpawn(object sender, EventArgs e)
    {
        Unit unit = sender as Unit;

        unitList.Add(unit);
        

        if (unit.IsEnemy())
        {
            enemyUnitList.Add(unit);
            
        }
        else
        {
            friendlyUnitList.Add(unit);
            
        }
    }

    public List<Unit> GetUnitList()
    {
        return unitList;
    }

    public List<Unit> GetEnemyUnitList()
    {
        return enemyUnitList;
    }

    public List<Unit> GetFriendlyUnitList()
    {
        return friendlyUnitList;
    }
}
