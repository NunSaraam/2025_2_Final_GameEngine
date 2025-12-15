using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandInitializer : MonoBehaviour
{
    public IslandType islandType;

    void Start()
    {
        var map = FindObjectOfType<PerlinNoiseVoxelMap>();
        if (map != null)
        {
            map.islandType = islandType;
            map.StartGeneration(); // Start 대신 명시적 호출
        }
    }
}
