using System;
using UnityEngine;

public class DamageArea : MonoBehaviour
{
    public float damage = 1;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.transform.TryGetComponent(out Player player))
            player.Damage(damage);
    }
}
