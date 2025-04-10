using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Dash : MonoBehaviour
{

    public Movement Movement;
    public Rigidbody Rigidbody;
    public float DashForce;
    Vector3 PlayerInput;
    public float DashCooldown;
    private bool canDash;

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
            StartCoroutine(dash());
        }
        resetDash();
    }


    IEnumerator dash()
    {
        Rigidbody.AddForce(PlayerInput  * DashForce, ForceMode.Impulse);
        yield return new WaitForSeconds(DashCooldown);
    }

    void resetDash()
    {
        canDash = true;
    }
}
