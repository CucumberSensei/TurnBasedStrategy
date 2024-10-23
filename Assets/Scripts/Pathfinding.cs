using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

                                                         //PATHFINDING A*
public class Pathfinding : MonoBehaviour
{
    public static Pathfinding Instance { get; private set; }
    [SerializeField] private LayerMask obstacleLayerMask;
    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;

    [SerializeField] private Transform pathFindingGridDebugObject;
    
    private GridSystem<PathNode> gridSystem;

    private void Awake()
    {
        if (Instance != null)
        {
            UnityEngine.Debug.LogError("There is more than one Pathfinding" + transform);
        }
        Instance = this;      
    }

    public void Setup(int heigth, int width, float cellSize) 
    {
        gridSystem = new GridSystem<PathNode>(heigth, width, cellSize,
           (GridSystem<PathNode> gridSystem, GridPosition gridPosition) => new PathNode(gridPosition));

       // gridSystem.CreateDebugObjects(pathFindingGridDebugObject);

        for (int x = 0; x < width ; x++)
        {
            for(int z = 0; z < heigth; z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                Vector3 worldPosition = gridSystem.GetWorldPosition(gridPosition);
                if (Physics.Raycast(worldPosition + Vector3.down * 5f, Vector3.up, 50f, obstacleLayerMask))
                {
                    GetNode(x, z).SetIsWalkable(false);
                }
            }
        }
       
    }

    public List<GridPosition> FindPath(GridPosition startGridPosition, GridPosition endGridPosition, out int pathDistance)
    {        
        List<PathNode> openList = new List<PathNode>();
        List<PathNode> closedList = new List<PathNode>();
       

        for(int x = 0; x < gridSystem.GetWidth(); x++)
        {
            for(int z =0; z < gridSystem.GetHeight(); z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                PathNode pathNode = gridSystem.GetGridObjectAtGridPosition(gridPosition);

                pathNode.SetGCost(int.MaxValue);
                pathNode.SetHCost(0);
                pathNode.CalculateFCost();
                pathNode.ResetCameFromPathNode();
            }
        }


        PathNode startPathNode = gridSystem.GetGridObjectAtGridPosition(startGridPosition);
        PathNode endPathNode = gridSystem.GetGridObjectAtGridPosition(endGridPosition);
        openList.Add(startPathNode);

        startPathNode.SetGCost(0);
        startPathNode.SetHCost(CalculateDistance(startGridPosition, endGridPosition));
        startPathNode.CalculateFCost();


        while(openList.Count > 0)
        {
            PathNode currentNode = GetTheLowestFCostNode(openList);

            if(currentNode == endPathNode)
            {
                //Reached the final node
                pathDistance = endPathNode.GetFCost();
                return CalculatePath(endPathNode);
                
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            foreach (PathNode neighbourNode in GetNeighbourList(currentNode))
            {
                if (closedList.Contains(neighbourNode))
                {
                    continue;
                }

                if (!neighbourNode.IsWalkable())
                {
                    closedList.Add(neighbourNode);
                    continue;
                }

                int tentativeGCost =
                    currentNode.GetGCost() + CalculateDistance(currentNode.GetGridPosition(), neighbourNode.GetGridPosition());

                if(tentativeGCost < neighbourNode.GetGCost())
                {
                    neighbourNode.SetGCost(tentativeGCost);
                    neighbourNode.SetHCost(CalculateDistance(neighbourNode.GetGridPosition(), endGridPosition ));
                    neighbourNode.CalculateFCost();
                    neighbourNode.SetCameFromPathNode(currentNode);

                    if (!openList.Contains(neighbourNode))
                    {
                        openList.Add(neighbourNode);
                    }

                }
            }
            
        }
        
        pathDistance = 0;
        return null;
    }

    private List<GridPosition> CalculatePath(PathNode endNode)
    {
        List<GridPosition> pathGridPositionList = new List<GridPosition>();

        List<PathNode> pathNodeList = new List<PathNode>();
        pathNodeList.Add(endNode);

        PathNode currentNode = endNode;
        while(currentNode.GetCameFromPathNode() != null)
        {
            pathNodeList.Add(currentNode.GetCameFromPathNode());
            currentNode = currentNode.GetCameFromPathNode();
        }

        pathNodeList.Reverse();

        foreach (PathNode pathNode in pathNodeList)
        {
            pathGridPositionList.Add(pathNode.GetGridPosition());
        }


        return pathGridPositionList;
    }

    public int CalculateDistance(GridPosition a, GridPosition b)
    {
        GridPosition gridDistance = a - b;
        int xDistance = Mathf.Abs(gridDistance.x);
        int zDistance = Mathf.Abs(gridDistance.z);
        int remaing = Mathf.Abs(xDistance - zDistance);
        

        return Mathf.Min(xDistance,zDistance) * MOVE_DIAGONAL_COST + remaing * MOVE_STRAIGHT_COST;
    }

    private PathNode GetTheLowestFCostNode(List<PathNode> pathNodeList)
    {
        PathNode lowestFCostNode = pathNodeList[0];

        foreach (PathNode node in pathNodeList)
        {
            if (node.GetFCost() < lowestFCostNode.GetFCost())
            {
                lowestFCostNode = node;
            }
        }

        return lowestFCostNode; 
    }

    private PathNode GetNode(int x, int z)
    {      
        return gridSystem.GetGridObjectAtGridPosition(new GridPosition(x, z));
    }

    public List<PathNode> GetNeighbourList(PathNode currentNode)
    {
        List<PathNode> neighbourList = new List<PathNode>();

        GridPosition gridPosition = currentNode.GetGridPosition();


        if(gridPosition.x - 1 >= 0)
        {
            //Left
            neighbourList.Add(GetNode(gridPosition.x - 1, gridPosition.z + 0));

            if(gridPosition.z + 1 < gridSystem.GetHeight())
            {
                //LeftUp
                neighbourList.Add(GetNode(gridPosition.x - 1, gridPosition.z + 1));
            }
            
            if(gridPosition.z - 1 >= 0)
            {
                //LeftDown
                neighbourList.Add(GetNode(gridPosition.x - 1, gridPosition.z - 1));
            }           
        }

        if(gridPosition.x + 1 <= gridSystem.GetWidth())
        {
            //Right
            neighbourList.Add(GetNode(gridPosition.x + 1, gridPosition.z + 0));

            if(gridPosition.z + 1 < gridSystem.GetHeight())
            {
                //RightUp
                neighbourList.Add(GetNode(gridPosition.x + 1, gridPosition.z + 1));
            }
            
            if(gridPosition.z - 1 >= 0)
            {
                //RightDown
                neighbourList.Add(GetNode(gridPosition.x + 1, gridPosition.z - 1));
            }           
        }
        
        if(gridPosition.z + 1 < gridSystem.GetHeight())
        {
            //Up
            neighbourList.Add(GetNode(gridPosition.x + 0, gridPosition.z + 1));
        }
        
        if(gridPosition.z - 1 >= 0)
        {
            //Down
            neighbourList.Add(GetNode(gridPosition.x + 0, gridPosition.z - 1));
        }     

        return neighbourList;
    }

    public PathNode GetPathNode(GridPosition gridPosition)
    {
        return gridSystem.GetGridObjectAtGridPosition(gridPosition);
    }

    public bool IsWalkableGridPosition(GridPosition gridPosition)
    {
        return gridSystem.GetGridObjectAtGridPosition(gridPosition).IsWalkable();
    }

    public bool HasPath(GridPosition startGridPosition, GridPosition endGridPosition)
    {
        return FindPath(startGridPosition, endGridPosition, out int pathDistance) != null;
    }

    public int GetPathDistance(GridPosition startGridPosition, GridPosition endGridPosition)
    {
        FindPath(startGridPosition, endGridPosition, out int pathDistance);
        return pathDistance;
    }
}
