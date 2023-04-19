using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BossRotationAnimation : MonoBehaviour
{
    public int Duration;
    public float DelayTime;
    public Ease mEase;

    public Vector3 To;
    private void OnEnable()
    {
        StartCoroutine(delayRunRotation()); 
    }

    IEnumerator delayRunRotation()
    {
        yield return  new WaitForSeconds(DelayTime);
        this.gameObject.transform.DOLocalRotate(To, Duration,RotateMode.FastBeyond360).SetEase(mEase)
            .SetLoops(-1, LoopType.Restart);
    }

}
