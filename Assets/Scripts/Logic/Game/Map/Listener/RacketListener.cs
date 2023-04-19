using System;
using System.Collections;
using System.Collections.Generic;
using ECSModel;
using UnityEngine;

public class RacketListener : MonoBehaviour
{
    // 这里默认朝上
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("ball") ==false )
            return;
        
        int gameState = (int)GameCtrlComponent.Instance.CurGameState;
        if (gameState < (int)GameState.INGAMEMAP)
            return;


        long  BallID = long.Parse(other.gameObject.name);
        ContactPoint2D point = other.GetContact(0);
        float x= HitRacket(other);
        ProceBallAngle(point, x, BallID);
    }
    
    Ball GetInGameBall(long ballID)
    {
        BallSplitCom splitcom = MapComponent.Inst.CurMap.GetComponent<BallSplitCom>();
        if (splitcom == null) return null;
        Ball curBall = splitcom.GetBall(ballID);
        return curBall;
    }
    
    void ProceBallAngle(ContactPoint2D HitPoint, float x, long ballID)
    {
        Ball curBall = GetInGameBall(ballID);
        if (curBall == null)
        {
            Debug.LogError("BallID " + ballID + " 找不到");
            return;
        }

        BallMoveCom com = curBall.GetComponent<BallMoveCom>();
        if (com != null)
        {
            if (x != 0)
            {
                Vector2 dir = new Vector2(x, 1).normalized;
                com.Dir = dir;
            }
            else
            {
                com.Dir = Vector2.Reflect(com.Dir, Vector2.up);
            }

            com.UpdateAngle();
        }
    }
    
    
    float HitRacket(Collision2D collision)
    {
        float x = 0;
        x = this.gameObject.transform.position.x;
        float ballX = collision.gameObject.transform.position.x;
        x = (ballX - x) / 2.0f;
        return x;
    }
}
