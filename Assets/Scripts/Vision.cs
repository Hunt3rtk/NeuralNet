using UnityEditor;
using UnityEngine;

public class Vision : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        for (float i = -22.5f; i <= 22.5f; i += 5)
        {
            // Calculate the angle relative to the object's rotation
            float relativeAngle = i * Mathf.Deg2Rad;
            float worldAngle = transform.eulerAngles.z * Mathf.Deg2Rad + relativeAngle;
            
            // Create direction vector that follows the object's rotation
            Vector2 direction = new Vector2(Mathf.Cos(worldAngle), Mathf.Sin(worldAngle));
            
            // Use transform.position instead of localPosition for world space raycast
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 5f * transform.localScale.x, LayerMask.GetMask("Plant"));
            
            if (hit.collider != null && hit.collider.CompareTag("Plant"))
            {
                
            }
        }
    }

    private void OnDrawGizmos()
    { 
        // Get the object's rotation in radians
        float objectRotation = transform.eulerAngles.z * Mathf.Deg2Rad;

        
        float rayLength = 5f * transform.localScale.x;

        // Optional: Draw all rays for better visualization
        Gizmos.color = Color.yellow;
        for (float i = -22.5f; i <= 22.5f; i += 5)
        {
            float relativeAngle = i * Mathf.Deg2Rad;
            float worldAngle = objectRotation + relativeAngle;
            Vector3 direction = new Vector3(Mathf.Cos(worldAngle), Mathf.Sin(worldAngle), 0);
            Gizmos.DrawLine(transform.position, transform.position + direction * rayLength);
        }
    }
}
