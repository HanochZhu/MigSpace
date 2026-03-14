namespace OrbCreationExtensions
{
	public static class BoolExtensions
	{
		public static string MakeString(this bool aBool)
		{
			if (aBool)
			{
				return "1";
			}
			return "0";
		}

		public static int MakeInt(this bool aBool)
		{
			if (aBool)
			{
				return 1;
			}
			return 0;
		}

		public static bool MakeBool(this bool aBool)
		{
			return aBool;
		}

		public static float MakeFloat(this bool aBool)
		{
			if (aBool)
			{
				return 1f;
			}
			return 0f;
		}

		public static double MakeDouble(this bool aBool)
		{
			if (aBool)
			{
				return 1.0;
			}
			return 0.0;
		}
	}
}
