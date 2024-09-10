using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UnitActionSystem : MonoBehaviour
{
    public static UnitActionSystem Instance { get; private set; }


    public event EventHandler OnSelectedUnitChanged;
    private bool IsBusy = false;
    
    [SerializeField] private Unit selectedUnit;
    [SerializeField] private LayerMask unitLayerMask;


    private void Awake()
    {
        if(Instance != null)
        {
            Debug.LogError("There is more than one Unit selection system" + transform);
        }
        Instance = this;
      
    }

    private void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (IsBusy) { return; }


        if (Input.GetMouseButtonDown(0))
        {
            if (TryHandleUnitSelection()) return;

            

            GridPosition targetGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetPosition());
            SetBusy();
            selectedUnit.GetMoveAction().Move(targetGridPosition, ClearBusy);            
        }

        if (Input.GetMouseButtonDown(1))
        {
            SetBusy();
            selectedUnit.GetSpinAction().StartSpining(ClearBusy);           
        }
    }

    private bool TryHandleUnitSelection()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out RaycastHit hitInfo, float.MaxValue, unitLayerMask))
        {
            if(hitInfo.transform.TryGetComponent<Unit>(out Unit unit))
            {
                SetSelectedUnit(unit);
                OnSelectedUnitChanged?.Invoke(this, EventArgs.Empty);
                return true;
            }
        }
        return false;
    }

    private void SetSelectedUnit(Unit unit)
    {
        selectedUnit = unit;
    }

    public Unit GetSelectedUnit()
    {
        return selectedUnit;
    }

    private void SetBusy()
    {
        IsBusy = true;
    }

    private void ClearBusy()
    {
        IsBusy = false;
    }
}
