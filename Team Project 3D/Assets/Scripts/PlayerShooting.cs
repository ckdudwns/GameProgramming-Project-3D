using UnityEngine;
using System.Collections;

public class PlayerShooting : MonoBehaviour
{
    [Header("�� ���� ����")]
    public int damage = 10;
    public float range = 100f;
    public float fireRate = 10f;

    [Header("�ݵ� ����")]
    public float normalRecoil = 1.5f;    // �⺻ �ݵ�
    public float crouchingRecoil = 0.2f; // �ɾ��� �� �ݵ�
    public float sprintingRecoil = 4.0f; // �޸� �� �ݵ�

    [Header("ź�� ����")]
    public int maxAmmo = 30;
    private int currentAmmo;
    public float reloadTime = 1.5f;
    private bool isReloading = false;

    [Header("�ʼ� ���� ���")]
    public Camera playerCamera;
    public GameObject bulletPrefab;
    public GameObject bloodImpactPrefab;
    public GameObject genericImpactPrefab;
    public Transform firePoint;

    private float nextTimeToFire = 0f;
    private Player playerController; // Player.cs ��ũ��Ʈ�� ������ ����

    void Start()
    {
        currentAmmo = maxAmmo;
        // ���� ������Ʈ�� �ִ� Player.cs ��ũ��Ʈ�� ������ ����
        playerController = GetComponent<Player>();
    }

    void Update()
    {
        // Player ��ũ��Ʈ�� isPaused ������ Ȯ���Ͽ� �Ͻ����� �߿��� �Է¹��� ����
        if (isReloading || Player.isPaused) return;

        if (Input.GetMouseButton(0) && Time.time >= nextTimeToFire)
        {
            if (currentAmmo > 0)
            {
                nextTimeToFire = Time.time + 1f / fireRate;
                Shoot();
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(Reload());
        }
    }

    IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("���� ��...");
        yield return new WaitForSeconds(reloadTime);
        currentAmmo = maxAmmo;
        Debug.Log("���� �Ϸ�! ���� �Ѿ�: " + currentAmmo);
        isReloading = false;
    }

    void Shoot()
    {
        currentAmmo--;
        Debug.Log("�Ѿ� �߻�! ���� �Ѿ�: " + currentAmmo + " / " + maxAmmo);

        // --- �ݵ� ���� ���� ---
        if (playerController != null)
        {
            float currentRecoil = normalRecoil;
            if (playerController.IsCrouching)
            {
                currentRecoil = crouchingRecoil;
            }
            else if (playerController.IsSprinting)
            {
                currentRecoil = sprintingRecoil;
            }
            // Player.cs�� �ִ� ApplyRecoil �Լ� ȣ��
            playerController.ApplyRecoil(currentRecoil);
        }

        // ���� �߻� ������ ������ ����
        RaycastHit hit;
        Vector3 targetPoint;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, range))
        {
            targetPoint = hit.point;
            EnemyHealth enemy = hit.transform.GetComponentInParent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                if (bloodImpactPrefab != null)
                {
                    Instantiate(bloodImpactPrefab, hit.point, Quaternion.LookRotation(hit.normal));
                }
            }
            else
            {
                if (genericImpactPrefab != null)
                {
                    Instantiate(genericImpactPrefab, hit.point, Quaternion.LookRotation(hit.normal));
                }
            }
        }
        else
        {
            targetPoint = playerCamera.transform.position + playerCamera.transform.forward * range;
        }

        if (bulletPrefab != null && firePoint != null)
        {
            Vector3 direction = targetPoint - firePoint.position;
            Quaternion bulletRotation = Quaternion.LookRotation(direction);
            Instantiate(bulletPrefab, firePoint.position, bulletRotation);
        }
    }
}