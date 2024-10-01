using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActionButtonUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI actionName;
    [SerializeField] private Button button;
    [SerializeField] private BaseAction baseAction;
    [SerializeField] private GameObject selectedVisual;

   

    public void SetBaseAction(BaseAction baseAction)
    {   
        this.baseAction = baseAction;

        actionName.text = baseAction.GetActionName().ToUpper();

        button.onClick.AddListener(() =>
        {
            UnitActionSystem.Instance.SetSelectedAction(baseAction);
        });
    }

    public void UpdateSelectedVisual ()
    {        
        if (UnitActionSystem.Instance.GetSelectedAction() == baseAction) 
        {
            selectedVisual.SetActive(true);
        }
        else
        {
            selectedVisual.SetActive(false);
        }                
    }    
}
