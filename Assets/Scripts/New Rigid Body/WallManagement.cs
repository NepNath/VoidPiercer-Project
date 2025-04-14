using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class WallManagement : MonoBehaviour
{
    [SerializeField] Transform orientation;
    [SerializeField] private float wallDistance = 0.5f;


    bool WallFront;
    bool WallBack;
    bool WallLeft;
    bool WallRight;

    RaycastHit FrontWallHit;
    RaycastHit BackWallHit;
    RaycastHit LeftWallHit;
    RaycastHit RightWallHit;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        drawWalls();
        WallDetection();
    }

    void WallDetection()
    {
        WallFront = Physics.Raycast(transform.position, orientation.forward, out FrontWallHit, wallDistance);
        WallBack = Physics.Raycast(transform.position, -orientation.forward, out BackWallHit, wallDistance);
        WallLeft = Physics.Raycast(transform.position, -orientation.right, out LeftWallHit, wallDistance);
        WallRight = Physics.Raycast(transform.position, orientation.right, out RightWallHit, wallDistance);
    }

    void drawWalls()
    {
        Debug.DrawRay(transform.position, orientation.forward * wallDistance, Color.blue);
        Debug.DrawRay(transform.position, -orientation.forward * wallDistance, Color.cyan);
        Debug.DrawRay(transform.position, -orientation.right * wallDistance, Color.red);
        Debug.DrawRay(transform.position, orientation.right * wallDistance, Color.yellow);
    }
}
