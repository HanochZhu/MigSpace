using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(GridLayoutGroup))]
public class CenteredGridLayoutGroup : MonoBehaviour
{
    private GridLayoutGroup gridLayoutGroup;
    private RectTransform rectTransform;

    void Start()
    {
        gridLayoutGroup = GetComponent<GridLayoutGroup>();
        rectTransform = GetComponent<RectTransform>();
        AlignChildren();
    }

    void AlignChildren()
    {
        int childCount = gridLayoutGroup.transform.childCount;
        if (childCount == 0) return;

        Vector2 parentSize = rectTransform.rect.size;
        float cellWidth = gridLayoutGroup.cellSize.x + gridLayoutGroup.spacing.x;
        float cellHeight = gridLayoutGroup.cellSize.y + gridLayoutGroup.spacing.y;

        int columns = Mathf.FloorToInt(parentSize.x / cellWidth);
        int rows = Mathf.FloorToInt(parentSize.y / cellHeight);

        for (int i = 0; i < childCount; i++)
        {
            RectTransform child = gridLayoutGroup.transform.GetChild(i) as RectTransform;
            int row = i / columns;
            int column = i % columns;

            float offsetX = (columns - 1) * cellWidth * 0.5f;
            float offsetY = (rows - 1) * cellHeight * 0.5f;

            Vector2 anchoredPosition = new Vector2(column * cellWidth - offsetX, -row * cellHeight + offsetY);
            child.anchoredPosition = anchoredPosition;
        }
    }
}