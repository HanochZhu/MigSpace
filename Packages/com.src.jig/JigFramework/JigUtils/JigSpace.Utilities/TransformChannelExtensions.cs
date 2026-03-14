namespace JigSpace.Utilities
{
	public static class TransformChannelExtensions
	{
		public static bool HasFlag(this TransformChannel @enum, TransformChannel valueToContain)
		{
			return (@enum & valueToContain) == valueToContain;
		}
	}
}
