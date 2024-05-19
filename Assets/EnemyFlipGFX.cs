using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyFlipGFX : MonoBehaviour
{
    public AIPath aiPath;

    // Update is called once per frame
    void Update()
    {
        if (aiPath.desiredVelocity.x >= 0.01f)
        {
            transform.localScale = new Vector3(-0.6f, 3f, 1f);

        }
        else if (aiPath.desiredVelocity.x <= -0.01f)
        {
            transform.localScale = new Vector3(0.6f, 3f, 1f);
        }
    }
}
