using System.IO;
using System.Collections.Generic;

public enum TileType
{
    TILE1, TILE2, TILE3, TILE4, TILE5, TILE6,
    TILE7, TILE8, TILE9, TILE10, TILE11,
    TILE12, TILE13, TILE14, TILE15, TILE16,
    TILE17, TILE18, TILE19, TILE20, TILE21,
    TILE22, TILE23, TILE24, TILE25, TILE26,
    TILE27, TILE28, TILE29, TILE30, TILE31,
    TILE32, TILE33, TILE34, TILE35, EMPTY
}

public class MapReader
{
    public int width { get; protected set; }
    public int height { get; protected set; }

    private TileType[,] tilesData;

    public MapReader(TileType[,] tilesData)
    {
        this.tilesData = tilesData;

        this.width = this.tilesData.GetLength(0);

        this.height = this.tilesData.GetLength(1);
    }

    public TileType GetTileType(int x, int y)
    {
        return this.tilesData[x, y];
    }

    public static MapReader CreateWithStringData(string mapData)
    {

        StringReader reader = new StringReader(mapData);

        int mapWidth = 0;
        int mapHeight = 0;

        List<TileType> floatTilesData = new List<TileType>();

        while (true)
        {
            string line = reader.ReadLine();

            if (line == null) break;

            line = line.Trim();

            if (line.Length == 0) continue;

            mapWidth = line.Length;
            mapHeight++;

            foreach (var letter in line)
            {
                switch (letter)
                {
                    case '0':
                        floatTilesData.Add(TileType.TILE1);
                        break;
                    case '1':
                        floatTilesData.Add(TileType.TILE2);
                        break;
                    case '2':
                        floatTilesData.Add(TileType.TILE3);
                        break;
                    case '3':
                        floatTilesData.Add(TileType.TILE4);
                        break;
                    case '4':
                        floatTilesData.Add(TileType.TILE5);
                        break;
                    case '5':
                        floatTilesData.Add(TileType.TILE6);
                        break;
                    case '6':
                        floatTilesData.Add(TileType.TILE7);
                        break;
                    case '7':
                        floatTilesData.Add(TileType.TILE8);
                        break;
                    case '8':
                        floatTilesData.Add(TileType.TILE9);
                        break;
                    case '9':
                        floatTilesData.Add(TileType.TILE10);
                        break;
                    case 'a':
                        floatTilesData.Add(TileType.TILE11);
                        break;
                    case 'b':
                        floatTilesData.Add(TileType.TILE12);
                        break;
                    case 'c':
                        floatTilesData.Add(TileType.TILE13);
                        break;
                    case 'd':
                        floatTilesData.Add(TileType.TILE14);
                        break;
                    case 'e':
                        floatTilesData.Add(TileType.TILE15);
                        break;
                    case 'f':
                        floatTilesData.Add(TileType.TILE16);
                        break;
                    case 'g':
                        floatTilesData.Add(TileType.TILE17);
                        break;
                    case 'h':
                        floatTilesData.Add(TileType.TILE18);
                        break;
                    case 'i':
                        floatTilesData.Add(TileType.TILE19);
                        break;
                    case 'j':
                        floatTilesData.Add(TileType.TILE20);
                        break;
                    case 'k':
                        floatTilesData.Add(TileType.TILE21);
                        break;
                    case 'l':
                        floatTilesData.Add(TileType.TILE22);
                        break;
                    case 'm':
                        floatTilesData.Add(TileType.TILE23);
                        break;
                    case 'n':
                        floatTilesData.Add(TileType.TILE24);
                        break;
                    case 'o':
                        floatTilesData.Add(TileType.TILE25);
                        break;
                    case 'p':
                        floatTilesData.Add(TileType.TILE26);
                        break;
                    case 'q':
                        floatTilesData.Add(TileType.TILE27);
                        break;
                    case 'r':
                        floatTilesData.Add(TileType.TILE28);
                        break;
                    case 's':
                        floatTilesData.Add(TileType.TILE29);
                        break;
                    case 't':
                        floatTilesData.Add(TileType.TILE30);
                        break;
                    case 'u':
                        floatTilesData.Add(TileType.TILE31);
                        break;
                    case 'v':
                        floatTilesData.Add(TileType.TILE32);
                        break;
                    case 'x':
                        floatTilesData.Add(TileType.EMPTY);
                        break;
                    case 'w':
                        floatTilesData.Add(TileType.TILE33);
                        break;
                    case 'y':
                        floatTilesData.Add(TileType.TILE34);
                        break;
                    case 'z':
                        floatTilesData.Add(TileType.TILE35);
                        break;

                }
            }
        }

        TileType[,] finalMapData = new TileType[mapWidth, mapHeight];

        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                finalMapData[x, y] = floatTilesData[y * mapWidth + x];
            }
        }

        return new MapReader(finalMapData);
    }
}