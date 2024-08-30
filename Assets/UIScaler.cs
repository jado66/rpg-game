using UnityEngine;
using UnityEngine.UI;

public class ResponsiveUIScaler : MonoBehaviour
{
    public CanvasScaler canvasScaler;
    public float mobileScaleFactor = 1.5f;
    public Vector2 desktopReferenceResolution = new Vector2(1920, 1080);
    public Vector2 mobilePortraitReferenceResolution = new Vector2(1080, 1920);
    public Vector2 mobileLandscapeReferenceResolution = new Vector2(1920, 1080);

    private bool isMobile;
    private bool isLandscape;

    void Start()
    {
        isMobile = IsMobileDevice();
        UpdateUIScale();
    }

    void Update()
    {
        // Check for orientation changes
        if (isMobile && isLandscape != IsLandscapeOrientation())
        {
            UpdateUIScale();
        }
    }

    void UpdateUIScale()
    {
        isLandscape = IsLandscapeOrientation();

        canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvasScaler.matchWidthOrHeight = 0.5f;

        if (isMobile)
        {
            // canvasScaler.referenceResolution = isLandscape ? mobileLandscapeReferenceResolution : mobilePortraitReferenceResolution;
            canvasScaler.scaleFactor = mobileScaleFactor;
        }
        else
        {
            canvasScaler.referenceResolution = desktopReferenceResolution;
        }
    }

    bool IsMobileDevice()
    {
        return true;
        // #if UNITY_WEBGL && !UNITY_EDITOR
        //     return Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android;
        // #else
        //     return false;
        // #endif
    }

    bool IsLandscapeOrientation()
    {
        return Screen.width > Screen.height;
    }
}