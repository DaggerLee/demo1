using UnityEngine;
using System.Collections;
using UnityEngine.Rendering.Universal;

public class PlayerShooting : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public Light2D muzzleFlashLight; // 角色身上的灯

    [Header("灯光设置")]
    public float fullIntensityTime = 2f; // 保持全亮的时间
    public float fadeDuration = 0.5f;    // 逐渐熄灭的时间
    public float maxIntensity = 1.5f;    // 灯光最大亮度

    private Coroutine flashCoroutine;

    void Start()
    {
        if (muzzleFlashLight != null) muzzleFlashLight.intensity = 0f;
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
            // 生成子弹逻辑不变
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            Vector2 direction = (mousePos - firePoint.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            Instantiate(bulletPrefab, firePoint.position, Quaternion.Euler(0, 0, angle));

            // 触发灯光逻辑
            if (muzzleFlashLight != null)
            {
                if (flashCoroutine != null) StopCoroutine(flashCoroutine);
                flashCoroutine = StartCoroutine(SmartFlashRoutine());
            }
        }
    }

    IEnumerator SmartFlashRoutine()
    {
        // 1. 瞬间变亮并开启感应（让怪物在被照亮时会靠近）
        muzzleFlashLight.intensity = maxIntensity;
        if (muzzleFlashLight.GetComponent<CircleCollider2D>() != null)
            muzzleFlashLight.GetComponent<CircleCollider2D>().enabled = true;

        // 2. 保持全亮 2 秒
        yield return new WaitForSeconds(fullIntensityTime);

        // 3. 在 0.5 秒内逐渐变暗
        float startIntensity = muzzleFlashLight.intensity;
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            // 使用 Lerp 实现平滑减弱
            muzzleFlashLight.intensity = Mathf.Lerp(startIntensity, 0f, elapsed / fadeDuration);
            yield return null;
        }

        // 4. 彻底熄灭并关闭感应
        muzzleFlashLight.intensity = 0f;
        if (muzzleFlashLight.GetComponent<CircleCollider2D>() != null)
            muzzleFlashLight.GetComponent<CircleCollider2D>().enabled = false;
    }
}