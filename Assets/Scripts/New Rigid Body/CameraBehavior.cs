using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

interface IInteractable
{
    public void Interact();
}

public class CameraBehavior : MonoBehaviour
{
    [Header("References")]
    [SerializeField] WallActions wall;

    [Header("CameraVariables")]
    [SerializeField] GameObject Menu;
    private bool isOpenMenu;
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
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Awake()
    {
        wall = GetComponent<WallActions>();
        PlayerCam.transform.localRotation = Quaternion.Euler(xRotation, yRotation, wall.tilt);
    }

    void Update()
    {
        if (!isOpenMenu)
        {
            CameraMovement();
        }
        OpenMenu();

        PlayerCam.transform.localRotation = Quaternion.Euler(xRotation, yRotation, wall.tilt);
        Orientation.transform.rotation = Quaternion.Euler(0, yRotation, 0);
      
        
   }

    void CameraMovement()
    {
        mouseX = Input.GetAxisRaw("Mouse X");
        mouseY = Input.GetAxisRaw("Mouse Y");

        yRotation  += mouseX * sensX * SensMult;
        xRotation -= mouseY * sensY * SensMult;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
    }

    void OpenMenu()
    {

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            isOpenMenu = !isOpenMenu;
        }

        if(isOpenMenu)
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            Menu.SetActive(true);

        }else{

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Menu.SetActive(false);
        }
    }
}
