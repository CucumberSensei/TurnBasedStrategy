using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LevelGrid : MonoBehaviour
{   
    public static LevelGrid Instance {  get; private set; }

    private GridSystem gridSystem;
    [SerializeField] private Transform debugObjectPrefab;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one LevelGrid" + transform);

        }
        Instance = this;

        gridSystem = new GridSystem(20, 20, 2f);
        gridSystem.CreateDebugObjects(debugObjectPrefab);
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

    public void RemoveUnitAtGridPosition(Unit unit, GridPosition gridPosition)
    {
        GridObject gridObject = gridSystem.GetGridObjectAtGridPosition(gridPosition);
        gridObject.RemoveUnit(unit);
    }
             
    public void UnitMove(Unit unit, GridPosition formerGridPosition, GridPosition newGridPostion)
    {
        RemoveUnitAtGridPosition(unit, formerGridPosition);
        AddUnitAtGridPosition(unit, newGridPostion);
    }


    //PASS THROUGH FUNTIONS FROM THE GRID SYSTEM
    public GridPosition GetGridPosition(Vector3 worldPosition) => gridSystem.GetGridPosition(worldPosition);

    public bool IsValidGridPosition(GridPosition gridPosition) => gridSystem.IsValidGridPosition(gridPosition);

    public Vector3 GetWorldPosition(GridPosition gridPosition) => gridSystem.GetWorldPosition(gridPosition);

    public int GetWidth() => gridSystem.GetWidth();

    public int GetHeight() => gridSystem.GetHeight();



}
