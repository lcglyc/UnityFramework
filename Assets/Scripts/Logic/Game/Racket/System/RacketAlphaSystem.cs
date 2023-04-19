using DG.Tweening;
using  ECSModel;
using UnityEngine;

[Event(EventIdType.UpdateRacketAlpha)]
public class RacketAlphaSystem : AEvent<long,bool>
{
    public override void Run(long a, bool b)
    {
        Racket racket = RacketComponent.Instance.Get(a);
        if (racket == null)
            return;

        float alpha= b ? 1 : 0;
        racket.GameObject.GetComponent<SpriteRenderer>().DOFade(alpha, 0.2f);
    }
}
