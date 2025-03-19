using UnityEngine;

public class CameraPosition : MonoBehaviour
{
    public Transform cameraPositon;

    void Update()
    {
        transform.position = cameraPositon.position;
    }
}
