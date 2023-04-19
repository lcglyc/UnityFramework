using System;
namespace ECSModel
{
    public interface IMouseDown
    {
        Type Type();
        void Run(object o);
    }

    public abstract class MouseDownSystem<T> : IMouseDown
    {
        public void Run( object o)
        {
            this.OnMouseDwon((T)o);
        }

        public Type Type()
        {
            return typeof(T);
        }

        public abstract void OnMouseDwon(T self);
    }

}