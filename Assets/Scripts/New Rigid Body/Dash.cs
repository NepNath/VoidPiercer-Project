using System.Collections;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public class Dash : MonoBehaviour
{

    [Header("References")]
    public Movement move;
    public Rigidbody rb;
    public float DashForce;
    

    [Header("Variables")]
    public bool IsDashing;
    public bool canDash;
    [SerializeField] float dashCooldown;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        move = GetComponent<Movement>();
        rb = GetComponent<Rigidbody>();
        canDash = true;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftShift) && !IsDashing)
        {
            rb.AddForce(move.moveDirection * DashForce, ForceMode.Impulse);
            StartCoroutine(DashReset());
        }
    }

    IEnumerator DashReset()
    {
        IsDashing = true;
        canDash = false;

        yield return new WaitForSeconds(dashCooldown);

        IsDashing = false;
        canDash = true;
    }
}
