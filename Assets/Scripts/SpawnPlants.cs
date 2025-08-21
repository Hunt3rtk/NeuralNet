using System.Collections.Generic;
using UnityEngine;

public class SpawnPlants : MonoBehaviour
{

    [SerializeField]
    private GameObject plantPrefab;

    GameObject[] plants = new GameObject[100];

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < plants.Length; i++)
        {
            Vector3 position = new Vector3(Random.Range(-10f, 10f), Random.Range(-10f, 10f), Random.Range(-10f, 10f));
            plants[i] = Instantiate(plantPrefab, position, Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
