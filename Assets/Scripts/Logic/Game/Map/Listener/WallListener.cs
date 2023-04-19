using System;
using System.Collections;
using System.Collections.Generic;
using ECSModel;
using UnityEngine;

public enum WalllOrientation
{
    None,
    Up,
    Down,
    Left,
    Right
}

public class WallListener : MonoBehaviour
{
    public WalllOrientation mWall = WalllOrientation.None; 
    private void OnCollisionStay2D(Collision2D other)
    {
        if (GameCtrlComponent.Instance.CurGameState <= GameState.GM)
            return;
        
        if (!other.gameObject.CompareTag("ball"))
            return;
        
        long  BallID = long.Parse(other.gameObject.name);
        ContactPoint2D point = other.GetContact(0);
        ProceBallAngle(point ,BallID );
    }
    
    
    
    void ProceBallAngle(ContactPoint2D HitPoint, long ballID)
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
            Vector2 dir  = Vector2.Reflect(com.Dir, GetNormal(HitPoint.normal));
            dir = GetWallDir(dir);
            com.Dir = dir;
            com.UpdateAngle();
        }
    }
    
    Ball GetInGameBall(long ballID)
    {
        BallSplitCom splitcom = MapComponent.Inst.CurMap.GetComponent<BallSplitCom>();
        if (splitcom == null) return null;
        Ball curBall = splitcom.GetBall(ballID);
        return curBall;
    }

    Vector2 GetWallDir( Vector2 dir)
    {
        
        switch (mWall) 
        {
            case WalllOrientation.Down:
            {
                if (dir.y>0)
                    dir.y = -dir.y;
            }
                break;
            case WalllOrientation.Left:
            {
                if (dir.x <0)
                    dir.x = -dir.x;
            }
                break;
            case WalllOrientation.Right:
            {
                if (dir.x >0)
                    dir.x = -dir.x;
            }
                break;
            case WalllOrientation.Up:
            {
                if (dir.y<0)
                    dir.y = -dir.y;
            }
                break;
        }
        return dir;
    }

    Vector2 GetNormal(Vector2 defalut)
    {
        switch (mWall)
        {
            case WalllOrientation.Down:
                return  Vector2.down;
            case WalllOrientation.Left:
                return  Vector2.left;
            case WalllOrientation.Up:
                return  Vector2.up;
            case WalllOrientation.Right:
                return  Vector2.right;
        }

        return defalut;
    }
}
