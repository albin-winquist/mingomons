using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;
using TMPro;

public class Movement : MonoBehaviour
{
    const int MAX_POWER = 50;
    const int RAILGUN_CD = 3;
    float LENS_SIZE;
    float START_EMISSION;
    float START_EM_SPEED;
    float START_EM_SIZE;
    [SerializeField] float speed = 5;
    public Rigidbody2D rb;
    public Bullets weapon;
    public Transform firePoint;
    Transform playerTransform;
    public Transform eyePoint;

    GameObject particleAccelerator;
    ParticleSystem particleAccel2;
    float poweredByCharge;
    float powerSpeed;

    GameObject powerBar;
    GameObject chargeHealthBar;
    GameObject virtualCamera;
    
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

    public float healthPower = 2;
    public bool isHealthPower = false;

    private bool isHealthCharging = false;
    private bool isRailGunCharging = false;

    private WeaponParent weaponParent;
    CinemachineImpulseSource impulseSource;
    struct ChargeResult
    {
        public float power;
        public float speed;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        playerTransform = GetComponent<Transform>();
        weaponParent = GetComponentInChildren<WeaponParent>();
        impulseSource = GetComponent<CinemachineImpulseSource>();
        powerBar = GameObject.FindGameObjectWithTag("PowerBar");
        chargeHealthBar = GameObject.FindGameObjectWithTag("ChargeHealthBar");
        virtualCamera = GameObject.FindGameObjectWithTag("VirtualCamera");
        particleAccelerator = GameObject.FindGameObjectWithTag("ParticleTag");

        LENS_SIZE = virtualCamera.GetComponent<CinemachineVirtualCamera>().m_Lens.OrthographicSize;
        START_EM_SIZE = particleAccelerator.GetComponent<ParticleSystem>().startSize;
        START_EMISSION = particleAccelerator.GetComponent<ParticleSystem>().emissionRate;
        START_EM_SPEED = particleAccelerator.GetComponent<ParticleSystem>().startSpeed;
        particleAccel2 = particleAccelerator.GetComponent<ParticleSystem>();
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
            isRailGunCharging = true;
            
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
        if(Input.GetKeyDown(KeyCode.R))
        {
            isHealthCharging = true;

            isHealthPower = true;
        }
        if (Input.GetKeyUp(KeyCode.R))
        {
            isHealthPower = false;

            stillCharging = true;
            if (healthPower > 20 && railGunTimer > RAILGUN_CD)
            {
                railGunTimer = 0;

            }
            else if (healthPower < 20)
            {
                stillCharging = false;
            }
        }


        Vector2 movement = new Vector2(moveX, moveY);
        if(!isRailGunning)
        {
            rb.velocity = movement * speed;
        }
       

        if (chargeTimer >= .01f)
        {
            ChargeResult result = Charge(isCharging, chargePower, stillCharging, railGunTimer, "railgun");
            chargePower = result.power;

            if (isCharging || isHealthPower)
            {
                speed = 0.5f;
            }
            else
            {
                speed = 5;
            }
            if ((!isCharging || railGunTimer  <= RAILGUN_CD) && (chargePower <= 2))
            {
                isRailGunCharging = false;
                isHealthCharging = false;
            }


            ChargeResult healthResult = Charge(isHealthPower, healthPower, stillCharging, railGunTimer, "health");
            healthPower = healthResult.power;
            if(healthResult.power >= 50)
            {
                healthPower = 0;
            }
 
            if ((!isHealthPower || railGunTimer <= RAILGUN_CD) && (healthPower <= 2))
            {
                isRailGunCharging = false;
                isHealthCharging = false;
            }



            chargeTimer = 0;
        }
        

        if (!isHealthCharging)
        {
            powerBar.transform.localScale = new Vector3((chargePower - 2) / MAX_POWER * 2, powerBar.transform.localScale.y, powerBar.transform.localScale.z);
        }
        if (!isRailGunCharging)
        {
            chargeHealthBar.transform.localScale = new Vector3((healthPower - 2) / MAX_POWER * 2, chargeHealthBar.transform.localScale.y, chargeHealthBar.transform.localScale.z);
            if (isHealthPower && healthPower > 2)
            {
                ScreenShake(healthPower / 100, healthPower / 100);
                virtualCamera.GetComponent<CinemachineVirtualCamera>().Follow = playerTransform;
                EffectsOnCharge(healthPower);
                
            }
            else
            {
                particleAccelerator.GetComponent<ParticleSystem>().startSize = START_EM_SIZE;
                particleAccelerator.GetComponent<ParticleSystem>().startSpeed = START_EM_SPEED;
                particleAccelerator.GetComponent<ParticleSystem>().emissionRate = START_EMISSION;
                particleAccelerator.GetComponent<ParticleSystem>().enableEmission = false;
                virtualCamera.GetComponent<CinemachineVirtualCamera>().m_Lens.OrthographicSize = LENS_SIZE;
                virtualCamera.GetComponent<CinemachineVirtualCamera>().Follow = eyePoint.GetComponent<Transform>();
            }
        }
        
        Debug.Log(isHealthCharging + " <-H : R-> " + isRailGunCharging + "       :||:       " + healthPower + " <- HealthCharger : RailgunCharger -> " + chargePower);
    }
    public void ScreenShake(float amplifiedAmplitude, float frequency)
    {
        impulseSource.GenerateImpulse(new Vector2(amplifiedAmplitude, amplifiedAmplitude));
    }
    public void EffectsOnCharge(float ChargePower)
    {
        if (virtualCamera.GetComponent<CinemachineVirtualCamera>().m_Lens.OrthographicSize > 3.5f)
        {
            virtualCamera.GetComponent<CinemachineVirtualCamera>().m_Lens.OrthographicSize -= ChargePower / 5000;
        }
        particleAccel2.enableEmission = true;
        particleAccel2.emissionRate += ChargePower / 250;
        if (particleAccel2.startSize <= 0.7f)
        {
            particleAccel2.startSize += ChargePower / 10000;
        }
        particleAccel2.startSpeed += ChargePower / 5000;
    }

    private ChargeResult Charge(bool isCharging, float power, bool stillCharging, float timer, string chargeType)
    {

        ChargeResult result = new ChargeResult();

        

        if(chargeType == "railgun")
        {
            poweredByCharge = 1.0025f;
            powerSpeed = 0.1f;
        }
        else if(chargeType == "health")
        {
            poweredByCharge = 1.001f;
            powerSpeed = 0.05f;
        }

        result.power = power;
        if (isCharging && timer > RAILGUN_CD)
        {
            result.power = System.MathF.Pow(result.power, poweredByCharge);
            result.power = result.power + powerSpeed;
            if (result.power > MAX_POWER)
            {
                result.power = MAX_POWER;
            }
        }
        else
        {
            if (!stillCharging)
            {

                result.power = result.power - 2;
            }
            if (result.power < 2)
            {
                result.power = 2;

            }

        }

        return result;
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
        //if (GetComponent<StaminaBar>().Stamina > 25)
        //{
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
        //}
    }
}
