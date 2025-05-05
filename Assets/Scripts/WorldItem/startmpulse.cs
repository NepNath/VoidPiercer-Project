using UnityEngine;

public class startmpulse : MonoBehaviour
{
    [SerializeField] float force;
    Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Start()
    {
        rb.AddForce(Vector3.up * force, ForceMode.Impulse);
    }

}
