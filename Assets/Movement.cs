using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class Movement : MonoBehaviour
{
    const int MAX_POWER = 50;

    [SerializeField] float speed = 5;
    public Rigidbody2D rb;
    public Bullets weapon;
    public Transform firePoint;

    GameObject powerBar;

    [SerializeField] float dodgeSpeed = 10;
    [SerializeField] float dodgeTime = 0.5f;
    [SerializeField] float dodgeCd = 2;
    bool isRailGunning;
    private bool CanDodge = true;
    private bool IsDodging = false;
    
    private bool isCharging = false;
    private float chargePower = 2;

    private float chargeTimer = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        powerBar = GameObject.FindGameObjectWithTag("PowerBar");
    }
  
    
    void Update()
    {

        chargeTimer += Time.deltaTime;
        
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

     
        if (CanDodge && Input.GetKeyDown(KeyCode.LeftShift))
        {
            StartCoroutine(Dodge());
           
        }
        if(Input.GetKeyDown(KeyCode.X))
        {
            isCharging = true;
            
        }
        if(Input.GetKeyUp(KeyCode.X))
        {
            isCharging = false;
            if(chargePower > 2)
            {
                StartCoroutine(Railgun());
            }
        }

       

         
        
        Vector2 movement = new Vector2(moveX, moveY);
        if(!isRailGunning)
        {
            rb.velocity = movement * speed;
        }
        else
        {
        //rb.velocity = new Vector2(0, 0);
        }
        if (chargeTimer >= .01f)
        {

            if (isCharging)
            {
                chargePower = System.MathF.Pow(chargePower, 1.05f);
                if (chargePower > MAX_POWER)
                {
                    chargePower = MAX_POWER;
                }
            }
            else
            {
                chargePower = chargePower - 4;
                if (chargePower < 2)
                {
                    chargePower = 2;
                }

            }
            chargeTimer = 0;

        }

        powerBar.transform.localScale = new Vector3((chargePower - 2) / MAX_POWER * 2, powerBar.transform.localScale.y, powerBar.transform.localScale.z); 

        Debug.Log(chargePower + " " + isCharging);
    }
    IEnumerator Railgun()
    {

        isRailGunning = true;
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
        rb.AddForce(-direction * 10 * (chargePower - 2) / MAX_POWER, ForceMode2D.Impulse);

        weapon.RailGunFire();
        yield return new WaitForSeconds(0.5f);
        isRailGunning = false;

    }

    IEnumerator Dodge()
    {
        IsDodging = true;
        CanDodge = false;

        float dodgeTimer = 0f;

        while (dodgeTimer < dodgeTime)
        {
            Vector2 rollDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;
            transform.Translate(rollDirection * dodgeSpeed * Time.deltaTime);
            dodgeTimer += Time.deltaTime;
            yield return null;
        }

      IsDodging = false;

        yield return new WaitForSeconds(dodgeCd);
        CanDodge = true;
    }
}
