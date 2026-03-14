using UnityEngine;
using CameraOrbit.TutorialWizard;
using UnityEditor;

public class OrbitCameraDocumentation : TutorialWizard
{
    //required//////////////////////////////////////////////////////
    public string FolderPath = "camera-orbit/editor/";
    public NetworkImages[] m_ServerImages = new NetworkImages[]
    {
        new NetworkImages{Name = "img-0.png", Image = null},
        new NetworkImages{Name = "img-1.png", Image = null},
        new NetworkImages{Name = "img-2.png", Image = null},
        new NetworkImages{Name = "img-3.png", Image = null},
        new NetworkImages{Name = "img-4.png", Image = null},
    };
    public Steps[] AllSteps = new Steps[] {
    new Steps { Name = "Get Started", StepsLenght = 0 , DrawFunctionName = nameof(GetStartedDoc)},
    new Steps { Name = "Mobile", StepsLenght = 0, DrawFunctionName = nameof(UsageDoc) },
    new Steps { Name = "Buttons", StepsLenght = 0, DrawFunctionName = nameof(ButtonsDoc) },
    new Steps { Name = "Change Target", StepsLenght = 0, DrawFunctionName = nameof(ChangeTargetDoc) },
    new Steps { Name = "Properties", StepsLenght = 0, DrawFunctionName = nameof(PropertiesDoc) },
    new Steps { Name = "Zoom Effect", StepsLenght = 0, DrawFunctionName = nameof(ZoomEffectDoc) },
    };
    private readonly GifData[] AnimatedImages = new GifData[]
   {
        new GifData{ Path = "name.gif" },
   };

    public override void OnEnable()
    {
        base.OnEnable();
        base.Initizalized(m_ServerImages, AllSteps, FolderPath, AnimatedImages);
        Style.highlightColor = ("#3700B3").ToUnityColor();
        allowTextSuggestions = true;
    }

    public override void WindowArea(int window)
    {
        AutoDrawWindows();
    }
    //final required////////////////////////////////////////////////

    void GetStartedDoc()
    {
        DrawHyperlinkText("<b><size=16>Test</size></b>\nYou can test the orbit camera using the example scenes included in the asset located at <i>Assets -> Camera Orbit -> Example -> Scene->*</i>\n\n<b><size=16>Usage</size></b>\nIn order to use the orbit camera in your own scene, you can simply drag and drop the <link=asset:Assets/Camera Orbit/Content/Prefab/Orbit Camera.prefab>Orbit Camera</link> prefab in your scene hierarchy and set the target in the inspector of the orbit camera and then set up the properties as needed.");
    }

    void UsageDoc()
    {
        DrawText("<b><size=22>Setup camera for mobile</size></b>");
        DrawText("The Orbit Camera support touch input allowing using the camera on mobile devices or any touch screen device, setting up the orbit camera for touch input is really simple:\n \n1. in the Orbit Camera instance in your scene <i>(where <b>bl_CamerOrbit</b> is attached to)</i> add/attach the script <b>bl_OrbitTouch</b>.\n \n2. in the inspector of <i>bl_CamerOrbit</i> > check on the toggle <b>Is For Mobile</b>\n \n3. In the same script > set the property <b>Rotate Input Key</b> to <b>Mobile Touch</b>.\n \nThat's, you can also use the example scene included in the asset folder to see how the orbit camera is set up.");
        Space(10);
        DrawNote("The touch input rotation sensitivity maybe feels different from the mouse rotation sensitivity, so make sure to adjust the sensitivity in bl_OrbitCamera > <b>Input Sensitivity</b> until you get the desired result.");
        Space(10);
        DrawHorizontalSeparator();
        DrawText("<b><size=22>Mobile Buttons</size></b>");
        DrawText("If you use UI buttons in the same scene where the orbit camera is set up, you may face the problem that when you interact with the UI buttons the orbit camera moves, in order to solve this you simply have to attach the script <b>bl_OrbitUIBlocker</b> in the UI Button or any UI element that you want to block the orbit camera movement.");
    }

    void ButtonsDoc()
    {
        DrawText("<b><size=22>Rotate camera with buttons</size></b>\n \nWhen you need to rotate the camera with UI buttons, you only need to do this:\n    \n   - Create the Buttons <i>(Horizontals and Verticals)</i>\n   - in these buttons add the script <b>bl_OrbitButton.cs</b>\n   - in the var \"Camera Orbit\" set the camera target to rotate <i>(with a <b>bl_CameraOrbit</b> script attached to the gameObject).</i>\n   - Setting the directional Axis that you need to rotate.\n   - Where amount of Horizontal is < 0 = Right, > 0 is Left and in Vertical < 0 is down , > 0 is up.\n   - Ready!.");
    }

    void ChangeTargetDoc()
    {
        DrawText("<b><size=22>Change Target In Runtime</size></b>\n \nIf you want to change the orbit camera target in-runtime, the asset makes this possible with a smooth transition, you can even define the position and rotation where the camera will be located once change of target.\n \nAll you need to do is:\n \n1. In the game object that the orbit camera will change to in-runtime > add/attach the script <b>bl_OrbitTargetPlaceholder.cs</b> > using the Scene View window you can preview <i>(with the green gizmos)</i> the position and rotation where the camera will transition to, you can modify with the angle, distance, and tilt values in the inspector of the script.\n \n2. From a script <i>(any script from which you want to make the transition happens)</i>, call the function of the Orbit Camera > <b>bl_CameraOrbit > SetTarget(...)</b> and pass the <i>bl_OrbitTargetPlaceholder</i> reference of the new orbit camera target, e.g:");
        DrawCodeText("using Lovatto.OrbitCamera;\nusing UnityEngine;\n \npublic class TestScript : MonoBehaviour\n{\n    public bl_CameraOrbit orbitCamera;\n    public bl_OrbitTargetPlaceholder nextTarget;\n \n    public void ChangeOrbitTarget()\n    {\n        orbitCamera.SetTarget(nextTarget);\n    }\n}");
    }

    void PropertiesDoc()
    {
        DrawText("<b><size=22>bl_CameraOrbit Properties</size></b>");
        DrawPropertieInfo("Target", "Transform", "The target where the camera will rotate around.");
        DrawPropertieInfo("Target Offset", "Vector3", "The offset position from the target position.");
        DrawPropertieInfo("Executed In Edit Mode", "bool", "Simulate the orbit camera movement in the editor (not play mode)?");
        DrawPropertieInfo("Is For Mobile", "bool", "Is the camera going to be use for a mobile platform?");
        DrawPropertieInfo("Required Input To Rotate", "bool", "Does the user require to keep pressed the mouse to rotate the camera?");
        DrawPropertieInfo("Rotate Input Key", "enum", "The input that will be used to rotate the camera.");
        DrawPropertieInfo("Lock Cursor On Rotate", "bool", "Hide the mouse while the camera is rotating?");
        DrawPropertieInfo("Use Keyboard Axis", "bool", "Allow rotate the camera with the keyboard arrows?");
        DrawPropertieInfo("Movement Type", "enum", "Method to apply the camera rotation, this affect how the rotation feels.");
        DrawPropertieInfo("Inertia Speed", "float", "The inertia of the rotation after the player stop rotating the camera.");
        DrawPropertieInfo("Auto Rotation Side", "enum", "The direction where the camera will automatically rotate once it stop rotating with input.");
        DrawPropertieInfo("Use Zoom Out Effect", "bool", "Use the zoom in start effect.");
        DrawPropertieInfo("Zoom Out Amount", "float", "The camera zoom/fov where the effect will start from.");
    }

    void ZoomEffectDoc()
    {
        DrawText("<b><size=20>Remove the Zoom effect on start</size></b>\n \nIn the scene demo you will see a <i>zoom</i> effect on the start, this is a feature but if you don't want use it, simply uncheck the toggle <b>Use Start Zoom Effect</b> in <b>bl_CameraOrbit inspector.</b>\n\n<b><size=20>Remove the Zoom in Out when click and rotate</size></b>\n \nFor default <i>(in the example scene and prefab)</i> when you click to rotate the camera there are a small zoom effect, if you\nwant remove it simply uncheck the toggle <b>Use Zoom Out Effect</b> in <b>bl_CameraOrbit inspector.</b>\nthis will remove the effect.");
    }

    [MenuItem("Window/Documentation/Camera Orbit")]
    static void Open()
    {
        GetWindow<OrbitCameraDocumentation>();
    }
}