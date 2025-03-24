using System.Collections;
using System.Net.Mail;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using UnityEditor.ShaderGraph.Drawing.Colors;

public class DashManager : MonoBehaviour
{
    [Header("Refecrences")]
    public Transform orientation;
    public Transform playerCam;
    private Rigidbody rb;
    private PlayerMovement pm; 

    [Header("Dashing")]
    public float dashForce;
    public float dashUpwardForce;
    public float dashDuration;

    [Header("Dash rechage")]
    public int DashCount;
    public float RechargeSeconds;

    public bool Recharging = false;

    [Header("Inputs")]
    public KeyCode dashKey = KeyCode.LeftShift;

    [Header("UI")]
    public TextMeshProUGUI dashText;




    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(dashKey) && DashCount > 0)
        {
            Dash();
        }

        if(DashCount < 3 && !Recharging)
        {
            StartCoroutine(DashRecharge());
        }
        UpdateUI();
       
    }

    private void Dash()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        Vector3 forceToApply = (orientation.forward * verticalInput + orientation.right * horizontalInput) * dashForce * 10f;
        DelayedForceToApply = forceToApply;
        Invoke(nameof(DelayedDashForce), 0.025f);
        DashCount -= 1;
        pm.dashing = true;
        Invoke(nameof(ResetDash), dashDuration);
    }
    
    IEnumerator DashRecharge()
    {
        Recharging = true;
        while(DashCount < 3)
        {
            yield return new WaitForSeconds(RechargeSeconds);
            if(DashCount < 3)
            {
                DashCount++;
            }
        }

        Recharging = false;
    }

    private Vector3 DelayedForceToApply;

    private void DelayedDashForce()
    {
        rb.AddForce(DelayedForceToApply, ForceMode.Impulse);
    }

    private void ResetDash()
    {
        pm.dashing = false;
    }

    private void UpdateUI()
    {
        if(dashText != null)
        {
            dashText.text = "Dash Left : " + DashCount;

            if(DashCount == 0)
            {
                dashText.color = Color.blue;
            } else 
            {
                dashText.color = Color.white;
            }
        }
    }

}
