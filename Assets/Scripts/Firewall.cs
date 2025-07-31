using System;
using UnityEngine;

public class Firewall : MonoBehaviour
{
    public float speed;
    void Update()
    {
        transform.position += Vector3.right * (speed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.TryGetComponent(out Player player))
            player.Damage(9999);
    }
}
