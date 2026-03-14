using System;
using System.Runtime.InteropServices;

namespace JigSpace.Utilities
{
	public static class IntPtrUtils
	{
		public struct IntPtrStruct
		{
			public IntPtr Ptr;
		}

		public static void IntPtrToArrayOfType<T>(IntPtr unmanagedArray, int length, out T[] mangagedArray)
		{
			int num = Marshal.SizeOf(typeof(T));
			mangagedArray = new T[length];
			for (int i = 0; i < length; i++)
			{
				IntPtr ptr = new IntPtr(unmanagedArray.ToInt64() + i * num);
				mangagedArray[i] = (T)Marshal.PtrToStructure(ptr, typeof(T));
			}
		}
	}
}
