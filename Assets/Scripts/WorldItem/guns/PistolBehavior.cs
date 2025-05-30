
using System.Collections;
using UnityEngine;

public class PistolBehavior : MonoBehaviour
{
    [Header("Input Settings")]
    [SerializeField] Transform Camera;
    [SerializeField] private KeyCode fireKey = KeyCode.Mouse0; 

    [Header("Bullet Settings")]
    [SerializeField] float pistolDamage;
    [SerializeField] float bulletForce;
    [SerializeField] private GameObject bulletPrefab; 
    [SerializeField] private Transform spawnPoint; 

    [Header("Impacts & effects")]
    public GameObject NormalImpact;
    public GameObject EnemyImpact;
    [SerializeField] private GameObject BulletTrailPrefab;
    [SerializeField] private float BulletSpeed;

    [Header("Gun Settings")] 
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

        Ray aimDirection = new Ray(Camera.position, Camera.forward);

        if (Physics.Raycast(aimDirection, out RaycastHit aimHitDirection))
        {
            StartCoroutine(SpawnBulletTrail(spawnPoint.position, aimHitDirection));
            if (aimHitDirection.collider.CompareTag("Enemy"))
            {
                EnemyHealth enemyHealth = aimHitDirection.collider.GetComponent<EnemyHealth>();
                enemyHealth.takeDamage(pistolDamage);
                Debug.DrawRay(Camera.position, Camera.forward, Color.red);
                Instantiate(EnemyImpact, aimHitDirection.point, Quaternion.LookRotation(aimHitDirection.normal));

            }
            else
            {
                Debug.Log(aimHitDirection.transform.name);
                Instantiate(NormalImpact, aimHitDirection.point, Quaternion.LookRotation(aimHitDirection.normal));
            }
        }

        IEnumerator SpawnBulletTrail(Vector3 startPoint, RaycastHit hit)
        {
            Vector3 endPoint = hit.point;
            GameObject trail = Instantiate(BulletTrailPrefab, startPoint, Quaternion.identity);
            float Distance = Vector3.Distance(startPoint, endPoint);
            float time = 0f;
            float Duration = Distance / BulletSpeed;

            while (time < Duration)
            {
                time += Time.deltaTime;
                trail.transform.position = Vector3.Lerp(startPoint, endPoint, time / Duration);
                yield return null;
            }

            trail.transform.position = endPoint;
            Destroy(trail, 0.5f);
        }
    }

}