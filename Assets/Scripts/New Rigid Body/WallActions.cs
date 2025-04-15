using UnityEditor.UIElements;
using UnityEngine;

public class WallActions : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform orientation;

    public float minimumHeight;
    [SerializeField] float wallDistance;
    public float wallCheckRadius;
    [HideInInspector] public bool wallLeft, wallRight, wallFront, wallBack = false;
    [SerializeField] LayerMask wallTag;

    void Start()
    {

    }

    void Update()
    {
        checkWall();
    }

    bool canWallRun()
    {
        return !Physics.Raycast(transform.position, Vector3.down, minimumHeight );
    }

    void checkWall()
    {
        wallLeft = Physics.CheckSphere(transform.position - new Vector3(0.5f,0,0), wallCheckRadius, wallTag);
        wallRight = Physics.CheckSphere(transform.position - new Vector3(-0.5f,0,0), wallCheckRadius, wallTag);
    }
}
