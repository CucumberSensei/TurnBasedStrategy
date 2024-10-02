using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PathFindingGridDebugObject : GridDebugObject
{
    [SerializeField] private TextMeshPro gCost;
    [SerializeField] private TextMeshPro hCost;
    [SerializeField] private TextMeshPro fCost;
    private PathNode pathNode;

    
    protected override void Update()
    {
        base.Update();
        gCost.text = pathNode.GetGCost().ToString();
        hCost.text = pathNode.GetHCost().ToString();
        fCost.text = pathNode.GetFCost().ToString();
        
    }

    public override void SetGridObject(object gridObject)
    {
        base.SetGridObject(gridObject);
        pathNode = (PathNode)gridObject;        
    }
}
