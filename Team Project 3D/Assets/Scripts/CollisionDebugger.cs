using UnityEngine;

public class CollisionDebugger : MonoBehaviour
{
    // �� ��ũ��Ʈ�� � �ݶ��̴��� ��⸸ �ϸ� �� ��� �α׸� ����մϴ�.
    void OnTriggerEnter(Collider other)
    {
        // �αװ� ���� �� �絵�� ������� �����ϰ�, � ������Ʈ�� ��Ҵ���, �� ������Ʈ�� ���̾�� �������� ����մϴ�.
        Debug.Log("<color=lime>COLLISION EVENT FIRED!</color> Touched object: "
                  + other.gameObject.name
                  + ", Layer: "
                  + LayerMask.LayerToName(other.gameObject.layer), this.gameObject);
    }
}