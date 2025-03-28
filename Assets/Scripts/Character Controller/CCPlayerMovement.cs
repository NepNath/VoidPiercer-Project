using UnityEngine;
using System.Collections;
using TMPro;
using NUnit.Framework.Internal;
using UnityEditor.MPE;
using UnityEditor.Rendering;

public class CCPlayerMovement : MonoBehaviour
{
    [Header("Mouvement Général")]
    public float walkSpeed = 7f;
    public float sprintSpeed = 12f;
    public float gravity = -9.81f;
    public float impulseForce = 2f;
    public float impulseCancelForce = -2f;
    Vector3 move;
   

    [Header("References")]
    public Camera playerCamera;
    PlayerMovement pm;
    PlayerHealthManager hlth;
    public GameObject WallJumpOrigin;

    [Header("PlayerInput")]
    float horizontalInput;
    float verticalInput;

    [Header("Dash")]
    public float dashForce = 20f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;
    private bool canDash = true;
    private int dashCount;
    private bool recharging;
    public float rechargeTime;
    private bool isDashing;
    public int maxDash;

    [Header("jump")]
    public float jumpHeight = 2f;
    public float CharaControlHeightExtend;
    public bool isRayGrounded;
    public LayerMask GroundLayer;
    private int jumpCounter;
    public float jumpRechargeTimer = 1f;
    public bool CanJump;
    public bool IsJumping;
    public int maxjump = 3;

    [Header("Wall Jump")]
    public float wallJumpForce = 10f;
    public LayerMask wallLayer;
    public float wallJumpDetectionRange = 1f;
    public float elapsedTime = 0f;
    public float wallJumpDuration = 0.5f;
    public bool IsWallJumping;

    [Header("Slide")]
    public float slideSpeed = 15f;
    public float slideDuration = 0.5f;

    [Header("UI")]
    public TextMeshProUGUI VelocityText;
    public TextMeshProUGUI JumpText;
    public TextMeshProUGUI DashText;
    public TextMeshProUGUI PlayerFOVText;
    public TextMeshProUGUI PlayerHealthText;

    [Header("Raycast Colors")]
    public Color WallRay;
    public Color WallRayFront;
    public Color WallRayBack;
    public Color WallRayRight;
    public Color WallRayLeft;
    public Color LookRay;
    public Color GroundedRay;

    private CharacterController controller;
    private Vector3 velocity;
    private float originalHeight;
    private Vector3 previousPosition;
    

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        originalHeight = controller.height;
        previousPosition = transform.position;
        pm = GetComponent<PlayerMovement>();
        hlth = GetComponent<PlayerHealthManager>();
    }

    private void Update()
    {

        //--------------------------making variables--------------------------
        isRayGrounded = Physics.Raycast(transform.position, -transform.up, controller.height * CharaControlHeightExtend, GroundLayer);

        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        move = transform.forward * verticalInput + transform.right * horizontalInput; 
        
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
        // --------------------------------------------------------------------

        

        //--------------------------calling methods--------------------------
        UpdateUI();
        RaycastsDraw();
        PlayerMovement();
        Dash();
        textDebug();
        DshRrgManager();
        jumpRecharge();
        ResetLinearVelocity();
        //--------------------------------------------------------------------
    }


    private void PlayerMovement()
    {
        float speed = walkSpeed;

        if (isRayGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        
        if (!isDashing && !IsWallJumping)
        {
            controller.Move(move.normalized * speed * Time.deltaTime);
        }

        //--------------------------Player Jump & wall jump--------------------------
        if (Input.GetKeyDown(KeyCode.Space) && jumpCounter > 0 && CanJump && !CollideFrontwall() && !CollideBackwall() && !CollideLeftwall() && !CollideRightwall())
        {
            StartCoroutine(Jump());
            ResetLinearVelocity();
            IsWallJumping = false;
        }
        
        else if(Input.GetKeyDown(KeyCode.Space) && isRayGrounded && !IsWallJumping)
        {
            ResetLinearVelocity();
            StartCoroutine(Jump());
        }

        else if(Input.GetKeyDown(KeyCode.Space) && CollideBackwall())
        {
            Debug.Log("back wall Jump");
            velocity.y = Mathf.Sqrt(impulseForce * -2f * gravity);
            velocity += transform.forward * Mathf.Sqrt(impulseForce * -2f * gravity);
            IsWallJumping = true;

        }

        else if(Input.GetKeyDown(KeyCode.Space) && CollideFrontwall())
        {
            velocity.y = Mathf.Sqrt(impulseForce * -2f * gravity);
            velocity += -transform.forward * Mathf.Sqrt(impulseForce * -2f * gravity);
            IsWallJumping = true;
        }

        else if(Input.GetKeyDown(KeyCode.Space) && CollideRightwall())
        {
            velocity.y = Mathf.Sqrt(impulseForce * -2f * gravity);
            velocity += -transform.right * Mathf.Sqrt(impulseForce * -2f * gravity);
            IsWallJumping = true;
        }

        else if(Input.GetKeyDown(KeyCode.Space) && CollideLeftwall())
        {
            velocity.y = Mathf.Sqrt(impulseForce * -2f * gravity);
            velocity += transform.right * Mathf.Sqrt(impulseForce * -2f * gravity);
            IsWallJumping = true;
        }
        //--------------------------------------------------------------------
    }

    public void Addforce(Vector3 force)
    {
        velocity += force;
    }

    private void ResetLinearVelocity()
    {
        Debug.Log("Reseting LinearVelocity");
        if(isRayGrounded)
        {
            velocity = new Vector3(0,velocity.y,0);
            IsWallJumping = false;
        }
    }

    private IEnumerator Jump()
    {
        IsJumping = true;
        CanJump = false;
        jumpCounter -= 1;

        velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        yield return new WaitForSeconds(jumpRechargeTimer);

        IsJumping = false;
        CanJump = true;
    }

    private void jumpRecharge() //possible d'ajouter une recharge de tout les saut -> à voir
    {
        if(isRayGrounded)
        {
            jumpCounter = maxjump;
        }
        
    }

    private void Dash()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash && dashCount > 0)
        {
            StartCoroutine(Dash(move));
        }
    }

    private IEnumerator Dash(Vector3 direction)
    {
        canDash = false;
        isDashing = true;
        dashCount -= 1;
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

    private void DshRrgManager()
    {
        if(dashCount < maxDash && !recharging)
        {
            StartCoroutine(dashRecharge());
        }
    }

    IEnumerator dashRecharge()
    {
        recharging = true;
        while(dashCount < maxDash)
        {
            yield return new WaitForSeconds(rechargeTime);
            if(dashCount < maxDash)
            {
                dashCount ++;
            }
        }
        recharging = false;
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

        // if (PlayerFOVText != null)
        // {
        //     PlayerFOVText.text = "Actual FOV : " + playerCamera.fieldOfView;
        // }

        if(JumpText != null)
        {
            JumpText.text = "Jumps Left : " + jumpCounter + "/" + maxjump;
        }

        if(DashText != null)
        {
            DashText.text = "Dash Left : " + dashCount + "/" + maxDash;
        }

        if(PlayerHealthText != null)
        {
            PlayerHealthText.text =  hlth.playerHealth + "/" + hlth.maxHealth + " HP";
            if(hlth.IsTakingDamages)
            {
                PlayerHealthText.color = Color.red;
            }else if(hlth.IsBeingHealed)
            {
                PlayerHealthText.color = Color.green;
            }else if(!hlth.IsTakingDamages || hlth.IsBeingHealed) 
            {
                PlayerHealthText.color = Color.white;
            }
        } 
    }

    private void RaycastsDraw()
    {
        Debug.DrawRay(WallJumpOrigin.transform.position, -transform.up * controller.height * CharaControlHeightExtend, GroundedRay);
    }

    private bool CollideFrontwall()
    {
        Debug.DrawRay(WallJumpOrigin.transform.position, transform.forward * wallJumpDetectionRange, WallRayFront);
        return Physics.Raycast(WallJumpOrigin.transform.position, transform.forward, wallJumpDetectionRange, wallLayer);
    }

    private bool CollideBackwall()
    {
        Debug.DrawRay(WallJumpOrigin.transform.position, -transform.forward * wallJumpDetectionRange, WallRayBack);
        return Physics.Raycast(WallJumpOrigin.transform.position, -transform.forward, wallJumpDetectionRange, wallLayer);
    }

    private bool CollideRightwall()
    {
        Debug.DrawRay(WallJumpOrigin.transform.position, transform.right * wallJumpDetectionRange, WallRayRight);
        return Physics.Raycast(WallJumpOrigin.transform.position, transform.right, wallJumpDetectionRange, wallLayer);
    }

    private bool CollideLeftwall()
    {
        Debug.DrawRay(WallJumpOrigin.transform.position, -transform.right * wallJumpDetectionRange, WallRayLeft);
        return Physics.Raycast(WallJumpOrigin.transform.position, -transform.right, wallJumpDetectionRange, wallLayer);
    }

    private void textDebug()
    {
        if (isRayGrounded)
        {
            Debug.Log("Player Is Grounded by Raycast");
        }

        if (CollideFrontwall()){
            Debug.Log("Colliding Front Wall");
        }else if (CollideBackwall()){
            Debug.Log("Colliding Back Wall");
        }else if (CollideRightwall()){
            Debug.Log("Colliding Right Wall");
        }else if (CollideLeftwall()){
            Debug.Log("Colliding Left Wall");
        }
    }
}
