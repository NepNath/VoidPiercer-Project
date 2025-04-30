using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] float maxHealth = 100f;
    [SerializeField] float currentHealth;
    [SerializeField] HealthBar healthBar; // Ajout de SerializeField

    void Awake()
    {
        currentHealth = maxHealth;
        if (healthBar == null)
        {
            healthBar = GetComponentInChildren<HealthBar>();
            if (healthBar == null)
            {
                Debug.LogWarning("HealthBar component not found on " + gameObject.name);
            }
        }
    }

    void Update()
    {
        if(currentHealth < 0)
        {
            currentHealth = 0f;
        }

        if(currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }


    public void takeDamage(float damage)
    {
        currentHealth -= damage;
        if (healthBar != null) // Vérification de null avant d'utiliser healthBar
        {
            healthBar.UpdateHealthBar(currentHealth / maxHealth);
        }
        Debug.Log($"Enemy took {damage} damage. Current health: {currentHealth}");
    }
}