using System;
using UnityEngine;

public class Vision : MonoBehaviour
{
    [SerializeField]
    private float visionRange = 5f;

    [SerializeField]
    [Range(1, 360)]
    public int visionResolution = 5; // Default resolution

    // Array to hold one-hot encoded inputs for each ray
    // The second dimension will be of the one hot encoded vector which will be [time, distance, rotation, plant, cell, bigger, smaller, wall]
    [HideInInspector]
    [SerializeField]
    private float[] inputOneHot;

    private int tickCount = 0;

    private int oneHotLength = 3; // Size of the one-hot encoded vector

    void Start()
    {
        inputOneHot = new float[oneHotLength];
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        tickCount++;
        if ( tickCount % 5 != 0) return; // Only run every 5 ticks for performance
        

        for (int i = 1; i <= visionResolution; ++i)
        {

            // Calculate the angle relative to the object's rotation
            float angle = -180 + i * (360 / visionResolution);
            float relativeAngle = angle * Mathf.Deg2Rad;
            float worldAngle = transform.eulerAngles.z * Mathf.Deg2Rad + relativeAngle;

            // Create direction vector that follows the object's rotation
            Vector2 direction = new Vector2(Mathf.Cos(worldAngle), Mathf.Sin(worldAngle));

            // Use transform.position instead of localPosition for world space raycast
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, visionRange * transform.localScale.x, LayerMask.GetMask("Plant"));

            if (hit.collider != null && hit.collider.CompareTag("Plant"))
            {
                inputOneHot = new float[] {1, direction.x, direction.y}; // Detected a plant
            }
            else if (hit.collider != null && hit.collider.CompareTag("Cell") && inputOneHot != new float[] {1, direction.x, direction.y}) // Prioritize plants over cells
            {
                if (transform.localScale.x - hit.collider.transform.localScale.x > 1f)
                {
                    inputOneHot = new float[] {1, direction.x, direction.y}; // Detected a smaller cell
                }
                else if (hit.collider.transform.localScale.x - transform.localScale.x > 1f)
                {
                    inputOneHot = new float[] {-1, direction.x, direction.y}; // Detected a bigger cell
                }
                else
                {
                    inputOneHot = new float[] {0, direction.x, direction.y}; // Detected a cell of similar size
                }
            }
            else if (hit.collider != null && hit.collider.CompareTag("Wall") && inputOneHot != new float[] {1, direction.x, direction.y} && inputOneHot != new float[] {-1, direction.x, direction.y})
            {
                inputOneHot = new float[] {-1, direction.x, direction.y}; // Detected a wall
            }
            else
            {
                inputOneHot = new float[] {0, direction.x, direction.y};
                // Nothing detected within vision range
            }
        }

        // Send the inputOneHot to the neural network for processing
        Brain brain = GetComponent<Brain>();
        if (brain != null)
        {
            // Assuming Brain has a method to process the inputOneHot
            brain.InitializeForwardPass(inputOneHot);
        }
        else
        {
            Debug.LogWarning("Brain component not found on the GameObject.");
        }
    }
    
    public int GetInputSize()
    {
        return oneHotLength; // 3 is the size of the one-hot encoded vector
    }

    private void OnDrawGizmos()
    {
        // Get the object's rotation in radians
        float objectRotation = transform.eulerAngles.z * Mathf.Deg2Rad;


        float rayLength = visionRange * transform.localScale.x;

        // Optional: Draw all rays for better visualization
        Gizmos.color = Color.yellow;
        for (float i = -180; i <= 180; i += 360 / visionResolution)
        {
            float relativeAngle = i * Mathf.Deg2Rad;
            float worldAngle = objectRotation + relativeAngle;
            Vector3 direction = new Vector3(Mathf.Cos(worldAngle), Mathf.Sin(worldAngle), 0);
            Gizmos.DrawLine(transform.position, transform.position + direction * rayLength);
        }
    }
}
