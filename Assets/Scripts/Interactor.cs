using TMPro.Examples;
using UnityEngine;



public class Interactor : MonoBehaviour
{
   public Transform Source;
   public float Range;
   public KeyCode InteractKey = KeyCode.E;  


    private void Start()
    {
    
    }


    private void Update()
    {
        Debug.DrawRay(Source.position, Source.forward * Range, Color.magenta);  

        if(Input.GetKeyDown(InteractKey))
        {
            Ray InteractRay = new Ray(Source.position, Source.forward);
            if (Physics.Raycast(InteractRay, out RaycastHit RayHitInfo, Range))
            {
                if(RayHitInfo.collider.gameObject.TryGetComponent(out IInteractable ObjectInteract))
                {
                    ObjectInteract.Interact();
                }
            }
        }
    }

}
 
