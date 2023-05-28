// Decompiled with JetBrains decompiler
// Type: Minimap
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using Settings;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class Minimap : MonoBehaviour
{
  private bool assetsInitialized;
  private static Sprite borderSprite;
  private RectTransform borderT;
  private Canvas canvas;
  private Vector2 cornerPosition;
  private float cornerSizeRatio;
  private Minimap.Preset initialPreset;
  public static Minimap instance;
  private bool isEnabled;
  private bool isEnabledTemp;
  private Vector3 lastMinimapCenter;
  private float lastMinimapOrthoSize;
  private Camera lastUsedCamera;
  private bool maximized;
  private RectTransform minimap;
  private float MINIMAP_CORNER_SIZE;
  private float MINIMAP_CORNER_SIZE_SCALED;
  private Vector2 MINIMAP_ICON_SIZE;
  private float MINIMAP_POINTER_DIST;
  private float MINIMAP_POINTER_SIZE;
  private int MINIMAP_SIZE;
  private Vector2 MINIMAP_SUPPLY_SIZE;
  private Minimap.MinimapIcon[] minimapIcons;
  private bool minimapIsCreated;
  private RectTransform minimapMaskT;
  private Bounds minimapOrthographicBounds;
  public RenderTexture minimapRT;
  public Camera myCam;
  private static Sprite pointerSprite;
  private CanvasScaler scaler;
  private static Sprite supplySprite;
  private static Sprite whiteIconSprite;

  private void AddBorderToTexture(ref Texture2D texture, Color borderColor, int borderPixelSize)
  {
    int length = ((Texture) texture).width * borderPixelSize;
    Color[] colorArray = new Color[length];
    for (int index = 0; index < length; ++index)
      colorArray[index] = borderColor;
    texture.SetPixels(0, ((Texture) texture).height - borderPixelSize, ((Texture) texture).width - 1, borderPixelSize, colorArray);
    texture.SetPixels(0, 0, ((Texture) texture).width, borderPixelSize, colorArray);
    texture.SetPixels(0, 0, borderPixelSize, ((Texture) texture).height, colorArray);
    texture.SetPixels(((Texture) texture).width - borderPixelSize, 0, borderPixelSize, ((Texture) texture).height, colorArray);
    texture.Apply();
  }

  private void AutomaticSetCameraProperties(Camera cam)
  {
    Renderer[] objectsOfType = Object.FindObjectsOfType<Renderer>();
    if (objectsOfType.Length != 0)
    {
      this.minimapOrthographicBounds = new Bounds(((Component) objectsOfType[0]).transform.position, Vector3.zero);
      for (int index = 0; index < objectsOfType.Length; ++index)
      {
        if (((Component) objectsOfType[index]).gameObject.layer == 9)
          ((Bounds) ref this.minimapOrthographicBounds).Encapsulate(objectsOfType[index].bounds);
      }
    }
    Vector3 size = ((Bounds) ref this.minimapOrthographicBounds).size;
    float num = (double) size.x > (double) size.z ? size.x : size.z;
    size.z = size.x = num;
    ((Bounds) ref this.minimapOrthographicBounds).size = size;
    cam.orthographic = true;
    cam.orthographicSize = num * 0.5f;
    Vector3 center = ((Bounds) ref this.minimapOrthographicBounds).center;
    center.y = cam.farClipPlane * 0.5f;
    Transform transform = ((Component) cam).transform;
    transform.position = center;
    transform.eulerAngles = new Vector3(90f, 0.0f, 0.0f);
    cam.aspect = 1f;
    this.lastMinimapCenter = center;
    this.lastMinimapOrthoSize = cam.orthographicSize;
  }

  private void AutomaticSetOrthoBounds()
  {
    Renderer[] objectsOfType = Object.FindObjectsOfType<Renderer>();
    if (objectsOfType.Length != 0)
    {
      this.minimapOrthographicBounds = new Bounds(((Component) objectsOfType[0]).transform.position, Vector3.zero);
      for (int index = 0; index < objectsOfType.Length; ++index)
        ((Bounds) ref this.minimapOrthographicBounds).Encapsulate(objectsOfType[index].bounds);
    }
    Vector3 size = ((Bounds) ref this.minimapOrthographicBounds).size;
    float num = (double) size.x > (double) size.z ? size.x : size.z;
    size.z = size.x = num;
    ((Bounds) ref this.minimapOrthographicBounds).size = size;
    this.lastMinimapCenter = ((Bounds) ref this.minimapOrthographicBounds).center;
    this.lastMinimapOrthoSize = num * 0.5f;
  }

  private void Awake() => Minimap.instance = this;

  private Texture2D CaptureMinimap(Camera cam)
  {
    RenderTexture active = RenderTexture.active;
    RenderTexture.active = cam.targetTexture;
    cam.Render();
    Texture2D texture2D1 = new Texture2D(((Texture) cam.targetTexture).width, ((Texture) cam.targetTexture).height, (TextureFormat) 3, false);
    ((Texture) texture2D1).filterMode = (FilterMode) 1;
    Texture2D texture2D2 = texture2D1;
    texture2D2.ReadPixels(new Rect(0.0f, 0.0f, (float) ((Texture) cam.targetTexture).width, (float) ((Texture) cam.targetTexture).height), 0, 0);
    texture2D2.Apply();
    RenderTexture.active = active;
    return texture2D2;
  }

  private void CaptureMinimapRT(Camera cam)
  {
    RenderTexture active = RenderTexture.active;
    RenderTexture.active = this.minimapRT;
    cam.targetTexture = this.minimapRT;
    cam.Render();
    RenderTexture.active = active;
  }

  private void CheckUserInput()
  {
    if (SettingsManager.GeneralSettings.MinimapEnabled.Value && !SettingsManager.LegacyGameSettings.GlobalMinimapDisable.Value)
    {
      if (!this.minimapIsCreated)
        return;
      if (SettingsManager.InputSettings.General.MinimapMaximize.GetKey())
      {
        if (!this.maximized)
          this.Maximize();
      }
      else if (this.maximized)
        this.Minimize();
      if (SettingsManager.InputSettings.General.MinimapToggle.GetKeyDown())
        this.SetEnabled(!this.isEnabled);
      if (!this.maximized)
        return;
      bool flag = false;
      if (SettingsManager.InputSettings.General.MinimapReset.GetKey())
      {
        if (this.initialPreset != null)
          this.ManualSetCameraProperties(this.lastUsedCamera, this.initialPreset.center, this.initialPreset.orthographicSize);
        else
          this.AutomaticSetCameraProperties(this.lastUsedCamera);
        flag = true;
      }
      else
      {
        float axis = Input.GetAxis("Mouse ScrollWheel");
        if ((double) axis != 0.0)
        {
          if (Input.GetKey((KeyCode) 304))
            axis *= 3f;
          this.lastMinimapOrthoSize = Mathf.Max(this.lastMinimapOrthoSize + axis, 1f);
          flag = true;
        }
        if (Input.GetKey((KeyCode) 273))
        {
          this.lastMinimapCenter.z += Time.deltaTime * ((Input.GetKey((KeyCode) 304) ? 2f : 0.75f) * this.lastMinimapOrthoSize);
          flag = true;
        }
        else if (Input.GetKey((KeyCode) 274))
        {
          this.lastMinimapCenter.z -= Time.deltaTime * ((Input.GetKey((KeyCode) 304) ? 2f : 0.75f) * this.lastMinimapOrthoSize);
          flag = true;
        }
        if (Input.GetKey((KeyCode) 275))
        {
          this.lastMinimapCenter.x += Time.deltaTime * ((Input.GetKey((KeyCode) 304) ? 2f : 0.75f) * this.lastMinimapOrthoSize);
          flag = true;
        }
        else if (Input.GetKey((KeyCode) 276))
        {
          this.lastMinimapCenter.x -= Time.deltaTime * ((Input.GetKey((KeyCode) 304) ? 2f : 0.75f) * this.lastMinimapOrthoSize);
          flag = true;
        }
      }
      if (!flag)
        return;
      this.RecaptureMinimap(this.lastUsedCamera, this.lastMinimapCenter, this.lastMinimapOrthoSize);
    }
    else
    {
      if (!this.isEnabled)
        return;
      this.SetEnabled(!this.isEnabled);
    }
  }

  public void CreateMinimap(
    Camera cam,
    int minimapResolution = 512,
    float cornerSize = 0.3f,
    Minimap.Preset mapPreset = null)
  {
    if (!Minimap.Supported())
      return;
    this.isEnabled = true;
    this.lastUsedCamera = cam;
    if (!this.assetsInitialized)
      this.Initialize();
    GameObject gameObject = GameObject.Find("mainLight");
    Light light = (Light) null;
    Quaternion quaternion = Quaternion.identity;
    LightShadows lightShadows = (LightShadows) 0;
    Color color = Color.clear;
    float num = 0.0f;
    float nearClipPlane = cam.nearClipPlane;
    float farClipPlane = cam.farClipPlane;
    int cullingMask = cam.cullingMask;
    if (Object.op_Inequality((Object) gameObject, (Object) null))
    {
      light = gameObject.GetComponent<Light>();
      quaternion = ((Component) light).transform.rotation;
      lightShadows = light.shadows;
      num = light.intensity;
      color = light.color;
      light.shadows = (LightShadows) 0;
      light.color = Color.white;
      light.intensity = 0.5f;
      ((Component) light).transform.eulerAngles = new Vector3(90f, 0.0f, 0.0f);
    }
    cam.nearClipPlane = 0.3f;
    cam.farClipPlane = 1000f;
    cam.cullingMask = 512;
    cam.clearFlags = (CameraClearFlags) 2;
    this.MINIMAP_SIZE = minimapResolution;
    this.MINIMAP_CORNER_SIZE = (float) this.MINIMAP_SIZE * cornerSize;
    this.cornerSizeRatio = cornerSize;
    this.CreateMinimapRT(cam, minimapResolution);
    if (mapPreset != null)
    {
      this.initialPreset = mapPreset;
      this.ManualSetCameraProperties(cam, mapPreset.center, mapPreset.orthographicSize);
    }
    else
      this.AutomaticSetCameraProperties(cam);
    this.CaptureMinimapRT(cam);
    if (Object.op_Inequality((Object) gameObject, (Object) null))
    {
      light.shadows = lightShadows;
      ((Component) light).transform.rotation = quaternion;
      light.color = color;
      light.intensity = num;
    }
    cam.nearClipPlane = nearClipPlane;
    cam.farClipPlane = farClipPlane;
    cam.cullingMask = cullingMask;
    cam.orthographic = false;
    cam.clearFlags = (CameraClearFlags) 1;
    this.CreateUnityUIRT(minimapResolution);
    this.minimapIsCreated = true;
    this.StartCoroutine(this.HackRoutine());
  }

  private void CreateMinimapRT(Camera cam, int pixelSize)
  {
    if (Object.op_Equality((Object) this.minimapRT, (Object) null))
    {
      bool flag = SystemInfo.SupportsRenderTextureFormat((RenderTextureFormat) 4);
      RenderTextureFormat renderTextureFormat = flag ? (RenderTextureFormat) 4 : (RenderTextureFormat) 7;
      this.minimapRT = new RenderTexture(pixelSize, pixelSize, 16, (RenderTextureFormat) 4);
      if (!flag)
        Debug.Log((object) (SystemInfo.graphicsDeviceName + " (" + SystemInfo.graphicsDeviceVendor + ") does not support RGB565 format, the minimap will have transparency issues on certain maps"));
    }
    cam.targetTexture = this.minimapRT;
  }

  private void CreateUnityUI(Texture2D map, int minimapResolution)
  {
    GameObject gameObject1 = new GameObject("Canvas");
    gameObject1.AddComponent<RectTransform>();
    this.canvas = gameObject1.AddComponent<Canvas>();
    this.canvas.renderMode = (RenderMode) 0;
    this.scaler = gameObject1.AddComponent<CanvasScaler>();
    this.scaler.uiScaleMode = (CanvasScaler.ScaleMode) 1;
    this.scaler.referenceResolution = new Vector2(900f, 600f);
    this.scaler.screenMatchMode = (CanvasScaler.ScreenMatchMode) 0;
    GameObject gameObject2 = new GameObject("CircleMask");
    gameObject2.transform.SetParent(gameObject1.transform, false);
    this.minimapMaskT = gameObject2.AddComponent<RectTransform>();
    gameObject2.AddComponent<CanvasRenderer>();
    RectTransform minimapMaskT = this.minimapMaskT;
    Vector2 one;
    this.minimapMaskT.anchorMax = one = Vector2.one;
    Vector2 vector2_1 = one;
    minimapMaskT.anchorMin = vector2_1;
    float num = this.MINIMAP_CORNER_SIZE * 0.5f;
    this.cornerPosition = new Vector2((float) -((double) num + 5.0), (float) -((double) num + 70.0));
    this.minimapMaskT.anchoredPosition = this.cornerPosition;
    this.minimapMaskT.sizeDelta = new Vector2(this.MINIMAP_CORNER_SIZE, this.MINIMAP_CORNER_SIZE);
    GameObject gameObject3 = new GameObject(nameof (Minimap));
    gameObject3.transform.SetParent((Transform) this.minimapMaskT, false);
    this.minimap = gameObject3.AddComponent<RectTransform>();
    gameObject3.AddComponent<CanvasRenderer>();
    RectTransform minimap1 = this.minimap;
    RectTransform minimap2 = this.minimap;
    // ISSUE: explicit constructor call
    ((Vector2) ref one).\u002Ector(0.5f, 0.5f);
    Vector2 vector2_2 = one;
    minimap2.anchorMax = vector2_2;
    Vector2 vector2_3 = one;
    minimap1.anchorMin = vector2_3;
    this.minimap.anchoredPosition = Vector2.zero;
    this.minimap.sizeDelta = this.minimapMaskT.sizeDelta;
    Image image = gameObject3.AddComponent<Image>();
    Rect rect;
    // ISSUE: explicit constructor call
    ((Rect) ref rect).\u002Ector(0.0f, 0.0f, (float) ((Texture) map).width, (float) ((Texture) map).height);
    image.sprite = Sprite.Create(map, rect, Vector2.op_Implicit(new Vector3(0.5f, 0.5f)));
    image.type = (Image.Type) 0;
  }

  private void CreateUnityUIRT(int minimapResolution)
  {
    GameObject gameObject1 = new GameObject("Canvas");
    gameObject1.AddComponent<RectTransform>();
    this.canvas = gameObject1.AddComponent<Canvas>();
    this.canvas.renderMode = (RenderMode) 0;
    this.scaler = gameObject1.AddComponent<CanvasScaler>();
    this.scaler.uiScaleMode = (CanvasScaler.ScaleMode) 1;
    this.scaler.referenceResolution = new Vector2(800f, 600f);
    this.scaler.screenMatchMode = (CanvasScaler.ScreenMatchMode) 0;
    this.scaler.matchWidthOrHeight = 1f;
    GameObject gameObject2 = new GameObject("Mask");
    gameObject2.transform.SetParent(gameObject1.transform, false);
    this.minimapMaskT = gameObject2.AddComponent<RectTransform>();
    gameObject2.AddComponent<CanvasRenderer>();
    RectTransform minimapMaskT = this.minimapMaskT;
    Vector2 one;
    this.minimapMaskT.anchorMax = one = Vector2.one;
    Vector2 vector2_1 = one;
    minimapMaskT.anchorMin = vector2_1;
    float num = this.MINIMAP_CORNER_SIZE * 0.5f;
    this.cornerPosition = new Vector2((float) -((double) num + 5.0), (float) -((double) num + 70.0));
    this.minimapMaskT.anchoredPosition = this.cornerPosition;
    this.minimapMaskT.sizeDelta = new Vector2(this.MINIMAP_CORNER_SIZE, this.MINIMAP_CORNER_SIZE);
    GameObject gameObject3 = new GameObject("MapBorder");
    gameObject3.transform.SetParent((Transform) this.minimapMaskT, false);
    this.borderT = gameObject3.AddComponent<RectTransform>();
    RectTransform borderT1 = this.borderT;
    RectTransform borderT2 = this.borderT;
    // ISSUE: explicit constructor call
    ((Vector2) ref one).\u002Ector(0.5f, 0.5f);
    Vector2 vector2_2 = one;
    borderT2.anchorMax = vector2_2;
    Vector2 vector2_3 = one;
    borderT1.anchorMin = vector2_3;
    this.borderT.sizeDelta = this.minimapMaskT.sizeDelta;
    gameObject3.AddComponent<CanvasRenderer>();
    Image image = gameObject3.AddComponent<Image>();
    image.sprite = Minimap.borderSprite;
    image.type = (Image.Type) 1;
    GameObject gameObject4 = new GameObject(nameof (Minimap));
    gameObject4.transform.SetParent((Transform) this.minimapMaskT, false);
    this.minimap = gameObject4.AddComponent<RectTransform>();
    ((Transform) this.minimap).SetAsFirstSibling();
    gameObject4.AddComponent<CanvasRenderer>();
    RectTransform minimap1 = this.minimap;
    RectTransform minimap2 = this.minimap;
    // ISSUE: explicit constructor call
    ((Vector2) ref one).\u002Ector(0.5f, 0.5f);
    Vector2 vector2_4 = one;
    minimap2.anchorMax = vector2_4;
    Vector2 vector2_5 = one;
    minimap1.anchorMin = vector2_5;
    this.minimap.anchoredPosition = Vector2.zero;
    this.minimap.sizeDelta = this.minimapMaskT.sizeDelta;
    RawImage rawImage = gameObject4.AddComponent<RawImage>();
    rawImage.texture = (Texture) this.minimapRT;
    ((MaskableGraphic) rawImage).maskable = true;
    gameObject4.AddComponent<Mask>().showMaskGraphic = true;
  }

  private Vector2 GetSizeForStyle(Minimap.IconStyle style)
  {
    if (style == Minimap.IconStyle.CIRCLE)
      return this.MINIMAP_ICON_SIZE;
    return style == Minimap.IconStyle.SUPPLY ? this.MINIMAP_SUPPLY_SIZE : Vector2.zero;
  }

  private static Sprite GetSpriteForStyle(Minimap.IconStyle style)
  {
    if (style == Minimap.IconStyle.CIRCLE)
      return Minimap.whiteIconSprite;
    return style == Minimap.IconStyle.SUPPLY ? Minimap.supplySprite : (Sprite) null;
  }

  private IEnumerator HackRoutine()
  {
    yield return (object) new WaitForEndOfFrame();
    this.RecaptureMinimap(this.lastUsedCamera, this.lastMinimapCenter, this.lastMinimapOrthoSize);
  }

  private void Initialize()
  {
    Vector3 vector3;
    // ISSUE: explicit constructor call
    ((Vector3) ref vector3).\u002Ector(0.5f, 0.5f);
    Texture2D texture2D1 = (Texture2D) FengGameManagerMKII.RCassets.Load("icon");
    Rect rect;
    // ISSUE: explicit constructor call
    ((Rect) ref rect).\u002Ector(0.0f, 0.0f, (float) ((Texture) texture2D1).width, (float) ((Texture) texture2D1).height);
    Minimap.whiteIconSprite = Sprite.Create(texture2D1, rect, Vector2.op_Implicit(vector3));
    Texture2D texture2D2 = (Texture2D) FengGameManagerMKII.RCassets.Load("iconpointer");
    // ISSUE: explicit constructor call
    ((Rect) ref rect).\u002Ector(0.0f, 0.0f, (float) ((Texture) texture2D2).width, (float) ((Texture) texture2D2).height);
    Minimap.pointerSprite = Sprite.Create(texture2D2, rect, Vector2.op_Implicit(vector3));
    Texture2D texture2D3 = (Texture2D) FengGameManagerMKII.RCassets.Load("supplyicon");
    // ISSUE: explicit constructor call
    ((Rect) ref rect).\u002Ector(0.0f, 0.0f, (float) ((Texture) texture2D3).width, (float) ((Texture) texture2D3).height);
    Minimap.supplySprite = Sprite.Create(texture2D3, rect, Vector2.op_Implicit(vector3));
    Texture2D texture2D4 = (Texture2D) FengGameManagerMKII.RCassets.Load("mapborder");
    // ISSUE: explicit constructor call
    ((Rect) ref rect).\u002Ector(0.0f, 0.0f, (float) ((Texture) texture2D4).width, (float) ((Texture) texture2D4).height);
    Vector4 vector4;
    // ISSUE: explicit constructor call
    ((Vector4) ref vector4).\u002Ector(5f, 5f, 5f, 5f);
    Minimap.borderSprite = Sprite.Create(texture2D4, rect, Vector2.op_Implicit(vector3), 100f, 1U, (SpriteMeshType) 0, vector4);
    this.MINIMAP_ICON_SIZE = new Vector2((float) ((Texture) Minimap.whiteIconSprite.texture).width, (float) ((Texture) Minimap.whiteIconSprite.texture).height);
    this.MINIMAP_POINTER_SIZE = (float) (((Texture) Minimap.pointerSprite.texture).width + ((Texture) Minimap.pointerSprite.texture).height) / 2f;
    this.MINIMAP_POINTER_DIST = (float) (((double) this.MINIMAP_ICON_SIZE.x + (double) this.MINIMAP_ICON_SIZE.y) * 0.25);
    this.MINIMAP_SUPPLY_SIZE = new Vector2((float) ((Texture) Minimap.supplySprite.texture).width, (float) ((Texture) Minimap.supplySprite.texture).height);
    this.assetsInitialized = true;
  }

  private void ManualSetCameraProperties(Camera cam, Vector3 centerPoint, float orthoSize)
  {
    Transform transform = ((Component) cam).transform;
    centerPoint.y = cam.farClipPlane * 0.5f;
    transform.position = centerPoint;
    transform.eulerAngles = new Vector3(90f, 0.0f, 0.0f);
    cam.orthographic = true;
    cam.orthographicSize = orthoSize;
    float num = orthoSize * 2f;
    this.minimapOrthographicBounds = new Bounds(centerPoint, new Vector3(num, 0.0f, num));
    this.lastMinimapCenter = centerPoint;
    this.lastMinimapOrthoSize = orthoSize;
  }

  private void ManualSetOrthoBounds(Vector3 centerPoint, float orthoSize)
  {
    float num = orthoSize * 2f;
    this.minimapOrthographicBounds = new Bounds(centerPoint, new Vector3(num, 0.0f, num));
    this.lastMinimapCenter = centerPoint;
    this.lastMinimapOrthoSize = orthoSize;
  }

  public void Maximize()
  {
    this.isEnabledTemp = true;
    if (!this.isEnabled)
      this.SetEnabledTemp(true);
    RectTransform minimapMaskT1 = this.minimapMaskT;
    RectTransform minimapMaskT2 = this.minimapMaskT;
    Vector2 vector2_1;
    // ISSUE: explicit constructor call
    ((Vector2) ref vector2_1).\u002Ector(0.5f, 0.5f);
    Vector2 vector2_2 = vector2_1;
    minimapMaskT2.anchorMax = vector2_2;
    Vector2 vector2_3 = vector2_1;
    minimapMaskT1.anchorMin = vector2_3;
    this.minimapMaskT.anchoredPosition = Vector2.zero;
    this.minimapMaskT.sizeDelta = new Vector2((float) this.MINIMAP_SIZE, (float) this.MINIMAP_SIZE);
    this.minimap.sizeDelta = this.minimapMaskT.sizeDelta;
    this.borderT.sizeDelta = this.minimapMaskT.sizeDelta;
    if (this.minimapIcons != null)
    {
      for (int index = 0; index < this.minimapIcons.Length; ++index)
      {
        Minimap.MinimapIcon minimapIcon = this.minimapIcons[index];
        if (minimapIcon != null)
        {
          minimapIcon.SetSize(this.GetSizeForStyle(minimapIcon.style));
          if (minimapIcon.rotation)
            minimapIcon.SetPointerSize(this.MINIMAP_POINTER_SIZE, this.MINIMAP_POINTER_DIST);
        }
      }
    }
    this.maximized = true;
  }

  public void Minimize()
  {
    this.isEnabledTemp = false;
    if (!this.isEnabled)
      this.SetEnabledTemp(false);
    this.minimapMaskT.anchorMin = this.minimapMaskT.anchorMax = Vector2.one;
    this.minimapMaskT.anchoredPosition = this.cornerPosition;
    this.minimapMaskT.sizeDelta = new Vector2(this.MINIMAP_CORNER_SIZE, this.MINIMAP_CORNER_SIZE);
    this.minimap.sizeDelta = this.minimapMaskT.sizeDelta;
    this.borderT.sizeDelta = this.minimapMaskT.sizeDelta;
    if (this.minimapIcons != null)
    {
      float num = (float) (1.0 - ((double) this.MINIMAP_SIZE - (double) this.MINIMAP_CORNER_SIZE) / (double) this.MINIMAP_SIZE);
      float size = Mathf.Max(this.MINIMAP_POINTER_SIZE * num, this.MINIMAP_POINTER_SIZE * 0.5f);
      float originDistance = this.MINIMAP_POINTER_DIST * ((this.MINIMAP_POINTER_SIZE - size) / this.MINIMAP_POINTER_SIZE);
      for (int index = 0; index < this.minimapIcons.Length; ++index)
      {
        Minimap.MinimapIcon minimapIcon = this.minimapIcons[index];
        if (minimapIcon != null)
        {
          Vector2 sizeForStyle = this.GetSizeForStyle(minimapIcon.style);
          sizeForStyle.x = Mathf.Max(sizeForStyle.x * num, sizeForStyle.x * 0.5f);
          sizeForStyle.y = Mathf.Max(sizeForStyle.y * num, sizeForStyle.y * 0.5f);
          minimapIcon.SetSize(sizeForStyle);
          if (minimapIcon.rotation)
            minimapIcon.SetPointerSize(size, originDistance);
        }
      }
    }
    this.maximized = false;
  }

  public static void OnScreenResolutionChanged()
  {
    if (!Object.op_Inequality((Object) Minimap.instance, (Object) null) || !Minimap.Supported())
      return;
    Minimap instance = Minimap.instance;
    instance.StartCoroutine(instance.ScreenResolutionChangedRoutine());
  }

  private void RecaptureMinimap()
  {
    if (!Object.op_Inequality((Object) this.lastUsedCamera, (Object) null))
      return;
    this.RecaptureMinimap(this.lastUsedCamera, this.lastMinimapCenter, this.lastMinimapOrthoSize);
  }

  private void RecaptureMinimap(Camera cam, Vector3 centerPosition, float orthoSize)
  {
    if (!Object.op_Inequality((Object) this.minimap, (Object) null))
      return;
    GameObject gameObject = GameObject.Find("mainLight");
    Light light = (Light) null;
    Quaternion quaternion = Quaternion.identity;
    LightShadows lightShadows = (LightShadows) 0;
    Color color = Color.clear;
    float num = 0.0f;
    float nearClipPlane = cam.nearClipPlane;
    float farClipPlane = cam.farClipPlane;
    int cullingMask = cam.cullingMask;
    if (Object.op_Inequality((Object) gameObject, (Object) null))
    {
      light = gameObject.GetComponent<Light>();
      quaternion = ((Component) light).transform.rotation;
      lightShadows = light.shadows;
      color = light.color;
      num = light.intensity;
      light.shadows = (LightShadows) 0;
      light.color = Color.white;
      light.intensity = 0.5f;
      ((Component) light).transform.eulerAngles = new Vector3(90f, 0.0f, 0.0f);
    }
    cam.nearClipPlane = 0.3f;
    cam.farClipPlane = 1000f;
    cam.clearFlags = (CameraClearFlags) 2;
    cam.cullingMask = 512;
    this.CreateMinimapRT(cam, this.MINIMAP_SIZE);
    this.ManualSetCameraProperties(cam, centerPosition, orthoSize);
    this.CaptureMinimapRT(cam);
    if (Object.op_Inequality((Object) gameObject, (Object) null))
    {
      light.shadows = lightShadows;
      ((Component) light).transform.rotation = quaternion;
      light.color = color;
      light.intensity = num;
    }
    cam.nearClipPlane = nearClipPlane;
    cam.farClipPlane = farClipPlane;
    cam.cullingMask = cullingMask;
    cam.orthographic = false;
    cam.clearFlags = (CameraClearFlags) 1;
  }

  private IEnumerator ScreenResolutionChangedRoutine()
  {
    yield return (object) 0;
    this.RecaptureMinimap();
  }

  public void SetEnabled(bool enabled)
  {
    this.isEnabled = enabled;
    if (!Object.op_Inequality((Object) this.canvas, (Object) null))
      return;
    ((Component) this.canvas).gameObject.SetActive(enabled);
  }

  public void SetEnabledTemp(bool enabled)
  {
    if (!Object.op_Inequality((Object) this.canvas, (Object) null))
      return;
    ((Component) this.canvas).gameObject.SetActive(enabled);
  }

  public void TrackGameObjectOnMinimap(
    GameObject objToTrack,
    Color iconColor,
    bool trackOrientation,
    bool depthAboveAll = false,
    Minimap.IconStyle iconStyle = Minimap.IconStyle.CIRCLE)
  {
    if (!Object.op_Inequality((Object) this.minimap, (Object) null))
      return;
    Minimap.MinimapIcon minimapIcon = !trackOrientation ? Minimap.MinimapIcon.Create(this.minimap, objToTrack, iconStyle) : Minimap.MinimapIcon.CreateWithRotation(this.minimap, objToTrack, iconStyle, this.MINIMAP_POINTER_DIST);
    minimapIcon.SetColor(iconColor);
    minimapIcon.SetDepth(depthAboveAll);
    Vector2 sizeForStyle = this.GetSizeForStyle(iconStyle);
    if (this.maximized)
    {
      minimapIcon.SetSize(sizeForStyle);
      if (minimapIcon.rotation)
        minimapIcon.SetPointerSize(this.MINIMAP_POINTER_SIZE, this.MINIMAP_POINTER_DIST);
    }
    else
    {
      float num = (float) (1.0 - ((double) this.MINIMAP_SIZE - (double) this.MINIMAP_CORNER_SIZE) / (double) this.MINIMAP_SIZE);
      sizeForStyle.x = Mathf.Max(sizeForStyle.x * num, sizeForStyle.x * 0.5f);
      sizeForStyle.y = Mathf.Max(sizeForStyle.y * num, sizeForStyle.y * 0.5f);
      minimapIcon.SetSize(sizeForStyle);
      if (minimapIcon.rotation)
      {
        float size = Mathf.Max(this.MINIMAP_POINTER_SIZE * num, this.MINIMAP_POINTER_SIZE * 0.5f);
        float originDistance = this.MINIMAP_POINTER_DIST * ((this.MINIMAP_POINTER_SIZE - size) / this.MINIMAP_POINTER_SIZE);
        minimapIcon.SetPointerSize(size, originDistance);
      }
    }
    if (this.minimapIcons == null)
    {
      this.minimapIcons = new Minimap.MinimapIcon[1]
      {
        minimapIcon
      };
    }
    else
    {
      Minimap.MinimapIcon[] minimapIconArray = new Minimap.MinimapIcon[this.minimapIcons.Length + 1];
      for (int index = 0; index < this.minimapIcons.Length; ++index)
        minimapIconArray[index] = this.minimapIcons[index];
      minimapIconArray[minimapIconArray.Length - 1] = minimapIcon;
      this.minimapIcons = minimapIconArray;
    }
  }

  public static void TryRecaptureInstance()
  {
    if (!Object.op_Inequality((Object) Minimap.instance, (Object) null))
      return;
    Minimap.instance.RecaptureMinimap();
  }

  public IEnumerator TryRecaptureInstanceE(float time)
  {
    yield return (object) new WaitForSeconds(time);
    Minimap.TryRecaptureInstance();
  }

  private void Update()
  {
    this.CheckUserInput();
    if (!this.isEnabled && !this.isEnabledTemp || !this.minimapIsCreated || this.minimapIcons == null)
      return;
    for (int index = 0; index < this.minimapIcons.Length; ++index)
    {
      Minimap.MinimapIcon minimapIcon = this.minimapIcons[index];
      if (minimapIcon == null)
        RCextensions.RemoveAt<Minimap.MinimapIcon>(ref this.minimapIcons, index);
      else if (!minimapIcon.UpdateUI(this.minimapOrthographicBounds, this.maximized ? (float) this.MINIMAP_SIZE : this.MINIMAP_CORNER_SIZE))
      {
        minimapIcon.Destroy();
        RCextensions.RemoveAt<Minimap.MinimapIcon>(ref this.minimapIcons, index);
      }
    }
  }

  public static void WaitAndTryRecaptureInstance(float time) => Minimap.instance.StartCoroutine(Minimap.instance.TryRecaptureInstanceE(time));

  private static bool Supported() => Application.platform == 2;

  public enum IconStyle
  {
    CIRCLE,
    SUPPLY,
  }

  public class MinimapIcon
  {
    private Transform obj;
    private RectTransform pointerRect;
    public readonly bool rotation;
    public readonly Minimap.IconStyle style;
    private RectTransform uiRect;

    public MinimapIcon(GameObject trackedObject, GameObject uiElement, Minimap.IconStyle style)
    {
      this.rotation = false;
      this.style = style;
      this.obj = trackedObject.transform;
      this.uiRect = uiElement.GetComponent<RectTransform>();
      CatchDestroy component = ((Component) this.obj).GetComponent<CatchDestroy>();
      if (Object.op_Equality((Object) component, (Object) null))
        ((Component) this.obj).gameObject.AddComponent<CatchDestroy>().target = uiElement;
      else if (Object.op_Inequality((Object) component.target, (Object) null) && Object.op_Inequality((Object) component.target, (Object) uiElement))
        Object.Destroy((Object) component.target);
      else
        component.target = uiElement;
    }

    public MinimapIcon(
      GameObject trackedObject,
      GameObject uiElement,
      GameObject uiPointer,
      Minimap.IconStyle style)
    {
      this.rotation = true;
      this.style = style;
      this.obj = trackedObject.transform;
      this.uiRect = uiElement.GetComponent<RectTransform>();
      this.pointerRect = uiPointer.GetComponent<RectTransform>();
      CatchDestroy component = ((Component) this.obj).GetComponent<CatchDestroy>();
      if (Object.op_Equality((Object) component, (Object) null))
        ((Component) this.obj).gameObject.AddComponent<CatchDestroy>().target = uiElement;
      else if (Object.op_Inequality((Object) component.target, (Object) null) && Object.op_Inequality((Object) component.target, (Object) uiElement))
        Object.Destroy((Object) component.target);
      else
        component.target = uiElement;
    }

    public static Minimap.MinimapIcon Create(
      RectTransform parent,
      GameObject trackedObject,
      Minimap.IconStyle style)
    {
      Sprite spriteForStyle = Minimap.GetSpriteForStyle(style);
      GameObject uiElement = new GameObject(nameof (MinimapIcon));
      RectTransform rectTransform = uiElement.AddComponent<RectTransform>();
      rectTransform.anchorMin = rectTransform.anchorMax = Vector2.op_Implicit(new Vector3(0.5f, 0.5f));
      rectTransform.sizeDelta = new Vector2((float) ((Texture) spriteForStyle.texture).width, (float) ((Texture) spriteForStyle.texture).height);
      Image image = uiElement.AddComponent<Image>();
      image.sprite = spriteForStyle;
      image.type = (Image.Type) 0;
      uiElement.transform.SetParent((Transform) parent, false);
      return new Minimap.MinimapIcon(trackedObject, uiElement, style);
    }

    public static Minimap.MinimapIcon CreateWithRotation(
      RectTransform parent,
      GameObject trackedObject,
      Minimap.IconStyle style,
      float pointerDist)
    {
      Sprite spriteForStyle = Minimap.GetSpriteForStyle(style);
      GameObject uiElement = new GameObject(nameof (MinimapIcon));
      RectTransform rectTransform1 = uiElement.AddComponent<RectTransform>();
      rectTransform1.anchorMin = rectTransform1.anchorMax = Vector2.op_Implicit(new Vector3(0.5f, 0.5f));
      rectTransform1.sizeDelta = new Vector2((float) ((Texture) spriteForStyle.texture).width, (float) ((Texture) spriteForStyle.texture).height);
      Image image1 = uiElement.AddComponent<Image>();
      image1.sprite = spriteForStyle;
      image1.type = (Image.Type) 0;
      uiElement.transform.SetParent((Transform) parent, false);
      GameObject uiPointer = new GameObject("IconPointer");
      RectTransform rectTransform2 = uiPointer.AddComponent<RectTransform>();
      rectTransform2.anchorMin = rectTransform2.anchorMax = rectTransform1.anchorMin;
      rectTransform2.sizeDelta = new Vector2((float) ((Texture) Minimap.pointerSprite.texture).width, (float) ((Texture) Minimap.pointerSprite.texture).height);
      Image image2 = uiPointer.AddComponent<Image>();
      image2.sprite = Minimap.pointerSprite;
      image2.type = (Image.Type) 0;
      uiPointer.transform.SetParent((Transform) rectTransform1, false);
      rectTransform2.anchoredPosition = new Vector2(0.0f, pointerDist);
      return new Minimap.MinimapIcon(trackedObject, uiElement, uiPointer, style);
    }

    public void Destroy()
    {
      if (!Object.op_Inequality((Object) this.uiRect, (Object) null))
        return;
      Object.Destroy((Object) ((Component) this.uiRect).gameObject);
    }

    public void SetColor(Color color)
    {
      if (!Object.op_Inequality((Object) this.uiRect, (Object) null))
        return;
      ((Graphic) ((Component) this.uiRect).GetComponent<Image>()).color = color;
    }

    public void SetDepth(bool aboveAll)
    {
      if (!Object.op_Inequality((Object) this.uiRect, (Object) null))
        return;
      if (aboveAll)
        ((Transform) this.uiRect).SetAsLastSibling();
      else
        ((Transform) this.uiRect).SetAsFirstSibling();
    }

    public void SetPointerSize(float size, float originDistance)
    {
      if (!Object.op_Inequality((Object) this.pointerRect, (Object) null))
        return;
      this.pointerRect.sizeDelta = new Vector2(size, size);
      this.pointerRect.anchoredPosition = new Vector2(0.0f, originDistance);
    }

    public void SetSize(Vector2 size)
    {
      if (!Object.op_Inequality((Object) this.uiRect, (Object) null))
        return;
      this.uiRect.sizeDelta = size;
    }

    public bool UpdateUI(Bounds worldBounds, float minimapSize)
    {
      if (Object.op_Equality((Object) this.obj, (Object) null))
        return false;
      float x = ((Bounds) ref worldBounds).size.x;
      Vector3 vector3 = Vector3.op_Subtraction(this.obj.position, ((Bounds) ref worldBounds).center);
      vector3.y = vector3.z;
      vector3.z = 0.0f;
      float num1 = Mathf.Abs(vector3.x) / x;
      vector3.x = (double) vector3.x < 0.0 ? -num1 : num1;
      float num2 = Mathf.Abs(vector3.y) / x;
      vector3.y = (double) vector3.y < 0.0 ? -num2 : num2;
      this.uiRect.anchoredPosition = Vector2.op_Implicit(Vector3.op_Multiply(vector3, minimapSize));
      if (this.rotation)
        ((Transform) this.uiRect).eulerAngles = new Vector3(0.0f, 0.0f, (float) ((double) Mathf.Atan2(this.obj.forward.z, this.obj.forward.x) * 57.295780181884766 - 90.0));
      return true;
    }
  }

  public class Preset
  {
    public readonly Vector3 center;
    public readonly float orthographicSize;

    public Preset(Vector3 center, float orthographicSize)
    {
      this.center = center;
      this.orthographicSize = orthographicSize;
    }
  }
}
