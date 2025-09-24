using UnityEngine;
using System.Collections;

// �� ��ũ��Ʈ�� ��(Enemy) ������Ʈ�� �߰��մϴ�.
public class Enemy : MonoBehaviour
{
    [Header("Ÿ�� ����")]
    [Tooltip("���� ������ ����Դϴ�. ���� �÷��̾ �����մϴ�.")]
    public Transform player;
    [Tooltip("���� ������ ���� ��Ʈ�ڽ� ������Ʈ�� �����մϴ�.")]
    public GameObject hitbox;

    [Header("AI �ൿ ����")]
    [Tooltip("�÷��̾ �����ϴ� �ִ� �Ÿ��Դϴ�.")]
    public float detectionRange = 15f;
    [Tooltip("�� �Ÿ� ������ ������ �̵��� ���߰� ������ �����մϴ�.")]
    public float attackRange = 1.5f;
    [Tooltip("���� �̵� �ӵ��Դϴ�.")]
    public float moveSpeed = 3.5f;
    [Tooltip("���� �÷��̾ ���� ȸ���ϴ� �ӵ��Դϴ�.")]
    public float rotationSpeed = 10f;

    [Header("���� ����")]
    [Tooltip("���� �� ���� ���ݱ����� ��� �ð�(��Ÿ��)�Դϴ�.")]
    public float attackCooldown = 2f;
    private bool canAttack = true;

    void Awake()
    {
        if (hitbox != null)
        {
            hitbox.SetActive(false);
        }

        if (player == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
            {
                player = playerObject.transform;
            }
        }
    }

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange)
        {
            RotateTowardsPlayer();

            if (distanceToPlayer > attackRange)
            {
                MoveTowardsPlayer();
            }
            else
            {
                if (canAttack)
                {
                    StartCoroutine(Attack());
                }
            }
        }
    }

    IEnumerator Attack()
    {
        // --- ���Ⱑ �߰��� �κ��Դϴ� ---
        Debug.Log("����!");
        // --- ������� ---

        canAttack = false;
        yield return new WaitForSeconds(0.5f);

        if (hitbox != null)
        {
            hitbox.SetActive(true);
        }

        yield return new WaitForSeconds(0.2f);
        if (hitbox != null)
        {
            hitbox.SetActive(false);
        }

        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    void RotateTowardsPlayer()
    {
        Vector3 direction = player.position - transform.position;
        direction.y = 0;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    void MoveTowardsPlayer()
    {
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}