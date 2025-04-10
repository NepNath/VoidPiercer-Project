using UnityEngine;

public class Movement : MonoBehaviour
{
    private Vector3 PlayerInput;

    public float speed;
    public float JumpForce;
    public Rigidbody Rigidbody;

    private void Update()
    {
        PlayerInput = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        PlayerMove();
    }

    private void PlayerMove()
    {
        Vector3 MoveVector = transform.TransformDirection(PlayerInput) * speed;
        Vector3 clampedMoveVector = Vector3.ClampMagnitude(MoveVector, speed);
        Rigidbody.linearVelocity = new Vector3(clampedMoveVector.x, Rigidbody.linearVelocity.y, clampedMoveVector.z);

        if(Input.GetKeyDown(KeyCode.Space))
        {
            Rigidbody.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
        }
    }

}