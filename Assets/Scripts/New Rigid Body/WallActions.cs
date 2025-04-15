using UnityEngine;

public class WallActions : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform orientation;
    Rigidbody rb;
    Movement move;

    RaycastHit LeftWallHit;
    RaycastHit RightWallHit;
    RaycastHit FrontWallHit;
    RaycastHit BackWallHit;
    

    [Header("WallRun & Jump")]
    public bool isWallRunning;
    public bool isClimbing;
    [SerializeField] float wallRunGravity;
    public float minimumHeight;
    public float wallJumpForce;
    [SerializeField] float wallDistance;
    public float wallCheckRadius;

    [HideInInspector] public bool wallLeft, wallRight, wallFront, wallBack = false;
    [SerializeField] LayerMask wallTag;

    void Start()
    {
        move = GetComponent<Movement>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        checkWall();
        wallGrip();

        if (canWallRun())
        {
            if(wallLeft)
            {
                wallRun();
                Debug.Log("Wall Running Left");
            }
            else if(wallRight)
            {
                wallRun();
                Debug.Log("Wall Running Right");
            }
            else
            {
                stopWallRun();
            }
        }
    }

    public bool canWallRun()
    {
        return !Physics.Raycast(transform.position, Vector3.down, minimumHeight );
    }

    void checkWall()
    {
        wallLeft = Physics.SphereCast(transform.position, wallCheckRadius, -orientation.right, out LeftWallHit, wallDistance, wallTag);
        wallRight = Physics.SphereCast(transform.position,  wallCheckRadius, orientation.right, out RightWallHit, wallDistance, wallTag);
        wallFront = Physics.SphereCast(transform.position,  wallCheckRadius, orientation.forward, out FrontWallHit, wallDistance, wallTag);
        wallBack = Physics.SphereCast(transform.position,  wallCheckRadius, -orientation.forward, out BackWallHit, wallDistance, wallTag);
    }

    void wallGrip()
    {
        if(wallFront || wallBack)
        {
            isClimbing = true;
        }
        else
        {
            isClimbing = false;
        }
    }

    void wallRun()
    {
        rb.useGravity = false;
        isWallRunning = true;

        rb.AddForce(Vector3.down * wallRunGravity, ForceMode.Force);

        if(Input.GetKeyDown(KeyCode.Space))
        {

            if(wallLeft)
            {
                Vector3 wallRunDirection = transform.up + LeftWallHit.normal;
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
                rb.AddForce(wallRunDirection * wallJumpForce, ForceMode.Impulse);   
            }

            if(wallRight)
            {
                Vector3 wallRunDirection = transform.up + RightWallHit.normal;
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
                rb.AddForce(wallRunDirection * wallJumpForce, ForceMode.Impulse);   
            }
            if(wallFront)
            {
                Debug.Log("front jump");
                Vector3 wallRunDirection = transform.up + FrontWallHit.normal;
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
                rb.AddForce(wallRunDirection * wallJumpForce, ForceMode.Impulse);   
            }
            if(wallBack)
            {
                Debug.Log("back jump");
                Vector3 wallRunDirection = transform.up + BackWallHit.normal;
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
                rb.AddForce(wallRunDirection * wallJumpForce, ForceMode.Impulse);   
            }

        }
    }

    void stopWallRun()
    {
        isWallRunning = false;
        rb.useGravity = true;
    }
}