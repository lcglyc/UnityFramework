using System;

namespace ECSModel
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public class ObjectSystemAttribute: BaseAttribute
	{
	}
}