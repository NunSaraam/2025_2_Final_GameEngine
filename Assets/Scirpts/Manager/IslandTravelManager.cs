using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IslandTravelManager : MonoBehaviour
{
    public static IslandTravelManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void TravelToIsland(IslandType islandType)
    {
        switch (islandType)
        {
            case IslandType.Resource:
                SceneManager.LoadScene("Island_Resource");
                break;
            case IslandType.Mineral:
                SceneManager.LoadScene("Island_Mineral");
                break;
            case IslandType.Flora:
                SceneManager.LoadScene("Island_Flora");
                break;
            case IslandType.Main:
                SceneManager.LoadScene("MainIsland");
                break;
        }
    }
}