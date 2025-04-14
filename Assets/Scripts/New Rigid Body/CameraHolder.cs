using UnityEngine;

public class CameraHolder : MonoBehaviour
{
    public Transform CameraPosition;

    private void Update()
    {
        transform.position = Vector3.Slerp(transform.position, CameraPosition.position, Time.deltaTime * 100f);
    }
}
