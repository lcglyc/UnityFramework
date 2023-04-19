using ECSModel;
using DG.Tweening;
using UnityEngine;
using Component = ECSModel.Component;

public class RacketPosCom : Component
{

    GameObject thisRacketGo;
    Racket thisRacket;
    public void Init()
    {
        thisRacket = this.GetParent<Racket>();
        thisRacketGo = thisRacket.GameObject;
        SetRacketStartPostion();
    }

    // TODO：这个数字要经过计算得出，主要是分辨率。
    public void SetRacketStartPostion()
    {
        thisRacket.Position = new Vector3(0, -5.5f, 0);
    }

    
    public void SetUpgradePanelPosition(int index)
    {
        thisRacket.LocalPosition = new UnityEngine.Vector3(index * Define.RacketDistance, 2.48f, 0);
    }

    public void MoveToCenter( int index )
    {
        Vector3 position = new UnityEngine.Vector3(index * Define.RacketDistance, 2.48f, 0);
        var curSelectRacket = RacketComponent.Instance.GetCurRacketIndex();
        if (index != curSelectRacket)
        {
            thisRacket.LocalPosition = position;
        }
        else
        {
            thisRacketGo.transform.DOLocalMove(position, 0.5f);
        }
    }

    public void MoveToBottom()
    {
        thisRacketGo.transform.DOLocalMove(new Vector3(0, -5.5f, 0), 0.5f);
    }

    public void UpdateSpriteAlpha(bool isShow)
    {
        float alpha = isShow ? 1 : 0;
        thisRacketGo.GetComponent<SpriteRenderer>().DOFade(alpha, 0.2f);
    }

}
