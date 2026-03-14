using UnityEngine;

namespace JigSpace.Utilities
{
	public class MeshUtils
	{
		public static Vector2[] CreateUV(ref Mesh mesh)
		{
			Vector3 up = Vector3.up;
			Vector3 vector = Vector3.Cross(up, Vector3.forward);
			vector = ((!(Vector3.Dot(vector, vector) < 0.001f)) ? Vector3.Normalize(vector) : Vector3.right);
			Vector3.Normalize(Vector3.Cross(up, vector));
			Vector3[] vertices = mesh.vertices;
			int[] triangles = mesh.triangles;
			Vector2[] array = new Vector2[vertices.Length];
			for (int i = 0; i < triangles.Length; i += 3)
			{
				Vector3 vector2 = vertices[triangles[i]];
				Vector3 vector3 = vertices[triangles[i + 1]];
				Vector3 vector4 = vertices[triangles[i + 2]];
				Vector3 lhs = vector3 - vector2;
				Vector3 rhs = vector4 - vector2;
				Vector3 vector5 = Vector3.Cross(lhs, rhs);
				vector5 = new Vector3(Mathf.Abs(vector5.normalized.x), Mathf.Abs(vector5.normalized.y), Mathf.Abs(vector5.normalized.z));
				if (vector5.x > vector5.y && vector5.x > vector5.z)
				{
					array[triangles[i]] = new Vector2(vertices[triangles[i]].z, vertices[triangles[i]].y);
					array[triangles[i + 1]] = new Vector2(vertices[triangles[i + 1]].z, vertices[triangles[i + 1]].y);
					array[triangles[i + 2]] = new Vector2(vertices[triangles[i + 2]].z, vertices[triangles[i + 2]].y);
				}
				else if (vector5.y > vector5.x && vector5.y > vector5.z)
				{
					array[triangles[i]] = new Vector2(vertices[triangles[i]].x, vertices[triangles[i]].z);
					array[triangles[i + 1]] = new Vector2(vertices[triangles[i + 1]].x, vertices[triangles[i + 1]].z);
					array[triangles[i + 2]] = new Vector2(vertices[triangles[i + 2]].x, vertices[triangles[i + 2]].z);
				}
				else if (vector5.z > vector5.x && vector5.z > vector5.y)
				{
					array[triangles[i]] = new Vector2(vertices[triangles[i]].x, vertices[triangles[i]].y);
					array[triangles[i + 1]] = new Vector2(vertices[triangles[i + 1]].x, vertices[triangles[i + 1]].y);
					array[triangles[i + 2]] = new Vector2(vertices[triangles[i + 2]].x, vertices[triangles[i + 2]].y);
				}
			}
			mesh.uv = array;
			return array;
		}
	}
}
