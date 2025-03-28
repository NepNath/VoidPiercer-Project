using UnityEngine;

public class WJOrigin : MonoBehaviour
{
    CCPlayerMovement CC;
    public GameObject Origin;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CC = GetComponent<CCPlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        CC.WallJumpOrigin = Origin;
    }
}
