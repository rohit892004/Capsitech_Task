using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int width = 4;
    public int height = 4;

    public float cellSize = 1.5f;

    public GameObject cellPrefab;
    public Transform gridRoot;

    private float startX;
    private float startY;

    private void Awake()
    {
        startX = -(width - 1) * cellSize * 0.5f;
        startY = -(height - 1) * cellSize * 0.5f;
    }

    private void Start()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        foreach (Transform child in gridRoot)
            Destroy(child.gameObject);

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Vector3 pos = GetWorldPosition(x, y);

                GameObject cell =
                    Instantiate(
                        cellPrefab,
                        pos,
                        Quaternion.identity,
                        gridRoot);

                cell.name = $"Cell_{x}_{y}";
            }
        }
    }

    public Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(
            startX + x * cellSize,
            startY + y * cellSize,
            0
        );
    }
    public Vector2Int WorldToGrid(Vector3 worldPos)
{
    float startX = -(width - 1) * cellSize * 0.5f;
    float startY = -(height - 1) * cellSize * 0.5f;

    int x = Mathf.RoundToInt((worldPos.x - startX) / cellSize);
    int y = Mathf.RoundToInt((worldPos.y - startY) / cellSize);

    return new Vector2Int(x, y);
}
}