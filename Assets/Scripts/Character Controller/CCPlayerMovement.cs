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
   
    Vector3 move;

    [Header("References")]
    public Camera playerCamera;
    PlayerMovement pm;
    PlayerHealthManager hlth;

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
        //--------------------------------------------------------------------

        if(Input.GetKeyDown(KeyCode.Keypad5))
        {
           velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
           velocity.z = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        if(velocity.z > 0)
        {
            velocity.z += gravity * Time.deltaTime; //je dois ajouter une force de pression pour stabiliser le joueur
        }
       
    }


    private void PlayerMovement()
    {
        float speed = walkSpeed;

        if (isRayGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        
        if (!isDashing)
        {
            controller.Move(move.normalized * speed * Time.deltaTime);
        }

        if (Input.GetKeyDown(KeyCode.Space) && jumpCounter > 0 && CanJump)
        {
            StartCoroutine(Jump());

        }
    }

    public void Addforce(Vector3 force)
    {
        velocity += force;
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
        Debug.DrawRay(transform.position, transform.right * wallJumpDetectionRange, WallRay);
        Debug.DrawRay(transform.position, -transform.right * wallJumpDetectionRange, WallRay);
        Debug.DrawRay(transform.position, transform.forward * wallJumpDetectionRange, WallRay);
        Debug.DrawRay(transform.position, -transform.forward * wallJumpDetectionRange, WallRay);
        Debug.DrawRay(transform.position, -transform.up * controller.height * CharaControlHeightExtend, GroundedRay);
    }

    private bool WallRayCasts()
    {
        return Physics.Raycast(transform.position, transform.right, wallJumpDetectionRange, wallLayer) ||
        Physics.Raycast(transform.position, -transform.right, wallJumpDetectionRange, wallLayer) ||
        Physics.Raycast(transform.position, transform.forward, wallJumpDetectionRange, wallLayer) ||
        Physics.Raycast(transform.position, -transform.forward, wallJumpDetectionRange, wallLayer);
    }   
    private void textDebug()
    {
        if(isRayGrounded)
        {
            Debug.Log("Player Is Grounded by Raycast");
        }

        if (WallRayCasts())
        {
            Debug.Log("Colliding Wall");
        }
    }
}
