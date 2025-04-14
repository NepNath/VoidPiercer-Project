using System;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;

public class Movement : MonoBehaviour
{
    [Header("References")]
    Rigidbody rb;
    [SerializeField] Transform cam;
    [SerializeField] Transform orientaiton;

    [Header("Mouvements")]
    [SerializeField] float moveSpeed = 6f;
    [SerializeField] float speedMultiplier;
    [SerializeField] float airMultiplier = 0.4f;
    [SerializeField] float maxSpeed = 15f;

    [Header("Drags")]
    [SerializeField] float GroundDrag;
    [SerializeField] float AirDrag;

    [Header("Jump")]
    public float JumpForce = 10f;
    [SerializeField] float GroundRayLength;

    private float horizontalMovement;
    private float verticalMovement;
    private Vector3 moveDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

    }

    private void Update()
    {
        
        Inputs();
        Debugs();        
        jump();
        rbDrag();

    }

    private void FixedUpdate()
    {
        PlayerMove();
        
    }

    private void PlayerMove()
    {

        if(IsRayGrounded())
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * speedMultiplier, ForceMode.Acceleration);
        }
        else if(!IsRayGrounded())
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * speedMultiplier * airMultiplier, ForceMode.Acceleration);
        }
        
    }

    void jump()
    {
        if(Input.GetKeyDown(KeyCode.Space) && IsRayGrounded())
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            rb.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
        }
    }

    void rbDrag()
    {
        if(IsRayGrounded())
        {
            speedMultiplier = 10f;
            rb.linearDamping = GroundDrag;
        } 
        else if (!IsRayGrounded())
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

    private void Debugs()
    {
        Debug.DrawRay(transform.position, Vector3.down * GroundRayLength, Color.green);

        if(IsRayGrounded())
        {
            Debug.Log("Is Grounded by Raycast");
        }
    }

    

    private bool IsRayGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, GroundRayLength);
    }

}