using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float bulletSpeed = 50f;

    void Awake()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = transform.forward * bulletSpeed;
        }
        Destroy(gameObject, 15f);
    }

    // ���𰡿� �浹�ϸ� ������ ���� �׳� ������⸸ ��
    void OnTriggerEnter(Collider other)
    {
        // ������ ������ ��� �����մϴ�.
        Destroy(gameObject);
    }
}