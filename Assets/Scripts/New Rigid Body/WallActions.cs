using JetBrains.Rider.Unity.Editor;
using UnityEditor.UIElements;
using UnityEngine;

public class WallActions : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform orientation;
    Rigidbody rb;

    [Header("WallRun")]
    [SerializeField] float wallRunGravity;
    public float minimumHeight;
    [SerializeField] float wallDistance;
    public float wallCheckRadius;
    [HideInInspector] public bool wallLeft, wallRight, wallFront, wallBack = false;
    [SerializeField] LayerMask wallTag;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        checkWall();

        if (canWallRun())
        {
            if(wallLeft)
            {
                Debug.Log("Wall Running Left");
            }
            else if(wallRight)
            {
                Debug.Log("Wall Running Right");
            }
        }
    }

    bool canWallRun()
    {
        return !Physics.Raycast(transform.position, Vector3.down, minimumHeight );
    }

    void checkWall()
    {
        wallLeft = Physics.CheckSphere(transform.position - orientation.right, wallCheckRadius, wallTag);
        wallRight = Physics.CheckSphere(transform.position + orientation.right, wallCheckRadius, wallTag);
    }

    void wallRun()
    {
        rb.useGravity = false;

        rb.AddForce(Vector3.down * wallRunGravity, ForceMode.Force);

        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(wallLeft)
            {
                
            }
            if(wallRight)
            {

            }

        }


    }
}
