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
        if(isGunTimerdone)
        {
            gunTimer += Time.deltaTime;

        }
        if(gunTimer >= 0.25f)
        {
            Movement player = GetComponent<Movement>();
            player.isRailing(false);
        }
       
       
        if (Input.GetMouseButtonDown(0))
        {
            weapon.Fire();
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            Railgun();
        }

        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void FixedUpdate()
    {
        Vector2 aimDir = mousePosition - rb.position;
        aimAngle = Mathf.Atan2(aimDir.y, aimDir.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = aimAngle;
        

    }
    public void Railgun()
    {
        isGunTimerdone = true;
        Movement player = GetComponent<Movement>();
        
        player.isRailing(true);


        Matrix4x4 recoilMatrix = new Matrix4x4();
        Vector4 recoil = new Vector4();
        float angle = Mathf.Deg2Rad * Random.Range(-5f, 5f);
        recoil.x = Mathf.Cos(angle);
        recoil.y = Mathf.Sin(angle);
        recoilMatrix[0, 0] = recoil.x;
        recoilMatrix[0, 1] = -recoil.y;
        recoilMatrix[1, 0] = recoil.y;
        recoilMatrix[1, 1] = recoil.x;

        Vector3 direction = recoilMatrix.MultiplyVector(firePoint.up);
        rb.AddForce(-direction * 10, ForceMode2D.Impulse);
        weapon.RailGunFire();
       
        
    }

    
}
