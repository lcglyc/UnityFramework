using UnityEngine;
using ECSModel;
using Component = ECSModel.Component;
using Debug = System.Diagnostics.Debug;

//  用来控制球的移动，以及碰撞后反射的角度
public class BallMoveCom : Component
{
    private float speed;
    Rigidbody2D ballRigidbody;
    public Vector2 Dir;


    public void Init(float speed,float tmpDir =0,int delaytime=0 )
    {
        GameObject thisBall = this.GetParent<Ball>().GameObject;
        this.speed = speed;
        Dir =  new Vector2(tmpDir, 1).normalized;

        ballRigidbody = thisBall.GetComponent<Rigidbody2D>();
        if (ballRigidbody == null)
            ballRigidbody = thisBall.AddComponent<Rigidbody2D>();
    }

    public void UpdateAngle( )
    {
        ballRigidbody.velocity = Dir * speed;
    }
    
    public override void Dispose()
    {
        base.Dispose();
    }
}
