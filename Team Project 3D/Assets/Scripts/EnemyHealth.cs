using UnityEngine;

// 이 스크립트는 적(Enemy) 오브젝트에 추가합니다.
public class EnemyHealth : MonoBehaviour
{
    [Header("체력 설정")]
    [Tooltip("적의 최대 체력입니다.")]
    public int maxHealth = 100;

    [Header("아이템 드롭 설정")]
    [Tooltip("적이 죽었을 때 드롭할 아이템 프리팹 목록입니다.")]
    public GameObject[] itemDrops;

    private int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    // --- 추가된 부분 ---
    // 매 프레임마다 호출되는 함수
    void Update()
    {
        // 만약 K 키를 누르면 Die() 함수를 즉시 호출
        if (Input.GetKeyDown(KeyCode.K))
        {
            Die();
        }
    }
    // --- 여기까지 ---

    // 외부에서 호출하여 적에게 피해를 주는 함수
    public void TakeDamage(int damage)
    {
        // 이미 죽었다면 피해를 받지 않도록 처리
        if (currentHealth <= 0) return;

        currentHealth -= damage;
        Debug.Log(gameObject.name + "가 " + damage + "의 피해를 입었습니다! 현재 체력: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // 적이 죽었을 때 처리하는 함수
    void Die()
    {
        Debug.Log(gameObject.name + "가 쓰러졌습니다.");

        // --- 디버깅용 로그 추가 ---
        if (itemDrops == null)
        {
            // 배열 자체가 할당되지 않은 경우 (가장 심각한 오류)
            Debug.LogError("오류: Item Drops 배열이 null입니다! Inspector에서 할당이 필요합니다.");
        }
        else if (itemDrops.Length == 0)
        {
            // 배열은 있지만, 내용물이 하나도 없는 경우
            Debug.LogWarning("경고: Item Drops 배열에 아이템이 없습니다. (Size: 0)");
        }
        else
        {
            // 배열에 아이템이 하나 이상 할당된 경우
            Debug.Log("<color=lime>성공: Item Drops 배열에 " + itemDrops.Length + "개의 아이템이 연결되어 있습니다.</color>");

            // 추가 확인: 배열 안에 빈 슬롯(None)이 있는지 검사
            for (int i = 0; i < itemDrops.Length; i++)
            {
                if (itemDrops[i] == null)
                {
                    Debug.LogWarning("경고: Item Drops 배열의 " + i + "번째 슬롯이 비어있습니다 (None).");
                }
            }
        }
        // --- 디버깅 끝 ---

        DropItem(); // 원래 드롭 함수 호출

        Destroy(gameObject);
    }

    // EnemyHealth.cs 스크립트의 DropItem 함수
    void DropItem()
    {
        // 드롭할 아이템 목록이 비어있지 않은지 확인
        if (itemDrops != null && itemDrops.Length > 0)
        {
            int randomIndex = Random.Range(0, itemDrops.Length);
            GameObject itemToDrop = itemDrops[randomIndex];

            if (itemToDrop != null)
            {
                // --- 여기가 수정된 핵심 부분입니다 ---
                Vector3 dropPosition = transform.position; // 기본 드롭 위치는 적의 위치로 설정
                RaycastHit hit;

                // 적의 위치에서 아래(Vector3.down)로 100유닛만큼 광선을 쏴서 바닥을 찾음
                if (Physics.Raycast(transform.position, Vector3.down, out hit, 100f))
                {
                    // 광선이 바닥에 닿았다면, 그 닿은 지점(hit.point)을 드롭 위치로 설정
                    // 약간 위에서 생성해야 땅에 파묻히지 않으므로 Y값을 살짝 더해줌
                    dropPosition = hit.point + new Vector3(0, 0.5f, 0);
                }
                // --- 여기까지 ---

                // 계산된 최종 위치(dropPosition)에 아이템을 생성
                Instantiate(itemToDrop, dropPosition, Quaternion.identity);
                Debug.Log(itemToDrop.name + " 아이템을 드롭했습니다!");
            }
        }
    }
}