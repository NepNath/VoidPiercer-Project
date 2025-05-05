using UnityEngine;

public class PistolBehavior : MonoBehaviour
{
    [Header("Input Settings")]
    [SerializeField] private KeyCode fireKey = KeyCode.Mouse0; // Permet de modifier la touche dans l'inspector

    [Header("Bullet Settings")]

    [SerializeField] float bulletDamages;
    [SerializeField] private Camera playerCamera;
    [SerializeField] float bulletForce;
    [SerializeField] private GameObject bulletPrefab; // Renommé pour plus de clarté
    [SerializeField] private Transform spawnPoint; // Point de spawn des balles

    [Header("Gun Settings")] // Optionnel : ajoutez des paramètres supplémentaires
    [SerializeField] private float fireRate = 0.5f;
    private float nextTimeToFire = 0f;

    private void Update()
    {
        HandleShooting();
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
        // Utilise le spawnPoint si disponible, sinon utilise la position de l'arme
        Vector3 spawnPos = spawnPoint != null ? spawnPoint.position : transform.position;
        
        GameObject bullet = Instantiate(bulletPrefab, spawnPos, transform.rotation);
         Quaternion spawnRotation = playerCamera != null ? 
            playerCamera.transform.rotation : 
            transform.rotation;
        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
        if (bulletRb != null)
        {
            bulletRb.AddForce(spawnRotation * Vector3.forward * bulletForce, ForceMode.Impulse);
        }
        
        // Optionnel : Ajouter des effets sonores ou visuels
        // PlayShootingEffect();
    }
}