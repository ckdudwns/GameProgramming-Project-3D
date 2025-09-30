using UnityEngine;

// 이 스크립트는 각각의 총기 프리팹(Pistol, Rifle 등)에 붙여줍니다.
public class Gun : MonoBehaviour
{
    [Header("총기 고유 능력치")]
    public string gunName = "Rifle";
    public int damage = 10;
    public int maxAmmo = 30;
    public float reloadTime = 1.5f;
    public float fireRate = 10f;

    [Header("반동 설정")]
    public float normalRecoil = 1.5f;
    public float crouchingRecoil = 0.2f;
    public float sprintingRecoil = 4.0f;

    [Header("총기 필수 요소")]
    public Transform firePoint;
    public GameObject bulletPrefab;
    public GameObject bloodImpactPrefab;
    public GameObject genericImpactPrefab;
}