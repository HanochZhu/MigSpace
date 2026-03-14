public static class Events
{
	public static string EnvironmentChanged = "EnvironmentChanged";

	public static string ElementSelected = "ElementSelected";

	public static string ElementUnSelected = "ElementUnSelected";

	public static string SelectionChanged = "SelectionChanged";

	public static string MouseDownOnElement = "MouseDownOnElement";

	public static string ClickedInVoid = "ClickedInVoid";

	public static string ElementLongPressed = "ElementLongPressed";

	public static string JigLoaded = "JigLoaded";

	public static string UnloadingJig = "UnloadingJig";

	public static string JigSaved = "JigSaved";

	public static string JigDeleted = "JigDeleted";

	public static string JigPlaced = "JigPlaced";

	public const string ThumbnailCameraOpened = "ThumbnailCamereaOpened";

	public static string ThumbnailTaken = "ThumbnailTaken";

	public static string UploadThumbnailFinished = "UploadThumbnailFinished";

	public static string StageButtonUnSelected = "StageButtonUnSelected";

	public static string NewJigCreated = "NewJigCreated";

	public static string StageButtonSelected = "StageButtonSelected";

	public static string AdditionalModelLoaded = "AdditionalModelLoaded";

	public static string ColorChanged = "ColorChanged";

	public static string ElementVisibilityChanged = "ElementVisibilityChanged";

	public static string StageChanged = "StageChanged";

	public static string ParentChanged = "ParentChanged";

	public static string UserLoggedIn = "UserLoggedIn";

	public static string UserLoggedOut = "UserLoggedOut";

	public static string SignUpFailed = "SignUpFailed";

	public static string TenantDataRead = "TenantDataRead";

	public static string DownloadingJigListFailed = "DownloadingJigListFailed";

	public static string DownloadingJigFailed = "DownloadingJigFailed";

	public static string DownloadingModelFromStoreFailed = "DownloadingModelFromStoreFailed";

	public static string DownloadingLibraryFailed = "DownloadingLibraryFailed";

	public static string CannotConnect = "CannotConnect";

	public static string StateChanged = "StateChanged";

	public static string InvalidURLLoaded = "InvalidURLLoaded";

	public static string UploadDownloadProgressUpdate = "UploadDownloadProgressUpdate";

	public static string ImportStageSelected = "ImportStageSelected";

	public static string JigListDownloaded = "JigListDownloaded";

	public static string JigListPopulated = "JigListPopulated";

	public static string JigSavedToDevice = "JigSavedToDevice";

	public static string SavingJigToDeviceFailed = "SavingJigToDeviceFailed";

	public static string JigMetaDataUpdated = "JigMetaDataUpdated";

	public static string CategoryModified = "CategoryModified";

	public static string DoneUsingTool = "DoneUsingTool";

	public static string StartedUsingTool = "StartedUsingTool";

	public static string StartedSlidingOffScreen = "StartedSlidingOffScreen";

	public static string StartedSlidingOnScreen = "StartedSlidingOnScreen";

	public static string DoneSlidingIn = "DoneSlidingIn";

	public static string DoneSlidingOut = "DoneSlidingOut";

	public static string WorkshopVersionReceived = "WorkshopVersionReceived";

	public static string WorkshopVersionCheckFailed = "WorkshopVersionCheckFailed";

	public static string GoogleResponseReceived = "GoogleResponseReceived";

	public static string RequestedReposition = "RequestedReposition";

	public static string LeavingMainView = "LeavingMainView";

	public static string EnteredAR = "EnteredAR";

	public static string LeftAR = "LeftAR";

	public static string EnteredFlat = "EnteredFlat";

	public static string AppVersionCheckStarted = "AppVersionCheckStarted";

	public static string AppVersionCheckFinished = "AppVersionCheckFinished";

	public static string AppVersionSupported = "AppVersionSupported";

	public static string AppVersionGetLatest = "AppVersionGetLatest";

	public static string AppVersionExpiringSoon = "AppVersionExpiringSoon";

	public static string AppVersionExpired = "AppVersionExpired";

	public static string AppVersionUnknown = "AppVersionUnknown";

	public static string AccountManagerReady = "AccountManagerReady";

	public const string Undo = "Undo";

	public const string Redo = "Redo";

	public const string ObjectDeleted = "ObjectDeleted";

	public const string ObjectDuplicated = "ObjectDuplicated";

	public static string MoveModeStarted = "MoveModeStarted";

	public static string MoveModeEnded = "MoveModeEnded";

	public static string UserDidSignUp = "UserDidSignUp";

	public static string PackWasUnlocked = "PackWasUnlocked";

	public static string DoubleTapped = "DoubleTapped";

	public static string JigsCleared = "JigsCleared";

	public static string JigBeganLoading = "JigBeganLoading";

	public static string LibraryModelDeleted = "LibraryModelDeleted";

	public static string ModelLoaded = "ModelLoaded";

	public static string ModelSelected = "ModelSelected";

	public static string ModelStoreThumbnailDownloaded = "ModelStoreThumbnailDownloaded";

	public static string ModelCategorySelected = "ModelCategorySelected";

	public static string ResolutionChanged = "ResolutionChanged";

	public static string DeviceOrientationChanged = "DeviceOrientationChanged";

	public static string EnteringLoginPanel = "EnteringLoginPanel";

	public static string EnteringHomePanel = "EnteringHomePanel";

	public static string ExitingHomePanel = "ExitingHomePanel";

	public static string EnteringWorkspace = "EnteringWorkspace";

	public static string ExitingWorkSpace = "ExitingWorkSpace";

	public static string JigSettingsPanelOpened = "JigSettingsPanelOpened";

	public static string ModelStorePanelOpened = "ModelStorePanelOpened";

	public static string WasHidden = "WasHidden";

	public static string WasShown = "WasShown";

	public const string WorkshopThumbnailsLoaded = "WorkshopThumbnailsLoaded";

	public const string PreviewEffects = "PreviewEffectsChanged";

	public const string EffectAppliedOrChanged = "EffectChangedOnJig";

	public static string ConfirmedExitWorkspace = "ConfirmedExitWorkspace";

	public static string ConfirmDeleteJig = "ConfirmDeleteJig";

	public static string GrabStart = "GrabStart";

	public static string GrabEnd = "GraEnd";

	public static string GrabHighlightBegin = "GrabHighlightBegin";

	public static string GrabHighlightEnd = "GrabHighlightEnd";

	public static string GazeStart = "GazeStart";

	public static string GazeEnd = "GazeEnd";

	public static string JigMoveBegin = "JigMoveBegin";

	public static string JigMoveEnd = "JigMoveEnd";

	public static string JigRotateBegin = "JigRotateBegin";

	public static string JigRotateEnd = "JigRotateEnd";

	public static string JigGraphGrabStart = "JigGraphGrabStart";

	public static string JigGraphGrabEnd = "JigGraphGrabEnd";

	public static string TileSelected = "TileSelected";

	public static string TileUnselected = "TileSelected";

	public static string ReturnToGallery = "ReturnToGallery";

	public static string JigGraphLoaded = "JigGraphLoaded";

	public static string CompanionTalkBegin = "CompanionTalkBegin";

	public static string CompanionTalkEnd = "CompanionTalkEnd";

	public static string VideoRecordingStarted = "VideoRecordingStarted";

	public static string VideoRecordingEnded = "VideoRecordingEnded";

	public static string PencilInteractionDidTap = "PencilInteractionDidTap";

	public static string UnitsChanged = "UnitsChanged";

	public const string UserProfileAttributesSet = "UserProfileAttributesSet";

	public const string PullNotificationsUpdated = "PullNotificationsUpdated";


    public static string OnGizmosDragBegin = "OnGizmosDragBegin";

    public static string OnGizmosDragEnd = "OnGizmosDragEnd";

    public static string OnInputHitModel = "OnInputHitModel";


    public static string OnModelRefresh = "OnModelRefresh";

    public static string OnDeleteModel = "OnDeleteModel";

    public static string OnLoadHomeScene = "OnLoadHomeScene";

    public static string OnCameraViewClick = "OnCameraViewClick"; 

    public static string OnColorImagePointerUp = "OnColorImagePointerUp";

    public static string OnTreeListItemActive = "OnTreeListItemActive";


    public static string OnDeleteItems = "OnDeleteItems";

}
