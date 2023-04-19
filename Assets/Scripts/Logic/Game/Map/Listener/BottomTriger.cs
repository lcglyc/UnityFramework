using ECSModel;
using UnityEngine;

public class BottomTriger : MonoBehaviour
{


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!collision.CompareTag("ball"))
            return;

        long id = 0;
        if ( long.TryParse(collision.name,out id))
        {
            Game.EventSystem.Run(EventIdType.OnBallOutFrame, id);
        }
    }

}
