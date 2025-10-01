using UnityEngine;
using UnityEngine.UI; // 조준선 Sprite를 사용하기 위해 추가

// 이 스크립트는 각각의 총기 프리팹(Pistol, Rifle 등)에 붙여줍니다.
public class Gun : MonoBehaviour
{
    [Header("총기 정보")]
    [Tooltip("총의 이름입니다. (UI나 로그 표시에 사용)")]
    public string gunName = "Rifle";

    [Header("총기 고유 능력치")]
    [Tooltip("이 총의 데미지입니다.")]
    public int damage = 10;
    [Tooltip("한 탄창에 들어가는 최대 총알 수입니다.")]
    public int maxAmmo = 30;
    [Tooltip("재장전에 걸리는 시간(초)입니다.")]
    public float reloadTime = 1.5f;
    [Tooltip("연사 속도 (초당 발사 수)")]
    public float fireRate = 10f;

    [Header("반동 설정")]
    [Tooltip("기본 상태일 때의 수직 반동 세기입니다.")]
    public float normalRecoil = 1.5f;
    [Tooltip("앉았을 때의 수직 반동 세기입니다.")]
    public float crouchingRecoil = 0.2f;
    [Tooltip("달릴 때의 수직 반동 세기입니다.")]
    public float sprintingRecoil = 4.0f;

    [Header("총기 필수 요소")]
    [Tooltip("이 총의 총구 위치입니다. 총알이 여기서 발사됩니다.")]
    public Transform firePoint;
    [Tooltip("이 총이 사용하는 총알 프리팹입니다.")]
    public GameObject bulletPrefab;
    [Tooltip("적이 맞았을 때의 파티클 효과입니다.")]
    public GameObject bloodImpactPrefab;
    [Tooltip("환경이 맞았을 때의 파티클 효과입니다.")]
    public GameObject genericImpactPrefab;

    [Header("UI 설정")]
    [Tooltip("이 총을 들었을 때 표시될 조준선 이미지입니다.")]
    public Sprite crosshairSprite;

    [Header("위치/회전 오프셋")]
    [Tooltip("Gun Holder를 기준으로 한 총의 상대 위치 값입니다.")]
    public Vector3 positionOffset;
    [Tooltip("Gun Holder를 기준으로 한 총의 상대 회전 값입니다.")]
    public Vector3 rotationOffset;
}