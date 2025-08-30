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
                if (i < 4) SpawnCell(cells[i % (int)(numbOfCells * mutationRate)], false); //Elite cells that don't mutate
                else
                SpawnCell(cells[i % (int)(numbOfCells * mutationRate)]); //Mutated cells
            else
                SpawnCell(); //First generation of random cells
        }
    }

    private void SpawnCell(Brain topCell = null, bool mutate = true)
    {
       
        Vector3 position = new Vector3(Random.Range(-spawnAreaSize, spawnAreaSize), Random.Range(-spawnAreaSize, spawnAreaSize), 0);

        GameObject cell;

        cell = Instantiate(cellPrefab, position, Quaternion.identity);
        cell.transform.localScale = Vector3.one;

        if (topCell != null)
        {
            cell.GetComponent<Brain>().SetNeuralNet(topCell.GetNeuralNet());
            cell.GetComponent<Brain>().SetOutputLayer(topCell.GetOutputLayer());
            if (mutate) cell.GetComponent<Brain>().Mutate();
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
            .OrderBy(x => x.transform.localScale.magnitude / x.GetComponent<Brain>().GetDistanceFromStart())
            .ToArray();
    }
}
