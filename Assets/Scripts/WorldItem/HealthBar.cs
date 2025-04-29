using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
  
    [SerializeField] Slider slider;

    public void UpdateHealthBar(float currentValue, float maxValue)
    {
        slider.value = currentValue / maxValue;
    }

    void Update()
    {
        
    }
}
