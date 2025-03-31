
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class RBMovement : MonoBehaviour
{

    [Header("References")]
    public Rigidbody rb;

    [Header("PlayerInputs")]
    public float HorizontalInput;
    public float VerticalInput;
    public KeyCode JumpKey = KeyCode.Space;
    public KeyCode DashKey = KeyCode.LeftControl;

    [Header("Movement Variables")]
    public float MaxWalkSpeed;
    public float WalkSpeed;
    public float AirSpeed;
    Vector3 moveDirection;
    public float GroundDrag;

    [Header("Jumping")]
    public float JumpForce;
    public bool IsJumping;

    [Header("UI")]
    public TextMeshProUGUI VelocityText;

    [Header ("Raycasts")]
    public float GroundedRayLength;
    public Color GroundedRayColor;




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {   
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

        // ------------------------ Variables declarations ------------------------
        HorizontalInput = Input.GetAxisRaw("Horizontal");
        VerticalInput = Input.GetAxisRaw("Vertical");
        moveDirection = transform.forward * VerticalInput + transform.right * HorizontalInput;

        // ---------------------------- Calling Methods ----------------------------
        rb.linearDamping = GroundDrag;
        PlayerMovement();
        UpdateUI();
        DrawRays();
    }

    private void PlayerMovement()
    {
        if(isRayGrounded())
        {
            rb.AddForce(moveDirection.normalized * WalkSpeed * 10f * Time.deltaTime, ForceMode.Force);
        }else if (!isRayGrounded())
        {
            rb.AddForce(moveDirection.normalized * AirSpeed * 10f * Time.deltaTime, ForceMode.Force);
            rb.linearDamping = 0f;
        }
    
        if(Input.GetKeyDown(JumpKey) && isRayGrounded())
        {
            PlayerJump();
        }

        if(rb.linearVelocity.magnitude > MaxWalkSpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * MaxWalkSpeed;
        }
    }

    private void PlayerJump()
    {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        rb.AddForce(transform.up * JumpForce, ForceMode.Impulse);
    }

    private bool isRayGrounded()
    {
        return Physics.Raycast(transform.position, -transform.up, GroundedRayLength);
    }

    private void DrawRays()
    {
        Debug.DrawRay(transform.position, -transform.up * GroundedRayLength, GroundedRayColor);
        if (isRayGrounded())
        {
            Debug.Log("Is Grounded by Raycast");
        }
    }

    private void UpdateUI()
    {
        if (VelocityText != null)
        {
            VelocityText.text = "Vitesse : " + rb.linearVelocity.magnitude.ToString("F2");
        }
    }
}
