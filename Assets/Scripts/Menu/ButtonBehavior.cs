using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonBehavior : MonoBehaviour
{
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
}
