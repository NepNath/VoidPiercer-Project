using Unity.VisualScripting;
using UnityEngine;

interface IInteractable
{
    public void Interact();
}

public class CameraBehavior : MonoBehaviour
{
    [Header("CameraVariables")]
    public Transform PlayerCamera;
    public Vector2 Sensitivity;
    

    private Vector2 XYRotation;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
    }

    void Update()
    {  
        Vector2 MouseInput = new Vector2
        {
            x = Input.GetAxis("Mouse X"),
            y = Input.GetAxis("Mouse Y")
        };

        XYRotation.x -= MouseInput.y * Sensitivity.y;
        XYRotation.y += MouseInput.x * Sensitivity.x;

        XYRotation.x = Mathf.Clamp(XYRotation.x , -90f, 90f);

        transform.eulerAngles = new Vector3(0f, XYRotation.y, 0f);
        PlayerCamera.localEulerAngles = new Vector3(XYRotation.x, 0f, 0f);
    }

}
