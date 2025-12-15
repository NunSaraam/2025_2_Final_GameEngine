using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum IslandType
{
    Main,
    Resource,
    Mineral,
    Flora,
}

public class PerlinNoiseVoxelMap : MonoBehaviour
{
    public class TileInfos
    {
        public int tileX;
        public int tileY;
        public int tileZ;
        public string tileType;
    }

    public IslandType islandType = IslandType.Resource;

    public GameObject grassPrefab;
    public GameObject dirtPrefab;
    public GameObject waterPrefab;
    public GameObject mineralPrefab;
    public GameObject mushroomPrefab;


    public int width = 20;

    public int depth = 20;

    public int maxHeight = 16;

    private bool[,,] hasBlock;

    [SerializeField] float noiseScale = 20f;

    List<TileInfos> tileInfos = new List<TileInfos>();

    private void Start()
    {
        GenerateIsland();
    }

    public void StartGeneration()
    {
        GenerateIsland();
    }

    void GenerateIsland()
    {
        float offsetX = Random.Range(-9999f, 9999f);
        float offsetZ = Random.Range(-9999f, 9999f);

        hasBlock = new bool[width, maxHeight, depth];

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < depth; z++)
            {
                float nx = (x + offsetX) / noiseScale;
                float nz = (z + offsetZ) / noiseScale;
                float noise = Mathf.PerlinNoise(nx, nz);
                int h = Mathf.FloorToInt(noise * maxHeight);

                if (h <= 0) continue;

                for (int y = 0; y <= h; y++)
                {
                    if (y == h)
                        PlaceTopBlock(x, y, z); // 중요!
                    else
                        Place(x, y, z);
                }
            }
        }

        // 물 채우기
        // ...
    }

    void PlaceTopBlock(int x, int y, int z)
    {
        switch (islandType)
        {
            case IslandType.Resource:
                PlaceGrass(x, y, z);
                break;
            case IslandType.Mineral:
                PlaceMineral(x, y, z);
                break;
            case IslandType.Flora:
                PlaceMushroom(x, y, z);
                break;
        }
    }
    void PlaceMineral(int x, int y, int z)
    {
        var go = Instantiate(mineralPrefab, new Vector3(x, y, z), Quaternion.identity, transform);
        go.name = $"Mineral_{x}_{y}_{z}";
        hasBlock[x, y, z] = true;

        tileInfos.Add(new TileInfos { tileX = x, tileY = y, tileZ = z, tileType = "Mineral" });

        var b = go.GetComponent<Block>() ?? go.AddComponent<Block>();
        b.type = ItemType.Iron;
        b.maxHP = 5;
        b.dropCount = 2;
        b.mineable = true;
    }

    void PlaceMushroom(int x, int y, int z)
    {
        var go = Instantiate(mushroomPrefab, new Vector3(x, y, z), Quaternion.identity, transform);
        go.name = $"Mushroom_{x}_{y}_{z}";
        hasBlock[x, y, z] = true;

        tileInfos.Add(new TileInfos { tileX = x, tileY = y, tileZ = z, tileType = "Flora" });

        var b = go.GetComponent<Block>() ?? go.AddComponent<Block>();
        b.type = ItemType.Mushroom;
        b.maxHP = 2;
        b.dropCount = 1;
        b.mineable = true;
    }

    public void PlaceTile(Vector3Int pos, ItemType type)
    {
        switch (type)
        {
            case ItemType.Dirt:
                Place(pos.x, pos.y, pos.z);
                break;
            case ItemType.Grass:
                PlaceGrass(pos.x, pos.y, pos.z);
                break;
            case ItemType.Water:
                PlaceWater(pos.x, pos.y, pos.z);
                break;
        }
    }

    void Place(int x, int y, int z)
    {
        var go = Instantiate(dirtPrefab, new Vector3(x, y, z), Quaternion.identity, transform);
        go.name = $"Dirt_{x}_{y}_{z}";
        hasBlock[x, y, z] = true;

        tileInfos.Add(new TileInfos()
        {
            tileX = x,
            tileY = y,
            tileZ = z,
            tileType = "Dirt"
        });

        var b = go.GetComponent<Block>() ?? go.AddComponent<Block>();
        b.type = ItemType.Dirt;
        b.maxHP = 3;
        b.dropCount = 1;
        b.mineable = true;
    }

    void PlaceGrass(int x, int y, int z)
    {
        var go = Instantiate(grassPrefab, new Vector3(x, y, z), Quaternion.identity, transform);
        go.name = $"Grass_{x}_{y}_{z}";
        hasBlock[x, y, z] = true;

        tileInfos.Add(new TileInfos()
        {
            tileX = x,
            tileY = y,
            tileZ = z,
            tileType = "Grass"
        });

        var b = go.GetComponent<Block>() ?? go.AddComponent<Block>();
        b.type = ItemType.Grass;
        b.maxHP = 3;
        b.dropCount = 1;
        b.mineable = true;
    }

    void PlaceWater(int x, int y, int z)
    {
        var go = Instantiate(waterPrefab, new Vector3(x, y, z), Quaternion.identity, transform);
        go.name = $"Water_{x}_{y}_{z}";
        hasBlock[x, y, z] = true;

        tileInfos.Add(new TileInfos()
        {
            tileX = x,
            tileY = y,
            tileZ = z,
            tileType = "Water"
        });

        var b = go.GetComponent<Block>() ?? go.AddComponent<Block>();
        b.type = ItemType.Water;
        b.maxHP = 3;
        b.dropCount = 1;
        b.mineable = false;
    }
}
