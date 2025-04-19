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
    [SerializeField] int dashCount;
    public int maxDash;
    [SerializeField] float DashReSec;
    [SerializeField] bool Recharging;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        move = GetComponent<Movement>();
        rb = GetComponent<Rigidbody>();
        canDash = true;
        dashCount = maxDash;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftShift) && !IsDashing && dashCount > 0)
        {
            dashCount -= 1;
            rb.AddForce(move.moveDirection * DashForce, ForceMode.Impulse);
            StartCoroutine(DashReset());
        }

         if(dashCount < 3 && !Recharging)
        {
            StartCoroutine(dashRecharge());
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

    IEnumerator dashRecharge()
    {
        Recharging = true;
        while(dashCount < 3)
        {
            yield return new WaitForSeconds(DashReSec);
            if(dashCount < 3)
            {
                dashCount++;
            }
        }

        Recharging = false;
    }

}
