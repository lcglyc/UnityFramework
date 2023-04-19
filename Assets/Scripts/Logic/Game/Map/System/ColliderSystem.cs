using ECSModel;
using UnityEngine;
[Event(EventIdType.OnBoxCollider2DEnter)]
public class ColliderSystem : AEvent<CollisionBridge>
{
    public override void Run(CollisionBridge hitObj)
    {
        if (hitObj.CollisionType == CollisionType.Bricks)
        {
            if (UpdateTileHPAndCheckDead(hitObj.TileID, hitObj.BallID, hitObj.WaveID))
                return;
        }
 
        // 计算角度
        ProceBallAngle(hitObj.HitPoint, hitObj.BallID);
    }


    bool UpdateTileHPAndCheckDead(int id, long ballID, int waveID)
    {
        WaveEntity entity = WaveComponent.Instance.GetByWaveID(waveID);
        TileOperationCom tiles = entity.GetComponent<TileOperationCom>();
        if (tiles == null)
        {
            Debug.LogError("Tile is Null, wave ID : " + waveID + "    id: " + id + "  ");
            return false;
        }

        Ball ball = GetInGameBall(ballID);
        if (ball == null)
        {
            return false;
        }
        BallAttributeCom ballAtb = ball.GetComponent<BallAttributeCom>();
        float atk = ballAtb.BallAtk;
        return tiles.UpdateTileHp(id, atk);
    }

    Ball GetInGameBall(long ballID)
    {
        BallSplitCom splitcom = MapComponent.Inst.CurMap.GetComponent<BallSplitCom>();
        if (splitcom == null) return null;
        Ball curBall = splitcom.GetBall(ballID);
        return curBall;
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
            com.Dir = Vector2.Reflect(com.Dir, HitPoint.normal);
            com.UpdateAngle();
        }
        else
        {
            Debug.LogError("没有改变朝向");
        }
    }
}


    [Event(EventIdType.OnBulletCollsionEnter)]
    public class BulletColliderSystem : AEvent<CollisionBridge>
    {
        public override void Run(CollisionBridge hitObj)
        {
            if (hitObj.CollisionType == CollisionType.Bricks)
            {
                UpdateTileHp(hitObj.TileID, hitObj.WaveID);
            }
            
            DestroyBullet(hitObj.BulletID);
    }

        bool UpdateTileHp(int tileId, int waveId)
        {
            WaveEntity entity = WaveComponent.Instance.GetByWaveID(waveId);
            if(entity == null) Log.Debug("update tile hp entity is null");
            TileOperationCom tiles = entity.GetComponent<TileOperationCom>();
            if(tiles == null) Log.Debug("update tile hp TileOperationCom is null");
            var  atk =RacketComponent.Instance.CurRacket.GetComponent<RacketAttributeCom>().Atk;
            tiles.UpdateTileHp(tileId, atk);
            return true;
        }

        bool DestroyBullet(long id )
        {
            RacketShootingCom shootingCom = Game.Scene.GetComponent<RacketComponent>().CurRacket.GetComponent<RacketShootingCom>();
            shootingCom.ReduceBullet(id);
            return true;
        }
    }
