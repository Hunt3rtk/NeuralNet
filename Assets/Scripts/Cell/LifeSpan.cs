using UnityEngine;

public class LifeSpan : MonoBehaviour
{
    [SerializeField]
    private float lifespan = 20f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, lifespan); // Destroy the GameObject after the specified lifespan
    }
}
