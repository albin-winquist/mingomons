using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Bullets weapon;
    public Rigidbody2D rb;
   
    public Transform firePoint;
    public Rigidbody2D parentRB;
    Vector2 mousePosition;
    public float aimAngle;
    [SerializeField] public int shotInMag = 12;
    float reloadTime = 2;
    bool isReloading = false;
    float shootCd = 0.5f;
    float CurrentCd = 0f;
    bool CanFire = true; 
    
    void Start()
    {
        shotInMag = 12;
    }

    // Update is called once per frame
    public void Update()
    {

       
       
        if (Input.GetMouseButtonDown(0)&& shotInMag >=1&& CanFire)
        {
            weapon.Fire(); 
            shotInMag--;
            CurrentCd = 0;
            CanFire = false;
        }

        if (!CanFire)
        {
            CurrentCd += Time.deltaTime;
            if (CurrentCd >= shootCd)
            {
                CanFire = true;
            }
        }
        
        if (shotInMag <=0)
        {
            //press r to reload
        }

        if (Input.GetKeyDown(KeyCode.R) && shotInMag<12 && !isReloading)
        {
            StartCoroutine(reload());
           
        } 
        


        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
    
    IEnumerator reload()
    {
        isReloading = true;
        yield return new WaitForSeconds(reloadTime);

        shotInMag = 12;
        isReloading = false;
    }

    private void FixedUpdate()
    {
        Vector2 aimDir = mousePosition - rb.position;
        aimAngle = Mathf.Atan2(aimDir.y, aimDir.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = aimAngle;
        

    }


    
}
