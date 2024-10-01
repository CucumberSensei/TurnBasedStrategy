using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private GameObject actionCamera;
    private Unit shootingUnit;
    private Unit targetUnit;  
    private bool isActive = false;

    private void Awake()
    {
        HideActionCamera();
    }

    private void Update()
    {
        if (!isActive)
        {
            return;
        }

        if (shootingUnit != null)
        {
            actionCamera.transform.position = shootingUnit.GetActionCameraSpot().position;
        }
        
        if(targetUnit != null)
        {
            actionCamera.transform.LookAt(targetUnit.GetWorldPosition() + Vector3.up * 1.7f);
        }       
    }


    private void Start()
    {
        ShootAction.OnShootCameraStart += ShootAction_OnShootCameraStart;
        ShootAction.OnShootCameraEnd += ShootAction_OnShootCameraEnd;
    }


    private void ShootAction_OnShootCameraStart(object sender, ShootAction.OnShootStartEventArgs e)
    {
        SetUpCamera(e.shootingUnit, e.targetUnit);

        isActive = true;

        ShowActionCamera();
    }

    private void ShootAction_OnShootCameraEnd(object sender, EventArgs e)
    {                  
        HideActionCamera();
        isActive = false;
    }

    private void ShowActionCamera()
    {   
        actionCamera.SetActive(true);
    }

    private void HideActionCamera()
    {
        actionCamera.SetActive(false);
    }

    private void SetUpCamera(Unit shootingUnit, Unit targetUnit)
    {
        this.targetUnit = targetUnit;
      
        this.shootingUnit = shootingUnit;
    }
}
