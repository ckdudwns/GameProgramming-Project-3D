using UnityEngine;

// �� ��ũ��Ʈ�� ��(Enemy) ������Ʈ�� �߰��մϴ�.
public class EnemyHealth : MonoBehaviour
{
    [Header("ü�� ����")]
    [Tooltip("���� �ִ� ü���Դϴ�.")]
    public int maxHealth = 100;

    [Header("������ ��� ����")]
    [Tooltip("���� �׾��� �� ����� ������ ������ ����Դϴ�.")]
    public GameObject[] itemDrops;

    private int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    // --- �߰��� �κ� ---
    // �� �����Ӹ��� ȣ��Ǵ� �Լ�
    void Update()
    {
        // ���� K Ű�� ������ Die() �Լ��� ��� ȣ��
        if (Input.GetKeyDown(KeyCode.K))
        {
            Die();
        }
    }
    // --- ������� ---

    // �ܺο��� ȣ���Ͽ� ������ ���ظ� �ִ� �Լ�
    public void TakeDamage(int damage)
    {
        // �̹� �׾��ٸ� ���ظ� ���� �ʵ��� ó��
        if (currentHealth <= 0) return;

        currentHealth -= damage;
        Debug.Log(gameObject.name + "�� " + damage + "�� ���ظ� �Ծ����ϴ�! ���� ü��: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // ���� �׾��� �� ó���ϴ� �Լ�
    void Die()
    {
        Debug.Log(gameObject.name + "�� ���������ϴ�.");

        // --- ������ �α� �߰� ---
        if (itemDrops == null)
        {
            // �迭 ��ü�� �Ҵ���� ���� ��� (���� �ɰ��� ����)
            Debug.LogError("����: Item Drops �迭�� null�Դϴ�! Inspector���� �Ҵ��� �ʿ��մϴ�.");
        }
        else if (itemDrops.Length == 0)
        {
            // �迭�� ������, ���빰�� �ϳ��� ���� ���
            Debug.LogWarning("���: Item Drops �迭�� �������� �����ϴ�. (Size: 0)");
        }
        else
        {
            // �迭�� �������� �ϳ� �̻� �Ҵ�� ���
            Debug.Log("<color=lime>����: Item Drops �迭�� " + itemDrops.Length + "���� �������� ����Ǿ� �ֽ��ϴ�.</color>");

            // �߰� Ȯ��: �迭 �ȿ� �� ����(None)�� �ִ��� �˻�
            for (int i = 0; i < itemDrops.Length; i++)
            {
                if (itemDrops[i] == null)
                {
                    Debug.LogWarning("���: Item Drops �迭�� " + i + "��° ������ ����ֽ��ϴ� (None).");
                }
            }
        }
        // --- ����� �� ---

        DropItem(); // ���� ��� �Լ� ȣ��

        Destroy(gameObject);
    }

    // ������ ��� �Լ�
    void DropItem()
    {
        if (itemDrops != null && itemDrops.Length > 0)
        {
            int randomIndex = Random.Range(0, itemDrops.Length);
            GameObject itemToDrop = itemDrops[randomIndex];

            if (itemToDrop != null)
            {
                Instantiate(itemToDrop, transform.position, Quaternion.identity);
                Debug.Log(itemToDrop.name + " �������� ����߽��ϴ�!");
            }
        }
    }
}