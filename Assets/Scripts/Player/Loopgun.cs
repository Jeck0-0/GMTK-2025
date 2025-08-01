using Sirenix.OdinInspector;
using UnityEngine;

public class Loopgun : Weapon
{
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] Transform firePoint;
    private MuzzleFlash muzzleFlash;

    public float damage = 5;
    public float attackSpeed = 2;
    public float projectileSpeed = 5;
    public float projectileSpeedVariation = 1;
    public float projectileDirectionVariation = 0;
    public float projectileLifetime = 3;

    public int projectilesPerShot = 1;

    public float reloadSpeed = 1;
    public int maxAmmo = 6;

    public bool recordShots = true;

    [DisableInEditorMode] public int currentAmmo;
    protected float nextShotMinTime = 0;
    protected bool isReloading;

    protected override void Awake()
    {
        currentAmmo = maxAmmo;
        muzzleFlash = firePoint.GetComponent<MuzzleFlash>();
        base.Awake();
    }

    public override void Reload()
    {
        if (currentAmmo == maxAmmo)
            return;

        //TODO: add animation/sound
        isReloading = true;
        Invoke(nameof(DoReload), reloadSpeed);
    }

    //end of the reload animation
    protected void DoReload()
    {
        currentAmmo = maxAmmo;
        isReloading = false;
    }

    public override void TryAttacking()
    {
        //attack if possible.
        if (isReloading)
            return;

        if (currentAmmo <= 0)
        {
            Reload();
            return;
        }

        if (nextShotMinTime > Time.time)
            return;

        Attack();
    }


    public override void Attack()
    {
        Shaker.Instance.ShakeCamera(2.5f, 0.4f);
        muzzleFlash.Flash();
        for (int i = 0; i < projectilesPerShot; i++)
        {
            if(recordShots)
            LoopManager.Instance.RecordShot(firePoint.position, transform.rotation);
            var go = Instantiate(projectilePrefab, firePoint.position, GetProjectileDirection());
            var proj = go.GetComponent<Projectile>();
            InitializeProjectile(proj);
        }
        nextShotMinTime = Time.time + attackSpeed;
        currentAmmo--;
    }

    //inheriting classes can override this to make it easier to have different types of projectiles
    protected virtual void InitializeProjectile(Projectile projectile)
    {
        projectile.Initialize(damage, GetProjectileSpeed(), projectileLifetime, owner);
    }

    protected float GetProjectileSpeed()
    {
        return projectileSpeed + Random.Range(0, projectileSpeedVariation) - projectileSpeedVariation / 2;
    }

    protected Quaternion GetProjectileDirection()
    {
        var variation = Random.Range(0, projectileDirectionVariation) - projectileDirectionVariation / 2;
        return Quaternion.Euler(transform.rotation.eulerAngles + Vector3.forward * variation);
    }

}
