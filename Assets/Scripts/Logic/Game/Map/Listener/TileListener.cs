using UnityEngine;
using ECSModel;

//  这里有点问题，要讨论一下怎么修改。
public class TileListener : MonoBehaviour
{
    public CollisionType CurType = CollisionType.NONE;
    int id = 0, waveID = 0;
    CollisionBridge curBrid = null;

    private void Start()
    {
        if (CurType == CollisionType.Bricks)
        {
            id = int.Parse(this.gameObject.name);
            Transform parent = gameObject.transform.parent.parent;
            if (parent != null)
                waveID = int.Parse(parent.name);
        }
        curBrid = new CollisionBridge();
    }

    float x = 0.0f;
    void OnCollisionEnter2D(Collision2D collision)
    {
        int gameState = (int)GameCtrlComponent.Instance.CurGameState;
        if (gameState < (int)GameState.INGAMEMAP)
            return;

        //if (collision.gameObject.tag != "ball" && collision.gameObject.tag != "bullet")
        if (!collision.gameObject.CompareTag("ball") && !collision.gameObject.CompareTag("bullet"))
        {
            return;
        }
        
            

        ContactPoint2D point = collision.GetContact(0);
        curBrid.TileID = id;
        curBrid.WaveID = waveID;
        curBrid.CollisionType = CurType;
        curBrid.HitPoint = point;

        if (collision.gameObject.CompareTag( "bullet"))
        {
            if (CurType == CollisionType.Racket) return;
            curBrid.BulletID = long.Parse(collision.gameObject.name);
            Game.EventSystem.Run<CollisionBridge>(EventIdType.OnBulletCollsionEnter, curBrid);
            return;
        }
        
        curBrid.BallID = long.Parse(collision.gameObject.name);
        Game.EventSystem.Run<CollisionBridge>(EventIdType.OnBoxCollider2DEnter, curBrid);
 
    }

}

