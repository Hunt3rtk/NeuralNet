using UnityEngine;

public class Mitosis : MonoBehaviour
{

    [SerializeField]
    private GameObject cellPrefab;

    private Brain brain;

    [SerializeField]
    [Tooltip("Local scale to initiate mitosis and split into two cells")]
    private float splitSize = 2f;

    void Start()
    {
        brain = GetComponent<Brain>();
        if (brain == null) Debug.LogError("Brain component not found on the cell.");
    }

    public void CheckSize(float currentSize)
    {
        if (currentSize >= splitSize)
        {
            Split();
        }
    }

    private void Split()
    {
        GameObject cellOne = Instantiate(cellPrefab, transform.position + new Vector3(-1f, 0, 0), Quaternion.identity);
        GameObject cellTwo = Instantiate(cellPrefab, transform.position + new Vector3(1f, 0, 0), Quaternion.identity);

        cellOne.transform.localScale = Vector3.one;
        cellTwo.transform.localScale = Vector3.one;

        cellOne.GetComponent<Brain>().SetNeuralNet(brain.GetNeuralNet());
        cellTwo.GetComponent<Brain>().SetNeuralNet(brain.GetNeuralNet());
        cellOne.GetComponent<Brain>().SetOutputLayer(brain.GetOutputLayer());
        cellTwo.GetComponent<Brain>().SetOutputLayer(brain.GetOutputLayer());

        cellOne.GetComponent<Brain>().Mutate();
        cellTwo.GetComponent<Brain>().Mutate();

        Destroy(gameObject);
    }
}
