using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[ImageEffectAllowedInSceneView]
[RequireComponent(typeof(Camera))]
[ExecuteInEditMode]
[AddComponentMenu("Effects/Post Effect Mask", -1)]
public class PostEffectMask : MonoBehaviour, ISerializationCallbackReceiver
{
	private static class Uniforms
	{
		internal static readonly int _MainTex = Shader.PropertyToID("_MainTex");

		internal static readonly int _Color = Shader.PropertyToID("_Color");

		internal static readonly int _ColorMask = Shader.PropertyToID("_ColorMask");

		internal static readonly int _CullMode = Shader.PropertyToID("_CullMode");

		internal static readonly int _DepthComp = Shader.PropertyToID("_DepthComp");

		internal static readonly int _SrcFactor = Shader.PropertyToID("_SrcFactor");

		internal static readonly int _DstFactor = Shader.PropertyToID("_DstFactor");

		internal static readonly int _BlurSize = Shader.PropertyToID("_BlurSize");

		internal static readonly int _BlurTempRT = Shader.PropertyToID("_BlurTempRT");

		internal static readonly int _Extrude = Shader.PropertyToID("_Extrude");
	}

	public enum MaskCaptureMode
	{
		BeforeOpaqueEffects,
		BeforeEffects
	}

	[Serializable]
	public class RendererOptions
	{
		[Range(0f, 1f)]
		public float opacity = 1f;

		public Vector3 scale = Vector3.one;

		public float extrude;

		[Tooltip("Only alpha channel is used")]
		public Texture texture;

		public bool depthTest = true;

		public CullMode cullMode;
	}

	[Range(0f, 1f)]
	public float opacity;

	public bool invert;

	[Range(0f, 20f)]
	public float blur;

	public MaskCaptureMode captureMode;

	public Texture fullScreenTexture;

	[Header("Global Renderer options")]
	public bool renderersEnabled = true;

	public RendererOptions globalRendererOptions = new RendererOptions();

	public HashSet<PostEffectMaskRenderer> maskRenderers = new HashSet<PostEffectMaskRenderer>();

	private Material m_alphaClearMaterial;

	private Material m_alphaBlendMaterial;

	private Material m_alphaBlurMaterial;

	private Material m_alphaWriteMaterial;

	private Camera m_camera;

	private CameraEvent m_attachedEvent;

	private CommandBuffer m_beforePostFX;

	private RenderTexture m_unprocessed;

	private void Awake()
	{
		m_camera = GetComponent<Camera>();
	}

	private void OnEnable()
	{
		CreateResources();
		m_attachedEvent = GetCBEvent();
		AddAsFirstCommandBuffer(m_camera, m_attachedEvent, m_beforePostFX);
	}

	private void OnDisable()
	{
		if (m_beforePostFX != null)
		{
			m_camera.RemoveCommandBuffer(m_attachedEvent, m_beforePostFX);
		}
		DisposeResources();
	}

	private CameraEvent GetCBEvent()
	{
		if (captureMode != 0)
		{
			return CameraEvent.BeforeImageEffects;
		}
		return CameraEvent.BeforeImageEffectsOpaque;
	}

	private void CreateResources()
	{
		m_beforePostFX = new CommandBuffer
		{
			name = "PostEffectMask"
		};
		Shader shader = Shader.Find("Unlit/Alpha");
		if (shader == null)
		{
			base.enabled = false;
			throw new NullReferenceException("Cannot find Unlit/Alpha shader.");
		}
		m_alphaClearMaterial = new Material(shader)
		{
			hideFlags = HideFlags.DontSave
		};
		m_alphaClearMaterial.SetInt(Uniforms._ColorMask, 1);
		m_alphaWriteMaterial = new Material(shader)
		{
			hideFlags = HideFlags.DontSave
		};
		m_alphaWriteMaterial.SetInt(Uniforms._ColorMask, 1);
		m_alphaWriteMaterial.SetInt(Uniforms._SrcFactor, 5);
		m_alphaWriteMaterial.SetInt(Uniforms._DstFactor, 10);
		m_alphaBlendMaterial = new Material(shader)
		{
			hideFlags = HideFlags.DontSave
		};
		Shader shader2 = Shader.Find("Hidden/Post FX/AlphaBlur");
		if (shader2 == null)
		{
			base.enabled = false;
			throw new NullReferenceException("Cannot find AlphaBlur shader.");
		}
		m_alphaBlurMaterial = new Material(shader2)
		{
			hideFlags = HideFlags.DontSave
		};
	}

	private void DisposeResources()
	{
		Destroy(m_alphaClearMaterial);
		Destroy(m_alphaWriteMaterial);
		Destroy(m_alphaBlendMaterial);
		Destroy(m_alphaBlurMaterial);
		if (m_unprocessed != null)
		{
			RenderTexture.ReleaseTemporary(m_unprocessed);
		}
		m_unprocessed = null;
		if (m_beforePostFX != null)
		{
			m_beforePostFX.Dispose();
		}
		m_beforePostFX = null;
	}

	private void OnPreCull()
	{
		if (m_unprocessed != null)
		{
			RenderTexture.ReleaseTemporary(m_unprocessed);
		}
		m_unprocessed = RenderTexture.GetTemporary(Screen.width, Screen.height, 0);
		m_unprocessed.name = "PostEffectMask_unprocessed";
		m_alphaClearMaterial.SetColor(Uniforms._Color, new Color(1f, 1f, 1f, opacity));
		m_alphaWriteMaterial.SetColor(Uniforms._Color, new Color(1f, 1f, 1f, globalRendererOptions.opacity));
		m_alphaWriteMaterial.SetFloat(Uniforms._Extrude, globalRendererOptions.extrude);
		m_alphaWriteMaterial.SetTexture(Uniforms._MainTex, globalRendererOptions.texture);
		CompareFunction value = (globalRendererOptions.depthTest ? CompareFunction.LessEqual : CompareFunction.Always);
		m_alphaWriteMaterial.SetInt(Uniforms._DepthComp, (int)value);
		m_alphaWriteMaterial.SetInt(Uniforms._CullMode, (int)globalRendererOptions.cullMode);
		CameraEvent cBEvent = GetCBEvent();
		if (cBEvent != m_attachedEvent)
		{
			m_camera.RemoveCommandBuffer(m_attachedEvent, m_beforePostFX);
			m_attachedEvent = cBEvent;
			AddAsFirstCommandBuffer(m_camera, m_attachedEvent, m_beforePostFX);
		}
	}

	private void OnPreRender()
	{
		m_beforePostFX.Clear();
		m_beforePostFX.Blit(null, BuiltinRenderTextureType.CurrentActive, m_alphaClearMaterial);
		if (fullScreenTexture != null)
		{
			m_beforePostFX.Blit(fullScreenTexture, BuiltinRenderTextureType.CurrentActive, m_alphaWriteMaterial);
		}
		if (renderersEnabled)
		{
			foreach (PostEffectMaskRenderer maskRenderer in maskRenderers)
			{
				if (maskRenderer == null || !maskRenderer.isActiveAndEnabled)
				{
					continue;
				}
				switch (maskRenderer.mode)
				{
				case PostEffectMaskRenderer.Mode.UseMeshes:
				{
					Matrix4x4 matrix4x = Matrix4x4.Scale(maskRenderer.overrideGlobalOptions ? maskRenderer.scale : globalRendererOptions.scale);
					MaterialPropertyBlock materialProps2 = maskRenderer.materialProps;
					foreach (PostEffectMaskRenderer.ChildMesh childMesh in maskRenderer.childMeshes)
					{
						Matrix4x4 matrix2 = childMesh.transform.localToWorldMatrix * matrix4x;
						if (childMesh.meshFilter != null)
						{
							Mesh sharedMesh = childMesh.meshFilter.sharedMesh;
							if (sharedMesh != null)
							{
								for (int k = 0; k < sharedMesh.subMeshCount; k++)
								{
									m_beforePostFX.DrawMesh(sharedMesh, matrix2, m_alphaWriteMaterial, k, -1, materialProps2);
								}
							}
						}
						if (!(childMesh.skinnedMeshFilter != null))
						{
							continue;
						}
						Mesh sharedMesh2 = childMesh.skinnedMeshFilter.sharedMesh;
						if (sharedMesh2 != null)
						{
							for (int l = 0; l < sharedMesh2.subMeshCount; l++)
							{
								m_beforePostFX.DrawMesh(sharedMesh2, matrix2, m_alphaWriteMaterial, l, -1, materialProps2);
							}
						}
					}
					break;
				}
				case PostEffectMaskRenderer.Mode.UseRenderers:
					foreach (PostEffectMaskRenderer.ChildRenderer childRenderer in maskRenderer.childRenderers)
					{
						if (childRenderer.renderer.enabled)
						{
							for (int j = 0; j < childRenderer.submeshCount; j++)
							{
								m_beforePostFX.DrawRenderer(childRenderer.renderer, m_alphaWriteMaterial, j);
							}
						}
					}
					break;
				case PostEffectMaskRenderer.Mode.UseCustomMesh:
				{
					Mesh customMesh = maskRenderer.customMesh;
					if (customMesh != null)
					{
						Matrix4x4 matrix = maskRenderer.transform.localToWorldMatrix * Matrix4x4.Scale(maskRenderer.overrideGlobalOptions ? maskRenderer.scale : globalRendererOptions.scale);
						MaterialPropertyBlock materialProps = maskRenderer.materialProps;
						for (int i = 0; i < customMesh.subMeshCount; i++)
						{
							m_beforePostFX.DrawMesh(customMesh, matrix, m_alphaWriteMaterial, i, -1, materialProps);
						}
					}
					break;
				}
				}
			}
		}
		m_beforePostFX.Blit(BuiltinRenderTextureType.CurrentActive, m_unprocessed);
		if (blur > 1E-06f)
		{
			m_beforePostFX.GetTemporaryRT(Uniforms._BlurTempRT, m_unprocessed.width, m_unprocessed.height, 0);
			m_alphaBlurMaterial.SetFloat(Uniforms._BlurSize, blur);
			m_beforePostFX.Blit(m_unprocessed, Uniforms._BlurTempRT, m_alphaBlurMaterial, 0);
			m_beforePostFX.Blit(Uniforms._BlurTempRT, m_unprocessed, m_alphaBlurMaterial, 1);
			m_beforePostFX.ReleaseTemporaryRT(Uniforms._BlurTempRT);
		}
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		BlendMode value = ((!invert) ? BlendMode.DstAlpha : BlendMode.OneMinusDstAlpha);
		BlendMode value2 = ((!invert) ? BlendMode.OneMinusDstAlpha : BlendMode.DstAlpha);
		m_alphaBlendMaterial.SetInt(Uniforms._SrcFactor, (int)value);
		m_alphaBlendMaterial.SetInt(Uniforms._DstFactor, (int)value2);
		Graphics.Blit(source, m_unprocessed, m_alphaBlendMaterial);
		source = m_unprocessed;
		Graphics.Blit(source, destination);
		RenderTexture.ReleaseTemporary(m_unprocessed);
		m_unprocessed = null;
	}

	public void OnBeforeSerialize()
	{
	}

	public void OnAfterDeserialize()
	{
	}

	private static void AddAsFirstCommandBuffer(Camera cam, CameraEvent evt, CommandBuffer buffer)
	{
		CommandBuffer[] commandBuffers = cam.GetCommandBuffers(evt);
		cam.RemoveCommandBuffers(evt);
		cam.AddCommandBuffer(evt, buffer);
		CommandBuffer[] array = commandBuffers;
		foreach (CommandBuffer buffer2 in array)
		{
			cam.AddCommandBuffer(evt, buffer2);
		}
	}

	private new static void Destroy(UnityEngine.Object obj)
	{
		if (obj != null)
		{
			UnityEngine.Object.Destroy(obj);
		}
	}
}
