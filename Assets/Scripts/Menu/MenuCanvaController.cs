using UnityEngine;
using UnityEngine.Rendering;

public class MenuCanvaController : MonoBehaviour
{
    
    [SerializeField] GameObject InfoVersionCanva;
    [SerializeField] GameObject LoginCanva;
    [SerializeField] GameObject RegisterCanva;
    void Awake()
    {
        InfoVersionCanva.SetActive(false);
        LoginCanva.SetActive(false);
        RegisterCanva.SetActive(false);
    }
}
