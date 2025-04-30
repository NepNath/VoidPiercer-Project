using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class Mele : MonoBehaviour
{
    [SerializeField] float damage;
    [SerializeField] Transform playerLook;
    [SerializeField] float range;
    public KeyCode meleKey = KeyCode.F;


    void Update()
    {
        if(Input.GetKeyDown(meleKey))
        {
            Debug.Log("mele 👊🏻");
        }
        Ray meleRay = new Ray(playerLook.position, playerLook.forward);
        if (Physics.Raycast(meleRay, out RaycastHit RayHitInfo, range))
        {
            if(RayHitInfo.collider.gameObject.CompareTag("Enemy"))
            {
                Debug.Log("In Range of Enemy");
                EnemyHealth enemyHealth = RayHitInfo.collider.GetComponent<EnemyHealth>();
                if(Input.GetKeyDown(meleKey))
                {
                    Debug.Log("Trying to punch");
                    if(enemyHealth != null)
                    {
                        enemyHealth.takeDamage(damage);
                        Debug.Log("Inflicting mele Damages");
                    }
                    else
                    {
                        Debug.Log("Error while gathering enemy health script");
                    }

                }
            }
        }
    }
}
