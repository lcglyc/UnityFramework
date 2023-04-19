using DG.Tweening;
using ECSModel;
using UnityEngine;

[Event(EventIdType.UpdateBallAlpha)]
public class BallAlphaSystem:AEvent<long,bool>
{
    public override void Run(long a,bool isShow)
    {
        Ball ball = BallComponent.Instance.Get(a);
        if (ball == null)
            return;

        float alpha = isShow ? 1 : 0;
      //  ball.GameObject.GetComponent<SpriteRenderer>().DOFade(alpha, 0.2f);
      var spriteRender = ball.GameObject.GetComponent<SpriteRenderer>();
      spriteRender.DOFade(alpha, 0.2f);
    }
}