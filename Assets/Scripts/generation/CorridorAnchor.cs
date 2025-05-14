using UnityEngine;

public enum CorridorDirection
{
    North,
    South,
    East,
    West
}

public class CorridorAnchor : MonoBehaviour
{
    public CorridorDirection direction;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(transform.position, 0.15f);

        Vector3 dir = direction switch
        {
            CorridorDirection.North => Vector3.forward,
            CorridorDirection.South => Vector3.back,
            CorridorDirection.East => Vector3.right,
            CorridorDirection.West => Vector3.left,
            _ => Vector3.forward
        };
        Gizmos.DrawLine(transform.position, transform.position + dir * 0.5f);
    }
}
