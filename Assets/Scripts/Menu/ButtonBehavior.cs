using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.SceneManagement;
public class ButtonBehavior : MonoBehaviour
{

     [SerializeField] GameObject VersionInfoCanva;
     [SerializeField] GameObject MenuButton;
     [SerializeField] GameObject LoginCanva;
     [SerializeField] GameObject RegisterCanva;


     public void LoadTestMap()
     {
          UnityEngine.SceneManagement.SceneManager.LoadScene("TestMap");

     }

     public void QuitGame()
     {
          Application.Quit();
     }

     public void login()
     {

     }

     public void VersionInfo()
     {
          VersionInfoCanva.SetActive(true);
          MenuButton.SetActive(false);
     }

     public void CloseVersionInfo()
     {
          VersionInfoCanva.SetActive(false);
          MenuButton.SetActive(true);
     }

     public void ToggleLogin()
     {
          LoginCanva.SetActive(true);
          MenuButton.SetActive(false);
     }

     public void CloseLogin()
     {
          LoginCanva.SetActive(false);
          MenuButton.SetActive(true);
     }
     
     public void ToggleRegister()
     {
          RegisterCanva.SetActive(true);
          LoginCanva.SetActive(false);
          MenuButton.SetActive(false);
     }

     public void CloseRegister()
     {
          RegisterCanva.SetActive(false);
          LoginCanva.SetActive(true);
     }
}
