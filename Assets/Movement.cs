using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;
public class Movement : MonoBehaviour
{
    const int MAX_POWER = 50;
    const int RAILGUN_CD = 3;

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
    private bool stillCharging = false;

    public float chargePower = 2;
    private float chargeTimer = 0;
    private float railGunTimer = 0;
    private float waitRGtimer = 0;

    private WeaponParent weaponParent;
    CinemachineImpulseSource impulseSource;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        weaponParent = GetComponentInChildren<WeaponParent>();
        impulseSource = GetComponent<CinemachineImpulseSource>();
        powerBar = GameObject.FindGameObjectWithTag("PowerBar");
    }
  
    
    void Update()
    {
        
        

        if(stillCharging)
        {
            waitRGtimer += Time.deltaTime;
        }
        if(waitRGtimer > 1f)
        {
            stillCharging = false;
            waitRGtimer = 0;
        }

        chargeTimer += Time.deltaTime;
        railGunTimer += Time.deltaTime;

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
            stillCharging = true;
            if (chargePower > 20 && railGunTimer > RAILGUN_CD)
            {
                
                StartCoroutine(Railgun());
                railGunTimer = 0;
               
            }
            else if(chargePower < 20)
            {
                stillCharging = false;
            }
        }

       

         
        
        Vector2 movement = new Vector2(moveX, moveY);
        if(!isRailGunning)
        {
            rb.velocity = movement * speed;
        }
        else
        {
        
        }
        if (chargeTimer >= .01f)
        {
            if (isCharging)
            {
                speed = 1.5f;
            }
            else
            {
                speed = 5;
            }
            if (isCharging && railGunTimer > RAILGUN_CD)
            {
                chargePower = System.MathF.Pow(chargePower, 1.005f);
                chargePower = chargePower + 0.25f;
                if (chargePower > MAX_POWER)
                {
                    chargePower = MAX_POWER;
                }
            }
            else
            {
                if(!stillCharging)
                {
                    chargePower = chargePower - 2;
                }
                if (chargePower < 2)
                {
                    chargePower = 2;
                }

            }
            chargeTimer = 0;

        }

        powerBar.transform.localScale = new Vector3((chargePower - 2) / MAX_POWER * 2, powerBar.transform.localScale.y, powerBar.transform.localScale.z); 

       // Debug.Log(chargePower + " " + isCharging);
    }
    public void ScreenShake(float amplifiedAmplitude, float frequency)
    {
        impulseSource.GenerateImpulse(new Vector2(amplifiedAmplitude, amplifiedAmplitude));
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
        ScreenShake(chargePower/ 10, chargePower/ 50);
        
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
