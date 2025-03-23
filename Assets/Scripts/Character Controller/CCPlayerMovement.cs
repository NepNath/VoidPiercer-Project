using UnityEngine;
using System.Collections;
using TMPro;

public class CCPlayerMovement : MonoBehaviour
{
    [Header("Mouvement Général")]
    public float walkSpeed = 7f;
    public float sprintSpeed = 12f;
    public float gravity = -9.81f;
    public float jumpHeight = 2f;

    [Header("References")]
    public Camera playerCamera;
    PlayerHealthManager hlth;
    PlayerMovement pm;


    [Header("Dash")]
    public float dashForce = 20f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;
    private bool canDash = true;

    [Header("Wall Jump")]
    public float wallJumpForce = 10f;
    public LayerMask wallLayer;
    public float wallJumpDetectionRange = 1f;

    [Header("Slide")]
    public float slideSpeed = 15f;
    public float slideDuration = 0.5f;
    private bool isSliding = false;

    [Header("UI")]
    public TextMeshProUGUI VelocityText;
    public TextMeshProUGUI PlayerStateText;
    public TextMeshProUGUI PlayerFOVText;
    public TextMeshProUGUI PlayerHealthText;

    [Header("Raycast Colors")]
    public Color WallRay;
    public Color LookRay;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private bool isDashing = false;
    private float originalHeight;
    private Vector3 previousPosition;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        originalHeight = controller.height;
        previousPosition = transform.position;
        hlth = GetComponent<PlayerHealthManager>();
        pm = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        isGrounded = controller.isGrounded;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float InputX = Input.GetAxis("Horizontal");
        float InputZ = Input.GetAxis("Vertical");

        Vector3 move = transform.right * InputX + transform.forward * InputZ;
        float speed = walkSpeed;

        if (!isDashing && !isSliding)
        {
            controller.Move(move * speed * Time.deltaTime);
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.LeftControl) && isGrounded)
        {
            StartCoroutine(Slide());
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash(move));
        }

        if (!isGrounded && CheckWall() && Input.GetKeyDown(KeyCode.Space))
        {
            WallJump();
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);


        UpdateUI();
        RaycastsDraw();
    }

    private void Jump()
    {
        velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
    }

    private IEnumerator Dash(Vector3 direction)
    {
        canDash = false;
        isDashing = true;
        float startTime = Time.time;

        while (Time.time < startTime + dashDuration)
        {
            controller.Move(direction.normalized * dashForce * Time.deltaTime);
            yield return null;
        }

        isDashing = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    private IEnumerator Slide()
    {
        isSliding = true;
        controller.height = originalHeight / 2;
        float startTime = Time.time;

        while (Time.time < startTime + slideDuration)
        {
            controller.Move(transform.forward * slideSpeed * Time.deltaTime);
            yield return null;
        }

        controller.height = originalHeight;
        isSliding = false;
    }

    private bool CheckWall()
    {
        return Physics.Raycast(transform.position, transform.right, wallJumpDetectionRange, wallLayer) ||
               Physics.Raycast(transform.position, -transform.right, wallJumpDetectionRange, wallLayer);
    }

    private void WallJump()
    {
        velocity.y = Mathf.Sqrt(wallJumpForce * -2f * gravity);
        velocity += transform.forward * 5f;
    }

    private void UpdateUI()
    {
        if (VelocityText != null)
        {
            Vector3 currentPosition = transform.position;
            float velocityMagnitude = (currentPosition - previousPosition).magnitude / Time.deltaTime;
            previousPosition = currentPosition;

            VelocityText.text = "Vitesse : " + velocityMagnitude.ToString("F2");
            if (velocityMagnitude > 21)
            {
                VelocityText.color = Color.red;
                VelocityText.text = "Vitesse : " + velocityMagnitude.ToString("F2") + " ! ! !";
            }
            else
            {
                VelocityText.color = Color.white;
            }
            }

            if (PlayerFOVText != null)
            {
                PlayerFOVText.text = "Actual FOV : " + playerCamera.fieldOfView;
            }

            if (PlayerHealthText != null)
            {
                PlayerHealthText.text = "Health : " + hlth.playerHealth;
            }
    }

    private void RaycastsDraw()
    {
        Debug.DrawRay(transform.position, transform.right * wallJumpDetectionRange, WallRay);
        Debug.DrawRay(transform.position, -transform.right * wallJumpDetectionRange, WallRay);
        Debug.DrawRay(transform.position, transform.forward * wallJumpDetectionRange, WallRay);
        Debug.DrawRay(transform.position, -transform.forward * wallJumpDetectionRange, WallRay);
    }
}
