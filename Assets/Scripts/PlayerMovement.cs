using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Management")]
    public Transform orientation;
    Vector3 moveDirection;
    private float movementSpeed;
    public float walkSpeed;
    public float dashSpeed;
    public float airSpeed;
    public float GroundDrag;

    public bool dashing;

    [Header("Ground Detection")]
    public float Height;
    public LayerMask Ground;
    bool IsGrounded;

    [Header("Slope management")]
    public float  maxSlopeAngle;
    private RaycastHit slopeHit;
    private bool exitingSlope;

    [Header("Jump Management")]
    public float playerWeight;
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool ReadyToJump;

    [Header("PlayerInput")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode DashKey = KeyCode.LeftShift;

    [Header("UI")]
    public TextMeshProUGUI VelocityText;
    public TextMeshProUGUI PlayerStateText;
    public TextMeshProUGUI PlayerFOVText;
    public TextMeshProUGUI PlayerHealthText;

    [Header("Camera Settings")]
    public Camera playerCamera;
    public float baseFOV;
    public float maxFOV;
    public float fovLerpSpeed;

    [Header("Dash Variables")]
    private float DesiredMoveSpeed;
    private float LaseDesiredMoveSpeed;
    private MovementState LastState;
    private bool KeepMomentum;

    float horizontalInput;
    float verticalInput;
    
    Rigidbody rb;
    PlayerHealthManager hlth;
    
    public MovementState state;

    public enum MovementState
    {
        walking,
        dashing,
        air,
        slope,
    }
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        ReadyToJump = true; 
        hlth = GetComponent<PlayerHealthManager>();
       
    }

    private void Update()
    {
        
        IsGrounded = Physics.Raycast(transform.position, Vector3.down, Height * 0.5f + 0.2f, Ground);
        if (IsGrounded) {
            Debug.DrawRay(transform.position, Vector3.down * (Height * 0.5f + 0.2f), Color.green);
        } else {
            Debug.DrawRay(transform.position, Vector3.down * (Height * 0.5f + 0.2f), Color.red);
        }

        if(state == MovementState.walking){
            rb.linearDamping = GroundDrag;
        } else {
            rb.linearDamping = 0;
        }
        if(!IsGrounded)
        {
            rb.AddForce(Vector3.down * playerWeight, ForceMode.Acceleration);
        }

        SpeedControl();
        PlayerInput();
        StateHandler();
        UpdateUI();
        UpdateFOV();
    }

    private void FixedUpdate()
    {
        PlayerMove();
    }

    private void StateHandler()
    {
        if(IsGrounded)
        {
            state = MovementState.walking;
            DesiredMoveSpeed = walkSpeed;
        }
        
        if(!IsGrounded)
        {
            state = MovementState.air;
            DesiredMoveSpeed = airSpeed;
        }
        if(OnSlope())
        {
            state = MovementState.slope;
        }

        if(dashing)
        {
            state = MovementState.dashing;
            DesiredMoveSpeed  = dashSpeed;
        }
    }

    private void PlayerInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if(Input.GetKey(jumpKey) && ReadyToJump && IsGrounded)
        {
            ReadyToJump = false;
            Jump();
            Invoke(nameof(JumpReset), jumpCooldown);
        }
    }

    private void PlayerMove()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput; 
        if(IsGrounded)
        {
            rb.AddForce(moveDirection.normalized * movementSpeed * 10f, ForceMode.Force); 
        } 
        if (!IsGrounded)
        {
            rb.AddForce(moveDirection.normalized * movementSpeed * 10f * airMultiplier, ForceMode.Force);
        }
        if(OnSlope() && !exitingSlope )
        {
            rb.AddForce(GetSlopeMoveDirection() * movementSpeed * 20f, ForceMode.Force);
            if(rb.linearVelocity.y > 0)
            {
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
            }
        }
        rb.useGravity = !OnSlope();
    }

    private void SpeedControl()
    {
        if(OnSlope() && !exitingSlope)
        {
            if(rb.linearVelocity.magnitude > movementSpeed)
            {
                rb.linearVelocity = rb.linearVelocity.normalized * movementSpeed;
            }
        } else {
            Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            if (flatVel.magnitude > movementSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * movementSpeed;
                rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
            }
        }
        
        
    }

    private void Jump()
    {
        exitingSlope = true;
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void JumpReset()
    {
        ReadyToJump = true;

        exitingSlope = false; 
    }

    private bool OnSlope()
    {
        if(Physics.Raycast(transform.position, Vector3.down, out slopeHit, Height * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }
        return false;
    }

    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }

    private void UpdateUI()
    {
        if(VelocityText != null)
        {
            VelocityText.text = "Vitesse : " + rb.linearVelocity.magnitude.ToString("F2");
           if(rb.linearVelocity.magnitude > 21)
            {
                VelocityText.color = Color.red;
                VelocityText.text = "Vitesse : " + rb.linearVelocity.magnitude.ToString("F2") + " ! ! !";
            } else
            {
                VelocityText.color = Color.white;
            }
        }

        if(PlayerStateText != null)
        {
            PlayerStateText.text = "Player State : " + state;
        }

        if(PlayerFOVText != null)
        {
            PlayerFOVText.text = "Actual FOV : " + playerCamera.fieldOfView;
        }

        if(PlayerHealthText != null)
        {
            PlayerHealthText.text = "Health : " + hlth.playerHealth;
        }

    }

    private void UpdateFOV()
    {
        if (playerCamera != null)
        {
            float speed = rb.linearVelocity.magnitude;
            float targetFOV = Mathf.Lerp(baseFOV, maxFOV, speed / dashSpeed); // Ajuste selon la vitesse
            playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, targetFOV, Time.deltaTime * fovLerpSpeed);
        }
    }
}
