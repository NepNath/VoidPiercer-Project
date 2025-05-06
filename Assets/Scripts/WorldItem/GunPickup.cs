using UnityEngine;
using UnityEngine.UI;

public class GunPickup : MonoBehaviour
{
    [SerializeField] GameObject gunHolder;

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            gunHolder.SetActive(true);
            Destroy(gameObject);
        }
    }


}
