using System;
using UnityEngine;

public class Vision : MonoBehaviour
{
    [SerializeField]
    private float visionRange = 5f;

    [SerializeField]
    [Range(1, 360)]
    private int visionResolution = 5; // Default resolution

    // Array to hold one-hot encoded inputs for each ray
    // The second dimension will be of the one hot encoded vector which will be [time, distance, rotation, plant, cell, bigger, smaller, wall]
    [HideInInspector]
    [SerializeField]
    private float[][] inputOneHot;

    private int tickCount = 0;

    void Start()
    {
        inputOneHot = new float[visionResolution][];
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        tickCount++;
        if ( tickCount % 5 != 0) return; // Only run every 5 ticks for performance
        

        for (int i = 1; i <= visionResolution; ++i)
        {

            inputOneHot[i - 1] = new float[8]; // Reset the one-hot encoded vector for this ray

            // Calculate the angle relative to the object's rotation
            float angle = -180 + i * (360 / visionResolution);
            float relativeAngle = angle * Mathf.Deg2Rad;
            float worldAngle = transform.eulerAngles.z * Mathf.Deg2Rad + relativeAngle;

            // Create direction vector that follows the object's rotation
            Vector2 direction = new Vector2(Mathf.Cos(worldAngle), Mathf.Sin(worldAngle));

            // Use transform.position instead of localPosition for world space raycast
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, visionRange * transform.localScale.x, LayerMask.GetMask("Plant"));

            float distance = 0;

            //TODO: Send input to the neural network
            if (hit.collider != null)
            {
                //Get distance to the detected collider
                Collider2D collider = this.GetComponent<Collider2D>();
                distance = Physics2D.Distance(collider, hit.collider).distance;



            }
            else if (hit.collider != null && hit.collider.CompareTag("Plant"))
            {
                inputOneHot[i - 1] = new float[] {Time.time, distance, angle, 1, 0, 0, 0, 0 }; // Detected a plant
            }
            else if (hit.collider != null && hit.collider.CompareTag("Cell"))
            {
                if (transform.localScale.x - hit.collider.transform.localScale.x > 1f)
                {
                    inputOneHot[i - 1] = new float[] {Time.time, distance, angle, 0, 1, 1, 0, 0 }; // Detected a smaller cell
                }
                else if (hit.collider.transform.localScale.x - transform.localScale.x > 1f)
                {
                    inputOneHot[i - 1] = new float[] {Time.time, distance, angle, 0, 1, 0, 1, 0 }; // Detected a bigger cell
                }
                else
                {
                    inputOneHot[i - 1] = new float[] {Time.time, distance, angle, 0, 1, 0, 0, 0 }; // Detected a cell of similar size
                }
            }
            else if (hit.collider != null && hit.collider.CompareTag("Wall"))
            {
                inputOneHot[i - 1] = new float[] {Time.time, distance, angle, 0, 0, 0, 0, 1 }; // Detected a wall
            }
            else
            {
                inputOneHot[i - 1] = new float[] {Time.time, distance, angle, 0, 0, 0, 0, 0 };
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

    private void OnDrawGizmos()
    { 
        // Get the object's rotation in radians
        float objectRotation = transform.eulerAngles.z * Mathf.Deg2Rad;

        
        float rayLength = visionRange * transform.localScale.x;

        // Optional: Draw all rays for better visualization
        Gizmos.color = Color.yellow;
        for (float i = -180; i <= 180; i += 360/visionResolution)
        {
            float relativeAngle = i * Mathf.Deg2Rad;
            float worldAngle = objectRotation + relativeAngle;
            Vector3 direction = new Vector3(Mathf.Cos(worldAngle), Mathf.Sin(worldAngle), 0);
            Gizmos.DrawLine(transform.position, transform.position + direction * rayLength);
        }
    }
}
