using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    [SerializeField] private float damage = 10f;
    [SerializeField] private float lifeTime = 5f;
    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        Destroy(gameObject, lifeTime); // Détruit la balle après un certain temps
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
            if(enemyHealth != null)
            {
                enemyHealth.takeDamage(damage);
                Debug.Log($"Hit enemy for {damage} damage");
            }
        }
        Destroy(gameObject);
    }
}