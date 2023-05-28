// Decompiled with JetBrains decompiler
// Type: HERO
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using Constants;
using CustomSkins;
using ExitGames.Client.Photon;
using GameProgress;
using Photon;
using Settings;
using System;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using Weather;
using Xft;

internal class HERO : MonoBehaviour
{
  private HERO_STATE _state;
  private bool almostSingleHook;
  private string attackAnimation;
  private int attackLoop;
  private bool attackMove;
  private bool attackReleased;
  public AudioSource audio_ally;
  public AudioSource audio_hitwall;
  private GameObject badGuy;
  public Animation baseAnimation;
  public Rigidbody baseRigidBody;
  public Transform baseTransform;
  private bool bigLean;
  public float bombCD;
  public bool bombImmune;
  public float bombRadius;
  public float bombSpeed;
  public float bombTime;
  public float bombTimeMax;
  private float buffTime;
  public GameObject bulletLeft;
  private int bulletMAX = 7;
  public GameObject bulletRight;
  private bool buttonAttackRelease;
  public Dictionary<string, UISprite> cachedSprites;
  public float CameraMultiplier;
  public bool canJump = true;
  public GameObject checkBoxLeft;
  public GameObject checkBoxRight;
  public GameObject cross1;
  public GameObject cross2;
  public GameObject crossL1;
  public GameObject crossL2;
  public GameObject crossR1;
  public GameObject crossR2;
  public string currentAnimation;
  private int currentBladeNum = 5;
  private float currentBladeSta = 100f;
  private BUFF currentBuff;
  public Camera currentCamera;
  private float currentGas = 100f;
  public float currentSpeed;
  private bool dashD;
  private Vector3 dashDirection;
  private bool dashL;
  private bool dashR;
  private float dashTime;
  private bool dashU;
  private Vector3 dashV;
  public bool detonate;
  private float dTapTime = -1f;
  private bool EHold;
  private GameObject eren_titan;
  private int escapeTimes = 1;
  private float facingDirection;
  private float flare1CD;
  private float flare2CD;
  private float flare3CD;
  private float flareTotalCD = 30f;
  private Transform forearmL;
  private Transform forearmR;
  private float gravity = 20f;
  private bool grounded;
  private GameObject gunDummy;
  private Vector3 gunTarget;
  private Transform handL;
  private Transform handR;
  private bool hasDied;
  public bool hasspawn;
  private bool hookBySomeOne = true;
  public GameObject hookRefL1;
  public GameObject hookRefL2;
  public GameObject hookRefR1;
  public GameObject hookRefR2;
  private bool hookSomeOne;
  private GameObject hookTarget;
  private float invincible = 3f;
  public bool isCannon;
  private bool isLaunchLeft;
  private bool isLaunchRight;
  private bool isLeftHandHooked;
  private bool isMounted;
  public bool isPhotonCamera;
  private bool isRightHandHooked;
  public float jumpHeight = 2f;
  private bool justGrounded;
  public GameObject LabelDistance;
  public Transform lastHook;
  private float launchElapsedTimeL;
  private float launchElapsedTimeR;
  private Vector3 launchForce;
  private Vector3 launchPointLeft;
  private Vector3 launchPointRight;
  private bool leanLeft;
  private bool leftArmAim;
  public XWeaponTrail leftbladetrail;
  public XWeaponTrail leftbladetrail2;
  private int leftBulletLeft = 7;
  private bool leftGunHasBullet = true;
  private float lTapTime = -1f;
  public GameObject maincamera;
  public float maxVelocityChange = 10f;
  public AudioSource meatDie;
  public Bomb myBomb;
  public GameObject myCannon;
  public Transform myCannonBase;
  public Transform myCannonPlayer;
  public CannonPropRegion myCannonRegion;
  public GROUP myGroup;
  private GameObject myHorse;
  public GameObject myNetWorkName;
  public float myScale = 1f;
  public int myTeam = 1;
  public List<TITAN> myTitans;
  private bool needLean;
  private Quaternion oldHeadRotation;
  private float originVM;
  private bool QHold;
  private string reloadAnimation = string.Empty;
  private bool rightArmAim;
  public XWeaponTrail rightbladetrail;
  public XWeaponTrail rightbladetrail2;
  private int rightBulletLeft = 7;
  private bool rightGunHasBullet = true;
  public AudioSource rope;
  private float rTapTime = -1f;
  public HERO_SETUP setup;
  private GameObject skillCD;
  public float skillCDDuration;
  public float skillCDLast;
  public float skillCDLastCannon;
  private string skillId;
  public string skillIDHUD;
  public AudioSource slash;
  public AudioSource slashHit;
  private ParticleSystem smoke_3dmg;
  private ParticleSystem sparks;
  public float speed = 10f;
  public GameObject speedFX;
  public GameObject speedFX1;
  private ParticleSystem speedFXPS;
  private bool spinning;
  private string standAnimation = "stand";
  private Quaternion targetHeadRotation;
  private Quaternion targetRotation;
  private bool throwedBlades;
  public bool titanForm;
  private GameObject titanWhoGrabMe;
  private int titanWhoGrabMeID;
  private int totalBladeNum = 5;
  public float totalBladeSta = 100f;
  public float totalGas = 100f;
  private Transform upperarmL;
  private Transform upperarmR;
  private float useGasSpeed = 0.2f;
  public bool useGun;
  private float uTapTime = -1f;
  private bool wallJump;
  private float wallRunTime;
  private float _reelInAxis;
  private float _reelOutAxis;
  private float _reelOutScrollTimeLeft;
  private bool _animationStopped;
  private GameObject ThunderSpearL;
  private GameObject ThunderSpearR;
  public GameObject ThunderSpearLModel;
  public GameObject ThunderSpearRModel;
  private bool _hasRunStart;
  private bool _needSetupThunderspears;
  public HumanCustomSkinLoader _customSkinLoader;
  private bool _cancelGasDisable;
  private float _currentEmoteActionTime;
  public float _flareDelayAfterEmote;
  private float _dashCooldownLeft;

  public bool IsMine() => IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE || this.photonView.isMine;

  public void EmoteAction(string animation)
  {
    if (this.state == HERO_STATE.Grab || this.state == HERO_STATE.AirDodge)
      return;
    this.state = HERO_STATE.Salute;
    this.crossFade(animation, 0.1f);
    this._currentEmoteActionTime = this.baseAnimation[animation].length;
  }

  private void UpdateInput()
  {
    if (!GameMenu.Paused)
    {
      if (SettingsManager.InputSettings.Interaction.EmoteMenu.GetKeyDown())
        GameMenu.ToggleEmoteWheel(!GameMenu.WheelMenu);
      if (SettingsManager.InputSettings.Interaction.MenuNext.GetKeyDown())
        GameMenu.NextEmoteWheel();
    }
    this.UpdateReelInput();
  }

  private void UpdateReelInput()
  {
    this._reelOutScrollTimeLeft -= Time.deltaTime;
    if ((double) this._reelOutScrollTimeLeft <= 0.0)
      this._reelOutAxis = 0.0f;
    if (SettingsManager.InputSettings.Human.ReelIn.GetKey())
      this._reelInAxis = -1f;
    foreach (InputKey inputKey in SettingsManager.InputSettings.Human.ReelOut.InputKeys)
    {
      if (inputKey.GetKey())
      {
        this._reelOutAxis = 1f;
        if (inputKey.IsWheel())
          this._reelOutScrollTimeLeft = SettingsManager.InputSettings.Human.ReelOutScrollSmoothing.Value;
      }
    }
  }

  private float GetReelAxis() => (double) this._reelInAxis != 0.0 ? this._reelInAxis : this._reelOutAxis;

  private void SetupThunderSpears()
  {
    if (!this.photonView.isMine)
      return;
    this.photonView.RPC("SetupThunderSpearsRPC", PhotonTargets.AllBuffered);
  }

  [RPC]
  private void SetupThunderSpearsRPC(PhotonMessageInfo info)
  {
    if (info.sender != this.photonView.owner)
      return;
    if (!this._hasRunStart)
      this._needSetupThunderspears = true;
    else
      this.CreateAndAttachThunderSpears();
  }

  private void CreateAndAttachThunderSpears()
  {
    this.ThunderSpearL = (GameObject) Object.Instantiate(FengGameManagerMKII.RCassets.Load("ThunderSpearProp"));
    this.ThunderSpearR = (GameObject) Object.Instantiate(FengGameManagerMKII.RCassets.Load("ThunderSpearProp"));
    this.ThunderSpearLModel = ((Component) this.ThunderSpearL.transform.Find("ThunderSpearModel")).gameObject;
    this.ThunderSpearRModel = ((Component) this.ThunderSpearR.transform.Find("ThunderSpearModel")).gameObject;
    this.AttachThunderSpear(this.ThunderSpearL, ((Component) this.handL).transform, true);
    this.AttachThunderSpear(this.ThunderSpearR, ((Component) this.handR).transform, false);
    this.currentBladeNum = this.totalBladeNum = 0;
    this.totalBladeSta = this.currentBladeSta = 0.0f;
    this.setup.part_blade_l.SetActive(false);
    this.setup.part_blade_r.SetActive(false);
  }

  private void AttachThunderSpear(GameObject thunderSpear, Transform mount, bool left)
  {
    thunderSpear.transform.parent = mount.parent;
    Vector3 vector3 = left ? new Vector3(-0.001649f, 0.000775f, -0.000227f) : new Vector3(-0.001649f, -0.000775f, -0.000227f);
    Quaternion quaternion = left ? Quaternion.Euler(5f, -85f, 10f) : Quaternion.Euler(-5f, -85f, -10f);
    thunderSpear.transform.localPosition = vector3;
    thunderSpear.transform.localRotation = quaternion;
  }

  private void SetThunderSpears(bool hasLeft, bool hasRight) => this.photonView.RPC("SetThunderSpearsRPC", PhotonTargets.All, (object) hasLeft, (object) hasRight);

  [RPC]
  private void SetThunderSpearsRPC(bool hasLeft, bool hasRight, PhotonMessageInfo info)
  {
    if (info.sender != this.photonView.owner)
      return;
    if (Object.op_Inequality((Object) this.ThunderSpearLModel, (Object) null))
      this.ThunderSpearLModel.SetActive(hasLeft);
    if (!Object.op_Inequality((Object) this.ThunderSpearRModel, (Object) null))
      return;
    this.ThunderSpearRModel.SetActive(hasRight);
  }

  private void applyForceToBody(GameObject GO, Vector3 v)
  {
    GO.rigidbody.AddForce(v);
    GO.rigidbody.AddTorque(Random.Range(-10f, 10f), Random.Range(-10f, 10f), Random.Range(-10f, 10f));
  }

  public void attackAccordingToMouse()
  {
    if ((double) Input.mousePosition.x < (double) Screen.width * 0.5)
      this.attackAnimation = "attack2";
    else
      this.attackAnimation = "attack1";
  }

  public void attackAccordingToTarget(Transform a)
  {
    Vector3 vector3 = Vector3.op_Subtraction(a.position, ((Component) this).transform.position);
    double num1 = -(double) Mathf.Atan2(vector3.z, vector3.x) * 57.295780181884766;
    Quaternion rotation = ((Component) this).transform.rotation;
    double num2 = (double) ((Quaternion) ref rotation).eulerAngles.y - 90.0;
    float num3 = -Mathf.DeltaAngle((float) num1, (float) num2);
    if ((double) Mathf.Abs(num3) < 90.0 && (double) ((Vector3) ref vector3).magnitude < 6.0 && (double) a.position.y <= (double) ((Component) this).transform.position.y + 2.0 && (double) a.position.y >= (double) ((Component) this).transform.position.y - 5.0)
      this.attackAnimation = "attack4";
    else if ((double) num3 > 0.0)
      this.attackAnimation = "attack1";
    else
      this.attackAnimation = "attack2";
  }

  private void Awake()
  {
    this.cache();
    this.setup = ((Component) this).gameObject.GetComponent<HERO_SETUP>();
    this.baseRigidBody.freezeRotation = true;
    this.baseRigidBody.useGravity = false;
    this.handL = this.baseTransform.Find("Amarture/Controller_Body/hip/spine/chest/shoulder_L/upper_arm_L/forearm_L/hand_L");
    this.handR = this.baseTransform.Find("Amarture/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R/forearm_R/hand_R");
    this.forearmL = this.baseTransform.Find("Amarture/Controller_Body/hip/spine/chest/shoulder_L/upper_arm_L/forearm_L");
    this.forearmR = this.baseTransform.Find("Amarture/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R/forearm_R");
    this.upperarmL = this.baseTransform.Find("Amarture/Controller_Body/hip/spine/chest/shoulder_L/upper_arm_L");
    this.upperarmR = this.baseTransform.Find("Amarture/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R");
    this._customSkinLoader = ((Component) this).gameObject.AddComponent<HumanCustomSkinLoader>();
  }

  public void backToHuman()
  {
    ((Component) this).gameObject.GetComponent<SmoothSyncMovement>().disabled = false;
    ((Component) this).rigidbody.velocity = Vector3.zero;
    this.titanForm = false;
    this.ungrabbed();
    this.falseAttack();
    this.skillCDDuration = this.skillCDLast;
    GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().setMainObject(((Component) this).gameObject);
    if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
      return;
    this.photonView.RPC("backToHumanRPC", PhotonTargets.Others);
  }

  [RPC]
  private void backToHumanRPC()
  {
    this.titanForm = false;
    this.eren_titan = (GameObject) null;
    ((Component) this).gameObject.GetComponent<SmoothSyncMovement>().disabled = false;
  }

  [RPC]
  public void badGuyReleaseMe()
  {
    this.hookBySomeOne = false;
    this.badGuy = (GameObject) null;
  }

  [RPC]
  public void blowAway(Vector3 force, PhotonMessageInfo info)
  {
    if (info != null)
    {
      if ((double) Math.Abs(force.x) > 500.0 || (double) Math.Abs(force.y) > 500.0 || (double) Math.Abs(force.z) > 500.0)
      {
        FengGameManagerMKII.instance.kickPlayerRCIfMC(info.sender, true, "hero blowaway exploit");
        return;
      }
      if (!info.sender.isMasterClient && (Convert.ToInt32(info.sender.customProperties[(object) PhotonPlayerProperty.isTitan]) == 1 || Convert.ToBoolean(info.sender.customProperties[(object) PhotonPlayerProperty.dead])))
        return;
    }
    if (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE && !this.photonView.isMine)
      return;
    ((Component) this).rigidbody.AddForce(force, (ForceMode) 1);
    ((Component) this).transform.LookAt(((Component) this).transform.position);
  }

  private void bodyLean()
  {
    if (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE && !this.photonView.isMine)
      return;
    float num1 = 0.0f;
    this.needLean = false;
    if (!this.useGun && this.state == HERO_STATE.Attack && this.attackAnimation != "attack3_1" && this.attackAnimation != "attack3_2" && !this.IsFiringThunderSpear())
    {
      float y = ((Component) this).rigidbody.velocity.y;
      float x = ((Component) this).rigidbody.velocity.x;
      float z = ((Component) this).rigidbody.velocity.z;
      float num2 = Mathf.Sqrt((float) ((double) x * (double) x + (double) z * (double) z));
      this.targetRotation = Quaternion.Euler((float) (-(double) (Mathf.Atan2(y, num2) * 57.29578f) * (1.0 - (double) Vector3.Angle(((Component) this).rigidbody.velocity, ((Component) this).transform.forward) / 90.0)), this.facingDirection, 0.0f);
      if ((!this.isLeftHandHooked || !Object.op_Inequality((Object) this.bulletLeft, (Object) null)) && (!this.isRightHandHooked || !Object.op_Inequality((Object) this.bulletRight, (Object) null)))
        return;
      ((Component) this).transform.rotation = this.targetRotation;
    }
    else
    {
      if (this.isLeftHandHooked && Object.op_Inequality((Object) this.bulletLeft, (Object) null) && this.isRightHandHooked && Object.op_Inequality((Object) this.bulletRight, (Object) null))
      {
        if (this.almostSingleHook)
        {
          this.needLean = true;
          num1 = this.getLeanAngle(this.bulletRight.transform.position, true);
        }
      }
      else if (this.isLeftHandHooked && Object.op_Inequality((Object) this.bulletLeft, (Object) null))
      {
        this.needLean = true;
        num1 = this.getLeanAngle(this.bulletLeft.transform.position, true);
      }
      else if (this.isRightHandHooked && Object.op_Inequality((Object) this.bulletRight, (Object) null))
      {
        this.needLean = true;
        num1 = this.getLeanAngle(this.bulletRight.transform.position, false);
      }
      if (this.needLean)
      {
        float num3 = 0.0f;
        if (!this.useGun && this.state != HERO_STATE.Attack)
          num3 = Mathf.Min(this.currentSpeed * 0.1f, 20f);
        this.targetRotation = Quaternion.Euler(-num3, this.facingDirection, num1);
      }
      else
      {
        if (this.state == HERO_STATE.Attack)
          return;
        this.targetRotation = Quaternion.Euler(0.0f, this.facingDirection, 0.0f);
      }
    }
  }

  public void bombInit()
  {
    this.skillIDHUD = this.skillId;
    this.skillCDDuration = this.skillCDLast;
    if (!SettingsManager.LegacyGameSettings.BombModeEnabled.Value)
      return;
    int num1 = SettingsManager.AbilitySettings.BombRadius.Value;
    int num2 = SettingsManager.AbilitySettings.BombCooldown.Value;
    int num3 = SettingsManager.AbilitySettings.BombSpeed.Value;
    int num4 = SettingsManager.AbilitySettings.BombRange.Value;
    if (num1 + num2 + num3 + num4 > 16)
    {
      num1 = num3 = 6;
      num4 = 3;
      num2 = 1;
    }
    this.bombTimeMax = (float) (((double) num4 * 60.0 + 200.0) / ((double) num3 * 60.0 + 200.0));
    this.bombRadius = (float) ((double) num1 * 4.0 + 20.0);
    this.bombCD = (float) ((double) (num2 + 4) * -0.40000000596046448 + 5.0);
    this.bombSpeed = (float) ((double) num3 * 60.0 + 200.0);
    Hashtable propertiesToSet = new Hashtable();
    ((Dictionary<object, object>) propertiesToSet).Add((object) PhotonPlayerProperty.RCBombR, (object) SettingsManager.AbilitySettings.BombColor.Value.r);
    ((Dictionary<object, object>) propertiesToSet).Add((object) PhotonPlayerProperty.RCBombG, (object) SettingsManager.AbilitySettings.BombColor.Value.g);
    ((Dictionary<object, object>) propertiesToSet).Add((object) PhotonPlayerProperty.RCBombB, (object) SettingsManager.AbilitySettings.BombColor.Value.b);
    ((Dictionary<object, object>) propertiesToSet).Add((object) PhotonPlayerProperty.RCBombA, (object) SettingsManager.AbilitySettings.BombColor.Value.a);
    ((Dictionary<object, object>) propertiesToSet).Add((object) PhotonPlayerProperty.RCBombRadius, (object) this.bombRadius);
    PhotonNetwork.player.SetCustomProperties(propertiesToSet);
    this.skillId = "bomb";
    this.skillIDHUD = "armin";
    this.skillCDLast = this.bombCD;
    this.skillCDDuration = 10f;
    if ((double) FengGameManagerMKII.instance.roundTime <= 10.0)
      return;
    this.skillCDDuration = 5f;
  }

  private void breakApart2(Vector3 v, bool isBite)
  {
    GameObject GO1 = (GameObject) Object.Instantiate(Resources.Load("Character_parts/AOTTG_HERO_body"), ((Component) this).transform.position, ((Component) this).transform.rotation);
    GO1.gameObject.GetComponent<HERO_SETUP>().myCostume = this.setup.myCostume;
    GO1.GetComponent<HERO_SETUP>().isDeadBody = true;
    GO1.GetComponent<HERO_DEAD_BODY_SETUP>().init(this.currentAnimation, ((Component) this).animation[this.currentAnimation].normalizedTime, BODY_PARTS.ARM_R);
    if (!isBite)
    {
      GameObject GO2 = (GameObject) Object.Instantiate(Resources.Load("Character_parts/AOTTG_HERO_body"), ((Component) this).transform.position, ((Component) this).transform.rotation);
      GameObject GO3 = (GameObject) Object.Instantiate(Resources.Load("Character_parts/AOTTG_HERO_body"), ((Component) this).transform.position, ((Component) this).transform.rotation);
      GameObject GO4 = (GameObject) Object.Instantiate(Resources.Load("Character_parts/AOTTG_HERO_body"), ((Component) this).transform.position, ((Component) this).transform.rotation);
      GO2.gameObject.GetComponent<HERO_SETUP>().myCostume = this.setup.myCostume;
      GO3.gameObject.GetComponent<HERO_SETUP>().myCostume = this.setup.myCostume;
      GO4.gameObject.GetComponent<HERO_SETUP>().myCostume = this.setup.myCostume;
      GO2.GetComponent<HERO_SETUP>().isDeadBody = true;
      GO3.GetComponent<HERO_SETUP>().isDeadBody = true;
      GO4.GetComponent<HERO_SETUP>().isDeadBody = true;
      GO2.GetComponent<HERO_DEAD_BODY_SETUP>().init(this.currentAnimation, ((Component) this).animation[this.currentAnimation].normalizedTime, BODY_PARTS.UPPER);
      GO3.GetComponent<HERO_DEAD_BODY_SETUP>().init(this.currentAnimation, ((Component) this).animation[this.currentAnimation].normalizedTime, BODY_PARTS.LOWER);
      GO4.GetComponent<HERO_DEAD_BODY_SETUP>().init(this.currentAnimation, ((Component) this).animation[this.currentAnimation].normalizedTime, BODY_PARTS.ARM_L);
      this.applyForceToBody(GO2, v);
      this.applyForceToBody(GO3, v);
      this.applyForceToBody(GO4, v);
      if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE || this.photonView.isMine)
        ((Component) this.currentCamera).GetComponent<IN_GAME_MAIN_CAMERA>().setMainObject(GO2, false);
    }
    else if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE || this.photonView.isMine)
      ((Component) this.currentCamera).GetComponent<IN_GAME_MAIN_CAMERA>().setMainObject(GO1, false);
    this.applyForceToBody(GO1, v);
    Transform transform1 = ((Component) ((Component) this).transform.Find("Amarture/Controller_Body/hip/spine/chest/shoulder_L/upper_arm_L/forearm_L/hand_L")).transform;
    Transform transform2 = ((Component) ((Component) this).transform.Find("Amarture/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R/forearm_R/hand_R")).transform;
    GameObject GO5;
    GameObject GO6;
    GameObject GO7;
    GameObject GO8;
    GameObject GO9;
    if (this.useGun)
    {
      GO5 = (GameObject) Object.Instantiate(Resources.Load("Character_parts/character_gun_l"), transform1.position, transform1.rotation);
      GO6 = (GameObject) Object.Instantiate(Resources.Load("Character_parts/character_gun_r"), transform2.position, transform2.rotation);
      GO7 = (GameObject) Object.Instantiate(Resources.Load("Character_parts/character_3dmg_2"), ((Component) this).transform.position, ((Component) this).transform.rotation);
      GO8 = (GameObject) Object.Instantiate(Resources.Load("Character_parts/character_gun_mag_l"), ((Component) this).transform.position, ((Component) this).transform.rotation);
      GO9 = (GameObject) Object.Instantiate(Resources.Load("Character_parts/character_gun_mag_r"), ((Component) this).transform.position, ((Component) this).transform.rotation);
    }
    else
    {
      GO5 = (GameObject) Object.Instantiate(Resources.Load("Character_parts/character_blade_l"), transform1.position, transform1.rotation);
      GO6 = (GameObject) Object.Instantiate(Resources.Load("Character_parts/character_blade_r"), transform2.position, transform2.rotation);
      GO7 = (GameObject) Object.Instantiate(Resources.Load("Character_parts/character_3dmg"), ((Component) this).transform.position, ((Component) this).transform.rotation);
      GO8 = (GameObject) Object.Instantiate(Resources.Load("Character_parts/character_3dmg_gas_l"), ((Component) this).transform.position, ((Component) this).transform.rotation);
      GO9 = (GameObject) Object.Instantiate(Resources.Load("Character_parts/character_3dmg_gas_r"), ((Component) this).transform.position, ((Component) this).transform.rotation);
    }
    GO5.renderer.material = CharacterMaterials.materials[this.setup.myCostume._3dmg_texture];
    GO6.renderer.material = CharacterMaterials.materials[this.setup.myCostume._3dmg_texture];
    GO7.renderer.material = CharacterMaterials.materials[this.setup.myCostume._3dmg_texture];
    GO8.renderer.material = CharacterMaterials.materials[this.setup.myCostume._3dmg_texture];
    GO9.renderer.material = CharacterMaterials.materials[this.setup.myCostume._3dmg_texture];
    this.applyForceToBody(GO5, v);
    this.applyForceToBody(GO6, v);
    this.applyForceToBody(GO7, v);
    this.applyForceToBody(GO8, v);
    this.applyForceToBody(GO9, v);
  }

  private void bufferUpdate()
  {
    if ((double) this.buffTime <= 0.0)
      return;
    this.buffTime -= Time.deltaTime;
    if ((double) this.buffTime > 0.0)
      return;
    this.buffTime = 0.0f;
    if (this.currentBuff == BUFF.SpeedUp && ((Component) this).animation.IsPlaying("run_sasha"))
      this.crossFade("run", 0.1f);
    this.currentBuff = BUFF.NoBuff;
  }

  public void cache()
  {
    this.baseTransform = ((Component) this).transform;
    this.baseRigidBody = ((Component) this).rigidbody;
    this.maincamera = GameObject.Find("MainCamera");
    if (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE && !this.photonView.isMine)
      return;
    this.baseAnimation = ((Component) this).animation;
    this.cross1 = GameObject.Find("cross1");
    this.cross2 = GameObject.Find("cross2");
    this.crossL1 = GameObject.Find("crossL1");
    this.crossL2 = GameObject.Find("crossL2");
    this.crossR1 = GameObject.Find("crossR1");
    this.crossR2 = GameObject.Find("crossR2");
    this.LabelDistance = GameObject.Find("LabelDistance");
    this.cachedSprites = new Dictionary<string, UISprite>();
    foreach (GameObject gameObject in Object.FindObjectsOfType(typeof (GameObject)))
    {
      if (Object.op_Inequality((Object) gameObject.GetComponent<UISprite>(), (Object) null) && gameObject.activeInHierarchy)
      {
        string name = ((Object) gameObject).name;
        if ((name.Contains("blade") || name.Contains("bullet") || name.Contains("gas") || name.Contains("flare") || name.Contains("skill_cd")) && !this.cachedSprites.ContainsKey(name))
          this.cachedSprites.Add(name, gameObject.GetComponent<UISprite>());
      }
    }
    this.SetupCrosshairs();
  }

  private void SetupCrosshairs()
  {
    this.cross1.transform.localPosition = Vector3.op_Multiply(Vector3.up, 10000f);
    this.cross2.transform.localPosition = Vector3.op_Multiply(Vector3.up, 10000f);
    this.LabelDistance.transform.localPosition = Vector3.op_Multiply(Vector3.up, 10000f);
  }

  private void calcFlareCD()
  {
    if ((double) this.flare1CD > 0.0)
    {
      this.flare1CD -= Time.deltaTime;
      if ((double) this.flare1CD < 0.0)
        this.flare1CD = 0.0f;
    }
    if ((double) this.flare2CD > 0.0)
    {
      this.flare2CD -= Time.deltaTime;
      if ((double) this.flare2CD < 0.0)
        this.flare2CD = 0.0f;
    }
    if ((double) this.flare3CD <= 0.0)
      return;
    this.flare3CD -= Time.deltaTime;
    if ((double) this.flare3CD >= 0.0)
      return;
    this.flare3CD = 0.0f;
  }

  private void calcSkillCD()
  {
    if ((double) this.skillCDDuration <= 0.0)
      return;
    this.skillCDDuration -= Time.deltaTime;
    if ((double) this.skillCDDuration >= 0.0)
      return;
    this.skillCDDuration = 0.0f;
  }

  private float CalculateJumpVerticalSpeed() => Mathf.Sqrt(2f * this.jumpHeight * this.gravity);

  private void changeBlade()
  {
    if (this.useGun && !this.grounded && LevelInfo.getInfo(FengGameManagerMKII.level).type == GAMEMODE.PVP_AHSS)
      return;
    this.state = HERO_STATE.ChangeBlade;
    this.throwedBlades = false;
    if (this.useGun)
    {
      if (!this.leftGunHasBullet && !this.rightGunHasBullet)
        this.reloadAnimation = !this.grounded ? "AHSS_gun_reload_both_air" : "AHSS_gun_reload_both";
      else if (!this.leftGunHasBullet)
        this.reloadAnimation = !this.grounded ? "AHSS_gun_reload_l_air" : "AHSS_gun_reload_l";
      else if (!this.rightGunHasBullet)
      {
        this.reloadAnimation = !this.grounded ? "AHSS_gun_reload_r_air" : "AHSS_gun_reload_r";
      }
      else
      {
        this.reloadAnimation = !this.grounded ? "AHSS_gun_reload_both_air" : "AHSS_gun_reload_both";
        this.leftGunHasBullet = this.rightGunHasBullet = false;
      }
      this.crossFade(this.reloadAnimation, 0.05f);
    }
    else
    {
      this.reloadAnimation = this.grounded ? nameof (changeBlade) : "changeBlade_air";
      this.crossFade(this.reloadAnimation, 0.1f);
    }
  }

  private void checkDashDoubleTap()
  {
    if ((double) this.uTapTime >= 0.0)
    {
      this.uTapTime += Time.deltaTime;
      if ((double) this.uTapTime > 0.20000000298023224)
        this.uTapTime = -1f;
    }
    if ((double) this.dTapTime >= 0.0)
    {
      this.dTapTime += Time.deltaTime;
      if ((double) this.dTapTime > 0.20000000298023224)
        this.dTapTime = -1f;
    }
    if ((double) this.lTapTime >= 0.0)
    {
      this.lTapTime += Time.deltaTime;
      if ((double) this.lTapTime > 0.20000000298023224)
        this.lTapTime = -1f;
    }
    if ((double) this.rTapTime >= 0.0)
    {
      this.rTapTime += Time.deltaTime;
      if ((double) this.rTapTime > 0.20000000298023224)
        this.rTapTime = -1f;
    }
    if (SettingsManager.InputSettings.General.Forward.GetKeyDown())
    {
      if ((double) this.uTapTime == -1.0)
        this.uTapTime = 0.0f;
      if ((double) this.uTapTime != 0.0)
        this.dashU = true;
    }
    if (SettingsManager.InputSettings.General.Back.GetKeyDown())
    {
      if ((double) this.dTapTime == -1.0)
        this.dTapTime = 0.0f;
      if ((double) this.dTapTime != 0.0)
        this.dashD = true;
    }
    if (SettingsManager.InputSettings.General.Left.GetKeyDown())
    {
      if ((double) this.lTapTime == -1.0)
        this.lTapTime = 0.0f;
      if ((double) this.lTapTime != 0.0)
        this.dashL = true;
    }
    if (!SettingsManager.InputSettings.General.Right.GetKeyDown())
      return;
    if ((double) this.rTapTime == -1.0)
      this.rTapTime = 0.0f;
    if ((double) this.rTapTime == 0.0)
      return;
    this.dashR = true;
  }

  private void checkDashRebind()
  {
    if (!SettingsManager.InputSettings.Human.Dash.GetKeyDown())
      return;
    if (SettingsManager.InputSettings.General.Forward.GetKey())
      this.dashU = true;
    else if (SettingsManager.InputSettings.General.Back.GetKey())
      this.dashD = true;
    else if (SettingsManager.InputSettings.General.Left.GetKey())
    {
      this.dashL = true;
    }
    else
    {
      if (!SettingsManager.InputSettings.General.Right.GetKey())
        return;
      this.dashR = true;
    }
  }

  public void checkTitan()
  {
    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    LayerMask layerMask1 = LayerMask.op_Implicit(1 << PhysicsLayer.PlayerAttackBox);
    LayerMask layerMask2 = LayerMask.op_Implicit(1 << PhysicsLayer.Ground);
    LayerMask layerMask3 = LayerMask.op_Implicit(1 << PhysicsLayer.EnemyBox);
    LayerMask layerMask4 = LayerMask.op_Implicit(LayerMask.op_Implicit(layerMask1) | LayerMask.op_Implicit(layerMask2) | LayerMask.op_Implicit(layerMask3));
    RaycastHit[] raycastHitArray = Physics.RaycastAll(ray, 180f, ((LayerMask) ref layerMask4).value);
    List<RaycastHit> raycastHitList = new List<RaycastHit>();
    List<TITAN> titanList = new List<TITAN>();
    for (int index = 0; index < raycastHitArray.Length; ++index)
    {
      RaycastHit raycastHit = raycastHitArray[index];
      raycastHitList.Add(raycastHit);
    }
    raycastHitList.Sort((Comparison<RaycastHit>) ((x, y) => ((RaycastHit) ref x).distance.CompareTo(((RaycastHit) ref y).distance)));
    float num = 180f;
    for (int index = 0; index < raycastHitList.Count; ++index)
    {
      RaycastHit raycastHit1 = raycastHitList[index];
      GameObject gameObject = ((Component) ((RaycastHit) ref raycastHit1).collider).gameObject;
      if (gameObject.layer == 16)
      {
        if (((Object) gameObject).name.Contains("PlayerDetectorRC"))
        {
          RaycastHit raycastHit2;
          RaycastHit raycastHit3 = raycastHit2 = raycastHitList[index];
          if ((double) ((RaycastHit) ref raycastHit3).distance < (double) num)
          {
            num -= 60f;
            if ((double) num <= 60.0)
              index = raycastHitList.Count;
            TITAN component = ((Component) gameObject.transform.root).gameObject.GetComponent<TITAN>();
            if (Object.op_Inequality((Object) component, (Object) null))
              titanList.Add(component);
          }
        }
      }
      else
        index = raycastHitList.Count;
    }
    for (int index = 0; index < this.myTitans.Count; ++index)
    {
      TITAN titan = this.myTitans[index];
      if (!titanList.Contains(titan))
        titan.isLook = false;
    }
    for (int index = 0; index < titanList.Count; ++index)
      titanList[index].isLook = true;
    this.myTitans = titanList;
  }

  public void ClearPopup() => FengGameManagerMKII.instance.ShowHUDInfoCenter(string.Empty);

  public void continueAnimation()
  {
    if (!this._animationStopped)
      return;
    this._animationStopped = false;
    foreach (AnimationState animationState in ((Component) this).animation)
    {
      if ((double) animationState.speed == 1.0)
        return;
      animationState.speed = 1f;
    }
    this.customAnimationSpeed();
    this.playAnimation(this.currentPlayingClipName());
    if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE || !this.photonView.isMine)
      return;
    this.photonView.RPC("netContinueAnimation", PhotonTargets.Others);
  }

  public void crossFade(string aniName, float time)
  {
    this.currentAnimation = aniName;
    ((Component) this).animation.CrossFade(aniName, time);
    if (!PhotonNetwork.connected || !this.photonView.isMine)
      return;
    this.photonView.RPC("netCrossFade", PhotonTargets.Others, (object) aniName, (object) time);
  }

  public string currentPlayingClipName()
  {
    foreach (AnimationState animationState in ((Component) this).animation)
    {
      if (((Component) this).animation.IsPlaying(animationState.name))
        return animationState.name;
    }
    return string.Empty;
  }

  private void customAnimationSpeed()
  {
    ((Component) this).animation["attack5"].speed = 1.85f;
    ((Component) this).animation["changeBlade"].speed = 1.2f;
    ((Component) this).animation["air_release"].speed = 0.6f;
    ((Component) this).animation["changeBlade_air"].speed = 0.8f;
    ((Component) this).animation["AHSS_gun_reload_both"].speed = 0.38f;
    ((Component) this).animation["AHSS_gun_reload_both_air"].speed = 0.5f;
    ((Component) this).animation["AHSS_gun_reload_l"].speed = 0.4f;
    ((Component) this).animation["AHSS_gun_reload_l_air"].speed = 0.5f;
    ((Component) this).animation["AHSS_gun_reload_r"].speed = 0.4f;
    ((Component) this).animation["AHSS_gun_reload_r_air"].speed = 0.5f;
  }

  private void dash(float horizontal, float vertical)
  {
    if ((double) this.dashTime > 0.0 || (double) this.currentGas <= 0.0 || this.isMounted || (double) this._dashCooldownLeft > 0.0)
      return;
    this.useGas(this.totalGas * -0.04f);
    this.facingDirection = this.getGlobalFacingDirection(horizontal, vertical);
    this.dashV = this.getGlobaleFacingVector3(this.facingDirection);
    this.originVM = this.currentSpeed;
    Quaternion quaternion = Quaternion.Euler(0.0f, this.facingDirection, 0.0f);
    ((Component) this).rigidbody.rotation = quaternion;
    this.targetRotation = quaternion;
    if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
      Object.Instantiate(Resources.Load("FX/boost_smoke"), ((Component) this).transform.position, ((Component) this).transform.rotation);
    else
      PhotonNetwork.Instantiate("FX/boost_smoke", ((Component) this).transform.position, ((Component) this).transform.rotation, 0);
    this.dashTime = 0.5f;
    this.crossFade(nameof (dash), 0.1f);
    ((Component) this).animation[nameof (dash)].time = 0.1f;
    this.state = HERO_STATE.AirDodge;
    this.falseAttack();
    ((Component) this).rigidbody.AddForce(Vector3.op_Multiply(this.dashV, 40f), (ForceMode) 2);
    this._dashCooldownLeft = 0.2f;
  }

  public void die(Vector3 v, bool isBite)
  {
    if ((double) this.invincible > 0.0)
      return;
    if (this.titanForm && Object.op_Inequality((Object) this.eren_titan, (Object) null))
      this.eren_titan.GetComponent<TITAN_EREN>().lifeTime = 0.1f;
    if (Object.op_Inequality((Object) this.bulletLeft, (Object) null))
      this.bulletLeft.GetComponent<Bullet>().removeMe();
    if (Object.op_Inequality((Object) this.bulletRight, (Object) null))
      this.bulletRight.GetComponent<Bullet>().removeMe();
    this.meatDie.Play();
    if ((IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE || this.photonView.isMine) && !this.useGun)
    {
      this.leftbladetrail.Deactivate();
      this.rightbladetrail.Deactivate();
      this.leftbladetrail2.Deactivate();
      this.rightbladetrail2.Deactivate();
    }
    if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
      GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().ReportKillToChatFeed("Titan", "You", 0);
    this.breakApart2(v, isBite);
    ((Component) this.currentCamera).GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = true;
    GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().gameLose2();
    this.falseAttack();
    this.hasDied = true;
    Transform transform = ((Component) this).transform.Find("audio_die");
    transform.parent = (Transform) null;
    ((Component) transform).GetComponent<AudioSource>().Play();
    if (SettingsManager.GeneralSettings.SnapshotsEnabled.Value)
      GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().startSnapShot2(((Component) this).transform.position, 0, (GameObject) null, 0.02f);
    Object.Destroy((Object) ((Component) this).gameObject);
  }

  public void die2(Transform tf)
  {
    if ((double) this.invincible > 0.0)
      return;
    if (this.titanForm && Object.op_Inequality((Object) this.eren_titan, (Object) null))
      this.eren_titan.GetComponent<TITAN_EREN>().lifeTime = 0.1f;
    if (Object.op_Inequality((Object) this.bulletLeft, (Object) null))
      this.bulletLeft.GetComponent<Bullet>().removeMe();
    if (Object.op_Inequality((Object) this.bulletRight, (Object) null))
      this.bulletRight.GetComponent<Bullet>().removeMe();
    Transform transform = ((Component) this).transform.Find("audio_die");
    transform.parent = (Transform) null;
    ((Component) transform).GetComponent<AudioSource>().Play();
    this.meatDie.Play();
    ((Component) this.currentCamera).GetComponent<IN_GAME_MAIN_CAMERA>().setMainObject((GameObject) null);
    ((Component) this.currentCamera).GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = true;
    if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
      GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().ReportKillToChatFeed("Titan", "You", 0);
    GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().gameLose2();
    this.falseAttack();
    this.hasDied = true;
    ((GameObject) Object.Instantiate(Resources.Load("hitMeat2"))).transform.position = ((Component) this).transform.position;
    Object.Destroy((Object) ((Component) this).gameObject);
  }

  private void dodge2(bool offTheWall = false)
  {
    if (SettingsManager.InputSettings.Human.HorseMount.GetKey() && !Object.op_Equality((Object) this.myHorse, (Object) null) && !this.isMounted && (double) Vector3.Distance(this.myHorse.transform.position, ((Component) this).transform.position) < 15.0)
      return;
    this.state = HERO_STATE.GroundDodge;
    if (!offTheWall)
    {
      float vertical = !SettingsManager.InputSettings.General.Forward.GetKey() ? (!SettingsManager.InputSettings.General.Back.GetKey() ? 0.0f : -1f) : 1f;
      float horizontal = !SettingsManager.InputSettings.General.Left.GetKey() ? (!SettingsManager.InputSettings.General.Right.GetKey() ? 0.0f : 1f) : -1f;
      float globalFacingDirection = this.getGlobalFacingDirection(horizontal, vertical);
      if ((double) horizontal != 0.0 || (double) vertical != 0.0)
      {
        this.facingDirection = globalFacingDirection + 180f;
        this.targetRotation = Quaternion.Euler(0.0f, this.facingDirection, 0.0f);
      }
      this.crossFade("dodge", 0.1f);
    }
    else
    {
      this.playAnimation("dodge");
      this.playAnimationAt("dodge", 0.2f);
    }
    this.sparks.enableEmission = false;
  }

  private void erenTransform()
  {
    this.skillCDDuration = this.skillCDLast;
    if (Object.op_Inequality((Object) this.bulletLeft, (Object) null))
      this.bulletLeft.GetComponent<Bullet>().removeMe();
    if (Object.op_Inequality((Object) this.bulletRight, (Object) null))
      this.bulletRight.GetComponent<Bullet>().removeMe();
    this.eren_titan = IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE ? PhotonNetwork.Instantiate("TITAN_EREN", ((Component) this).transform.position, ((Component) this).transform.rotation, 0) : (GameObject) Object.Instantiate(Resources.Load("TITAN_EREN"), ((Component) this).transform.position, ((Component) this).transform.rotation);
    this.eren_titan.GetComponent<TITAN_EREN>().realBody = ((Component) this).gameObject;
    GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().flashBlind();
    GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().setMainObject(this.eren_titan);
    this.eren_titan.GetComponent<TITAN_EREN>().born();
    this.eren_titan.rigidbody.velocity = ((Component) this).rigidbody.velocity;
    ((Component) this).rigidbody.velocity = Vector3.zero;
    ((Component) this).transform.position = this.eren_titan.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck").position;
    this.titanForm = true;
    if (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE)
      this.photonView.RPC("whoIsMyErenTitan", PhotonTargets.Others, (object) this.eren_titan.GetPhotonView().viewID);
    if (this.smoke_3dmg.enableEmission && IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE && this.photonView.isMine)
      this.photonView.RPC("net3DMGSMOKE", PhotonTargets.Others, (object) false);
    this.smoke_3dmg.enableEmission = false;
  }

  private void escapeFromGrab()
  {
  }

  public void falseAttack()
  {
    this.attackMove = false;
    if (this.useGun)
    {
      if (this.attackReleased)
        return;
      this.continueAnimation();
      this.attackReleased = true;
    }
    else
    {
      if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE || this.photonView.isMine)
      {
        this.checkBoxLeft.GetComponent<TriggerColliderWeapon>().active_me = false;
        this.checkBoxRight.GetComponent<TriggerColliderWeapon>().active_me = false;
        this.checkBoxLeft.GetComponent<TriggerColliderWeapon>().clearHits();
        this.checkBoxRight.GetComponent<TriggerColliderWeapon>().clearHits();
        this.leftbladetrail.StopSmoothly(0.2f);
        this.rightbladetrail.StopSmoothly(0.2f);
        this.leftbladetrail2.StopSmoothly(0.2f);
        this.rightbladetrail2.StopSmoothly(0.2f);
      }
      this.attackLoop = 0;
      if (this.attackReleased)
        return;
      this.continueAnimation();
      this.attackReleased = true;
    }
  }

  public void fillGas() => this.currentGas = this.totalGas;

  private GameObject findNearestTitan()
  {
    GameObject[] gameObjectsWithTag = GameObject.FindGameObjectsWithTag("titan");
    GameObject nearestTitan = (GameObject) null;
    float num = float.PositiveInfinity;
    Vector3 position = ((Component) this).transform.position;
    foreach (GameObject gameObject in gameObjectsWithTag)
    {
      Vector3 vector3 = Vector3.op_Subtraction(gameObject.transform.position, position);
      float sqrMagnitude = ((Vector3) ref vector3).sqrMagnitude;
      if ((double) sqrMagnitude < (double) num)
      {
        nearestTitan = gameObject;
        num = sqrMagnitude;
      }
    }
    return nearestTitan;
  }

  private void FixedUpdate()
  {
    if (!this.titanForm && !this.isCannon && (!GameMenu.Paused || IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE))
    {
      Vector3 velocity1 = this.baseRigidBody.velocity;
      this.currentSpeed = ((Vector3) ref velocity1).magnitude;
      if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE || this.photonView.isMine)
      {
        GameObject gameObject = ((Component) this).gameObject;
        velocity1 = this.baseRigidBody.velocity;
        double magnitude1 = (double) ((Vector3) ref velocity1).magnitude;
        GameProgressManager.RegisterSpeed(gameObject, (float) magnitude1);
        if (!this.baseAnimation.IsPlaying("attack3_2") && !this.baseAnimation.IsPlaying("attack5") && !this.baseAnimation.IsPlaying("special_petra"))
          this.baseRigidBody.rotation = Quaternion.Lerp(((Component) this).gameObject.transform.rotation, this.targetRotation, Time.deltaTime * 6f);
        if (this.state == HERO_STATE.Grab)
        {
          this.baseRigidBody.AddForce(Vector3.op_UnaryNegation(this.baseRigidBody.velocity), (ForceMode) 2);
        }
        else
        {
          if (this.IsGrounded())
          {
            if (!this.grounded)
              this.justGrounded = true;
            this.grounded = true;
          }
          else
            this.grounded = false;
          if (this.hookSomeOne)
          {
            if (Object.op_Inequality((Object) this.hookTarget, (Object) null))
            {
              Vector3 vector3 = Vector3.op_Subtraction(this.hookTarget.transform.position, this.baseTransform.position);
              float magnitude2 = ((Vector3) ref vector3).magnitude;
              if ((double) magnitude2 > 2.0)
                this.baseRigidBody.AddForce(Vector3.op_Subtraction(Vector3.op_Multiply(Vector3.op_Multiply(((Vector3) ref vector3).normalized, Mathf.Pow(magnitude2, 0.15f)), 30f), Vector3.op_Multiply(this.baseRigidBody.velocity, 0.95f)), (ForceMode) 2);
            }
            else
              this.hookSomeOne = false;
          }
          else if (this.hookBySomeOne && Object.op_Inequality((Object) this.badGuy, (Object) null))
          {
            if (Object.op_Inequality((Object) this.badGuy, (Object) null))
            {
              Vector3 vector3 = Vector3.op_Subtraction(this.badGuy.transform.position, this.baseTransform.position);
              float magnitude3 = ((Vector3) ref vector3).magnitude;
              if ((double) magnitude3 > 5.0)
                this.baseRigidBody.AddForce(Vector3.op_Multiply(Vector3.op_Multiply(((Vector3) ref vector3).normalized, Mathf.Pow(magnitude3, 0.15f)), 0.2f), (ForceMode) 1);
            }
            else
              this.hookBySomeOne = false;
          }
          float num1 = 0.0f;
          float num2 = 0.0f;
          if (!IN_GAME_MAIN_CAMERA.isTyping && !GameMenu.InMenu())
          {
            num2 = !SettingsManager.InputSettings.General.Forward.GetKey() ? (!SettingsManager.InputSettings.General.Back.GetKey() ? 0.0f : -1f) : 1f;
            num1 = !SettingsManager.InputSettings.General.Left.GetKey() ? (!SettingsManager.InputSettings.General.Right.GetKey() ? 0.0f : 1f) : -1f;
          }
          bool flag1 = false;
          bool flag2 = false;
          bool flag3 = false;
          this.isLeftHandHooked = false;
          this.isRightHandHooked = false;
          if (this.isLaunchLeft)
          {
            if (Object.op_Inequality((Object) this.bulletLeft, (Object) null) && this.bulletLeft.GetComponent<Bullet>().isHooked())
            {
              this.isLeftHandHooked = true;
              Vector3 vector3_1 = Vector3.op_Subtraction(this.bulletLeft.transform.position, this.baseTransform.position);
              ((Vector3) ref vector3_1).Normalize();
              Vector3 vector3_2 = Vector3.op_Multiply(vector3_1, 10f);
              if (!this.isLaunchRight)
                vector3_2 = Vector3.op_Multiply(vector3_2, 2f);
              if ((double) Vector3.Angle(this.baseRigidBody.velocity, vector3_2) > 90.0 && SettingsManager.InputSettings.Human.Jump.GetKey())
              {
                flag2 = true;
                flag1 = true;
              }
              if (!flag2)
              {
                this.baseRigidBody.AddForce(vector3_2);
                if ((double) Vector3.Angle(this.baseRigidBody.velocity, vector3_2) > 90.0)
                  this.baseRigidBody.AddForce(Vector3.op_Multiply(Vector3.op_UnaryNegation(this.baseRigidBody.velocity), 2f), (ForceMode) 5);
              }
            }
            this.launchElapsedTimeL += Time.deltaTime;
            if (this.QHold && (double) this.currentGas > 0.0)
              this.useGas(this.useGasSpeed * Time.deltaTime);
            else if ((double) this.launchElapsedTimeL > 0.30000001192092896)
            {
              this.isLaunchLeft = false;
              if (Object.op_Inequality((Object) this.bulletLeft, (Object) null))
              {
                this.bulletLeft.GetComponent<Bullet>().disable();
                this.releaseIfIHookSb();
                this.bulletLeft = (GameObject) null;
                flag2 = false;
              }
            }
          }
          if (this.isLaunchRight)
          {
            if (Object.op_Inequality((Object) this.bulletRight, (Object) null) && this.bulletRight.GetComponent<Bullet>().isHooked())
            {
              this.isRightHandHooked = true;
              Vector3 vector3_3 = Vector3.op_Subtraction(this.bulletRight.transform.position, this.baseTransform.position);
              ((Vector3) ref vector3_3).Normalize();
              Vector3 vector3_4 = Vector3.op_Multiply(vector3_3, 10f);
              if (!this.isLaunchLeft)
                vector3_4 = Vector3.op_Multiply(vector3_4, 2f);
              if ((double) Vector3.Angle(this.baseRigidBody.velocity, vector3_4) > 90.0 && SettingsManager.InputSettings.Human.Jump.GetKey())
              {
                flag3 = true;
                flag1 = true;
              }
              if (!flag3)
              {
                this.baseRigidBody.AddForce(vector3_4);
                if ((double) Vector3.Angle(this.baseRigidBody.velocity, vector3_4) > 90.0)
                  this.baseRigidBody.AddForce(Vector3.op_Multiply(Vector3.op_UnaryNegation(this.baseRigidBody.velocity), 2f), (ForceMode) 5);
              }
            }
            this.launchElapsedTimeR += Time.deltaTime;
            if (this.EHold && (double) this.currentGas > 0.0)
              this.useGas(this.useGasSpeed * Time.deltaTime);
            else if ((double) this.launchElapsedTimeR > 0.30000001192092896)
            {
              this.isLaunchRight = false;
              if (Object.op_Inequality((Object) this.bulletRight, (Object) null))
              {
                this.bulletRight.GetComponent<Bullet>().disable();
                this.releaseIfIHookSb();
                this.bulletRight = (GameObject) null;
                flag3 = false;
              }
            }
          }
          if (this.grounded)
          {
            Vector3 vector3_5 = Vector3.zero;
            if (this.state == HERO_STATE.Attack)
            {
              if (this.attackAnimation == "attack5")
              {
                if ((double) this.baseAnimation[this.attackAnimation].normalizedTime > 0.40000000596046448 && (double) this.baseAnimation[this.attackAnimation].normalizedTime < 0.61000001430511475)
                  this.baseRigidBody.AddForce(Vector3.op_Multiply(((Component) this).gameObject.transform.forward, 200f));
              }
              else if (this.attackAnimation == "special_petra")
              {
                if ((double) this.baseAnimation[this.attackAnimation].normalizedTime > 0.34999999403953552 && (double) this.baseAnimation[this.attackAnimation].normalizedTime < 0.47999998927116394)
                  this.baseRigidBody.AddForce(Vector3.op_Multiply(((Component) this).gameObject.transform.forward, 200f));
              }
              else if (this.baseAnimation.IsPlaying("attack3_2"))
                vector3_5 = Vector3.zero;
              else if (this.baseAnimation.IsPlaying("attack1") || this.baseAnimation.IsPlaying("attack2"))
                this.baseRigidBody.AddForce(Vector3.op_Multiply(((Component) this).gameObject.transform.forward, 200f));
              if (this.baseAnimation.IsPlaying("attack3_2"))
                vector3_5 = Vector3.zero;
            }
            if (this.justGrounded)
            {
              if (this.state != HERO_STATE.Attack || this.attackAnimation != "attack3_1" && this.attackAnimation != "attack5" && this.attackAnimation != "special_petra")
              {
                if (this.state != HERO_STATE.Attack && (double) num1 == 0.0 && (double) num2 == 0.0 && Object.op_Equality((Object) this.bulletLeft, (Object) null) && Object.op_Equality((Object) this.bulletRight, (Object) null) && this.state != HERO_STATE.FillGas)
                {
                  this.state = HERO_STATE.Land;
                  this.crossFade("dash_land", 0.01f);
                }
                else
                {
                  this.buttonAttackRelease = true;
                  if (this.state != HERO_STATE.Attack && (double) this.baseRigidBody.velocity.x * (double) this.baseRigidBody.velocity.x + (double) this.baseRigidBody.velocity.z * (double) this.baseRigidBody.velocity.z > (double) this.speed * (double) this.speed * 1.5 && this.state != HERO_STATE.FillGas)
                  {
                    this.state = HERO_STATE.Slide;
                    this.crossFade("slide", 0.05f);
                    this.facingDirection = Mathf.Atan2(this.baseRigidBody.velocity.x, this.baseRigidBody.velocity.z) * 57.29578f;
                    this.targetRotation = Quaternion.Euler(0.0f, this.facingDirection, 0.0f);
                    this.sparks.enableEmission = true;
                  }
                }
              }
              this.justGrounded = false;
              vector3_5 = this.baseRigidBody.velocity;
            }
            if (this.state == HERO_STATE.Attack && this.attackAnimation == "attack3_1" && (double) this.baseAnimation[this.attackAnimation].normalizedTime >= 1.0)
            {
              this.playAnimation("attack3_2");
              this.resetAnimationSpeed();
              Vector3 zero = Vector3.zero;
              this.baseRigidBody.velocity = zero;
              vector3_5 = zero;
              ((Component) this.currentCamera).GetComponent<IN_GAME_MAIN_CAMERA>().startShake(0.2f, 0.3f);
            }
            if (this.state == HERO_STATE.GroundDodge)
            {
              if ((double) this.baseAnimation["dodge"].normalizedTime >= 0.20000000298023224 && (double) this.baseAnimation["dodge"].normalizedTime < 0.800000011920929)
                vector3_5 = Vector3.op_Multiply(Vector3.op_Multiply(Vector3.op_UnaryNegation(this.baseTransform.forward), 2.4f), this.speed);
              if ((double) this.baseAnimation["dodge"].normalizedTime > 0.800000011920929)
                vector3_5 = Vector3.op_Multiply(this.baseRigidBody.velocity, 0.9f);
            }
            else if (this.state == HERO_STATE.Idle)
            {
              Vector3 vector3_6;
              // ISSUE: explicit constructor call
              ((Vector3) ref vector3_6).\u002Ector(num1, 0.0f, num2);
              float resultAngle = this.getGlobalFacingDirection(num1, num2);
              vector3_5 = Vector3.op_Multiply(Vector3.op_Multiply(this.getGlobaleFacingVector3(resultAngle), (double) ((Vector3) ref vector3_6).magnitude <= 0.949999988079071 ? ((double) ((Vector3) ref vector3_6).magnitude >= 0.25 ? ((Vector3) ref vector3_6).magnitude : 0.0f) : 1f), this.speed);
              if ((double) this.buffTime > 0.0 && this.currentBuff == BUFF.SpeedUp)
                vector3_5 = Vector3.op_Multiply(vector3_5, 4f);
              if ((double) num1 != 0.0 || (double) num2 != 0.0)
              {
                if (!this.baseAnimation.IsPlaying("run") && !this.baseAnimation.IsPlaying("jump") && !this.baseAnimation.IsPlaying("run_sasha") && (!this.baseAnimation.IsPlaying("horse_geton") || (double) this.baseAnimation["horse_geton"].normalizedTime >= 0.5))
                {
                  if ((double) this.buffTime > 0.0 && this.currentBuff == BUFF.SpeedUp)
                    this.crossFade("run_sasha", 0.1f);
                  else
                    this.crossFade("run", 0.1f);
                }
              }
              else
              {
                if (!this.baseAnimation.IsPlaying(this.standAnimation) && this.state != HERO_STATE.Land && !this.baseAnimation.IsPlaying("jump") && !this.baseAnimation.IsPlaying("horse_geton") && !this.baseAnimation.IsPlaying("grabbed"))
                {
                  this.crossFade(this.standAnimation, 0.1f);
                  vector3_5 = Vector3.op_Multiply(vector3_5, 0.0f);
                }
                resultAngle = -874f;
              }
              if ((double) resultAngle != -874.0)
              {
                this.facingDirection = resultAngle;
                this.targetRotation = Quaternion.Euler(0.0f, this.facingDirection, 0.0f);
              }
            }
            else if (this.state == HERO_STATE.Land)
              vector3_5 = Vector3.op_Multiply(this.baseRigidBody.velocity, 0.96f);
            else if (this.state == HERO_STATE.Slide)
            {
              vector3_5 = Vector3.op_Multiply(this.baseRigidBody.velocity, 0.99f);
              if ((double) this.currentSpeed < (double) this.speed * 1.2000000476837158)
              {
                this.idle();
                this.sparks.enableEmission = false;
              }
            }
            Vector3 velocity2 = this.baseRigidBody.velocity;
            Vector3 vector3_7 = Vector3.op_Subtraction(vector3_5, velocity2);
            vector3_7.x = Mathf.Clamp(vector3_7.x, -this.maxVelocityChange, this.maxVelocityChange);
            vector3_7.z = Mathf.Clamp(vector3_7.z, -this.maxVelocityChange, this.maxVelocityChange);
            vector3_7.y = 0.0f;
            if (this.baseAnimation.IsPlaying("jump") && (double) this.baseAnimation["jump"].normalizedTime > 0.18000000715255737)
              vector3_7.y += 8f;
            if (this.baseAnimation.IsPlaying("horse_geton") && (double) this.baseAnimation["horse_geton"].normalizedTime > 0.18000000715255737 && (double) this.baseAnimation["horse_geton"].normalizedTime < 1.0)
            {
              float num3 = 6f;
              vector3_7 = Vector3.op_UnaryNegation(this.baseRigidBody.velocity);
              vector3_7.y = num3;
              float num4 = (float) (0.60000002384185791 * (double) this.gravity * (double) Vector3.Distance(this.myHorse.transform.position, this.baseTransform.position) / (2.0 * (double) num3));
              Vector3 vector3_8 = Vector3.op_Subtraction(this.myHorse.transform.position, this.baseTransform.position);
              vector3_7 = Vector3.op_Addition(vector3_7, Vector3.op_Multiply(num4, ((Vector3) ref vector3_8).normalized));
            }
            if (this.state != HERO_STATE.Attack || !this.useGun)
            {
              this.baseRigidBody.AddForce(vector3_7, (ForceMode) 2);
              this.baseRigidBody.rotation = Quaternion.Lerp(((Component) this).gameObject.transform.rotation, Quaternion.Euler(0.0f, this.facingDirection, 0.0f), Time.deltaTime * 10f);
            }
          }
          else
          {
            if (this.sparks.enableEmission)
              this.sparks.enableEmission = false;
            if (Object.op_Inequality((Object) this.myHorse, (Object) null) && (this.baseAnimation.IsPlaying("horse_geton") || this.baseAnimation.IsPlaying("air_fall")) && (double) this.baseRigidBody.velocity.y < 0.0 && (double) Vector3.Distance(Vector3.op_Addition(this.myHorse.transform.position, Vector3.op_Multiply(Vector3.up, 1.65f)), this.baseTransform.position) < 0.5)
            {
              this.baseTransform.position = Vector3.op_Addition(this.myHorse.transform.position, Vector3.op_Multiply(Vector3.up, 1.65f));
              this.baseTransform.rotation = this.myHorse.transform.rotation;
              this.isMounted = true;
              if (!((Component) this).animation.IsPlaying("horse_idle"))
                this.crossFade("horse_idle", 0.1f);
              this.myHorse.GetComponent<Horse>().mounted();
            }
            if (this.state == HERO_STATE.Idle && !this.baseAnimation.IsPlaying("dash") && !this.baseAnimation.IsPlaying("wallrun") && !this.baseAnimation.IsPlaying("toRoof") && !this.baseAnimation.IsPlaying("horse_geton") && !this.baseAnimation.IsPlaying("horse_getoff") && !this.baseAnimation.IsPlaying("air_release") && !this.isMounted && (!this.baseAnimation.IsPlaying("air_hook_l_just") || (double) this.baseAnimation["air_hook_l_just"].normalizedTime >= 1.0) && (!this.baseAnimation.IsPlaying("air_hook_r_just") || (double) this.baseAnimation["air_hook_r_just"].normalizedTime >= 1.0) || (double) this.baseAnimation["dash"].normalizedTime >= 0.99000000953674316)
            {
              if (!this.isLeftHandHooked && !this.isRightHandHooked && (this.baseAnimation.IsPlaying("air_hook_l") || this.baseAnimation.IsPlaying("air_hook_r") || this.baseAnimation.IsPlaying("air_hook")) && (double) this.baseRigidBody.velocity.y > 20.0)
              {
                this.baseAnimation.CrossFade("air_release");
              }
              else
              {
                bool flag4 = (double) Mathf.Abs(this.baseRigidBody.velocity.x) + (double) Mathf.Abs(this.baseRigidBody.velocity.z) > 25.0;
                bool flag5 = (double) this.baseRigidBody.velocity.y < 0.0;
                if (!flag4)
                {
                  if (flag5)
                  {
                    if (!this.baseAnimation.IsPlaying("air_fall"))
                      this.crossFade("air_fall", 0.2f);
                  }
                  else if (!this.baseAnimation.IsPlaying("air_rise"))
                    this.crossFade("air_rise", 0.2f);
                }
                else if (!this.isLeftHandHooked && !this.isRightHandHooked)
                {
                  double num5 = -(double) Mathf.Atan2(this.baseRigidBody.velocity.z, this.baseRigidBody.velocity.x) * 57.295780181884766;
                  Quaternion rotation = this.baseTransform.rotation;
                  double num6 = (double) ((Quaternion) ref rotation).eulerAngles.y - 90.0;
                  float num7 = -Mathf.DeltaAngle((float) num5, (float) num6);
                  if ((double) Mathf.Abs(num7) < 45.0)
                  {
                    if (!this.baseAnimation.IsPlaying("air2"))
                      this.crossFade("air2", 0.2f);
                  }
                  else if ((double) num7 < 135.0 && (double) num7 > 0.0)
                  {
                    if (!this.baseAnimation.IsPlaying("air2_right"))
                      this.crossFade("air2_right", 0.2f);
                  }
                  else if ((double) num7 > -135.0 && (double) num7 < 0.0)
                  {
                    if (!this.baseAnimation.IsPlaying("air2_left"))
                      this.crossFade("air2_left", 0.2f);
                  }
                  else if (!this.baseAnimation.IsPlaying("air2_backward"))
                    this.crossFade("air2_backward", 0.2f);
                }
                else if (this.useGun)
                {
                  if (!this.isRightHandHooked)
                  {
                    if (!this.baseAnimation.IsPlaying("AHSS_hook_forward_l"))
                      this.crossFade("AHSS_hook_forward_l", 0.1f);
                  }
                  else if (!this.isLeftHandHooked)
                  {
                    if (!this.baseAnimation.IsPlaying("AHSS_hook_forward_r"))
                      this.crossFade("AHSS_hook_forward_r", 0.1f);
                  }
                  else if (!this.baseAnimation.IsPlaying("AHSS_hook_forward_both"))
                    this.crossFade("AHSS_hook_forward_both", 0.1f);
                }
                else if (!this.isRightHandHooked)
                {
                  if (!this.baseAnimation.IsPlaying("air_hook_l"))
                    this.crossFade("air_hook_l", 0.1f);
                }
                else if (!this.isLeftHandHooked)
                {
                  if (!this.baseAnimation.IsPlaying("air_hook_r"))
                    this.crossFade("air_hook_r", 0.1f);
                }
                else if (!this.baseAnimation.IsPlaying("air_hook"))
                  this.crossFade("air_hook", 0.1f);
              }
            }
            if (!this.baseAnimation.IsPlaying("air_rise"))
            {
              if (this.state == HERO_STATE.Idle && this.baseAnimation.IsPlaying("air_release") && (double) this.baseAnimation["air_release"].normalizedTime >= 1.0)
                this.crossFade("air_rise", 0.2f);
              if (this.baseAnimation.IsPlaying("horse_getoff") && (double) this.baseAnimation["horse_getoff"].normalizedTime >= 1.0)
                this.crossFade("air_rise", 0.2f);
            }
            if (this.baseAnimation.IsPlaying("toRoof"))
            {
              if ((double) this.baseAnimation["toRoof"].normalizedTime < 0.2199999988079071)
              {
                this.baseRigidBody.velocity = Vector3.zero;
                this.baseRigidBody.AddForce(new Vector3(0.0f, this.gravity * this.baseRigidBody.mass, 0.0f));
              }
              else
              {
                if (!this.wallJump)
                {
                  this.wallJump = true;
                  this.baseRigidBody.AddForce(Vector3.op_Multiply(Vector3.up, 8f), (ForceMode) 1);
                }
                this.baseRigidBody.AddForce(Vector3.op_Multiply(this.baseTransform.forward, 0.05f), (ForceMode) 1);
              }
              if ((double) this.baseAnimation["toRoof"].normalizedTime >= 1.0)
                this.playAnimation("air_rise");
            }
            else if (this.state == HERO_STATE.Idle && this.isPressDirectionTowardsHero(num1, num2) && !SettingsManager.InputSettings.Human.Jump.GetKey() && !SettingsManager.InputSettings.Human.HookLeft.GetKey() && !SettingsManager.InputSettings.Human.HookRight.GetKey() && !SettingsManager.InputSettings.Human.HookBoth.GetKey() && this.IsFrontGrounded() && !this.baseAnimation.IsPlaying("wallrun") && !this.baseAnimation.IsPlaying("dodge"))
            {
              this.crossFade("wallrun", 0.1f);
              this.wallRunTime = 0.0f;
            }
            else if (this.baseAnimation.IsPlaying("wallrun"))
            {
              this.baseRigidBody.AddForce(Vector3.op_Subtraction(Vector3.op_Multiply(Vector3.up, this.speed), this.baseRigidBody.velocity), (ForceMode) 2);
              this.wallRunTime += Time.deltaTime;
              if ((double) this.wallRunTime > 1.0 || (double) num2 == 0.0 && (double) num1 == 0.0)
              {
                this.baseRigidBody.AddForce(Vector3.op_Multiply(Vector3.op_Multiply(Vector3.op_UnaryNegation(this.baseTransform.forward), this.speed), 0.75f), (ForceMode) 1);
                this.dodge2(true);
              }
              else if (!this.IsUpFrontGrounded())
              {
                this.wallJump = false;
                this.crossFade("toRoof", 0.1f);
              }
              else if (!this.IsFrontGrounded())
                this.crossFade("air_fall", 0.1f);
            }
            else if (!this.baseAnimation.IsPlaying("attack5") && !this.baseAnimation.IsPlaying("special_petra") && !this.baseAnimation.IsPlaying("dash") && !this.baseAnimation.IsPlaying("jump") && !this.IsFiringThunderSpear())
            {
              Vector3 vector3_9;
              // ISSUE: explicit constructor call
              ((Vector3) ref vector3_9).\u002Ector(num1, 0.0f, num2);
              float resultAngle = this.getGlobalFacingDirection(num1, num2);
              Vector3 vector3_10 = Vector3.op_Multiply(Vector3.op_Multiply(this.getGlobaleFacingVector3(resultAngle), (double) ((Vector3) ref vector3_9).magnitude <= 0.949999988079071 ? ((double) ((Vector3) ref vector3_9).magnitude >= 0.25 ? ((Vector3) ref vector3_9).magnitude : 0.0f) : 1f), (float) ((double) this.setup.myCostume.stat.ACL / 10.0 * 2.0));
              if ((double) num1 == 0.0 && (double) num2 == 0.0)
              {
                if (this.state == HERO_STATE.Attack)
                  vector3_10 = Vector3.op_Multiply(vector3_10, 0.0f);
                resultAngle = -874f;
              }
              if ((double) resultAngle != -874.0)
              {
                this.facingDirection = resultAngle;
                this.targetRotation = Quaternion.Euler(0.0f, this.facingDirection, 0.0f);
              }
              if (!flag2 && !flag3 && !this.isMounted && SettingsManager.InputSettings.Human.Jump.GetKey() && (double) this.currentGas > 0.0)
              {
                if ((double) num1 != 0.0 || (double) num2 != 0.0)
                  this.baseRigidBody.AddForce(vector3_10, (ForceMode) 5);
                else
                  this.baseRigidBody.AddForce(Vector3.op_Multiply(this.baseTransform.forward, ((Vector3) ref vector3_10).magnitude), (ForceMode) 5);
                flag1 = true;
              }
            }
            if (this.baseAnimation.IsPlaying("air_fall") && (double) this.currentSpeed < 0.20000000298023224 && this.IsFrontGrounded())
              this.crossFade("onWall", 0.3f);
          }
          this.spinning = false;
          if (flag2 & flag3)
          {
            float num8 = this.currentSpeed + 0.1f;
            this.baseRigidBody.AddForce(Vector3.op_UnaryNegation(this.baseRigidBody.velocity), (ForceMode) 2);
            Vector3 vector3_11 = Vector3.op_Subtraction(Vector3.op_Multiply(Vector3.op_Addition(this.bulletRight.transform.position, this.bulletLeft.transform.position), 0.5f), this.baseTransform.position);
            float num9 = 1f + Mathf.Clamp(this.GetReelAxis(), -0.8f, 0.8f);
            Vector3 vector3_12 = Vector3.RotateTowards(vector3_11, this.baseRigidBody.velocity, 1.53938f * num9, 1.53938f * num9);
            ((Vector3) ref vector3_12).Normalize();
            this.spinning = true;
            this.baseRigidBody.velocity = Vector3.op_Multiply(vector3_12, num8);
          }
          else if (flag2)
          {
            float num10 = this.currentSpeed + 0.1f;
            this.baseRigidBody.AddForce(Vector3.op_UnaryNegation(this.baseRigidBody.velocity), (ForceMode) 2);
            Vector3 vector3_13 = Vector3.op_Subtraction(this.bulletLeft.transform.position, this.baseTransform.position);
            float num11 = 1f + Mathf.Clamp(this.GetReelAxis(), -0.8f, 0.8f);
            Vector3 vector3_14 = Vector3.RotateTowards(vector3_13, this.baseRigidBody.velocity, 1.53938f * num11, 1.53938f * num11);
            ((Vector3) ref vector3_14).Normalize();
            this.spinning = true;
            this.baseRigidBody.velocity = Vector3.op_Multiply(vector3_14, num10);
          }
          else if (flag3)
          {
            float num12 = this.currentSpeed + 0.1f;
            this.baseRigidBody.AddForce(Vector3.op_UnaryNegation(this.baseRigidBody.velocity), (ForceMode) 2);
            Vector3 vector3_15 = Vector3.op_Subtraction(this.bulletRight.transform.position, this.baseTransform.position);
            float num13 = 1f + Mathf.Clamp(this.GetReelAxis(), -0.8f, 0.8f);
            Vector3 vector3_16 = Vector3.RotateTowards(vector3_15, this.baseRigidBody.velocity, 1.53938f * num13, 1.53938f * num13);
            ((Vector3) ref vector3_16).Normalize();
            this.spinning = true;
            this.baseRigidBody.velocity = Vector3.op_Multiply(vector3_16, num12);
          }
          if (this.state == HERO_STATE.Attack && (this.attackAnimation == "attack5" || this.attackAnimation == "special_petra") && (double) this.baseAnimation[this.attackAnimation].normalizedTime > 0.40000000596046448 && !this.attackMove)
          {
            this.attackMove = true;
            if ((double) ((Vector3) ref this.launchPointRight).magnitude > 0.0)
            {
              Vector3 vector3 = Vector3.op_Subtraction(this.launchPointRight, this.baseTransform.position);
              ((Vector3) ref vector3).Normalize();
              vector3 = Vector3.op_Multiply(vector3, 13f);
              this.baseRigidBody.AddForce(vector3, (ForceMode) 1);
            }
            if (this.attackAnimation == "special_petra" && (double) ((Vector3) ref this.launchPointLeft).magnitude > 0.0)
            {
              Vector3 vector3 = Vector3.op_Subtraction(this.launchPointLeft, this.baseTransform.position);
              ((Vector3) ref vector3).Normalize();
              vector3 = Vector3.op_Multiply(vector3, 13f);
              this.baseRigidBody.AddForce(vector3, (ForceMode) 1);
              if (Object.op_Inequality((Object) this.bulletRight, (Object) null))
              {
                this.bulletRight.GetComponent<Bullet>().disable();
                this.releaseIfIHookSb();
              }
              if (Object.op_Inequality((Object) this.bulletLeft, (Object) null))
              {
                this.bulletLeft.GetComponent<Bullet>().disable();
                this.releaseIfIHookSb();
              }
            }
            this.baseRigidBody.AddForce(Vector3.op_Multiply(Vector3.up, 2f), (ForceMode) 1);
          }
          bool flag6 = false;
          if (Object.op_Inequality((Object) this.bulletLeft, (Object) null) || Object.op_Inequality((Object) this.bulletRight, (Object) null))
          {
            if (Object.op_Inequality((Object) this.bulletLeft, (Object) null) && (double) this.bulletLeft.transform.position.y > (double) ((Component) this).gameObject.transform.position.y && this.isLaunchLeft && this.bulletLeft.GetComponent<Bullet>().isHooked())
              flag6 = true;
            if (Object.op_Inequality((Object) this.bulletRight, (Object) null) && (double) this.bulletRight.transform.position.y > (double) ((Component) this).gameObject.transform.position.y && this.isLaunchRight && this.bulletRight.GetComponent<Bullet>().isHooked())
              flag6 = true;
          }
          if (flag6)
            this.baseRigidBody.AddForce(new Vector3(0.0f, -10f * this.baseRigidBody.mass, 0.0f));
          else
            this.baseRigidBody.AddForce(new Vector3(0.0f, -this.gravity * this.baseRigidBody.mass, 0.0f));
          if ((double) this.currentSpeed > 10.0)
            ((Component) this.currentCamera).GetComponent<Camera>().fieldOfView = Mathf.Lerp(((Component) this.currentCamera).GetComponent<Camera>().fieldOfView, Mathf.Min(100f, this.currentSpeed + 40f), 0.1f);
          else
            ((Component) this.currentCamera).GetComponent<Camera>().fieldOfView = Mathf.Lerp(((Component) this.currentCamera).GetComponent<Camera>().fieldOfView, 50f, 0.1f);
          if (!this._cancelGasDisable)
          {
            if (flag1)
            {
              this.useGas(this.useGasSpeed * Time.deltaTime);
              if (!this.smoke_3dmg.enableEmission && IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE && this.photonView.isMine)
                this.photonView.RPC("net3DMGSMOKE", PhotonTargets.Others, (object) true);
              this.smoke_3dmg.enableEmission = true;
            }
            else
            {
              if (this.smoke_3dmg.enableEmission && IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE && this.photonView.isMine)
                this.photonView.RPC("net3DMGSMOKE", PhotonTargets.Others, (object) false);
              this.smoke_3dmg.enableEmission = false;
            }
          }
          else
            this._cancelGasDisable = false;
          if (WindWeatherEffect.WindEnabled)
          {
            if (!this.speedFXPS.enableEmission)
              this.speedFXPS.enableEmission = true;
            this.speedFXPS.startSpeed = 100f;
            this.speedFX.transform.LookAt(Vector3.op_Addition(this.baseTransform.position, WindWeatherEffect.WindDirection));
          }
          else if ((double) this.currentSpeed > 80.0 && SettingsManager.GraphicsSettings.WindEffectEnabled.Value)
          {
            if (!this.speedFXPS.enableEmission)
              this.speedFXPS.enableEmission = true;
            this.speedFXPS.startSpeed = this.currentSpeed;
            this.speedFX.transform.LookAt(Vector3.op_Addition(this.baseTransform.position, this.baseRigidBody.velocity));
          }
          else if (this.speedFXPS.enableEmission)
            this.speedFXPS.enableEmission = false;
        }
      }
      this.setHookedPplDirection();
      this.bodyLean();
    }
    this._reelInAxis = 0.0f;
  }

  public string getDebugInfo()
  {
    string str1 = "Left:" + this.isLeftHandHooked.ToString() + " ";
    int num;
    if (this.isLeftHandHooked && Object.op_Inequality((Object) this.bulletLeft, (Object) null))
    {
      Vector3 vector3 = Vector3.op_Subtraction(this.bulletLeft.transform.position, ((Component) this).transform.position);
      string str2 = str1;
      num = (int) ((double) Mathf.Atan2(vector3.x, vector3.z) * 57.295780181884766);
      string str3 = num.ToString();
      str1 = str2 + str3;
    }
    string str4 = str1 + "\nRight:" + (object) this.isRightHandHooked + " ";
    if (this.isRightHandHooked && Object.op_Inequality((Object) this.bulletRight, (Object) null))
    {
      Vector3 vector3 = Vector3.op_Subtraction(this.bulletRight.transform.position, ((Component) this).transform.position);
      string str5 = str4;
      num = (int) ((double) Mathf.Atan2(vector3.x, vector3.z) * 57.295780181884766);
      string str6 = num.ToString();
      str4 = str5 + str6;
    }
    string[] strArray = new string[8];
    strArray[0] = str4;
    strArray[1] = "\nfacingDirection:";
    num = (int) this.facingDirection;
    strArray[2] = num.ToString();
    strArray[3] = "\nActual facingDirection:";
    Quaternion rotation = ((Component) this).transform.rotation;
    num = (int) ((Quaternion) ref rotation).eulerAngles.y;
    strArray[4] = num.ToString();
    strArray[5] = "\nState:";
    strArray[6] = this.state.ToString();
    strArray[7] = "\n\n\n\n\n";
    string debugInfo = string.Concat(strArray);
    if (this.state == HERO_STATE.Attack)
      this.targetRotation = Quaternion.Euler(0.0f, this.facingDirection, 0.0f);
    return debugInfo;
  }

  private Vector3 getGlobaleFacingVector3(float resultAngle)
  {
    float num = (float) (-(double) resultAngle + 90.0);
    return new Vector3(Mathf.Cos(num * ((float) Math.PI / 180f)), 0.0f, Mathf.Sin(num * ((float) Math.PI / 180f)));
  }

  private Vector3 getGlobaleFacingVector3(float horizontal, float vertical)
  {
    float num = (float) (-(double) this.getGlobalFacingDirection(horizontal, vertical) + 90.0);
    return new Vector3(Mathf.Cos(num * ((float) Math.PI / 180f)), 0.0f, Mathf.Sin(num * ((float) Math.PI / 180f)));
  }

  private float getGlobalFacingDirection(float horizontal, float vertical)
  {
    if ((double) vertical == 0.0 && (double) horizontal == 0.0)
    {
      Quaternion rotation = ((Component) this).transform.rotation;
      return ((Quaternion) ref rotation).eulerAngles.y;
    }
    Quaternion rotation1 = ((Component) this.currentCamera).transform.rotation;
    return ((Quaternion) ref rotation1).eulerAngles.y + (float) (-(double) (Mathf.Atan2(vertical, horizontal) * 57.29578f) + 90.0);
  }

  private float getLeanAngle(Vector3 p, bool left)
  {
    if (!this.useGun && this.state == HERO_STATE.Attack)
      return 0.0f;
    double num1 = (double) (Mathf.Acos((p.y - ((Component) this).transform.position.y) / Vector3.Distance(p, ((Component) this).transform.position)) * 57.29578f * 0.1f);
    Vector3 velocity = ((Component) this).rigidbody.velocity;
    double num2 = 1.0 + (double) Mathf.Pow(((Vector3) ref velocity).magnitude, 0.2f);
    float num3 = (float) (num1 * num2);
    Vector3 vector3 = Vector3.op_Subtraction(p, ((Component) this).transform.position);
    float num4 = Mathf.DeltaAngle(Mathf.Atan2(vector3.x, vector3.z) * 57.29578f, Mathf.Atan2(((Component) this).rigidbody.velocity.x, ((Component) this).rigidbody.velocity.z) * 57.29578f);
    float num5 = num3 + Mathf.Abs(num4 * 0.5f);
    if (this.state != HERO_STATE.Attack)
      num5 = Mathf.Min(num5, 80f);
    this.leanLeft = (double) num4 > 0.0;
    if (this.useGun)
      return num5 * ((double) num4 >= 0.0 ? 1f : -1f);
    float num6 = left && (double) num4 < 0.0 || !left && (double) num4 > 0.0 ? 0.1f : 0.5f;
    return num5 * ((double) num4 >= 0.0 ? num6 : -num6);
  }

  private void getOffHorse()
  {
    this.playAnimation("horse_getoff");
    ((Component) this).rigidbody.AddForce(Vector3.op_Subtraction(Vector3.op_Subtraction(Vector3.op_Multiply(Vector3.up, 10f), Vector3.op_Multiply(((Component) this).transform.forward, 2f)), Vector3.op_Multiply(((Component) this).transform.right, 1f)), (ForceMode) 2);
    this.unmounted();
  }

  private void getOnHorse()
  {
    this.playAnimation("horse_geton");
    Quaternion rotation = this.myHorse.transform.rotation;
    this.facingDirection = ((Quaternion) ref rotation).eulerAngles.y;
    this.targetRotation = Quaternion.Euler(0.0f, this.facingDirection, 0.0f);
  }

  public void getSupply()
  {
    if (!((Component) this).animation.IsPlaying(this.standAnimation) && !((Component) this).animation.IsPlaying("run") && !((Component) this).animation.IsPlaying("run_sasha") || (double) this.currentBladeSta == (double) this.totalBladeSta && this.currentBladeNum == this.totalBladeNum && (double) this.currentGas == (double) this.totalGas && this.leftBulletLeft == this.bulletMAX && this.rightBulletLeft == this.bulletMAX)
      return;
    this.state = HERO_STATE.FillGas;
    this.crossFade("supply", 0.1f);
  }

  public void grabbed(GameObject titan, bool leftHand)
  {
    if (this.isMounted)
      this.unmounted();
    this.state = HERO_STATE.Grab;
    ((Collider) ((Component) this).GetComponent<CapsuleCollider>()).isTrigger = true;
    this.falseAttack();
    this.titanWhoGrabMe = titan;
    if (this.titanForm && Object.op_Inequality((Object) this.eren_titan, (Object) null))
      this.eren_titan.GetComponent<TITAN_EREN>().lifeTime = 0.1f;
    if (!this.useGun && (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE || this.photonView.isMine))
    {
      this.leftbladetrail.Deactivate();
      this.rightbladetrail.Deactivate();
      this.leftbladetrail2.Deactivate();
      this.rightbladetrail2.Deactivate();
    }
    this.smoke_3dmg.enableEmission = false;
    this.sparks.enableEmission = false;
  }

  public bool HasDied() => this.hasDied || this.isInvincible();

  private void headMovement()
  {
    Transform transform1 = ((Component) this).transform.Find("Amarture/Controller_Body/hip/spine/chest/neck/head");
    Transform transform2 = ((Component) this).transform.Find("Amarture/Controller_Body/hip/spine/chest/neck");
    float num1 = Mathf.Sqrt((float) (((double) this.gunTarget.x - (double) ((Component) this).transform.position.x) * ((double) this.gunTarget.x - (double) ((Component) this).transform.position.x) + ((double) this.gunTarget.z - (double) ((Component) this).transform.position.z) * ((double) this.gunTarget.z - (double) ((Component) this).transform.position.z)));
    this.targetHeadRotation = transform1.rotation;
    Vector3 vector3 = Vector3.op_Subtraction(this.gunTarget, ((Component) this).transform.position);
    double num2 = -(double) Mathf.Atan2(vector3.z, vector3.x) * 57.295780181884766;
    Quaternion rotation1 = ((Component) this).transform.rotation;
    double num3 = (double) ((Quaternion) ref rotation1).eulerAngles.y - 90.0;
    float num4 = Mathf.Clamp(-Mathf.DeltaAngle((float) num2, (float) num3), -40f, 40f);
    float num5 = Mathf.Clamp(Mathf.Atan2(transform2.position.y - this.gunTarget.y, num1) * 57.29578f, -40f, 30f);
    Quaternion rotation2 = transform1.rotation;
    double num6 = (double) ((Quaternion) ref rotation2).eulerAngles.x + (double) num5;
    rotation2 = transform1.rotation;
    double num7 = (double) ((Quaternion) ref rotation2).eulerAngles.y + (double) num4;
    rotation2 = transform1.rotation;
    double z = (double) ((Quaternion) ref rotation2).eulerAngles.z;
    this.targetHeadRotation = Quaternion.Euler((float) num6, (float) num7, (float) z);
    this.oldHeadRotation = Quaternion.Lerp(this.oldHeadRotation, this.targetHeadRotation, Time.deltaTime * 60f);
    transform1.rotation = this.oldHeadRotation;
  }

  public void hookedByHuman(int hooker, Vector3 hookPosition) => this.photonView.RPC("RPCHookedByHuman", this.photonView.owner, (object) hooker, (object) hookPosition);

  [RPC]
  public void hookFail()
  {
    this.hookTarget = (GameObject) null;
    this.hookSomeOne = false;
  }

  public void hookToHuman(GameObject target, Vector3 hookPosition)
  {
    this.releaseIfIHookSb();
    this.hookTarget = target;
    this.hookSomeOne = true;
    if (Object.op_Inequality((Object) target.GetComponent<HERO>(), (Object) null))
      target.GetComponent<HERO>().hookedByHuman(this.photonView.viewID, hookPosition);
    this.launchForce = Vector3.op_Subtraction(hookPosition, ((Component) this).transform.position);
    float num = Mathf.Pow(((Vector3) ref this.launchForce).magnitude, 0.1f);
    if (this.grounded)
      ((Component) this).rigidbody.AddForce(Vector3.op_Multiply(Vector3.up, Mathf.Min(((Vector3) ref this.launchForce).magnitude * 0.2f, 10f)), (ForceMode) 1);
    ((Component) this).rigidbody.AddForce(Vector3.op_Multiply(Vector3.op_Multiply(this.launchForce, num), 0.1f), (ForceMode) 1);
  }

  private void idle()
  {
    if (this.state == HERO_STATE.Attack)
      this.falseAttack();
    this.state = HERO_STATE.Idle;
    this.crossFade(this.standAnimation, 0.1f);
  }

  private bool IsFrontGrounded()
  {
    LayerMask layerMask1 = LayerMask.op_Implicit(1 << LayerMask.NameToLayer("Ground"));
    LayerMask layerMask2 = LayerMask.op_Implicit(LayerMask.op_Implicit(LayerMask.op_Implicit(1 << LayerMask.NameToLayer("EnemyBox"))) | LayerMask.op_Implicit(layerMask1));
    return Physics.Raycast(Vector3.op_Addition(((Component) this).gameObject.transform.position, Vector3.op_Multiply(((Component) this).gameObject.transform.up, 1f)), ((Component) this).gameObject.transform.forward, 1f, ((LayerMask) ref layerMask2).value);
  }

  public bool IsGrounded()
  {
    LayerMask layerMask1 = LayerMask.op_Implicit(1 << LayerMask.NameToLayer("Ground"));
    LayerMask layerMask2 = LayerMask.op_Implicit(LayerMask.op_Implicit(LayerMask.op_Implicit(1 << LayerMask.NameToLayer("EnemyBox"))) | LayerMask.op_Implicit(layerMask1));
    return Physics.Raycast(Vector3.op_Addition(((Component) this).gameObject.transform.position, Vector3.op_Multiply(Vector3.up, 0.1f)), Vector3.op_UnaryNegation(Vector3.up), 0.3f, ((LayerMask) ref layerMask2).value);
  }

  public bool isInvincible() => (double) this.invincible > 0.0;

  private bool isPressDirectionTowardsHero(float h, float v)
  {
    if ((double) h == 0.0 && (double) v == 0.0)
      return false;
    double globalFacingDirection = (double) this.getGlobalFacingDirection(h, v);
    Quaternion rotation = ((Component) this).transform.rotation;
    double y = (double) ((Quaternion) ref rotation).eulerAngles.y;
    return (double) Mathf.Abs(Mathf.DeltaAngle((float) globalFacingDirection, (float) y)) < 45.0;
  }

  private bool IsUpFrontGrounded()
  {
    LayerMask layerMask1 = LayerMask.op_Implicit(1 << LayerMask.NameToLayer("Ground"));
    LayerMask layerMask2 = LayerMask.op_Implicit(LayerMask.op_Implicit(LayerMask.op_Implicit(1 << LayerMask.NameToLayer("EnemyBox"))) | LayerMask.op_Implicit(layerMask1));
    return Physics.Raycast(Vector3.op_Addition(((Component) this).gameObject.transform.position, Vector3.op_Multiply(((Component) this).gameObject.transform.up, 3f)), ((Component) this).gameObject.transform.forward, 1.2f, ((LayerMask) ref layerMask2).value);
  }

  [RPC]
  private void killObject(PhotonMessageInfo info)
  {
    if (info == null)
      return;
    FengGameManagerMKII.instance.kickPlayerRCIfMC(info.sender, true, "hero killObject exploit");
  }

  public void lateUpdate2()
  {
    if (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE && Object.op_Inequality((Object) this.myNetWorkName, (Object) null))
    {
      if (this.titanForm && Object.op_Inequality((Object) this.eren_titan, (Object) null))
        this.myNetWorkName.transform.localPosition = Vector3.op_Multiply(Vector3.op_Multiply(Vector3.up, (float) Screen.height), 2f);
      Vector3 vector3;
      // ISSUE: explicit constructor call
      ((Vector3) ref vector3).\u002Ector(this.baseTransform.position.x, this.baseTransform.position.y + 2f, this.baseTransform.position.z);
      GameObject maincamera = this.maincamera;
      LayerMask layerMask1 = LayerMask.op_Implicit(1 << LayerMask.NameToLayer("Ground"));
      LayerMask layerMask2 = LayerMask.op_Implicit(LayerMask.op_Implicit(LayerMask.op_Implicit(1 << LayerMask.NameToLayer("EnemyBox"))) | LayerMask.op_Implicit(layerMask1));
      if ((double) Vector3.Angle(maincamera.transform.forward, Vector3.op_Subtraction(vector3, maincamera.transform.position)) > 90.0 || Physics.Linecast(vector3, maincamera.transform.position, LayerMask.op_Implicit(layerMask2)))
      {
        this.myNetWorkName.transform.localPosition = Vector3.op_Multiply(Vector3.op_Multiply(Vector3.up, (float) Screen.height), 2f);
      }
      else
      {
        Vector2 vector2 = Vector2.op_Implicit(this.maincamera.GetComponent<Camera>().WorldToScreenPoint(vector3));
        this.myNetWorkName.transform.localPosition = new Vector3((float) (int) ((double) vector2.x - (double) Screen.width * 0.5), (float) (int) ((double) vector2.y - (double) Screen.height * 0.5), 0.0f);
      }
    }
    if (this.titanForm || this.isCannon)
      return;
    if (SettingsManager.GeneralSettings.CameraTilt.Value && (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE || this.photonView.isMine))
    {
      Vector3 vector3_1 = Vector3.zero;
      Vector3 vector3_2 = Vector3.zero;
      if (this.isLaunchLeft && Object.op_Inequality((Object) this.bulletLeft, (Object) null) && this.bulletLeft.GetComponent<Bullet>().isHooked())
        vector3_1 = this.bulletLeft.transform.position;
      if (this.isLaunchRight && Object.op_Inequality((Object) this.bulletRight, (Object) null) && this.bulletRight.GetComponent<Bullet>().isHooked())
        vector3_2 = this.bulletRight.transform.position;
      Vector3 vector3_3 = Vector3.zero;
      if ((double) ((Vector3) ref vector3_1).magnitude != 0.0 && (double) ((Vector3) ref vector3_2).magnitude == 0.0)
        vector3_3 = vector3_1;
      else if ((double) ((Vector3) ref vector3_1).magnitude == 0.0 && (double) ((Vector3) ref vector3_2).magnitude != 0.0)
        vector3_3 = vector3_2;
      else if ((double) ((Vector3) ref vector3_1).magnitude != 0.0 && (double) ((Vector3) ref vector3_2).magnitude != 0.0)
        vector3_3 = Vector3.op_Multiply(Vector3.op_Addition(vector3_1, vector3_2), 0.5f);
      Vector3 vector3_4 = Vector3.Project(Vector3.op_Subtraction(vector3_3, this.baseTransform.position), this.maincamera.transform.up);
      Vector3 vector3_5 = Vector3.Project(Vector3.op_Subtraction(vector3_3, this.baseTransform.position), this.maincamera.transform.right);
      Quaternion quaternion;
      if ((double) ((Vector3) ref vector3_3).magnitude > 0.0)
      {
        Vector3 vector3_6 = Vector3.op_Addition(vector3_4, vector3_5);
        float num1 = Vector3.Angle(Vector3.op_Subtraction(vector3_3, this.baseTransform.position), this.baseRigidBody.velocity) * 0.005f;
        Vector3 vector3_7 = Vector3.op_Addition(this.maincamera.transform.right, ((Vector3) ref vector3_5).normalized);
        Quaternion rotation = this.maincamera.transform.rotation;
        double x = (double) ((Quaternion) ref rotation).eulerAngles.x;
        rotation = this.maincamera.transform.rotation;
        double y = (double) ((Quaternion) ref rotation).eulerAngles.y;
        double num2 = (double) ((Vector3) ref vector3_7).magnitude >= 1.0 ? -(double) Vector3.Angle(vector3_4, vector3_6) * (double) num1 : (double) Vector3.Angle(vector3_4, vector3_6) * (double) num1;
        quaternion = Quaternion.Euler((float) x, (float) y, (float) num2);
      }
      else
      {
        Quaternion rotation1 = this.maincamera.transform.rotation;
        double x = (double) ((Quaternion) ref rotation1).eulerAngles.x;
        Quaternion rotation2 = this.maincamera.transform.rotation;
        double y = (double) ((Quaternion) ref rotation2).eulerAngles.y;
        quaternion = Quaternion.Euler((float) x, (float) y, 0.0f);
      }
      this.maincamera.transform.rotation = Quaternion.Lerp(this.maincamera.transform.rotation, quaternion, Time.deltaTime * 2f);
    }
    if (this.state == HERO_STATE.Grab && Object.op_Inequality((Object) this.titanWhoGrabMe, (Object) null))
    {
      if (Object.op_Inequality((Object) this.titanWhoGrabMe.GetComponent<TITAN>(), (Object) null))
      {
        this.baseTransform.position = this.titanWhoGrabMe.GetComponent<TITAN>().grabTF.transform.position;
        this.baseTransform.rotation = this.titanWhoGrabMe.GetComponent<TITAN>().grabTF.transform.rotation;
      }
      else if (Object.op_Inequality((Object) this.titanWhoGrabMe.GetComponent<FEMALE_TITAN>(), (Object) null))
      {
        this.baseTransform.position = this.titanWhoGrabMe.GetComponent<FEMALE_TITAN>().grabTF.transform.position;
        this.baseTransform.rotation = this.titanWhoGrabMe.GetComponent<FEMALE_TITAN>().grabTF.transform.rotation;
      }
    }
    if (!this.useGun)
      return;
    if (this.leftArmAim || this.rightArmAim)
    {
      Vector3 vector3 = Vector3.op_Subtraction(this.gunTarget, this.baseTransform.position);
      double num3 = -(double) Mathf.Atan2(vector3.z, vector3.x) * 57.295780181884766;
      Quaternion rotation = this.baseTransform.rotation;
      double num4 = (double) ((Quaternion) ref rotation).eulerAngles.y - 90.0;
      float num5 = -Mathf.DeltaAngle((float) num3, (float) num4);
      this.headMovement();
      if (!this.isLeftHandHooked && this.leftArmAim && (double) num5 < 40.0 && (double) num5 > -90.0)
        this.leftArmAimTo(this.gunTarget);
      if (!this.isRightHandHooked && this.rightArmAim && (double) num5 > -40.0 && (double) num5 < 90.0)
        this.rightArmAimTo(this.gunTarget);
    }
    else if (!this.grounded)
    {
      this.handL.localRotation = Quaternion.Euler(90f, 0.0f, 0.0f);
      this.handR.localRotation = Quaternion.Euler(-90f, 0.0f, 0.0f);
    }
    if (this.isLeftHandHooked && Object.op_Inequality((Object) this.bulletLeft, (Object) null))
      this.leftArmAimTo(this.bulletLeft.transform.position);
    if (!this.isRightHandHooked || !Object.op_Inequality((Object) this.bulletRight, (Object) null))
      return;
    this.rightArmAimTo(this.bulletRight.transform.position);
  }

  public void launch(Vector3 des, bool left = true, bool leviMode = false)
  {
    if (left)
    {
      this.isLaunchLeft = true;
      this.launchElapsedTimeL = 0.0f;
    }
    else
    {
      this.isLaunchRight = true;
      this.launchElapsedTimeR = 0.0f;
    }
    if (this.state == HERO_STATE.Grab)
      return;
    if (this.isMounted)
      this.unmounted();
    if (this.state != HERO_STATE.Attack)
      this.idle();
    Vector3 vector3_1 = Vector3.op_Subtraction(des, ((Component) this).transform.position);
    if (left)
      this.launchPointLeft = des;
    else
      this.launchPointRight = des;
    ((Vector3) ref vector3_1).Normalize();
    Vector3 vector3_2 = Vector3.op_Multiply(vector3_1, 20f);
    if (Object.op_Inequality((Object) this.bulletLeft, (Object) null) && Object.op_Inequality((Object) this.bulletRight, (Object) null) && this.bulletLeft.GetComponent<Bullet>().isHooked() && this.bulletRight.GetComponent<Bullet>().isHooked())
      vector3_2 = Vector3.op_Multiply(vector3_2, 0.8f);
    leviMode = ((Component) this).animation.IsPlaying("attack5") || ((Component) this).animation.IsPlaying("special_petra");
    if (!leviMode)
    {
      this.falseAttack();
      this.idle();
      if (this.useGun)
        this.crossFade("AHSS_hook_forward_both", 0.1f);
      else if (left && !this.isRightHandHooked)
        this.crossFade("air_hook_l_just", 0.1f);
      else if (!left && !this.isLeftHandHooked)
      {
        this.crossFade("air_hook_r_just", 0.1f);
      }
      else
      {
        this.crossFade("dash", 0.1f);
        ((Component) this).animation["dash"].time = 0.0f;
      }
    }
    this.launchForce = vector3_2;
    if (!leviMode)
    {
      if ((double) vector3_2.y < 30.0)
        this.launchForce = Vector3.op_Addition(this.launchForce, Vector3.op_Multiply(Vector3.up, 30f - vector3_2.y));
      if ((double) des.y >= (double) ((Component) this).transform.position.y)
        this.launchForce = Vector3.op_Addition(this.launchForce, Vector3.op_Multiply(Vector3.op_Multiply(Vector3.up, des.y - ((Component) this).transform.position.y), 10f));
      ((Component) this).rigidbody.AddForce(this.launchForce);
    }
    this.facingDirection = Mathf.Atan2(this.launchForce.x, this.launchForce.z) * 57.29578f;
    Quaternion quaternion = Quaternion.Euler(0.0f, this.facingDirection, 0.0f);
    ((Component) this).gameObject.transform.rotation = quaternion;
    ((Component) this).rigidbody.rotation = quaternion;
    this.targetRotation = quaternion;
    if (leviMode)
      this.launchElapsedTimeR = -100f;
    if (((Component) this).animation.IsPlaying("special_petra"))
    {
      this.launchElapsedTimeR = -100f;
      this.launchElapsedTimeL = -100f;
      if (Object.op_Inequality((Object) this.bulletRight, (Object) null))
      {
        this.bulletRight.GetComponent<Bullet>().disable();
        this.releaseIfIHookSb();
      }
      if (Object.op_Inequality((Object) this.bulletLeft, (Object) null))
      {
        this.bulletLeft.GetComponent<Bullet>().disable();
        this.releaseIfIHookSb();
      }
    }
    this._cancelGasDisable = true;
    this.sparks.enableEmission = false;
  }

  private void launchLeftRope(RaycastHit hit, bool single, int mode = 0)
  {
    if ((double) this.currentGas == 0.0)
      return;
    this.useGas();
    if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
      this.bulletLeft = (GameObject) Object.Instantiate(Resources.Load("hook"));
    else if (this.photonView.isMine)
      this.bulletLeft = PhotonNetwork.Instantiate("hook", ((Component) this).transform.position, ((Component) this).transform.rotation, 0);
    GameObject gameObject = !this.useGun ? this.hookRefL1 : this.hookRefL2;
    string launcher_ref = !this.useGun ? "hookRefL1" : "hookRefL2";
    this.bulletLeft.transform.position = gameObject.transform.position;
    Bullet component = this.bulletLeft.GetComponent<Bullet>();
    float num = !single ? ((double) ((RaycastHit) ref hit).distance <= 50.0 ? ((RaycastHit) ref hit).distance * 0.05f : ((RaycastHit) ref hit).distance * 0.3f) : 0.0f;
    Vector3 vector3 = Vector3.op_Subtraction(Vector3.op_Subtraction(((RaycastHit) ref hit).point, Vector3.op_Multiply(((Component) this).transform.right, num)), this.bulletLeft.transform.position);
    ((Vector3) ref vector3).Normalize();
    if (mode == 1)
      component.launch(Vector3.op_Multiply(vector3, 3f), ((Component) this).rigidbody.velocity, launcher_ref, true, ((Component) this).gameObject, true);
    else
      component.launch(Vector3.op_Multiply(vector3, 3f), ((Component) this).rigidbody.velocity, launcher_ref, true, ((Component) this).gameObject);
    this.launchPointLeft = Vector3.zero;
  }

  private void launchRightRope(RaycastHit hit, bool single, int mode = 0)
  {
    if ((double) this.currentGas == 0.0)
      return;
    this.useGas();
    if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
      this.bulletRight = (GameObject) Object.Instantiate(Resources.Load("hook"));
    else if (this.photonView.isMine)
      this.bulletRight = PhotonNetwork.Instantiate("hook", ((Component) this).transform.position, ((Component) this).transform.rotation, 0);
    GameObject gameObject = !this.useGun ? this.hookRefR1 : this.hookRefR2;
    string launcher_ref = !this.useGun ? "hookRefR1" : "hookRefR2";
    this.bulletRight.transform.position = gameObject.transform.position;
    Bullet component = this.bulletRight.GetComponent<Bullet>();
    float num = !single ? ((double) ((RaycastHit) ref hit).distance <= 50.0 ? ((RaycastHit) ref hit).distance * 0.05f : ((RaycastHit) ref hit).distance * 0.3f) : 0.0f;
    Vector3 vector3 = Vector3.op_Subtraction(Vector3.op_Addition(((RaycastHit) ref hit).point, Vector3.op_Multiply(((Component) this).transform.right, num)), this.bulletRight.transform.position);
    ((Vector3) ref vector3).Normalize();
    if (mode == 1)
      component.launch(Vector3.op_Multiply(vector3, 5f), ((Component) this).rigidbody.velocity, launcher_ref, false, ((Component) this).gameObject, true);
    else
      component.launch(Vector3.op_Multiply(vector3, 3f), ((Component) this).rigidbody.velocity, launcher_ref, false, ((Component) this).gameObject);
    this.launchPointRight = Vector3.zero;
  }

  private void leftArmAimTo(Vector3 target)
  {
    float num1 = target.x - ((Component) this.upperarmL).transform.position.x;
    float num2 = target.y - ((Component) this.upperarmL).transform.position.y;
    float num3 = target.z - ((Component) this.upperarmL).transform.position.z;
    float num4 = Mathf.Sqrt((float) ((double) num1 * (double) num1 + (double) num3 * (double) num3));
    this.handL.localRotation = Quaternion.Euler(90f, 0.0f, 0.0f);
    this.forearmL.localRotation = Quaternion.Euler(-90f, 0.0f, 0.0f);
    this.upperarmL.rotation = Quaternion.Euler(0.0f, (float) (90.0 + (double) Mathf.Atan2(num1, num3) * 57.295780181884766), (float) (-(double) Mathf.Atan2(num2, num4) * 57.295780181884766));
  }

  public void loadskin()
  {
    if (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE && !this.photonView.isMine || !SettingsManager.CustomSkinSettings.Human.SkinsEnabled.Value)
      return;
    HumanCustomSkinSet selectedSet = (HumanCustomSkinSet) SettingsManager.CustomSkinSettings.Human.GetSelectedSet();
    string url = string.Join(",", new string[19]
    {
      selectedSet.Horse.Value,
      selectedSet.Hair.Value,
      selectedSet.Eye.Value,
      selectedSet.Glass.Value,
      selectedSet.Face.Value,
      selectedSet.Skin.Value,
      selectedSet.Costume.Value,
      selectedSet.Logo.Value,
      selectedSet.GearL.Value,
      selectedSet.GearR.Value,
      selectedSet.Gas.Value,
      selectedSet.Hoodie.Value,
      selectedSet.WeaponTrail.Value,
      selectedSet.ThunderSpearL.Value,
      selectedSet.ThunderSpearR.Value,
      selectedSet.HookL.Value,
      selectedSet.HookLTiling.Value.ToString(),
      selectedSet.HookR.Value,
      selectedSet.HookRTiling.Value.ToString()
    });
    if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
    {
      this.StartCoroutine(this.loadskinE(-1, url));
    }
    else
    {
      int num = -1;
      if (Object.op_Inequality((Object) this.myHorse, (Object) null))
        num = this.myHorse.GetPhotonView().viewID;
      this.photonView.RPC("loadskinRPC", PhotonTargets.AllBuffered, (object) num, (object) url);
    }
  }

  public IEnumerator loadskinE(int horse, string url)
  {
    while (!this._hasRunStart)
      yield return (object) null;
    this._customSkinLoader.StartCoroutine(this._customSkinLoader.LoadSkinsFromRPC(new object[2]
    {
      (object) horse,
      (object) url
    }));
  }

  [RPC]
  public void loadskinRPC(int horse, string url, PhotonMessageInfo info)
  {
    if (info.sender != this.photonView.owner)
      return;
    HumanCustomSkinSettings human = SettingsManager.CustomSkinSettings.Human;
    if (!human.SkinsEnabled.Value || human.SkinsLocal.Value && !this.photonView.isMine)
      return;
    this.StartCoroutine(this.loadskinE(horse, url));
  }

  public void markDie()
  {
    this.hasDied = true;
    this.state = HERO_STATE.Die;
  }

  [RPC]
  public void moveToRPC(float posX, float posY, float posZ, PhotonMessageInfo info)
  {
    if (info == null || !info.sender.isMasterClient)
      return;
    ((Component) this).transform.position = new Vector3(posX, posY, posZ);
  }

  [RPC]
  private void net3DMGSMOKE(bool ifON, PhotonMessageInfo info)
  {
    if (info != null && info.sender != this.photonView.owner)
    {
      FengGameManagerMKII.instance.kickPlayerRCIfMC(info.sender, true, "hero net3dmgsmoke exploit");
    }
    else
    {
      if (!Object.op_Inequality((Object) this.smoke_3dmg, (Object) null))
        return;
      this.smoke_3dmg.enableEmission = ifON;
    }
  }

  [RPC]
  private void netContinueAnimation(PhotonMessageInfo info)
  {
    if (info != null && info.sender != this.photonView.owner)
      FengGameManagerMKII.instance.kickPlayerRCIfMC(info.sender, true, "hero continueanimation exploit");
    foreach (AnimationState animationState in ((Component) this).animation)
    {
      if ((double) animationState.speed == 1.0)
        return;
      animationState.speed = 1f;
    }
    this.playAnimation(this.currentPlayingClipName());
  }

  [RPC]
  private void netCrossFade(string aniName, float time, PhotonMessageInfo info)
  {
    if (info != null && info.sender != this.photonView.owner)
    {
      FengGameManagerMKII.instance.kickPlayerRCIfMC(info.sender, true, "hero netCrossFade exploit");
    }
    else
    {
      this.currentAnimation = aniName;
      if (!Object.op_Inequality((Object) ((Component) this).animation, (Object) null))
        return;
      ((Component) this).animation.CrossFade(aniName, time);
    }
  }

  [RPC]
  public void netDie(
    Vector3 v,
    bool isBite,
    int viewID = -1,
    string titanName = "",
    bool killByTitan = true,
    PhotonMessageInfo info = null)
  {
    if (this.photonView.isMine && info != null && IN_GAME_MAIN_CAMERA.gamemode != GAMEMODE.BOSS_FIGHT_CT)
    {
      if (FengGameManagerMKII.ignoreList.Contains(info.sender.ID))
      {
        this.photonView.RPC("backToHumanRPC", PhotonTargets.Others);
        return;
      }
      if (!info.sender.isLocal && !info.sender.isMasterClient)
      {
        if (info.sender.customProperties[(object) PhotonPlayerProperty.name] == null || info.sender.customProperties[(object) PhotonPlayerProperty.isTitan] == null)
          FengGameManagerMKII.instance.chatRoom.addLINE("<color=#FFCC00>Unusual Kill from ID " + info.sender.ID.ToString() + "</color>");
        else if (viewID < 0)
        {
          if (titanName == "")
            FengGameManagerMKII.instance.chatRoom.addLINE("<color=#FFCC00>Unusual Kill from ID " + info.sender.ID.ToString() + " (possibly valid).</color>");
          else
            FengGameManagerMKII.instance.chatRoom.addLINE("<color=#FFCC00>Unusual Kill from ID " + info.sender.ID.ToString() + "</color>");
        }
        else if (Object.op_Equality((Object) PhotonView.Find(viewID), (Object) null))
          FengGameManagerMKII.instance.chatRoom.addLINE("<color=#FFCC00>Unusual Kill from ID " + info.sender.ID.ToString() + "</color>");
        else if (PhotonView.Find(viewID).owner.ID != info.sender.ID)
          FengGameManagerMKII.instance.chatRoom.addLINE("<color=#FFCC00>Unusual Kill from ID " + info.sender.ID.ToString() + "</color>");
      }
    }
    if (PhotonNetwork.isMasterClient)
    {
      this.onDeathEvent(viewID, killByTitan);
      int id = this.photonView.owner.ID;
      if (((Dictionary<object, object>) FengGameManagerMKII.heroHash).ContainsKey((object) id))
        ((Dictionary<object, object>) FengGameManagerMKII.heroHash).Remove((object) id);
    }
    if (this.photonView.isMine)
    {
      Vector3 vector3 = Vector3.op_Multiply(Vector3.up, 5000f);
      if (Object.op_Inequality((Object) this.myBomb, (Object) null))
        this.myBomb.DestroySelf();
      if (Object.op_Inequality((Object) this.myCannon, (Object) null))
        PhotonNetwork.Destroy(this.myCannon);
      if (this.titanForm && Object.op_Inequality((Object) this.eren_titan, (Object) null))
        this.eren_titan.GetComponent<TITAN_EREN>().lifeTime = 0.1f;
      if (Object.op_Inequality((Object) this.skillCD, (Object) null))
        this.skillCD.transform.localPosition = vector3;
    }
    if (Object.op_Inequality((Object) this.bulletLeft, (Object) null))
      this.bulletLeft.GetComponent<Bullet>().removeMe();
    if (Object.op_Inequality((Object) this.bulletRight, (Object) null))
      this.bulletRight.GetComponent<Bullet>().removeMe();
    this.meatDie.Play();
    if (!this.useGun && (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE || this.photonView.isMine))
    {
      this.leftbladetrail.Deactivate();
      this.rightbladetrail.Deactivate();
      this.leftbladetrail2.Deactivate();
      this.rightbladetrail2.Deactivate();
    }
    this.falseAttack();
    this.breakApart2(v, isBite);
    if (this.photonView.isMine)
    {
      ((Component) this.currentCamera).GetComponent<IN_GAME_MAIN_CAMERA>().setSpectorMode(false);
      ((Component) this.currentCamera).GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = true;
      FengGameManagerMKII.instance.myRespawnTime = 0.0f;
    }
    this.hasDied = true;
    Transform transform = ((Component) this).transform.Find("audio_die");
    if (Object.op_Inequality((Object) transform, (Object) null))
    {
      transform.parent = (Transform) null;
      ((Component) transform).GetComponent<AudioSource>().Play();
    }
    ((Component) this).gameObject.GetComponent<SmoothSyncMovement>().disabled = true;
    if (this.photonView.isMine)
    {
      PhotonNetwork.RemoveRPCs(this.photonView);
      Hashtable propertiesToSet1 = new Hashtable();
      ((Dictionary<object, object>) propertiesToSet1).Add((object) PhotonPlayerProperty.dead, (object) true);
      PhotonNetwork.player.SetCustomProperties(propertiesToSet1);
      Hashtable propertiesToSet2 = new Hashtable();
      ((Dictionary<object, object>) propertiesToSet2).Add((object) PhotonPlayerProperty.deaths, (object) (RCextensions.returnIntFromObject(PhotonNetwork.player.customProperties[(object) PhotonPlayerProperty.deaths]) + 1));
      PhotonNetwork.player.SetCustomProperties(propertiesToSet2);
      object[] objArray = new object[1]
      {
        (object) (!(titanName == string.Empty) ? 1 : 0)
      };
      FengGameManagerMKII.instance.photonView.RPC("someOneIsDead", PhotonTargets.MasterClient, objArray);
      if (viewID != -1)
      {
        PhotonView photonView = PhotonView.Find(viewID);
        if (Object.op_Inequality((Object) photonView, (Object) null))
        {
          FengGameManagerMKII.instance.sendKillInfo(killByTitan, "[FFC000][" + info.sender.ID.ToString() + "][FFFFFF]" + RCextensions.returnStringFromObject(photonView.owner.customProperties[(object) PhotonPlayerProperty.name]), false, RCextensions.returnStringFromObject(PhotonNetwork.player.customProperties[(object) PhotonPlayerProperty.name]));
          Hashtable propertiesToSet3 = new Hashtable();
          ((Dictionary<object, object>) propertiesToSet3).Add((object) PhotonPlayerProperty.kills, (object) (RCextensions.returnIntFromObject(photonView.owner.customProperties[(object) PhotonPlayerProperty.kills]) + 1));
          photonView.owner.SetCustomProperties(propertiesToSet3);
        }
      }
      else
        FengGameManagerMKII.instance.sendKillInfo(!(titanName == string.Empty), "[FFC000][" + info.sender.ID.ToString() + "][FFFFFF]" + titanName, false, RCextensions.returnStringFromObject(PhotonNetwork.player.customProperties[(object) PhotonPlayerProperty.name]));
    }
    if (this.photonView.isMine)
      PhotonNetwork.Destroy(this.photonView);
    if (viewID == -1)
      return;
    PhotonView photonView1 = PhotonView.Find(viewID);
    if (!Object.op_Inequality((Object) photonView1, (Object) null) || !photonView1.isMine || !Object.op_Inequality((Object) ((Component) photonView1).GetComponent<TITAN>(), (Object) null))
      return;
    GameProgressManager.RegisterHumanKill(((Component) photonView1).gameObject, this, KillWeapon.Titan);
  }

  [RPC]
  private void netDie2(int viewID = -1, string titanName = "", PhotonMessageInfo info = null)
  {
    if (this.photonView.isMine && info != null && IN_GAME_MAIN_CAMERA.gamemode != GAMEMODE.BOSS_FIGHT_CT)
    {
      if (FengGameManagerMKII.ignoreList.Contains(info.sender.ID))
      {
        this.photonView.RPC("backToHumanRPC", PhotonTargets.Others);
        return;
      }
      if (!info.sender.isLocal && !info.sender.isMasterClient)
      {
        if (info.sender.customProperties[(object) PhotonPlayerProperty.name] == null || info.sender.customProperties[(object) PhotonPlayerProperty.isTitan] == null)
          FengGameManagerMKII.instance.chatRoom.addLINE("<color=#FFCC00>Unusual Kill from ID " + info.sender.ID.ToString() + "</color>");
        else if (viewID < 0)
        {
          if (titanName == "")
            FengGameManagerMKII.instance.chatRoom.addLINE("<color=#FFCC00>Unusual Kill from ID " + info.sender.ID.ToString() + " (possibly valid).</color>");
          else if (!SettingsManager.LegacyGameSettings.BombModeEnabled.Value && !SettingsManager.LegacyGameSettings.CannonsFriendlyFire.Value)
            FengGameManagerMKII.instance.chatRoom.addLINE("<color=#FFCC00>Unusual Kill from ID " + info.sender.ID.ToString() + "</color>");
        }
        else if (Object.op_Equality((Object) PhotonView.Find(viewID), (Object) null))
          FengGameManagerMKII.instance.chatRoom.addLINE("<color=#FFCC00>Unusual Kill from ID " + info.sender.ID.ToString() + "</color>");
        else if (PhotonView.Find(viewID).owner.ID != info.sender.ID)
          FengGameManagerMKII.instance.chatRoom.addLINE("<color=#FFCC00>Unusual Kill from ID " + info.sender.ID.ToString() + "</color>");
      }
    }
    if (this.photonView.isMine)
    {
      Vector3 vector3 = Vector3.op_Multiply(Vector3.up, 5000f);
      if (Object.op_Inequality((Object) this.myBomb, (Object) null))
        this.myBomb.DestroySelf();
      if (Object.op_Inequality((Object) this.myCannon, (Object) null))
        PhotonNetwork.Destroy(this.myCannon);
      PhotonNetwork.RemoveRPCs(this.photonView);
      if (this.titanForm && Object.op_Inequality((Object) this.eren_titan, (Object) null))
        this.eren_titan.GetComponent<TITAN_EREN>().lifeTime = 0.1f;
      if (Object.op_Inequality((Object) this.skillCD, (Object) null))
        this.skillCD.transform.localPosition = vector3;
    }
    this.meatDie.Play();
    if (Object.op_Inequality((Object) this.bulletLeft, (Object) null))
      this.bulletLeft.GetComponent<Bullet>().removeMe();
    if (Object.op_Inequality((Object) this.bulletRight, (Object) null))
      this.bulletRight.GetComponent<Bullet>().removeMe();
    Transform transform = ((Component) this).transform.Find("audio_die");
    transform.parent = (Transform) null;
    ((Component) transform).GetComponent<AudioSource>().Play();
    if (this.photonView.isMine)
    {
      ((Component) this.currentCamera).GetComponent<IN_GAME_MAIN_CAMERA>().setMainObject((GameObject) null);
      ((Component) this.currentCamera).GetComponent<IN_GAME_MAIN_CAMERA>().setSpectorMode(true);
      ((Component) this.currentCamera).GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = true;
      FengGameManagerMKII.instance.myRespawnTime = 0.0f;
    }
    this.falseAttack();
    this.hasDied = true;
    ((Component) this).gameObject.GetComponent<SmoothSyncMovement>().disabled = true;
    if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER && this.photonView.isMine)
    {
      PhotonNetwork.RemoveRPCs(this.photonView);
      Hashtable propertiesToSet1 = new Hashtable();
      ((Dictionary<object, object>) propertiesToSet1).Add((object) PhotonPlayerProperty.dead, (object) true);
      PhotonNetwork.player.SetCustomProperties(propertiesToSet1);
      Hashtable propertiesToSet2 = new Hashtable();
      ((Dictionary<object, object>) propertiesToSet2).Add((object) PhotonPlayerProperty.deaths, (object) ((int) PhotonNetwork.player.customProperties[(object) PhotonPlayerProperty.deaths] + 1));
      PhotonNetwork.player.SetCustomProperties(propertiesToSet2);
      if (viewID != -1)
      {
        PhotonView photonView = PhotonView.Find(viewID);
        if (Object.op_Inequality((Object) photonView, (Object) null))
        {
          FengGameManagerMKII.instance.sendKillInfo(true, "[FFC000][" + info.sender.ID.ToString() + "][FFFFFF]" + RCextensions.returnStringFromObject(photonView.owner.customProperties[(object) PhotonPlayerProperty.name]), false, RCextensions.returnStringFromObject(PhotonNetwork.player.customProperties[(object) PhotonPlayerProperty.name]));
          Hashtable propertiesToSet3 = new Hashtable();
          ((Dictionary<object, object>) propertiesToSet3).Add((object) PhotonPlayerProperty.kills, (object) (RCextensions.returnIntFromObject(photonView.owner.customProperties[(object) PhotonPlayerProperty.kills]) + 1));
          photonView.owner.SetCustomProperties(propertiesToSet3);
        }
      }
      else
        FengGameManagerMKII.instance.sendKillInfo(true, "[FFC000][" + info.sender.ID.ToString() + "][FFFFFF]" + titanName, false, RCextensions.returnStringFromObject(PhotonNetwork.player.customProperties[(object) PhotonPlayerProperty.name]));
      object[] objArray = new object[1]
      {
        (object) (!(titanName == string.Empty) ? 1 : 0)
      };
      FengGameManagerMKII.instance.photonView.RPC("someOneIsDead", PhotonTargets.MasterClient, objArray);
    }
    (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE || !this.photonView.isMine ? (GameObject) Object.Instantiate(Resources.Load("hitMeat2")) : PhotonNetwork.Instantiate("hitMeat2", ((Component) this).transform.position, Quaternion.Euler(270f, 0.0f, 0.0f), 0)).transform.position = ((Component) this).transform.position;
    if (this.photonView.isMine)
      PhotonNetwork.Destroy(this.photonView);
    if (PhotonNetwork.isMasterClient)
    {
      this.onDeathEvent(viewID, true);
      int id = this.photonView.owner.ID;
      if (((Dictionary<object, object>) FengGameManagerMKII.heroHash).ContainsKey((object) id))
        ((Dictionary<object, object>) FengGameManagerMKII.heroHash).Remove((object) id);
    }
    if (viewID == -1)
      return;
    PhotonView photonView1 = PhotonView.Find(viewID);
    if (!Object.op_Inequality((Object) photonView1, (Object) null) || !photonView1.isMine || !Object.op_Inequality((Object) ((Component) photonView1).GetComponent<TITAN>(), (Object) null))
      return;
    GameProgressManager.RegisterHumanKill(((Component) photonView1).gameObject, this, KillWeapon.Titan);
  }

  public void netDieLocal(Vector3 v, bool isBite, int viewID = -1, string titanName = "", bool killByTitan = true)
  {
    if (this.photonView.isMine)
    {
      Vector3 vector3 = Vector3.op_Multiply(Vector3.up, 5000f);
      if (this.titanForm && Object.op_Inequality((Object) this.eren_titan, (Object) null))
        this.eren_titan.GetComponent<TITAN_EREN>().lifeTime = 0.1f;
      if (Object.op_Inequality((Object) this.myBomb, (Object) null))
        this.myBomb.DestroySelf();
      if (Object.op_Inequality((Object) this.myCannon, (Object) null))
        PhotonNetwork.Destroy(this.myCannon);
      if (Object.op_Inequality((Object) this.skillCD, (Object) null))
        this.skillCD.transform.localPosition = vector3;
    }
    if (Object.op_Inequality((Object) this.bulletLeft, (Object) null))
      this.bulletLeft.GetComponent<Bullet>().removeMe();
    if (Object.op_Inequality((Object) this.bulletRight, (Object) null))
      this.bulletRight.GetComponent<Bullet>().removeMe();
    this.meatDie.Play();
    if (!this.useGun && (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE || this.photonView.isMine))
    {
      this.leftbladetrail.Deactivate();
      this.rightbladetrail.Deactivate();
      this.leftbladetrail2.Deactivate();
      this.rightbladetrail2.Deactivate();
    }
    this.falseAttack();
    this.breakApart2(v, isBite);
    if (this.photonView.isMine)
    {
      ((Component) this.currentCamera).GetComponent<IN_GAME_MAIN_CAMERA>().setSpectorMode(false);
      ((Component) this.currentCamera).GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = true;
      FengGameManagerMKII.instance.myRespawnTime = 0.0f;
    }
    this.hasDied = true;
    Transform transform = ((Component) this).transform.Find("audio_die");
    transform.parent = (Transform) null;
    ((Component) transform).GetComponent<AudioSource>().Play();
    ((Component) this).gameObject.GetComponent<SmoothSyncMovement>().disabled = true;
    if (this.photonView.isMine)
    {
      PhotonNetwork.RemoveRPCs(this.photonView);
      Hashtable propertiesToSet1 = new Hashtable();
      ((Dictionary<object, object>) propertiesToSet1).Add((object) PhotonPlayerProperty.dead, (object) true);
      PhotonNetwork.player.SetCustomProperties(propertiesToSet1);
      Hashtable propertiesToSet2 = new Hashtable();
      ((Dictionary<object, object>) propertiesToSet2).Add((object) PhotonPlayerProperty.deaths, (object) (RCextensions.returnIntFromObject(PhotonNetwork.player.customProperties[(object) PhotonPlayerProperty.deaths]) + 1));
      PhotonNetwork.player.SetCustomProperties(propertiesToSet2);
      object[] objArray = new object[1]
      {
        (object) (!(titanName == string.Empty) ? 1 : 0)
      };
      FengGameManagerMKII.instance.photonView.RPC("someOneIsDead", PhotonTargets.MasterClient, objArray);
      if (viewID != -1)
      {
        PhotonView photonView = PhotonView.Find(viewID);
        if (Object.op_Inequality((Object) photonView, (Object) null))
        {
          FengGameManagerMKII.instance.sendKillInfo(killByTitan, RCextensions.returnStringFromObject(photonView.owner.customProperties[(object) PhotonPlayerProperty.name]), false, RCextensions.returnStringFromObject(PhotonNetwork.player.customProperties[(object) PhotonPlayerProperty.name]));
          Hashtable propertiesToSet3 = new Hashtable();
          ((Dictionary<object, object>) propertiesToSet3).Add((object) PhotonPlayerProperty.kills, (object) (RCextensions.returnIntFromObject(photonView.owner.customProperties[(object) PhotonPlayerProperty.kills]) + 1));
          photonView.owner.SetCustomProperties(propertiesToSet3);
        }
      }
      else
        FengGameManagerMKII.instance.sendKillInfo(!(titanName == string.Empty), titanName, false, RCextensions.returnStringFromObject(PhotonNetwork.player.customProperties[(object) PhotonPlayerProperty.name]));
    }
    if (this.photonView.isMine)
      PhotonNetwork.Destroy(this.photonView);
    if (!PhotonNetwork.isMasterClient)
      return;
    this.onDeathEvent(viewID, killByTitan);
    int id = this.photonView.owner.ID;
    if (!((Dictionary<object, object>) FengGameManagerMKII.heroHash).ContainsKey((object) id))
      return;
    ((Dictionary<object, object>) FengGameManagerMKII.heroHash).Remove((object) id);
  }

  [RPC]
  private void netGrabbed(int id, bool leftHand, PhotonMessageInfo info)
  {
    if (info != null && !info.sender.isMasterClient && (RCextensions.returnIntFromObject(info.sender.customProperties[(object) PhotonPlayerProperty.isTitan]) != 2 || RCextensions.returnBoolFromObject(info.sender.customProperties[(object) PhotonPlayerProperty.dead])))
    {
      FengGameManagerMKII.instance.kickPlayerRCIfMC(info.sender, true, "hero netGrabbed exploit");
    }
    else
    {
      this.titanWhoGrabMeID = id;
      this.grabbed(((Component) PhotonView.Find(id)).gameObject, leftHand);
    }
  }

  [RPC]
  private void netlaughAttack(PhotonMessageInfo info)
  {
    if (info != null && info.sender != this.photonView.owner)
      FengGameManagerMKII.instance.kickPlayerRCIfMC(info.sender, true, "hero netlaughattack exploit");
    foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("titan"))
    {
      if ((double) Vector3.Distance(gameObject.transform.position, ((Component) this).transform.position) < 50.0 && (double) Vector3.Angle(gameObject.transform.forward, Vector3.op_Subtraction(((Component) this).transform.position, gameObject.transform.position)) < 90.0 && Object.op_Inequality((Object) gameObject.GetComponent<TITAN>(), (Object) null))
        gameObject.GetComponent<TITAN>().beLaughAttacked();
    }
  }

  [RPC]
  private void netPauseAnimation(PhotonMessageInfo info)
  {
    if (info != null && info.sender != this.photonView.owner)
    {
      FengGameManagerMKII.instance.kickPlayerRCIfMC(info.sender, true, "hero netPauseAniamtion");
    }
    else
    {
      foreach (AnimationState animationState in ((Component) this).animation)
        animationState.speed = 0.0f;
    }
  }

  [RPC]
  private void netPlayAnimation(string aniName, PhotonMessageInfo info)
  {
    if (info != null && info.sender != this.photonView.owner && aniName != "grabbed")
    {
      FengGameManagerMKII.instance.kickPlayerRCIfMC(info.sender, true, "hero netPlayAnimation exploit");
    }
    else
    {
      this.currentAnimation = aniName;
      if (!Object.op_Inequality((Object) ((Component) this).animation, (Object) null))
        return;
      ((Component) this).animation.Play(aniName);
    }
  }

  [RPC]
  private void netPlayAnimationAt(string aniName, float normalizedTime, PhotonMessageInfo info)
  {
    if (info != null && info.sender != this.photonView.owner)
    {
      FengGameManagerMKII.instance.kickPlayerRCIfMC(info.sender, true, "hero netPlayAnimationAt exploit");
    }
    else
    {
      this.currentAnimation = aniName;
      if (!Object.op_Inequality((Object) ((Component) this).animation, (Object) null))
        return;
      ((Component) this).animation.Play(aniName);
      ((Component) this).animation[aniName].normalizedTime = normalizedTime;
    }
  }

  [RPC]
  private void netSetIsGrabbedFalse(PhotonMessageInfo info)
  {
    if (info != null && info.sender != this.photonView.owner)
      FengGameManagerMKII.instance.kickPlayerRCIfMC(info.sender, true, "hero netSetIsGrabbedFalse");
    else
      this.state = HERO_STATE.Idle;
  }

  [RPC]
  private void netTauntAttack(float tauntTime, float distance, PhotonMessageInfo info)
  {
    if (info != null && info.sender != this.photonView.owner)
    {
      FengGameManagerMKII.instance.kickPlayerRCIfMC(info.sender, true, "hero netTauntAttack");
    }
    else
    {
      foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("titan"))
      {
        if ((double) Vector3.Distance(gameObject.transform.position, ((Component) this).transform.position) < (double) distance && Object.op_Inequality((Object) gameObject.GetComponent<TITAN>(), (Object) null))
          gameObject.GetComponent<TITAN>().beTauntedBy(((Component) this).gameObject, tauntTime);
      }
    }
  }

  [RPC]
  public void netUngrabbed()
  {
    this.ungrabbed();
    this.netPlayAnimation(this.standAnimation, (PhotonMessageInfo) null);
    this.falseAttack();
  }

  public void onDeathEvent(int viewID, bool isTitan)
  {
    if (isTitan)
    {
      if (!((Dictionary<object, object>) FengGameManagerMKII.RCEvents).ContainsKey((object) "OnPlayerDieByTitan"))
        return;
      RCEvent rcEvent = (RCEvent) FengGameManagerMKII.RCEvents[(object) "OnPlayerDieByTitan"];
      string[] rcVariableName = (string[]) FengGameManagerMKII.RCVariableNames[(object) "OnPlayerDieByTitan"];
      if (((Dictionary<object, object>) FengGameManagerMKII.playerVariables).ContainsKey((object) rcVariableName[0]))
        FengGameManagerMKII.playerVariables[(object) rcVariableName[0]] = (object) this.photonView.owner;
      else
        ((Dictionary<object, object>) FengGameManagerMKII.playerVariables).Add((object) rcVariableName[0], (object) this.photonView.owner);
      if (((Dictionary<object, object>) FengGameManagerMKII.titanVariables).ContainsKey((object) rcVariableName[1]))
        FengGameManagerMKII.titanVariables[(object) rcVariableName[1]] = (object) ((Component) PhotonView.Find(viewID)).gameObject.GetComponent<TITAN>();
      else
        ((Dictionary<object, object>) FengGameManagerMKII.titanVariables).Add((object) rcVariableName[1], (object) ((Component) PhotonView.Find(viewID)).gameObject.GetComponent<TITAN>());
      rcEvent.checkEvent();
    }
    else
    {
      if (!((Dictionary<object, object>) FengGameManagerMKII.RCEvents).ContainsKey((object) "OnPlayerDieByPlayer"))
        return;
      RCEvent rcEvent = (RCEvent) FengGameManagerMKII.RCEvents[(object) "OnPlayerDieByPlayer"];
      string[] rcVariableName = (string[]) FengGameManagerMKII.RCVariableNames[(object) "OnPlayerDieByPlayer"];
      if (((Dictionary<object, object>) FengGameManagerMKII.playerVariables).ContainsKey((object) rcVariableName[0]))
        FengGameManagerMKII.playerVariables[(object) rcVariableName[0]] = (object) this.photonView.owner;
      else
        ((Dictionary<object, object>) FengGameManagerMKII.playerVariables).Add((object) rcVariableName[0], (object) this.photonView.owner);
      if (((Dictionary<object, object>) FengGameManagerMKII.playerVariables).ContainsKey((object) rcVariableName[1]))
        FengGameManagerMKII.playerVariables[(object) rcVariableName[1]] = (object) PhotonView.Find(viewID).owner;
      else
        ((Dictionary<object, object>) FengGameManagerMKII.playerVariables).Add((object) rcVariableName[1], (object) PhotonView.Find(viewID).owner);
      rcEvent.checkEvent();
    }
  }

  private void OnDestroy()
  {
    if (Object.op_Inequality((Object) this.myNetWorkName, (Object) null))
      Object.Destroy((Object) this.myNetWorkName);
    if (Object.op_Inequality((Object) this.gunDummy, (Object) null))
      Object.Destroy((Object) this.gunDummy);
    if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)
      this.releaseIfIHookSb();
    if (Object.op_Inequality((Object) GameObject.Find("MultiplayerManager"), (Object) null))
      GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().removeHero(this);
    if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER && this.photonView.isMine)
    {
      Vector3 vector3 = Vector3.op_Multiply(Vector3.up, 5000f);
      this.cross1.transform.localPosition = vector3;
      this.cross2.transform.localPosition = vector3;
      this.crossL1.transform.localPosition = vector3;
      this.crossL2.transform.localPosition = vector3;
      this.crossR1.transform.localPosition = vector3;
      this.crossR2.transform.localPosition = vector3;
      this.LabelDistance.transform.localPosition = vector3;
    }
    if (Object.op_Inequality((Object) this.setup.part_cape, (Object) null))
      ClothFactory.DisposeObject(this.setup.part_cape);
    if (Object.op_Inequality((Object) this.setup.part_hair_1, (Object) null))
      ClothFactory.DisposeObject(this.setup.part_hair_1);
    if (Object.op_Inequality((Object) this.setup.part_hair_2, (Object) null))
      ClothFactory.DisposeObject(this.setup.part_hair_2);
    if (!this.IsMine())
      return;
    GameMenu.ToggleEmoteWheel(false);
  }

  public void pauseAnimation()
  {
    if (this._animationStopped)
      return;
    this._animationStopped = true;
    foreach (AnimationState animationState in ((Component) this).animation)
      animationState.speed = 0.0f;
    if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE || !this.photonView.isMine)
      return;
    this.photonView.RPC("netPauseAnimation", PhotonTargets.Others);
  }

  public void playAnimation(string aniName)
  {
    this.currentAnimation = aniName;
    ((Component) this).animation.Play(aniName);
    if (!PhotonNetwork.connected || !this.photonView.isMine)
      return;
    this.photonView.RPC("netPlayAnimation", PhotonTargets.Others, (object) aniName);
  }

  private void playAnimationAt(string aniName, float normalizedTime)
  {
    this.currentAnimation = aniName;
    ((Component) this).animation.Play(aniName);
    ((Component) this).animation[aniName].normalizedTime = normalizedTime;
    if (!PhotonNetwork.connected || !this.photonView.isMine)
      return;
    this.photonView.RPC("netPlayAnimationAt", PhotonTargets.Others, (object) aniName, (object) normalizedTime);
  }

  private void releaseIfIHookSb()
  {
    if (!this.hookSomeOne || !Object.op_Inequality((Object) this.hookTarget, (Object) null))
      return;
    this.hookTarget.GetPhotonView().RPC("badGuyReleaseMe", this.hookTarget.GetPhotonView().owner);
    this.hookTarget = (GameObject) null;
    this.hookSomeOne = false;
  }

  public void resetAnimationSpeed()
  {
    foreach (AnimationState animationState in ((Component) this).animation)
      animationState.speed = 1f;
    this.customAnimationSpeed();
  }

  [RPC]
  public void ReturnFromCannon(PhotonMessageInfo info)
  {
    if (info == null || info.sender != this.photonView.owner)
      return;
    this.isCannon = false;
    ((Component) this).gameObject.GetComponent<SmoothSyncMovement>().disabled = false;
  }

  private void rightArmAimTo(Vector3 target)
  {
    float num1 = target.x - ((Component) this.upperarmR).transform.position.x;
    float num2 = target.y - ((Component) this.upperarmR).transform.position.y;
    float num3 = target.z - ((Component) this.upperarmR).transform.position.z;
    float num4 = Mathf.Sqrt((float) ((double) num1 * (double) num1 + (double) num3 * (double) num3));
    this.handR.localRotation = Quaternion.Euler(-90f, 0.0f, 0.0f);
    this.forearmR.localRotation = Quaternion.Euler(90f, 0.0f, 0.0f);
    this.upperarmR.rotation = Quaternion.Euler(180f, (float) (90.0 + (double) Mathf.Atan2(num1, num3) * 57.295780181884766), Mathf.Atan2(num2, num4) * 57.29578f);
  }

  [RPC]
  private void RPCHookedByHuman(int hooker, Vector3 hookPosition)
  {
    this.hookBySomeOne = true;
    this.badGuy = ((Component) PhotonView.Find(hooker)).gameObject;
    if ((double) Vector3.Distance(hookPosition, ((Component) this).transform.position) < 15.0)
    {
      this.launchForce = Vector3.op_Subtraction(((Component) PhotonView.Find(hooker)).gameObject.transform.position, ((Component) this).transform.position);
      ((Component) this).rigidbody.AddForce(Vector3.op_Multiply(Vector3.op_UnaryNegation(((Component) this).rigidbody.velocity), 0.9f), (ForceMode) 2);
      float num = Mathf.Pow(((Vector3) ref this.launchForce).magnitude, 0.1f);
      if (this.grounded)
        ((Component) this).rigidbody.AddForce(Vector3.op_Multiply(Vector3.up, Mathf.Min(((Vector3) ref this.launchForce).magnitude * 0.2f, 10f)), (ForceMode) 1);
      ((Component) this).rigidbody.AddForce(Vector3.op_Multiply(Vector3.op_Multiply(this.launchForce, num), 0.1f), (ForceMode) 1);
      if (this.state == HERO_STATE.Grab)
        return;
      this.dashTime = 1f;
      this.crossFade("dash", 0.05f);
      ((Component) this).animation["dash"].time = 0.1f;
      this.state = HERO_STATE.AirDodge;
      this.falseAttack();
      this.facingDirection = Mathf.Atan2(this.launchForce.x, this.launchForce.z) * 57.29578f;
      Quaternion quaternion = Quaternion.Euler(0.0f, this.facingDirection, 0.0f);
      ((Component) this).gameObject.transform.rotation = quaternion;
      ((Component) this).rigidbody.rotation = quaternion;
      this.targetRotation = quaternion;
    }
    else
    {
      this.hookBySomeOne = false;
      this.badGuy = (GameObject) null;
      PhotonView.Find(hooker).RPC("hookFail", PhotonView.Find(hooker).owner);
    }
  }

  private void setHookedPplDirection()
  {
    this.almostSingleHook = false;
    float facingDirection = this.facingDirection;
    if (this.isRightHandHooked && this.isLeftHandHooked)
    {
      if (Object.op_Inequality((Object) this.bulletLeft, (Object) null) && Object.op_Inequality((Object) this.bulletRight, (Object) null))
      {
        Vector3 vector3_1 = Vector3.op_Subtraction(this.bulletLeft.transform.position, this.bulletRight.transform.position);
        if ((double) ((Vector3) ref vector3_1).sqrMagnitude < 4.0)
        {
          Vector3 vector3_2 = Vector3.op_Subtraction(Vector3.op_Multiply(Vector3.op_Addition(this.bulletLeft.transform.position, this.bulletRight.transform.position), 0.5f), ((Component) this).transform.position);
          this.facingDirection = Mathf.Atan2(vector3_2.x, vector3_2.z) * 57.29578f;
          if (this.useGun && this.state != HERO_STATE.Attack)
            this.facingDirection += -Mathf.DeltaAngle((float) (-(double) Mathf.Atan2(((Component) this).rigidbody.velocity.z, ((Component) this).rigidbody.velocity.x) * 57.295780181884766), (float) (-(double) Mathf.Atan2(vector3_2.z, vector3_2.x) * 57.295780181884766));
          this.almostSingleHook = true;
        }
        else
        {
          Vector3 vector3_3 = Vector3.op_Subtraction(((Component) this).transform.position, this.bulletLeft.transform.position);
          Vector3 vector3_4 = Vector3.op_Subtraction(((Component) this).transform.position, this.bulletRight.transform.position);
          Vector3 vector3_5 = Vector3.op_Multiply(Vector3.op_Addition(this.bulletLeft.transform.position, this.bulletRight.transform.position), 0.5f);
          Vector3 vector3_6 = Vector3.op_Subtraction(((Component) this).transform.position, vector3_5);
          if ((double) Vector3.Angle(vector3_6, vector3_3) < 30.0 && (double) Vector3.Angle(vector3_6, vector3_4) < 30.0)
          {
            this.almostSingleHook = true;
            Vector3 vector3_7 = Vector3.op_Subtraction(vector3_5, ((Component) this).transform.position);
            this.facingDirection = Mathf.Atan2(vector3_7.x, vector3_7.z) * 57.29578f;
          }
          else
          {
            this.almostSingleHook = false;
            Vector3 forward = ((Component) this).transform.forward;
            Vector3.OrthoNormalize(ref vector3_1, ref forward);
            this.facingDirection = Mathf.Atan2(forward.x, forward.z) * 57.29578f;
            if ((double) Mathf.DeltaAngle(Mathf.Atan2(vector3_3.x, vector3_3.z) * 57.29578f, this.facingDirection) > 0.0)
              this.facingDirection += 180f;
          }
        }
      }
    }
    else
    {
      this.almostSingleHook = true;
      Vector3 zero = Vector3.zero;
      Vector3 vector3;
      if (this.isRightHandHooked && Object.op_Inequality((Object) this.bulletRight, (Object) null))
      {
        vector3 = Vector3.op_Subtraction(this.bulletRight.transform.position, ((Component) this).transform.position);
      }
      else
      {
        if (!this.isLeftHandHooked || !Object.op_Inequality((Object) this.bulletLeft, (Object) null))
          return;
        vector3 = Vector3.op_Subtraction(this.bulletLeft.transform.position, ((Component) this).transform.position);
      }
      this.facingDirection = Mathf.Atan2(vector3.x, vector3.z) * 57.29578f;
      if (this.state != HERO_STATE.Attack)
      {
        float num1 = -Mathf.DeltaAngle((float) (-(double) Mathf.Atan2(((Component) this).rigidbody.velocity.z, ((Component) this).rigidbody.velocity.x) * 57.295780181884766), (float) (-(double) Mathf.Atan2(vector3.z, vector3.x) * 57.295780181884766));
        if (this.useGun)
        {
          this.facingDirection += num1;
        }
        else
        {
          float num2 = this.isLeftHandHooked && (double) num1 < 0.0 || this.isRightHandHooked && (double) num1 > 0.0 ? -0.1f : 0.1f;
          this.facingDirection += num1 * num2;
        }
      }
    }
    if (!this.IsFiringThunderSpear())
      return;
    this.facingDirection = facingDirection;
  }

  [RPC]
  public void SetMyCannon(int viewID, PhotonMessageInfo info)
  {
    if (info.sender != this.photonView.owner || viewID < 0)
      FengGameManagerMKII.instance.kickPlayerRCIfMC(info.sender, true, "hero setcannon exploit");
    else if (PhotonView.Find(viewID).owner != info.sender)
    {
      FengGameManagerMKII.instance.kickPlayerRCIfMC(info.sender, true, "hero setcannon exploit");
    }
    else
    {
      if (info.sender != this.photonView.owner)
        return;
      PhotonView photonView = PhotonView.Find(viewID);
      if (!Object.op_Inequality((Object) photonView, (Object) null))
        return;
      this.myCannon = ((Component) photonView).gameObject;
      if (!Object.op_Inequality((Object) this.myCannon, (Object) null))
        return;
      this.myCannonBase = this.myCannon.transform;
      this.myCannonPlayer = this.myCannonBase.Find("PlayerPoint");
      this.isCannon = true;
    }
  }

  [RPC]
  public void SetMyPhotonCamera(float offset, PhotonMessageInfo info)
  {
    if (info == null || this.photonView.owner != info.sender)
      return;
    this.CameraMultiplier = offset;
    ((Component) this).GetComponent<SmoothSyncMovement>().PhotonCamera = true;
    this.isPhotonCamera = true;
  }

  [RPC]
  private void setMyTeam(int val)
  {
    this.myTeam = val;
    if (Object.op_Inequality((Object) this.checkBoxLeft, (Object) null))
      this.checkBoxLeft.GetComponent<TriggerColliderWeapon>().myTeam = val;
    if (Object.op_Inequality((Object) this.checkBoxRight, (Object) null))
      this.checkBoxRight.GetComponent<TriggerColliderWeapon>().myTeam = val;
    if (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.MULTIPLAYER || !PhotonNetwork.isMasterClient)
      return;
    if (SettingsManager.LegacyGameSettings.FriendlyMode.Value)
    {
      if (val == 1)
        return;
      this.photonView.RPC(nameof (setMyTeam), PhotonTargets.AllBuffered, (object) 1);
    }
    else if (SettingsManager.LegacyGameSettings.BladePVP.Value == 1)
    {
      int num = 0;
      if (this.photonView.owner.customProperties[(object) PhotonPlayerProperty.RCteam] != null)
        num = RCextensions.returnIntFromObject(this.photonView.owner.customProperties[(object) PhotonPlayerProperty.RCteam]);
      if (val == num)
        return;
      this.photonView.RPC(nameof (setMyTeam), PhotonTargets.AllBuffered, (object) num);
    }
    else
    {
      if (SettingsManager.LegacyGameSettings.BladePVP.Value != 2 || val == this.photonView.owner.ID)
        return;
      this.photonView.RPC(nameof (setMyTeam), PhotonTargets.AllBuffered, (object) this.photonView.owner.ID);
    }
  }

  public void setSkillHUDPosition2()
  {
    this.skillCD = GameObject.Find("skill_cd_" + this.skillIDHUD);
    if (Object.op_Inequality((Object) this.skillCD, (Object) null))
      this.skillCD.transform.localPosition = GameObject.Find("skill_cd_bottom").transform.localPosition;
    if (!this.useGun || SettingsManager.LegacyGameSettings.BombModeEnabled.Value)
      return;
    this.skillCD.transform.localPosition = Vector3.op_Multiply(Vector3.up, 5000f);
  }

  public void setStat2()
  {
    this.skillCDLast = 1.5f;
    this.skillId = this.setup.myCostume.stat.skillId;
    if (this.skillId == "levi")
      this.skillCDLast = 3.5f;
    this.customAnimationSpeed();
    if (this.skillId == "armin")
      this.skillCDLast = 5f;
    if (this.skillId == "marco")
      this.skillCDLast = 10f;
    if (this.skillId == "jean")
      this.skillCDLast = 1f / 1000f;
    if (this.skillId == "eren")
    {
      this.skillCDLast = 120f;
      if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER && (LevelInfo.getInfo(FengGameManagerMKII.level).teamTitan || LevelInfo.getInfo(FengGameManagerMKII.level).type == GAMEMODE.RACING || LevelInfo.getInfo(FengGameManagerMKII.level).type == GAMEMODE.PVP_CAPTURE || LevelInfo.getInfo(FengGameManagerMKII.level).type == GAMEMODE.TROST))
      {
        this.skillId = "petra";
        this.skillCDLast = 1f;
      }
    }
    if (this.skillId == "sasha")
      this.skillCDLast = 20f;
    if (this.skillId == "petra")
      this.skillCDLast = 3.5f;
    this.bombInit();
    this.speed = (float) this.setup.myCostume.stat.SPD / 10f;
    this.totalGas = this.currentGas = (float) this.setup.myCostume.stat.GAS;
    this.totalBladeSta = this.currentBladeSta = (float) this.setup.myCostume.stat.BLA;
    this.baseRigidBody.mass = (float) (0.5 - (double) (this.setup.myCostume.stat.ACL - 100) * (1.0 / 1000.0));
    GameObject.Find("skill_cd_bottom").transform.localPosition = new Vector3(0.0f, (float) ((double) -Screen.height * 0.5 + 5.0), 0.0f);
    this.skillCD = GameObject.Find("skill_cd_" + this.skillIDHUD);
    this.skillCD.transform.localPosition = GameObject.Find("skill_cd_bottom").transform.localPosition;
    GameObject.Find("GasUI").transform.localPosition = GameObject.Find("skill_cd_bottom").transform.localPosition;
    if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE || this.photonView.isMine)
    {
      ((Behaviour) GameObject.Find("bulletL").GetComponent<UISprite>()).enabled = false;
      ((Behaviour) GameObject.Find("bulletR").GetComponent<UISprite>()).enabled = false;
      ((Behaviour) GameObject.Find("bulletL1").GetComponent<UISprite>()).enabled = false;
      ((Behaviour) GameObject.Find("bulletR1").GetComponent<UISprite>()).enabled = false;
      ((Behaviour) GameObject.Find("bulletL2").GetComponent<UISprite>()).enabled = false;
      ((Behaviour) GameObject.Find("bulletR2").GetComponent<UISprite>()).enabled = false;
      ((Behaviour) GameObject.Find("bulletL3").GetComponent<UISprite>()).enabled = false;
      ((Behaviour) GameObject.Find("bulletR3").GetComponent<UISprite>()).enabled = false;
      ((Behaviour) GameObject.Find("bulletL4").GetComponent<UISprite>()).enabled = false;
      ((Behaviour) GameObject.Find("bulletR4").GetComponent<UISprite>()).enabled = false;
      ((Behaviour) GameObject.Find("bulletL5").GetComponent<UISprite>()).enabled = false;
      ((Behaviour) GameObject.Find("bulletR5").GetComponent<UISprite>()).enabled = false;
      ((Behaviour) GameObject.Find("bulletL6").GetComponent<UISprite>()).enabled = false;
      ((Behaviour) GameObject.Find("bulletR6").GetComponent<UISprite>()).enabled = false;
      ((Behaviour) GameObject.Find("bulletL7").GetComponent<UISprite>()).enabled = false;
      ((Behaviour) GameObject.Find("bulletR7").GetComponent<UISprite>()).enabled = false;
    }
    if (this.setup.myCostume.uniform_type == UNIFORM_TYPE.CasualAHSS)
    {
      this.standAnimation = "AHSS_stand_gun";
      this.useGun = true;
      this.gunDummy = new GameObject();
      ((Object) this.gunDummy).name = "gunDummy";
      this.gunDummy.transform.position = this.baseTransform.position;
      this.gunDummy.transform.rotation = this.baseTransform.rotation;
      this.myGroup = GROUP.A;
      this.setTeam2(2);
      if (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE && !this.photonView.isMine)
        return;
      ((Behaviour) GameObject.Find("bladeCL").GetComponent<UISprite>()).enabled = false;
      ((Behaviour) GameObject.Find("bladeCR").GetComponent<UISprite>()).enabled = false;
      ((Behaviour) GameObject.Find("bladel1").GetComponent<UISprite>()).enabled = false;
      ((Behaviour) GameObject.Find("blader1").GetComponent<UISprite>()).enabled = false;
      ((Behaviour) GameObject.Find("bladel2").GetComponent<UISprite>()).enabled = false;
      ((Behaviour) GameObject.Find("blader2").GetComponent<UISprite>()).enabled = false;
      ((Behaviour) GameObject.Find("bladel3").GetComponent<UISprite>()).enabled = false;
      ((Behaviour) GameObject.Find("blader3").GetComponent<UISprite>()).enabled = false;
      ((Behaviour) GameObject.Find("bladel4").GetComponent<UISprite>()).enabled = false;
      ((Behaviour) GameObject.Find("blader4").GetComponent<UISprite>()).enabled = false;
      ((Behaviour) GameObject.Find("bladel5").GetComponent<UISprite>()).enabled = false;
      ((Behaviour) GameObject.Find("blader5").GetComponent<UISprite>()).enabled = false;
      ((Behaviour) GameObject.Find("bulletL").GetComponent<UISprite>()).enabled = true;
      ((Behaviour) GameObject.Find("bulletR").GetComponent<UISprite>()).enabled = true;
      ((Behaviour) GameObject.Find("bulletL1").GetComponent<UISprite>()).enabled = true;
      ((Behaviour) GameObject.Find("bulletR1").GetComponent<UISprite>()).enabled = true;
      ((Behaviour) GameObject.Find("bulletL2").GetComponent<UISprite>()).enabled = true;
      ((Behaviour) GameObject.Find("bulletR2").GetComponent<UISprite>()).enabled = true;
      ((Behaviour) GameObject.Find("bulletL3").GetComponent<UISprite>()).enabled = true;
      ((Behaviour) GameObject.Find("bulletR3").GetComponent<UISprite>()).enabled = true;
      ((Behaviour) GameObject.Find("bulletL4").GetComponent<UISprite>()).enabled = true;
      ((Behaviour) GameObject.Find("bulletR4").GetComponent<UISprite>()).enabled = true;
      ((Behaviour) GameObject.Find("bulletL5").GetComponent<UISprite>()).enabled = true;
      ((Behaviour) GameObject.Find("bulletR5").GetComponent<UISprite>()).enabled = true;
      ((Behaviour) GameObject.Find("bulletL6").GetComponent<UISprite>()).enabled = true;
      ((Behaviour) GameObject.Find("bulletR6").GetComponent<UISprite>()).enabled = true;
      ((Behaviour) GameObject.Find("bulletL7").GetComponent<UISprite>()).enabled = true;
      ((Behaviour) GameObject.Find("bulletR7").GetComponent<UISprite>()).enabled = true;
      if (!(this.skillId != "bomb"))
        return;
      this.skillCD.transform.localPosition = Vector3.op_Multiply(Vector3.up, 5000f);
    }
    else if (this.setup.myCostume.sex == SEX.FEMALE)
    {
      this.standAnimation = "stand";
      this.setTeam2(1);
    }
    else
    {
      this.standAnimation = "stand_levi";
      this.setTeam2(1);
    }
  }

  public void setTeam2(int team)
  {
    if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER && this.photonView.isMine)
    {
      this.photonView.RPC("setMyTeam", PhotonTargets.AllBuffered, (object) team);
      Hashtable propertiesToSet = new Hashtable();
      ((Dictionary<object, object>) propertiesToSet).Add((object) PhotonPlayerProperty.team, (object) team);
      PhotonNetwork.player.SetCustomProperties(propertiesToSet);
    }
    else
      this.setMyTeam(team);
  }

  public void shootFlare(int type)
  {
    bool flag = false;
    if (type == 1 && (double) this.flare1CD == 0.0)
    {
      this.flare1CD = this.flareTotalCD;
      flag = true;
    }
    if (type == 2 && (double) this.flare2CD == 0.0)
    {
      this.flare2CD = this.flareTotalCD;
      flag = true;
    }
    if (type == 3 && (double) this.flare3CD == 0.0)
    {
      this.flare3CD = this.flareTotalCD;
      flag = true;
    }
    if (!flag)
      return;
    if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
    {
      GameObject gameObject = (GameObject) Object.Instantiate(Resources.Load("FX/flareBullet" + type.ToString()), ((Component) this).transform.position, ((Component) this).transform.rotation);
      gameObject.GetComponent<FlareMovement>().dontShowHint();
      Object.Destroy((Object) gameObject, 25f);
    }
    else
      PhotonNetwork.Instantiate("FX/flareBullet" + type.ToString(), ((Component) this).transform.position, ((Component) this).transform.rotation, 0).GetComponent<FlareMovement>().dontShowHint();
  }

  private void showAimUI2()
  {
    if (CursorManager.State == CursorState.Pointer || GameMenu.HideCrosshair)
    {
      Vector3 vector3 = Vector3.op_Multiply(Vector3.up, 10000f);
      this.crossL1.transform.localPosition = vector3;
      this.crossL2.transform.localPosition = vector3;
      this.crossR1.transform.localPosition = vector3;
      this.crossR2.transform.localPosition = vector3;
    }
    else
    {
      this.checkTitan();
      Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
      LayerMask layerMask1 = LayerMask.op_Implicit(1 << PhysicsLayer.Ground);
      LayerMask layerMask2 = LayerMask.op_Implicit(LayerMask.op_Implicit(LayerMask.op_Implicit(1 << PhysicsLayer.EnemyBox)) | LayerMask.op_Implicit(layerMask1));
      RaycastHit raycastHit1;
      if (!Physics.Raycast(ray, ref raycastHit1, 1E+07f, ((LayerMask) ref layerMask2).value))
        return;
      Vector3 vector3_1 = Vector3.op_Subtraction(((RaycastHit) ref raycastHit1).point, this.baseTransform.position);
      float magnitude = ((Vector3) ref vector3_1).magnitude;
      string text = string.Empty;
      if (SettingsManager.UISettings.ShowCrosshairDistance.Value)
        text = (double) magnitude <= 1000.0 ? ((int) magnitude).ToString() : "???";
      if (SettingsManager.UISettings.Speedometer.Value == 1)
      {
        if (text != string.Empty)
          text += "\n";
        text = text + this.currentSpeed.ToString("F1") + " u/s";
      }
      else if (SettingsManager.UISettings.Speedometer.Value == 2)
      {
        if (text != string.Empty)
          text += "\n";
        text = text + (this.currentSpeed / 100f).ToString("F1") + "K";
      }
      CursorManager.SetCrosshairText(text);
      if ((double) magnitude > 120.0)
        CursorManager.SetCrosshairColor(false);
      else
        CursorManager.SetCrosshairColor(true);
      if (SettingsManager.UISettings.ShowCrosshairArrows.Value)
      {
        Vector3 vector3_2;
        // ISSUE: explicit constructor call
        ((Vector3) ref vector3_2).\u002Ector(0.0f, 0.4f, 0.0f);
        Vector3 vector3_3 = Vector3.op_Subtraction(vector3_2, Vector3.op_Multiply(this.baseTransform.right, 0.3f));
        Vector3 vector3_4;
        // ISSUE: explicit constructor call
        ((Vector3) ref vector3_4).\u002Ector(0.0f, 0.4f, 0.0f);
        Vector3 vector3_5 = Vector3.op_Addition(vector3_4, Vector3.op_Multiply(this.baseTransform.right, 0.3f));
        float num = (double) ((RaycastHit) ref raycastHit1).distance <= 50.0 ? ((RaycastHit) ref raycastHit1).distance * 0.05f : ((RaycastHit) ref raycastHit1).distance * 0.3f;
        Vector3 vector3_6 = Vector3.op_Subtraction(Vector3.op_Subtraction(((RaycastHit) ref raycastHit1).point, Vector3.op_Multiply(this.baseTransform.right, num)), Vector3.op_Addition(this.baseTransform.position, vector3_3));
        Vector3 vector3_7 = Vector3.op_Subtraction(Vector3.op_Addition(((RaycastHit) ref raycastHit1).point, Vector3.op_Multiply(this.baseTransform.right, num)), Vector3.op_Addition(this.baseTransform.position, vector3_5));
        ((Vector3) ref vector3_6).Normalize();
        ((Vector3) ref vector3_7).Normalize();
        Vector3 vector3_8 = Vector3.op_Multiply(vector3_6, 1000000f);
        Vector3 vector3_9 = Vector3.op_Multiply(vector3_7, 1000000f);
        RaycastHit raycastHit2;
        if (Physics.Linecast(Vector3.op_Addition(this.baseTransform.position, vector3_3), Vector3.op_Addition(Vector3.op_Addition(this.baseTransform.position, vector3_3), vector3_8), ref raycastHit2, ((LayerMask) ref layerMask2).value))
        {
          Transform transform1 = this.crossL1.transform;
          transform1.localPosition = this.currentCamera.WorldToScreenPoint(((RaycastHit) ref raycastHit2).point);
          Transform transform2 = transform1;
          transform2.localPosition = Vector3.op_Subtraction(transform2.localPosition, new Vector3((float) Screen.width * 0.5f, (float) Screen.height * 0.5f, 0.0f));
          transform1.localRotation = Quaternion.Euler(0.0f, 0.0f, (float) ((double) Mathf.Atan2(transform1.localPosition.y - (Input.mousePosition.y - (float) Screen.height * 0.5f), transform1.localPosition.x - (Input.mousePosition.x - (float) Screen.width * 0.5f)) * 57.295780181884766 + 180.0));
          Transform transform3 = this.crossL2.transform;
          transform3.localPosition = transform1.localPosition;
          transform3.localRotation = transform1.localRotation;
          if ((double) ((RaycastHit) ref raycastHit2).distance > 120.0)
          {
            Transform transform4 = transform1;
            transform4.localPosition = Vector3.op_Addition(transform4.localPosition, Vector3.op_Multiply(Vector3.up, 10000f));
          }
          else
          {
            Transform transform5 = transform3;
            transform5.localPosition = Vector3.op_Addition(transform5.localPosition, Vector3.op_Multiply(Vector3.up, 10000f));
          }
        }
        if (!Physics.Linecast(Vector3.op_Addition(this.baseTransform.position, vector3_5), Vector3.op_Addition(Vector3.op_Addition(this.baseTransform.position, vector3_5), vector3_9), ref raycastHit2, ((LayerMask) ref layerMask2).value))
          return;
        Transform transform6 = this.crossR1.transform;
        transform6.localPosition = this.currentCamera.WorldToScreenPoint(((RaycastHit) ref raycastHit2).point);
        Transform transform7 = transform6;
        transform7.localPosition = Vector3.op_Subtraction(transform7.localPosition, new Vector3((float) Screen.width * 0.5f, (float) Screen.height * 0.5f, 0.0f));
        transform6.localRotation = Quaternion.Euler(0.0f, 0.0f, Mathf.Atan2(transform6.localPosition.y - (Input.mousePosition.y - (float) Screen.height * 0.5f), transform6.localPosition.x - (Input.mousePosition.x - (float) Screen.width * 0.5f)) * 57.29578f);
        Transform transform8 = this.crossR2.transform;
        transform8.localPosition = transform6.localPosition;
        transform8.localRotation = transform6.localRotation;
        if ((double) ((RaycastHit) ref raycastHit2).distance > 120.0)
        {
          Transform transform9 = transform6;
          transform9.localPosition = Vector3.op_Addition(transform9.localPosition, Vector3.op_Multiply(Vector3.up, 10000f));
        }
        else
        {
          Transform transform10 = transform8;
          transform10.localPosition = Vector3.op_Addition(transform10.localPosition, Vector3.op_Multiply(Vector3.up, 10000f));
        }
      }
      else
      {
        vector3_1 = Vector3.op_Multiply(Vector3.up, 10000f);
        this.crossL1.transform.localPosition = vector3_1;
        this.crossL2.transform.localPosition = vector3_1;
        this.crossR1.transform.localPosition = vector3_1;
        this.crossR2.transform.localPosition = vector3_1;
      }
    }
  }

  private void showFlareCD2()
  {
    if (!Object.op_Inequality((Object) this.cachedSprites["UIflare1"], (Object) null))
      return;
    this.cachedSprites["UIflare1"].fillAmount = (this.flareTotalCD - this.flare1CD) / this.flareTotalCD;
    this.cachedSprites["UIflare2"].fillAmount = (this.flareTotalCD - this.flare2CD) / this.flareTotalCD;
    this.cachedSprites["UIflare3"].fillAmount = (this.flareTotalCD - this.flare3CD) / this.flareTotalCD;
  }

  private void showGas()
  {
    float num1 = this.currentGas / this.totalGas;
    float num2 = this.currentBladeSta / this.totalBladeSta;
    GameObject.Find("gasL1").GetComponent<UISprite>().fillAmount = this.currentGas / this.totalGas;
    GameObject.Find("gasR1").GetComponent<UISprite>().fillAmount = this.currentGas / this.totalGas;
    if (!this.useGun)
    {
      GameObject.Find("bladeCL").GetComponent<UISprite>().fillAmount = this.currentBladeSta / this.totalBladeSta;
      GameObject.Find("bladeCR").GetComponent<UISprite>().fillAmount = this.currentBladeSta / this.totalBladeSta;
      if ((double) num1 <= 0.0)
      {
        GameObject.Find("gasL").GetComponent<UISprite>().color = Color.red;
        GameObject.Find("gasR").GetComponent<UISprite>().color = Color.red;
      }
      else if ((double) num1 < 0.30000001192092896)
      {
        GameObject.Find("gasL").GetComponent<UISprite>().color = Color.yellow;
        GameObject.Find("gasR").GetComponent<UISprite>().color = Color.yellow;
      }
      else
      {
        GameObject.Find("gasL").GetComponent<UISprite>().color = Color.white;
        GameObject.Find("gasR").GetComponent<UISprite>().color = Color.white;
      }
      if ((double) num2 <= 0.0)
      {
        GameObject.Find("bladel1").GetComponent<UISprite>().color = Color.red;
        GameObject.Find("blader1").GetComponent<UISprite>().color = Color.red;
      }
      else if ((double) num2 < 0.30000001192092896)
      {
        GameObject.Find("bladel1").GetComponent<UISprite>().color = Color.yellow;
        GameObject.Find("blader1").GetComponent<UISprite>().color = Color.yellow;
      }
      else
      {
        GameObject.Find("bladel1").GetComponent<UISprite>().color = Color.white;
        GameObject.Find("blader1").GetComponent<UISprite>().color = Color.white;
      }
      if (this.currentBladeNum <= 4)
      {
        ((Behaviour) GameObject.Find("bladel5").GetComponent<UISprite>()).enabled = false;
        ((Behaviour) GameObject.Find("blader5").GetComponent<UISprite>()).enabled = false;
      }
      else
      {
        ((Behaviour) GameObject.Find("bladel5").GetComponent<UISprite>()).enabled = true;
        ((Behaviour) GameObject.Find("blader5").GetComponent<UISprite>()).enabled = true;
      }
      if (this.currentBladeNum <= 3)
      {
        ((Behaviour) GameObject.Find("bladel4").GetComponent<UISprite>()).enabled = false;
        ((Behaviour) GameObject.Find("blader4").GetComponent<UISprite>()).enabled = false;
      }
      else
      {
        ((Behaviour) GameObject.Find("bladel4").GetComponent<UISprite>()).enabled = true;
        ((Behaviour) GameObject.Find("blader4").GetComponent<UISprite>()).enabled = true;
      }
      if (this.currentBladeNum <= 2)
      {
        ((Behaviour) GameObject.Find("bladel3").GetComponent<UISprite>()).enabled = false;
        ((Behaviour) GameObject.Find("blader3").GetComponent<UISprite>()).enabled = false;
      }
      else
      {
        ((Behaviour) GameObject.Find("bladel3").GetComponent<UISprite>()).enabled = true;
        ((Behaviour) GameObject.Find("blader3").GetComponent<UISprite>()).enabled = true;
      }
      if (this.currentBladeNum <= 1)
      {
        ((Behaviour) GameObject.Find("bladel2").GetComponent<UISprite>()).enabled = false;
        ((Behaviour) GameObject.Find("blader2").GetComponent<UISprite>()).enabled = false;
      }
      else
      {
        ((Behaviour) GameObject.Find("bladel2").GetComponent<UISprite>()).enabled = true;
        ((Behaviour) GameObject.Find("blader2").GetComponent<UISprite>()).enabled = true;
      }
      if (this.currentBladeNum <= 0)
      {
        ((Behaviour) GameObject.Find("bladel1").GetComponent<UISprite>()).enabled = false;
        ((Behaviour) GameObject.Find("blader1").GetComponent<UISprite>()).enabled = false;
      }
      else
      {
        ((Behaviour) GameObject.Find("bladel1").GetComponent<UISprite>()).enabled = true;
        ((Behaviour) GameObject.Find("blader1").GetComponent<UISprite>()).enabled = true;
      }
    }
    else
    {
      if (this.leftGunHasBullet)
        ((Behaviour) GameObject.Find("bulletL").GetComponent<UISprite>()).enabled = true;
      else
        ((Behaviour) GameObject.Find("bulletL").GetComponent<UISprite>()).enabled = false;
      if (this.rightGunHasBullet)
        ((Behaviour) GameObject.Find("bulletR").GetComponent<UISprite>()).enabled = true;
      else
        ((Behaviour) GameObject.Find("bulletR").GetComponent<UISprite>()).enabled = false;
    }
  }

  private void showGas2()
  {
    float num1 = this.currentGas / this.totalGas;
    float num2 = this.currentBladeSta / this.totalBladeSta;
    this.cachedSprites["gasL1"].fillAmount = this.currentGas / this.totalGas;
    this.cachedSprites["gasR1"].fillAmount = this.currentGas / this.totalGas;
    if (!this.useGun)
    {
      this.cachedSprites["bladeCL"].fillAmount = this.currentBladeSta / this.totalBladeSta;
      this.cachedSprites["bladeCR"].fillAmount = this.currentBladeSta / this.totalBladeSta;
      if ((double) num1 <= 0.0)
      {
        this.cachedSprites["gasL"].color = Color.red;
        this.cachedSprites["gasR"].color = Color.red;
      }
      else if ((double) num1 < 0.30000001192092896)
      {
        this.cachedSprites["gasL"].color = Color.yellow;
        this.cachedSprites["gasR"].color = Color.yellow;
      }
      else
      {
        this.cachedSprites["gasL"].color = Color.white;
        this.cachedSprites["gasR"].color = Color.white;
      }
      if ((double) num2 <= 0.0)
      {
        this.cachedSprites["bladel1"].color = Color.red;
        this.cachedSprites["blader1"].color = Color.red;
      }
      else if ((double) num2 < 0.30000001192092896)
      {
        this.cachedSprites["bladel1"].color = Color.yellow;
        this.cachedSprites["blader1"].color = Color.yellow;
      }
      else
      {
        this.cachedSprites["bladel1"].color = Color.white;
        this.cachedSprites["blader1"].color = Color.white;
      }
      if (this.currentBladeNum <= 4)
      {
        ((Behaviour) this.cachedSprites["bladel5"]).enabled = false;
        ((Behaviour) this.cachedSprites["blader5"]).enabled = false;
      }
      else
      {
        ((Behaviour) this.cachedSprites["bladel5"]).enabled = true;
        ((Behaviour) this.cachedSprites["blader5"]).enabled = true;
      }
      if (this.currentBladeNum <= 3)
      {
        ((Behaviour) this.cachedSprites["bladel4"]).enabled = false;
        ((Behaviour) this.cachedSprites["blader4"]).enabled = false;
      }
      else
      {
        ((Behaviour) this.cachedSprites["bladel4"]).enabled = true;
        ((Behaviour) this.cachedSprites["blader4"]).enabled = true;
      }
      if (this.currentBladeNum <= 2)
      {
        ((Behaviour) this.cachedSprites["bladel3"]).enabled = false;
        ((Behaviour) this.cachedSprites["blader3"]).enabled = false;
      }
      else
      {
        ((Behaviour) this.cachedSprites["bladel3"]).enabled = true;
        ((Behaviour) this.cachedSprites["blader3"]).enabled = true;
      }
      if (this.currentBladeNum <= 1)
      {
        ((Behaviour) this.cachedSprites["bladel2"]).enabled = false;
        ((Behaviour) this.cachedSprites["blader2"]).enabled = false;
      }
      else
      {
        ((Behaviour) this.cachedSprites["bladel2"]).enabled = true;
        ((Behaviour) this.cachedSprites["blader2"]).enabled = true;
      }
      if (this.currentBladeNum <= 0)
      {
        ((Behaviour) this.cachedSprites["bladel1"]).enabled = false;
        ((Behaviour) this.cachedSprites["blader1"]).enabled = false;
      }
      else
      {
        ((Behaviour) this.cachedSprites["bladel1"]).enabled = true;
        ((Behaviour) this.cachedSprites["blader1"]).enabled = true;
      }
    }
    else
    {
      if (this.leftGunHasBullet)
        ((Behaviour) this.cachedSprites["bulletL"]).enabled = true;
      else
        ((Behaviour) this.cachedSprites["bulletL"]).enabled = false;
      if (this.rightGunHasBullet)
        ((Behaviour) this.cachedSprites["bulletR"]).enabled = true;
      else
        ((Behaviour) this.cachedSprites["bulletR"]).enabled = false;
    }
  }

  [RPC]
  private void showHitDamage(PhotonMessageInfo info)
  {
    if (info == null)
      return;
    FengGameManagerMKII.instance.kickPlayerRCIfMC(info.sender, true, "hero showHitDamage exploit");
  }

  private void showSkillCD()
  {
    if (!Object.op_Inequality((Object) this.skillCD, (Object) null))
      return;
    this.skillCD.GetComponent<UISprite>().fillAmount = (this.skillCDLast - this.skillCDDuration) / this.skillCDLast;
  }

  [RPC]
  public void SpawnCannonRPC(string settings, PhotonMessageInfo info)
  {
    if (!info.sender.isMasterClient || !this.photonView.isMine || !Object.op_Equality((Object) this.myCannon, (Object) null))
      return;
    if (Object.op_Inequality((Object) this.myHorse, (Object) null) && this.isMounted)
      this.getOffHorse();
    this.idle();
    if (Object.op_Inequality((Object) this.bulletLeft, (Object) null))
      this.bulletLeft.GetComponent<Bullet>().removeMe();
    if (Object.op_Inequality((Object) this.bulletRight, (Object) null))
      this.bulletRight.GetComponent<Bullet>().removeMe();
    if (this.smoke_3dmg.enableEmission && IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE && this.photonView.isMine)
      this.photonView.RPC("net3DMGSMOKE", PhotonTargets.Others, (object) false);
    this.smoke_3dmg.enableEmission = false;
    ((Component) this).rigidbody.velocity = Vector3.zero;
    string[] strArray = settings.Split(',');
    this.myCannon = strArray.Length <= 15 ? PhotonNetwork.Instantiate("RCAsset/" + strArray[1], new Vector3(Convert.ToSingle(strArray[2]), Convert.ToSingle(strArray[3]), Convert.ToSingle(strArray[4])), new Quaternion(Convert.ToSingle(strArray[5]), Convert.ToSingle(strArray[6]), Convert.ToSingle(strArray[7]), Convert.ToSingle(strArray[8])), 0) : PhotonNetwork.Instantiate("RCAsset/" + strArray[1], new Vector3(Convert.ToSingle(strArray[12]), Convert.ToSingle(strArray[13]), Convert.ToSingle(strArray[14])), new Quaternion(Convert.ToSingle(strArray[15]), Convert.ToSingle(strArray[16]), Convert.ToSingle(strArray[17]), Convert.ToSingle(strArray[18])), 0);
    this.myCannonBase = this.myCannon.transform;
    this.myCannonPlayer = this.myCannon.transform.Find("PlayerPoint");
    this.isCannon = true;
    this.myCannon.GetComponent<Cannon>().myHero = this;
    this.myCannonRegion = (CannonPropRegion) null;
    ((Component) Camera.main).GetComponent<IN_GAME_MAIN_CAMERA>().setMainObject(((Component) this.myCannon.transform.Find("Barrel").Find("FiringPoint")).gameObject);
    Camera.main.fieldOfView = 55f;
    this.photonView.RPC("SetMyCannon", PhotonTargets.OthersBuffered, (object) this.myCannon.GetPhotonView().viewID);
    this.skillCDLastCannon = this.skillCDLast;
    this.skillCDLast = 3.5f;
    this.skillCDDuration = 3.5f;
  }

  private void Start()
  {
    FengGameManagerMKII.instance.addHero(this);
    if ((LevelInfo.getInfo(FengGameManagerMKII.level).horse || SettingsManager.LegacyGameSettings.AllowHorses.Value) && IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER && this.photonView.isMine)
    {
      this.myHorse = PhotonNetwork.Instantiate("horse", Vector3.op_Addition(this.baseTransform.position, Vector3.op_Multiply(Vector3.up, 5f)), this.baseTransform.rotation, 0);
      this.myHorse.GetComponent<Horse>().myHero = ((Component) this).gameObject;
      this.myHorse.GetComponent<TITAN_CONTROLLER>().isHorse = true;
    }
    this.sparks = ((Component) this.baseTransform.Find("slideSparks")).GetComponent<ParticleSystem>();
    this.smoke_3dmg = ((Component) this.baseTransform.Find("3dmg_smoke")).GetComponent<ParticleSystem>();
    this.baseTransform.localScale = new Vector3(this.myScale, this.myScale, this.myScale);
    Quaternion rotation = this.baseTransform.rotation;
    this.facingDirection = ((Quaternion) ref rotation).eulerAngles.y;
    this.targetRotation = Quaternion.Euler(0.0f, this.facingDirection, 0.0f);
    this.smoke_3dmg.enableEmission = false;
    this.sparks.enableEmission = false;
    this.speedFXPS = this.speedFX1.GetComponent<ParticleSystem>();
    this.speedFXPS.enableEmission = false;
    if (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.MULTIPLAYER)
    {
      if (Object.op_Inequality((Object) Minimap.instance, (Object) null))
        Minimap.instance.TrackGameObjectOnMinimap(((Component) this).gameObject, Color.green, false, true);
    }
    else
    {
      if (PhotonNetwork.isMasterClient)
      {
        int id = this.photonView.owner.ID;
        if (((Dictionary<object, object>) FengGameManagerMKII.heroHash).ContainsKey((object) id))
          FengGameManagerMKII.heroHash[(object) id] = (object) this;
        else
          ((Dictionary<object, object>) FengGameManagerMKII.heroHash).Add((object) id, (object) this);
      }
      GameObject gameObject = GameObject.Find("UI_IN_GAME");
      this.myNetWorkName = (GameObject) Object.Instantiate(Resources.Load("UI/LabelNameOverHead"));
      ((Object) this.myNetWorkName).name = "LabelNameOverHead";
      this.myNetWorkName.transform.parent = gameObject.GetComponent<UIReferArray>().panels[0].transform;
      this.myNetWorkName.transform.localScale = new Vector3(14f, 14f, 14f);
      this.myNetWorkName.GetComponent<UILabel>().text = string.Empty;
      if (this.photonView.isMine)
      {
        if (Object.op_Inequality((Object) Minimap.instance, (Object) null))
          Minimap.instance.TrackGameObjectOnMinimap(((Component) this).gameObject, Color.green, false, true);
        ((Component) this).GetComponent<SmoothSyncMovement>().PhotonCamera = true;
        this.photonView.RPC("SetMyPhotonCamera", PhotonTargets.OthersBuffered, (object) (float) ((double) SettingsManager.GeneralSettings.CameraDistance.Value + 0.30000001192092896));
      }
      else
      {
        bool flag = false;
        if (this.photonView.owner.customProperties[(object) PhotonPlayerProperty.RCteam] != null)
        {
          switch (RCextensions.returnIntFromObject(this.photonView.owner.customProperties[(object) PhotonPlayerProperty.RCteam]))
          {
            case 1:
              flag = true;
              if (Object.op_Inequality((Object) Minimap.instance, (Object) null))
              {
                Minimap.instance.TrackGameObjectOnMinimap(((Component) this).gameObject, Color.cyan, false, true);
                break;
              }
              break;
            case 2:
              flag = true;
              if (Object.op_Inequality((Object) Minimap.instance, (Object) null))
              {
                Minimap.instance.TrackGameObjectOnMinimap(((Component) this).gameObject, Color.magenta, false, true);
                break;
              }
              break;
          }
        }
        if (RCextensions.returnIntFromObject(this.photonView.owner.customProperties[(object) PhotonPlayerProperty.team]) == 2)
        {
          this.myNetWorkName.GetComponent<UILabel>().text = "[FF0000]AHSS\n[FFFFFF]";
          if (!flag && Object.op_Inequality((Object) Minimap.instance, (Object) null))
            Minimap.instance.TrackGameObjectOnMinimap(((Component) this).gameObject, Color.red, false, true);
        }
        else if (!flag && Object.op_Inequality((Object) Minimap.instance, (Object) null))
          Minimap.instance.TrackGameObjectOnMinimap(((Component) this).gameObject, Color.blue, false, true);
      }
      string str = RCextensions.returnStringFromObject(this.photonView.owner.customProperties[(object) PhotonPlayerProperty.guildName]);
      if (str != string.Empty)
      {
        UILabel component = this.myNetWorkName.GetComponent<UILabel>();
        string[] strArray = new string[5]
        {
          component.text,
          "[FFFF00]",
          str,
          "\n[FFFFFF]",
          RCextensions.returnStringFromObject(this.photonView.owner.customProperties[(object) PhotonPlayerProperty.name])
        };
        component.text = string.Concat(strArray);
      }
      else
        this.myNetWorkName.GetComponent<UILabel>().text += RCextensions.returnStringFromObject(this.photonView.owner.customProperties[(object) PhotonPlayerProperty.name]);
    }
    if (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE && !this.photonView.isMine)
    {
      ((Component) this).gameObject.layer = LayerMask.NameToLayer("NetworkObject");
      this.setup.init();
      this.setup.myCostume = new HeroCostume();
      this.setup.myCostume = CostumeConeveter.PhotonDataToHeroCostume2(this.photonView.owner);
      this.setup.setCharacterComponent();
      Object.Destroy((Object) this.checkBoxLeft);
      Object.Destroy((Object) this.checkBoxRight);
      Object.Destroy((Object) this.leftbladetrail);
      Object.Destroy((Object) this.rightbladetrail);
      Object.Destroy((Object) this.leftbladetrail2);
      Object.Destroy((Object) this.rightbladetrail2);
      this.hasspawn = true;
    }
    else
    {
      this.SetInterpolationIfEnabled(true);
      this.currentCamera = GameObject.Find("MainCamera").GetComponent<Camera>();
      this.loadskin();
      this.hasspawn = true;
    }
    this.bombImmune = false;
    if (SettingsManager.LegacyGameSettings.BombModeEnabled.Value)
    {
      this.bombImmune = true;
      this.StartCoroutine(this.stopImmunity());
      this.SetupThunderSpears();
    }
    if (this._needSetupThunderspears)
      this.CreateAndAttachThunderSpears();
    this._hasRunStart = true;
    this.SetName();
  }

  public void SetName()
  {
    if (Object.op_Equality((Object) this.myNetWorkName, (Object) null) || Object.op_Equality((Object) this.myNetWorkName.GetComponent<UILabel>(), (Object) null))
      return;
    if (SettingsManager.UISettings.DisableNameColors.Value)
      this.ForceWhiteName();
    if (!SettingsManager.LegacyGameSettings.GlobalHideNames.Value && !SettingsManager.UISettings.HideNames.Value)
      return;
    this.HideName();
  }

  public void HideName() => this.myNetWorkName.GetComponent<UILabel>().text = string.Empty;

  public void ForceWhiteName()
  {
    UILabel component = this.myNetWorkName.GetComponent<UILabel>();
    component.text = component.text.StripHex();
  }

  public void SetInterpolationIfEnabled(bool interpolate)
  {
    if (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE && !this.photonView.isMine)
      return;
    if (interpolate && SettingsManager.GraphicsSettings.InterpolationEnabled.Value)
      ((Component) this).rigidbody.interpolation = (RigidbodyInterpolation) 1;
    else
      ((Component) this).rigidbody.interpolation = (RigidbodyInterpolation) 0;
  }

  public IEnumerator stopImmunity()
  {
    yield return (object) new WaitForSeconds(5f);
    this.bombImmune = false;
  }

  private void suicide()
  {
  }

  private void suicide2()
  {
    if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
      return;
    this.netDieLocal(Vector3.op_Multiply(((Component) this).rigidbody.velocity, 50f), false, titanName: string.Empty);
    FengGameManagerMKII.instance.needChooseSide = true;
    FengGameManagerMKII.instance.justSuicide = true;
  }

  private void throwBlades()
  {
    Transform transform1 = this.setup.part_blade_l.transform;
    Transform transform2 = this.setup.part_blade_r.transform;
    GameObject gameObject1 = (GameObject) Object.Instantiate(Resources.Load("Character_parts/character_blade_l"), transform1.position, transform1.rotation);
    GameObject gameObject2 = (GameObject) Object.Instantiate(Resources.Load("Character_parts/character_blade_r"), transform2.position, transform2.rotation);
    gameObject1.renderer.material = CharacterMaterials.materials[this.setup.myCostume._3dmg_texture];
    gameObject2.renderer.material = CharacterMaterials.materials[this.setup.myCostume._3dmg_texture];
    Vector3 vector3_1 = Vector3.op_Subtraction(Vector3.op_Addition(((Component) this).transform.forward, Vector3.op_Multiply(((Component) this).transform.up, 2f)), ((Component) this).transform.right);
    gameObject1.rigidbody.AddForce(vector3_1, (ForceMode) 1);
    Vector3 vector3_2 = Vector3.op_Addition(Vector3.op_Addition(((Component) this).transform.forward, Vector3.op_Multiply(((Component) this).transform.up, 2f)), ((Component) this).transform.right);
    gameObject2.rigidbody.AddForce(vector3_2, (ForceMode) 1);
    Vector3 vector3_3;
    // ISSUE: explicit constructor call
    ((Vector3) ref vector3_3).\u002Ector((float) Random.Range(-100, 100), (float) Random.Range(-100, 100), (float) Random.Range(-100, 100));
    ((Vector3) ref vector3_3).Normalize();
    gameObject1.rigidbody.AddTorque(vector3_3);
    // ISSUE: explicit constructor call
    ((Vector3) ref vector3_3).\u002Ector((float) Random.Range(-100, 100), (float) Random.Range(-100, 100), (float) Random.Range(-100, 100));
    ((Vector3) ref vector3_3).Normalize();
    gameObject2.rigidbody.AddTorque(vector3_3);
    this.setup.part_blade_l.SetActive(false);
    this.setup.part_blade_r.SetActive(false);
    --this.currentBladeNum;
    if (this.currentBladeNum == 0)
      this.currentBladeSta = 0.0f;
    if (this.state != HERO_STATE.Attack)
      return;
    this.falseAttack();
  }

  public void ungrabbed()
  {
    this.facingDirection = 0.0f;
    this.targetRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
    ((Component) this).transform.parent = (Transform) null;
    ((Collider) ((Component) this).GetComponent<CapsuleCollider>()).isTrigger = false;
    this.state = HERO_STATE.Idle;
  }

  private void unmounted()
  {
    this.myHorse.GetComponent<Horse>().unmounted();
    this.isMounted = false;
  }

  public void update2()
  {
    if (GameMenu.Paused && IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
      return;
    if ((double) this.invincible > 0.0)
      this.invincible -= Time.deltaTime;
    if (this.hasDied)
      return;
    if (this.titanForm && Object.op_Inequality((Object) this.eren_titan, (Object) null))
    {
      this.baseTransform.position = this.eren_titan.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck").position;
      ((Component) this).gameObject.GetComponent<SmoothSyncMovement>().disabled = true;
    }
    else if (this.isCannon && Object.op_Inequality((Object) this.myCannon, (Object) null))
    {
      this.updateCannon();
      ((Component) this).gameObject.GetComponent<SmoothSyncMovement>().disabled = true;
    }
    if (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE && !this.photonView.isMine)
      return;
    this.UpdateInput();
    this._dashCooldownLeft -= Time.deltaTime;
    if ((double) this._dashCooldownLeft < 0.0)
      this._dashCooldownLeft = 0.0f;
    if (Object.op_Inequality((Object) this.myCannonRegion, (Object) null))
    {
      FengGameManagerMKII.instance.ShowHUDInfoCenter(string.Format("Press {0} to use Cannon.", (object) SettingsManager.InputSettings.Interaction.Interact.ToString()));
      if (SettingsManager.InputSettings.Interaction.Interact.GetKeyDown())
        this.myCannonRegion.photonView.RPC("RequestControlRPC", PhotonTargets.MasterClient, (object) this.photonView.viewID);
    }
    if (this.state == HERO_STATE.Grab && !this.useGun)
    {
      if (this.skillId == "jean")
      {
        if (this.state != HERO_STATE.Attack && (SettingsManager.InputSettings.Human.AttackDefault.GetKeyDown() || SettingsManager.InputSettings.Human.AttackSpecial.GetKeyDown()) && this.escapeTimes > 0 && !this.baseAnimation.IsPlaying("grabbed_jean"))
        {
          this.playAnimation("grabbed_jean");
          this.baseAnimation["grabbed_jean"].time = 0.0f;
          --this.escapeTimes;
        }
        if (!this.baseAnimation.IsPlaying("grabbed_jean") || (double) this.baseAnimation["grabbed_jean"].normalizedTime <= 0.63999998569488525 || !Object.op_Inequality((Object) this.titanWhoGrabMe.GetComponent<TITAN>(), (Object) null))
          return;
        this.ungrabbed();
        this.baseRigidBody.velocity = Vector3.op_Multiply(Vector3.up, 30f);
        if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
        {
          this.titanWhoGrabMe.GetComponent<TITAN>().grabbedTargetEscape((PhotonMessageInfo) null);
        }
        else
        {
          this.photonView.RPC("netSetIsGrabbedFalse", PhotonTargets.All);
          if (PhotonNetwork.isMasterClient)
            this.titanWhoGrabMe.GetComponent<TITAN>().grabbedTargetEscape((PhotonMessageInfo) null);
          else
            PhotonView.Find(this.titanWhoGrabMeID).RPC("grabbedTargetEscape", PhotonTargets.MasterClient);
        }
      }
      else
      {
        if (!(this.skillId == "eren"))
          return;
        this.showSkillCD();
        if (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE || IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE && !GameMenu.Paused)
        {
          this.calcSkillCD();
          this.calcFlareCD();
        }
        if (!SettingsManager.InputSettings.Human.AttackSpecial.GetKeyDown())
          return;
        if (!((double) this.skillCDDuration > 0.0 | false))
        {
          this.skillCDDuration = this.skillCDLast;
          if (!(this.skillId == "eren") || !Object.op_Inequality((Object) this.titanWhoGrabMe.GetComponent<TITAN>(), (Object) null))
            return;
          this.ungrabbed();
          if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
          {
            this.titanWhoGrabMe.GetComponent<TITAN>().grabbedTargetEscape((PhotonMessageInfo) null);
          }
          else
          {
            this.photonView.RPC("netSetIsGrabbedFalse", PhotonTargets.All);
            if (PhotonNetwork.isMasterClient)
              this.titanWhoGrabMe.GetComponent<TITAN>().grabbedTargetEscape((PhotonMessageInfo) null);
            else
              PhotonView.Find(this.titanWhoGrabMeID).photonView.RPC("grabbedTargetEscape", PhotonTargets.MasterClient);
          }
          this.erenTransform();
        }
      }
    }
    else if (!this.titanForm && !this.isCannon)
    {
      this.bufferUpdate();
      this.UpdateThunderSpear();
      if (!GameMenu.InMenu())
      {
        if (!this.grounded && this.state != HERO_STATE.AirDodge && (!this.isMounted || !Object.op_Inequality((Object) this.myHorse, (Object) null)))
        {
          this.checkDashRebind();
          if (SettingsManager.InputSettings.Human.DashDoubleTap.Value)
            this.checkDashDoubleTap();
          if (this.dashD)
          {
            this.dashD = false;
            this.dash(0.0f, -1f);
            return;
          }
          if (this.dashU)
          {
            this.dashU = false;
            this.dash(0.0f, 1f);
            return;
          }
          if (this.dashL)
          {
            this.dashL = false;
            this.dash(-1f, 0.0f);
            return;
          }
          if (this.dashR)
          {
            this.dashR = false;
            this.dash(1f, 0.0f);
            return;
          }
        }
        if (this.grounded && (this.state == HERO_STATE.Idle || this.state == HERO_STATE.Slide))
        {
          if (SettingsManager.InputSettings.Human.Jump.GetKeyDown() && !this.baseAnimation.IsPlaying("jump") && !this.baseAnimation.IsPlaying("horse_geton"))
          {
            this.idle();
            this.crossFade("jump", 0.1f);
            this.sparks.enableEmission = false;
          }
          if (SettingsManager.InputSettings.Human.HorseMount.GetKeyDown() && !this.baseAnimation.IsPlaying("jump") && !this.baseAnimation.IsPlaying("horse_geton") && Object.op_Inequality((Object) this.myHorse, (Object) null) && !this.isMounted && (double) Vector3.Distance(this.myHorse.transform.position, ((Component) this).transform.position) < 15.0)
            this.getOnHorse();
          if (SettingsManager.InputSettings.Human.Dodge.GetKeyDown() && !this.baseAnimation.IsPlaying("jump") && !this.baseAnimation.IsPlaying("horse_geton"))
          {
            this.dodge2();
            return;
          }
        }
      }
      if (this.state == HERO_STATE.Idle && !GameMenu.InMenu())
      {
        this._flareDelayAfterEmote -= Time.deltaTime;
        if ((double) this._flareDelayAfterEmote <= 0.0)
        {
          if (SettingsManager.InputSettings.Human.Flare1.GetKeyDown())
            this.shootFlare(1);
          if (SettingsManager.InputSettings.Human.Flare2.GetKeyDown())
            this.shootFlare(2);
          if (SettingsManager.InputSettings.Human.Flare3.GetKeyDown())
            this.shootFlare(3);
        }
        if (SettingsManager.InputSettings.General.ChangeCharacter.GetKeyDown())
          this.suicide2();
        if (Object.op_Inequality((Object) this.myHorse, (Object) null) && this.isMounted && SettingsManager.InputSettings.Human.HorseMount.GetKeyDown())
          this.getOffHorse();
        if ((((Component) this).animation.IsPlaying(this.standAnimation) || !this.grounded) && SettingsManager.InputSettings.Human.Reload.GetKeyDown() && (!this.useGun || SettingsManager.LegacyGameSettings.AHSSAirReload.Value || this.grounded))
        {
          this.changeBlade();
          return;
        }
        if (!this.isMounted && (SettingsManager.InputSettings.Human.AttackDefault.GetKeyDown() || SettingsManager.InputSettings.Human.AttackSpecial.GetKeyDown()) && !this.useGun)
        {
          bool flag = false;
          if (SettingsManager.InputSettings.Human.AttackSpecial.GetKeyDown())
          {
            if ((double) this.skillCDDuration > 0.0 | flag)
            {
              flag = true;
            }
            else
            {
              this.skillCDDuration = this.skillCDLast;
              if (this.skillId == "eren")
              {
                this.erenTransform();
                return;
              }
              if (this.skillId == "marco")
              {
                if (this.IsGrounded())
                {
                  this.attackAnimation = Random.Range(0, 2) != 0 ? "special_marco_1" : "special_marco_0";
                  this.playAnimation(this.attackAnimation);
                }
                else
                {
                  flag = true;
                  this.skillCDDuration = 0.0f;
                }
              }
              else if (this.skillId == "armin")
              {
                if (this.IsGrounded())
                {
                  this.attackAnimation = "special_armin";
                  this.playAnimation("special_armin");
                }
                else
                {
                  flag = true;
                  this.skillCDDuration = 0.0f;
                }
              }
              else if (this.skillId == "sasha")
              {
                if (this.IsGrounded())
                {
                  this.attackAnimation = "special_sasha";
                  this.playAnimation("special_sasha");
                  this.currentBuff = BUFF.SpeedUp;
                  this.buffTime = 10f;
                }
                else
                {
                  flag = true;
                  this.skillCDDuration = 0.0f;
                }
              }
              else if (this.skillId == "mikasa")
              {
                this.attackAnimation = "attack3_1";
                this.playAnimation("attack3_1");
                this.baseRigidBody.velocity = Vector3.op_Multiply(Vector3.up, 10f);
              }
              else if (this.skillId == "levi")
              {
                this.attackAnimation = "attack5";
                this.playAnimation("attack5");
                Rigidbody baseRigidBody = this.baseRigidBody;
                baseRigidBody.velocity = Vector3.op_Addition(baseRigidBody.velocity, Vector3.op_Multiply(Vector3.up, 5f));
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                LayerMask layerMask1 = LayerMask.op_Implicit(1 << LayerMask.NameToLayer("Ground"));
                LayerMask layerMask2 = LayerMask.op_Implicit(LayerMask.op_Implicit(LayerMask.op_Implicit(1 << LayerMask.NameToLayer("EnemyBox"))) | LayerMask.op_Implicit(layerMask1));
                RaycastHit hit;
                if (Physics.Raycast(ray, ref hit, 1E+07f, ((LayerMask) ref layerMask2).value))
                {
                  if (Object.op_Inequality((Object) this.bulletRight, (Object) null))
                  {
                    this.bulletRight.GetComponent<Bullet>().disable();
                    this.releaseIfIHookSb();
                  }
                  this.dashDirection = Vector3.op_Subtraction(((RaycastHit) ref hit).point, this.baseTransform.position);
                  this.launchRightRope(hit, true, 1);
                  this.rope.Play();
                }
                this.facingDirection = Mathf.Atan2(this.dashDirection.x, this.dashDirection.z) * 57.29578f;
                this.targetRotation = Quaternion.Euler(0.0f, this.facingDirection, 0.0f);
                this.attackLoop = 3;
              }
              else if (this.skillId == "petra")
              {
                this.attackAnimation = "special_petra";
                this.playAnimation("special_petra");
                Rigidbody baseRigidBody = this.baseRigidBody;
                baseRigidBody.velocity = Vector3.op_Addition(baseRigidBody.velocity, Vector3.op_Multiply(Vector3.up, 5f));
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                LayerMask layerMask3 = LayerMask.op_Implicit(1 << LayerMask.NameToLayer("Ground"));
                LayerMask layerMask4 = LayerMask.op_Implicit(LayerMask.op_Implicit(LayerMask.op_Implicit(1 << LayerMask.NameToLayer("EnemyBox"))) | LayerMask.op_Implicit(layerMask3));
                RaycastHit hit;
                if (Physics.Raycast(ray, ref hit, 1E+07f, ((LayerMask) ref layerMask4).value))
                {
                  if (Object.op_Inequality((Object) this.bulletRight, (Object) null))
                  {
                    this.bulletRight.GetComponent<Bullet>().disable();
                    this.releaseIfIHookSb();
                  }
                  if (Object.op_Inequality((Object) this.bulletLeft, (Object) null))
                  {
                    this.bulletLeft.GetComponent<Bullet>().disable();
                    this.releaseIfIHookSb();
                  }
                  this.dashDirection = Vector3.op_Subtraction(((RaycastHit) ref hit).point, this.baseTransform.position);
                  this.launchLeftRope(hit, true);
                  this.launchRightRope(hit, true);
                  this.rope.Play();
                }
                this.facingDirection = Mathf.Atan2(this.dashDirection.x, this.dashDirection.z) * 57.29578f;
                this.targetRotation = Quaternion.Euler(0.0f, this.facingDirection, 0.0f);
                this.attackLoop = 3;
              }
              else
              {
                this.attackAnimation = !this.needLean ? "attack1" : (!this.leanLeft ? (Random.Range(0, 100) >= 50 ? "attack1_hook_r1" : "attack1_hook_r2") : (Random.Range(0, 100) >= 50 ? "attack1_hook_l1" : "attack1_hook_l2"));
                this.playAnimation(this.attackAnimation);
              }
            }
          }
          else if (SettingsManager.InputSettings.Human.AttackDefault.GetKeyDown())
          {
            if (this.needLean)
              this.attackAnimation = !SettingsManager.InputSettings.General.Left.GetKey() ? (!SettingsManager.InputSettings.General.Right.GetKey() ? (!this.leanLeft ? (Random.Range(0, 100) >= 50 ? "attack1_hook_r1" : "attack1_hook_r2") : (Random.Range(0, 100) >= 50 ? "attack1_hook_l1" : "attack1_hook_l2")) : (Random.Range(0, 100) >= 50 ? "attack1_hook_r1" : "attack1_hook_r2")) : (Random.Range(0, 100) >= 50 ? "attack1_hook_l1" : "attack1_hook_l2");
            else if (SettingsManager.InputSettings.General.Left.GetKey())
              this.attackAnimation = "attack2";
            else if (SettingsManager.InputSettings.General.Right.GetKey())
              this.attackAnimation = "attack1";
            else if (Object.op_Inequality((Object) this.lastHook, (Object) null))
            {
              if (Object.op_Inequality((Object) this.lastHook.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck"), (Object) null))
                this.attackAccordingToTarget(this.lastHook.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck"));
              else
                flag = true;
            }
            else if (Object.op_Inequality((Object) this.bulletLeft, (Object) null) && Object.op_Inequality((Object) this.bulletLeft.transform.parent, (Object) null))
            {
              Transform a = ((Component) this.bulletLeft.transform.parent).transform.root.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck");
              if (Object.op_Inequality((Object) a, (Object) null))
                this.attackAccordingToTarget(a);
              else
                this.attackAccordingToMouse();
            }
            else if (Object.op_Inequality((Object) this.bulletRight, (Object) null) && Object.op_Inequality((Object) this.bulletRight.transform.parent, (Object) null))
            {
              Transform a = ((Component) this.bulletRight.transform.parent).transform.root.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck");
              if (Object.op_Inequality((Object) a, (Object) null))
                this.attackAccordingToTarget(a);
              else
                this.attackAccordingToMouse();
            }
            else
            {
              GameObject nearestTitan = this.findNearestTitan();
              if (Object.op_Inequality((Object) nearestTitan, (Object) null))
              {
                Transform a = nearestTitan.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck");
                if (Object.op_Inequality((Object) a, (Object) null))
                  this.attackAccordingToTarget(a);
                else
                  this.attackAccordingToMouse();
              }
              else
                this.attackAccordingToMouse();
            }
          }
          if (!flag)
          {
            this.checkBoxLeft.GetComponent<TriggerColliderWeapon>().clearHits();
            this.checkBoxRight.GetComponent<TriggerColliderWeapon>().clearHits();
            if (this.grounded)
              this.baseRigidBody.AddForce(Vector3.op_Multiply(((Component) this).gameObject.transform.forward, 200f));
            this.playAnimation(this.attackAnimation);
            this.baseAnimation[this.attackAnimation].time = 0.0f;
            this.buttonAttackRelease = false;
            this.state = HERO_STATE.Attack;
            if (this.grounded || this.attackAnimation == "attack3_1" || this.attackAnimation == "attack5" || this.attackAnimation == "special_petra")
            {
              this.attackReleased = true;
              this.buttonAttackRelease = true;
            }
            else
              this.attackReleased = false;
            this.sparks.enableEmission = false;
          }
        }
        if (this.useGun)
        {
          if (SettingsManager.InputSettings.Human.AttackSpecial.GetKey())
          {
            this.leftArmAim = true;
            this.rightArmAim = true;
          }
          else if (SettingsManager.InputSettings.Human.AttackDefault.GetKey())
          {
            if (this.leftGunHasBullet)
            {
              this.leftArmAim = true;
              this.rightArmAim = false;
            }
            else
            {
              this.leftArmAim = false;
              this.rightArmAim = this.rightGunHasBullet;
            }
          }
          else
          {
            this.leftArmAim = false;
            this.rightArmAim = false;
          }
          if (this.leftArmAim || this.rightArmAim)
          {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            LayerMask layerMask5 = LayerMask.op_Implicit(1 << LayerMask.NameToLayer("Ground"));
            LayerMask layerMask6 = LayerMask.op_Implicit(LayerMask.op_Implicit(LayerMask.op_Implicit(1 << LayerMask.NameToLayer("EnemyBox"))) | LayerMask.op_Implicit(layerMask5));
            RaycastHit raycastHit;
            if (Physics.Raycast(ray, ref raycastHit, 1E+07f, ((LayerMask) ref layerMask6).value))
              this.gunTarget = ((RaycastHit) ref raycastHit).point;
          }
          bool flag1 = false;
          bool flag2 = false;
          bool flag3 = false;
          if (SettingsManager.InputSettings.Human.AttackSpecial.GetKeyUp() && this.skillId != "bomb")
          {
            if (this.leftGunHasBullet && this.rightGunHasBullet)
            {
              this.attackAnimation = !this.grounded ? "AHSS_shoot_both_air" : "AHSS_shoot_both";
              flag1 = true;
            }
            else if (!this.leftGunHasBullet && !this.rightGunHasBullet)
              flag2 = true;
            else
              flag3 = true;
          }
          if (flag3 || SettingsManager.InputSettings.Human.AttackDefault.GetKeyUp())
          {
            if (this.grounded)
            {
              if (this.leftGunHasBullet && this.rightGunHasBullet)
                this.attackAnimation = !this.isLeftHandHooked ? "AHSS_shoot_l" : "AHSS_shoot_r";
              else if (this.leftGunHasBullet)
                this.attackAnimation = "AHSS_shoot_l";
              else if (this.rightGunHasBullet)
                this.attackAnimation = "AHSS_shoot_r";
            }
            else if (this.leftGunHasBullet && this.rightGunHasBullet)
              this.attackAnimation = !this.isLeftHandHooked ? "AHSS_shoot_l_air" : "AHSS_shoot_r_air";
            else if (this.leftGunHasBullet)
              this.attackAnimation = "AHSS_shoot_l_air";
            else if (this.rightGunHasBullet)
              this.attackAnimation = "AHSS_shoot_r_air";
            if (this.leftGunHasBullet || this.rightGunHasBullet)
              flag1 = true;
            else
              flag2 = true;
          }
          if (flag1)
          {
            this.state = HERO_STATE.Attack;
            this.crossFade(this.attackAnimation, 0.05f);
            this.gunDummy.transform.position = this.baseTransform.position;
            this.gunDummy.transform.rotation = this.baseTransform.rotation;
            this.gunDummy.transform.LookAt(this.gunTarget);
            this.attackReleased = false;
            Quaternion rotation = this.gunDummy.transform.rotation;
            this.facingDirection = ((Quaternion) ref rotation).eulerAngles.y;
            this.targetRotation = Quaternion.Euler(0.0f, this.facingDirection, 0.0f);
          }
          else if (flag2 && (this.grounded || LevelInfo.getInfo(FengGameManagerMKII.level).type != GAMEMODE.PVP_AHSS && SettingsManager.LegacyGameSettings.AHSSAirReload.Value))
            this.changeBlade();
        }
      }
      else if (this.state == HERO_STATE.Attack)
      {
        if (!this.useGun)
        {
          if (!SettingsManager.InputSettings.Human.AttackDefault.GetKey())
            this.buttonAttackRelease = true;
          if (!this.attackReleased)
          {
            if (this.buttonAttackRelease)
            {
              this.continueAnimation();
              this.attackReleased = true;
            }
            else if ((double) this.baseAnimation[this.attackAnimation].normalizedTime >= 0.31999999284744263)
              this.pauseAnimation();
          }
          if (this.attackAnimation == "attack3_1" && (double) this.currentBladeSta > 0.0)
          {
            if ((double) this.baseAnimation[this.attackAnimation].normalizedTime >= 0.800000011920929)
            {
              if (!this.checkBoxLeft.GetComponent<TriggerColliderWeapon>().active_me)
              {
                this.checkBoxLeft.GetComponent<TriggerColliderWeapon>().active_me = true;
                if (SettingsManager.GraphicsSettings.WeaponTrailEnabled.Value)
                {
                  this.leftbladetrail2.Activate();
                  this.rightbladetrail2.Activate();
                  this.leftbladetrail.Activate();
                  this.rightbladetrail.Activate();
                }
                this.baseRigidBody.velocity = Vector3.op_Multiply(Vector3.op_UnaryNegation(Vector3.up), 30f);
              }
              if (!this.checkBoxRight.GetComponent<TriggerColliderWeapon>().active_me)
              {
                this.checkBoxRight.GetComponent<TriggerColliderWeapon>().active_me = true;
                this.slash.Play();
              }
            }
            else if (this.checkBoxLeft.GetComponent<TriggerColliderWeapon>().active_me)
            {
              this.checkBoxLeft.GetComponent<TriggerColliderWeapon>().active_me = false;
              this.checkBoxRight.GetComponent<TriggerColliderWeapon>().active_me = false;
              this.checkBoxLeft.GetComponent<TriggerColliderWeapon>().clearHits();
              this.checkBoxRight.GetComponent<TriggerColliderWeapon>().clearHits();
              this.leftbladetrail.StopSmoothly(0.1f);
              this.rightbladetrail.StopSmoothly(0.1f);
              this.leftbladetrail2.StopSmoothly(0.1f);
              this.rightbladetrail2.StopSmoothly(0.1f);
            }
          }
          else
          {
            float num;
            float normalizedTime;
            if ((double) this.currentBladeSta == 0.0)
              normalizedTime = num = -1f;
            else if (this.attackAnimation == "attack5")
            {
              normalizedTime = 0.35f;
              num = 0.5f;
            }
            else if (this.attackAnimation == "special_petra")
            {
              normalizedTime = 0.35f;
              num = 0.48f;
            }
            else if (this.attackAnimation == "special_armin")
            {
              normalizedTime = 0.25f;
              num = 0.35f;
            }
            else if (this.attackAnimation == "attack4")
            {
              normalizedTime = 0.6f;
              num = 0.9f;
            }
            else if (this.attackAnimation == "special_sasha")
            {
              normalizedTime = num = -1f;
            }
            else
            {
              normalizedTime = 0.5f;
              num = 0.85f;
            }
            if ((double) this.baseAnimation[this.attackAnimation].normalizedTime > (double) normalizedTime && (double) this.baseAnimation[this.attackAnimation].normalizedTime < (double) num)
            {
              if (!this.checkBoxLeft.GetComponent<TriggerColliderWeapon>().active_me)
              {
                this.checkBoxLeft.GetComponent<TriggerColliderWeapon>().active_me = true;
                this.slash.Play();
                if (SettingsManager.GraphicsSettings.WeaponTrailEnabled.Value)
                {
                  this.leftbladetrail2.Activate();
                  this.rightbladetrail2.Activate();
                  this.leftbladetrail.Activate();
                  this.rightbladetrail.Activate();
                }
              }
              if (!this.checkBoxRight.GetComponent<TriggerColliderWeapon>().active_me)
                this.checkBoxRight.GetComponent<TriggerColliderWeapon>().active_me = true;
            }
            else if (this.checkBoxLeft.GetComponent<TriggerColliderWeapon>().active_me)
            {
              this.checkBoxLeft.GetComponent<TriggerColliderWeapon>().active_me = false;
              this.checkBoxRight.GetComponent<TriggerColliderWeapon>().active_me = false;
              this.checkBoxLeft.GetComponent<TriggerColliderWeapon>().clearHits();
              this.checkBoxRight.GetComponent<TriggerColliderWeapon>().clearHits();
              this.leftbladetrail2.StopSmoothly(0.1f);
              this.rightbladetrail2.StopSmoothly(0.1f);
              this.leftbladetrail.StopSmoothly(0.1f);
              this.rightbladetrail.StopSmoothly(0.1f);
            }
            if (this.attackLoop > 0 && (double) this.baseAnimation[this.attackAnimation].normalizedTime > (double) num)
            {
              --this.attackLoop;
              this.playAnimationAt(this.attackAnimation, normalizedTime);
            }
          }
          if ((double) this.baseAnimation[this.attackAnimation].normalizedTime >= 1.0)
          {
            if (this.attackAnimation == "special_marco_0" || this.attackAnimation == "special_marco_1")
            {
              if (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE)
              {
                if (!PhotonNetwork.isMasterClient)
                  this.photonView.RPC("netTauntAttack", PhotonTargets.MasterClient, (object) 5f, (object) 100f);
                else
                  this.netTauntAttack(5f, 100f, (PhotonMessageInfo) null);
              }
              else
                this.netTauntAttack(5f, 100f, (PhotonMessageInfo) null);
              this.falseAttack();
              this.idle();
            }
            else if (this.attackAnimation == "special_armin")
            {
              if (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE)
              {
                if (!PhotonNetwork.isMasterClient)
                  this.photonView.RPC("netlaughAttack", PhotonTargets.MasterClient);
                else
                  this.netlaughAttack((PhotonMessageInfo) null);
              }
              else
              {
                foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("titan"))
                {
                  if ((double) Vector3.Distance(gameObject.transform.position, this.baseTransform.position) < 50.0 && (double) Vector3.Angle(gameObject.transform.forward, Vector3.op_Subtraction(this.baseTransform.position, gameObject.transform.position)) < 90.0 && Object.op_Inequality((Object) gameObject.GetComponent<TITAN>(), (Object) null))
                    gameObject.GetComponent<TITAN>().beLaughAttacked();
                }
              }
              this.falseAttack();
              this.idle();
            }
            else if (this.attackAnimation == "attack3_1")
            {
              Rigidbody baseRigidBody = this.baseRigidBody;
              baseRigidBody.velocity = Vector3.op_Subtraction(baseRigidBody.velocity, Vector3.op_Multiply(Vector3.op_Multiply(Vector3.up, Time.deltaTime), 30f));
            }
            else
            {
              this.falseAttack();
              this.idle();
            }
          }
          if (this.baseAnimation.IsPlaying("attack3_2") && (double) this.baseAnimation["attack3_2"].normalizedTime >= 1.0)
          {
            this.falseAttack();
            this.idle();
          }
        }
        else
        {
          this.baseTransform.rotation = Quaternion.Lerp(this.baseTransform.rotation, this.gunDummy.transform.rotation, Time.deltaTime * 30f);
          if (!this.attackReleased && (double) this.baseAnimation[this.attackAnimation].normalizedTime > 0.16699999570846558)
          {
            this.attackReleased = true;
            bool flag = false;
            if (this.attackAnimation == "AHSS_shoot_both" || this.attackAnimation == "AHSS_shoot_both_air")
            {
              flag = true;
              this.leftGunHasBullet = false;
              this.rightGunHasBullet = false;
              this.baseRigidBody.AddForce(Vector3.op_Multiply(Vector3.op_UnaryNegation(this.baseTransform.forward), 1000f), (ForceMode) 5);
            }
            else
            {
              if (this.attackAnimation == "AHSS_shoot_l" || this.attackAnimation == "AHSS_shoot_l_air")
                this.leftGunHasBullet = false;
              else
                this.rightGunHasBullet = false;
              this.baseRigidBody.AddForce(Vector3.op_Multiply(Vector3.op_UnaryNegation(this.baseTransform.forward), 600f), (ForceMode) 5);
            }
            this.baseRigidBody.AddForce(Vector3.op_Multiply(Vector3.up, 200f), (ForceMode) 5);
            string prefabName = "FX/shotGun";
            if (flag)
              prefabName = "FX/shotGun 1";
            if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER && this.photonView.isMine)
            {
              GameObject gameObject = PhotonNetwork.Instantiate(prefabName, Vector3.op_Subtraction(Vector3.op_Addition(this.baseTransform.position, Vector3.op_Multiply(this.baseTransform.up, 0.8f)), Vector3.op_Multiply(this.baseTransform.right, 0.1f)), this.baseTransform.rotation, 0);
              if (Object.op_Inequality((Object) gameObject.GetComponent<EnemyfxIDcontainer>(), (Object) null))
                gameObject.GetComponent<EnemyfxIDcontainer>().myOwnerViewID = this.photonView.viewID;
            }
            else
            {
              GameObject gameObject1 = (GameObject) Object.Instantiate(Resources.Load(prefabName), Vector3.op_Subtraction(Vector3.op_Addition(this.baseTransform.position, Vector3.op_Multiply(this.baseTransform.up, 0.8f)), Vector3.op_Multiply(this.baseTransform.right, 0.1f)), this.baseTransform.rotation);
            }
          }
          if ((double) this.baseAnimation[this.attackAnimation].normalizedTime >= 1.0)
          {
            this.falseAttack();
            this.idle();
          }
          if (!this.baseAnimation.IsPlaying(this.attackAnimation))
          {
            this.falseAttack();
            this.idle();
          }
        }
      }
      else if (this.state == HERO_STATE.ChangeBlade)
      {
        if (this.useGun)
        {
          if ((double) this.baseAnimation[this.reloadAnimation].normalizedTime > 0.2199999988079071)
          {
            if (!this.leftGunHasBullet && this.setup.part_blade_l.activeSelf)
            {
              this.setup.part_blade_l.SetActive(false);
              Transform transform = this.setup.part_blade_l.transform;
              GameObject gameObject = (GameObject) Object.Instantiate(Resources.Load("Character_parts/character_gun_l"), transform.position, transform.rotation);
              gameObject.renderer.material = CharacterMaterials.materials[this.setup.myCostume._3dmg_texture];
              Vector3 vector3_1 = Vector3.op_Subtraction(Vector3.op_Addition(Vector3.op_Multiply(Vector3.op_UnaryNegation(this.baseTransform.forward), 10f), Vector3.op_Multiply(this.baseTransform.up, 5f)), this.baseTransform.right);
              gameObject.rigidbody.AddForce(vector3_1, (ForceMode) 1);
              Vector3 vector3_2;
              // ISSUE: explicit constructor call
              ((Vector3) ref vector3_2).\u002Ector((float) Random.Range(-100, 100), (float) Random.Range(-100, 100), (float) Random.Range(-100, 100));
              gameObject.rigidbody.AddTorque(vector3_2, (ForceMode) 5);
            }
            if (!this.rightGunHasBullet && this.setup.part_blade_r.activeSelf)
            {
              this.setup.part_blade_r.SetActive(false);
              Transform transform = this.setup.part_blade_r.transform;
              GameObject gameObject = (GameObject) Object.Instantiate(Resources.Load("Character_parts/character_gun_r"), transform.position, transform.rotation);
              gameObject.renderer.material = CharacterMaterials.materials[this.setup.myCostume._3dmg_texture];
              Vector3 vector3_3 = Vector3.op_Addition(Vector3.op_Addition(Vector3.op_Multiply(Vector3.op_UnaryNegation(this.baseTransform.forward), 10f), Vector3.op_Multiply(this.baseTransform.up, 5f)), this.baseTransform.right);
              gameObject.rigidbody.AddForce(vector3_3, (ForceMode) 1);
              Vector3 vector3_4;
              // ISSUE: explicit constructor call
              ((Vector3) ref vector3_4).\u002Ector((float) Random.Range(-300, 300), (float) Random.Range(-300, 300), (float) Random.Range(-300, 300));
              gameObject.rigidbody.AddTorque(vector3_4, (ForceMode) 5);
            }
          }
          if ((double) this.baseAnimation[this.reloadAnimation].normalizedTime > 0.62000000476837158 && !this.throwedBlades)
          {
            this.throwedBlades = true;
            if (this.leftBulletLeft > 0 && !this.leftGunHasBullet)
            {
              --this.leftBulletLeft;
              this.setup.part_blade_l.SetActive(true);
              this.leftGunHasBullet = true;
            }
            if (this.rightBulletLeft > 0 && !this.rightGunHasBullet)
            {
              this.setup.part_blade_r.SetActive(true);
              --this.rightBulletLeft;
              this.rightGunHasBullet = true;
            }
            this.updateRightMagUI();
            this.updateLeftMagUI();
          }
          if ((double) this.baseAnimation[this.reloadAnimation].normalizedTime > 1.0)
            this.idle();
        }
        else
        {
          if (!this.grounded)
          {
            if ((double) ((Component) this).animation[this.reloadAnimation].normalizedTime >= 0.20000000298023224 && !this.throwedBlades)
            {
              this.throwedBlades = true;
              if (this.setup.part_blade_l.activeSelf)
                this.throwBlades();
            }
            if ((double) ((Component) this).animation[this.reloadAnimation].normalizedTime >= 0.56000000238418579 && this.currentBladeNum > 0)
            {
              this.setup.part_blade_l.SetActive(true);
              this.setup.part_blade_r.SetActive(true);
              this.currentBladeSta = this.totalBladeSta;
            }
          }
          else
          {
            if ((double) this.baseAnimation[this.reloadAnimation].normalizedTime >= 0.12999999523162842 && !this.throwedBlades)
            {
              this.throwedBlades = true;
              if (this.setup.part_blade_l.activeSelf)
                this.throwBlades();
            }
            if ((double) this.baseAnimation[this.reloadAnimation].normalizedTime >= 0.37000000476837158 && this.currentBladeNum > 0)
            {
              this.setup.part_blade_l.SetActive(true);
              this.setup.part_blade_r.SetActive(true);
              this.currentBladeSta = this.totalBladeSta;
            }
          }
          if ((double) this.baseAnimation[this.reloadAnimation].normalizedTime >= 1.0)
            this.idle();
        }
      }
      else if (this.state == HERO_STATE.Salute)
      {
        this._currentEmoteActionTime -= Time.deltaTime;
        if ((double) this._currentEmoteActionTime <= 0.0)
          this.idle();
      }
      else if (this.state == HERO_STATE.GroundDodge)
      {
        if (this.baseAnimation.IsPlaying("dodge"))
        {
          if (!this.grounded && (double) this.baseAnimation["dodge"].normalizedTime > 0.60000002384185791)
            this.idle();
          if ((double) this.baseAnimation["dodge"].normalizedTime >= 1.0)
            this.idle();
        }
      }
      else if (this.state == HERO_STATE.Land)
      {
        if (this.baseAnimation.IsPlaying("dash_land") && (double) this.baseAnimation["dash_land"].normalizedTime >= 1.0)
          this.idle();
      }
      else if (this.state == HERO_STATE.FillGas)
      {
        if (this.baseAnimation.IsPlaying("supply") && (double) this.baseAnimation["supply"].normalizedTime >= 1.0)
        {
          if (this.skillId != "bomb")
          {
            this.currentBladeSta = this.totalBladeSta;
            this.currentBladeNum = this.totalBladeNum;
            if (!this.useGun)
            {
              this.setup.part_blade_l.SetActive(true);
              this.setup.part_blade_r.SetActive(true);
            }
            else
            {
              this.leftBulletLeft = this.rightBulletLeft = this.bulletMAX;
              this.leftGunHasBullet = this.rightGunHasBullet = true;
              this.setup.part_blade_l.SetActive(true);
              this.setup.part_blade_r.SetActive(true);
              this.updateRightMagUI();
              this.updateLeftMagUI();
            }
          }
          this.currentGas = this.totalGas;
          this.idle();
        }
      }
      else if (this.state == HERO_STATE.Slide)
      {
        if (!this.grounded)
          this.idle();
      }
      else if (this.state == HERO_STATE.AirDodge)
      {
        if ((double) this.dashTime > 0.0)
        {
          this.dashTime -= Time.deltaTime;
          if ((double) this.currentSpeed > (double) this.originVM)
            this.baseRigidBody.AddForce(Vector3.op_Multiply(Vector3.op_Multiply(Vector3.op_UnaryNegation(this.baseRigidBody.velocity), Time.deltaTime), 1.7f), (ForceMode) 2);
        }
        else
        {
          this.dashTime = 0.0f;
          this.idle();
        }
      }
      if (!GameMenu.InMenu())
      {
        if (SettingsManager.InputSettings.Human.HookLeft.GetKey() && (!this.baseAnimation.IsPlaying("attack3_1") && !this.baseAnimation.IsPlaying("attack5") && !this.baseAnimation.IsPlaying("special_petra") && this.state != HERO_STATE.Grab || this.state == HERO_STATE.Idle))
        {
          if (Object.op_Inequality((Object) this.bulletLeft, (Object) null))
          {
            this.QHold = true;
          }
          else
          {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            LayerMask layerMask7 = LayerMask.op_Implicit(1 << LayerMask.NameToLayer("Ground"));
            LayerMask layerMask8 = LayerMask.op_Implicit(LayerMask.op_Implicit(LayerMask.op_Implicit(1 << LayerMask.NameToLayer("EnemyBox"))) | LayerMask.op_Implicit(layerMask7));
            RaycastHit hit;
            if (Physics.Raycast(ray, ref hit, 10000f, ((LayerMask) ref layerMask8).value))
            {
              this.launchLeftRope(hit, true);
              this.rope.Play();
            }
          }
        }
        else
          this.QHold = false;
        if (SettingsManager.InputSettings.Human.HookRight.GetKey() && (!this.baseAnimation.IsPlaying("attack3_1") && !this.baseAnimation.IsPlaying("attack5") && !this.baseAnimation.IsPlaying("special_petra") && this.state != HERO_STATE.Grab || this.state == HERO_STATE.Idle))
        {
          if (Object.op_Inequality((Object) this.bulletRight, (Object) null))
          {
            this.EHold = true;
          }
          else
          {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            LayerMask layerMask9 = LayerMask.op_Implicit(1 << LayerMask.NameToLayer("Ground"));
            LayerMask layerMask10 = LayerMask.op_Implicit(LayerMask.op_Implicit(LayerMask.op_Implicit(1 << LayerMask.NameToLayer("EnemyBox"))) | LayerMask.op_Implicit(layerMask9));
            RaycastHit hit;
            if (Physics.Raycast(ray, ref hit, 10000f, ((LayerMask) ref layerMask10).value))
            {
              this.launchRightRope(hit, true);
              this.rope.Play();
            }
          }
        }
        else
          this.EHold = false;
        if (SettingsManager.InputSettings.Human.HookBoth.GetKey() && (!this.baseAnimation.IsPlaying("attack3_1") && !this.baseAnimation.IsPlaying("attack5") && !this.baseAnimation.IsPlaying("special_petra") && this.state != HERO_STATE.Grab || this.state == HERO_STATE.Idle))
        {
          this.QHold = true;
          this.EHold = true;
          if (Object.op_Equality((Object) this.bulletLeft, (Object) null) && Object.op_Equality((Object) this.bulletRight, (Object) null))
          {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            LayerMask layerMask11 = LayerMask.op_Implicit(1 << LayerMask.NameToLayer("Ground"));
            LayerMask layerMask12 = LayerMask.op_Implicit(LayerMask.op_Implicit(LayerMask.op_Implicit(1 << LayerMask.NameToLayer("EnemyBox"))) | LayerMask.op_Implicit(layerMask11));
            RaycastHit hit;
            if (Physics.Raycast(ray, ref hit, 1000000f, ((LayerMask) ref layerMask12).value))
            {
              this.launchLeftRope(hit, false);
              this.launchRightRope(hit, false);
              this.rope.Play();
            }
          }
        }
      }
      if (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE || IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE && !GameMenu.Paused)
      {
        this.calcSkillCD();
        this.calcFlareCD();
      }
      if (!this.useGun)
      {
        if (((Component) this.leftbladetrail).gameObject.GetActive())
        {
          this.leftbladetrail.update();
          this.rightbladetrail.update();
        }
        if (((Component) this.leftbladetrail2).gameObject.GetActive())
        {
          this.leftbladetrail2.update();
          this.rightbladetrail2.update();
        }
        if (((Component) this.leftbladetrail).gameObject.GetActive())
        {
          this.leftbladetrail.lateUpdate();
          this.rightbladetrail.lateUpdate();
        }
        if (((Component) this.leftbladetrail2).gameObject.GetActive())
        {
          this.leftbladetrail2.lateUpdate();
          this.rightbladetrail2.lateUpdate();
        }
      }
      if (GameMenu.Paused)
        return;
      this.showSkillCD();
      this.showFlareCD2();
      this.showGas2();
      this.showAimUI2();
    }
    else
    {
      if (!this.isCannon || GameMenu.Paused)
        return;
      this.showAimUI2();
      this.calcSkillCD();
      this.showSkillCD();
    }
  }

  public void updateCannon()
  {
    this.baseTransform.position = this.myCannonPlayer.position;
    this.baseTransform.rotation = this.myCannonBase.rotation;
  }

  private void LaunchThunderSpear()
  {
    if (Object.op_Inequality((Object) this.myBomb, (Object) null) && !this.myBomb.Disabled)
      this.myBomb.Explode(this.bombRadius);
    this.detonate = false;
    this.bombTime = 0.0f;
    this.skillCDDuration = this.bombCD;
    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    LayerMask layerMask = LayerMask.op_Implicit(1 << LayerMask.NameToLayer("Ground") | 1 << LayerMask.NameToLayer("EnemyBox"));
    Vector3 vector3_1 = Vector3.op_Addition(this.baseTransform.position, Vector3.op_Multiply(Vector3.forward, 1000f));
    RaycastHit raycastHit;
    if (Physics.Raycast(ray, ref raycastHit, 1000000f, ((LayerMask) ref layerMask).value))
      vector3_1 = ((RaycastHit) ref raycastHit).point;
    Vector3 vector3_2 = Vector3.Normalize(Vector3.op_Subtraction(vector3_1, this.baseTransform.position));
    Vector3 position;
    if ((double) Vector3.Cross(this.baseTransform.forward, vector3_2).y < 0.0 && this.state != HERO_STATE.Land)
    {
      position = this.ThunderSpearL.transform.position;
      this.ThunderSpearL.audio.Play();
      this.SetThunderSpears(false, true);
      this.attackAnimation = "AHSS_shoot_l";
    }
    else
    {
      position = this.ThunderSpearR.transform.position;
      this.ThunderSpearR.audio.Play();
      this.SetThunderSpears(true, false);
      this.attackAnimation = "AHSS_shoot_r";
    }
    Vector3 vector3_3 = Vector3.Normalize(Vector3.op_Subtraction(vector3_1, position));
    if (this.grounded)
      position = Vector3.op_Addition(position, Vector3.op_Multiply(vector3_3, 1f));
    if (this.state != HERO_STATE.Slide)
    {
      if (this.state == HERO_STATE.Attack)
        this.buttonAttackRelease = true;
      this.playAnimationAt(this.attackAnimation, 0.1f);
      this.state = HERO_STATE.Attack;
      Quaternion quaternion = Quaternion.LookRotation(vector3_2);
      this.facingDirection = ((Quaternion) ref quaternion).eulerAngles.y;
      this.targetRotation = Quaternion.Euler(0.0f, this.facingDirection, 0.0f);
    }
    GameObject gameObject = PhotonNetwork.Instantiate("RCAsset/BombMain", position, Quaternion.LookRotation(vector3_3), 0);
    gameObject.rigidbody.velocity = Vector3.op_Multiply(vector3_3, this.bombSpeed);
    this.myBomb = gameObject.GetComponent<Bomb>();
    this.myBomb.Setup(this, this.bombRadius);
  }

  public void UpdateThunderSpear()
  {
    if (!(this.skillId == "bomb"))
      return;
    this.leftArmAim = false;
    this.rightArmAim = false;
    bool keyDown = SettingsManager.InputSettings.Human.AttackSpecial.GetKeyDown();
    bool keyUp = SettingsManager.InputSettings.Human.AttackSpecial.GetKeyUp();
    if ((double) this.skillCDDuration <= 0.0 && (!this.ThunderSpearLModel.activeSelf || !this.ThunderSpearRModel.activeSelf))
      this.SetThunderSpears(true, true);
    if (keyDown && (double) this.skillCDDuration <= 0.0)
    {
      this.LaunchThunderSpear();
    }
    else
    {
      if (!Object.op_Inequality((Object) this.myBomb, (Object) null) || this.myBomb.Disabled)
        return;
      this.bombTime += Time.deltaTime;
      bool flag = false;
      if (keyUp)
        this.detonate = true;
      else if (keyDown && this.detonate)
      {
        this.detonate = false;
        flag = true;
      }
      if ((double) this.bombTime >= (double) this.bombTimeMax)
        flag = true;
      if (!flag)
        return;
      this.myBomb.Explode(this.bombRadius);
      this.detonate = false;
    }
  }

  private bool IsFiringThunderSpear()
  {
    if (!(this.skillId == "bomb"))
      return false;
    return this.baseAnimation.IsPlaying("AHSS_shoot_r") || this.baseAnimation.IsPlaying("AHSS_shoot_l");
  }

  private void updateLeftMagUI()
  {
    for (int index = 1; index <= this.bulletMAX; ++index)
      ((Behaviour) GameObject.Find("bulletL" + index.ToString()).GetComponent<UISprite>()).enabled = false;
    for (int index = 1; index <= this.leftBulletLeft; ++index)
      ((Behaviour) GameObject.Find("bulletL" + index.ToString()).GetComponent<UISprite>()).enabled = true;
  }

  private void updateRightMagUI()
  {
    for (int index = 1; index <= this.bulletMAX; ++index)
      ((Behaviour) GameObject.Find("bulletR" + index.ToString()).GetComponent<UISprite>()).enabled = false;
    for (int index = 1; index <= this.rightBulletLeft; ++index)
      ((Behaviour) GameObject.Find("bulletR" + index.ToString()).GetComponent<UISprite>()).enabled = true;
  }

  public void useBlade(int amount = 0)
  {
    if (amount == 0)
      amount = -1;
    amount *= 2;
    if ((double) this.currentBladeSta <= 0.0)
      return;
    this.currentBladeSta -= (float) amount;
    if ((double) this.currentBladeSta > 0.0)
      return;
    if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE || this.photonView.isMine)
    {
      this.leftbladetrail.Deactivate();
      this.rightbladetrail.Deactivate();
      this.leftbladetrail2.Deactivate();
      this.rightbladetrail2.Deactivate();
      this.checkBoxLeft.GetComponent<TriggerColliderWeapon>().active_me = false;
      this.checkBoxRight.GetComponent<TriggerColliderWeapon>().active_me = false;
    }
    this.currentBladeSta = 0.0f;
    this.throwBlades();
  }

  private void useGas(float amount = 0.0f)
  {
    if (SettingsManager.LegacyGameSettings.BombModeEnabled.Value && SettingsManager.LegacyGameSettings.BombModeInfiniteGas.Value)
      return;
    if ((double) amount == 0.0)
      amount = this.useGasSpeed;
    if ((double) this.currentGas <= 0.0)
      return;
    this.currentGas -= amount;
    if ((double) this.currentGas >= 0.0)
      return;
    this.currentGas = 0.0f;
  }

  [RPC]
  private void whoIsMyErenTitan(int id, PhotonMessageInfo info)
  {
    if (info != null && info.sender != this.photonView.owner)
    {
      FengGameManagerMKII.instance.kickPlayerRCIfMC(info.sender, true, "hero eren titan exploit");
    }
    else
    {
      this.eren_titan = ((Component) PhotonView.Find(id)).gameObject;
      this.titanForm = true;
    }
  }

  public bool isGrabbed => this.state == HERO_STATE.Grab;

  private HERO_STATE state
  {
    get => this._state;
    set
    {
      if (this._state == HERO_STATE.AirDodge || this._state == HERO_STATE.GroundDodge)
        this.dashTime = 0.0f;
      this._state = value;
    }
  }
}
