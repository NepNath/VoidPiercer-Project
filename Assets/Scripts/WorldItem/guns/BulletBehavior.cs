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

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        {
            EnemyHealth enemyHealth = collision.gameObject.GetComponent<EnemyHealth>();
            if(enemyHealth != null)
            {
                enemyHealth.takeDamage(damage);
                Debug.Log($"Hit enemy for {damage} damage");
            }
        }
        Destroy(gameObject);
    }
}