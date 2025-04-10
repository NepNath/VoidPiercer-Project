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
    private bool canDash;
    private bool isDashing;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Movement = GetComponent<Movement>();
    }

    // Update is called once per frame
    void Update()
    {
        
        PlayerInput = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));

        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            StartCoroutine(dashForce());
        }
        resetDash();
    }


    IEnumerator dashForce()
    {
        Rigidbody.AddForce(PlayerInput  * DashForce, ForceMode.Impulse);
        yield return new WaitForSeconds(DashCooldown);
    }

    IEnumerator dashTime()
    {
        canDash = false;
        isDashing = true;
        float Timer = Time.time;

        while (Time.time < Timer + dashDuration)
        {
            Rigidbody.linearVelocity = new Vector3()
        }

    }

    void resetDash()
    {
        canDash = true;
        isDashing = false;
    }
}
