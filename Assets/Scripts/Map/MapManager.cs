using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{
    private static MapManager _instance;
    public static MapManager Instance { get { return _instance; } }

    [SerializeField] GameObject overlayPrefab;
    [SerializeField] GameObject overlayContainer;
    [SerializeField] GameObject charactersContainer;
    [SerializeField] Tilemap tilemap;

    private Dictionary<Vector2Int, OverlayTile> map;

    public Dictionary<Vector2Int, OverlayTile> GetMap()
    {
        return map;
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    //  Create the tiles where the arrows and the cursor are going to be printed
    public Dictionary<Vector2Int, OverlayTile> CreateOverlayTiles(List<Vector3Int> forbidden)
    {
        map = new Dictionary<Vector2Int, OverlayTile>();

        BoundsInt bounds = tilemap.cellBounds;

        for (int z = bounds.max.z; z > bounds.min.z; z--)
        {
            for (int y = bounds.min.y; y < bounds.max.y; y++)
            {
                for (int x = bounds.min.x; x < bounds.max.x; x++)
                {
                    if (tilemap.HasTile(new Vector3Int(x, y, z)))
                    {
                        if (!forbidden.Contains(new Vector3Int(x, y, z)) && !map.ContainsKey(new Vector2Int(x, y)))
                        {
                            var overlayTile = Instantiate(overlayPrefab, overlayContainer.transform);
                            var cellWorldPosition = tilemap.GetCellCenterWorld(new Vector3Int(x, y, z));
                            overlayTile.transform.position = new Vector3(cellWorldPosition.x, cellWorldPosition.y, cellWorldPosition.z + 1);
                            overlayTile.GetComponent<SpriteRenderer>().sortingOrder = tilemap.GetComponent<TilemapRenderer>().sortingOrder;
                            overlayTile.gameObject.GetComponent<OverlayTile>().SetGridLocation(new Vector3Int(x, y, z));

                            map.Add(new Vector2Int(x, y), overlayTile.gameObject.GetComponent<OverlayTile>());
                        }
                    }
                }
            }
        }
        return map;
    }

    public List<OverlayTile> GetSurroundingTiles(Vector2Int originTile)
    {
        var surroundingTiles = new List<OverlayTile>();


        Vector2Int TileToCheck = new Vector2Int(originTile.x + 1, originTile.y);
        if (map.ContainsKey(TileToCheck))
        {
            if (Mathf.Abs(map[TileToCheck].transform.position.z - map[originTile].transform.position.z) <= 1)
                surroundingTiles.Add(map[TileToCheck]);
        }

        TileToCheck = new Vector2Int(originTile.x - 1, originTile.y);
        if (map.ContainsKey(TileToCheck))
        {
            if (Mathf.Abs(map[TileToCheck].transform.position.z - map[originTile].transform.position.z) <= 1)
                surroundingTiles.Add(map[TileToCheck]);
        }

        TileToCheck = new Vector2Int(originTile.x, originTile.y + 1);
        if (map.ContainsKey(TileToCheck))
        {
            if (Mathf.Abs(map[TileToCheck].transform.position.z - map[originTile].transform.position.z) <= 1)
                surroundingTiles.Add(map[TileToCheck]);
        }

        TileToCheck = new Vector2Int(originTile.x, originTile.y - 1);
        if (map.ContainsKey(TileToCheck))
        {
            if (Mathf.Abs(map[TileToCheck].transform.position.z - map[originTile].transform.position.z) <= 1)
                surroundingTiles.Add(map[TileToCheck]);
        }

        return surroundingTiles;
    }

    public List<OverlayTile> GetOccupiedTiles()
    {
        var occupiedTiles = new List<OverlayTile>();

        foreach (CharacterInfo character in charactersContainer.GetComponent<CharacterContainer>().GetAllCharacters())
        {
            occupiedTiles.Add(character.GetCharacterStandingOnTile());
        }

        return occupiedTiles;
    }
}
