using UnityEngine;
using System.Collections;

// 이 스크립트는 적(Enemy) 오브젝트에 추가합니다.
public class Enemy : MonoBehaviour
{
    [Header("타겟 설정")]
    [Tooltip("적이 추적할 대상입니다. 보통 플레이어를 연결합니다.")]
    public Transform player;
    [Tooltip("공격 판정을 위한 히트박스 오브젝트를 연결합니다.")]
    public GameObject hitbox;

    [Header("AI 행동 설정")]
    [Tooltip("플레이어를 감지하는 최대 거리입니다.")]
    public float detectionRange = 15f;
    [Tooltip("이 거리 안으로 들어오면 이동을 멈추고 공격을 시작합니다.")]
    public float attackRange = 1.5f;
    [Tooltip("적의 이동 속도입니다.")]
    public float moveSpeed = 3.5f;
    [Tooltip("적이 플레이어를 향해 회전하는 속도입니다.")]
    public float rotationSpeed = 10f;

    [Header("공격 설정")]
    [Tooltip("공격 후 다음 공격까지의 대기 시간(쿨타임)입니다.")]
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
        // --- 여기가 추가된 부분입니다 ---
        Debug.Log("공격!");
        // --- 여기까지 ---

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