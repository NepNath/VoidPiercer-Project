using UnityEngine;

public class MeshRotation : MonoBehaviour
{
    public GameObject orientation;

    void Update()
    {
        transform.rotation = orientation.transform.rotation;
    }
}
