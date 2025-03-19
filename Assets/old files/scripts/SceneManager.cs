using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneManager : MonoBehaviour
{
    void Update()
    {
       if(Input.GetKeyDown("k")){
            UnityEngine.SceneManagement.SceneManager.LoadScene("Main");
        }
        
    }
    
}
