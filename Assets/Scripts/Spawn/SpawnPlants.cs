using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnPlants : MonoBehaviour
{

    [Header("Plant Spawning Settings")]

    [SerializeField]
    [Range(1, 100000)]
    private int numberOfPlants = 10000; // Number of plants to spawn

    [SerializeField]
    [Range(1, 100)]
    private int spawnAreaSize = 50; // Size of the area in which plants will be spawned

    [SerializeField]
    private GameObject plantPrefab;

    private int tick = 0;

    // Update is called once per frame
    void Update()
    {
        tick++;
        if (tick % 10 == 0) // Every 10 ticks
        {
            SpawnPlant();
        }
    }

    public void SpawnMultiplePlants()
    {
        for (int i = 0; i < numberOfPlants; i++)
        {
            SpawnPlant();
        }
    }

    private void SpawnPlant()
    {
        Vector3 position = new Vector3(Random.Range(-spawnAreaSize, spawnAreaSize), Random.Range(-spawnAreaSize, spawnAreaSize), Random.Range(-spawnAreaSize, spawnAreaSize));
        Instantiate(plantPrefab, position, Quaternion.identity);
    }

    public void DestroyPlants()
    {

        GameObject[] plants = GetPlants();
        foreach (GameObject plant in plants)
        {
            if (plant != null)
            {
                Destroy(plant);
            }
        }
    }

    public GameObject[] GetPlants()
    {
        return Object.FindObjectsByType<GameObject>(FindObjectsSortMode.None)
            .Where(go => go.CompareTag("Plant"))
            .ToArray();
    }
}
