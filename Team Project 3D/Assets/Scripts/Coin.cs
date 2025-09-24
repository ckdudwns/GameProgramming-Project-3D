using UnityEngine;

public class Coin : MonoBehaviour
{
    // �÷��̾ ��� �� ������ ��
    public int coinValue = 100;

    // OnTriggerEnter�� ����� Update �Լ��� �����߽��ϴ�.
    // ���� ������ �÷��̾ ���� �ε����߸� ȹ��˴ϴ�.
    private void OnTriggerEnter(Collider other)
    {
        // ���� ����� �÷��̾����� �±׷� Ȯ��
        if (other.CompareTag("Player"))
        {
            // �÷��̾��� Player ��ũ��Ʈ�� ã�Ƽ� AddCoins �Լ� ȣ��
            Player playerScript = other.GetComponent<Player>();
            if (playerScript != null)
            {
                playerScript.AddCoins(coinValue);
            }

            // ������ �Ծ����Ƿ� ������Ʈ �ı�
            Destroy(gameObject);
        }
    }
}