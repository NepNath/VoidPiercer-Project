using System.Collections;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public class Dash : MonoBehaviour
{

    [Header("References")]
    public Movement Movement;
    public Rigidbody Rigidbody;
    public float DashForce;
    Vector3 PlayerInput;

    [Header("dash")]
    public float DashCooldown;
    public float dashDuration;
    [SerializeField] private bool canDash;
    private bool isDashing;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Movement = GetComponent<Movement>();
        Rigidbody = GetComponent<Rigidbody>();

        isDashing = false;
        canDash = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        PlayerInput = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical")).normalized;

        if(Input.GetKeyDown(KeyCode.LeftShift) && canDash && !isDashing)
        {
            DashMTHD();
        }
    }

    private void DashMTHD()
    {
        Rigidbody.AddForce(PlayerInput  * DashForce, ForceMode.Impulse);
        StartCoroutine(DashCoroutine());
    }

    
     IEnumerator DashCoroutine()
    {
        isDashing = true;
        canDash = false;
        float startTime = Time.time;

        while(Time.time < startTime + dashDuration)
        {
            Rigidbody.AddForce(PlayerInput * DashForce * Time.deltaTime, ForceMode.Force);
            yield return new WaitForSeconds(DashCooldown);
        }
        isDashing = false;
        canDash = true;

    }
   
}
