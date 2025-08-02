using System;
using UnityEngine;

public class Unit : Targetable
{
    public Weapon weapon;

    public event Action WeaponDropped;

    protected Rigidbody2D m_rigidbody;

    protected override void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();

        if(!weapon)
        weapon = GetComponentInChildren<Weapon>();

        base.Awake();
    }

    public virtual void TryAttacking()
    {
        weapon?.TryAttacking();
    }
    
    public void PickupWeapon(Weapon newWeapon)
    {
        if(weapon == newWeapon) 
            return;
        
        if(weapon != null)
            DropWeapon();

        newWeapon.SetEquipped(this);
        weapon = newWeapon;
    }

    public void DropWeapon()
    {
        WeaponDropped?.Invoke();
        weapon = null;
    }
}