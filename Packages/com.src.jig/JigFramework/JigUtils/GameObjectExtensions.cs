using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace JigSpace
{
	public static class GameObjectExtensions
	{
		public static Bounds GetTotalBounds(this GameObject go)
		{
			Vector3 position = go.transform.position;
			Quaternion rotation = Quaternion.identity;

            if (go.transform.parent != null)
			{
				rotation = go.transform.parent.rotation;
				go.transform.parent.rotation = Quaternion.Euler(0f, 0f, 0f);
			}
			Bounds result = new Bounds(position, Vector3.zero);
			MeshRenderer[] componentsInChildren = go.GetComponentsInChildren<MeshRenderer>(true);
			foreach (Renderer renderer in componentsInChildren)
			{
				if (!(renderer.gameObject.tag == "InnerParent"))
				{
					result.Encapsulate(renderer.bounds);
				}
			}
			result.center -= position;
			if (go.transform.parent != null)
			{
				go.transform.parent.rotation = rotation;
			}
			return result;
		}

		public static List<T> GetComponentsInChildrenOnly<T>(this GameObject parent, bool includeInactive = false, bool recursive = true)
		{
			List<T> list = new List<T>();
			if (parent != null)
			{
				foreach (Transform child in parent.transform)
				{
                    if (child.TryGetComponent<T>(out _) && (includeInactive || child.gameObject.activeInHierarchy))
                    {
                        list.AddRange(child.GetComponents<T>());
                    }
                    if (recursive && child.transform.childCount > 0)
                    {
                        list.AddRange(child.gameObject.GetComponentsInChildrenOnly<T>(includeInactive, recursive));
                    }
                }
			}
			return list;
		}

		public static bool IsInnerParent(this GameObject go)
		{
			return go.CompareTag("InnerParent");
		}

		public static string GetGameObjectTreePath(GameObject current, Transform stopSearchGO = null)
		{
			string result = current.name;
            Transform currentGO = current.transform.parent;
            while (currentGO != null && currentGO != stopSearchGO)
            {
                result = string.Join("/",currentGO.name, result);
                currentGO = currentGO.transform.parent;
            }
            return result;
        }
	}
}
