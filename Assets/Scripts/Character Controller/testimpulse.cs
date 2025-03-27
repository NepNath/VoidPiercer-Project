using UnityEngine;

public class testimpulse : MonoBehaviour
{

    CCPlayerMovement player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GetComponent<CCPlayerMovement>();
    }

    private void knockBack()
    {
        player.Addforce(Vector3.up * 5f);
        player.Addforce(-transform.forward * 3f);
    }
}
