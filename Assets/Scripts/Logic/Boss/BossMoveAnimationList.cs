using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BossMoveAnimationList : MonoBehaviour
{
    public int MoveIndex;
    public  List<Vector3 > MovePosition;
    public  List<float> Duration;
    public List<float> MoveDelay;
    public float startDelay;
    public Ease ease;
    public void OnEnable()
    {
        Vector3 tarGetPostin = GetNextPostion();
        float curDuration = GetDuration();
        float curDelay = GetDelay();
        
        DoMove(tarGetPostin,curDuration,startDelay);
    }

    void DoMove(Vector3 tarGetPostin,float curDuration ,float curDelay)
    {
        this.gameObject.transform.DOLocalMove(tarGetPostin, curDuration).SetDelay(curDelay).SetEase(ease).OnComplete(
            () =>
            {
                Vector3 position = GetNextPostion();
                float newDuration = GetDuration();
                float newDelay = GetDelay();
                DoMove(position,newDuration,newDelay);
            }
        );
    }

    Vector3 GetNextPostion()
    {
        MoveIndex++;
        MoveIndex = MoveIndex > MovePosition.Count -1 ? 0: MoveIndex;
        return  MovePosition[MoveIndex];
    }

    float GetDuration()
    {
        return Duration[MoveIndex];
    }

    float GetDelay()
    {
        return MoveDelay[MoveIndex];
    }
}
