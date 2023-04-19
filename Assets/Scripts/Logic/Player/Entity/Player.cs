using System;
using UnityEngine;

namespace ECSModel
{
    [Serializable]
    public sealed class Player : Entity
    {
        [SerializeField]
        public long UnitId { get; set; }

        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }

            base.Dispose();
        }
    }
}