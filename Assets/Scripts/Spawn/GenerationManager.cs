using System.Collections;
using UnityEngine;

public class GenerationManager : MonoBehaviour
{

    [Header("Generation Settings")]

    [SerializeField]
    private int generationTime = 120; // Time in seconds for each generation

    [SerializeField]
    private int currentGeneration = 0;

    private SpawnCells cellSpawner;

    private SpawnPlants plantSpawner;

    float tickRate = 10f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Time.timeScale = tickRate;

        cellSpawner = GetComponent<SpawnCells>();
        plantSpawner = GetComponent<SpawnPlants>();

        StartCoroutine(NextGeneration());
    }

    private IEnumerator NextGeneration()
    {
        cellSpawner.DestroyCells();
        plantSpawner.DestroyPlants();

        currentGeneration++;
        Debug.Log("Generation: " + currentGeneration);

        cellSpawner.SpawnMultipleCells();
        plantSpawner.SpawnMultiplePlants();

        yield return new WaitForSeconds(generationTime);

        StopAllCoroutines();
        StartCoroutine(NextGeneration());
    }
}
