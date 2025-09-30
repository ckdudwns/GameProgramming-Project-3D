using UnityEngine;

public class Coin : MonoBehaviour
{
    [Header("코인 설정")]
    [Tooltip("플레이어가 얻게 될 코인의 양입니다.")]
    public int coinValue = 100;

    [Header("회전 설정")]
    [Tooltip("코인의 초당 회전 속도입니다.")]
    public float rotationSpeed = 50f;

    // 매 프레임마다 호출되는 함수
    void Update()
    {
        // Y축(Vector3.up)을 기준으로 코인을 회전시킵니다.
        // Time.deltaTime을 곱해줘서 프레임 속도와 상관없이 일정한 속도로 회전하게 합니다.
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }

    // 플레이어의 콜라이더가 코인의 Trigger 영역에 들어왔을 때 호출됨
    private void OnTriggerEnter(Collider other)
    {
        // 들어온 대상이 플레이어인지 태그로 확인
        if (other.CompareTag("Player"))
        {
            // 플레이어의 Player 스크립트를 찾아서 AddCoins 함수 호출
            Player playerScript = other.GetComponent<Player>();
            if (playerScript != null)
            {
                playerScript.AddCoins(coinValue);
            }

            // 코인을 먹었으므로 오브젝트 파괴
            Destroy(gameObject);
        }
    }
}