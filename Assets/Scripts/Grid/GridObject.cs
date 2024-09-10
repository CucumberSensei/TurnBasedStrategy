using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject
{
    private GridSystem gridSystem;
    private GridPosition gridPosition;
    private  List<Unit> unitList;

    public GridObject(GridSystem gridSystem, GridPosition gridPosition)
    {
        this.gridSystem = gridSystem;
        this.gridPosition = gridPosition;
        this.unitList = new List<Unit>();
    }

    public GridPosition GetGridPosition()
    {
        return this.gridPosition;
    }

    public void AddUnit(Unit unit)
    {
        unitList.Add(unit);
    }

    public void RemoveUnit(Unit unit)
    {
        unitList.Remove(unit);
    }

    public List<Unit> GetUnitList()
    {
        return unitList;
    }

    public override string ToString()
    {
        string stringUnitList = "";

        foreach (Unit unit in unitList)
        {
            stringUnitList += unit + "\n";
        }
        return this.gridPosition.ToString() + "\n" + stringUnitList.Trim();
    }
}
