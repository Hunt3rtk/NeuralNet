using System.Linq;
using UnityEngine;

public class SpawnCells : MonoBehaviour
{

    [Header("Cell Spawning Settings")]

    [SerializeField]
    [Range(1, 100000)]
    private int numbOfCells = 50; // Number of plants to spawn

    [SerializeField]
    [Range(1, 100)]
    private int spawnAreaSize = 50; // Size of the area in which plants will be spawned

    [SerializeField]
    private float mutationRate = 0.1f;

    [SerializeField]
    private GameObject cellPrefab;

    public void SpawnMultipleCells()
    {

        Brain[] cells = GetCells();

         for (int i = 0; i < numbOfCells; i++)
        {
            if (cells.Length > 0)
                SpawnCell(cells[i % (int)(numbOfCells * mutationRate)]);
            else
                SpawnCell();
        }
    }

    private void SpawnCell(Brain topCell = null)
    {
       
        Vector3 position = new Vector3(Random.Range(-spawnAreaSize, spawnAreaSize), Random.Range(-spawnAreaSize, spawnAreaSize), Random.Range(-spawnAreaSize, spawnAreaSize));

        GameObject cell;

        cell = Instantiate(cellPrefab, position, Quaternion.identity);
        cell.transform.localScale = Vector3.one;

        if (topCell != null)
        {
            cell.GetComponent<Brain>().SetNeuralNet(topCell.GetNeuralNet());
            cell.GetComponent<Brain>().SetOutputLayer(topCell.GetOutputLayer());
            cell.GetComponent<Brain>().Mutate();
        }
    }

    public void DestroyCells()
    {
        Brain[] cells = GetCells();
        foreach (Brain cell in cells)
        {
            if (cell != null)
            {
                Destroy(cell.gameObject);
            }
        }
    }

    public Brain[] GetCells()
    {
        return Object.FindObjectsByType<Brain>(FindObjectsSortMode.None)
            .OrderBy(go => go.transform.localScale.magnitude)
            .ToArray();
    }
}
