﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BossMoveAnimation : MonoBehaviour
{
    public int MoveIndex;
    public  List<Vector3 > MovePosition;
    public float Duration;
    public float MoveDelay;
    public Ease ease;
    public void OnEnable()
    {
        Vector3 tarGetPostin = GetNextPostion();
        DoMove(tarGetPostin);
    }

    void DoMove(Vector3 tarGetPostin )
    {
        this.gameObject.transform.DOLocalMove(tarGetPostin, Duration).SetDelay(MoveDelay).SetEase(ease).OnComplete(
            () =>
            {
                Vector3 position = GetNextPostion();
                DoMove(position);
            }
        );
    }

    Vector3 GetNextPostion()
    {
        MoveIndex++;
        MoveIndex = MoveIndex > MovePosition.Count -1 ? 0: MoveIndex;
        return  MovePosition[MoveIndex];
    }
}