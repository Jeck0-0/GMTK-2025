using UnityEngine;

public class ProjectileWeapon : Weapon
{
    [SerializeField] AudioClip shotSound;
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] Transform firePoint;
    private MuzzleFlash muzzleFlash;

    public float damage = 50;
    public float attackSpeed = 2;
    public float projectileSpeed = 5;
    public float projectileSpeedVariation = 1;
    public float projectileDirectionVariation = 0;
    public float projectileLifetime = 3;

    public int projectilesPerShot = 1;
    protected float nextShotMinTime = 0;

    protected override void Awake()
    {
        muzzleFlash = firePoint.GetComponent<MuzzleFlash>();
        base.Awake();
    }

    public override void TryAttacking()
    {
        if (nextShotMinTime > Time.time)
        return;

        Attack();
    }
    

    public override void Attack()
    {
        if (shotSound)
        AudioManager.Instance.PlaySound(shotSound, 0.6f, transform);

        muzzleFlash.Flash();
        for (int i = 0; i < projectilesPerShot; i++)
        {
            var go = Instantiate(projectilePrefab, transform.position, GetProjectileDirection());
            var proj = go.GetComponent<Projectile>();
            InitializeProjectile(proj);
        }
        nextShotMinTime = Time.time + attackSpeed;
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