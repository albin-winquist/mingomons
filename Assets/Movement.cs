using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] float speed = 5;
    public Rigidbody2D rb;
   
    [SerializeField] float dodgeSpeed = 10;
    [SerializeField] float dodgeTime = 0.5f;
    [SerializeField] float dodgeCd = 2;
    public bool isRailGunning;
    private bool CanDodge = true;
    private bool IsDodging = false; 
 

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
  
    
    void Update()
    {
       


        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

     
        if (CanDodge && Input.GetKeyDown(KeyCode.LeftShift))
        {
            StartCoroutine(Dodge());
           
        }


        
        Vector2 movement = new Vector2(moveX, moveY);
        if(!isRailGunning)
        {
            rb.velocity = movement * speed;
        }
        else
        {
            rb.velocity = new Vector2(0, 0);
        }


    }
    public void  isRailing(bool isTrue)
    {
        isRailGunning = isTrue;
    }
   
        IEnumerator Dodge()
    {
        IsDodging = true;
        CanDodge = false;

        float dodgeTimer = 0f;

        while (dodgeTimer < dodgeTime)
        {
            Vector3 rollDirection = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0f).normalized;
            transform.Translate(rollDirection * dodgeSpeed * Time.deltaTime);
            dodgeTimer += Time.deltaTime;
            yield return null;
        }

      IsDodging = false;

        yield return new WaitForSeconds(dodgeCd);
        CanDodge = true;
    }
}
