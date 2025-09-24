using UnityEngine;
using System.Collections;

public class PlayerShooting : MonoBehaviour
{
    [Header("총 관련 설정")]
    public int damage = 10;
    public float range = 100f;
    public float fireRate = 10f;

    [Header("반동 설정")]
    public float normalRecoil = 1.5f;    // 기본 반동
    public float crouchingRecoil = 0.2f; // 앉았을 때 반동
    public float sprintingRecoil = 4.0f; // 달릴 때 반동

    [Header("탄약 설정")]
    public int maxAmmo = 30;
    private int currentAmmo;
    public float reloadTime = 1.5f;
    private bool isReloading = false;

    [Header("필수 연결 요소")]
    public Camera playerCamera;
    public GameObject bulletPrefab;
    public GameObject bloodImpactPrefab;
    public GameObject genericImpactPrefab;
    public Transform firePoint;

    private float nextTimeToFire = 0f;
    private Player playerController; // Player.cs 스크립트를 저장할 변수

    void Start()
    {
        currentAmmo = maxAmmo;
        // 같은 오브젝트에 있는 Player.cs 스크립트를 가져와 저장
        playerController = GetComponent<Player>();
    }

    void Update()
    {
        // Player 스크립트의 isPaused 변수를 확인하여 일시정지 중에는 입력받지 않음
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
        Debug.Log("장전 중...");
        yield return new WaitForSeconds(reloadTime);
        currentAmmo = maxAmmo;
        Debug.Log("장전 완료! 남은 총알: " + currentAmmo);
        isReloading = false;
    }

    void Shoot()
    {
        currentAmmo--;
        Debug.Log("총알 발사! 남은 총알: " + currentAmmo + " / " + maxAmmo);

        // --- 반동 적용 로직 ---
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
            // Player.cs에 있는 ApplyRecoil 함수 호출
            playerController.ApplyRecoil(currentRecoil);
        }

        // 이하 발사 로직은 이전과 동일
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