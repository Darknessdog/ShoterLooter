using UnityEngine;

public class GunScript : MonoBehaviour
{
    public Transform firePoint; // The position where the bullets will be spawned
    public GameObject bulletPrefab;

    // You can adjust fire rate and other properties as needed
    public float fireRate = 0.2f;
    private float nextFireTime = 0f;

    private void Update()
    {
        if (Input.GetButton("Fire1") && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate;
            FireBullet();
        }
    }

    private void FireBullet()
    {
        // Use object pooling to get a bullet from the pool
        GameObject bullet = BulletPoolManager.Instance.GetBulletFromPool();
        if (bullet != null)
        {
            bullet.transform.position = firePoint.position;
            bullet.transform.rotation = firePoint.rotation;
            bullet.SetActive(true);
        }
    }
}