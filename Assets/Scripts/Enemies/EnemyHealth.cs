using UnityEngine;

public class EnemyHealth : MonoBehaviour
{

    [SerializeField] float maxHealth = 100f;
    [SerializeField] float health;

    HealthBar healthBar;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        health = maxHealth;
        healthBar = GetComponentInChildren<HealthBar>();


    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.UpdateHealthBar(health, maxHealth);
    }
}
