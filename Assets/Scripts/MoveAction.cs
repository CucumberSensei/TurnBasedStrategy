using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : BaseAction
{
    private Vector3 targetDirection;
    private const string isWalking = "IsWalking";
    private Action onMoveComplete;
    
    [SerializeField] private Animator unitAnimator;
    [SerializeField] private int maxGridMovementDistance = 1;
    

    protected override void Awake()
    {   
        base.Awake();
        targetDirection = transform.position;
    }

    void Start()
    {
        
    }
  
    void Update()
    {
        if (!isActive)
        {
            return;
        }

        float moveSpeed = 4f;
        float stoppingDistance = .1f;

        if (Vector3.Distance(transform.position, targetDirection) > stoppingDistance)
        {
            Vector3 moveDirection = (targetDirection - transform.position).normalized;
            transform.position += moveDirection.normalized * moveSpeed * Time.deltaTime;

            unitAnimator.SetBool(isWalking, true);

            float unitRotationSpeed = 30f;
            transform.forward = Vector3.Slerp(transform.forward, moveDirection, Time.deltaTime * unitRotationSpeed);
           
        }
        else
        {
            unitAnimator.SetBool(isWalking, false);
            isActive = false;
            onMoveComplete();
        }
    }

    public void Move(GridPosition targetGridPosition, Action onMoveComplete)
    {   
        this.onMoveComplete = onMoveComplete;

        if (GetValidActionGridPositionList().Contains(targetGridPosition))
        {
            this.targetDirection = LevelGrid.Instance.GetWorldPosition(targetGridPosition);
            isActive = true;
        }
        else
        {
            onMoveComplete();
        }
    }

    public List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validActionGridPositionList = new List<GridPosition>();

        for (int x = -maxGridMovementDistance; x <= maxGridMovementDistance; x++ )
        {
            for (int z = -maxGridMovementDistance; z <= maxGridMovementDistance; z++)
            {
                GridPosition offsetGrisPosition = new GridPosition(x,z);
                GridPosition targetGridPositon = unit.GetGridPosition() + offsetGrisPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(targetGridPositon))
                {
                    continue;
                }
                if(LevelGrid.Instance.GetUnitListAtGridPosition(targetGridPositon).Count > 0)
                {
                    continue;
                }
                if(targetGridPositon == unit.GetGridPosition())
                {
                    continue;
                }

                validActionGridPositionList.Add(targetGridPositon);
            }
        }
        return validActionGridPositionList;
    }

    public override string GetActionName()
    {
        return "Move";
    }
}
