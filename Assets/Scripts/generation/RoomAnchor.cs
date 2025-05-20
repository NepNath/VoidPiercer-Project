using UnityEngine;

public enum AnchorDirection
{
    North,
    South,
    East,
    West
}

public class RoomAnchor : MonoBehaviour
{
    public AnchorDirection direction;
    public CorridorAnchor linkedCorridorAnchor;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, 0.2f);

        Vector3 dir = direction switch
        {
            AnchorDirection.North => Vector3.forward,
            AnchorDirection.South => Vector3.back,
            AnchorDirection.East  => Vector3.right,
            AnchorDirection.West  => Vector3.left,
            _ => Vector3.forward
        };

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + dir * 0.5f);
    }
}
