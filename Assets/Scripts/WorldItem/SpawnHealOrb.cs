using UnityEngine;

public class SpawnHealOrb : MonoBehaviour, IInteractable
{
    public GameObject healOrbPrefab;
    public Transform spawnPoint;

    public void Interact()
    {
        Debug.Log("Spawning HealthOrb");
        Instantiate(healOrbPrefab, spawnPoint.position, spawnPoint.rotation);
    }
}

