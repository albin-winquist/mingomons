using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Bullets weapon;
    public Rigidbody2D rb;
    GameObject player;
    public Transform firePoint;
    public Rigidbody2D parentRB;
    Vector2 mousePosition;
    public float aimAngle;

    bool isGunTimerdone;
    float gunTimer = 0;
    void Start()
    {
       
    }

    // Update is called once per frame
    public void Update()
    {

       
       
        if (Input.GetMouseButtonDown(0))
        {
            weapon.Fire();
        }
      

        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void FixedUpdate()
    {
        Vector2 aimDir = mousePosition - rb.position;
        aimAngle = Mathf.Atan2(aimDir.y, aimDir.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = aimAngle;
        

    }


    
}
