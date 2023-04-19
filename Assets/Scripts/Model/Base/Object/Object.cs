using System.ComponentModel;

namespace ECSModel
{
	public abstract class Object: ISupportInitialize
	{
		public virtual void BeginInit()
		{
		}

		public virtual void EndInit()
		{
		}
	}
}