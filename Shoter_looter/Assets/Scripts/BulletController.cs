using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float speed = 20f; // Adjust the bullet speed as needed
    public float maxDistance = 50f; // The maximum distance the bullet can travel before being returned to the pool

    private Vector3 initialPosition;

    private void OnEnable()
    {
        initialPosition = transform.position;
    }

    private void Update()
    {
        // Move the bullet forward based on its speed
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        // Check if the bullet has traveled its maximum distance
        if (Vector3.Distance(transform.position, initialPosition) >= maxDistance)
        {
            ReturnToPool();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Add collision handling if needed (e.g., if the bullet should disappear upon hitting an object)
        ReturnToPool();
    }

    private void ReturnToPool()
    {
        // Reset any properties that might affect the bullet's behavior
        // (e.g., speed, direction, etc.)
        gameObject.SetActive(false);
        BulletPoolManager.Instance.ReturnBulletToPool(gameObject);
    }
}