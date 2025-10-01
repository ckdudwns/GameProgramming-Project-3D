using UnityEngine;
using System.Collections;
using System.Collections.Generic; // List를 사용하기 위해 추가
using UnityEngine.UI;             // UI(Image)를 사용하기 위해 추가

public class PlayerShooting : MonoBehaviour
{
    [Header("공통 설정")]
    [Tooltip("총알이 판정되는 최대 사거리입니다.")]
    public float range = 100f;

    [Header("무기 설정")]
    [Tooltip("사용 가능한 총기들의 프리팹 목록입니다.")]
    public List<Gun> availableGuns;
    [Tooltip("총기 모델이 위치할 기준점입니다. 보통 카메라의 자식으로 만듭니다.")]
    public Transform gunHolder;
    private Gun currentGun; // 현재 장착하고 있는 총의 Gun.cs 스크립트
    private int currentGunIndex = -1;

    [Header("필수 연결 요소")]
    [Tooltip("레이캐스트를 발사할 메인 카메라입니다.")]
    public Camera playerCamera;
    [Tooltip("화면에 표시될 조준선 UI Image 컴포넌트입니다.")]
    public Image crosshairImage;

    // --- Private 변수 ---
    private int currentAmmo; // 현재 총의 남은 총알 수
    private bool isReloading = false;
    private float nextTimeToFire = 0f;
    private Player playerController; // Player.cs 스크립트를 저장할 변수
    private Animator gunAnimator; // 현재 총의 애니메이터를 저장할 변수

    void Start()
    {
        playerController = GetComponent<Player>();

        if (availableGuns != null && availableGuns.Count > 0)
        {
            EquipGun(0); // 첫 번째 총으로 시작
        }
    }

    void Update()
    {
        if (Player.isPaused) return;

        HandleWeaponSwitching();

        if (currentGun == null || isReloading) return;

        // 발사 입력 (마우스 좌클릭)
        if (Input.GetMouseButton(0) && Time.time >= nextTimeToFire)
        {
            if (currentAmmo > 0)
            {
                nextTimeToFire = Time.time + 1f / currentGun.fireRate;
                Shoot();
            }
        }

        // 재장전 입력 (R키), 탄창이 가득 차지 않았을 때
        if (Input.GetKeyDown(KeyCode.R) && currentAmmo < currentGun.maxAmmo)
        {
            StartCoroutine(Reload());
        }
    }

    // 숫자키로 무기를 교체하는 함수
    void HandleWeaponSwitching()
    {
        for (int i = 1; i <= 9; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha0 + i))
            {
                int targetIndex = i - 1;
                if (targetIndex < availableGuns.Count && targetIndex != currentGunIndex)
                {
                    EquipGun(targetIndex);
                }
                break;
            }
        }
    }

    // 지정된 인덱스의 총을 장착하는 함수
    void EquipGun(int gunIndex)
    {
        if (isReloading)
        {
            StopAllCoroutines();
            isReloading = false;
        }

        currentGunIndex = gunIndex;

        if (gunHolder.childCount > 0)
        {
            Destroy(gunHolder.GetChild(0).gameObject);
        }

        Gun newGunPrefab = availableGuns[gunIndex];
        GameObject newGunObject = Instantiate(newGunPrefab.gameObject, gunHolder.position, gunHolder.rotation, gunHolder);
        currentGun = newGunObject.GetComponent<Gun>();

        // 새로 생성된 총 오브젝트에서 Animator 컴포넌트를 찾음
        gunAnimator = newGunObject.GetComponent<Animator>();

        if (currentGun != null)
        {
            newGunObject.transform.localPosition = currentGun.positionOffset;
            newGunObject.transform.localEulerAngles = currentGun.rotationOffset;
        }

        currentAmmo = currentGun.maxAmmo;
        Debug.Log(currentGun.gunName + "으로 교체! 탄약: " + currentAmmo + "/" + currentGun.maxAmmo);

        if (crosshairImage != null)
        {
            if (currentGun.crosshairSprite != null)
            {
                crosshairImage.sprite = currentGun.crosshairSprite;
                crosshairImage.enabled = true;
            }
            else
            {
                crosshairImage.enabled = false;
            }
        }
    }

    // 재장전 코루틴
    IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log(currentGun.gunName + " 장전 중...");

        // 애니메이터가 있다면 "Reload" 트리거를 발동시켜 애니메이션 재생
        if (gunAnimator != null)
        {
            gunAnimator.SetTrigger("Reload");
        }

        yield return new WaitForSeconds(currentGun.reloadTime);

        currentAmmo = currentGun.maxAmmo;
        Debug.Log("장전 완료! 남은 총알: " + currentAmmo);
        isReloading = false;
    }

    // 발사 함수
    void Shoot()
    {
        // --- 여기가 추가된 부분입니다 ---
        Debug.Log("총알 발사! 남은 총알: " + (currentAmmo - 1) + " / " + currentGun.maxAmmo);
        // --- 여기까지 ---
        currentAmmo--;

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

        // 총알 프리팹 생성
        if (currentGun.bulletPrefab != null && currentGun.firePoint != null)
        {
            Vector3 direction = targetPoint - currentGun.firePoint.position;
            Quaternion bulletRotation = Quaternion.LookRotation(direction);
            Instantiate(currentGun.bulletPrefab, currentGun.firePoint.position, bulletRotation);
        }
    }
}