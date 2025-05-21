using UnityEngine;
using UnityEngine.Rendering;

public class MenuCanvaController : MonoBehaviour
{
    
    [SerializeField] GameObject InfoVersionCanva;

    void Start()
    {
        InfoVersionCanva.SetActive(false);
    }
}
