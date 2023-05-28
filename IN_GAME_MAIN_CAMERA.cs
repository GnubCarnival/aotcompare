// Decompiled with JetBrains decompiler
// Type: IN_GAME_MAIN_CAMERA
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using ApplicationManagers;
using Constants;
using Settings;
using System.Collections;
using UI;
using UnityEngine;
using Weather;

internal class IN_GAME_MAIN_CAMERA : MonoBehaviour
{
  public IN_GAME_MAIN_CAMERA.RotationAxes axes;
  public AudioSource bgmusic;
  public static float cameraDistance = 0.6f;
  public static CAMERA_TYPE cameraMode;
  public static int character = 1;
  private float closestDistance;
  private int currentPeekPlayerIndex;
  private float decay;
  public static int difficulty;
  private float distance = 10f;
  private float distanceMulti;
  private float distanceOffsetMulti;
  private float duration;
  private float flashDuration;
  private bool flip;
  public static GAMEMODE gamemode;
  public bool gameOver;
  public static GAMETYPE gametype = GAMETYPE.STOP;
  private bool hasSnapShot;
  private Transform head;
  private float height = 5f;
  private float heightDamping = 2f;
  private float heightMulti;
  public static bool isCheating;
  public static bool isTyping;
  public float justHit;
  public int lastScore;
  public static int level;
  private bool lockAngle;
  private Vector3 lockCameraPosition;
  private GameObject locker;
  private GameObject lockTarget;
  public GameObject main_object;
  public float maximumX = 360f;
  public float maximumY = 60f;
  public float minimumX = -360f;
  public float minimumY = -60f;
  public static bool needSetHUD;
  private float R;
  private float rotationY;
  public int score;
  public static string singleCharacter;
  public Material skyBoxDAWN;
  public Material skyBoxDAY;
  public Material skyBoxNIGHT;
  public GameObject snapShotCamera;
  public RenderTexture snapshotRT;
  public bool spectatorMode;
  public static STEREO_3D_TYPE stereoType;
  private Transform target;
  public Texture texture;
  public float timer;
  public static bool triggerAutoLock;
  public static bool usingTitan;
  private Vector3 verticalHeightOffset = Vector3.zero;
  private float verticalRotationOffset;
  private float xSpeed = -3f;
  private float ySpeed = -0.8f;
  public static IN_GAME_MAIN_CAMERA Instance;
  private Transform _transform;
  private UILabel _bottomRightText;
  private static float _lastRestartTime = 0.0f;

  private void Awake()
  {
    this.Cache();
    IN_GAME_MAIN_CAMERA.Instance = this;
    IN_GAME_MAIN_CAMERA.isTyping = false;
    GameMenu.TogglePause(false);
    ((Object) this).name = "MainCamera";
    IN_GAME_MAIN_CAMERA.ApplyGraphicsSettings();
    this.CreateMinimap();
    WeatherManager.TakeFlashlight(((Component) this).transform);
  }

  public static void ApplyGraphicsSettings()
  {
    Camera main = Camera.main;
    GraphicsSettings graphicsSettings = SettingsManager.GraphicsSettings;
    if (graphicsSettings == null || !Object.op_Inequality((Object) main, (Object) null))
      return;
    main.farClipPlane = (float) graphicsSettings.RenderDistance.Value;
    if (!FengGameManagerMKII.level.StartsWith("Custom"))
      ((Behaviour) ((Component) main).GetComponent<TiltShift>()).enabled = graphicsSettings.BlurEnabled.Value;
    else
      ((Behaviour) ((Component) main).GetComponent<TiltShift>()).enabled = false;
  }

  private void Cache() => this._transform = ((Component) this).transform;

  private void camareMovement()
  {
    Camera camera = ((Component) this).camera;
    this.distanceOffsetMulti = (float) ((double) IN_GAME_MAIN_CAMERA.cameraDistance * (200.0 - (double) camera.fieldOfView) / 150.0);
    this._transform.position = Object.op_Equality((Object) this.head, (Object) null) ? this.main_object.transform.position : this.head.position;
    Transform transform1 = this._transform;
    transform1.position = Vector3.op_Addition(transform1.position, Vector3.op_Multiply(Vector3.up, this.heightMulti));
    Transform transform2 = this._transform;
    transform2.position = Vector3.op_Subtraction(transform2.position, Vector3.op_Multiply(Vector3.op_Multiply(Vector3.up, 0.6f - IN_GAME_MAIN_CAMERA.cameraDistance), 2f));
    float sensitivity = SettingsManager.GeneralSettings.MouseSpeed.Value;
    int num1 = SettingsManager.GeneralSettings.InvertMouse.Value ? -1 : 1;
    if (GameMenu.InMenu())
      sensitivity = 0.0f;
    switch (IN_GAME_MAIN_CAMERA.cameraMode)
    {
      case CAMERA_TYPE.ORIGINAL:
        if ((double) Input.mousePosition.x < (double) Screen.width * 0.40000000596046448)
          this._transform.RotateAround(this._transform.position, Vector3.up, (float) (-(((double) Screen.width * 0.40000000596046448 - (double) Input.mousePosition.x) / (double) Screen.width * 0.40000000596046448) * (double) this.getSensitivityMultiWithDeltaTime(sensitivity) * 150.0));
        else if ((double) Input.mousePosition.x > (double) Screen.width * 0.60000002384185791)
          this._transform.RotateAround(this._transform.position, Vector3.up, (float) (((double) Input.mousePosition.x - (double) Screen.width * 0.60000002384185791) / (double) Screen.width * 0.40000000596046448 * (double) this.getSensitivityMultiWithDeltaTime(sensitivity) * 150.0));
        float num2 = (float) (140.0 * ((double) Screen.height * 0.60000002384185791 - (double) Input.mousePosition.y) / (double) Screen.height * 0.5);
        Transform transform3 = this._transform;
        double num3 = (double) num2;
        Quaternion rotation1 = this._transform.rotation;
        double y = (double) ((Quaternion) ref rotation1).eulerAngles.y;
        rotation1 = this._transform.rotation;
        double z = (double) ((Quaternion) ref rotation1).eulerAngles.z;
        Quaternion quaternion = Quaternion.Euler((float) num3, (float) y, (float) z);
        transform3.rotation = quaternion;
        Transform transform4 = this._transform;
        transform4.position = Vector3.op_Subtraction(transform4.position, Vector3.op_Multiply(Vector3.op_Multiply(Vector3.op_Multiply(this._transform.forward, this.distance), this.distanceMulti), this.distanceOffsetMulti));
        break;
      case CAMERA_TYPE.WOW:
        if (Input.GetKey((KeyCode) 324))
        {
          float num4 = Input.GetAxis("Mouse X") * 10f * sensitivity;
          float num5 = (float) (-(double) Input.GetAxis("Mouse Y") * 10.0) * sensitivity * (float) num1;
          this._transform.RotateAround(this._transform.position, Vector3.up, num4);
          this._transform.RotateAround(this._transform.position, this._transform.right, num5);
        }
        Transform transform5 = this._transform;
        transform5.position = Vector3.op_Subtraction(transform5.position, Vector3.op_Multiply(Vector3.op_Multiply(Vector3.op_Multiply(((Component) this._transform).transform.forward, this.distance), this.distanceMulti), this.distanceOffsetMulti));
        break;
      case CAMERA_TYPE.TPS:
        float num6 = Input.GetAxis("Mouse X") * 10f * sensitivity;
        float num7 = (float) (-(double) Input.GetAxis("Mouse Y") * 10.0) * sensitivity * (float) num1;
        this._transform.RotateAround(this._transform.position, Vector3.up, num6);
        Quaternion rotation2 = this._transform.rotation;
        float num8 = ((Quaternion) ref rotation2).eulerAngles.x % 360f;
        float num9 = num8 + num7;
        if (((double) num7 <= 0.0 || ((double) num8 >= 260.0 || (double) num9 <= 260.0) && ((double) num8 >= 80.0 || (double) num9 <= 80.0)) && ((double) num7 >= 0.0 || ((double) num8 <= 280.0 || (double) num9 >= 280.0) && ((double) num8 <= 100.0 || (double) num9 >= 100.0)))
          this._transform.RotateAround(this._transform.position, this._transform.right, num7);
        Transform transform6 = this._transform;
        transform6.position = Vector3.op_Subtraction(transform6.position, Vector3.op_Multiply(Vector3.op_Multiply(Vector3.op_Multiply(this._transform.forward, this.distance), this.distanceMulti), this.distanceOffsetMulti));
        break;
    }
    if ((double) IN_GAME_MAIN_CAMERA.cameraDistance >= 0.64999997615814209)
      return;
    Transform transform7 = this._transform;
    transform7.position = Vector3.op_Addition(transform7.position, Vector3.op_Multiply(this._transform.right, Mathf.Max((float) ((0.60000002384185791 - (double) IN_GAME_MAIN_CAMERA.cameraDistance) * 2.0), 0.65f)));
  }

  public void CameraMovementLive(HERO hero)
  {
    Vector3 velocity = ((Component) hero).rigidbody.velocity;
    float magnitude = ((Vector3) ref velocity).magnitude;
    Camera.main.fieldOfView = (double) magnitude <= 10.0 ? Mathf.Lerp(Camera.main.fieldOfView, 50f, 0.1f) : Mathf.Lerp(Camera.main.fieldOfView, Mathf.Min(100f, magnitude + 40f), 0.1f);
    float num = (float) ((double) hero.CameraMultiplier * (200.0 - (double) Camera.main.fieldOfView) / 150.0);
    ((Component) this).transform.position = Vector3.op_Subtraction(Vector3.op_Addition(((Component) this.head).transform.position, Vector3.op_Multiply(Vector3.up, this.heightMulti)), Vector3.op_Multiply(Vector3.op_Multiply(Vector3.up, 0.6f - IN_GAME_MAIN_CAMERA.cameraDistance), 2f));
    Transform transform1 = ((Component) this).transform;
    transform1.position = Vector3.op_Subtraction(transform1.position, Vector3.op_Multiply(Vector3.op_Multiply(Vector3.op_Multiply(((Component) this).transform.forward, this.distance), this.distanceMulti), num));
    if ((double) hero.CameraMultiplier < 0.64999997615814209)
    {
      Transform transform2 = ((Component) this).transform;
      transform2.position = Vector3.op_Addition(transform2.position, Vector3.op_Multiply(((Component) this).transform.right, Mathf.Max((float) ((0.60000002384185791 - (double) hero.CameraMultiplier) * 2.0), 0.65f)));
    }
    ((Component) this).transform.rotation = Quaternion.Lerp(((Component) Camera.main).transform.rotation, ((Component) hero).GetComponent<SmoothSyncMovement>().correctCameraRot, Time.deltaTime * 5f);
  }

  private void CreateMinimap()
  {
    LevelInfo info = LevelInfo.getInfo(FengGameManagerMKII.level);
    if (info == null)
      return;
    Minimap minimap = ((Component) this).gameObject.AddComponent<Minimap>();
    if (Object.op_Equality((Object) Minimap.instance.myCam, (Object) null))
    {
      Minimap.instance.myCam = new GameObject().AddComponent<Camera>();
      Minimap.instance.myCam.nearClipPlane = 0.3f;
      Minimap.instance.myCam.farClipPlane = 1000f;
      ((Behaviour) Minimap.instance.myCam).enabled = false;
    }
    if (!SettingsManager.GeneralSettings.MinimapEnabled.Value || SettingsManager.LegacyGameSettings.GlobalMinimapDisable.Value)
    {
      minimap.SetEnabled(false);
      ((Component) Minimap.instance.myCam).gameObject.SetActive(false);
    }
    else
    {
      ((Component) Minimap.instance.myCam).gameObject.SetActive(true);
      minimap.CreateMinimap(Minimap.instance.myCam, mapPreset: info.minimapPreset);
    }
  }

  public void createSnapShotRT2()
  {
    if (Object.op_Inequality((Object) this.snapshotRT, (Object) null))
      this.snapshotRT.Release();
    if (SettingsManager.GeneralSettings.SnapshotsEnabled.Value)
    {
      this.snapShotCamera.SetActive(true);
      this.snapshotRT = new RenderTexture((int) ((double) Screen.width * 0.40000000596046448), (int) ((double) Screen.height * 0.40000000596046448), 24);
      this.snapShotCamera.GetComponent<Camera>().targetTexture = this.snapshotRT;
    }
    else
      this.snapShotCamera.SetActive(false);
  }

  private GameObject findNearestTitan()
  {
    GameObject[] gameObjectsWithTag = GameObject.FindGameObjectsWithTag("titan");
    GameObject nearestTitan = (GameObject) null;
    float num = this.closestDistance = float.PositiveInfinity;
    Vector3 position = this.main_object.transform.position;
    foreach (GameObject gameObject in gameObjectsWithTag)
    {
      Vector3 vector3 = Vector3.op_Subtraction(gameObject.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck").position, position);
      float magnitude = ((Vector3) ref vector3).magnitude;
      if ((double) magnitude < (double) num && (Object.op_Equality((Object) gameObject.GetComponent<TITAN>(), (Object) null) || !gameObject.GetComponent<TITAN>().hasDie))
      {
        nearestTitan = gameObject;
        num = magnitude;
        this.closestDistance = num;
      }
    }
    return nearestTitan;
  }

  public void flashBlind()
  {
    GameObject.Find("flash").GetComponent<UISprite>().alpha = 1f;
    this.flashDuration = 2f;
  }

  private float getSensitivityMultiWithDeltaTime(float sensitivity) => (float) ((double) sensitivity * (double) Time.deltaTime * 62.0);

  private void reset()
  {
    if (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE)
      return;
    GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().restartGameSingle2();
  }

  private Texture2D RTImage2(Camera cam)
  {
    RenderTexture active = RenderTexture.active;
    RenderTexture.active = cam.targetTexture;
    cam.Render();
    Texture2D texture2D1 = new Texture2D(((Texture) cam.targetTexture).width, ((Texture) cam.targetTexture).height, (TextureFormat) 3, false);
    int num1 = (int) ((double) ((Texture) cam.targetTexture).width * 0.039999999105930328);
    int num2 = (int) ((double) ((Texture) cam.targetTexture).width * 0.019999999552965164);
    try
    {
      texture2D1.SetPixel(0, 0, Color.white);
      texture2D1.ReadPixels(new Rect((float) num1, (float) num1, (float) (((Texture) cam.targetTexture).width - num1), (float) (((Texture) cam.targetTexture).height - num1)), num2, num2);
      RenderTexture.active = active;
    }
    catch
    {
      Texture2D texture2D2 = new Texture2D(1, 1);
      texture2D2.SetPixel(0, 0, Color.white);
      return texture2D2;
    }
    return texture2D1;
  }

  public void UpdateSnapshotSkybox() => this.snapShotCamera.gameObject.GetComponent<Skybox>().material = ((Component) this).gameObject.GetComponent<Skybox>().material;

  private void UpdateBottomRightText()
  {
    if (Object.op_Equality((Object) this._bottomRightText, (Object) null))
    {
      GameObject gameObject = GameObject.Find("LabelInfoBottomRight");
      if (Object.op_Inequality((Object) gameObject, (Object) null))
        this._bottomRightText = gameObject.GetComponent<UILabel>();
    }
    if (!Object.op_Inequality((Object) this._bottomRightText, (Object) null))
      return;
    this._bottomRightText.text = "Pause : " + SettingsManager.InputSettings.General.Pause.ToString() + " ";
    if (!SettingsManager.UISettings.ShowInterpolation.Value || !Object.op_Inequality((Object) this.main_object, (Object) null))
      return;
    HERO component = this.main_object.GetComponent<HERO>();
    if (Object.op_Inequality((Object) component, (Object) null) && component.baseRigidBody.interpolation == 1)
      this._bottomRightText.text = "Interpolation : ON \n" + this._bottomRightText.text;
    else
      this._bottomRightText.text = "Interpolation: OFF \n" + this._bottomRightText.text;
  }

  public void setHUDposition()
  {
    GameObject.Find("Flare").transform.localPosition = new Vector3((float) ((int) ((double) -Screen.width * 0.5) + 14), (float) (int) ((double) -Screen.height * 0.5), 0.0f);
    GameObject.Find("LabelInfoBottomRight").transform.localPosition = new Vector3((float) (int) ((double) Screen.width * 0.5), (float) (int) ((double) -Screen.height * 0.5), 0.0f);
    GameObject.Find("LabelInfoTopCenter").transform.localPosition = new Vector3(0.0f, (float) (int) ((double) Screen.height * 0.5), 0.0f);
    GameObject.Find("LabelInfoTopRight").transform.localPosition = new Vector3((float) (int) ((double) Screen.width * 0.5), (float) (int) ((double) Screen.height * 0.5), 0.0f);
    GameObject.Find("LabelNetworkStatus").transform.localPosition = new Vector3((float) (int) ((double) -Screen.width * 0.5), (float) (int) ((double) Screen.height * 0.5), 0.0f);
    GameObject.Find("LabelInfoTopLeft").transform.localPosition = new Vector3((float) (int) ((double) -Screen.width * 0.5), (float) (int) ((double) Screen.height * 0.5 - 20.0), 0.0f);
    GameObject.Find("Chatroom").transform.localPosition = new Vector3((float) (int) ((double) -Screen.width * 0.5), (float) (int) ((double) -Screen.height * 0.5), 0.0f);
    if (Object.op_Inequality((Object) GameObject.Find("Chatroom"), (Object) null))
      GameObject.Find("Chatroom").GetComponent<InRoomChat>().setPosition();
    if (!IN_GAME_MAIN_CAMERA.usingTitan || IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
    {
      GameObject.Find("skill_cd_bottom").transform.localPosition = new Vector3(0.0f, (float) (int) ((double) -Screen.height * 0.5 + 5.0), 0.0f);
      GameObject.Find("GasUI").transform.localPosition = GameObject.Find("skill_cd_bottom").transform.localPosition;
      GameObject.Find("stamina_titan").transform.localPosition = new Vector3(0.0f, 9999f, 0.0f);
      GameObject.Find("stamina_titan_bottom").transform.localPosition = new Vector3(0.0f, 9999f, 0.0f);
    }
    else
    {
      Vector3 vector3;
      // ISSUE: explicit constructor call
      ((Vector3) ref vector3).\u002Ector(0.0f, 9999f, 0.0f);
      GameObject.Find("skill_cd_bottom").transform.localPosition = vector3;
      GameObject.Find("skill_cd_armin").transform.localPosition = vector3;
      GameObject.Find("skill_cd_eren").transform.localPosition = vector3;
      GameObject.Find("skill_cd_jean").transform.localPosition = vector3;
      GameObject.Find("skill_cd_levi").transform.localPosition = vector3;
      GameObject.Find("skill_cd_marco").transform.localPosition = vector3;
      GameObject.Find("skill_cd_mikasa").transform.localPosition = vector3;
      GameObject.Find("skill_cd_petra").transform.localPosition = vector3;
      GameObject.Find("skill_cd_sasha").transform.localPosition = vector3;
      GameObject.Find("GasUI").transform.localPosition = vector3;
      GameObject.Find("stamina_titan").transform.localPosition = new Vector3(-160f, (float) (int) ((double) -Screen.height * 0.5 + 15.0), 0.0f);
      GameObject.Find("stamina_titan_bottom").transform.localPosition = new Vector3(-160f, (float) (int) ((double) -Screen.height * 0.5 + 15.0), 0.0f);
    }
    if (Object.op_Inequality((Object) this.main_object, (Object) null) && Object.op_Inequality((Object) this.main_object.GetComponent<HERO>(), (Object) null))
    {
      if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
        this.main_object.GetComponent<HERO>().setSkillHUDPosition2();
      else if (Object.op_Inequality((Object) this.main_object.GetPhotonView(), (Object) null) && this.main_object.GetPhotonView().isMine)
        this.main_object.GetComponent<HERO>().setSkillHUDPosition2();
    }
    if (IN_GAME_MAIN_CAMERA.stereoType == STEREO_3D_TYPE.SIDE_BY_SIDE)
      ((Component) this).gameObject.GetComponent<Camera>().aspect = (float) (Screen.width / Screen.height);
    this.createSnapShotRT2();
  }

  public GameObject setMainObject(GameObject obj, bool resetRotation = true, bool lockAngle = false)
  {
    this.main_object = obj;
    if (Object.op_Equality((Object) obj, (Object) null))
    {
      this.head = (Transform) null;
      this.distanceMulti = this.heightMulti = 1f;
    }
    else if (Object.op_Inequality((Object) this.main_object.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck/head"), (Object) null))
    {
      this.head = this.main_object.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck/head");
      this.distanceMulti = Object.op_Inequality((Object) this.head, (Object) null) ? Vector3.Distance(((Component) this.head).transform.position, this.main_object.transform.position) * 0.2f : 1f;
      this.heightMulti = Object.op_Inequality((Object) this.head, (Object) null) ? Vector3.Distance(((Component) this.head).transform.position, this.main_object.transform.position) * 0.33f : 1f;
      if (resetRotation)
        ((Component) this).transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
    }
    else if (Object.op_Inequality((Object) this.main_object.transform.Find("Amarture/Controller_Body/hip/spine/chest/neck/head"), (Object) null))
    {
      this.head = this.main_object.transform.Find("Amarture/Controller_Body/hip/spine/chest/neck/head");
      this.distanceMulti = this.heightMulti = 0.64f;
      if (resetRotation)
        ((Component) this).transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
    }
    else
    {
      this.head = (Transform) null;
      this.distanceMulti = this.heightMulti = 1f;
      if (resetRotation)
        ((Component) this).transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
    }
    this.lockAngle = lockAngle;
    return obj;
  }

  public GameObject setMainObjectASTITAN(GameObject obj)
  {
    this.main_object = obj;
    if (Object.op_Inequality((Object) this.main_object.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck/head"), (Object) null))
    {
      this.head = this.main_object.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck/head");
      this.distanceMulti = Object.op_Inequality((Object) this.head, (Object) null) ? Vector3.Distance(((Component) this.head).transform.position, this.main_object.transform.position) * 0.4f : 1f;
      this.heightMulti = Object.op_Inequality((Object) this.head, (Object) null) ? Vector3.Distance(((Component) this.head).transform.position, this.main_object.transform.position) * 0.45f : 1f;
      ((Component) this).transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
    }
    return obj;
  }

  public void setSpectorMode(bool val)
  {
    this.spectatorMode = val;
    GameObject.Find("MainCamera").GetComponent<SpectatorMovement>().disable = !val;
    GameObject.Find("MainCamera").GetComponent<MouseLook>().disable = !val;
  }

  private void shakeUpdate()
  {
    if ((double) this.duration <= 0.0)
      return;
    this.duration -= Time.deltaTime;
    if (this.flip)
    {
      Transform transform = ((Component) this).gameObject.transform;
      transform.position = Vector3.op_Addition(transform.position, Vector3.op_Multiply(Vector3.up, this.R));
    }
    else
    {
      Transform transform = ((Component) this).gameObject.transform;
      transform.position = Vector3.op_Subtraction(transform.position, Vector3.op_Multiply(Vector3.up, this.R));
    }
    this.flip = !this.flip;
    this.R *= this.decay;
  }

  private void Start()
  {
    GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().addCamera(this);
    this.locker = GameObject.Find("locker");
    IN_GAME_MAIN_CAMERA.cameraDistance = SettingsManager.GeneralSettings.CameraDistance.Value + 0.3f;
    ((Component) this).camera.farClipPlane = (float) SettingsManager.GraphicsSettings.RenderDistance.Value;
    this.createSnapShotRT2();
  }

  public void startShake(float R, float duration, float decay = 0.95f)
  {
    if ((double) this.duration >= (double) duration)
      return;
    this.R = R;
    this.duration = duration;
    this.decay = decay;
  }

  public void startSnapShot2(Vector3 p, int dmg, GameObject target, float startTime)
  {
    if (!this.snapShotCamera.activeSelf || dmg < SettingsManager.GeneralSettings.SnapshotsMinimumDamage.Value)
      return;
    this.StartCoroutine(this.CreateSnapshot(p, dmg, target, startTime));
  }

  private IEnumerator CreateSnapshot(
    Vector3 position,
    int damage,
    GameObject target,
    float startTime)
  {
    UITexture display = ((Component) GameObject.Find("UI_IN_GAME").GetComponent<UIReferArray>().panels[0].transform.Find("snapshot1")).GetComponent<UITexture>();
    yield return (object) new WaitForSeconds(startTime);
    this.SetSnapshotPosition(target, position);
    Texture2D snapshot = this.RTImage2(this.snapShotCamera.GetComponent<Camera>());
    yield return (object) new WaitForSeconds(0.2f);
    snapshot.Apply();
    display.mainTexture = (Texture) snapshot;
    ((Component) display).transform.localScale = new Vector3((float) Screen.width * 0.4f, (float) Screen.height * 0.4f, 1f);
    ((Component) display).transform.localPosition = new Vector3((float) -Screen.width * 0.225f, (float) Screen.height * 0.225f, 0.0f);
    ((Component) display).transform.rotation = Quaternion.Euler(0.0f, 0.0f, 10f);
    if (SettingsManager.GeneralSettings.SnapshotsShowInGame.Value)
      ((Behaviour) display).enabled = true;
    else
      ((Behaviour) display).enabled = false;
    yield return (object) new WaitForSeconds(0.2f);
    SnapshotManager.AddSnapshot(snapshot, damage);
    yield return (object) new WaitForSeconds(2f);
    ((Behaviour) display).enabled = false;
    Object.Destroy((Object) snapshot);
  }

  private void SetSnapshotPosition(GameObject target, Vector3 snapshotPosition)
  {
    this.snapShotCamera.transform.position = Object.op_Equality((Object) this.head, (Object) null) ? this.main_object.transform.position : ((Component) this.head).transform.position;
    Transform transform1 = this.snapShotCamera.transform;
    transform1.position = Vector3.op_Addition(transform1.position, Vector3.op_Multiply(Vector3.up, this.heightMulti));
    Transform transform2 = this.snapShotCamera.transform;
    transform2.position = Vector3.op_Subtraction(transform2.position, Vector3.op_Multiply(Vector3.up, 1.1f));
    Vector3 position;
    Vector3 vector3_1 = Vector3.op_Multiply(Vector3.op_Addition(position = this.snapShotCamera.transform.position, snapshotPosition), 0.5f);
    this.snapShotCamera.transform.position = vector3_1;
    Vector3 vector3_2 = vector3_1;
    this.snapShotCamera.transform.LookAt(snapshotPosition);
    this.snapShotCamera.transform.RotateAround(((Component) this).transform.position, Vector3.up, Random.Range(-20f, 20f));
    this.snapShotCamera.transform.LookAt(vector3_2);
    this.snapShotCamera.transform.RotateAround(vector3_2, ((Component) this).transform.right, Random.Range(-20f, 20f));
    float num = Vector3.Distance(snapshotPosition, position);
    if (Object.op_Inequality((Object) target, (Object) null) && Object.op_Inequality((Object) target.GetComponent<TITAN>(), (Object) null))
      num += target.transform.localScale.x * 15f;
    Transform transform3 = this.snapShotCamera.transform;
    transform3.position = Vector3.op_Subtraction(transform3.position, Vector3.op_Multiply(this.snapShotCamera.transform.forward, Random.Range(num + 3f, num + 10f)));
    this.snapShotCamera.transform.LookAt(vector3_2);
    this.snapShotCamera.transform.RotateAround(vector3_2, ((Component) this).transform.forward, Random.Range(-30f, 30f));
    Vector3 vector3_3 = Object.op_Equality((Object) this.head, (Object) null) ? this.main_object.transform.position : ((Component) this.head).transform.position;
    Vector3 vector3_4 = Vector3.op_Subtraction(Object.op_Equality((Object) this.head, (Object) null) ? this.main_object.transform.position : ((Component) this.head).transform.position, this.snapShotCamera.transform.position);
    Vector3 vector3_5 = Vector3.op_Subtraction(vector3_3, vector3_4);
    LayerMask layerMask1 = LayerMask.op_Implicit(1 << LayerMask.NameToLayer("Ground"));
    LayerMask layerMask2 = LayerMask.op_Implicit(1 << LayerMask.NameToLayer("EnemyBox"));
    LayerMask layerMask3 = LayerMask.op_Implicit(LayerMask.op_Implicit(layerMask1) | LayerMask.op_Implicit(layerMask2));
    if (Object.op_Inequality((Object) this.head, (Object) null))
    {
      RaycastHit raycastHit;
      if (Physics.Linecast(((Component) this.head).transform.position, vector3_5, ref raycastHit, LayerMask.op_Implicit(layerMask1)))
      {
        this.snapShotCamera.transform.position = ((RaycastHit) ref raycastHit).point;
      }
      else
      {
        if (!Physics.Linecast(Vector3.op_Subtraction(((Component) this.head).transform.position, Vector3.op_Multiply(Vector3.op_Multiply(vector3_4, this.distanceMulti), 3f)), vector3_5, ref raycastHit, LayerMask.op_Implicit(layerMask3)))
          return;
        this.snapShotCamera.transform.position = ((RaycastHit) ref raycastHit).point;
      }
    }
    else
    {
      RaycastHit raycastHit;
      if (!Physics.Linecast(Vector3.op_Addition(this.main_object.transform.position, Vector3.up), vector3_5, ref raycastHit, LayerMask.op_Implicit(layerMask3)))
        return;
      this.snapShotCamera.transform.position = ((RaycastHit) ref raycastHit).point;
    }
  }

  public void update2()
  {
    this.UpdateBottomRightText();
    if ((double) this.flashDuration > 0.0)
    {
      this.flashDuration -= Time.deltaTime;
      if ((double) this.flashDuration <= 0.0)
        this.flashDuration = 0.0f;
      GameObject.Find("flash").GetComponent<UISprite>().alpha = this.flashDuration * 0.5f;
    }
    switch (IN_GAME_MAIN_CAMERA.gametype)
    {
      case GAMETYPE.SINGLE:
        if (GameMenu.Paused)
        {
          if (!Object.op_Inequality((Object) this.main_object, (Object) null))
            break;
          Vector3 position = ((Component) this).transform.position;
          ((Component) this).transform.position = Vector3.Lerp(((Component) this).transform.position, Vector3.op_Subtraction(Vector3.op_Addition(Object.op_Equality((Object) this.head, (Object) null) ? this.main_object.transform.position : ((Component) this.head).transform.position, Vector3.op_Multiply(Vector3.up, this.heightMulti)), Vector3.op_Multiply(((Component) this).transform.forward, 5f)), 0.2f);
          break;
        }
        if (SettingsManager.InputSettings.General.Pause.GetKeyDown())
          GameMenu.TogglePause(true);
        if (IN_GAME_MAIN_CAMERA.needSetHUD)
        {
          IN_GAME_MAIN_CAMERA.needSetHUD = false;
          this.setHUDposition();
        }
        if (SettingsManager.InputSettings.General.ToggleFullscreen.GetKeyDown())
        {
          FullscreenHandler.ToggleFullscreen();
          IN_GAME_MAIN_CAMERA.needSetHUD = true;
        }
        if (SettingsManager.InputSettings.General.RestartGame.GetKeyDown())
        {
          float num = Time.realtimeSinceStartup - IN_GAME_MAIN_CAMERA._lastRestartTime;
          if (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE && PhotonNetwork.isMasterClient && (double) num > 2.0)
          {
            IN_GAME_MAIN_CAMERA._lastRestartTime = Time.realtimeSinceStartup;
            object[] objArray = new object[2]
            {
              (object) "<color=#FFCC00>MasterClient has restarted the game!</color>",
              (object) ""
            };
            FengGameManagerMKII.instance.photonView.RPC("Chat", PhotonTargets.All, objArray);
            FengGameManagerMKII.instance.restartRC();
          }
        }
        if (SettingsManager.InputSettings.General.RestartGame.GetKeyDown() || SettingsManager.InputSettings.General.ChangeCharacter.GetKeyDown())
          this.reset();
        if (!Object.op_Inequality((Object) this.main_object, (Object) null))
          break;
        if (SettingsManager.InputSettings.General.ChangeCamera.GetKeyDown())
        {
          switch (IN_GAME_MAIN_CAMERA.cameraMode)
          {
            case CAMERA_TYPE.ORIGINAL:
              IN_GAME_MAIN_CAMERA.cameraMode = CAMERA_TYPE.WOW;
              break;
            case CAMERA_TYPE.WOW:
              IN_GAME_MAIN_CAMERA.cameraMode = CAMERA_TYPE.TPS;
              break;
            case CAMERA_TYPE.TPS:
              IN_GAME_MAIN_CAMERA.cameraMode = CAMERA_TYPE.ORIGINAL;
              break;
          }
          this.verticalRotationOffset = 0.0f;
        }
        if (SettingsManager.InputSettings.General.HideUI.GetKeyDown())
          GameMenu.HideCrosshair = !GameMenu.HideCrosshair;
        if (SettingsManager.InputSettings.Human.FocusTitan.GetKeyDown())
        {
          IN_GAME_MAIN_CAMERA.triggerAutoLock = !IN_GAME_MAIN_CAMERA.triggerAutoLock;
          if (IN_GAME_MAIN_CAMERA.triggerAutoLock)
          {
            this.lockTarget = this.findNearestTitan();
            if ((double) this.closestDistance >= 150.0)
            {
              this.lockTarget = (GameObject) null;
              IN_GAME_MAIN_CAMERA.triggerAutoLock = false;
            }
          }
        }
        if (this.gameOver && Object.op_Inequality((Object) this.main_object, (Object) null))
        {
          if (SettingsManager.InputSettings.General.SpectateToggleLive.GetKeyDown())
            SettingsManager.LegacyGeneralSettings.LiveSpectate.Value = !SettingsManager.LegacyGeneralSettings.LiveSpectate.Value;
          HERO component = this.main_object.GetComponent<HERO>();
          if (Object.op_Inequality((Object) component, (Object) null) && SettingsManager.LegacyGeneralSettings.LiveSpectate.Value && ((Behaviour) ((Component) component).GetComponent<SmoothSyncMovement>()).enabled && component.isPhotonCamera)
            this.CameraMovementLive(component);
          else if (this.lockAngle)
          {
            ((Component) this).transform.rotation = Quaternion.Lerp(((Component) this).transform.rotation, this.main_object.transform.rotation, 0.2f);
            ((Component) this).transform.position = Vector3.Lerp(((Component) this).transform.position, Vector3.op_Subtraction(this.main_object.transform.position, Vector3.op_Multiply(this.main_object.transform.forward, 5f)), 0.2f);
          }
          else
            this.camareMovement();
        }
        else
          this.camareMovement();
        if (IN_GAME_MAIN_CAMERA.triggerAutoLock && Object.op_Inequality((Object) this.lockTarget, (Object) null))
        {
          float z = ((Component) this).transform.eulerAngles.z;
          Transform transform = this.lockTarget.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck");
          Vector3 vector3 = Vector3.op_Subtraction(transform.position, Object.op_Equality((Object) this.head, (Object) null) ? this.main_object.transform.position : ((Component) this.head).transform.position);
          ((Vector3) ref vector3).Normalize();
          this.lockCameraPosition = Object.op_Equality((Object) this.head, (Object) null) ? this.main_object.transform.position : ((Component) this.head).transform.position;
          this.lockCameraPosition = Vector3.op_Subtraction(this.lockCameraPosition, Vector3.op_Multiply(Vector3.op_Multiply(Vector3.op_Multiply(vector3, this.distance), this.distanceMulti), this.distanceOffsetMulti));
          this.lockCameraPosition = Vector3.op_Addition(this.lockCameraPosition, Vector3.op_Multiply(Vector3.op_Multiply(Vector3.op_Multiply(Vector3.up, 3f), this.heightMulti), this.distanceOffsetMulti));
          ((Component) this).transform.position = Vector3.Lerp(((Component) this).transform.position, this.lockCameraPosition, Time.deltaTime * 4f);
          if (Object.op_Inequality((Object) this.head, (Object) null))
            ((Component) this).transform.LookAt(Vector3.op_Addition(Vector3.op_Multiply(((Component) this.head).transform.position, 0.8f), Vector3.op_Multiply(transform.position, 0.2f)));
          else
            ((Component) this).transform.LookAt(Vector3.op_Addition(Vector3.op_Multiply(this.main_object.transform.position, 0.8f), Vector3.op_Multiply(transform.position, 0.2f)));
          ((Component) this).transform.localEulerAngles = new Vector3(((Component) this).transform.eulerAngles.x, ((Component) this).transform.eulerAngles.y, z);
          Vector2 vector2 = Vector2.op_Implicit(((Component) this).camera.WorldToScreenPoint(Vector3.op_Subtraction(transform.position, Vector3.op_Multiply(transform.forward, this.lockTarget.transform.localScale.x))));
          this.locker.transform.localPosition = new Vector3(vector2.x - (float) Screen.width * 0.5f, vector2.y - (float) Screen.height * 0.5f, 0.0f);
          if (Object.op_Inequality((Object) this.lockTarget.GetComponent<TITAN>(), (Object) null) && this.lockTarget.GetComponent<TITAN>().hasDie)
            this.lockTarget = (GameObject) null;
        }
        else
          this.locker.transform.localPosition = new Vector3(0.0f, (float) ((double) -Screen.height * 0.5 - 50.0), 0.0f);
        Vector3 vector3_1 = Object.op_Equality((Object) this.head, (Object) null) ? this.main_object.transform.position : this.head.position;
        Vector3 vector3_2 = Vector3.op_Subtraction(Object.op_Equality((Object) this.head, (Object) null) ? this.main_object.transform.position : this.head.position, this._transform.position);
        Vector3 normalized = ((Vector3) ref vector3_2).normalized;
        Vector3 vector3_3 = Vector3.op_Subtraction(vector3_1, Vector3.op_Multiply(Vector3.op_Multiply(this.distance, normalized), this.distanceMulti));
        LayerMask layerMask1 = LayerMask.op_Implicit(1 << PhysicsLayer.Ground);
        LayerMask layerMask2 = LayerMask.op_Implicit(1 << PhysicsLayer.EnemyBox);
        LayerMask layerMask3 = LayerMask.op_Implicit(LayerMask.op_Implicit(layerMask1) | LayerMask.op_Implicit(layerMask2));
        if (Object.op_Inequality((Object) this.head, (Object) null))
        {
          RaycastHit raycastHit;
          if (Physics.Linecast(this.head.position, vector3_3, ref raycastHit, LayerMask.op_Implicit(layerMask1)))
            this._transform.position = ((RaycastHit) ref raycastHit).point;
          else if (Physics.Linecast(Vector3.op_Subtraction(this.head.position, Vector3.op_Multiply(Vector3.op_Multiply(normalized, this.distanceMulti), 3f)), vector3_3, ref raycastHit, LayerMask.op_Implicit(layerMask2)))
            this._transform.position = ((RaycastHit) ref raycastHit).point;
        }
        else
        {
          RaycastHit raycastHit;
          if (Physics.Linecast(Vector3.op_Addition(this.main_object.transform.position, Vector3.up), vector3_3, ref raycastHit, LayerMask.op_Implicit(layerMask3)))
            this._transform.position = ((RaycastHit) ref raycastHit).point;
        }
        this.shakeUpdate();
        break;
      case GAMETYPE.STOP:
        break;
      default:
        if (this.gameOver)
        {
          if (SettingsManager.InputSettings.Human.AttackSpecial.GetKeyDown())
          {
            if (this.spectatorMode)
              this.setSpectorMode(false);
            else
              this.setSpectorMode(true);
          }
          if (SettingsManager.InputSettings.General.SpectateNextPlayer.GetKeyDown())
          {
            ++this.currentPeekPlayerIndex;
            int length = GameObject.FindGameObjectsWithTag("Player").Length;
            if (this.currentPeekPlayerIndex >= length)
              this.currentPeekPlayerIndex = 0;
            if (length > 0)
            {
              this.setMainObject(GameObject.FindGameObjectsWithTag("Player")[this.currentPeekPlayerIndex]);
              this.setSpectorMode(false);
              this.lockAngle = false;
            }
          }
          if (SettingsManager.InputSettings.General.SpectatePreviousPlayer.GetKeyDown())
          {
            --this.currentPeekPlayerIndex;
            int length = GameObject.FindGameObjectsWithTag("Player").Length;
            if (this.currentPeekPlayerIndex >= length)
              this.currentPeekPlayerIndex = 0;
            if (this.currentPeekPlayerIndex < 0)
              this.currentPeekPlayerIndex = length - 1;
            if (length > 0)
            {
              this.setMainObject(GameObject.FindGameObjectsWithTag("Player")[this.currentPeekPlayerIndex]);
              this.setSpectorMode(false);
              this.lockAngle = false;
            }
          }
          if (this.spectatorMode)
            break;
          goto case GAMETYPE.SINGLE;
        }
        else
          goto case GAMETYPE.SINGLE;
    }
  }

  public enum RotationAxes
  {
    MouseXAndY,
    MouseX,
    MouseY,
  }
}
