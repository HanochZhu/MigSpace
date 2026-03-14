namespace OrbCreationExtensions
{
	public static class IntExtensions
	{
		public static string MakeString(this int[] anArray)
		{
			string text = "";
			if (anArray != null)
			{
				if (anArray.Length != 0)
				{
					text += anArray[0];
				}
				for (int i = 1; i < anArray.Length; i++)
				{
					if (anArray.Length == 0)
					{
						return text;
					}
					text = text + "," + anArray[i];
				}
			}
			return text;
		}

		public static bool ContentsEqualTo(this int[] anArray, int[] array2)
		{
			for (int i = 0; i < anArray.Length; i++)
			{
				if (array2.Length <= i || anArray[i] != array2[i])
				{
					return false;
				}
			}
			return true;
		}

		public static string MakeString(this int anInt)
		{
			return anInt.ToString() ?? "";
		}

		public static int MakeInt(this int anInt)
		{
			return anInt;
		}

		public static bool MakeBool(this int anInt)
		{
			return (float)anInt > 0f;
		}

		public static float MakeFloat(this int anInt)
		{
			return anInt;
		}

		public static double MakeDouble(this int anInt)
		{
			return anInt;
		}
	}
}
