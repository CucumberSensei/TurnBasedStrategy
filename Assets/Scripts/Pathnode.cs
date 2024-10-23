using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode
{
    private int gCost;
    private int hCost;
    private int fCost;
    private bool isWalkable = true;
    private GridPosition gridPosition;
    private PathNode cameFromPathNode;

    public PathNode(GridPosition gridPosition)
    {
        this.gridPosition = gridPosition;
    }

    public override string ToString()
    {

        return this.gridPosition.ToString();
    }

    public int GetGCost()
    {
        return this.gCost;
    }

    public int GetHCost()
    {
        return this.hCost;
    }

    public int GetFCost()
    {
        return this.fCost;
    }

    public void SetGCost(int gCost) 
    {
        this.gCost = gCost;
    }

    public void SetHCost(int hCost)
    {
        this.hCost = hCost;
    }
    public void CalculateFCost()
    {
        fCost = gCost + hCost;
    }

    public void ResetCameFromPathNode()
    {
        cameFromPathNode = null;
    }

    public void SetCameFromPathNode(PathNode cameFromPathNode)
    {
        this.cameFromPathNode = cameFromPathNode;
    }

    public PathNode GetCameFromPathNode()
    {
        return cameFromPathNode;
    }

    public GridPosition GetGridPosition()
    {
        return gridPosition;
    }

    public bool IsWalkable()
    {
        return isWalkable;
    }

    public void SetIsWalkable(bool isWalkable)
    {
        this.isWalkable = isWalkable;
    }

}
