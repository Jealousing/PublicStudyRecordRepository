using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(GridLayoutGroup))]
public class ResizeGrid : MonoBehaviour
{
    public int rows;
    public int columns;
    public Vector2 spacing;
    public Vector2 padding;

    public GridLayoutGroup grid;
    public RectTransform rectTransform;

    private void Start()
    {
        Resize();
    }

    private void OnRectTransformDimensionsChange()
    {
        Resize();
    }

    private void Resize()
    {
        float width = rectTransform.rect.width;
        float height = rectTransform.rect.height;

        Vector2 parentPivot = rectTransform.pivot;

        float xOffset = (1 - parentPivot.x) * width;
        float yOffset = (1 - parentPivot.y) * height;

        float totalWidth = width - padding.x * 2 - xOffset * 2;
        float totalHeight = height - padding.y * 2 - yOffset * 2;

        float cellWidth = (totalWidth - spacing.x * (columns - 1)) / columns;
        float cellHeight = (totalHeight - spacing.y * (rows - 1)) / rows;

        float cellSize = Mathf.Min(cellWidth, cellHeight);

        grid.cellSize = new Vector2(cellSize, cellSize);
        grid.spacing = spacing;

        float newPaddingX = (totalWidth - cellSize * columns - spacing.x * (columns - 1)) / 2;
        float newPaddingY = (totalHeight - cellSize * rows - spacing.y * (rows - 1)) / 2;

        grid.padding = new RectOffset((int)(padding.x + newPaddingX + xOffset), (int)(padding.x + newPaddingX - xOffset),
                                       (int)(padding.y + newPaddingY + yOffset), (int)(padding.y + newPaddingY - yOffset));
    }
}
