using UnityEngine;
using System.Collections;
using System.Collections.Generic; // List를 사용하기 위해 추가

public class PlayerShooting : MonoBehaviour
{
    [Header("공통 설정")]
    public float range = 100f;

    [Header("무기 설정")]
    public List<Gun> availableGuns; // 보유하고 있는 총기 목록
    public Transform gunHolder; // 총기 프리팹이 생성될 위치 (카메라의 자식)
    private Gun currentGun; // 현재 장착하고 있는 총
    private int currentGunIndex = -1;

    [Header("필수 연결 요소")]
    public Camera playerCamera;

    // 탄약 및 상태 변수
    private int currentAmmo;
    private bool isReloading = false;
    private float nextTimeToFire = 0f;
    private Player playerController;

    void Start()
    {
        playerController = GetComponent<Player>();

        // 보유한 총이 있다면 첫 번째 총으로 시작
        if (availableGuns != null && availableGuns.Count > 0)
        {
            EquipGun(0);
        }
    }

    void Update()
    {
        if (Player.isPaused) return;

        // 무기 교체 입력 확인
        HandleWeaponSwitching();

        // 현재 총이 없거나 재장전 중이면 발사/재장전 불가
        if (currentGun == null || isReloading) return;

        // 발사 입력
        if (Input.GetMouseButton(0) && Time.time >= nextTimeToFire)
        {
            if (currentAmmo > 0)
            {
                nextTimeToFire = Time.time + 1f / currentGun.fireRate;
                Shoot();
            }
        }

        // 재장전 입력
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(Reload());
        }
    }

    void HandleWeaponSwitching()
    {
        // 숫자 키 1~9 입력 확인
        for (int i = 1; i <= 9; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha0 + i))
            {
                // 인덱스는 0부터 시작하므로 i-1
                // 보유한 총의 개수보다 작고, 현재 들고 있는 총과 다른 총일 때만 교체
                if (i - 1 < availableGuns.Count && i - 1 != currentGunIndex)
                {
                    EquipGun(i - 1);
                }
                break;
            }
        }
    }

    void EquipGun(int gunIndex)
    {
        // 이미 진행 중인 재장전이 있다면 중단
        if (isReloading)
        {
            StopAllCoroutines();
            isReloading = false;
        }

        currentGunIndex = gunIndex;

        // 기존에 들고 있던 총 오브젝트 파괴
        if (gunHolder.childCount > 0)
        {
            Destroy(gunHolder.GetChild(0).gameObject);
        }

        // 새로운 총 프리팹을 gunHolder 자식으로 생성
        Gun newGunPrefab = availableGuns[gunIndex];
        GameObject newGunObject = Instantiate(newGunPrefab.gameObject, gunHolder.position, gunHolder.rotation, gunHolder);
        currentGun = newGunObject.GetComponent<Gun>();

        // 새 총의 정보로 탄약 초기화
        currentAmmo = currentGun.maxAmmo;
        Debug.Log(currentGun.gunName + "으로 교체! 탄약: " + currentAmmo + "/" + currentGun.maxAmmo);
    }

    IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log(currentGun.gunName + " 장전 중...");
        yield return new WaitForSeconds(currentGun.reloadTime);
        currentAmmo = currentGun.maxAmmo;
        Debug.Log("장전 완료! 남은 총알: " + currentAmmo);
        isReloading = false;
    }

    void Shoot()
    {
        currentAmmo--;
        Debug.Log("총알 발사! 남은 총알: " + currentAmmo + " / " + currentGun.maxAmmo);

        // 반동 적용 로직
        if (playerController != null)
        {
            float currentRecoil = currentGun.normalRecoil;
            if (playerController.IsCrouching) currentRecoil = currentGun.crouchingRecoil;
            else if (playerController.IsSprinting) currentRecoil = currentGun.sprintingRecoil;
            playerController.ApplyRecoil(currentRecoil);
        }

        // 레이캐스트 및 시각 효과 로직
        RaycastHit hit;
        Vector3 targetPoint;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, range))
        {
            targetPoint = hit.point;
            EnemyHealth enemy = hit.transform.GetComponentInParent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.TakeDamage(currentGun.damage);
                if (currentGun.bloodImpactPrefab != null) Instantiate(currentGun.bloodImpactPrefab, hit.point, Quaternion.LookRotation(hit.normal));
            }
            else
            {
                if (currentGun.genericImpactPrefab != null) Instantiate(currentGun.genericImpactPrefab, hit.point, Quaternion.LookRotation(hit.normal));
            }
        }
        else
        {
            targetPoint = playerCamera.transform.position + playerCamera.transform.forward * range;
        }

        if (currentGun.bulletPrefab != null && currentGun.firePoint != null)
        {
            Vector3 direction = targetPoint - currentGun.firePoint.position;
            Quaternion bulletRotation = Quaternion.LookRotation(direction);
            Instantiate(currentGun.bulletPrefab, currentGun.firePoint.position, bulletRotation);
        }
    }
}