using Sirenix.OdinInspector;
using UnityEngine;

public class Loopgun : Weapon
{
    [SerializeField] AudioClip shotSound;
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

    public int startAmmo = 6;

    public bool recordShots = true;

    [DisableInEditorMode] public int currentAmmo;
    protected float nextShotMinTime = 0;

    protected override void Awake()
    {
        currentAmmo = startAmmo;
        PlayerUI.Instance.UpdateAmmo(currentAmmo);
        muzzleFlash = firePoint.GetComponent<MuzzleFlash>();
        base.Awake();
    }
    private void Start()
    {
        LoopManager.Instance.OnGameReset += OnReset;
    }

    public override void TryAttacking()
    {
        if (currentAmmo <= 0)
        return;

        if (nextShotMinTime > Time.time)
        return;

        Attack();
    }


    public override void Attack()
    {
        AudioManager.Instance.PlaySound(shotSound, 0.8f);
        Shaker.Instance.ShakeCamera(5f, 0.4f);
        muzzleFlash.Flash();
        for (int i = 0; i < projectilesPerShot; i++)
        {
            if(recordShots)
            LoopManager.Instance.RecordShot(firePoint.position, firePoint.right);
            var go = Instantiate(projectilePrefab, firePoint.position, GetProjectileDirection());
            var proj = go.GetComponent<Loopbullet>();
            InitializeProjectile(proj);
        }
        nextShotMinTime = Time.time + attackSpeed;
        currentAmmo--;
        PlayerUI.Instance.UpdateAmmo(currentAmmo);
    }

    //inheriting classes can override this to make it easier to have different types of projectiles
    protected virtual void InitializeProjectile(Loopbullet projectile)
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
    private void OnDestroy()
    {
        if (LoopManager.Instance != null)
        LoopManager.Instance.OnGameReset -= OnReset;
    }

    private void OnReset()
    {
        currentAmmo = startAmmo;
        StopAllCoroutines();
    }
}