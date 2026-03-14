using System;

namespace JigSpace.Extensions
{
	public static class ActionExtensions
	{
		public static void Trigger<TA>(this Action<TA> action, TA valueA)
		{
			action?.Invoke(valueA);
		}
	}
}
