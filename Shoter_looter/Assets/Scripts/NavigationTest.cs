using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavigationTest : MonoBehaviour
{
    public int maxHealth = 100; // Maximum health of the enemy
    private int currentHealth;   // Current health of the enemy

    public Transform player;
    private NavMeshAgent agent;

    public float detectionDistance = 10f;
    public float shootingDistance = 5f;

    public Color gizmoColor = Color.red;
    public float sphereRadius = 10f;
    public Color gizmoColor2 = Color.green;
    public float sphereRadius2 = 5f;

    public GameObject projectilePrefab;
    public Transform projectileSpawnPoint;
    public float shootingCooldown = 2f; // Cooldown between shots
    private float lastShotTime; // Time when the last shot was fired


    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        currentHealth = maxHealth;
        lastShotTime = -shootingCooldown;
    }

    // Update is called once per frame
    void Update()
    {
        // Calculate the distance between the enemy and the player
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Check if the player is within the detection distance
        if (distanceToPlayer <= detectionDistance)
        {
            agent.destination = player.position;

            if (distanceToPlayer <= shootingDistance)
            {

                agent.isStopped = true;

                // Rotate the enemy to face the player (optional)
                Vector3 directionToPlayer = player.position - transform.position;
                Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);

                // Check if enough time has passed since the last shot
                if (Time.time - lastShotTime >= shootingCooldown)
                {
                    Shoot();
                    lastShotTime = Time.time;
                }
            }
            else
            {
                agent.isStopped = false;
            }
        }
        else
        {
            agent.ResetPath();
        }
            
    }

    private void Shoot()
    {
        // Instantiate the projectile at the spawn point's position and rotation
        GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, projectileSpawnPoint.rotation);

        // Calculate the direction to the player
        Vector3 directionToPlayer = player.position - projectileSpawnPoint.position;

        // Apply force to the projectile to make it move
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        rb.AddForce(directionToPlayer.normalized * 10f, ForceMode.Impulse);

    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        Debug.Log(currentHealth);

        if (currentHealth <= 0)
        {
            Destroy(gameObject); // Enemy is destroyed when health reaches 0 or less
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawWireSphere(transform.position, sphereRadius);

        Gizmos.color = gizmoColor2;
        Gizmos.DrawWireSphere(transform.position, sphereRadius2);
    }
}
