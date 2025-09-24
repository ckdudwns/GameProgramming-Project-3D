using UnityEngine;

public class Coin : MonoBehaviour
{
    // 플레이어가 얻게 될 코인의 양
    public int coinValue = 100;

    // OnTriggerEnter만 남기고 Update 함수는 삭제했습니다.
    // 이제 코인은 플레이어가 직접 부딪혀야만 획득됩니다.
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