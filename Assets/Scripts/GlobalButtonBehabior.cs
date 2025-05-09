using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalButtonBehabior : MonoBehaviour
{
   public void ReturnToMenu()
   {
      UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
   }

   public void reloadScene()
   {
      string currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
      UnityEngine.SceneManagement.SceneManager.LoadScene(currentScene);
   }
}
