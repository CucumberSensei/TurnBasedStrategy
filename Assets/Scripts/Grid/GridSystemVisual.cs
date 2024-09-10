using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystemVisual : MonoBehaviour
{
    public static GridSystemVisual Instance { get; private set; }


    [SerializeField] private Transform GridSystemSingleVisualTransform;
    private GridSystemSingleVisual[,] gridSystemSingleVisualsArray;
    private int width;
    private int height;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one Grid System Visual" + transform);
        }
        Instance = this;
    }


    // Start is called before the first frame update
    void Start()
    {
        width = LevelGrid.Instance.GetWidth();
        height = LevelGrid.Instance.GetHeight();

        gridSystemSingleVisualsArray = new GridSystemSingleVisual[width, height];

        for (int x = 0; x < width; x++)
        {
            for(int z = 0; z< height; z++)
            {
                GridPosition gridPosition = new GridPosition(x,z);
                Vector3 worldPosition = LevelGrid.Instance.GetWorldPosition(gridPosition);
                Transform gridVisual = Instantiate(GridSystemSingleVisualTransform, worldPosition, Quaternion.identity);
                GridSystemSingleVisual gridSystemSingleVisual = gridVisual.GetComponent<GridSystemSingleVisual>();
                gridSystemSingleVisualsArray[x,z] = gridSystemSingleVisual;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateGridVisuals();
    }

    public void HideAllVisuals()
    {
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                gridSystemSingleVisualsArray[x,z].Hide();
            }
        }

    }

    public void ShowAllVisuals()
    {
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                gridSystemSingleVisualsArray[x, z].Show();
            }
        }
    }

    public void ShowGridPositionListVisual(List<GridPosition> gridPositions)
    {
        foreach (GridPosition gridPosition in gridPositions)
        {
            gridSystemSingleVisualsArray[gridPosition.x, gridPosition.z].Show();
        }
    }

    public void UpdateGridVisuals()
    {   
        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();
        HideAllVisuals();
        ShowGridPositionListVisual(selectedUnit.GetMoveAction().GetValidActionGridPositionList());
    }


}
