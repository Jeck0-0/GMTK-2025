using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopManager : Singleton<LoopManager>
{
    [SerializeField] GameObject projectilePrefab;

    public List<ShotRecord> previousShots = new List<ShotRecord>();
    public Loopgun loopgun;

    private float runTime = 0f;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        ResetGame();
        runTime += Time.deltaTime;
    }

    public void RecordShot(Vector2 pos, Vector2 dir)
    {
        previousShots.Add(new ShotRecord
        {
            position = pos,
            direction = dir.normalized,
            timeSinceStart = runTime
        });
    }

    public void ResetGame()
    {
        runTime = 0f;
        StartCoroutine(ReplayShots());
    }

    IEnumerator ReplayShots()
    {

        foreach (var shot in previousShots)
        {
            yield return new WaitForSeconds(shot.timeSinceStart);
            FireBullet(shot.position, shot.direction);
        }
    }
    public  void FireBullet(Vector2 position, Vector2 direction)
    {
        var go = Instantiate(projectilePrefab, position, Quaternion.identity);
        go.GetComponent<Rigidbody2D>().linearVelocity = direction * 10f;
        var proj = go.GetComponent<Projectile>();
        InitializeProjectile(proj);
    }
    protected virtual void InitializeProjectile(Projectile projectile)
    {
        projectile.Initialize(loopgun.damage, GetProjectileSpeed(), loopgun.projectileLifetime);
    }

    protected float GetProjectileSpeed()
    {
        return loopgun.projectileSpeed + Random.Range(0, loopgun.projectileSpeedVariation) - loopgun.projectileSpeedVariation / 2;
    }
}
[System.Serializable]
public class ShotRecord
{
    public Vector2 position;
    public Vector2 direction;
    public float timeSinceStart;
}