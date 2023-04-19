using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ECSModel;
using Component = ECSModel.Component;
public class BulletMoveCom : Component
{
    private float speed;
    Rigidbody2D bulletRigidbody;
    public Vector2 Dir;
    private TimerComponent timerCom;

    public float Speed
    {
        get => speed;
        set => speed = value;
    }

    public void Init(float speed, float tmpDir = 0, int delaytime = 0)
    {
        GameObject thisBullet= this.GetParent<Bullet>().GameObject;
        this.speed = speed;
        Dir = new Vector2(tmpDir, 1).normalized;

        bulletRigidbody = thisBullet.GetComponent<Rigidbody2D>();
        if (bulletRigidbody == null)
            bulletRigidbody = thisBullet.AddComponent<Rigidbody2D>();

        bulletRigidbody.velocity = new Vector2(0, speed);
        timerCom = Game.Scene.GetComponent<TimerComponent>();
    }

    public void UpdateAngle()
    {
 
    }


    public override void Dispose()
    {
        base.Dispose();
    }
}
