using System;
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
        else if (other.CompareTag("Cell"))
        {

            if (transform.localScale.x - other.transform.localScale.x > 1f)
            {
                float scaleFactor = Math.Clamp(other.transform.localScale.x - 1, .1f, 2f);
                transform.localScale += new Vector3(scaleFactor, scaleFactor, scaleFactor); // Increase size of the GameObject based on the other object's size
                // If this object is larger, consume the other object
                Destroy(other.gameObject);
            }
        }
   }
}
