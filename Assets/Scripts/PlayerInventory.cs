using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public bool hasKey = false; // 玩家是否持有钥匙

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 如果碰到钥匙
        if (other.CompareTag("Key"))
        {
            hasKey = true;
            // 调用钥匙的拾取逻辑
            other.GetComponent<KeyItem>().PickUp(this.transform);
        }
    }
}