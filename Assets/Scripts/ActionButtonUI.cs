using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActionButtonUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI actionName;
    [SerializeField] private Button button;
    
    void Start()
    {
        
    }
   
    void Update()
    {
        
    }

    public void SetBaseAction(BaseAction baseAction)
    {
        actionName.text = baseAction.GetActionName().ToUpper();
    }
}
