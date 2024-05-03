using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleLocatedPlayer : MonoBehaviour
{
    public Transform player;
    Transform currPos;
    public GameObject playerOBJ;
  
    void Start()
    {
        currPos = GetComponent<Transform>();
        
    }


    void Update()
    {
   
        if (player != null && currPos != null && playerOBJ.GetComponent<Movement>().isHealthPower)
        {
            
            currPos.position = new Vector3(player.position.x, player.position.y, currPos.position.z);
        }
    }
}
