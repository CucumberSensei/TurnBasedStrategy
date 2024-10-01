using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System;
using UnityEngine.UI;

public class UnitWorldUI : MonoBehaviour
{
    [SerializeField] private Unit unit;
    [SerializeField] private TextMeshProUGUI actionPoints;
    [SerializeField] private Image healthBar;
    [SerializeField] private HealthSystem healthSystem;


    private void Start()
    {
        Unit.OnAnyActionPointChange += Unit_OnAnyActionPointsChage;
        healthSystem.OnDamage += HealthSystem_Ondamage;
        UpdateActionPoinstWorldVisual();
        UpdateHealthBarAmount();
    }

    private void HealthSystem_Ondamage(object sender, EventArgs e)
    {
        UpdateHealthBarAmount();
    }

    private void Unit_OnAnyActionPointsChage(object sender, EventArgs e)
    {
        UpdateActionPoinstWorldVisual();
    }

    private void UpdateActionPoinstWorldVisual()
    {
        actionPoints.text = unit.GetActionPoints().ToString();
    }

    private void UpdateHealthBarAmount()
    {
        healthBar.fillAmount = healthSystem.GetHealthNormalized();
    }


}
