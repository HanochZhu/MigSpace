using System;
using JigSpace.Utilities;
using UnityEngine;

namespace JigSpace.Extensions
{
	public static class AudioClipExtensions
	{
		public enum AudioConversionFormat
		{
			wav
		}

		public static byte[] ToWav(this AudioClip clip, AudioConversionFormat audioConversionFormat = AudioConversionFormat.wav)
		{
			byte[] array = clip.ToByteArray();
			byte[] header = WavUtils.GetHeader(clip.frequency, clip.channels, clip.samples, array.Length);
			byte[] array2 = new byte[array.Length + header.Length];
			Buffer.BlockCopy(header, 0, array2, 0, header.Length);
			Buffer.BlockCopy(array, 0, array2, header.Length, array.Length);
			return array2;
		}

		public static byte[] ToByteArray(this AudioClip clip)
		{
			float[] array = new float[clip.samples];
			clip.GetData(array, 0);
			short[] array2 = new short[array.Length];
			byte[] array3 = new byte[array.Length * 2];
			int num = 32767;
			for (int i = 0; i < array.Length; i++)
			{
				array2[i] = (short)(array[i] * (float)num);
				BitConverter.GetBytes(array2[i]).CopyTo(array3, i * 2);
			}
			return array3;
		}
	}
}
