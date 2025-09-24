using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    [Header("�ɷ�ġ ����")]
    public int currentCoins; // ���� ���� (public���� �����Ͽ� Inspector���� Ȯ�� ����)

    [Header("�̵� �ӵ�")]
    public float moveSpeed = 6.0f;
    public float sprintSpeed = 10.0f;

    [Header("�ɱ� ����")]
    public float crouchSpeed = 3.0f;
    public float standingHeight = 2.0f;
    public float crouchingHeight = 1.0f;

    [Header("���� ����")]
    public float jumpHeight = 1.0f;
    [Header("�߷� ��")]
    public float gravityValue = -9.81f;

    [Header("���콺 ����")]
    public float mouseSensitivity = 2.0f;

    [Header("ī�޶� ������Ʈ")]
    public Transform playerCamera;

    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private float verticalLookRotation = 0f;

    public bool IsSprinting { get; private set; }
    public bool IsCrouching { get; private set; }
    public static bool isPaused = false;

    void Start()
    {
        // --- 1. ���� �� ���� ���� �� �α� ��� ---
        currentCoins = 1000;
        // ���� ���� ������ �α׷� ���
        Debug.Log("���� ����! ���� ���� ����: " + currentCoins);
        // --- ������� ---

        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        controller.height = standingHeight;
    }

    // ������ �߰��ϰ� �α׸� ����ϴ� �Լ�
    public void AddCoins(int amount)
    {
        currentCoins += amount;
        // --- 2. ���� ȹ�� �� �α� ��� ---
        // ȹ���� ���ΰ� �Բ� ���� �� ���� ������ �α׷� ���
        Debug.Log(amount + " ���� ȹ��! ���� ���� ����: " + currentCoins);
        // --- ������� ---
    }

    // --- ���� �ڵ�� ������ �����մϴ� ---
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) TogglePause();
        if (isPaused) return;
        HandleCrouch();
        HandleMovement();
        HandleLook();
    }

    public void ApplyRecoil(float verticalRecoil)
    {
        verticalLookRotation -= verticalRecoil;
    }

    void HandleCrouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            if (!IsCrouching)
            {
                controller.height = crouchingHeight;
                IsCrouching = true;
            }
            else
            {
                if (CanStandUp())
                {
                    controller.height = standingHeight;
                    IsCrouching = false;
                }
            }
        }
    }

    bool CanStandUp()
    {
        return !Physics.Raycast(transform.position, Vector3.up, standingHeight);
    }

    void HandleMovement()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0) playerVelocity.y = -2f;

        IsSprinting = Input.GetKey(KeyCode.LeftShift) && !IsCrouching;

        float currentSpeed = moveSpeed;
        if (IsCrouching) currentSpeed = crouchSpeed;
        else if (IsSprinting) currentSpeed = sprintSpeed;

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 move = transform.right * horizontal + transform.forward * vertical;
        controller.Move(move * currentSpeed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && groundedPlayer && !IsCrouching)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    void HandleLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;
        transform.Rotate(Vector3.up * mouseX);

        verticalLookRotation -= mouseY;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90f, 90f);
        playerCamera.localRotation = Quaternion.Euler(verticalLookRotation, 0f, 0f);
    }

    void TogglePause()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0f : 1f;
        Cursor.lockState = isPaused ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = isPaused;
    }
}