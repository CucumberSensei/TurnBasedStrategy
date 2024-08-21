using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Unit : MonoBehaviour
{
    
    private Vector3 targetDirection;
    private const string isWalking = "IsWalking";
    [SerializeField] private Animator unitAnimator;


    private void Awake()
    {
        targetDirection = transform.position;
    }

    private void Update()
    {             
        float moveSpeed = 4f;
        float stoppingDistance = .1f;

        if (Vector3.Distance(transform.position, targetDirection) > stoppingDistance)
        {
            Vector3 moveDirection = (targetDirection - transform.position).normalized;
            transform.position += moveDirection.normalized * moveSpeed * Time.deltaTime;

            unitAnimator.SetBool(isWalking, true);

            float unitRotationSpeed = 20f;
            transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * unitRotationSpeed);

        }
        else
        {
            unitAnimator.SetBool(isWalking, false);
        }       
    }


    public void Move(Vector3 targetDirection)
    {
        this.targetDirection = targetDirection;  
    }
}
