using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalButtonBehabior : MonoBehaviour
{
   public void ReturnToMenu()
   {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
   }
}
