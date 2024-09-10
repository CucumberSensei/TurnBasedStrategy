using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UnitActionSystemUI : MonoBehaviour
{
    [SerializeField] private Transform prefabActionButon;
    [SerializeField] private Transform ActionButtonUIContainer;


    void Start()
    {
        UnitActionSystem.Instance.OnSelectedUnitChanged += UnitActionSystem_OnSelectedUnitChanged;
        CreateUnitActionButtons();
    }

    private void UnitActionSystem_OnSelectedUnitChanged(object sender, System.EventArgs e)
    {
        CreateUnitActionButtons();
    }

    private void CreateUnitActionButtons()
    {
        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();
        BaseAction[] selectedUnitActions = selectedUnit.GetActionsArray();

        foreach (Transform child in ActionButtonUIContainer)
        {
            Destroy(child.gameObject);
        }


        foreach (BaseAction baseAction in selectedUnitActions)
        {
            Transform transformActionButton = Instantiate(prefabActionButon, ActionButtonUIContainer);
            ActionButtonUI actionButton = transformActionButton.GetComponent<ActionButtonUI>();
            actionButton.SetBaseAction(baseAction);          
        }       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
