using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HealthBar;

public class StaminaBar : MonoBehaviour
{

    public float Stamina;
    public float MaxStamina;
    public Image staminaBar;
    public Slider staminaSlider;
    
    // Start is called before the first frame update
    void Start()
    {
        Stamina = MaxStamina;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (staminaSlider.value != Stamina)
        {
            staminaSlider.value = Stamina;
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            SpendStamina(25);
           
        }
    }

    public void SpendStamina(float Spent)
    {
        Stamina -= Spent;
        staminaBar.fillAmount = Stamina / 100f;
    }
}
