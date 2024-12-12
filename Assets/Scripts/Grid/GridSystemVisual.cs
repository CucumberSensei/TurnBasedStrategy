using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystemVisual : MonoBehaviour
{
    public static GridSystemVisual Instance { get; private set; }

    [Serializable]
    public struct GridVisualMaterial
    {
        public GridVisualMaterialName name;
        public Material material;
    }

    public enum GridVisualMaterialName
    {
        White,
        Red,
        Blue,
        SoftRed,
        Yellow,
    }

    [SerializeField] private List<GridVisualMaterial> GridVisualMaterialsList;
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
            for (int z = 0; z < height; z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                Vector3 worldPosition = LevelGrid.Instance.GetWorldPosition(gridPosition);
                Transform gridVisual = Instantiate(GridSystemSingleVisualTransform, worldPosition, Quaternion.identity);
                GridSystemSingleVisual gridSystemSingleVisual = gridVisual.GetComponent<GridSystemSingleVisual>();
                gridSystemSingleVisualsArray[x, z] = gridSystemSingleVisual;
            }
        }

        UnitActionSystem.Instance.OnSelectedUnitChanged += UnitActionSystem_OnSelectedUnitChanged;
        UnitActionSystem.Instance.OnSelectedActionChange += UnitActionSystem_OnSelectedActionChange;
        LevelGrid.Instance.OnAnyUnitChangeGridPosition += LevelGrid_OnAnyUnitChangeGridPosition;

        UpdateGridVisuals();
    }

    private void LevelGrid_OnAnyUnitChangeGridPosition(object sender, EventArgs e)
    {
        UpdateGridVisuals();
    }

    private void UnitActionSystem_OnSelectedActionChange(object sender, System.EventArgs e)
    {
        UpdateGridVisuals();
    }

    private void UnitActionSystem_OnSelectedUnitChanged(object sender, System.EventArgs e)
    {
        UpdateGridVisuals();
    }

    public void HideAllVisuals()
    {
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                gridSystemSingleVisualsArray[x, z].Hide();
            }
        }

    }

    public void ShowGridPositionListVisual(List<GridPosition> gridPositions, GridVisualMaterialName gridVisualName)
    {
        foreach (GridPosition gridPosition in gridPositions)
        {
            gridSystemSingleVisualsArray[gridPosition.x, gridPosition.z].Show(GetGridVisualMaterialFromList(gridVisualName));
        }
    }

    public void UpdateGridVisuals()
    {
        HideAllVisuals();

        BaseAction selectedAction = UnitActionSystem.Instance.GetSelectedAction();

        GridVisualMaterialName gridVisualMaterialName;

        switch (selectedAction)
        {
            default:
            case MoveAction moveAction:
                gridVisualMaterialName = GridVisualMaterialName.White;
                break;
            case ShootAction shotAction:
                gridVisualMaterialName = GridVisualMaterialName.Red;
                ShowActionRange(shotAction.GetMaxShootDistance(), UnitActionSystem.Instance.GetSelectedUnit());
                break;
            case SwordAction swordAction:
                gridVisualMaterialName = GridVisualMaterialName.Red;
                ShowActionRange(swordAction.GetMaxSwordDistance(), UnitActionSystem.Instance.GetSelectedUnit());
                break;
            case GrenadeAction grenadeAction:
                gridVisualMaterialName = GridVisualMaterialName.Yellow;
                break;
            case SpinAction spinAction:
                gridVisualMaterialName = GridVisualMaterialName.Blue;
                break;
        }

        ShowGridPositionListVisual(selectedAction.GetValidActionGridPositionList(), gridVisualMaterialName);
    }

    private Material GetGridVisualMaterialFromList(GridVisualMaterialName name)
    {
        foreach (GridVisualMaterial gridVisualMaterial in GridVisualMaterialsList)
        {
            if (gridVisualMaterial.name == name)
            {
                return gridVisualMaterial.material;
            }
        }

        return null;
    }

    private void ShowActionRange(int range, Unit actionUnit)
    {
        List<GridPosition> shootRangeGridPositionList = new List<GridPosition>();


        for (int x = -range; x <= range; x++)
        {
            for (int z = -range; z <= range; z++)
            {
                GridPosition offsetGrisPosition = new GridPosition(x, z);
                GridPosition targetGridPositon = actionUnit.GetGridPosition() + offsetGrisPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(targetGridPositon))
                {
                    continue;
                }

                Vector3 unitWordlPosition = LevelGrid.Instance.GetWorldPosition(actionUnit.GetGridPosition());
                Vector3 targetWorldPosition = LevelGrid.Instance.GetWorldPosition(targetGridPositon);

                if (Vector3.Distance(unitWordlPosition, targetWorldPosition) > 10.5)
                {
                    continue;
                }


                shootRangeGridPositionList.Add(targetGridPositon);

            }

        }

        ShowGridPositionListVisual(shootRangeGridPositionList, GridVisualMaterialName.SoftRed);
    }
}
