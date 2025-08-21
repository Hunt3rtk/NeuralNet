using UnityEngine;

public class Eat : MonoBehaviour
{
   void OnTriggerEnter2D(Collider2D other)
   {
        if (other.CompareTag("Plant"))
        {

            transform.localScale += new Vector3(.1f, .1f, .1f); // Increase size of the GameObject

            Destroy(other.gameObject);
            // Optionally, you can add logic to increase score or perform other actions
        }
   }
}
