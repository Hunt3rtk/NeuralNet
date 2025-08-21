using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Vision : MonoBehaviour
{
    [SerializeField]
    private float visionRange = 5f;

    [SerializeField]
    private float visionAngle = 45f;

    [SerializeField]
    [Range(1, 360)]
    private int visionResolution = 5; // Default resolution

    private int VisionResolution
    {
        get { return visionResolution; }
        set
        { visionResolution = (value % visionAngle == 0) ? value : (int)((float)value - ((float)value % visionAngle)); }  // Ensure resolution is a multiple of visionAngle;
    }

    // Update is called once per frame
    void Update()
    {
        for (float i = -(visionAngle/2); i <= visionAngle/2; i += VisionResolution)
        {
            // Calculate the angle relative to the object's rotation
            float relativeAngle = i * Mathf.Deg2Rad;
            float worldAngle = transform.eulerAngles.z * Mathf.Deg2Rad + relativeAngle;
            
            // Create direction vector that follows the object's rotation
            Vector2 direction = new Vector2(Mathf.Cos(worldAngle), Mathf.Sin(worldAngle));
            
            // Use transform.position instead of localPosition for world space raycast
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, visionRange * transform.localScale.x, LayerMask.GetMask("Plant"));
            
            if (hit.collider != null && hit.collider.CompareTag("Plant"))
            {
                
            }
        }
    }

    private void OnDrawGizmos()
    { 
        // Get the object's rotation in radians
        float objectRotation = transform.eulerAngles.z * Mathf.Deg2Rad;

        
        float rayLength = visionRange * transform.localScale.x;

        // Optional: Draw all rays for better visualization
        Gizmos.color = Color.yellow;
        for (float i = -(visionAngle/2); i <= visionAngle/2; i += VisionResolution)
        {
            float relativeAngle = i * Mathf.Deg2Rad;
            float worldAngle = objectRotation + relativeAngle;
            Vector3 direction = new Vector3(Mathf.Cos(worldAngle), Mathf.Sin(worldAngle), 0);
            Gizmos.DrawLine(transform.position, transform.position + direction * rayLength);
        }
    }
}
