using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovementCC : MonoBehaviour
{
    //private PlayerInputs _input;
    [SerializeField] private CharacterController controller;
    [SerializeField] private Transform playerCam;
    [SerializeField] private float speed = 3f;
    [SerializeField] private float sprintSpeed = 4f;

    [Header("Lean parameters")]
    [SerializeField] private Transform leanPivot;
    private float currentLean;
    private float targetLean;
    [SerializeField] private float leanAngle;
    [SerializeField] private float leanSmoothing;
    private float leanVelocity;
    private bool isLeaningLeft;
    private bool isLeaningRight;

    [Header("")]
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float jumpHeight = 3f;
    private Vector3 crouchScale = new Vector3(1f, 0.5f, 1f);
    private Vector3 playerScale;
    private Vector3 velocity;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance = 0.4f;
    [SerializeField] private LayerMask groundMask;
    Vector2 HorizontalInput;
    [SerializeField] private bool isGrounded;
    [SerializeField] public static bool isCrouching;
    public static bool isSprinting = false;
    public static bool canSprint = true;
    public bool paused = false;
    public static bool isWalking = false;
    [SerializeField] protected bool canUseHeadBob = true;

    [Header("FootStep Parametrs")]
    [SerializeField] private float baseStepSpeed = 0.2f;
    [SerializeField] private float crouchStepMultipler = 1.5f;
    [SerializeField] private float sprintStepMultipler = 0.6f;
    [SerializeField] private AudioSource footstepAudioSource = default;
    [SerializeField] private AudioClip[] MudStepClips = default;
    [SerializeField] private AudioClip[] WaterStepClips = default;
    [SerializeField] private AudioClip[] MetalStepClips = default;
    [SerializeField] private AudioClip[] RockStepClips = default;
    [SerializeField] private AudioClip[] SnowStepClips = default;
    public float footStepTimer = 0;
    private float GetCurrentOffset => isCrouching ? baseStepSpeed * crouchStepMultipler : isSprinting ? baseStepSpeed * sprintStepMultipler : baseStepSpeed;

    [Header("Headbob Parameters")]
    [SerializeField] private float walkBobSpeed = 14f;
    [SerializeField] private float walkBobAmount = 0.5f;
    [SerializeField] private float sprintBobSpeed = 18f;
    [SerializeField] private float sprintBobAmount = 1f;
    [SerializeField] private float crouchBobSpeed = 8f;
    [SerializeField] private float crouchBobAmount= 0.25f;
    private float defaultYPos = 0f;
    private float timer;

    private void Awake()
    {
        defaultYPos = playerCam.transform.localPosition.y;
        isSprinting = false;
        isWalking = false;
    }
    void Start()
    {
        playerScale = transform.localScale;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    void Update()
    {
        CheckGround();
        Movement();
        checkWalk();
        StartCrouch();
        StopCrouch();
        Jump();

        
        StartSprint();

        if (Input.GetKeyUp(KeyCode.LeftShift) || PlayerStats.instance.currentStamina <= 0f)
            StopSprint();

        HandleHeadbob();

        if (isWalking)
        {
            Handle_FootSteps();
        }
    }

    void Movement()
    {
        //HorizontalInput = _input.Player.Move.ReadValue<Vector2>();
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        //With ClampMagnitude, the magnitude can be smaller than 1, giving a more smooth movement. Also makes Diagonal Speed normal
        move = Vector3.ClampMagnitude(move, 1f);

        controller.Move(move * speed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }

    private void StartCrouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            if (!isCrouching && !Physics.Raycast(playerCam.transform.position, Vector3.up, 1f))
            {
                isCrouching = true;
                controller.height = 1;
                speed = 1f;
            }
        }
    }

    public void StopCrouch()
    {
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            if (isCrouching && !Physics.Raycast(playerCam.transform.position, Vector3.up, 1f))
            {
                isCrouching = false;
                controller.height = 2;
                speed = 3f;
            }
        }
    }

    private void Handle_FootSteps()
    {
        if (!isGrounded) return;

        footStepTimer -= Time.deltaTime;
        if (footStepTimer <= 0)
        {
            if (Physics.Raycast(groundCheck.transform.position, Vector3.down, out RaycastHit hit, 3))
            {
                switch (hit.collider.tag)
                {
                    case "Footsteps/Metal":
                        footstepAudioSource.PlayOneShot(MetalStepClips[UnityEngine.Random.Range(0, MetalStepClips.Length - 1)]);
                        break;
                    case "Footsteps/Mud":
                        footstepAudioSource.PlayOneShot(MudStepClips[UnityEngine.Random.Range(0, MudStepClips.Length - 1)]);
                        break;
                    case "Footsteps/Water":
                        footstepAudioSource.PlayOneShot(WaterStepClips[UnityEngine.Random.Range(0, WaterStepClips.Length - 1)]);
                        break;
                    case "Footsteps/Rock":
                        footstepAudioSource.PlayOneShot(RockStepClips[UnityEngine.Random.Range(0, RockStepClips.Length - 1)]);
                        break;
                    case "Footsteps/Snow":
                        footstepAudioSource.PlayOneShot(SnowStepClips[UnityEngine.Random.Range(0, SnowStepClips.Length - 1)]);
                        break;
                    default:
                        break;
                }
            }
            footStepTimer = GetCurrentOffset;
        }
    }

    private void HandleHeadbob()
    {
        if(!canUseHeadBob && !isGrounded) return;

        if (isWalking)
        {
            timer += Time.deltaTime * (isCrouching ? crouchBobSpeed : isSprinting ? sprintBobSpeed : walkBobSpeed);
            playerCam.transform.localPosition = new Vector3(playerCam.transform.localPosition.x, defaultYPos + Mathf.Sin(timer)
                * (isCrouching ? crouchBobAmount : isSprinting ? sprintBobAmount : walkBobAmount), playerCam.transform.localPosition.z);
        }
    }

    void CheckGround()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
    }

    public void checkWalk()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)))
        {
            isWalking = true;
        }
        else if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S) || (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D)))
        {
            isWalking = false;
        }
    }

    void Jump()
    {
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    public void StartSprint()
    {
        if (!isCrouching && Input.GetKeyDown(KeyCode.LeftShift) && PlayerStats.instance.currentStamina > 0f && canSprint)
        {
            isSprinting = true;
            speed = sprintSpeed;
        }
    }

    public void StopSprint()
    {
        isSprinting = false;
        speed = 3f;
    }

    public void pauseMenu()
    {
        if (!paused)
        {
            PauseGame();
        }
        else
        {
            UnPauseGame();
        }
    }

    public void PauseGame()
    {
       Time.timeScale = 0;
       paused = true;
    }

    public void UnPauseGame()
    {
        Time.timeScale = 1;
        paused = false;
    }
}
