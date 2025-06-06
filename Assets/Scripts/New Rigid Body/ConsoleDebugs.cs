
using UnityEngine;

public class ConsoleDebugs : MonoBehaviour
{
    private Movement move;
    private WallActions wall;
    public Transform orientation;

    [SerializeField] bool showConsoleInfo;
    [SerializeField] bool showRaycasts;
    [SerializeField] bool showGizmos;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {  
        move = GetComponent<Movement>();
        wall = GetComponent<WallActions>();
    }

    // Update is called once per frame
    void Update()
    {
        if(showConsoleInfo)
        {
            ConsoleInfos();
        }
    }

    private void OnDrawGizmos()
    {
        if(showGizmos)
        {
            Gizmos.DrawSphere(transform.position - new Vector3(0,0.75f, 0), move.JumpGizmoRad);
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(transform.position - orientation.right * 0.5f, wall.wallCheckRadius);
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position + orientation.right * 0.5f, wall.wallCheckRadius);
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(transform.position + orientation.forward * 0.5f, wall.wallCheckRadius);
            Gizmos.DrawSphere(transform.position - orientation.forward * 0.5f, wall.wallCheckRadius);
        }
    }

    private void ConsoleInfos()
    {
        if(wall.wallLeft)
        {
            Debug.Log("Colliding wall on left");
        }
        if(wall.wallRight)
        {
            Debug.Log("Colliding wall on right");
        }
        if(wall.wallFront)
        {
            Debug.Log("Colliding wall on Front");
        }
        if(wall.wallBack)
        {
            Debug.Log("Colliding wall on Back");
        }

        //slope raycast
        Debug.DrawRay(transform.position, Vector3.down * move.GroundRayLength, Color.cyan);

        if(move.IsGrounded() && !move.OnSlope())
        {
            Debug.Log("Is Grounded");
        }
        if(move.IsGrounded() && move.OnSlope())
        {
            Debug.Log("On Slope");
        }
    }
}
