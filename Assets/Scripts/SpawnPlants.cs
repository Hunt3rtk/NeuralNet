using System.Collections.Generic;
using UnityEngine;

public class SpawnPlants : MonoBehaviour
{

    [Header("Plant Spawning Settings")]

    [SerializeField]
    [Range(1, 100000)]
    private int numbOfPlants = 10000; // Number of plants to spawn

    [SerializeField]
    [Range(1, 100)]
    private int spawnAreaSize = 50; // Size of the area in which plants will be spawned

    [SerializeField]
    private GameObject plantPrefab;

    private GameObject[] plants;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        plants = new GameObject[numbOfPlants];


        for (int i = 0; i < plants.Length; i++)
        {
            Vector3 position = new Vector3(Random.Range(-spawnAreaSize, spawnAreaSize), Random.Range(-spawnAreaSize, spawnAreaSize), Random.Range(-spawnAreaSize, spawnAreaSize));
            plants[i] = Instantiate(plantPrefab, position, Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
