using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using DG.Tweening.Core;

public class WaggleBoss : MonoBehaviour
{
    public float Duration;
    public float Delay;
    public Vector3 ToPostion;
    public Ease ease = Ease.Linear;
    public int loopTime = -1;
    public LoopType loopType = LoopType.Yoyo;

    private Vector3 Postion;

    // Start is called before the first frame update
    void Awake()
    {
        Postion = this.transform.position;
    }

    private void OnEnable()
    {
        StartCoroutine(DelayMove());
    }

    IEnumerator DelayMove()
    {
        yield return  new WaitForSeconds(0.8f);
        this.transform.DOLocalMove(ToPostion, Duration).SetEase(ease).SetLoops(loopTime, loopType).SetDelay(Delay);
    }
}
