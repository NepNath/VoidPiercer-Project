using System;
using System.IO.Compression;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;

public class Movement : MonoBehaviour
{
    [Header("References")]
    WallActions wall;
    Rigidbody rb;
    Dash dash;
    [SerializeField] Transform cam;
    [SerializeField] Transform orientaiton;
    [SerializeField] LayerMask groundLayer;

    [Header("Mouvements")]
    [SerializeField] float moveSpeed = 6f;
    public float speedMultiplier;
    [SerializeField] float airMultiplier = 0.4f;
    [SerializeField] float wallSpeedMultiplier = 2f;
    [SerializeField] float maxSpeed = 15f;

    [Header("Drags")]
    [SerializeField] float GroundDrag;
    [SerializeField] float AirDrag;
    [SerializeField] float wallDrag;
    [SerializeField] float dashDrag;

    [Header("Jump")]
    public float JumpForce = 10f;
    public float GroundRayLength;
    public float JumpGizmoRad;

    [Header("Slope")]
    RaycastHit SlopeHit;
    Vector3 slopeMoveDirection;

    private float horizontalMovement;
    private float verticalMovement;
    public Vector3 moveDirection;

    void Start()
    {
        wall = GetComponent<WallActions>();
        rb = GetComponent<Rigidbody>();
        dash = GetComponent<Dash>();
        rb.freezeRotation = true;
    }

    private void Update()
    {
        Inputs();
        jump();
        rbDrag();

        slopeMoveDirection = Vector3.ProjectOnPlane(moveDirection, SlopeHit.normal);
    }

    private void FixedUpdate()
    {
        PlayerMove();
    }

    private void PlayerMove()
    {
        if(IsGrounded() && !OnSlope())
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * speedMultiplier, ForceMode.Acceleration);
        }
        else if(IsGrounded() && OnSlope())
        {
            rb.useGravity = false;
            rb.AddForce(slopeMoveDirection.normalized * moveSpeed * speedMultiplier, ForceMode.Acceleration);
        }
        else if(!IsGrounded())
        {
            rb.useGravity = true;
            rb.AddForce(moveDirection.normalized * moveSpeed * speedMultiplier * airMultiplier, ForceMode.Acceleration);
        }
        if(wall.isWallRunning && !IsGrounded())
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * speedMultiplier * wallSpeedMultiplier, ForceMode.Acceleration);
        }
        
        
    }

    public bool OnSlope()
    {
        if(Physics.Raycast(transform.position, Vector3.down, out SlopeHit, GroundRayLength))
        {
            if(SlopeHit.normal != Vector3.up)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }

    void jump()
    {
        if(Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            rb.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
        }
    }

    public void rbDrag()
    {
        if(IsGrounded())
        {
            speedMultiplier = 10f;
            rb.linearDamping = GroundDrag;
        } 
        else if(dash.IsDashing)
        {
            rb.linearDamping = dashDrag;
        }
        else if(wall.isWallRunning)
        {
            speedMultiplier = wallSpeedMultiplier;
            rb.linearDamping = wallDrag;
        }
        else if(wall.isClimbing)
        {
            rb.linearDamping = wallDrag;
        }
        else if (!IsGrounded())
        {
            speedMultiplier = 1f;
            rb.linearDamping = AirDrag;
        }
    }

    private void Inputs()
    {
        horizontalMovement = Input.GetAxisRaw("Horizontal");
        verticalMovement = Input.GetAxisRaw("Vertical");
        moveDirection = orientaiton.forward * verticalMovement + orientaiton.right * horizontalMovement;
    }

    public bool IsGrounded()
    {
        return Physics.CheckSphere(transform.position - new Vector3(0, 0.75f, 0),JumpGizmoRad, groundLayer);
    }

}