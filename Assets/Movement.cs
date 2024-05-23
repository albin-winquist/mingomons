using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;
using TMPro;
using HealthBar;


public class Movement : MonoBehaviour
{
    const int MAX_POWER = 50;
    const int RAILGUN_CD = 3;
    const float STILL_CHARGING_TIMER = 0.5f;
    float LENS_SIZE;
    float START_EMISSION;
    float START_EM_SPEED;
    float START_EM_SIZE;

    [SerializeField] GameObject attackPoint;
    float attackRange = 0.5f;
    [SerializeField] float speed = 5;
    public Rigidbody2D rb;
    public Bullets weapon;
    public Transform firePoint;
    Transform playerTransform;
    public Transform eyePoint;
    float minuesValue = 0.007f;
    bool preJumper = false;
    float subJumper = 1;
    Vector2 thing;

    bool cantRailGun = false;

    float yup = 0;

    bool hadCharged = false;

    public bool tryde = false;
    bool railgunTride = false;
    bool canJump = false;

    GameObject particleAccelerator;
    ParticleSystem particleAccel2;
    GameObject particleExplosion;
    ParticleSystem particleExp2;
    ParticleSystem particleExp3;

    ParticleSystem bloodPart;

    GameObject jumpPart;
    GameObject particleExp4;

    float poweredByCharge;
    float powerSpeed;

    public bool isJumping;

    GameObject powerBar;
    GameObject chargeHealthBar;
    GameObject virtualCamera;
    GameObject jumpSHOPTag;
    
    [SerializeField] float dodgeSpeed = 10;
    [SerializeField] float dodgeTime = 0.5f;
    [SerializeField] float dodgeCd = 2;

    bool isRailGunning;
    bool currentlyGunning = false;


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
    private bool isJumpCharging = false;

    float tempJumpPower = 2;

    private bool isHealthCharging = false;
    private bool isRailGunCharging = false;


    private bool isJumpPower = false;
    public float jumpPower = 2;
    private float jumpTimer = 0;
    private float jumpMaxed = 0;

    private float minusPower = 2;

    float jumpPreTimer = 0;

    

    float tapChargeTimer = 0f;
    bool isTapping = false;

    float dampeningRate = 0.1f;

    private WeaponParent weaponParent;
    CinemachineImpulseSource impulseSource;
    private StaminaBar staminaAccess;
    private Healthbar healthBar;
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
        particleExplosion = GameObject.FindGameObjectWithTag("ExplosionTag");
        particleExp4 = GameObject.FindGameObjectWithTag("ShotgunTag");
        jumpPart = GameObject.FindGameObjectWithTag("JumpPTag");
        jumpSHOPTag = GameObject.FindGameObjectWithTag("JumpSHOPTag");
        bloodPart = GameObject.FindGameObjectWithTag("playerBlood").GetComponent<ParticleSystem>();
        staminaAccess = GetComponentInChildren<StaminaBar>();
        healthBar = GetComponentInChildren<Healthbar>();
        LENS_SIZE = virtualCamera.GetComponent<CinemachineVirtualCamera>().m_Lens.OrthographicSize;
        START_EM_SIZE = particleAccelerator.GetComponent<ParticleSystem>().startSize;
        START_EMISSION = particleAccelerator.GetComponent<ParticleSystem>().emissionRate;
        START_EM_SPEED = particleAccelerator.GetComponent<ParticleSystem>().startSpeed;
        particleAccel2 = particleAccelerator.GetComponent<ParticleSystem>();
        particleExp2 = particleExplosion.GetComponent<ParticleSystem>();
        particleExp3 = particleExp4.GetComponent<ParticleSystem>();
    }
  
    
    void Update()
    {
        if(healthBar.Health <= 0)
        {
           Destroy(gameObject);
        }


        

        if(stillCharging)
        {
            waitRGtimer += Time.deltaTime;
        }
        if(waitRGtimer > STILL_CHARGING_TIMER)
        {
            stillCharging = false;
           
            waitRGtimer = 0;
        }

        chargeTimer += Time.deltaTime;
        railGunTimer += Time.deltaTime;
        jumpPreTimer += Time.deltaTime;

        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");
        if(isTapping)
        {
            tapChargeTimer += Time.deltaTime;
        }
     
        if (CanDodge && Input.GetKeyDown(KeyCode.LeftShift))
        {
            StartCoroutine(Dodge());
           
        }
        if (!isJumpPower && !isHealthPower && !isJumping)
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                currentlyGunning = true;
                railgunTride = true;
                isRailGunCharging = true;
                
                isCharging = true;

            }
            if (Input.GetKeyUp(KeyCode.X))
            {
                
                    isCharging = false;
                
                    
                
                    hadCharged = true;
                if (!cantRailGun)
                {
                    stillCharging = true;
                    if (chargePower > 20 && railGunTimer > RAILGUN_CD)
                    {

                        StartCoroutine(Railgun());
                        StartCoroutine(ShotgunFire(chargePower));
                        railGunTimer = 0;


                    }
                    else if (chargePower < 20)
                    {
                        stillCharging = false;
                    }
                }
            }
        }
        if (!isJumpPower && !isCharging && !isJumping && staminaAccess.Stamina > 40)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                currentlyGunning = true;
                isHealthCharging = true;

                isHealthPower = true;
            }
            if (Input.GetKeyUp(KeyCode.F))
            {
                isHealthPower = false;
                currentlyGunning = false;
                
                if (healthPower > 20 && railGunTimer > RAILGUN_CD)
                {
                    railGunTimer = 0;
                   
                }
                
            }
        }
        if(staminaAccess.Stamina <= 0)
        {
            isHealthPower = false;
            currentlyGunning = false;
        }

        if (Input.GetKeyDown(KeyCode.Space) && jumpPower <= 2 && !canJump && !currentlyGunning && staminaAccess.Stamina > 25)
        {
            StartCoroutine(preJump());
         
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            jumpMaxed = jumpPower;
        }
        if (tapChargeTimer > 1)
        {
           if (!Input.GetKey(KeyCode.Space))
            {
                isJumpPower = false;
                stillCharging = true;
                
                if (railGunTimer > RAILGUN_CD)
                {
                    railGunTimer = 0;
                }
                
                isTapping = false;
                tapChargeTimer = 0;
            }
        }

        if (stillCharging)
        {
            jumpTimer += Time.deltaTime;
            if (jumpTimer > STILL_CHARGING_TIMER)
            {
                jumpTimer = STILL_CHARGING_TIMER;
            }
        }
        else
        {
            jumpTimer -= Time.deltaTime;
            if (jumpTimer < 0)
            {
                jumpTimer = 0;
            }
        }


       



            Vector2 movement = new Vector2(moveX, moveY);
        if(!isRailGunning)
        {
            rb.velocity = movement * speed;
        }

        Debug.Log(currentlyGunning);
        if (chargeTimer >= .01f)
        {

            ChargeResult result = Charge(isCharging, chargePower, stillCharging, railGunTimer, "railgun");
            chargePower = result.power;

            if (isCharging)
            {
                speed = 2f;
            }
            else if(isHealthPower)
            {
                speed = 0.7f;
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
                particleExp2.Play();
                staminaAccess.SpendStamina(50f, 25f);

               
            }
            
            if ((!isHealthPower || railGunTimer <= RAILGUN_CD) && (healthPower <= 2))
            {
                isRailGunCharging = false;
                isHealthCharging = false;
            }
            if (!railgunTride)
            {
                ChargeResult jumpResult = ChargeJump(isJumpPower, jumpPower, stillCharging, railGunTimer, jumpTimer, jumpMaxed);
                jumpPower = jumpResult.power;

                if ((!isJumpPower || railGunTimer <= RAILGUN_CD) && (jumpPower <= 2))
                {
                    isJumpCharging = false;
                }
            }
            chargeTimer = 0;
        }
        


        if(chargePower == MAX_POWER)
        {
            bloodPart.Play();
            ScreenShake(5, 5);
            isCharging = false;
            
            staminaAccess.SpendStamina(0, -15f);
            cantRailGun = true;
            stillCharging = false;
           
        }
       


        if (!isCharging && chargePower  <= 2.01f) 
        {
            railgunTride = false;
            cantRailGun = false;
        }
       
        if (!isHealthCharging)
        {
            powerBar.transform.localScale = new Vector3((chargePower * 10 - 2) / MAX_POWER * 2, powerBar.transform.localScale.y, powerBar.transform.localScale.z);
        }
        if (!isRailGunCharging)
        {
            chargeHealthBar.transform.localScale = new Vector3((healthPower * 10 - 2) / MAX_POWER * 2, chargeHealthBar.transform.localScale.y, chargeHealthBar.transform.localScale.z);
            if (isHealthPower && healthPower > 2)
            {
                EffectsOnCharge(healthPower);
                ScreenShake(healthPower / 100, healthPower / 100);
                virtualCamera.GetComponent<CinemachineVirtualCamera>().Follow = playerTransform;
                
                
            }
            else
            {
                particleAccelerator.GetComponent<ParticleSystem>().startSize = START_EM_SIZE;
                particleAccelerator.GetComponent<ParticleSystem>().startSpeed = START_EM_SPEED;
                particleAccelerator.GetComponent<ParticleSystem>().emissionRate = START_EMISSION;
                particleAccelerator.GetComponent<ParticleSystem>().enableEmission = false;


                virtualCamera.GetComponent<CinemachineVirtualCamera>().m_Lens.OrthographicSize = LENS_SIZE;
                if(!isHealthPower)
                {
                    virtualCamera.GetComponent<CinemachineVirtualCamera>().Follow = eyePoint.GetComponent<Transform>();
                }
                
            }
        }
        if (jumpPower > 2 && chargePower <= 2)
        {

           
            if(yup<jumpPower)
            {
                yup = jumpPower;
            }
            else if(!tryde)
            {
                yup = 0;
            }
            attackRange = yup;
            thing = new Vector3(playerTransform.localScale.x, playerTransform.localScale.y, playerTransform.localScale.z);
            virtualCamera.GetComponent<CinemachineVirtualCamera>().Follow = playerTransform;
            gameObject.GetComponent<BoxCollider2D>().enabled = false;


            virtualCamera.GetComponent<CinemachineVirtualCamera>().m_Lens.OrthographicSize += jumpPower;

            playerTransform.localScale = new Vector3(jumpPower,jumpPower, playerTransform.localScale.z);

        }
        else
        {
            if(!preJumper)
            {
                 playerTransform.localScale = new Vector3(1,1,1);
            }
            gameObject.GetComponent<BoxCollider2D>().enabled = true;
        }


        if (jumpPreTimer >= .01f)
        {
            if (preJumper)
            {
                
                minuesValue += 0.00001f;
                subJumper -= minuesValue;

            }
            else
            {
                minuesValue = 0.007f;
            }
            jumpPreTimer = 0;
        }
        if (preJumper)
        {
            speed = 0.5f;

        }
        if(subJumper < 1)
        {
            playerTransform.localScale = new Vector3(subJumper, subJumper, 1);
        }

       

        // Debug.Log(isHealthCharging + " <-H : R-> " + isRailGunCharging + "       :||:       " + healthPower + " <- HealthCharger : RailgunCharger -> " + chargePower + "         JumpPower -> " + jumpPower);
    }

    public void setExplosionPower(float chargePower)
    {
        jumpSHOPTag.GetComponent<ParticleSystem>().startSize *= chargePower;
        jumpSHOPTag.GetComponent<ParticleSystem>().startSpeed *= chargePower;
        jumpSHOPTag.GetComponent<ParticleSystem>().Play();
        ScreenShake(chargePower * 5, chargePower * 5);
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
        particleAccel2.emissionRate += ChargePower / 2000;
        if (particleAccel2.startSize <= 0.7f)
        {
            particleAccel2.startSize += ChargePower / 10000;
        }
        particleAccel2.startSpeed += ChargePower / 5000;
    }
    IEnumerator ShotgunFire(float ChargePower)
    {
        particleExp3.startSpeed += ChargePower / 4;
        Vector3 originalScale = new Vector3(particleExp4.transform.localScale.x, particleExp4.transform.localScale.y, particleExp4.transform.localScale.z);
        Vector3 newScale = new Vector3(particleExp4.transform.localScale.x + ChargePower / 250, particleExp4.transform.localScale.y, particleExp4.transform.localScale.z);
        particleExp4.transform.localScale = newScale;
        particleExp3.startSpeed += ChargePower/5;
        particleExp3.startSize += ChargePower/500;

        particleExp3.Play();
        yield return new WaitForSeconds(0.1f);
        particleExp4.transform.localScale = originalScale;
        

    }

    
    private ChargeResult Charge(bool isCharging, float power, bool stillCharging, float timer, string chargeType)
    {

        ChargeResult result = new ChargeResult();

        

        if(chargeType == "railgun")
        {
            poweredByCharge = 1.0025f;
            powerSpeed = 0.1f;
            minusPower = this.minusPower;

        }
        else if(chargeType == "health")
        {
            poweredByCharge = 1.001f;
            powerSpeed = 0.1f;
            minusPower = this.minusPower;
        }
        else if (chargeType == "jump")
        {
            poweredByCharge = 1.005f;
            powerSpeed = 0.05f;
            minusPower = 0.5f;
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

                result.power = result.power - minusPower;
            }
            if (result.power < 2)
            {
                result.power = 2;

            }
            if (hadCharged && result.power < 2.05f && currentlyGunning && chargeType == "railgun")
            {
                currentlyGunning = false;
                hadCharged = false;
            }


        }
        //if(stillCharging && chargeType == "jump")
        //{
        //    result.power += Mathf.Pow(2f, 0.5f);
        //}

        return result;
    }
    private void JumpCollider()
    {
        Collider2D[] enemiesHit = Physics2D.OverlapCircleAll(attackPoint.transform.position, attackRange/2);

        foreach (Collider2D enemy in enemiesHit)
        {
            if (enemy.gameObject.CompareTag("Enemy") && enemy.GetComponent<EnemyHealth>() != null)
            {
                enemy.GetComponent<EnemyHealth>().takeDamage(5);
            }
                
        }
    }
    private ChargeResult ChargeJump(bool isCharging, float power, bool stillCharging, float timer, float jumpTimer, float jumpMaxed)
    {
        ChargeResult result = new ChargeResult();

        result.power = power;

        poweredByCharge = 1.005f;
        powerSpeed = 0.1f;
        minusPower = 0.5f;

        if (isCharging)
        {
           
            result.power = System.MathF.Pow(result.power, poweredByCharge - 0.0055f);
            result.power = result.power + powerSpeed;
            if (result.power > MAX_POWER)
            {
                result.power = MAX_POWER;
            }
        }
        else
        {
           if (stillCharging)
           {
                tryde = true;
                float percent = (STILL_CHARGING_TIMER - jumpTimer) / STILL_CHARGING_TIMER;

                poweredByCharge = 1.0f + 0.0015f * percent;
                powerSpeed = powerSpeed * percent;

                result.power = System.MathF.Pow(result.power, poweredByCharge);
                result.power = result.power + powerSpeed;

                
                
            } else
            {
                if (!stillCharging && !isCharging && result.power > 2 && !railgunTride)
                {
              
                    ScreenShake(result.power / 10, result.power / 10);
                }
                
                if (tryde && result.power < 2.05f && !railgunTride)
                {
                    setExplosionPower(yup/10);

                    JumpCollider();
                    tryde = false;
                    canJump = false;

                    //attack innan isjumoing
                    isJumping = false;
                }

                result.power = result.power - minusPower;

               
                if (result.power < 2)
                {
                   
                    result.power = 2;
                }
            }

        }

   

        return result;

    }


    IEnumerator preJump()
    {
        preJumper = true;
        isJumping = true;
        yield return new WaitForSeconds(0.5f);
        staminaAccess.SpendStamina(25f, 0f);
        jumpPart.GetComponent<ParticleSystem>().Play();
        subJumper = 1;
        preJumper = false;
        canJump = true;
        isJumpCharging = true;
        isJumpPower = true;
        isTapping = true;

        jumpTimer = 0;
    }
    IEnumerator Railgun()
    {
        if (!cantRailGun)
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
            ScreenShake(chargePower / 10, chargePower / 50);

            weapon.RailGunFire();
            yield return new WaitForSeconds(0.5f);
            isRailGunning = false;
        }

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
