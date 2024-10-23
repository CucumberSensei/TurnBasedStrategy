using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LevelGrid : MonoBehaviour
{   
    public static LevelGrid Instance {  get; private set; }

    public event EventHandler OnAnyUnitChangeGridPosition;
    [SerializeField] private int heigth;
    [SerializeField] private int width;
    [SerializeField] private float cellSize;
    private GridSystem<GridObject> gridSystem;
    [SerializeField] private Transform debugObjectPrefab;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one LevelGrid" + transform);

        }
        Instance = this;

        gridSystem = new GridSystem<GridObject>(heigth, width, cellSize, 
            (GridSystem<GridObject> g, GridPosition gridPosition)=> new GridObject(g, gridPosition));
        //gridSystem.CreateDebugObjects(debugObjectPrefab);
    }

    private void Start()
    {
        Pathfinding.Instance.Setup(heigth, width, cellSize);
    }

    public void AddUnitAtGridPosition(Unit unit, GridPosition gridPosition)
    {
        GridObject gridObject = gridSystem.GetGridObjectAtGridPosition(gridPosition);
        gridObject.AddUnit(unit);
    }

    public List<Unit> GetUnitListAtGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = gridSystem.GetGridObjectAtGridPosition(gridPosition);
        return gridObject.GetUnitList();
    }

    public Unit GetUnitAtGridPosition(GridPosition gridPosition)
    {
        //ONLY USE ON IDLE

        GridObject gridObject = gridSystem.GetGridObjectAtGridPosition(gridPosition);
        List<Unit> unitList = gridObject.GetUnitList();

        return unitList[0];
    }

    public void ClearUnitsAtGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = gridSystem.GetGridObjectAtGridPosition(gridPosition);
        List<Unit> unitList = gridObject.GetUnitList();
        unitList.Clear();
    }

    public void RemoveUnitAtGridPosition(Unit unit, GridPosition gridPosition)
    {
        GridObject gridObject = gridSystem.GetGridObjectAtGridPosition(gridPosition);
        gridObject.RemoveUnit(unit);
    }
             
    public void UnitMove(Unit unit, GridPosition formerGridPosition, GridPosition newGridPostion)
    {
        RemoveUnitAtGridPosition(unit, formerGridPosition);
        AddUnitAtGridPosition(unit, newGridPostion);

        OnAnyUnitChangeGridPosition?.Invoke(this, EventArgs.Empty);
    }


    //PASS THROUGH FUNTIONS FROM THE GRID SYSTEM
    public GridPosition GetGridPosition(Vector3 worldPosition) => gridSystem.GetGridPosition(worldPosition);

    public bool IsValidGridPosition(GridPosition gridPosition) => gridSystem.IsValidGridPosition(gridPosition);

    public Vector3 GetWorldPosition(GridPosition gridPosition) => gridSystem.GetWorldPosition(gridPosition);

    public int GetWidth() => gridSystem.GetWidth();

    public int GetHeight() => gridSystem.GetHeight();



}
