using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpinAction : BaseAction
{   

    private float totalSpinAmount;
    private Action onSpinComplete;

    private void Update()
    {   
        if (!isActive)
        {
            return;
        }

        float spiningAmount = 360f * Time.deltaTime;

        transform.eulerAngles += new Vector3(0, spiningAmount, 0);
        totalSpinAmount += spiningAmount;

        if (totalSpinAmount >= 360) 
        { 
            isActive = false;
            onSpinComplete();
        }
    }

    public void StartSpining(Action onSpinComplete)
    {   
        this.onSpinComplete = onSpinComplete;
        totalSpinAmount = 0;
        isActive = true;
    }

    public override string GetActionName()
    {
        return "Spin";
    }
}
