using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleBulletBlood : MonoBehaviour
{
    GameObject bullet;
    void Start()
    {
        if (GameObject.FindGameObjectWithTag("RailBulletTag") != null)
        {
            bullet = GameObject.FindGameObjectWithTag("RailBulletTag");
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if(bullet != null)
        {
            transform.position = bullet.transform.position;
            transform.rotation = bullet.transform.rotation;
        }
       
    }

}
