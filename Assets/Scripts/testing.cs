using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testing : MonoBehaviour
{
    [SerializeField] private Unit unit;
    private MoveAction moveAction;

    private void Awake()
    {
        moveAction = unit.GetComponent<MoveAction>();
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            GridSystemVisual.Instance.HideAllVisuals();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            GridSystemVisual.Instance.ShowGridPositionListVisual(moveAction.GetValidActionGridPositionList());
        }
    }




}
