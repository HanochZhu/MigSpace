using System;

public static class Utility
{
	public static bool IsJigSpaceTenantId(int tenantId)
	{
		if (tenantId != 1)
		{
			return tenantId == -1;
		}
		return true;
	}

	public static string ConvertBytesToString(long bytes)
	{
		if ((double)bytes > Math.Pow(1024.0, 3.0))
		{
			double value = BytesToGB(bytes);
			value = Math.Round(value, 2);
			return ((value >= 0.0) ? value.ToString() : "-") + "GB";
		}
		int num = BytesToMegaBytes(bytes);
		return ((num >= 0) ? num.ToString() : "-") + "MB";
	}

	public static string ConvertMegaBytesToString(int megabytes, bool divideByThousand = false)
	{
		if (megabytes >= 1000)
		{
			double value = (divideByThousand ? ((float)megabytes / 1000f) : ((float)megabytes / 1024f));
			value = Math.Round(value, 2);
			return $"{value}GB";
		}
		return $"{megabytes}MB";
	}

	public static int BytesToMegaBytes(long bytes)
	{
		return (int)((double)bytes / Math.Pow(1024.0, 2.0));
	}

	public static int MegaBytesToBytes(long megaBytes)
	{
		return (int)((double)megaBytes * Math.Pow(1024.0, 2.0));
	}

	public static double BytesToGB(long bytes)
	{
		return (double)bytes / Math.Pow(1024.0, 3.0);
	}
}
