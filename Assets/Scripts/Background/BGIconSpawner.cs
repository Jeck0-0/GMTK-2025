using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;


// Horrible code. Look away
public class BGIconSpawner : MonoBehaviour
{
    [SerializeField] protected GameObject iconPrefab;
    [SerializeField] protected float distance;
    [SerializeField] protected Vector2 offset;
    [SerializeField] protected List<Vector2Int> ignoreTiles;
    [SerializeField] protected Vector2Int size;
    [SerializeField] protected Dictionary<Vector2Int, BackgroundItem> grid = new();
    [SerializeField, ColorPalette] protected Color color = new Color(0.4627451f, 0.2588235f, 0.5411765f); 
    void Start()
    {
        col = color;
        SpawnTiles();
    }


    private void LateUpdate()
    {
        UpdateTiles();
    }

    private void UpdateTiles()
    {
        //var cursorPos = Helpers.Camera.ScreenToWorldPoint(Input.mousePosition);
        var cursorPos = Player.instance.transform.position;
        cursorPos.z = transform.position.z;

        foreach (var tile in grid)
        {
            if(tile.Value.gameObject.activeInHierarchy)
                UpdateTile(tile.Key, tile.Value, cursorPos);
        }
    }

    protected Color col;
    protected virtual void UpdateTile(Vector2Int pos, BackgroundItem item, Vector3 cursorPos)
    {
        var dir = (cursorPos - item.transform.position).normalized;
        var dist = Vector3.Distance(item.transform.position, cursorPos);

        item.icon.transform.localScale = Vector3.one * item.scaleCurve.Evaluate(dist);

        item.icon.transform.localRotation = Quaternion.Euler(0, 0, item.rotationCurve.Evaluate(dist) * 90);

        item.icon.transform.localPosition = dir * item.positionCurve.Evaluate(dist);

        item.icon.color = item.colorGradient.Evaluate(dist / item.colorGradientDistance);
    }

    protected virtual void SpawnTiles()
    {
        Vector2Int p;
        for (int x = -size.x / 2; x < size.x / 2; x++)
        {
            for (int y = -size.y / 2; y < size.y / 2; y++)
            {
                Vector3 spawnPos = new Vector3(x * distance + offset.x, y * distance + offset.y, 0);
                p = new Vector2Int(x, y);
                if (!ignoreTiles.Contains(p))
                {
                    var go = Instantiate(iconPrefab, transform.position + spawnPos, Quaternion.identity, transform);
                    if (go.TryGetComponent(out BackgroundItem item))
                    {
                        grid.Add(p, item);
                    }
                    else
                    {
                        Debug.LogError("Background item does not have BackgroundItem script", item);
                    }
                }
            }
        }

        //hex grid
        /*for (int x = -size.x / 2; x < size.x / 2; x++) 
        {
            for (int y = -size.y / 2; y < size.y / 2; y++)
            {
                float offset = y % 2 == 0 ? 0 : (2f/3f*distance);
                Vector3 spawnPos = new Vector3(x * Mathf.Sqrt(3) * (distance * 2 / 3) + offset, y * (3 / 2) * distance, 0);
                p = new Vector2Int(x, y);
                if (!ignoreTiles.Contains(p))
                    Instantiate(iconPrefab, spawnPos, Quaternion.identity);
            }
        }*/
    }
}

