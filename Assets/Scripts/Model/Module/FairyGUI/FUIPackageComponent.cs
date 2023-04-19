using System.Collections.Generic;
using FairyGUI;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace ECSModel
{
    /// <summary>
    /// 管理所有UI Package
    /// </summary>
    public class FUIPackageComponent : Component
    {
        public const string FUI_PACKAGE_DIR = "Assets/Resources/FairyGUI";
        public const string FUI_RunTime_DIR = "FairyGUI";

        private readonly Dictionary<string, UIPackage> packages = new Dictionary<string, UIPackage>();

        public bool HasPackage(string type)
        {
            return packages.ContainsKey(type);
        }

        public void AddPackage(string type)
        {
            UIPackage uiPackage = null;
            if (Define.IsAsync)
            {
                string uiBundleDesName = $"{type}_fui".StringToAB();
                string uiBundleResName = type.StringToAB();
                ResourcesComponent resourcesComponent = Game.Scene.GetComponent<ResourcesComponent>();
                resourcesComponent.LoadBundle(uiBundleDesName);
                resourcesComponent.LoadBundle(uiBundleResName);

                AssetBundle desAssetBundle = resourcesComponent.GetAssetBundle(uiBundleDesName);
                AssetBundle resAssetBundle = resourcesComponent.GetAssetBundle(uiBundleResName);
                uiPackage = UIPackage.AddPackage(desAssetBundle, resAssetBundle);
            }
            else
            {
                string package = packageName(type);
                uiPackage = UIPackage.AddPackage(package);
            }

            this.packages.Add(type, uiPackage);
        }

        public async UniTask AddPackageAsync(string type)
        {
            if (Define.IsAsync)
            {
                string uiBundleDesName = $"{type}_fui".StringToAB();
                string uiBundleResName = type.StringToAB();
                ResourcesComponent resourcesComponent = Game.Scene.GetComponent<ResourcesComponent>();
                await resourcesComponent.LoadBundleAsync(uiBundleDesName);
                await resourcesComponent.LoadBundleAsync(uiBundleResName);

                AssetBundle desAssetBundle = resourcesComponent.GetAssetBundle(uiBundleDesName);
                AssetBundle resAssetBundle = resourcesComponent.GetAssetBundle(uiBundleResName);
                UIPackage uiPackage = UIPackage.AddPackage(desAssetBundle, resAssetBundle);
                this.packages.Add(type, uiPackage);
            }
            else
            {
                await UniTask.CompletedTask;
                string package = packageName(type);
                UIPackage uiPackage = UIPackage.AddPackage(package);
                this.packages.Add(type, uiPackage);
            }
        }

        public void RemovePackage(string type)
        {
            UIPackage package;
            if (packages.TryGetValue(type, out package))
            {
                var p = UIPackage.GetByName(package.name);

                if (p != null)
                {
                    UIPackage.RemovePackage(package.name);
                }

                packages.Remove(package.name);
            }

            if (Define.IsAsync)
            {
                string uiBundleDesName = $"{type}_fui".StringToAB();
                string uiBundleResName = type.StringToAB();
                Game.Scene.GetComponent<ResourcesComponent>().UnloadBundle(uiBundleDesName);
                Game.Scene.GetComponent<ResourcesComponent>().UnloadBundle(uiBundleResName);
            }

        }


        string packageName(string type)
        {
            return $"{FUI_PACKAGE_DIR}/{type}";
        }
    }

}