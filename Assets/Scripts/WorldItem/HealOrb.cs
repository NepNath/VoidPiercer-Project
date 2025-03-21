using UnityEngine;

public class HealOrb : MonoBehaviour
{
    [Header("Variables")]
    public float HealAmount;

    private PlayerHealthManager phm;

    void Start()
    {
        phm = FindFirstObjectByType<PlayerHealthManager>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            phm.playerHealth += HealAmount;
            Destroy(gameObject);
        }
    }
}
