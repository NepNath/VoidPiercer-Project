using UnityEngine;
using UnityEngine.SceneManagement;
public class ButtonBehavior : MonoBehaviour
{

    [SerializeField] GameObject VersionInfoCanva;
    [SerializeField] GameObject ButtonManager;


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
        ButtonManager.SetActive(false);
    }

   public void CloseVersionInfo()
   {
        VersionInfoCanva.SetActive(false);
        ButtonManager.SetActive(true);
   }
}
