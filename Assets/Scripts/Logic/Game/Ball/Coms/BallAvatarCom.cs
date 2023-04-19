using Cysharp.Threading.Tasks;
using ECSModel;
using UnityEngine;
using MonogolyConfig;
using Component = ECSModel.Component;

public class BallAvatarCom : Component
{
    ResourcesComponent resCom = null;
    SpriteRenderer mRender = null;
    Sprite mThisAvatar = null;
    public void Init(BallBaseData baseBallData)
    {
        string resName = baseBallData.BallRes;
        resCom = Game.Scene.GetComponent<ResourcesComponent>();
        mRender = this.Parent.GameObject.GetComponentInChildren<SpriteRenderer>();
        LoadAvatar(resName);
    }

    async UniTaskVoid LoadAvatar(string resName)
    {
        string resabName = resName.ToLower() + ".unity3d";
        await resCom.LoadBundleAsync(resabName);

        if (!Define.IsAsync)
        {
            Texture2D tmp = resCom.GetAsset(resName.ToLower() + ".unity3d", resName) as Texture2D;
            mThisAvatar = Sprite.Create(tmp, new Rect(0, 0, tmp.width, tmp.height), new Vector2(0.5f, 0.5f));
        }
        else
        {
            mThisAvatar = (Sprite)resCom.GetAsset(resName.ToLower() + ".unity3d", resName);
        }

        mRender.sprite = mThisAvatar;
    }


    public override void Dispose()
    {
        if (mThisAvatar != null)
        {
            mThisAvatar = null;
        }

        base.Dispose();
    }

}
