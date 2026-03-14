using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using cakeslice;
using Crosstales.Common.Util;
using Crosstales.FB;
using DG.Tweening;
using JigSpace;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : JigSingleton<GameManager>
{
	public enum AppView
	{
		Home,
		Edit,
		View
	}

	public enum Units
	{
		Metric,
		Imperial
	}

	public static readonly int TARGET_STANDARD_FRAME_RATE = 60;

	public static readonly int TARGET_AR_FRAME_RATE = 60;

	public static readonly int BACKGROUND_FRAME_RATE = 20;

	public int LastFrameRate;

	[Header("App Details")]
	public string AppName;

	public string AppShortName;

	public string Platform;

	[Header("Settings")]
	public float TweenSpeed = GlobalSettings.TweenSlow;

	public LayerMask IgnoreLayers;

	[SerializeField]
	private LayerMask _jigElementLayerMask;

	public DateTime FirstRunDate;

	[Header("States Debug")]
	public AppView CurrentView;

	public bool ViewOnly;

	public bool IsPickingColor;

	public bool IsReplacing;

	//[ReadOnly]
	public bool IsClickHolding;

	//[ReadOnly]
	public bool IsViewer;

	//[ReadOnly]
	public bool IsIpad;

	//[ReadOnly]
	public bool IsJigLoaded;

	//[ReadOnly]
	public bool IsJigSaved;

	//[ReadOnly]
	public bool IsModelLoaded;

	//[ReadOnly]
	public string CurrentTool;

	//[ReadOnly]
	public static bool IsNewJig;

	public bool IsViewingExampleJig;

	private static bool _previewEffects;

	[Header("Cameras")]
	public Camera MainCamera;

	public ThumbnailCameraController ThumbnailCamera;

	public Camera ScreenshotCamera;

	public Camera UICamera;

	[SerializeField]
	private bl_CameraOrbit _modelThumbnailCamera;

	[Header("Scene Objects")]
	public GameObject CentreLock;

	public GameObject RootParent;

	public Light DirectionalLight;


	public Vector3 RootParentScale;

	public List<GameObject> ShowcasedObjects;

	public GameObject PRFB_ElementDescriptionPanel;


	[TextArea]
	[Header("Other")]
	public string LoadingStage1String;

	[TextArea]
	public string LoadingStage2String;

	[TextArea]
	public string LoadingStage3String;

	[TextArea]
	public string LoadingStage4String;

	private int _lastOrbitSpeed;

	private bool _isParenting;

	private string _jigLoadedOrigin;

	private string _jigLoadedCampaign;

	private DateTime _becameInactiveDateTime;

	private static bool CanQuitFlag = false;

	public int LayerMask { get; private set; }

	public LayerMask JigElementLayerMask => _jigElementLayerMask;

	public int JigElementLayerMaskIndex => Mathf.RoundToInt(Mathf.Log(_jigElementLayerMask.value, 2f));

	public float KeyboardHeightPercent { get; private set; }

	public Units CurrentUnits { get; private set; }

	public bool IsMobile { get; private set; }

	public bool OpenWorkshopOnJigLoaded { get; private set; }

	public bool IsAR => false;

	public bool IsDesktop => true;

	public bool IsEditingJig
	{
		get
		{
			if (IsJigLoaded)
			{
				return !IsAR;
			}
			return false;
		}
	}

	public bool HasEnteredJig { get; set; }

	public bool WasJigTutorial { get; set; }

	public static bool ShowEffects
	{
		get
		{
			if (JigSingleton<GameManager>.Instance.IsEditingJig)
			{
				return _previewEffects;
			}
			return true;
		}
		set
		{
			if (_previewEffects != value)
			{
				_previewEffects = value;
			}
		}
	}

	public bl_CameraOrbit ModelThumbnailCamera => _modelThumbnailCamera;

	public bl_CameraOrbit CameraOrbit { get; private set; }

	public string CurrentDeviceType { get; private set; }



	public bool IsParenting => _isParenting;

	public void SetTutorialSeen()
	{
	}

	public override void Awake()
	{
		base.Awake();
		CheckCultureValidity();
		Application.targetFrameRate = TARGET_STANDARD_FRAME_RATE;
		LastFrameRate = Application.targetFrameRate;
		//JigSingleton<UIController>.Instance.MainCanvasController.HomePanelController.AppOptionsPanelController.EnvironmentDropDownController.SetValue((int)ServerIOCore.CurrentEnvironment);
		TouchScreenKeyboard.hideInput = true;
		LayerMask = ~(int)IgnoreLayers;
		CameraOrbit = MainCamera.GetComponent<bl_CameraOrbit>();
		SetKeyboardHeight();
		if (Application.isMobilePlatform)
		{
			IsMobile = true;
		}
		if (AnyIpad())
		{
			IsIpad = true;
		}
	}

	private void Start()
	{
		Screen.sleepTimeout = -1;
		TweenSpeed = GlobalSettings.TweenSlow;
		_lastOrbitSpeed = 10;
		MainCamera.depthTextureMode |= DepthTextureMode.Depth;
	}

	public void SetIsParenting(bool isParenting)
	{
		if (_isParenting != isParenting)
		{
			_isParenting = isParenting;
			//if (isParenting)
			//{
			//	JigSingleton<UIController>.Instance.MainCanvasController.AdvicePanelController.Show(Deserialize.DeserializeLiteral(new byte[128]
			//	{
			//		89, 98, 85, 90, 89, 9, 139, 36, 2, 238,
			//		125, 13, 56, 87, 242, 32, 107, 151, 13, 201,
			//		113, 36, 138, 40, 5, 168, 86, 10, 112, 228,
			//		150, 142, 60, 176, 167, 252, 106, 91, 1, 101,
			//		8, 148, 64, 72, 204, 93, 53, 221, 162, 159,
			//		81, 198, 17, 223, 142, 7, 13, 55, 97, 21,
			//		48, 241, 189, 226, 29, 200, 99, 187, 6, 149,
			//		24, 200, 22, 192, 185, 101, 117, 109, 0, 116,
			//		224, 207, 73, 160, 193, 198, 73, 95, 255, 87,
			//		187, 113, 159, 201, 50, 55, 99, 64, 15, 126,
			//		80, 223, 215, 147, 1, 223, 90, 8, 106, 186,
			//		10, 120, 228, 6, 143, 105, 9, 83, 43, 207,
			//		240, 29, 80, 159, 15, 63, 179, 205
			//	}, new byte[128]
			//	{
			//		10, 7, 57, 63, 58, 125, 171, 71, 106, 135,
			//		17, 105, 214, 198, 125, 202, 149, 96, 7, 222,
			//		131, 130, 216, 178, 211, 228, 76, 253, 198, 85,
			//		101, 60, 56, 153, 251, 22, 156, 140, 37, 91,
			//		78, 212, 209, 113, 204, 24, 158, 16, 93, 238,
			//		193, 37, 96, 95, 167, 189, 109, 249, 146, 30,
			//		241, 151, 97, 91, 121, 177, 131, 13, 1, 182,
			//		194, 208, 251, 83, 154, 65, 26, 102, 146, 31,
			//		51, 26, 247, 91, 129, 134, 239, 135, 165, 79,
			//		115, 70, 211, 65, 48, 163, 82, 152, 208, 198,
			//		92, 110, 133, 242, 223, 76, 199, 255, 57, 39,
			//		22, 142, 199, 250, 143, 118, 5, 86, 8, 66,
			//		251, 172, 140, 220, 10, 28, 215, 23
			//	}));
			//}
			//else
			//{
			//	JigSingleton<UIController>.Instance.MainCanvasController.AdvicePanelController.Hide();
			//}
		}
	}

	private void CheckCultureValidity()
	{
	}

	public void SetUnits(Units units, bool updatePlayerPref)
	{
	}

	private void SetKeyboardHeight()
	{
		KeyboardHeightPercent = 0f;
	}

	public bool IPadSupportsAR()
	{
		return false;
	}

	public static bool RequestReview()
	{
		return false;
	}

	public static void ShowPostSaveHints()
	{
		//if (JigSingleton<UIController>.Instance.MainCanvasController.JigSettingsPanelController.IsVisible)
		//{
		//	JigSingleton<HintManager>.Instance?.SetTarget(Deserialize.DeserializeLiteral(new byte[128]
		//	{
		//		59, 95, 132, 102, 154, 77, 174, 152, 133, 226,
		//		37, 240, 180, 163, 21, 178, 173, 92, 163, 18,
		//		3, 208, 5, 242, 241, 120, 228, 243, 249, 212,
		//		9, 47, 118, 226, 193, 129, 211, 139, 80, 134,
		//		223, 219, 125, 100, 42, 79, 210, 86, 198, 67,
		//		246, 38, 214, 207, 109, 85, 197, 113, 88, 170,
		//		76, 93, 185, 246, 224, 254, 205, 142, 27, 73,
		//		155, 211, 220, 24, 115, 60, 99, 131, 88, 242,
		//		170, 243, 134, 49, 87, 176, 223, 249, 55, 125,
		//		182, 113, 110, 201, 204, 208, 177, 81, 230, 29,
		//		145, 38, 83, 140, 8, 69, 80, 61, 74, 223,
		//		210, 13, 127, 193, 64, 214, 207, 135, 180, 235,
		//		215, 236, 215, 142, 89, 41, 42, 53
		//	}, new byte[128]
		//	{
		//		72, 55, 229, 20, 255, 7, 199, 255, 107, 115,
		//		170, 204, 161, 92, 17, 80, 120, 27, 244, 232,
		//		111, 65, 0, 221, 56, 169, 147, 28, 229, 228,
		//		84, 144, 119, 1, 224, 194, 196, 87, 99, 145,
		//		65, 124, 100, 36, 7, 95, 140, 115, 7, 228,
		//		136, 120, 245, 27, 53, 60, 121, 117, 171, 141,
		//		252, 97, 176, 170, 202, 83, 190, 112, 112, 106,
		//		181, 12, 104, 198, 114, 124, 8, 223, 64, 168,
		//		36, 19, 199, 84, 216, 7, 208, 137, 175, 186,
		//		62, 230, 202, 196, 146, 10, 215, 121, 0, 163,
		//		91, 212, 19, 36, 128, 67, 140, 57, 222, 250,
		//		19, 150, 78, 123, 177, 246, 231, 109, 59, 133,
		//		34, 123, 249, 19, 64, 120, 15, 87
		//	}), JigSingleton<UIController>.Instance.MainCanvasController.JigSettingsPanelController.ShareButton.gameObject);
		//	JigSingleton<HintManager>.Instance?.Show(Deserialize.DeserializeLiteral(new byte[128]
		//	{
		//		67, 208, 220, 88, 129, 228, 249, 254, 120, 231,
		//		33, 97, 127, 87, 75, 74, 222, 168, 49, 158,
		//		107, 18, 77, 175, 27, 100, 205, 4, 172, 81,
		//		126, 91, 172, 4, 92, 224, 244, 147, 140, 145,
		//		246, 87, 54, 55, 126, 215, 197, 109, 105, 165,
		//		161, 72, 189, 172, 173, 215, 106, 167, 131, 126,
		//		242, 182, 240, 233, 244, 7, 235, 78, 200, 228,
		//		106, 12, 240, 231, 227, 131, 153, 37, 14, 93,
		//		198, 4, 152, 248, 98, 66, 205, 169, 162, 38,
		//		185, 211, 43, 247, 173, 216, 128, 152, 120, 202,
		//		35, 93, 27, 195, 48, 107, 249, 85, 231, 255,
		//		128, 201, 169, 39, 32, 40, 246, 87, 91, 184,
		//		150, 231, 127, 241, 42, 45, 192, 101
		//	}, new byte[128]
		//	{
		//		48, 184, 189, 42, 228, 174, 144, 153, 150, 118,
		//		174, 137, 8, 224, 104, 14, 28, 188, 174, 234,
		//		112, 9, 222, 182, 140, 8, 103, 248, 201, 159,
		//		53, 200, 72, 183, 235, 199, 216, 120, 7, 18,
		//		58, 185, 171, 61, 27, 149, 225, 114, 221, 130,
		//		196, 193, 199, 231, 196, 26, 4, 223, 221, 123,
		//		39, 204, 213, 213, 120, 146, 226, 145, 49, 74,
		//		166, 51, 29, 192, 106, 128, 66, 160, 121, 57,
		//		253, 146, 15, 178, 200, 169, 236, 54, 78, 102,
		//		63, 180, 27, 165, 18, 244, 190, 232, 20, 206,
		//		230, 248, 155, 198, 104, 253, 207, 122, 250, 71,
		//		166, 199, 0, 35, 115, 143, 55, 225, 103, 193,
		//		54, 34, 81, 130, 44, 207, 199, 235
		//	}));
		//}
		//else
		//{
		//	JigSingleton<HintManager>.Instance?.Show(Deserialize.DeserializeLiteral(new byte[128]
		//	{
		//		223, 187, 207, 217, 139, 107, 59, 184, 67, 27,
		//		46, 8, 177, 186, 5, 11, 211, 31, 174, 136,
		//		158, 90, 39, 46, 231, 187, 90, 25, 201, 129,
		//		16, 217, 36, 224, 44, 14, 226, 153, 163, 142,
		//		85, 195, 110, 17, 224, 216, 238, 2, 121, 213,
		//		221, 113, 91, 113, 19, 171, 210, 140, 110, 89,
		//		127, 241, 83, 243, 12, 28, 176, 94, 161, 144,
		//		206, 94, 144, 196, 147, 107, 140, 103, 44, 72,
		//		102, 92, 13, 106, 201, 119, 65, 88, 11, 175,
		//		158, 164, 31, 110, 133, 182, 189, 32, 212, 80,
		//		241, 135, 169, 148, 37, 249, 52, 137, 186, 52,
		//		115, 200, 78, 133, 115, 13, 41, 66, 57, 81,
		//		36, 235, 142, 109, 124, 33, 150, 110
		//	}, new byte[128]
		//	{
		//		181, 210, 168, 144, 229, 13, 84, 86, 210, 148,
		//		247, 228, 187, 180, 227, 182, 3, 3, 255, 162,
		//		110, 63, 230, 120, 255, 3, 14, 30, 199, 214,
		//		17, 109, 98, 192, 117, 110, 175, 198, 167, 112,
		//		103, 67, 42, 202, 223, 17, 34, 248, 94, 35,
		//		52, 50, 78, 49, 73, 166, 43, 136, 70, 25,
		//		125, 123, 150, 75, 228, 70, 22, 243, 1, 97,
		//		101, 0, 85, 66, 60, 189, 221, 193, 47, 107,
		//		90, 177, 170, 83, 137, 92, 126, 4, 216, 66,
		//		109, 96, 91, 101, 194, 220, 154, 157, 227, 55,
		//		65, 215, 80, 227, 28, 171, 182, 74, 235, 246,
		//		49, 1, 121, 207, 214, 166, 47, 143, 97, 232,
		//		19, 111, 45, 115, 24, 224, 44, 128
		//	}));
		//}
	}

	public void ClearCamera()
	{
	}

	public void PrintDebug(string message)
	{
	}

	public void StopPickingColor()
	{
		IsPickingColor = false;
	}

	public void StartPickingColor()
	{
		IsPickingColor = true;
	}

	public void StartReplacing()
	{
		IsReplacing = true;
	}

	public void StopReplacing()
	{
		IsReplacing = false;
	}

	public void ApplyScreenshot(GameObject target, Action callback = null)
	{
		ScreenshotCamera.gameObject.SetActive(true);
	}

	public void OrbitAroundJig()
	{
		
	}

	public string GetUniqueJigName(string proposedName)
	{
		return proposedName;
	}

	public void SaveAs()
	{
	}


	private void OnSaveFileComplete(bool selected, string file)
	{
		if (selected)
		{
			if (file.IsNullOrEmpty())
			{
				return;
			}
		}
		Singleton<FileBrowser>.Instance.OnSaveFileComplete -= OnSaveFileComplete;
	}

	public void SaveJig(Action<bool> callback = null, bool quiet = false)
	{
		CheckBeforeUpdatingFormatVersion(delegate(bool updateFormatVersion)
		{
			GameManager gameManager = this;
			bool updateFormatVersion2 = updateFormatVersion;
		});
	}

	public void SaveAndQuit(Action callback = null)
	{
		CheckBeforeUpdatingFormatVersion(delegate(bool updateFormatVersion)
		{
		});
	}

	public void CheckBeforeUpdatingFormatVersion(Action<bool> callback)
	{
		
		{
			callback?.Invoke(true);
		}
	}

	public void DeleteJigIfEmpty(string jigId)
	{
		
	}

	public void LogOut()
	{
		if (CurrentView == AppView.Edit)
		{
		}
	}

	private void FinishLogout()
	{
	}

	public void ExitWorkspace(Action callback = null)
	{
		JigSingleton<GameManager>.Instance.StartCoroutine(CRExitWorkspace(callback));
	}

	private IEnumerator CRExitWorkspace(Action callback = null)
	{
		if (IsJigLoaded)
		{
			UnloadJig();
		}
		//JigSingleton<UIController>.Instance.HideWorkspaceUI();
		EventManager.TriggerEvent(Events.ExitingWorkSpace);
		//JigSingleton<UIController>.Instance.MainCanvasController.HomePanelController.Show();
		//CameraOrbit.ResetCamera();
		Camera.main.GetComponent<OutlineEffect>().enabled = false;
		callback?.Invoke();
		yield return null;
	}

	public void UnloadJig()
	{
		CentreLock.transform.localScale = new Vector3(1f, 1f, 1f);
		IsJigLoaded = false;
		if (!JigSingleton<GameManager>.Instance.ViewOnly)
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
		}
		EventManager.TriggerEvent(Events.UnloadingJig);
		//JigSingleton<UIController>.Instance.DescriptionCanvasController.HiddenLabels.Clear();
		if (RootParent != null)
		{
			MeshRenderer[] componentsInChildren = RootParent.GetComponentsInChildren<MeshRenderer>();
			for (int j = 0; j < componentsInChildren.Length; j++)
			{
				UnityEngine.Object.Destroy(componentsInChildren[j].material);
			}
			UnityEngine.Object.Destroy(RootParent);
		}
		JigUtilities.UnloadUnusedAssets();
	}

	public void CreateJig(string origin, string campaign = "Unknown")
	{
	}

	public void LoadJig(string id, int version = 0, bool useLocalBackup = false, bool viewOnly = false, bool fromLink = false, string loadOrigin = "HeroPanel", string campaign = "Unknown", Action onJigLoaded = null)
	{
		OpenWorkshopOnJigLoaded = false;
		if (fromLink)
		{
		}
		else
		{
			_jigLoadedOrigin = loadOrigin;
		}
		_jigLoadedCampaign = campaign;
		SetCameraBackgroundColor(GlobalSettings.DefaultBackgroundColor);
		ViewOnly = viewOnly;
		if (!viewOnly)
		{
			if (CurrentView == AppView.Home)
			{
				CurrentView = AppView.Edit;
				//JigSingleton<UIController>.Instance.MainCanvasController.HomePanelController.OpenWorkShopUI();
			}
			else if (CurrentView == AppView.View)
			{
				OpenWorkshopOnJigLoaded = true;
			}
			if (useLocalBackup)
			{
				return;
			}
			if (CurrentView == AppView.Edit)
			{
				
			}
			return;
		}
		
	}

	public void FitJigToScreen()
	{
		if (!(RootParent == null))
		{
			FitModelToScreen(RootParent);
		}
	}

	public void FitModelToScreen(GameObject modelGameObject)
	{
		FitModelToCamera(CameraOrbit, modelGameObject, 1.4f);
	}

	public void FitModelToCamera(bl_CameraOrbit camera, GameObject modelGameObject, float margin)
	{
		Bounds objectTotalBounds = JigUtilities.GetObjectTotalBounds(modelGameObject);
		//float distance = objectTotalBounds.extents.magnitude * margin / Mathf.Sin((float)Math.PI / 180f * camera.Camera.fieldOfView / 2f);
		//camera.SetDistance(distance);
		//camera.FocusOnWorldPoint(objectTotalBounds.center, false);
	}

	public bool ElementsAreDoneTweening()
	{
		List<Tween> list = DOTween.PlayingTweens();
		if (list == null)
		{
			return true;
		}
		foreach (Tween item in list)
		{
			if (item == null || item.target == null)
			{
				continue;
			}
			Material material = item.target as Material;
			if ((object)material != null)
			{
				continue;
			}
			Transform transform = item.target as Transform;
			if ((object)transform != null)
			{
				if (!(transform != null))
				{
					continue;
				}
			}
			bool flag = item.target is RectTransform;
		}
		return true;
	}

	public void StopAllLoops()
	{
		if (DOTween.PlayingTweens() == null)
		{
			return;
		}
		foreach (Tween item in from t in DOTween.PlayingTweens()
			where t.Loops() < 0
			select t)
		{
		}
	}

	public void CompleteAllTweens()
	{
	}

	public void ClearAllDescriptions()
	{
		
	}

	public void SetCameraBackgroundColor(Color color)
	{
		MainCamera.backgroundColor = color;
	}

	public void ResetCameraBackgroundColor()
	{
		Color backgroundColor = Color.white;// JigSingleton<DataManager>.Instance.JigData.JigSceneData.BackgroundColor;
		if (backgroundColor.a == 0f)
		{
			backgroundColor = GlobalSettings.DefaultBackgroundColor;
		}
		MainCamera.backgroundColor = backgroundColor;
	}

	public bool JigLoaded()
	{
		return IsJigLoaded;
	}

	public bool JigSaved()
	{
		return IsJigSaved;
	}

	public void DisplayJigCapacityFeatureGate()
	{
		
	}

	public void StartImportButtonClickedTimer()
	{
		return;
	}

	public void ForceLandscapeLeft()
	{
		Screen.orientation = ScreenOrientation.LandscapeLeft;
	}

	private IEnumerator ForceAndFixLandscape()
	{
		ScreenOrientation prev = ScreenOrientation.Portrait;
		for (int i = 0; i < 3; i++)
		{
			Debug.Log(Screen.orientation);
			Screen.orientation = ((prev != ScreenOrientation.Portrait) ? ScreenOrientation.Portrait : ScreenOrientation.LandscapeLeft);
			yield return new WaitWhile(() => prev == Screen.orientation);
			prev = Screen.orientation;
			yield return new WaitForSeconds(0.2f);
		}
	}

	public bool AnyIPhone()
	{
		return false;
	}

	public bool AnyIPod()
	{
		return false;
	}

	public bool IPhoneX()
	{
		return false;
	}

	public bool AnyIpad()
	{
		return false;
	}

	public bool IPadPro2018()
	{
		return false;
	}

	private void OnEnable()
	{
		EventManager.StartListening(Events.JigLoaded, OnJigLoaded);
		EventManager.StartListening(Events.ModelLoaded, OnModelLoaded);
		EventManager.StartListening(Events.NewJigCreated, OnJigCreated);
		EventManager.StartListening(Events.StageChanged, OnStageChanged);
		EventManager.StartListening(Events.UserLoggedIn, OnLoggedIn);
		EventManager.StartListening(Events.LeftAR, OnThumbnailCameraOpened);
		EventManager.StartListening(Events.EnteringWorkspace, OnEnteringWorkspace);
		EventManager.StartListening(Events.ExitingWorkSpace, OnExitingWorkspace);
	}

	private void OnDisable()
	{
		EventManager.StopListening(Events.JigLoaded, OnJigLoaded);
		EventManager.StopListening(Events.ModelLoaded, OnModelLoaded);
		EventManager.StopListening(Events.NewJigCreated, OnJigCreated);
		EventManager.StopListening(Events.StageChanged, OnStageChanged);
		EventManager.StopListening(Events.UserLoggedIn, OnLoggedIn);
		EventManager.StopListening(Events.LeftAR, OnThumbnailCameraOpened);
		EventManager.StopListening(Events.EnteringWorkspace, OnEnteringWorkspace);
		EventManager.StopListening(Events.ExitingWorkSpace, OnExitingWorkspace);
	}

	private void OnLoggedIn(object target, object data)
	{
		ClearCamera();
		
	}

	private void OnJigLoaded(object target, object data)
	{
		IsJigLoaded = true;
		FitJigToScreen();
		
		if (ViewOnly || CurrentView == AppView.View)
		{
			if (CurrentView != AppView.View)
			{
				//JigSingleton<UIController>.Instance.MainCanvasController.NavigationBarController.ToggleViewInARButton(true, false);
			}
		}
		else
		{
			ResetCameraBackgroundColor();
		}
		CameraOrbit.Interact = true;
		//CameraOrbit.ZoomEnabled = true;
		Camera.main.GetComponent<OutlineEffect>().enabled = true;
		WasJigTutorial = false;// JigSingleton<UIController>.Instance.TutoController.IsTutorial;
	}

	private void OnJigCreated(object target, object data)
	{
		IsJigLoaded = true;
		ViewOnly = false;
		CurrentView = AppView.Edit;
		OpenWorkshopOnJigLoaded = false;
		CentreLock.transform.localScale = Vector3.one;
		ResetCameraBackgroundColor();
		CameraOrbit.Interact = true;
		//CameraOrbit.ZoomEnabled = true;
		Camera.main.GetComponent<OutlineEffect>().enabled = true;
		WasJigTutorial = false;// JigSingleton<UIController>.Instance.TutoController.IsTutorial;
	}

	private void OnModelLoaded(object target, object data)
	{
		IsModelLoaded = true;
	}

	private void OnStageChanged(object target, object data)
	{
		SetIsParenting(false);
		StopPickingColor();
		StopReplacing();
	}

	private void OnThumbnailCameraOpened(object arg0, object arg1)
	{
		ShowEffects = false;
	}

	private void OnApplicationFocus(bool hasFocus)
	{
		if (hasFocus)
		{
			TimeSpan timeSpan = DateTime.Now.Subtract(_becameInactiveDateTime);
			Application.targetFrameRate = LastFrameRate;
			if (!IsEditingJig)
			{
				return;
			}
		}
		else
		{
			_becameInactiveDateTime = DateTime.Now;
			LastFrameRate = Application.targetFrameRate;
			Application.targetFrameRate = BACKGROUND_FRAME_RATE;
		}
	}

	//[RuntimeInitializeOnLoadMethod]
	private static void RunOnStart()
	{
		Application.wantsToQuit += JigSingleton<GameManager>.Instance.WantsToQuit;
	}

	private bool WantsToQuit()
	{
		if (CanQuitFlag || !IsEditingJig || ViewOnly)// || JigSingleton<UIController>.Instance.TutoController.IsTutorial)
		{
			return true;
		}
		return false;
	}

	private void OnSaveSucced()
	{
		CanQuitFlag = true;
		Application.Quit();
	}

	private void OnSaveFailed()
	{

	}

	private void OnEnteringWorkspace(object target, object data)
	{
		HasEnteredJig = true;
	}

	private void OnExitingWorkspace(object target, object data)
	{
	}
}
