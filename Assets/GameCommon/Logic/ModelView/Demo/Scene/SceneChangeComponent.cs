﻿using Cysharp.Threading.Tasks;
using UnityEngine;

namespace ET
{
    [ComponentOf(typeof(Scene))]
    public class SceneChangeComponent : Entity, IAwake, IUpdate, IDestroy
    {
        public AsyncOperation loadMapOperation;
        public UniTask tcs;
    }
}