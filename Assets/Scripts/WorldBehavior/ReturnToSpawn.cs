using UnityEngine;

public class ReturnToSpawn : MonoBehaviour
{

    [SerializeField] GameObject SpawnPoint;
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("TestMap");
        }
    }
}
