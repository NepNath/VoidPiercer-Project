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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        move = GetComponent<Movement>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            rb.AddForce(move.moveDirection * DashForce, ForceMode.Impulse);
        }
    }
}
