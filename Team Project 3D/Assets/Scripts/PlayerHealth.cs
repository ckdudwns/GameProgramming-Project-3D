using UnityEngine;

// �� ��ũ��Ʈ�� �÷��̾�(Player) ������Ʈ�� �߰��մϴ�.
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

        // --- ���Ⱑ �߰�/������ �κ��Դϴ� ---
        // ���ط��� �Բ� ���� ü���� �α׷� ����մϴ�.
        Debug.Log("�÷��̾ " + damage + "�� ���ظ� �Ծ����ϴ�! ���� ü��: " + currentHealth);
        // --- ������� ---

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("�÷��̾ ���������ϴ�.");
        // ���⿡ �÷��̾ �׾��� ���� ������ �߰��� �� �ֽ��ϴ�.
    }
}