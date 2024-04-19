using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullets : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public Transform railGunPoint;
    public float fireForce = 20f;
    public GameObject railgunBulletPrefab;
   

    GameObject player;
    void Start()
    {
        
        player = GameObject.FindGameObjectWithTag("player");
    }
   
    public void Fire()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        bullet.GetComponent<Rigidbody2D>().AddForce(firePoint.up * fireForce, ForceMode2D.Impulse);
    }

    public void RailGunFire()
    {
        GameObject railBullet = Instantiate(railgunBulletPrefab, firePoint.position, firePoint.rotation);
        railBullet.GetComponent<Rigidbody2D>().AddForce(railGunPoint.up * fireForce * player.GetComponent<Movement>().chargePower / 15, ForceMode2D.Impulse);
    }
    
}
