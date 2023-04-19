using System.Collections;
using System.Collections.Generic;
using ECSModel;
using UnityEngine;
using Component = ECSModel.Component;

[ObjectSystem]
public class RacketShootingAwakeSystem : UpdateSystem<RacketShootingCom>
{
    public override void Update(RacketShootingCom self)
    {
        self.Update();
    }
}

public class RacketShootingCom : Component
{
    List<Bullet> inGameBullets = new List<Bullet>();

    float fireCD;
    float fireRateInitial;
    public float FireRateInitial
    {
        get => fireRateInitial;
        set => fireRateInitial = value;
    }
    public bool CanShooting { get; set; }
    public void Init(float fireRate)
    {
        CanShooting = false;
        fireRateInitial = 0.5f;
        fireCD = fireRate;
    }

    public void Update()
    {
        if (!CanShooting) return;

        fireCD -= UnityEngine.Time.deltaTime;
        if (fireCD <= 0)
        {
            Fire();
            fireCD = fireRateInitial;
        }
    }

    void Fire()
    {
        var firePos = Game.Scene.GetComponent<RacketComponent>().CurRacket.Position;
        long id = RandomHelper.RandInt64();
        Bullet tmpBullet = BulletFactory.Create(id, firePos);
        tmpBullet.GameObject.transform.parent = this.GameObject.transform.parent.parent;
        inGameBullets.Add(tmpBullet);
    }

    public void ClearInSceneBullets()
    {
        foreach(var bullet in inGameBullets)
        {
            bullet.Dispose();
        }
        inGameBullets.Clear();
    }

    public bool ReduceBullet(long id)
    {
        Bullet bullet = GetBulletByID(id);
        if (bullet == null) return false;

        bullet.Dispose();
        inGameBullets.Remove(bullet);
        return true;
    }

    public Bullet GetBulletByID(long id)
    {
        return inGameBullets.Find(x => x.Id == id);
    }

    public override void Dispose()
    {
        if (inGameBullets != null)
            inGameBullets.Clear();
        base.Dispose();
    }
}
