using UnityEngine;

// 이 스크립트는 플레이어(Player) 오브젝트에 추가합니다.
public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        // --- 여기가 추가/수정된 부분입니다 ---
        // 피해량과 함께 현재 체력을 로그로 출력합니다.
        Debug.Log("플레이어가 " + damage + "의 피해를 입었습니다! 현재 체력: " + currentHealth);
        // --- 여기까지 ---

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("플레이어가 쓰러졌습니다.");
        // 여기에 플레이어가 죽었을 때의 로직을 추가할 수 있습니다.
    }
}