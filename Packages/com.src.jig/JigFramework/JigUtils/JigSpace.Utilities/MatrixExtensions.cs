using UnityEngine;

namespace JigSpace.Utilities
{
	public static class MatrixExtensions
	{
		public static Vector3 ExtractTranslationFromMatrix(this Matrix4x4 matrix)
		{
			Vector3 result = default(Vector3);
			result.x = matrix.m03;
			result.y = matrix.m13;
			result.z = matrix.m23;
			return result;
		}

		public static Quaternion ExtractRotationFromMatrix(this Matrix4x4 matrix)
		{
			Vector3 forward = default(Vector3);
			forward.x = matrix.m02;
			forward.y = matrix.m12;
			forward.z = matrix.m22;
			Vector3 upwards = default(Vector3);
			upwards.x = matrix.m01;
			upwards.y = matrix.m11;
			upwards.z = matrix.m21;
			return Quaternion.LookRotation(forward, upwards);
		}

		public static Vector3 ExtractScaleFromMatrix(this Matrix4x4 matrix)
		{
			Vector3 result = default(Vector3);
			result.x = new Vector4(matrix.m00, matrix.m10, matrix.m20, matrix.m30).magnitude;
			result.y = new Vector4(matrix.m01, matrix.m11, matrix.m21, matrix.m31).magnitude;
			result.z = new Vector4(matrix.m02, matrix.m12, matrix.m22, matrix.m32).magnitude;
			return result;
		}

		private static void DoSetTransformFromMatrix(Transform transform, Matrix4x4 matrix)
		{
			transform.localPosition = matrix.ExtractTranslationFromMatrix();
			transform.localRotation = matrix.ExtractRotationFromMatrix();
			transform.localScale = matrix.ExtractScaleFromMatrix();
		}
	}
}
