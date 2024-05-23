using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;


public class EnemyHealth : MonoBehaviour
{
    
    public float maxHealth = 3;
    public float currHealth;
    
    // Start is called before the first frame update
    void Start()
    {
       
        currHealth = UnityEngine.Random.Range(3, 8);

      
      

        
        
        
    }


    // Update is called once per frame
    void Update()
    {
        
        if (currHealth <= 0)
        {
            Destroy(gameObject);
        }
       



    }

    public void takeDamage(float damageTaken)
    {
        currHealth -= damageTaken;
    }
}
