using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Effects/Post Effect Mask Renderer", -1)]
public class PostEffectMaskRenderer : MonoBehaviour
{
	private static class Uniforms
	{
		internal static readonly int _MainTex = Shader.PropertyToID("_MainTex");

		internal static readonly int _Color = Shader.PropertyToID("_Color");

		internal static readonly int _Extrude = Shader.PropertyToID("_Extrude");

		internal static readonly int _CullMode = Shader.PropertyToID("_CullMode");

		internal static readonly int _DepthComp = Shader.PropertyToID("_DepthComp");
	}

	public struct ChildMesh
	{
		public MeshFilter meshFilter;

		public SkinnedMeshRenderer skinnedMeshFilter;

		public Transform transform;
	}

	public struct ChildRenderer
	{
		public Renderer renderer;

		public Mesh cachedMesh;

		public int submeshCount
		{
			get
			{
				if (!(cachedMesh != null))
				{
					return 1;
				}
				return cachedMesh.subMeshCount;
			}
		}
	}

	public enum Mode
	{
		UseMeshes,
		UseRenderers,
		UseCustomMesh
	}

	[SerializeField]
	private PostEffectMask m_mask;

	public Mode mode;

	[SerializeField]
	private bool m_includeChildren = true;

	public Mesh customMesh;

	public readonly List<ChildRenderer> childRenderers = new List<ChildRenderer>();

	public readonly List<ChildMesh> childMeshes = new List<ChildMesh>();

	[Header("Options (Only when using meshes)")]
	public bool overrideGlobalOptions = true;

	[Range(0f, 1f)]
	public float opacity = 1f;

	public Vector3 scale = Vector3.one;

	public float extrude;

	[Tooltip("Only alpha channel is used")]
	public Texture texture;

	private PostEffectMask m_attachedMask;

	private MaterialPropertyBlock m_materialProps;

	private Transform m_transform;

	public PostEffectMask mask
	{
		get
		{
			return m_attachedMask;
		}
		set
		{
			m_mask = value;
			if (m_attachedMask != value)
			{
				if (m_attachedMask != null)
				{
					m_attachedMask.maskRenderers.Remove(this);
				}
				m_attachedMask = value;
				if (m_attachedMask != null)
				{
					m_attachedMask.maskRenderers.Add(this);
				}
			}
		}
	}

	public bool includeChildren
	{
		get
		{
			return m_includeChildren;
		}
		set
		{
			if (m_includeChildren != value)
			{
				m_includeChildren = value;
				UpdateChildren();
			}
		}
	}

	public new Transform transform
	{
		get
		{
			if (!(m_transform == null))
			{
				return m_transform;
			}
			return m_transform = base.transform;
		}
	}

	internal MaterialPropertyBlock materialProps
	{
		get
		{
			if (m_materialProps == null)
			{
				m_materialProps = new MaterialPropertyBlock();
			}
			m_materialProps.Clear();
			if (overrideGlobalOptions)
			{
				m_materialProps.SetColor(Uniforms._Color, new Color(1f, 1f, 1f, opacity));
				m_materialProps.SetFloat(Uniforms._Extrude, extrude);
				if (texture != null)
				{
					m_materialProps.SetTexture(Uniforms._MainTex, texture);
				}
			}
			return m_materialProps;
		}
	}

	private void Reset()
	{
		m_mask = Object.FindObjectOfType<PostEffectMask>();
		OnValidate();
	}

	private void Awake()
	{
		m_transform = GetComponent<Transform>();
		if (m_mask == null)
		{
			m_mask = Object.FindObjectOfType<PostEffectMask>();
		}
		if (m_mask != null)
		{
			m_attachedMask = m_mask;
			m_attachedMask.maskRenderers.Add(this);
		}
	}

	private void OnEnable()
	{
		UpdateChildren();
	}

	private void OnDestroy()
	{
		if (m_attachedMask != null)
		{
			m_attachedMask.maskRenderers.Remove(this);
		}
		m_attachedMask = null;
	}

	private void OnValidate()
	{
		mask = m_mask;
		UpdateChildren();
	}

	private void OnTransformChildrenChanged()
	{
		if (mode != Mode.UseCustomMesh)
		{
			UpdateChildren();
		}
	}

	public void UpdateChildren()
	{
		UpdateChildRenderers();
		UpdateChildMeshes();
	}

	public void UpdateChildRenderers()
	{
		childRenderers.Clear();
		FindRenderers(childRenderers, transform, m_includeChildren);
	}

	public void UpdateChildMeshes()
	{
		childMeshes.Clear();
		FindMeshes(childMeshes, transform, m_includeChildren);
	}

	private static void FindRenderers(ICollection<ChildRenderer> renderers, Transform go, bool includeChildren)
	{
		Renderer component = go.GetComponent<Renderer>();
		if (component != null)
		{
			renderers.Add(new ChildRenderer
			{
				renderer = component,
				cachedMesh = GetMesh(component)
			});
		}
		if (!includeChildren)
		{
			return;
		}
		for (int i = 0; i < go.childCount; i++)
		{
			Transform child = go.GetChild(i);
			if (child.GetComponent<PostEffectMaskRenderer>() == null)
			{
				FindRenderers(renderers, child, includeChildren);
			}
		}
	}

	private static void FindMeshes(ICollection<ChildMesh> meshes, Transform go, bool includeChildren)
	{
		ChildMesh mesh;
		if (TryGetMesh(go, out mesh))
		{
			meshes.Add(mesh);
		}
		if (!includeChildren)
		{
			return;
		}
		for (int i = 0; i < go.childCount; i++)
		{
			Transform child = go.GetChild(i);
			if (child.GetComponent<PostEffectMaskRenderer>() == null)
			{
				FindMeshes(meshes, child, includeChildren);
			}
		}
	}

	private static bool TryGetMesh(Transform go, out ChildMesh mesh)
	{
		mesh = default(ChildMesh);
		MeshFilter component = go.GetComponent<MeshFilter>();
		if (component != null)
		{
			mesh.meshFilter = component;
		}
		SkinnedMeshRenderer component2 = go.GetComponent<SkinnedMeshRenderer>();
		if (component2 != null)
		{
			mesh.skinnedMeshFilter = component2;
		}
		if (mesh.meshFilter != null || mesh.skinnedMeshFilter != null)
		{
			mesh.transform = go;
			return true;
		}
		return false;
	}

	private static Mesh GetMesh(Renderer renderer)
	{
		if (renderer is MeshRenderer)
		{
			MeshFilter component = renderer.GetComponent<MeshFilter>();
			if (!(component != null))
			{
				return null;
			}
			return component.sharedMesh;
		}
		if (renderer is SkinnedMeshRenderer)
		{
			return ((SkinnedMeshRenderer)renderer).sharedMesh;
		}
		if (renderer is ParticleSystemRenderer)
		{
			return ((ParticleSystemRenderer)renderer).mesh;
		}
		return null;
	}
}
