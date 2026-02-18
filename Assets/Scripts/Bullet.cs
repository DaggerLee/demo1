using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 15f;
    public GameObject impactEchoPrefab;
    public LayerMask hitLayers; // 在 Inspector 中选择 "Enemy" 和 "Obstacle/Wall"

    private Rigidbody2D rb;
    private Vector2 lastPosition;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = transform.up * speed;
        lastPosition = transform.position;

        Destroy(gameObject, 3f);
    }

    void FixedUpdate()
    {
        // 计算这一帧子弹飞过的位移
        Vector2 currentPosition = transform.position;
        Vector2 direction = (currentPosition - lastPosition);
        float distance = direction.magnitude;

        if (distance > 0)
        {
            // 发射一条极短的射线，检查路径上是否有漏掉的碰撞
            RaycastHit2D hit = Physics2D.Raycast(lastPosition, direction.normalized, distance, hitLayers);

            if (hit.collider != null)
            {
                // 如果射线扫到了东西，手动触发碰撞逻辑
                OnCustomHit(hit.collider);
            }
        }
        lastPosition = currentPosition;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // 忽略这些物体
        if (other.CompareTag("Player") || other.isTrigger) return;

        // 如果打中怪，执行伤害
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyHealth>()?.TakeDamage(1);
            // 打中怪也产生一个小光效果（可选）
            SpawnImpactLight();
        }
        else
        {
            // 打中墙壁产生光效果
            SpawnImpactLight();
        }

        Destroy(gameObject);
    }

    void SpawnImpactLight()
    {
        if (impactEchoPrefab != null)
        {
            Instantiate(impactEchoPrefab, transform.position, Quaternion.identity);
        }
    }

    void OnCustomHit(Collider2D other)
    {
        // 排除掉不该撞的东西
        if (other.CompareTag("Player") || other.isTrigger) return;

        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyHealth>()?.TakeDamage(1);
            HandleImpact(false);
            return;
        }

        // 撞墙逻辑
        HandleImpact(true);
    }

    void HandleImpact(bool spawnLight)
    {
        if (spawnLight && impactEchoPrefab != null)
        {
            Instantiate(impactEchoPrefab, transform.position, Quaternion.identity);
        }
        // 防止一发子弹触发两次（射线一次，碰撞一次）
        this.enabled = false;
        Destroy(gameObject);
    }
}