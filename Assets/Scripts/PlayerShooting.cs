using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerShooting : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public Light2D muzzleFlashLight;

    [Header("灯光设置")]
    public float maxIntensity = 1.5f;

    void Start()
    {
        if (muzzleFlashLight != null)
        {
            muzzleFlashLight.intensity = maxIntensity;
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        if (bulletPrefab != null && firePoint != null)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;

            Vector2 direction = (mousePos - firePoint.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;

            Instantiate(bulletPrefab, firePoint.position, Quaternion.Euler(0, 0, angle));
        }
    }
}
