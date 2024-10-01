using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UnitActionSystemUI : MonoBehaviour
{
    [SerializeField] private Transform prefabActionButon;
    [SerializeField] private Transform ActionButtonUIContainer;
    [SerializeField] private TextMeshProUGUI actionPointsUI;

    private List<ActionButtonUI> actionButtonUIList;

    private void Awake()
    {
        actionButtonUIList = new List<ActionButtonUI>();        
    }

    void Start()
    {
        UnitActionSystem.Instance.OnSelectedUnitChanged += UnitActionSystem_OnSelectedUnitChanged;
        UnitActionSystem.Instance.OnSelectedActionChange += UnitActionSystem_OnSelectedActionChange;
        UnitActionSystem.Instance.OnActionTake += UnitActionSystem_OnActionTake;
        Unit.OnAnyActionPointChange += Unit_OnOnAnyActionPointChange;

        CreateUnitActionButtons();
        UpdateSelectedVisuals();
        UpdateActionPointsUI();
    }

    private void Unit_OnOnAnyActionPointChange(object sender, System.EventArgs e)
    {
        UpdateActionPointsUI();
    }

    private void UnitActionSystem_OnActionTake(object sender, System.EventArgs e)
    {
        UpdateActionPointsUI();
    }

    private void UnitActionSystem_OnSelectedActionChange(object sender, System.EventArgs e)
    {
        UpdateSelectedVisuals();
    }

    private void UnitActionSystem_OnSelectedUnitChanged(object sender, System.EventArgs e)
    {
        CreateUnitActionButtons();
        UpdateSelectedVisuals();
        UpdateActionPointsUI();
    }

    private void CreateUnitActionButtons()
    {
        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();
        BaseAction[] selectedUnitActions = selectedUnit.GetActionsArray();

        foreach (Transform child in ActionButtonUIContainer)
        {
            Destroy(child.gameObject);
            
        }

        actionButtonUIList.Clear();

        foreach (BaseAction baseAction in selectedUnitActions)
        {
            Transform transformActionButton = Instantiate(prefabActionButon, ActionButtonUIContainer);
            ActionButtonUI actionButton = transformActionButton.GetComponent<ActionButtonUI>();
            actionButton.SetBaseAction(baseAction);
            
            actionButtonUIList.Add(actionButton);
        }       
    }

    private void UpdateSelectedVisuals()
    {
        foreach (ActionButtonUI actionButton in actionButtonUIList)
        {
            actionButton.UpdateSelectedVisual();
        }
    }

    private void UpdateActionPointsUI()
    {
        actionPointsUI.text = "Action Points " + UnitActionSystem.Instance.GetSelectedUnit().GetActionPoints();
    }
}
