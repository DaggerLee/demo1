using UnityEngine;

public class Door : MonoBehaviour
{
    public bool isLocked = true;

    // 当有物体撞击门时触发（门有实心 Collider）
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 1. 检查撞击的是不是玩家
        if (collision.gameObject.CompareTag("Player"))
        {
            // 2. 尝试从玩家身上获取背包组件
            PlayerInventory inventory = collision.gameObject.GetComponent<PlayerInventory>();

            // 3. 如果门锁着，且玩家手里有钥匙
            if (isLocked && inventory != null && inventory.hasKey)
            {
                UnlockAndRemove(inventory);
            }
            else if (isLocked)
            {
                Debug.Log("The door is locked. Find a key!");
            }
        }
    }

    void UnlockAndRemove(PlayerInventory inventory)
    {
        // 消耗玩家的钥匙状态
        inventory.hasKey = false;

        // 找到悬浮在玩家旁边的钥匙实例并销毁
        GameObject keyObj = GameObject.FindWithTag("Key");
        if (keyObj != null)
        {
            Destroy(keyObj);
        }

        // 门消失：有两种选法
        // A. 彻底从世界里删掉（推荐）
        Destroy(gameObject);

        // B. 只是关掉，不显示也不触发碰撞（如果你后面想让门复原，用这个）
        // gameObject.SetActive(false);

        Debug.Log("Door is open. Good Luck.");
    }
}