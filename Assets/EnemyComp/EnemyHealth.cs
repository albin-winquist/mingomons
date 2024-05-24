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
    public float currHealth = 3;
    
    // Start is called before the first frame update
    void Start()
    {
       
        currHealth = UnityEngine.Random.Range(3, 6);

    }


    // Update is called once per frame
    void Update()
    {
        
       
       



    }

    public void takeDamage(float damageTaken)
    {
        currHealth -= damageTaken;
    }
}
