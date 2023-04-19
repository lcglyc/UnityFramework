using ECSModel;
using DG.Tweening;
using UnityEngine;
using Component = ECSModel.Component;
using Object = UnityEngine.Object;

//有一个缓动效果的进入
public class ChangeWaveComponent : Component
{
    private GameObject waveGo;
    private GameObject baffle;
    private float durationTime = 0;

    public void Init(GameObject go)
    {
        waveGo = go;
        JsonLibComponent jsonlib = Game.Scene.GetComponent<JsonLibComponent>();
        durationTime = jsonlib.GetChangeWaveDuraionTime();
    }

    //  这个是变小
    public void StartShrink()
    {
        waveGo.transform.DOMoveY(-1.0f, durationTime).SetEase(Ease.InSine);
        waveGo.transform.DOScale(0.5f, durationTime).SetEase(Ease.InSine);
    }


    public void StartNormal()
    {
        waveGo.transform.DOMoveY(0, durationTime).SetEase(Ease.InSine);
        waveGo.transform.DOScale(1.0f, durationTime).SetEase(Ease.InSine).OnComplete(() => {
            baffle.SetActive(false);
        });
    }

    // 创建一个挡板
    public void CreateBaffle( Object tmpBaffle )
    {
        baffle = (GameObject)GameObject.Instantiate(tmpBaffle);
        baffle.transform.parent = this.GameObject.transform;
        baffle.transform.localPosition = new Vector3(0, 1.5f, 0);
        baffle.transform.localScale = Vector3.one;
        baffle.transform.localRotation = Quaternion.identity;
    }

    public override void Dispose()
    {
        if (baffle != null)
        {
            GameObject.Destroy(baffle);
            baffle = null;
        }
    }
}
