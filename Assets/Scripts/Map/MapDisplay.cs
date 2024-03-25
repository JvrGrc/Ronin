using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public struct MapTilePair
{
    public TileType type;
    public TileBase visualTile;
}

public class MapDisplay : MonoBehaviour
{
    
    [SerializeField] MapTilePair[] mapTilePairs;
    [SerializeField] Tilemap tilemapTiles;
    [SerializeField] Tilemap decorationTiles;
    [SerializeField] Camera gameCamera;
    [SerializeField] GameObject cursor;
    [SerializeField] TileType[] walls;

    private MapReader map;
    public MapTilePair[] GetMapTilePairs() { return mapTilePairs; }

    //  Gets the information from the mapreader and fills the tilemap
    public List<Vector3Int> RenderMapData(MapReader mapData)
    {
        List <Vector3Int> forbidden = new List<Vector3Int> ();
        this.map = mapData;

        for (int x = 0; x < this.map.width; x++)
        {
            for (int y = 0; y < this.map.height; y++)
            {
                TileType type = this.map.GetTileType(x,y);

                TileBase tile = this.GetTileForType(type);
                foreach (TileType tileType in walls) {
                    if (type.Equals(tileType)){
                        forbidden.Add(new Vector3Int(x, -y, 1));
                        break;
                    }
                }

                this.tilemapTiles.SetTile(new Vector3Int(x, -y, 1), tile);
            }
        }
        return forbidden;
    }

    //  Gets the information from the mapreader and fills the tilemap (Decoration version)
    public List<Vector3Int> RenderDecoration(MapReader decorationData)
    {
        List<Vector3Int> forbidden = new List<Vector3Int>();
        this.map = decorationData;

        for (int x = 0; x < this.map.width; x++)
        {
            for (int y = 0; y < this.map.height; y++)
            {
                TileType type = this.map.GetTileType(x, y);

                TileBase tile = this.GetTileForType(type);
                foreach (TileType tileType in walls)
                {
                    if (type.Equals(tileType))
                    {
                        forbidden.Add(new Vector3Int(x, -y, 1));
                        break;
                    }
                }

                this.decorationTiles.SetTile(new Vector3Int(x, -y, 0), tile);
            }
        }

        return forbidden;
    }

    private TileBase GetTileForType(TileType type)
    {
        foreach(var pair in this.mapTilePairs)
        {
            if (pair.type == type)
                return pair.visualTile;
        }

        Debug.LogError("No hay tile para: " + type);
        return null;
    }
}