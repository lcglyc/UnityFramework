using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ECSModel
{
    public static class BulletFactory
    {
        public static Bullet Create(long id,Vector3 firePos)
        {
            ResourcesComponent resCom = Game.Scene.GetComponent<ResourcesComponent>();
            resCom.LoadBundle("bullet.unity3d");
            GameObject bulletobj = (GameObject)resCom.GetAsset("bullet.unity3d", "Bullet");
            GameObject go = UnityEngine.Object.Instantiate(bulletobj);
            go.transform.localPosition = Vector3.zero;
            go.transform.localRotation = Quaternion.identity;
            go.transform.localScale = Vector3.one;
            go.transform.position = firePos;
            go.name = id.ToString();

            Bullet bullet = ComponentFactory.CreateWithId<Bullet, GameObject>(id, go);
            var movCom = bullet.AddComponent<BulletMoveCom>();
            movCom.Init(20);
            var bulletAttr = bullet.AddComponent<BulletAttributeCom>();


            return bullet;
        }





    }




}
