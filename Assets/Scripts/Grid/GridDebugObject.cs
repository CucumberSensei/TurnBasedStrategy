using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GridDebugObject : MonoBehaviour
{
    private object gridObject;
    [SerializeField] private TextMeshPro textMeshPro;

    protected virtual void Update()
    {
        textMeshPro.text = gridObject.ToString();
    }

    public virtual void SetGridObject(object gridObject)
    {
        this.gridObject = gridObject;
    }

}
