using UnityEngine;

public class Movement : MonoBehaviour
{
    private Vector3 PlayerInput;

    public float speed;
    public float maxSpeed;
    public float JumpForce;
    public Rigidbody Rigidbody;
    public float GroundRayLength;

    private void Update()
    {
        PlayerInput = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        PlayerMove();

        Debug.DrawRay(transform.position, Vector3.down * GroundRayLength, Color.green);

        if(IsRayGrounded())
        {
            Debug.Log("Is Ray grounded");
        }
    }

    private void PlayerMove()
    {
        Vector3 MoveVector = transform.TransformDirection(PlayerInput) * speed;
        Vector3 clampedMoveVector = Vector3.ClampMagnitude(MoveVector, maxSpeed);
        Rigidbody.linearVelocity = new Vector3(clampedMoveVector.x, Rigidbody.linearVelocity.y, clampedMoveVector.z);

        if(Input.GetKeyDown(KeyCode.Space) && IsRayGrounded())
        {
            Rigidbody.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
        }
    }

    private bool IsRayGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, GroundRayLength);
    }

    

}