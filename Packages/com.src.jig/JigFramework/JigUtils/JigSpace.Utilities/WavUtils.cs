using System;
using System.Text;

namespace JigSpace.Utilities
{
	public static class WavUtils
	{
		public static ushort WAV_HEADER_SIZE = 44;

		public static byte[] GetHeader(int frequency, int channels, int samples, int dataLenght)
		{
			byte[] array = new byte[44];
			Encoding.UTF8.GetBytes("RIFF").CopyTo(array, 0);
			BitConverter.GetBytes(dataLenght + WAV_HEADER_SIZE - 8).CopyTo(array, 4);
			Encoding.UTF8.GetBytes("WAVE").CopyTo(array, 8);
			Encoding.UTF8.GetBytes("fmt ").CopyTo(array, 12);
			BitConverter.GetBytes(16).CopyTo(array, 16);
			BitConverter.GetBytes((ushort)1).CopyTo(array, 20);
			BitConverter.GetBytes(channels).CopyTo(array, 22);
			BitConverter.GetBytes(frequency).CopyTo(array, 24);
			BitConverter.GetBytes(frequency * channels * 2).CopyTo(array, 28);
			BitConverter.GetBytes((ushort)(channels * 2)).CopyTo(array, 32);
			BitConverter.GetBytes((ushort)16).CopyTo(array, 34);
			Encoding.UTF8.GetBytes("data").CopyTo(array, 36);
			BitConverter.GetBytes(samples * channels * 2).CopyTo(array, 40);
			return array;
		}
	}
}
