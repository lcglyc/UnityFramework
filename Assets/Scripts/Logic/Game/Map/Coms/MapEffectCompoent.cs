using Cysharp.Threading.Tasks;
using ECSModel;
using UnityEngine;
using Component = ECSModel.Component;
using Object = UnityEngine.Object;

public class MapEffectCompoent : Component
{
    private GameObject mMapEffect;
    public async  UniTask LoadLevelEffectPrefabs( string abName,string resName)
    {
        ResourcesComponent rescom = Game.Scene.GetComponent<ResourcesComponent>();
        await  rescom.LoadBundleAsync(abName);
        mMapEffect  = MonoBehaviour.Instantiate(rescom.GetAsset(abName, resName)) as  GameObject;
        mMapEffect.transform.parent = Camera.main.transform;
        mMapEffect.transform.localPosition = Vector3.zero;
        mMapEffect.SetActive(false);
    }

    public void ShowEffect()
    {
        mMapEffect.SetActive(true);
    }
   
    
    public override void Dispose()
    {
        if (mMapEffect != null)
        {
            GameObject.Destroy(mMapEffect);
        }

        mMapEffect = null;
        base.Dispose();
    }
}
