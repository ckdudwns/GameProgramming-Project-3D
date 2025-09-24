using UnityEngine;

public class EnemyHitbox : MonoBehaviour
{
    public int damage = 10;

    // Trigger�� �ٸ� Collider(�÷��̾�)�� �浹���� �� ȣ��
    private void OnTriggerEnter(Collider other)
    {
        // �浹�� ��밡 PlayerHealth ��ũ��Ʈ�� ������ �ִ��� Ȯ��
        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            // ����� TakeDamage �Լ��� ȣ���Ͽ� ���ظ� ��
            playerHealth.TakeDamage(damage);

            // �� ���� ���ݿ� ���� �� ���ظ� ���� �ʵ��� ��� ��Ȱ��ȭ
            gameObject.SetActive(false);
        }
    }
}