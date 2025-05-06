using UnityEngine;

public class MenuCamera : MonoBehaviour
{
    
    [Header("CameraVariables")]
    public Transform PlayerCam;
    public Transform Orientation;
    private float mouseY;
    private float mouseX;
    private float xRotation;
    private float yRotation;
    public float sensX;
    public float sensY;
    public float SensMult;

    void Start()
    {

        
    }

    void Update()
   {
        CameraMovement();

        PlayerCam.transform.localRotation = Quaternion.Euler(xRotation,yRotation,0);
        Orientation.transform.rotation = Quaternion.Euler(0,yRotation,0);
   }

    void CameraMovement()
    {
        mouseX = Input.GetAxisRaw("Mouse X");
        mouseY = Input.GetAxisRaw("Mouse Y");

        yRotation  += mouseX * sensX * SensMult;
        xRotation -= mouseY * sensY * SensMult;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
    }

}


