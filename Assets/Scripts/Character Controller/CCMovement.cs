using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class CCMovement : MonoBehaviour
{
    [Header("PlayerInput")]
    public KeyCode JumpInput = KeyCode.Space;
    public KeyCode DashInput = KeyCode.LeftShift;

    [Header("Movement")]
    private float horizontalInput;
    private float verticalInput;
    public float Speed = 6.0f;
    public float gravity = -10f;

    [Header("PlayerJump")]
    public float jumpHeight = 6f;
    public int MaxJump;
    private int JumpLeft = 0;
    private bool IsJumping;
    private bool CanJump = true;
    public float JumpRechargeSeconds = 1f;

    [Header("WallJump")]
    public GameObject WallJumpOrigin;
    public float wallJumpDetectionRange;
    public LayerMask wallLayer;
    public float WallJumpForce;
    public int MaxWallJump;
    private int WallJumpLeft = 0;
    private bool IsWallJumping;

    [Header("RayGround")]
    public float GroundedRayLength = 1f;
    public Color GroundedRayColor;

    [Header("Raycast Colors")]
    public Color WallRayFront;
    public Color WallRayBack;
    public Color WallRayRight;
    public Color WallRayLeft;

    CharacterController controller;
    Vector3 velocity;
    float velocityY;
    float decelerationRate = 1f; // Facteur de ralentissement après un Wall Jump

    void Start()
    {
        controller = GetComponent<CharacterController>();
        JumpLeft = MaxJump;
        WallJumpLeft = MaxWallJump;
    }

    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        GravityControl();
        PlayerMove(); 
        isRayGrounded();
        DrawRays();
        PlayerJump();
        PlayerWallJump();

        // Appliquer la réduction de vitesse X/Z après un Wall Jump
        if (IsWallJumping)
        {
            velocity.x = Mathf.Lerp(velocity.x, 0, Time.deltaTime * decelerationRate);
            velocity.z = Mathf.Lerp(velocity.z, 0, Time.deltaTime * decelerationRate);
        }
    }

    public void GravityControl()
    {
        velocityY += gravity * Time.deltaTime * 2f;
    }

    void PlayerMove()
    {
        Vector2 targetDir = new Vector2(horizontalInput, verticalInput).normalized;

        // Applique la vitesse de déplacement seulement si on ne Wall Jump pas
        if (!IsWallJumping)
        {
            velocity.x = (transform.forward * targetDir.y + transform.right * targetDir.x).x * Speed;
            velocity.z = (transform.forward * targetDir.y + transform.right * targetDir.x).z * Speed;
        }

        // Toujours appliquer la gravité
        velocity.y = velocityY;

        controller.Move(velocity * Time.deltaTime);
    }

    private void PlayerJump() //appel de l'ienumerator la dedans
    {
        if (Input.GetKeyDown(JumpInput) && JumpLeft > 0 && CanJump )
        {

            StartCoroutine(Jump());
        }
        resetJump();

    }

    private void resetJump()
    {
        if(isRayGrounded() || IsWallJumping)
        {
            JumpLeft = MaxJump;
        }

        
    }



    private IEnumerator Jump()
    {
        IsJumping = true;
        CanJump = false;
        JumpLeft -= 1;

        velocityY = Mathf.Sqrt(jumpHeight * -2 * gravity);
        yield return new WaitForSeconds(JumpRechargeSeconds);

        IsJumping = false;
        CanJump = true;
    }

    private void PlayerWallJump()
    {
        if (IsWallJumping || isRayGrounded()) return; // Ne fait rien si déjà en Wall Jump ou au sol

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (CollideBackwall()) ApplyWallJump(transform.forward);
            else if (CollideFrontwall()) ApplyWallJump(-transform.forward);
            else if (CollideRightwall()) ApplyWallJump(-transform.right);
            else if (CollideLeftwall()) ApplyWallJump(transform.right);
        }
    }

    private void ApplyWallJump(Vector3 direction)
    {
        
        IsWallJumping = true;
        WallJumpLeft -= 1;
        Debug.Log("Wall Jump!");

        // Impulsion en Y pour donner du saut
        velocityY = Mathf.Sqrt(jumpHeight * -1.5f * gravity);
        
        // Impulsion en X et Z
        velocity += direction * Mathf.Sqrt(WallJumpForce * -2f * gravity);

        // Désactiver le Wall Jump après un certain temps
        StartCoroutine(EndWallJump());
    }

    IEnumerator EndWallJump()
    {
        yield return new WaitForSeconds(0.3f);
        IsWallJumping = false;
    }

    private bool isRayGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, GroundedRayLength);
    }

    private void DrawRays()
    {
        Debug.DrawRay(transform.position, Vector3.down * GroundedRayLength, GroundedRayColor);
        Debug.DrawRay(WallJumpOrigin.transform.position, transform.forward * wallJumpDetectionRange, WallRayFront);
        Debug.DrawRay(WallJumpOrigin.transform.position, -transform.forward * wallJumpDetectionRange, WallRayBack);
        Debug.DrawRay(WallJumpOrigin.transform.position, transform.right * wallJumpDetectionRange, WallRayRight);
        Debug.DrawRay(WallJumpOrigin.transform.position, -transform.right * wallJumpDetectionRange, WallRayLeft);
    }

    private bool CollideFrontwall() => Physics.Raycast(WallJumpOrigin.transform.position, transform.forward, wallJumpDetectionRange, wallLayer);
    private bool CollideBackwall() => Physics.Raycast(WallJumpOrigin.transform.position, -transform.forward, wallJumpDetectionRange, wallLayer);
    private bool CollideRightwall() => Physics.Raycast(WallJumpOrigin.transform.position, transform.right, wallJumpDetectionRange, wallLayer);
    private bool CollideLeftwall() => Physics.Raycast(WallJumpOrigin.transform.position, -transform.right, wallJumpDetectionRange, wallLayer);
}
