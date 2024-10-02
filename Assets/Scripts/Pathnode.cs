using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode
{
    private int gCost;
    private int hCost;
    private int fCost;
    private GridPosition gridPosition;
    private PathNode pathNode;

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
}
