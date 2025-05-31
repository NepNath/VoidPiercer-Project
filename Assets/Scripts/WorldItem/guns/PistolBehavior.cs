
using UnityEngine;
using System.Collections.Generic;

public class PistolBehavior : MonoBehaviour
{
    [Header("Input Settings")]
    [SerializeField] Transform Camera;
    [SerializeField] private KeyCode fireKey = KeyCode.Mouse0; // Permet de modifier la touche dans l'inspector

    [Header("Bullet Settings")]
    [SerializeField] float pistolDamage;
    [SerializeField] float bulletForce;
    [SerializeField] private GameObject bulletPrefab; // Renommé pour plus de clarté
    [SerializeField] private Transform spawnPoint; // Point de spawn des balles

    [Header("Impacts")]
    public GameObject NormalImpact;
    public GameObject EnemyImpact;

    [Header("Gun Settings")] // Optionnel : ajoutez des paramètres supplémentaires
    [SerializeField] private float fireRate = 0.5f;
    private float nextTimeToFire = 0f;
    [Header("Player Effects On-Hit")]
    public PlayerEffects playerEffects;
    private List<StatusEffect> effects;

    void Start()
    {
        effects = playerEffects.getActiveEffects();
    }

    private void Update()
    {
        HandleShooting();
        effects = playerEffects.getActiveEffects();
    }

    private void HandleShooting()
    {
        if (Input.GetKey(fireKey) && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + fireRate;
            Fire();
        }
    }

    private void Fire()
    {
        
        Ray aimDirection = new Ray(Camera.position, Camera.forward);

        if(Physics.Raycast(aimDirection, out RaycastHit aimHitDirection))
        {
        if(aimHitDirection.collider.CompareTag("Enemy"))
        {
            EnemyHealth enemyHealth = aimHitDirection.collider.GetComponent<EnemyHealth>();
            enemyHealth.takeDamage(pistolDamage);
            foreach (var effect in effects)
            {
                    effect.apply(aimHitDirection.collider.gameObject);
            }
            Debug.DrawRay(Camera.position, Camera.forward, Color.red);
            Instantiate(EnemyImpact, aimHitDirection.point, Quaternion.LookRotation(aimHitDirection.normal));

        }
        else
        {
            Debug.Log(aimHitDirection.transform.name);
            Instantiate(NormalImpact, aimHitDirection.point, Quaternion.LookRotation(aimHitDirection.normal));
        }
        }
    }

}