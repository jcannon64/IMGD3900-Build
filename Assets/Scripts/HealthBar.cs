using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;

    public void UpdateHealthBar(float currValue, float maxValue)
    {
        slider.value = currValue / maxValue;
    }


    
}
