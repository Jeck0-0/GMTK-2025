using System;
using UnityEngine;

public class Enemy : Unit
{
    public float visionRange;
    
    private void Update()
    {
        if (Vector3.Distance(Player.instance.transform.position, transform.position) <= visionRange)
        {
            weapon.transform.right = (Player.instance.transform.position - transform.position).normalized;
            TryAttacking();
        }
        
    }
}
