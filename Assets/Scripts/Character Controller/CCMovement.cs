using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// These videos take long to make so I hope this helps you out and if you want to help me out you can by leaving a like and subscribe, thanks!

public class Movement : MonoBehaviour
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
    
    [Header("WallJump")]
    public GameObject WallJumpOrigin;
    public float wallJumpDetectionRange;
    public LayerMask wallLayer;

    [Header("RayGround")]
    public float GroundedRayLength = 1f;
    public Color GroundedRayColor;

    [Header("Raycast Colors")]
    public Color WallRay;
    public Color WallRayFront;
    public Color WallRayBack;
    public Color WallRayRight;
    public Color WallRayLeft;
    public Color LookRay;
    public Color GroundedRay;

    CharacterController controller;
    Vector2 currentDir;
    Vector2 currentDirVelocity;
    Vector3 velocity;
    float velocityY;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        Vector2 targetDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        GravityControl();
        PlayerMove();
        isRayGrounded();
        DrawRays();
        PlayerJump();
    }

    public void GravityControl()
    {
        velocityY += gravity * 2f * Time.deltaTime;
    }

    void PlayerMove()
    {
        Vector2 targetDir = new Vector2(horizontalInput, verticalInput);
        targetDir.Normalize();

        Vector3 velocity = (transform.forward * targetDir.y + transform.right * targetDir.x) * Speed + Vector3.up * velocityY;

        controller.Move(velocity * Time.deltaTime);

        if (isRayGrounded() && Input.GetButtonDown("Jump"))
        {
            velocityY = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    private void PlayerJump()
    {
        if(Input.GetKeyDown(JumpInput) && isRayGrounded())
        {
            velocityY = Mathf.Sqrt(jumpHeight * -2 * gravity);
        }
    }

    private bool isRayGrounded()
    {
        return Physics.Raycast(transform.position, -transform.up * GroundedRayLength, GroundedRayLength);
    }

    private void DrawRays()
    {
        Debug.DrawRay(transform.position, -transform.up * GroundedRayLength, GroundedRayColor);
        if(isRayGrounded())
        {
            Debug.Log("Is Ray Grounded");
        }

        Debug.DrawRay(WallJumpOrigin.transform.position, transform.forward * wallJumpDetectionRange, WallRayFront);
        Debug.DrawRay(WallJumpOrigin.transform.position, -transform.forward * wallJumpDetectionRange, WallRayBack);
        Debug.DrawRay(WallJumpOrigin.transform.position, transform.right * wallJumpDetectionRange, WallRayRight);
        Debug.DrawRay(WallJumpOrigin.transform.position, -transform.right * wallJumpDetectionRange, WallRayLeft);


    }

    IEnumerator WallJump()
    {
        
    }


    private bool CollideFrontwall()
    {
        return Physics.Raycast(WallJumpOrigin.transform.position, transform.forward, wallJumpDetectionRange, wallLayer);
    }

    private bool CollideBackwall()
    {
        return Physics.Raycast(WallJumpOrigin.transform.position, -transform.forward, wallJumpDetectionRange, wallLayer);
    }

    private bool CollideRightwall()
    {
        return Physics.Raycast(WallJumpOrigin.transform.position, transform.right, wallJumpDetectionRange, wallLayer);
    }

    private bool CollideLeftwall()
    {
        return Physics.Raycast(WallJumpOrigin.transform.position, -transform.right, wallJumpDetectionRange, wallLayer);
    }
}