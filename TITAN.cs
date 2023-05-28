// Decompiled with JetBrains decompiler
// Type: TITAN
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using Constants;
using CustomSkins;
using ExitGames.Client.Photon;
using Photon;
using Settings;
using System;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;

internal class TITAN : MonoBehaviour
{
  private Vector3 abnorma_jump_bite_horizon_v;
  public AbnormalType abnormalType;
  public int activeRad = int.MaxValue;
  private float angle;
  public bool asClientLookTarget;
  private string attackAnimation;
  private float attackCheckTime;
  private float attackCheckTimeA;
  private float attackCheckTimeB;
  private int attackCount;
  public float attackDistance = 13f;
  private bool attacked;
  private float attackEndWait;
  public float attackWait = 1f;
  public Animation baseAnimation;
  public AudioSource baseAudioSource;
  public List<Collider> baseColliders;
  public Transform baseGameObjectTransform;
  public Rigidbody baseRigidBody;
  public Transform baseTransform;
  private float between2;
  public float chaseDistance = 80f;
  public ArrayList checkPoints = new ArrayList();
  public bool colliderEnabled;
  public TITAN_CONTROLLER controller;
  public GameObject currentCamera;
  private Transform currentGrabHand;
  public int currentHealth;
  private float desDeg;
  private float dieTime;
  public bool eye;
  private string fxName;
  private Vector3 fxPosition;
  private Quaternion fxRotation;
  private float getdownTime;
  private GameObject grabbedTarget;
  public GameObject grabTF;
  private float gravity = 120f;
  private bool grounded;
  public bool hasDie;
  private bool hasDieSteam;
  public bool hasExplode;
  public bool hasload;
  public bool hasSetLevel;
  public bool hasSpawn;
  private Transform head;
  private Vector3 headscale = Vector3.one;
  public GameObject healthLabel;
  public bool healthLabelEnabled;
  public float healthTime;
  private string hitAnimation;
  private float hitPause;
  public bool isAlarm;
  private bool isAttackMoveByCore;
  private bool isGrabHandLeft;
  public bool isHooked;
  public bool isLook;
  public bool isThunderSpear;
  public float lagMax;
  private bool leftHandAttack;
  public GameObject mainMaterial;
  public int maxHealth;
  private float maxStamina = 320f;
  public float maxVelocityChange = 10f;
  public static float minusDistance = 99999f;
  public static GameObject minusDistanceEnemy;
  public FengGameManagerMKII MultiplayerManager;
  public int myDifficulty;
  public float myDistance;
  public GROUP myGroup = GROUP.T;
  public GameObject myHero;
  public float myLevel = 1f;
  public TitanTrigger myTitanTrigger;
  private Transform neck;
  private bool needFreshCorePosition;
  private string nextAttackAnimation;
  public bool nonAI;
  private bool nonAIcombo;
  private Vector3 oldCorePosition;
  private Quaternion oldHeadRotation;
  public PVPcheckPoint PVPfromCheckPt;
  private float random_run_time;
  private float rockInterval;
  public bool rockthrow;
  private string runAnimation;
  private float sbtime;
  public int skin;
  private Vector3 spawnPt;
  public float speed = 7f;
  private float stamina = 320f;
  private TitanState state;
  private int stepSoundPhase = 2;
  private bool stuck;
  private float stuckTime;
  private float stuckTurnAngle;
  private Vector3 targetCheckPt;
  private Quaternion targetHeadRotation;
  private float targetR;
  private float tauntTime;
  private GameObject throwRock;
  private string turnAnimation;
  private float turnDeg;
  private GameObject whoHasTauntMe;
  private TitanCustomSkinLoader _customSkinLoader;
  private bool _hasRunStart;
  private HashSet<string> _ignoreLookTargetAnimations;
  private HashSet<string> _fastHeadRotationAnimations;
  private bool _ignoreLookTarget;
  private bool _fastHeadRotation;

  private void HideTitanIfBomb()
  {
    if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE || !PhotonNetwork.isMasterClient || !SettingsManager.LegacyGameSettings.BombModeEnabled.Value || !SettingsManager.LegacyGameSettings.BombModeDisableTitans.Value)
      return;
    ((Component) this).transform.position = new Vector3(-10000f, -10000f, -10000f);
  }

  public bool WillDIe(int damage) => !this.hasDie && (!SettingsManager.LegacyGameSettings.TitanArmorEnabled.Value || damage >= SettingsManager.LegacyGameSettings.TitanArmor.Value || this.abnormalType == AbnormalType.TYPE_CRAWLER && !SettingsManager.LegacyGameSettings.TitanArmorCrawlerEnabled.Value) && (double) (this.currentHealth - damage) <= 0.0;

  private void attack(string type)
  {
    this.state = TitanState.attack;
    this.attacked = false;
    this.isAlarm = true;
    if (this.attackAnimation == type)
    {
      this.attackAnimation = type;
      this.playAnimationAt("attack_" + type, 0.0f);
    }
    else
    {
      this.attackAnimation = type;
      this.playAnimationAt("attack_" + type, 0.0f);
    }
    this.nextAttackAnimation = (string) null;
    this.fxName = (string) null;
    this.isAttackMoveByCore = false;
    this.attackCheckTime = 0.0f;
    this.attackCheckTimeA = 0.0f;
    this.attackCheckTimeB = 0.0f;
    this.attackEndWait = 0.0f;
    this.fxRotation = Quaternion.Euler(270f, 0.0f, 0.0f);
    string key = type;
    if (key != null)
    {
      // ISSUE: reference to a compiler-generated field
      if (TITAN.fswitchSmap6 == null)
      {
        // ISSUE: reference to a compiler-generated field
        TITAN.fswitchSmap6 = new Dictionary<string, int>(22)
        {
          {
            "abnormal_getup",
            0
          },
          {
            "abnormal_jump",
            1
          },
          {
            "combo_1",
            2
          },
          {
            "combo_2",
            3
          },
          {
            "combo_3",
            4
          },
          {
            "front_ground",
            5
          },
          {
            "kick",
            6
          },
          {
            "slap_back",
            7
          },
          {
            "slap_face",
            8
          },
          {
            "stomp",
            9
          },
          {
            "bite",
            10
          },
          {
            "bite_l",
            11
          },
          {
            "bite_r",
            12
          },
          {
            "jumper_0",
            13
          },
          {
            "crawler_jump_0",
            14
          },
          {
            "anti_AE_l",
            15
          },
          {
            "anti_AE_r",
            16
          },
          {
            "anti_AE_low_l",
            17
          },
          {
            "anti_AE_low_r",
            18
          },
          {
            "quick_turn_l",
            19
          },
          {
            "quick_turn_r",
            20
          },
          {
            "throw",
            21
          }
        };
      }
      int num;
      // ISSUE: reference to a compiler-generated field
      if (TITAN.fswitchSmap6.TryGetValue(key, out num))
      {
        switch (num)
        {
          case 0:
            this.attackCheckTime = 0.0f;
            this.fxName = string.Empty;
            break;
          case 1:
            this.nextAttackAnimation = "abnormal_getup";
            this.attackEndWait = this.nonAI ? 0.0f : (this.myDifficulty <= 0 ? Random.Range(1f, 4f) : Random.Range(0.0f, 1f));
            this.attackCheckTime = 0.75f;
            this.fxName = "boom4";
            Quaternion rotation = ((Component) this).transform.rotation;
            this.fxRotation = Quaternion.Euler(270f, ((Quaternion) ref rotation).eulerAngles.y, 0.0f);
            break;
          case 2:
            this.nextAttackAnimation = "combo_2";
            this.attackCheckTimeA = 0.54f;
            this.attackCheckTimeB = 0.76f;
            this.nonAIcombo = false;
            this.isAttackMoveByCore = true;
            this.leftHandAttack = false;
            break;
          case 3:
            if (this.abnormalType != AbnormalType.TYPE_PUNK)
              this.nextAttackAnimation = "combo_3";
            this.attackCheckTimeA = 0.37f;
            this.attackCheckTimeB = 0.57f;
            this.nonAIcombo = false;
            this.isAttackMoveByCore = true;
            this.leftHandAttack = true;
            break;
          case 4:
            this.nonAIcombo = false;
            this.isAttackMoveByCore = true;
            this.attackCheckTime = 0.21f;
            this.fxName = "boom1";
            break;
          case 5:
            this.fxName = "boom1";
            this.attackCheckTime = 0.45f;
            break;
          case 6:
            this.fxName = "boom5";
            this.fxRotation = ((Component) this).transform.rotation;
            this.attackCheckTime = 0.43f;
            break;
          case 7:
            this.fxName = "boom3";
            this.attackCheckTime = 0.66f;
            break;
          case 8:
            this.fxName = "boom3";
            this.attackCheckTime = 0.655f;
            break;
          case 9:
            this.fxName = "boom2";
            this.attackCheckTime = 0.42f;
            break;
          case 10:
            this.fxName = "bite";
            this.attackCheckTime = 0.6f;
            break;
          case 11:
            this.fxName = "bite";
            this.attackCheckTime = 0.4f;
            break;
          case 12:
            this.fxName = "bite";
            this.attackCheckTime = 0.4f;
            break;
          case 13:
            this.abnorma_jump_bite_horizon_v = Vector3.zero;
            break;
          case 14:
            this.abnorma_jump_bite_horizon_v = Vector3.zero;
            break;
          case 15:
            this.attackCheckTimeA = 0.31f;
            this.attackCheckTimeB = 0.4f;
            this.leftHandAttack = true;
            break;
          case 16:
            this.attackCheckTimeA = 0.31f;
            this.attackCheckTimeB = 0.4f;
            this.leftHandAttack = false;
            break;
          case 17:
            this.attackCheckTimeA = 0.31f;
            this.attackCheckTimeB = 0.4f;
            this.leftHandAttack = true;
            break;
          case 18:
            this.attackCheckTimeA = 0.31f;
            this.attackCheckTimeB = 0.4f;
            this.leftHandAttack = false;
            break;
          case 19:
            this.attackCheckTimeA = 2f;
            this.attackCheckTimeB = 2f;
            this.isAttackMoveByCore = true;
            break;
          case 20:
            this.attackCheckTimeA = 2f;
            this.attackCheckTimeB = 2f;
            this.isAttackMoveByCore = true;
            break;
          case 21:
            this.isAlarm = true;
            this.chaseDistance = 99999f;
            break;
        }
      }
    }
    this.needFreshCorePosition = true;
  }

  private void attack2(string type)
  {
    this.state = TitanState.attack;
    this.attacked = false;
    this.isAlarm = true;
    if (this.attackAnimation == type)
    {
      this.attackAnimation = type;
      this.playAnimationAt("attack_" + type, 0.0f);
    }
    else
    {
      this.attackAnimation = type;
      this.playAnimationAt("attack_" + type, 0.0f);
    }
    this.nextAttackAnimation = (string) null;
    this.fxName = (string) null;
    this.isAttackMoveByCore = false;
    this.attackCheckTime = 0.0f;
    this.attackCheckTimeA = 0.0f;
    this.attackCheckTimeB = 0.0f;
    this.attackEndWait = 0.0f;
    this.fxRotation = Quaternion.Euler(270f, 0.0f, 0.0f);
    string key = type;
    if (key != null)
    {
      // ISSUE: reference to a compiler-generated field
      if (TITAN.fswitchmap6 == null)
      {
        // ISSUE: reference to a compiler-generated field
        TITAN.fswitchmap6 = new Dictionary<string, int>(22)
        {
          {
            "abnormal_getup",
            0
          },
          {
            "abnormal_jump",
            1
          },
          {
            "combo_1",
            2
          },
          {
            "combo_2",
            3
          },
          {
            "combo_3",
            4
          },
          {
            "front_ground",
            5
          },
          {
            "kick",
            6
          },
          {
            "slap_back",
            7
          },
          {
            "slap_face",
            8
          },
          {
            "stomp",
            9
          },
          {
            "bite",
            10
          },
          {
            "bite_l",
            11
          },
          {
            "bite_r",
            12
          },
          {
            "jumper_0",
            13
          },
          {
            "crawler_jump_0",
            14
          },
          {
            "anti_AE_l",
            15
          },
          {
            "anti_AE_r",
            16
          },
          {
            "anti_AE_low_l",
            17
          },
          {
            "anti_AE_low_r",
            18
          },
          {
            "quick_turn_l",
            19
          },
          {
            "quick_turn_r",
            20
          },
          {
            "throw",
            21
          }
        };
      }
      int num;
      // ISSUE: reference to a compiler-generated field
      if (TITAN.fswitchmap6.TryGetValue(key, out num))
      {
        switch (num)
        {
          case 0:
            this.attackCheckTime = 0.0f;
            this.fxName = string.Empty;
            break;
          case 1:
            this.nextAttackAnimation = "abnormal_getup";
            this.attackEndWait = !this.nonAI ? (this.myDifficulty <= 0 ? Random.Range(1f, 4f) : Random.Range(0.0f, 1f)) : 0.0f;
            this.attackCheckTime = 0.75f;
            this.fxName = "boom4";
            Quaternion rotation = this.baseTransform.rotation;
            this.fxRotation = Quaternion.Euler(270f, ((Quaternion) ref rotation).eulerAngles.y, 0.0f);
            break;
          case 2:
            this.nextAttackAnimation = "combo_2";
            this.attackCheckTimeA = 0.54f;
            this.attackCheckTimeB = 0.76f;
            this.nonAIcombo = false;
            this.isAttackMoveByCore = true;
            this.leftHandAttack = false;
            break;
          case 3:
            if (this.abnormalType != AbnormalType.TYPE_PUNK && !this.nonAI)
              this.nextAttackAnimation = "combo_3";
            this.attackCheckTimeA = 0.37f;
            this.attackCheckTimeB = 0.57f;
            this.nonAIcombo = false;
            this.isAttackMoveByCore = true;
            this.leftHandAttack = true;
            break;
          case 4:
            this.nonAIcombo = false;
            this.isAttackMoveByCore = true;
            this.attackCheckTime = 0.21f;
            this.fxName = "boom1";
            break;
          case 5:
            this.fxName = "boom1";
            this.attackCheckTime = 0.45f;
            break;
          case 6:
            this.fxName = "boom5";
            this.fxRotation = this.baseTransform.rotation;
            this.attackCheckTime = 0.43f;
            break;
          case 7:
            this.fxName = "boom3";
            this.attackCheckTime = 0.66f;
            break;
          case 8:
            this.fxName = "boom3";
            this.attackCheckTime = 0.655f;
            break;
          case 9:
            this.fxName = "boom2";
            this.attackCheckTime = 0.42f;
            break;
          case 10:
            this.fxName = "bite";
            this.attackCheckTime = 0.6f;
            break;
          case 11:
            this.fxName = "bite";
            this.attackCheckTime = 0.4f;
            break;
          case 12:
            this.fxName = "bite";
            this.attackCheckTime = 0.4f;
            break;
          case 13:
            this.abnorma_jump_bite_horizon_v = Vector3.zero;
            break;
          case 14:
            this.abnorma_jump_bite_horizon_v = Vector3.zero;
            break;
          case 15:
            this.attackCheckTimeA = 0.31f;
            this.attackCheckTimeB = 0.4f;
            this.leftHandAttack = true;
            break;
          case 16:
            this.attackCheckTimeA = 0.31f;
            this.attackCheckTimeB = 0.4f;
            this.leftHandAttack = false;
            break;
          case 17:
            this.attackCheckTimeA = 0.31f;
            this.attackCheckTimeB = 0.4f;
            this.leftHandAttack = true;
            break;
          case 18:
            this.attackCheckTimeA = 0.31f;
            this.attackCheckTimeB = 0.4f;
            this.leftHandAttack = false;
            break;
          case 19:
            this.attackCheckTimeA = 2f;
            this.attackCheckTimeB = 2f;
            this.isAttackMoveByCore = true;
            break;
          case 20:
            this.attackCheckTimeA = 2f;
            this.attackCheckTimeB = 2f;
            this.isAttackMoveByCore = true;
            break;
          case 21:
            this.isAlarm = true;
            this.chaseDistance = 99999f;
            break;
        }
      }
    }
    this.needFreshCorePosition = true;
  }

  private void Awake()
  {
    this.cache();
    this.baseRigidBody.freezeRotation = true;
    this.baseRigidBody.useGravity = false;
    this._customSkinLoader = ((Component) this).gameObject.AddComponent<TitanCustomSkinLoader>();
    this._ignoreLookTargetAnimations = new HashSet<string>()
    {
      "sit_hunt_down",
      "hit_eren_L",
      "hit_eren_R",
      "idle_recovery",
      "eat_l",
      "eat_r",
      "sit_hit_eye",
      "hit_eye"
    };
    this._fastHeadRotationAnimations = new HashSet<string>()
    {
      "hit_eren_L",
      "hit_eren_R",
      "sit_hit_eye",
      "hit_eye"
    };
    foreach (AnimationState animationState in ((Component) this).animation)
    {
      if (animationState.name.StartsWith("attack_"))
      {
        this._ignoreLookTargetAnimations.Add(animationState.name);
        this._fastHeadRotationAnimations.Add(animationState.name);
      }
    }
    this.HideTitanIfBomb();
  }

  private void CheckAnimationLookTarget(string animation)
  {
    this._ignoreLookTarget = this._ignoreLookTargetAnimations.Contains(animation);
    this._fastHeadRotation = this._fastHeadRotationAnimations.Contains(animation);
  }

  private IEnumerator HandleSpawnCollisionCoroutine(float time, float maxSpeed)
  {
    while ((double) time > 0.0)
    {
      Vector3 velocity1 = this.baseRigidBody.velocity;
      if ((double) ((Vector3) ref velocity1).magnitude > (double) maxSpeed)
      {
        Rigidbody baseRigidBody = this.baseRigidBody;
        Vector3 velocity2 = this.baseRigidBody.velocity;
        Vector3 vector3 = Vector3.op_Multiply(((Vector3) ref velocity2).normalized, maxSpeed);
        baseRigidBody.velocity = vector3;
      }
      time -= Time.fixedDeltaTime;
      yield return (object) new WaitForFixedUpdate();
    }
  }

  public void beLaughAttacked()
  {
    if (this.hasDie || this.abnormalType == AbnormalType.TYPE_CRAWLER)
      return;
    if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)
    {
      this.photonView.RPC("laugh", PhotonTargets.All, (object) 0.0f);
    }
    else
    {
      if (this.state != TitanState.idle && this.state != TitanState.turn && this.state != TitanState.chase)
        return;
      this.laugh();
    }
  }

  public void beTauntedBy(GameObject target, float tauntTime)
  {
    this.whoHasTauntMe = target;
    this.tauntTime = tauntTime;
    this.isAlarm = true;
  }

  public void cache()
  {
    this.baseAudioSource = ((Component) ((Component) this).transform.Find("snd_titan_foot")).GetComponent<AudioSource>();
    this.baseAnimation = ((Component) this).animation;
    this.baseTransform = ((Component) this).transform;
    this.baseRigidBody = ((Component) this).rigidbody;
    this.baseColliders = new List<Collider>();
    foreach (Collider componentsInChild in ((Component) this).GetComponentsInChildren<Collider>())
    {
      if (((Object) componentsInChild).name != "AABB")
        this.baseColliders.Add(componentsInChild);
    }
    GameObject gameObject1 = new GameObject();
    ((Object) gameObject1).name = "PlayerDetectorRC";
    GameObject gameObject2 = gameObject1;
    CapsuleCollider capsuleCollider = gameObject2.AddComponent<CapsuleCollider>();
    CapsuleCollider component = ((Component) this.baseTransform.Find("AABB")).GetComponent<CapsuleCollider>();
    capsuleCollider.center = component.center;
    capsuleCollider.radius = Math.Abs(this.baseTransform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck/head").position.y - this.baseTransform.position.y);
    capsuleCollider.height = component.height * 1.2f;
    ((Collider) capsuleCollider).material = ((Collider) component).material;
    ((Collider) capsuleCollider).isTrigger = true;
    ((Object) capsuleCollider).name = "PlayerDetectorRC";
    this.myTitanTrigger = gameObject2.AddComponent<TitanTrigger>();
    this.myTitanTrigger.isCollide = false;
    gameObject2.layer = PhysicsLayer.PlayerAttackBox;
    gameObject2.transform.parent = this.baseTransform.Find("AABB");
    gameObject2.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
    this.MultiplayerManager = FengGameManagerMKII.instance;
    if (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE && !this.photonView.isMine)
      return;
    this.baseGameObjectTransform = ((Component) this).gameObject.transform;
  }

  private void chase()
  {
    this.state = TitanState.chase;
    this.isAlarm = true;
    this.crossFade(this.runAnimation, 0.5f);
  }

  private GameObject checkIfHitCrawlerMouth(Transform head, float rad)
  {
    float num1 = rad * this.myLevel;
    foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("Player"))
    {
      if (Object.op_Equality((Object) gameObject.GetComponent<TITAN_EREN>(), (Object) null) && (Object.op_Equality((Object) gameObject.GetComponent<HERO>(), (Object) null) || !gameObject.GetComponent<HERO>().isInvincible()))
      {
        float num2 = gameObject.GetComponent<CapsuleCollider>().height * 0.5f;
        if ((double) Vector3.Distance(Vector3.op_Addition(gameObject.transform.position, Vector3.op_Multiply(Vector3.up, num2)), Vector3.op_Subtraction(head.position, Vector3.op_Multiply(Vector3.op_Multiply(Vector3.up, 1.5f), this.myLevel))) < (double) num1 + (double) num2)
          return gameObject;
      }
    }
    return (GameObject) null;
  }

  private GameObject checkIfHitHand(Transform hand)
  {
    float num = 2.4f * this.myLevel;
    foreach (Collider collider in Physics.OverlapSphere(((Component) ((Component) hand).GetComponent<SphereCollider>()).transform.position, num + 1f))
    {
      if (((Component) ((Component) collider).transform.root).tag == "Player")
      {
        GameObject gameObject = ((Component) ((Component) collider).transform.root).gameObject;
        if (Object.op_Inequality((Object) gameObject.GetComponent<TITAN_EREN>(), (Object) null))
        {
          if (!gameObject.GetComponent<TITAN_EREN>().isHit)
            gameObject.GetComponent<TITAN_EREN>().hitByTitan();
        }
        else if (Object.op_Inequality((Object) gameObject.GetComponent<HERO>(), (Object) null) && !gameObject.GetComponent<HERO>().isInvincible())
          return gameObject;
      }
    }
    return (GameObject) null;
  }

  private GameObject checkIfHitHead(Transform head, float rad)
  {
    float num1 = rad * this.myLevel;
    foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("Player"))
    {
      if (Object.op_Equality((Object) gameObject.GetComponent<TITAN_EREN>(), (Object) null) && (Object.op_Equality((Object) gameObject.GetComponent<HERO>(), (Object) null) || !gameObject.GetComponent<HERO>().isInvincible()))
      {
        float num2 = gameObject.GetComponent<CapsuleCollider>().height * 0.5f;
        if ((double) Vector3.Distance(Vector3.op_Addition(gameObject.transform.position, Vector3.op_Multiply(Vector3.up, num2)), Vector3.op_Addition(head.position, Vector3.op_Multiply(Vector3.op_Multiply(Vector3.up, 1.5f), this.myLevel))) < (double) num1 + (double) num2)
          return gameObject;
      }
    }
    return (GameObject) null;
  }

  private void crossFadeIfNotPlaying(string aniName, float time)
  {
    if (((Component) this).animation.IsPlaying(aniName) && IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE && !PhotonNetwork.offlineMode)
      return;
    this.crossFade(aniName, time);
  }

  private void crossFade(string aniName, float time)
  {
    ((Component) this).animation.CrossFade(aniName, time);
    this.CheckAnimationLookTarget(aniName);
    if (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.MULTIPLAYER || !this.photonView.isMine)
      return;
    this.photonView.RPC("netCrossFade", PhotonTargets.Others, (object) aniName, (object) time);
  }

  public bool die()
  {
    if (this.hasDie)
      return false;
    this.hasDie = true;
    GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().oneTitanDown(string.Empty, false);
    this.dieAnimation();
    return true;
  }

  private void dieAnimation()
  {
    if (((Component) this).animation.IsPlaying("sit_idle") || ((Component) this).animation.IsPlaying("sit_hit_eye"))
      this.crossFade("sit_die", 0.1f);
    else if (this.abnormalType == AbnormalType.TYPE_CRAWLER)
      this.crossFade("crawler_die", 0.2f);
    else if (this.abnormalType == AbnormalType.NORMAL)
      this.crossFade("die_front", 0.05f);
    else if (((Component) this).animation.IsPlaying("attack_abnormal_jump") && (double) ((Component) this).animation["attack_abnormal_jump"].normalizedTime > 0.699999988079071 || ((Component) this).animation.IsPlaying("attack_abnormal_getup") && (double) ((Component) this).animation["attack_abnormal_getup"].normalizedTime < 0.699999988079071 || ((Component) this).animation.IsPlaying("tired"))
      this.crossFade("die_ground", 0.2f);
    else
      this.crossFade("die_back", 0.05f);
  }

  public void dieBlow(Vector3 attacker, float hitPauseTime)
  {
    if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
    {
      this.dieBlowFunc(attacker, hitPauseTime);
      if (GameObject.FindGameObjectsWithTag("titan").Length > 1)
        return;
      GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = true;
    }
    else
      this.photonView.RPC("dieBlowRPC", PhotonTargets.All, (object) attacker, (object) hitPauseTime);
  }

  public void dieBlowFunc(Vector3 attacker, float hitPauseTime)
  {
    if (this.hasDie)
      return;
    Transform transform = ((Component) this).transform;
    Quaternion quaternion1 = Quaternion.LookRotation(Vector3.op_Subtraction(attacker, ((Component) this).transform.position));
    Quaternion quaternion2 = Quaternion.Euler(0.0f, ((Quaternion) ref quaternion1).eulerAngles.y, 0.0f);
    transform.rotation = quaternion2;
    this.hasDie = true;
    this.hitAnimation = "die_blow";
    this.hitPause = hitPauseTime;
    this.playAnimation(this.hitAnimation);
    ((Component) this).animation[this.hitAnimation].time = 0.0f;
    ((Component) this).animation[this.hitAnimation].speed = 0.0f;
    this.needFreshCorePosition = true;
    GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().oneTitanDown(string.Empty, false);
    if (!this.photonView.isMine)
      return;
    if (Object.op_Inequality((Object) this.grabbedTarget, (Object) null))
      this.grabbedTarget.GetPhotonView().RPC("netUngrabbed", PhotonTargets.All);
    if (!this.nonAI)
      return;
    this.currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().setMainObject((GameObject) null);
    this.currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().setSpectorMode(true);
    this.currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = true;
    Hashtable propertiesToSet1 = new Hashtable();
    ((Dictionary<object, object>) propertiesToSet1).Add((object) PhotonPlayerProperty.dead, (object) true);
    PhotonNetwork.player.SetCustomProperties(propertiesToSet1);
    Hashtable propertiesToSet2 = new Hashtable();
    ((Dictionary<object, object>) propertiesToSet2).Add((object) PhotonPlayerProperty.deaths, (object) ((int) PhotonNetwork.player.customProperties[(object) PhotonPlayerProperty.deaths] + 1));
    PhotonNetwork.player.SetCustomProperties(propertiesToSet2);
  }

  [RPC]
  private void dieBlowRPC(Vector3 attacker, float hitPauseTime)
  {
    if (!this.photonView.isMine)
      return;
    Vector3 vector3 = Vector3.op_Subtraction(attacker, ((Component) this).transform.position);
    if ((double) ((Vector3) ref vector3).magnitude >= 80.0)
      return;
    this.dieBlowFunc(attacker, hitPauseTime);
  }

  [RPC]
  public void DieByCannon(int viewID)
  {
    PhotonView view = PhotonView.Find(viewID);
    if (Object.op_Inequality((Object) view, (Object) null))
    {
      int Damage = 0;
      if (PhotonNetwork.isMasterClient)
        this.OnTitanDie(view);
      if (this.nonAI)
        FengGameManagerMKII.instance.titanGetKill(view.owner, Damage, (string) PhotonNetwork.player.customProperties[(object) PhotonPlayerProperty.name]);
      else
        FengGameManagerMKII.instance.titanGetKill(view.owner, Damage, ((Object) this).name);
    }
    else
      FengGameManagerMKII.instance.photonView.RPC("netShowDamage", view.owner, (object) this.speed);
  }

  public void dieHeadBlow(Vector3 attacker, float hitPauseTime)
  {
    if (this.abnormalType == AbnormalType.TYPE_CRAWLER)
      return;
    if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
    {
      this.dieHeadBlowFunc(attacker, hitPauseTime);
      if (GameObject.FindGameObjectsWithTag("titan").Length > 1)
        return;
      GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = true;
    }
    else
      this.photonView.RPC("dieHeadBlowRPC", PhotonTargets.All, (object) attacker, (object) hitPauseTime);
  }

  public void dieHeadBlowFunc(Vector3 attacker, float hitPauseTime)
  {
    if (this.hasDie)
      return;
    this.playSound("snd_titan_head_blow");
    Transform transform = ((Component) this).transform;
    Quaternion quaternion1 = Quaternion.LookRotation(Vector3.op_Subtraction(attacker, ((Component) this).transform.position));
    Quaternion quaternion2 = Quaternion.Euler(0.0f, ((Quaternion) ref quaternion1).eulerAngles.y, 0.0f);
    transform.rotation = quaternion2;
    this.hasDie = true;
    this.hitAnimation = "die_headOff";
    this.hitPause = hitPauseTime;
    this.playAnimation(this.hitAnimation);
    ((Component) this).animation[this.hitAnimation].time = 0.0f;
    ((Component) this).animation[this.hitAnimation].speed = 0.0f;
    GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().oneTitanDown(string.Empty, false);
    this.needFreshCorePosition = true;
    (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.MULTIPLAYER || !this.photonView.isMine ? (GameObject) Object.Instantiate(Resources.Load("bloodExplore"), Vector3.op_Addition(this.head.position, Vector3.op_Multiply(Vector3.op_Multiply(Vector3.up, 1f), this.myLevel)), Quaternion.Euler(270f, 0.0f, 0.0f)) : PhotonNetwork.Instantiate("bloodExplore", Vector3.op_Addition(this.head.position, Vector3.op_Multiply(Vector3.op_Multiply(Vector3.up, 1f), this.myLevel)), Quaternion.Euler(270f, 0.0f, 0.0f), 0)).transform.localScale = ((Component) this).transform.localScale;
    GameObject gameObject;
    if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER && this.photonView.isMine)
    {
      Vector3 position = this.head.position;
      Quaternion rotation1 = this.neck.rotation;
      double num = 270.0 + (double) ((Quaternion) ref rotation1).eulerAngles.x;
      Quaternion rotation2 = this.neck.rotation;
      double y = (double) ((Quaternion) ref rotation2).eulerAngles.y;
      Quaternion rotation3 = this.neck.rotation;
      double z = (double) ((Quaternion) ref rotation3).eulerAngles.z;
      Quaternion rotation4 = Quaternion.Euler((float) num, (float) y, (float) z);
      gameObject = PhotonNetwork.Instantiate("bloodsplatter", position, rotation4, 0);
    }
    else
    {
      Object @object = Resources.Load("bloodsplatter");
      Vector3 position = this.head.position;
      Quaternion rotation5 = this.neck.rotation;
      double num = 270.0 + (double) ((Quaternion) ref rotation5).eulerAngles.x;
      Quaternion rotation6 = this.neck.rotation;
      double y = (double) ((Quaternion) ref rotation6).eulerAngles.y;
      Quaternion rotation7 = this.neck.rotation;
      double z = (double) ((Quaternion) ref rotation7).eulerAngles.z;
      Quaternion quaternion3 = Quaternion.Euler((float) num, (float) y, (float) z);
      gameObject = (GameObject) Object.Instantiate(@object, position, quaternion3);
    }
    gameObject.transform.localScale = ((Component) this).transform.localScale;
    gameObject.transform.parent = this.neck;
    (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.MULTIPLAYER || !this.photonView.isMine ? (GameObject) Object.Instantiate(Resources.Load("FX/justSmoke"), this.neck.position, Quaternion.Euler(270f, 0.0f, 0.0f)) : PhotonNetwork.Instantiate("FX/justSmoke", this.neck.position, Quaternion.Euler(270f, 0.0f, 0.0f), 0)).transform.parent = this.neck;
    if (!this.photonView.isMine)
      return;
    if (Object.op_Inequality((Object) this.grabbedTarget, (Object) null))
      this.grabbedTarget.GetPhotonView().RPC("netUngrabbed", PhotonTargets.All);
    if (!this.nonAI)
      return;
    this.currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().setMainObject((GameObject) null);
    this.currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().setSpectorMode(true);
    this.currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = true;
    Hashtable propertiesToSet1 = new Hashtable();
    ((Dictionary<object, object>) propertiesToSet1).Add((object) PhotonPlayerProperty.dead, (object) true);
    PhotonNetwork.player.SetCustomProperties(propertiesToSet1);
    Hashtable propertiesToSet2 = new Hashtable();
    ((Dictionary<object, object>) propertiesToSet2).Add((object) PhotonPlayerProperty.deaths, (object) ((int) PhotonNetwork.player.customProperties[(object) PhotonPlayerProperty.deaths] + 1));
    PhotonNetwork.player.SetCustomProperties(propertiesToSet2);
  }

  [RPC]
  private void dieHeadBlowRPC(Vector3 attacker, float hitPauseTime)
  {
    if (!this.photonView.isMine)
      return;
    Vector3 vector3 = Vector3.op_Subtraction(attacker, this.neck.position);
    if ((double) ((Vector3) ref vector3).magnitude >= (double) this.lagMax)
      return;
    this.dieHeadBlowFunc(attacker, hitPauseTime);
  }

  private void eat()
  {
    this.state = TitanState.eat;
    this.attacked = false;
    if (this.isGrabHandLeft)
    {
      this.attackAnimation = "eat_l";
      this.crossFade("eat_l", 0.1f);
    }
    else
    {
      this.attackAnimation = "eat_r";
      this.crossFade("eat_r", 0.1f);
    }
  }

  private void eatSet(GameObject grabTarget)
  {
    switch (IN_GAME_MAIN_CAMERA.gametype)
    {
      case GAMETYPE.SINGLE:
        if (grabTarget.GetComponent<HERO>().isGrabbed)
          return;
        break;
      case GAMETYPE.MULTIPLAYER:
        if (!this.photonView.isMine)
          break;
        goto case GAMETYPE.SINGLE;
    }
    this.grabToRight((PhotonMessageInfo) null);
    if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER && this.photonView.isMine)
    {
      this.photonView.RPC("grabToRight", PhotonTargets.Others);
      object[] objArray1 = new object[1]
      {
        (object) "grabbed"
      };
      grabTarget.GetPhotonView().RPC("netPlayAnimation", PhotonTargets.All, objArray1);
      object[] objArray2 = new object[2]
      {
        (object) this.photonView.viewID,
        (object) false
      };
      grabTarget.GetPhotonView().RPC("netGrabbed", PhotonTargets.All, objArray2);
    }
    else
    {
      grabTarget.GetComponent<HERO>().grabbed(((Component) this).gameObject, false);
      ((Component) grabTarget.GetComponent<HERO>()).animation.Play("grabbed");
    }
  }

  private void eatSetL(GameObject grabTarget)
  {
    switch (IN_GAME_MAIN_CAMERA.gametype)
    {
      case GAMETYPE.SINGLE:
        if (grabTarget.GetComponent<HERO>().isGrabbed)
          return;
        break;
      case GAMETYPE.MULTIPLAYER:
        if (!this.photonView.isMine)
          break;
        goto case GAMETYPE.SINGLE;
    }
    this.grabToLeft((PhotonMessageInfo) null);
    if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER && this.photonView.isMine)
    {
      this.photonView.RPC("grabToLeft", PhotonTargets.Others);
      object[] objArray1 = new object[1]
      {
        (object) "grabbed"
      };
      grabTarget.GetPhotonView().RPC("netPlayAnimation", PhotonTargets.All, objArray1);
      object[] objArray2 = new object[2]
      {
        (object) this.photonView.viewID,
        (object) true
      };
      grabTarget.GetPhotonView().RPC("netGrabbed", PhotonTargets.All, objArray2);
    }
    else
    {
      grabTarget.GetComponent<HERO>().grabbed(((Component) this).gameObject, true);
      ((Component) grabTarget.GetComponent<HERO>()).animation.Play("grabbed");
    }
  }

  private bool executeAttack(string decidedAction)
  {
    string key = decidedAction;
    if (key != null)
    {
      // ISSUE: reference to a compiler-generated field
      if (TITAN.fswitchSmap5 == null)
      {
        // ISSUE: reference to a compiler-generated field
        TITAN.fswitchSmap5 = new Dictionary<string, int>(18)
        {
          {
            "grab_ground_front_l",
            0
          },
          {
            "grab_ground_front_r",
            1
          },
          {
            "grab_ground_back_l",
            2
          },
          {
            "grab_ground_back_r",
            3
          },
          {
            "grab_head_front_l",
            4
          },
          {
            "grab_head_front_r",
            5
          },
          {
            "grab_head_back_l",
            6
          },
          {
            "grab_head_back_r",
            7
          },
          {
            "attack_abnormal_jump",
            8
          },
          {
            "attack_combo",
            9
          },
          {
            "attack_front_ground",
            10
          },
          {
            "attack_kick",
            11
          },
          {
            "attack_slap_back",
            12
          },
          {
            "attack_slap_face",
            13
          },
          {
            "attack_stomp",
            14
          },
          {
            "attack_bite",
            15
          },
          {
            "attack_bite_l",
            16
          },
          {
            "attack_bite_r",
            17
          }
        };
      }
      int num;
      // ISSUE: reference to a compiler-generated field
      if (TITAN.fswitchSmap5.TryGetValue(key, out num))
      {
        switch (num)
        {
          case 0:
            this.grab("ground_front_l");
            return true;
          case 1:
            this.grab("ground_front_r");
            return true;
          case 2:
            this.grab("ground_back_l");
            return true;
          case 3:
            this.grab("ground_back_r");
            return true;
          case 4:
            this.grab("head_front_l");
            return true;
          case 5:
            this.grab("head_front_r");
            return true;
          case 6:
            this.grab("head_back_l");
            return true;
          case 7:
            this.grab("head_back_r");
            return true;
          case 8:
            this.attack("abnormal_jump");
            return true;
          case 9:
            this.attack("combo_1");
            return true;
          case 10:
            this.attack("front_ground");
            return true;
          case 11:
            this.attack("kick");
            return true;
          case 12:
            this.attack("slap_back");
            return true;
          case 13:
            this.attack("slap_face");
            return true;
          case 14:
            this.attack("stomp");
            return true;
          case 15:
            this.attack("bite");
            return true;
          case 16:
            this.attack("bite_l");
            return true;
          case 17:
            this.attack("bite_r");
            return true;
        }
      }
    }
    return false;
  }

  private bool executeAttack2(string decidedAction)
  {
    string key = decidedAction;
    if (key != null)
    {
      // ISSUE: reference to a compiler-generated field
      if (TITAN.fswitchmap5 == null)
      {
        // ISSUE: reference to a compiler-generated field
        TITAN.fswitchmap5 = new Dictionary<string, int>(18)
        {
          {
            "grab_ground_front_l",
            0
          },
          {
            "grab_ground_front_r",
            1
          },
          {
            "grab_ground_back_l",
            2
          },
          {
            "grab_ground_back_r",
            3
          },
          {
            "grab_head_front_l",
            4
          },
          {
            "grab_head_front_r",
            5
          },
          {
            "grab_head_back_l",
            6
          },
          {
            "grab_head_back_r",
            7
          },
          {
            "attack_abnormal_jump",
            8
          },
          {
            "attack_combo",
            9
          },
          {
            "attack_front_ground",
            10
          },
          {
            "attack_kick",
            11
          },
          {
            "attack_slap_back",
            12
          },
          {
            "attack_slap_face",
            13
          },
          {
            "attack_stomp",
            14
          },
          {
            "attack_bite",
            15
          },
          {
            "attack_bite_l",
            16
          },
          {
            "attack_bite_r",
            17
          }
        };
      }
      int num;
      // ISSUE: reference to a compiler-generated field
      if (TITAN.fswitchmap5.TryGetValue(key, out num))
      {
        switch (num)
        {
          case 0:
            this.grab("ground_front_l");
            return true;
          case 1:
            this.grab("ground_front_r");
            return true;
          case 2:
            this.grab("ground_back_l");
            return true;
          case 3:
            this.grab("ground_back_r");
            return true;
          case 4:
            this.grab("head_front_l");
            return true;
          case 5:
            this.grab("head_front_r");
            return true;
          case 6:
            this.grab("head_back_l");
            return true;
          case 7:
            this.grab("head_back_r");
            return true;
          case 8:
            this.attack2("abnormal_jump");
            return true;
          case 9:
            this.attack2("combo_1");
            return true;
          case 10:
            this.attack2("front_ground");
            return true;
          case 11:
            this.attack2("kick");
            return true;
          case 12:
            this.attack2("slap_back");
            return true;
          case 13:
            this.attack2("slap_face");
            return true;
          case 14:
            this.attack2("stomp");
            return true;
          case 15:
            this.attack2("bite");
            return true;
          case 16:
            this.attack2("bite_l");
            return true;
          case 17:
            this.attack2("bite_r");
            return true;
        }
      }
    }
    return false;
  }

  public void explode()
  {
    if (!SettingsManager.LegacyGameSettings.TitanExplodeEnabled.Value || !this.hasDie || (double) this.dieTime < 1.0 || this.hasExplode)
      return;
    int num1 = 0;
    float num2 = this.myLevel * 10f;
    if (this.abnormalType == AbnormalType.TYPE_CRAWLER)
    {
      if ((double) this.dieTime >= 2.0)
      {
        this.hasExplode = true;
        num2 = 0.0f;
        num1 = 1;
      }
    }
    else
    {
      num1 = 1;
      this.hasExplode = true;
    }
    if (num1 != 1)
      return;
    Vector3 position = Vector3.op_Addition(this.baseTransform.position, Vector3.op_Multiply(Vector3.up, num2));
    PhotonNetwork.Instantiate("FX/Thunder", position, Quaternion.Euler(270f, 0.0f, 0.0f), 0);
    PhotonNetwork.Instantiate("FX/boom1", position, Quaternion.Euler(270f, 0.0f, 0.0f), 0);
    foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("Player"))
    {
      if ((double) Vector3.Distance(gameObject.transform.position, position) < (double) SettingsManager.LegacyGameSettings.TitanExplodeRadius.Value)
      {
        gameObject.GetComponent<HERO>().markDie();
        gameObject.GetComponent<HERO>().photonView.RPC("netDie2", PhotonTargets.All, (object) -1, (object) "Server ");
      }
    }
  }

  private void findNearestFacingHero2()
  {
    GameObject gameObject = (GameObject) null;
    float num1 = float.PositiveInfinity;
    Vector3 position = this.baseTransform.position;
    float num2 = this.abnormalType != AbnormalType.NORMAL ? 180f : 100f;
    foreach (HERO player in this.MultiplayerManager.getPlayers())
    {
      float num3 = Vector3.Distance(((Component) player).transform.position, position);
      if ((double) num3 < (double) num1)
      {
        Vector3 vector3 = Vector3.op_Subtraction(((Component) player).transform.position, this.baseTransform.position);
        double num4 = -(double) Mathf.Atan2(vector3.z, vector3.x) * 57.295780181884766;
        Quaternion rotation = this.baseGameObjectTransform.rotation;
        double num5 = (double) ((Quaternion) ref rotation).eulerAngles.y - 90.0;
        if ((double) Mathf.Abs(-Mathf.DeltaAngle((float) num4, (float) num5)) < (double) num2)
        {
          gameObject = ((Component) player).gameObject;
          num1 = num3;
        }
      }
    }
    foreach (TITAN_EREN eren in this.MultiplayerManager.getErens())
    {
      float num6 = Vector3.Distance(((Component) eren).transform.position, position);
      if ((double) num6 < (double) num1)
      {
        Vector3 vector3 = Vector3.op_Subtraction(((Component) eren).transform.position, this.baseTransform.position);
        double num7 = -(double) Mathf.Atan2(vector3.z, vector3.x) * 57.295780181884766;
        Quaternion rotation = this.baseGameObjectTransform.rotation;
        double num8 = (double) ((Quaternion) ref rotation).eulerAngles.y - 90.0;
        if ((double) Mathf.Abs(-Mathf.DeltaAngle((float) num7, (float) num8)) < (double) num2)
        {
          gameObject = ((Component) eren).gameObject;
          num1 = num6;
        }
      }
    }
    if (!Object.op_Inequality((Object) gameObject, (Object) null))
      return;
    GameObject hero1 = this.myHero;
    this.myHero = gameObject;
    GameObject hero2 = this.myHero;
    if (Object.op_Inequality((Object) hero1, (Object) hero2) && IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER && PhotonNetwork.isMasterClient)
    {
      if (Object.op_Equality((Object) this.myHero, (Object) null))
        this.photonView.RPC("setMyTarget", PhotonTargets.Others, (object) -1);
      else
        this.photonView.RPC("setMyTarget", PhotonTargets.Others, (object) this.myHero.GetPhotonView().viewID);
    }
    this.tauntTime = 5f;
  }

  private void findNearestHero2()
  {
    GameObject hero = this.myHero;
    this.myHero = this.getNearestHero2();
    if (Object.op_Inequality((Object) this.myHero, (Object) hero) && IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER && PhotonNetwork.isMasterClient)
    {
      if (Object.op_Equality((Object) this.myHero, (Object) null))
        this.photonView.RPC("setMyTarget", PhotonTargets.Others, (object) -1);
      else
        this.photonView.RPC("setMyTarget", PhotonTargets.Others, (object) this.myHero.GetPhotonView().viewID);
    }
    this.oldHeadRotation = this.head.rotation;
  }

  private void FixedUpdate()
  {
    if (GameMenu.Paused && IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE || IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE && !this.photonView.isMine)
      return;
    this.baseRigidBody.AddForce(new Vector3(0.0f, -this.gravity * this.baseRigidBody.mass, 0.0f));
    if (this.needFreshCorePosition)
    {
      this.oldCorePosition = Vector3.op_Subtraction(this.baseTransform.position, this.baseTransform.Find("Amarture/Core").position);
      this.needFreshCorePosition = false;
    }
    if (this.hasDie)
    {
      if ((double) this.hitPause <= 0.0 && this.baseAnimation.IsPlaying("die_headOff"))
        this.baseRigidBody.velocity = Vector3.op_Addition(Vector3.op_Division(Vector3.op_Subtraction(Vector3.op_Subtraction(this.baseTransform.position, this.baseTransform.Find("Amarture/Core").position), this.oldCorePosition), Time.deltaTime), Vector3.op_Multiply(Vector3.up, this.baseRigidBody.velocity.y));
      this.oldCorePosition = Vector3.op_Subtraction(this.baseTransform.position, this.baseTransform.Find("Amarture/Core").position);
    }
    else if (this.state == TitanState.attack && this.isAttackMoveByCore || this.state == TitanState.hit)
    {
      this.baseRigidBody.velocity = Vector3.op_Addition(Vector3.op_Division(Vector3.op_Subtraction(Vector3.op_Subtraction(this.baseTransform.position, this.baseTransform.Find("Amarture/Core").position), this.oldCorePosition), Time.deltaTime), Vector3.op_Multiply(Vector3.up, this.baseRigidBody.velocity.y));
      this.oldCorePosition = Vector3.op_Subtraction(this.baseTransform.position, this.baseTransform.Find("Amarture/Core").position);
    }
    if (this.hasDie)
    {
      if ((double) this.hitPause > 0.0)
      {
        this.hitPause -= Time.deltaTime;
        if ((double) this.hitPause > 0.0)
          return;
        this.baseAnimation[this.hitAnimation].speed = 1f;
        this.hitPause = 0.0f;
      }
      else
      {
        if (!this.baseAnimation.IsPlaying("die_blow"))
          return;
        if ((double) this.baseAnimation["die_blow"].normalizedTime < 0.550000011920929)
          this.baseRigidBody.velocity = Vector3.op_Addition(Vector3.op_Multiply(Vector3.op_UnaryNegation(this.baseTransform.forward), 300f), Vector3.op_Multiply(Vector3.up, this.baseRigidBody.velocity.y));
        else if ((double) this.baseAnimation["die_blow"].normalizedTime < 0.82999998331069946)
          this.baseRigidBody.velocity = Vector3.op_Addition(Vector3.op_Multiply(Vector3.op_UnaryNegation(this.baseTransform.forward), 100f), Vector3.op_Multiply(Vector3.up, this.baseRigidBody.velocity.y));
        else
          this.baseRigidBody.velocity = Vector3.op_Multiply(Vector3.up, this.baseRigidBody.velocity.y);
      }
    }
    else
    {
      if (this.nonAI && !GameMenu.Paused && (this.state == TitanState.idle || this.state == TitanState.attack && this.attackAnimation == "jumper_1"))
      {
        Vector3 vector3_1 = Vector3.zero;
        if ((double) this.controller.targetDirection != -874.0)
        {
          bool flag = false;
          if ((double) this.stamina < 5.0)
            flag = true;
          else if ((double) this.stamina < 40.0 && !this.baseAnimation.IsPlaying("run_abnormal") && !this.baseAnimation.IsPlaying("crawler_run"))
            flag = true;
          vector3_1 = !(this.controller.isWALKDown | flag) ? Vector3.op_Multiply(Vector3.op_Multiply(this.baseTransform.forward, this.speed), Mathf.Sqrt(this.myLevel)) : Vector3.op_Multiply(Vector3.op_Multiply(Vector3.op_Multiply(this.baseTransform.forward, this.speed), Mathf.Sqrt(this.myLevel)), 0.2f);
          this.baseGameObjectTransform.rotation = Quaternion.Lerp(this.baseGameObjectTransform.rotation, Quaternion.Euler(0.0f, this.controller.targetDirection, 0.0f), this.speed * 0.15f * Time.deltaTime);
          if (this.state == TitanState.idle)
          {
            if (this.controller.isWALKDown | flag)
            {
              if (this.abnormalType == AbnormalType.TYPE_CRAWLER)
              {
                if (!this.baseAnimation.IsPlaying("crawler_run"))
                  this.crossFade("crawler_run", 0.1f);
              }
              else if (!this.baseAnimation.IsPlaying("run_walk"))
                this.crossFade("run_walk", 0.1f);
            }
            else if (this.abnormalType == AbnormalType.TYPE_CRAWLER)
            {
              if (!this.baseAnimation.IsPlaying("crawler_run"))
                this.crossFade("crawler_run", 0.1f);
              GameObject gameObject = this.checkIfHitCrawlerMouth(this.head, 2.2f);
              if (Object.op_Inequality((Object) gameObject, (Object) null))
              {
                Vector3 position = this.baseTransform.Find("Amarture/Core/Controller_Body/hip/spine/chest").position;
                switch (IN_GAME_MAIN_CAMERA.gametype)
                {
                  case GAMETYPE.SINGLE:
                    gameObject.GetComponent<HERO>().die(Vector3.op_Multiply(Vector3.op_Multiply(Vector3.op_Subtraction(gameObject.transform.position, position), 15f), this.myLevel), false);
                    break;
                  case GAMETYPE.MULTIPLAYER:
                    if (this.photonView.isMine && !gameObject.GetComponent<HERO>().HasDied())
                    {
                      gameObject.GetComponent<HERO>().markDie();
                      object[] objArray = new object[5]
                      {
                        (object) Vector3.op_Multiply(Vector3.op_Multiply(Vector3.op_Subtraction(gameObject.transform.position, position), 15f), this.myLevel),
                        (object) true,
                        (object) (!this.nonAI ? -1 : this.photonView.viewID),
                        (object) ((Object) this).name,
                        (object) true
                      };
                      gameObject.GetComponent<HERO>().photonView.RPC("netDie", PhotonTargets.All, objArray);
                      break;
                    }
                    break;
                }
              }
            }
            else if (!this.baseAnimation.IsPlaying("run_abnormal"))
              this.crossFade("run_abnormal", 0.1f);
          }
        }
        else if (this.state == TitanState.idle)
        {
          if (this.abnormalType == AbnormalType.TYPE_CRAWLER)
          {
            if (!this.baseAnimation.IsPlaying("crawler_idle"))
              this.crossFade("crawler_idle", 0.1f);
          }
          else if (!this.baseAnimation.IsPlaying("idle"))
            this.crossFade("idle", 0.1f);
          vector3_1 = Vector3.zero;
        }
        if (this.state == TitanState.idle)
        {
          Vector3 velocity = this.baseRigidBody.velocity;
          Vector3 vector3_2 = Vector3.op_Subtraction(vector3_1, velocity);
          vector3_2.x = Mathf.Clamp(vector3_2.x, -this.maxVelocityChange, this.maxVelocityChange);
          vector3_2.z = Mathf.Clamp(vector3_2.z, -this.maxVelocityChange, this.maxVelocityChange);
          vector3_2.y = 0.0f;
          this.baseRigidBody.AddForce(vector3_2, (ForceMode) 2);
        }
        else if (this.state == TitanState.attack && this.attackAnimation == "jumper_0")
        {
          Vector3 velocity = this.baseRigidBody.velocity;
          Vector3 vector3_3 = Vector3.op_Subtraction(Vector3.op_Multiply(vector3_1, 0.8f), velocity);
          vector3_3.x = Mathf.Clamp(vector3_3.x, -this.maxVelocityChange, this.maxVelocityChange);
          vector3_3.z = Mathf.Clamp(vector3_3.z, -this.maxVelocityChange, this.maxVelocityChange);
          vector3_3.y = 0.0f;
          this.baseRigidBody.AddForce(vector3_3, (ForceMode) 2);
        }
      }
      if ((this.abnormalType == AbnormalType.TYPE_I || this.abnormalType == AbnormalType.TYPE_JUMPER) && !this.nonAI && this.state == TitanState.attack && this.attackAnimation == "jumper_0")
      {
        Vector3 vector3_4 = Vector3.op_Multiply(Vector3.op_Multiply(Vector3.op_Multiply(this.baseTransform.forward, this.speed), this.myLevel), 0.5f);
        Vector3 velocity = this.baseRigidBody.velocity;
        if ((double) this.baseAnimation["attack_jumper_0"].normalizedTime <= 0.2800000011920929 || (double) this.baseAnimation["attack_jumper_0"].normalizedTime >= 0.800000011920929)
          vector3_4 = Vector3.zero;
        Vector3 vector3_5 = Vector3.op_Subtraction(vector3_4, velocity);
        vector3_5.x = Mathf.Clamp(vector3_5.x, -this.maxVelocityChange, this.maxVelocityChange);
        vector3_5.z = Mathf.Clamp(vector3_5.z, -this.maxVelocityChange, this.maxVelocityChange);
        vector3_5.y = 0.0f;
        this.baseRigidBody.AddForce(vector3_5, (ForceMode) 2);
      }
      if (this.state != TitanState.chase && this.state != TitanState.wander && this.state != TitanState.to_check_point && this.state != TitanState.to_pvp_pt && this.state != TitanState.random_run)
        return;
      Vector3 vector3_6 = Vector3.op_Subtraction(Vector3.op_Multiply(this.baseTransform.forward, this.speed), this.baseRigidBody.velocity);
      vector3_6.x = Mathf.Clamp(vector3_6.x, -this.maxVelocityChange, this.maxVelocityChange);
      vector3_6.z = Mathf.Clamp(vector3_6.z, -this.maxVelocityChange, this.maxVelocityChange);
      vector3_6.y = 0.0f;
      this.baseRigidBody.AddForce(vector3_6, (ForceMode) 2);
      if (!this.stuck && this.abnormalType != AbnormalType.TYPE_CRAWLER && !this.nonAI)
      {
        if (this.baseAnimation.IsPlaying(this.runAnimation))
        {
          Vector3 velocity = this.baseRigidBody.velocity;
          if ((double) ((Vector3) ref velocity).magnitude < (double) this.speed * 0.5)
          {
            this.stuck = true;
            this.stuckTime = 2f;
            this.stuckTurnAngle = (float) ((double) Random.Range(0, 2) * 140.0 - 70.0);
          }
        }
        if (this.state == TitanState.chase && Object.op_Inequality((Object) this.myHero, (Object) null) && (double) this.myDistance > (double) this.attackDistance && (double) this.myDistance < 150.0)
        {
          float num1 = 0.05f;
          if (this.myDifficulty > 1)
            num1 += 0.05f;
          if (this.abnormalType != AbnormalType.NORMAL)
            num1 += 0.1f;
          if ((double) Random.Range(0.0f, 1f) < (double) num1)
          {
            this.stuck = true;
            this.stuckTime = 1f;
            float num2 = Random.Range(20f, 50f);
            this.stuckTurnAngle = (float) ((double) Random.Range(0, 2) * (double) num2 * 2.0) - num2;
          }
        }
      }
      float num3;
      if (this.state == TitanState.wander)
      {
        Quaternion rotation = this.baseTransform.rotation;
        num3 = ((Quaternion) ref rotation).eulerAngles.y - 90f;
      }
      else if (this.state == TitanState.to_check_point || this.state == TitanState.to_pvp_pt || this.state == TitanState.random_run)
      {
        Vector3 vector3_7 = Vector3.op_Subtraction(this.targetCheckPt, this.baseTransform.position);
        num3 = (float) (-(double) Mathf.Atan2(vector3_7.z, vector3_7.x) * 57.295780181884766);
      }
      else
      {
        if (Object.op_Equality((Object) this.myHero, (Object) null))
          return;
        Vector3 vector3_8 = Vector3.op_Subtraction(this.myHero.transform.position, this.baseTransform.position);
        num3 = (float) (-(double) Mathf.Atan2(vector3_8.z, vector3_8.x) * 57.295780181884766);
      }
      if (this.stuck)
      {
        this.stuckTime -= Time.deltaTime;
        if ((double) this.stuckTime < 0.0)
          this.stuck = false;
        if ((double) this.stuckTurnAngle > 0.0)
          this.stuckTurnAngle -= Time.deltaTime * 10f;
        else
          this.stuckTurnAngle += Time.deltaTime * 10f;
        num3 += this.stuckTurnAngle;
      }
      double num4 = (double) num3;
      Quaternion rotation1 = this.baseGameObjectTransform.rotation;
      double num5 = (double) ((Quaternion) ref rotation1).eulerAngles.y - 90.0;
      float num6 = -Mathf.DeltaAngle((float) num4, (float) num5);
      if (this.abnormalType == AbnormalType.TYPE_CRAWLER)
      {
        Transform gameObjectTransform = this.baseGameObjectTransform;
        Quaternion rotation2 = this.baseGameObjectTransform.rotation;
        rotation1 = this.baseGameObjectTransform.rotation;
        Quaternion quaternion1 = Quaternion.Euler(0.0f, (float) ((double) ((Quaternion) ref rotation1).eulerAngles.y + (double) num6), 0.0f);
        double num7 = (double) this.speed * 0.30000001192092896 * (double) Time.deltaTime / (double) this.myLevel;
        Quaternion quaternion2 = Quaternion.Lerp(rotation2, quaternion1, (float) num7);
        gameObjectTransform.rotation = quaternion2;
      }
      else
      {
        Transform gameObjectTransform = this.baseGameObjectTransform;
        Quaternion rotation3 = this.baseGameObjectTransform.rotation;
        rotation1 = this.baseGameObjectTransform.rotation;
        Quaternion quaternion3 = Quaternion.Euler(0.0f, (float) ((double) ((Quaternion) ref rotation1).eulerAngles.y + (double) num6), 0.0f);
        double num8 = (double) this.speed * 0.5 * (double) Time.deltaTime / (double) this.myLevel;
        Quaternion quaternion4 = Quaternion.Lerp(rotation3, quaternion3, (float) num8);
        gameObjectTransform.rotation = quaternion4;
      }
    }
  }

  private string[] GetAttackStrategy()
  {
    string[] attackStrategy = (string[]) null;
    if (this.isAlarm || (double) this.myHero.transform.position.y + 3.0 <= (double) this.neck.position.y + 10.0 * (double) this.myLevel)
    {
      if ((double) this.myHero.transform.position.y > (double) this.neck.position.y - 3.0 * (double) this.myLevel)
      {
        if ((double) this.myDistance < (double) this.attackDistance * 0.5)
        {
          if ((double) Vector3.Distance(this.myHero.transform.position, ((Component) this).transform.Find("chkOverHead").position) < 3.5999999046325684 * (double) this.myLevel)
          {
            if ((double) this.between2 > 0.0)
              attackStrategy = new string[1]
              {
                "grab_head_front_r"
              };
            else
              attackStrategy = new string[1]
              {
                "grab_head_front_l"
              };
          }
          else if ((double) Mathf.Abs(this.between2) < 90.0)
          {
            if ((double) Mathf.Abs(this.between2) < 30.0)
            {
              if ((double) Vector3.Distance(this.myHero.transform.position, ((Component) this).transform.Find("chkFront").position) < 2.5 * (double) this.myLevel)
                attackStrategy = new string[3]
                {
                  "attack_bite",
                  "attack_bite",
                  "attack_slap_face"
                };
            }
            else if ((double) this.between2 > 0.0)
            {
              if ((double) Vector3.Distance(this.myHero.transform.position, ((Component) this).transform.Find("chkFrontRight").position) < 2.5 * (double) this.myLevel)
                attackStrategy = new string[1]
                {
                  "attack_bite_r"
                };
            }
            else if ((double) Vector3.Distance(this.myHero.transform.position, ((Component) this).transform.Find("chkFrontLeft").position) < 2.5 * (double) this.myLevel)
              attackStrategy = new string[1]
              {
                "attack_bite_l"
              };
          }
          else if ((double) this.between2 > 0.0)
          {
            if ((double) Vector3.Distance(this.myHero.transform.position, ((Component) this).transform.Find("chkBackRight").position) < 2.7999999523162842 * (double) this.myLevel)
              attackStrategy = new string[3]
              {
                "grab_head_back_r",
                "grab_head_back_r",
                "attack_slap_back"
              };
          }
          else if ((double) Vector3.Distance(this.myHero.transform.position, ((Component) this).transform.Find("chkBackLeft").position) < 2.7999999523162842 * (double) this.myLevel)
            attackStrategy = new string[3]
            {
              "grab_head_back_l",
              "grab_head_back_l",
              "attack_slap_back"
            };
        }
        if (attackStrategy != null)
          return attackStrategy;
        if (this.abnormalType == AbnormalType.NORMAL || this.abnormalType == AbnormalType.TYPE_PUNK)
        {
          if (this.myDifficulty <= 0 && Random.Range(0, 1000) >= 3 || (double) Mathf.Abs(this.between2) >= 60.0)
            return attackStrategy;
          return new string[1]{ "attack_combo" };
        }
        if (this.abnormalType != AbnormalType.TYPE_I && this.abnormalType != AbnormalType.TYPE_JUMPER || this.myDifficulty <= 0 && Random.Range(0, 100) >= 50)
          return attackStrategy;
        return new string[1]{ "attack_abnormal_jump" };
      }
      switch ((double) Mathf.Abs(this.between2) >= 90.0 ? ((double) this.between2 <= 0.0 ? 3 : 4) : ((double) this.between2 <= 0.0 ? 2 : 1))
      {
        case 1:
          return (double) this.myDistance >= (double) this.attackDistance * 0.25 ? ((double) this.myDistance < (double) this.attackDistance * 0.5 ? (this.abnormalType != AbnormalType.TYPE_PUNK && this.abnormalType == AbnormalType.NORMAL ? new string[3]
          {
            "grab_ground_front_r",
            "grab_ground_front_r",
            "attack_stomp"
          } : new string[3]
          {
            "grab_ground_front_r",
            "grab_ground_front_r",
            "attack_abnormal_jump"
          }) : (this.abnormalType == AbnormalType.TYPE_PUNK ? new string[3]
          {
            "attack_combo",
            "attack_combo",
            "attack_abnormal_jump"
          } : (this.abnormalType == AbnormalType.NORMAL ? (this.myDifficulty > 0 ? new string[3]
          {
            "attack_front_ground",
            "attack_combo",
            "attack_combo"
          } : new string[5]
          {
            "attack_front_ground",
            "attack_front_ground",
            "attack_front_ground",
            "attack_front_ground",
            "attack_combo"
          }) : new string[1]{ "attack_abnormal_jump" }))) : (this.abnormalType != AbnormalType.TYPE_PUNK ? (this.abnormalType == AbnormalType.NORMAL ? new string[2]
          {
            "attack_front_ground",
            "attack_stomp"
          } : new string[1]{ "attack_kick" }) : new string[2]
          {
            "attack_kick",
            "attack_stomp"
          });
        case 2:
          return (double) this.myDistance >= (double) this.attackDistance * 0.25 ? ((double) this.myDistance < (double) this.attackDistance * 0.5 ? (this.abnormalType != AbnormalType.TYPE_PUNK && this.abnormalType == AbnormalType.NORMAL ? new string[3]
          {
            "grab_ground_front_l",
            "grab_ground_front_l",
            "attack_stomp"
          } : new string[3]
          {
            "grab_ground_front_l",
            "grab_ground_front_l",
            "attack_abnormal_jump"
          }) : (this.abnormalType == AbnormalType.TYPE_PUNK ? new string[3]
          {
            "attack_combo",
            "attack_combo",
            "attack_abnormal_jump"
          } : (this.abnormalType == AbnormalType.NORMAL ? (this.myDifficulty > 0 ? new string[3]
          {
            "attack_front_ground",
            "attack_combo",
            "attack_combo"
          } : new string[5]
          {
            "attack_front_ground",
            "attack_front_ground",
            "attack_front_ground",
            "attack_front_ground",
            "attack_combo"
          }) : new string[1]{ "attack_abnormal_jump" }))) : (this.abnormalType != AbnormalType.TYPE_PUNK ? (this.abnormalType == AbnormalType.NORMAL ? new string[2]
          {
            "attack_front_ground",
            "attack_stomp"
          } : new string[1]{ "attack_kick" }) : new string[2]
          {
            "attack_kick",
            "attack_stomp"
          });
        case 3:
          if ((double) this.myDistance >= (double) this.attackDistance * 0.5)
            return attackStrategy;
          int abnormalType1 = (int) this.abnormalType;
          return new string[1]{ "grab_ground_back_l" };
        case 4:
          if ((double) this.myDistance >= (double) this.attackDistance * 0.5)
            return attackStrategy;
          int abnormalType2 = (int) this.abnormalType;
          return new string[1]{ "grab_ground_back_r" };
      }
    }
    return attackStrategy;
  }

  private void getDown()
  {
    this.state = TitanState.down;
    this.isAlarm = true;
    this.playAnimation("sit_hunt_down");
    this.getdownTime = Random.Range(3f, 5f);
  }

  private GameObject getNearestHero2()
  {
    GameObject nearestHero2 = (GameObject) null;
    float num1 = float.PositiveInfinity;
    Vector3 position = this.baseTransform.position;
    foreach (HERO player in this.MultiplayerManager.getPlayers())
    {
      float num2 = Vector3.Distance(((Component) this).gameObject.transform.position, position);
      if ((double) num2 < (double) num1)
      {
        nearestHero2 = ((Component) player).gameObject;
        num1 = num2;
      }
    }
    foreach (TITAN_EREN eren in this.MultiplayerManager.getErens())
    {
      float num3 = Vector3.Distance(((Component) this).gameObject.transform.position, position);
      if ((double) num3 < (double) num1)
      {
        nearestHero2 = ((Component) eren).gameObject;
        num1 = num3;
      }
    }
    return nearestHero2;
  }

  private int getPunkNumber()
  {
    int punkNumber = 0;
    foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("titan"))
    {
      if (Object.op_Inequality((Object) gameObject.GetComponent<TITAN>(), (Object) null) && ((Object) gameObject.GetComponent<TITAN>()).name == "Punk")
        ++punkNumber;
    }
    return punkNumber;
  }

  private void grab(string type)
  {
    this.state = TitanState.grab;
    this.attacked = false;
    this.isAlarm = true;
    this.attackAnimation = type;
    this.crossFade("grab_" + type, 0.1f);
    this.isGrabHandLeft = true;
    this.grabbedTarget = (GameObject) null;
    string key = type;
    if (key != null)
    {
      // ISSUE: reference to a compiler-generated field
      if (TITAN.fswitchSmap7 == null)
      {
        // ISSUE: reference to a compiler-generated field
        TITAN.fswitchSmap7 = new Dictionary<string, int>(8)
        {
          {
            "ground_back_l",
            0
          },
          {
            "ground_back_r",
            1
          },
          {
            "ground_front_l",
            2
          },
          {
            "ground_front_r",
            3
          },
          {
            "head_back_l",
            4
          },
          {
            "head_back_r",
            5
          },
          {
            "head_front_l",
            6
          },
          {
            "head_front_r",
            7
          }
        };
      }
      int num;
      // ISSUE: reference to a compiler-generated field
      if (TITAN.fswitchSmap7.TryGetValue(key, out num))
      {
        switch (num)
        {
          case 0:
            this.attackCheckTimeA = 0.34f;
            this.attackCheckTimeB = 0.49f;
            break;
          case 1:
            this.attackCheckTimeA = 0.34f;
            this.attackCheckTimeB = 0.49f;
            this.isGrabHandLeft = false;
            break;
          case 2:
            this.attackCheckTimeA = 0.37f;
            this.attackCheckTimeB = 0.6f;
            break;
          case 3:
            this.attackCheckTimeA = 0.37f;
            this.attackCheckTimeB = 0.6f;
            this.isGrabHandLeft = false;
            break;
          case 4:
            this.attackCheckTimeA = 0.45f;
            this.attackCheckTimeB = 0.5f;
            this.isGrabHandLeft = false;
            break;
          case 5:
            this.attackCheckTimeA = 0.45f;
            this.attackCheckTimeB = 0.5f;
            break;
          case 6:
            this.attackCheckTimeA = 0.38f;
            this.attackCheckTimeB = 0.55f;
            break;
          case 7:
            this.attackCheckTimeA = 0.38f;
            this.attackCheckTimeB = 0.55f;
            this.isGrabHandLeft = false;
            break;
        }
      }
    }
    if (this.isGrabHandLeft)
      this.currentGrabHand = ((Component) this).transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_L/upper_arm_L/forearm_L/hand_L/hand_L_001");
    else
      this.currentGrabHand = ((Component) this).transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R/forearm_R/hand_R/hand_R_001");
  }

  [RPC]
  public void grabbedTargetEscape(PhotonMessageInfo info)
  {
    if (info != null && info.sender != this.grabbedTarget.GetComponent<PhotonView>().owner && PhotonNetwork.isMasterClient)
      FengGameManagerMKII.instance.kickPlayerRCIfMC(info.sender, true, "titan grabbedTargetEscape");
    else
      this.grabbedTarget = (GameObject) null;
  }

  [RPC]
  public void grabToLeft(PhotonMessageInfo info)
  {
    if (PhotonNetwork.isMasterClient && info != null && info.sender != this.photonView.owner)
    {
      FengGameManagerMKII.instance.kickPlayerRCIfMC(info.sender, true, "titan grabToLeft");
    }
    else
    {
      Transform transform1 = ((Component) this).transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_L/upper_arm_L/forearm_L/hand_L/hand_L_001");
      this.grabTF.transform.parent = transform1;
      this.grabTF.transform.position = ((Component) ((Component) transform1).GetComponent<SphereCollider>()).transform.position;
      this.grabTF.transform.rotation = ((Component) ((Component) transform1).GetComponent<SphereCollider>()).transform.rotation;
      Transform transform2 = this.grabTF.transform;
      transform2.localPosition = Vector3.op_Subtraction(transform2.localPosition, Vector3.op_Multiply(Vector3.op_Multiply(Vector3.right, ((Component) transform1).GetComponent<SphereCollider>().radius), 0.3f));
      Transform transform3 = this.grabTF.transform;
      transform3.localPosition = Vector3.op_Subtraction(transform3.localPosition, Vector3.op_Multiply(Vector3.op_Multiply(Vector3.up, ((Component) transform1).GetComponent<SphereCollider>().radius), 0.51f));
      Transform transform4 = this.grabTF.transform;
      transform4.localPosition = Vector3.op_Subtraction(transform4.localPosition, Vector3.op_Multiply(Vector3.op_Multiply(Vector3.forward, ((Component) transform1).GetComponent<SphereCollider>().radius), 0.3f));
      Transform transform5 = this.grabTF.transform;
      Quaternion localRotation = this.grabTF.transform.localRotation;
      double x = (double) ((Quaternion) ref localRotation).eulerAngles.x;
      localRotation = this.grabTF.transform.localRotation;
      double num1 = (double) ((Quaternion) ref localRotation).eulerAngles.y + 180.0;
      localRotation = this.grabTF.transform.localRotation;
      double num2 = (double) ((Quaternion) ref localRotation).eulerAngles.z + 180.0;
      Quaternion quaternion = Quaternion.Euler((float) x, (float) num1, (float) num2);
      transform5.localRotation = quaternion;
    }
  }

  [RPC]
  public void grabToRight(PhotonMessageInfo info)
  {
    if (PhotonNetwork.isMasterClient && info != null && info.sender != this.photonView.owner)
    {
      FengGameManagerMKII.instance.kickPlayerRCIfMC(info.sender, true, "titan grabToLeft");
    }
    else
    {
      Transform transform1 = ((Component) this).transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R/forearm_R/hand_R/hand_R_001");
      this.grabTF.transform.parent = transform1;
      this.grabTF.transform.position = ((Component) ((Component) transform1).GetComponent<SphereCollider>()).transform.position;
      this.grabTF.transform.rotation = ((Component) ((Component) transform1).GetComponent<SphereCollider>()).transform.rotation;
      Transform transform2 = this.grabTF.transform;
      transform2.localPosition = Vector3.op_Subtraction(transform2.localPosition, Vector3.op_Multiply(Vector3.op_Multiply(Vector3.right, ((Component) transform1).GetComponent<SphereCollider>().radius), 0.3f));
      Transform transform3 = this.grabTF.transform;
      transform3.localPosition = Vector3.op_Addition(transform3.localPosition, Vector3.op_Multiply(Vector3.op_Multiply(Vector3.up, ((Component) transform1).GetComponent<SphereCollider>().radius), 0.51f));
      Transform transform4 = this.grabTF.transform;
      transform4.localPosition = Vector3.op_Subtraction(transform4.localPosition, Vector3.op_Multiply(Vector3.op_Multiply(Vector3.forward, ((Component) transform1).GetComponent<SphereCollider>().radius), 0.3f));
      Transform transform5 = this.grabTF.transform;
      Quaternion localRotation = this.grabTF.transform.localRotation;
      double x = (double) ((Quaternion) ref localRotation).eulerAngles.x;
      localRotation = this.grabTF.transform.localRotation;
      double num = (double) ((Quaternion) ref localRotation).eulerAngles.y + 180.0;
      localRotation = this.grabTF.transform.localRotation;
      double z = (double) ((Quaternion) ref localRotation).eulerAngles.z;
      Quaternion quaternion = Quaternion.Euler((float) x, (float) num, (float) z);
      transform5.localRotation = quaternion;
    }
  }

  public void headMovement()
  {
    if (!this.hasDie)
    {
      if (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE)
      {
        if (this.photonView.isMine)
        {
          this.targetHeadRotation = this.head.rotation;
          bool flag = false;
          if (this.abnormalType != AbnormalType.TYPE_CRAWLER && this.state != TitanState.attack && this.state != TitanState.down && this.state != TitanState.hit && this.state != TitanState.recover && this.state != TitanState.eat && this.state != TitanState.hit_eye && !this.hasDie && (double) this.myDistance < 100.0 && Object.op_Inequality((Object) this.myHero, (Object) null))
          {
            Vector3 vector3 = Vector3.op_Subtraction(this.myHero.transform.position, ((Component) this).transform.position);
            this.angle = (float) (-(double) Mathf.Atan2(vector3.z, vector3.x) * 57.295780181884766);
            double angle = (double) this.angle;
            Quaternion rotation1 = ((Component) this).transform.rotation;
            double num1 = (double) ((Quaternion) ref rotation1).eulerAngles.y - 90.0;
            float num2 = Mathf.Clamp(-Mathf.DeltaAngle((float) angle, (float) num1), -40f, 40f);
            float num3 = Mathf.Clamp(Mathf.Atan2(this.neck.position.y + this.myLevel * 2f - this.myHero.transform.position.y, this.myDistance) * 57.29578f, -40f, 30f);
            Quaternion rotation2 = this.head.rotation;
            double num4 = (double) ((Quaternion) ref rotation2).eulerAngles.x + (double) num3;
            Quaternion rotation3 = this.head.rotation;
            double num5 = (double) ((Quaternion) ref rotation3).eulerAngles.y + (double) num2;
            Quaternion rotation4 = this.head.rotation;
            double z = (double) ((Quaternion) ref rotation4).eulerAngles.z;
            this.targetHeadRotation = Quaternion.Euler((float) num4, (float) num5, (float) z);
            if (!this.asClientLookTarget)
            {
              this.asClientLookTarget = true;
              this.photonView.RPC("setIfLookTarget", PhotonTargets.Others, (object) true);
            }
            flag = true;
          }
          if (!flag && this.asClientLookTarget)
          {
            this.asClientLookTarget = false;
            this.photonView.RPC("setIfLookTarget", PhotonTargets.Others, (object) false);
          }
          this.oldHeadRotation = this.state == TitanState.attack || this.state == TitanState.hit || this.state == TitanState.hit_eye ? Quaternion.Lerp(this.oldHeadRotation, this.targetHeadRotation, Time.deltaTime * 20f) : Quaternion.Lerp(this.oldHeadRotation, this.targetHeadRotation, Time.deltaTime * 10f);
        }
        else
        {
          this.targetHeadRotation = this.head.rotation;
          if (this.asClientLookTarget && Object.op_Inequality((Object) this.myHero, (Object) null))
          {
            Vector3 vector3 = Vector3.op_Subtraction(this.myHero.transform.position, ((Component) this).transform.position);
            this.angle = (float) (-(double) Mathf.Atan2(vector3.z, vector3.x) * 57.295780181884766);
            double angle = (double) this.angle;
            Quaternion rotation5 = ((Component) this).transform.rotation;
            double num6 = (double) ((Quaternion) ref rotation5).eulerAngles.y - 90.0;
            float num7 = Mathf.Clamp(-Mathf.DeltaAngle((float) angle, (float) num6), -40f, 40f);
            float num8 = Mathf.Clamp(Mathf.Atan2(this.neck.position.y + this.myLevel * 2f - this.myHero.transform.position.y, this.myDistance) * 57.29578f, -40f, 30f);
            Quaternion rotation6 = this.head.rotation;
            double num9 = (double) ((Quaternion) ref rotation6).eulerAngles.x + (double) num8;
            Quaternion rotation7 = this.head.rotation;
            double num10 = (double) ((Quaternion) ref rotation7).eulerAngles.y + (double) num7;
            Quaternion rotation8 = this.head.rotation;
            double z = (double) ((Quaternion) ref rotation8).eulerAngles.z;
            this.targetHeadRotation = Quaternion.Euler((float) num9, (float) num10, (float) z);
          }
          if (!this.hasDie)
            this.oldHeadRotation = Quaternion.Lerp(this.oldHeadRotation, this.targetHeadRotation, Time.deltaTime * 10f);
        }
      }
      else
      {
        this.targetHeadRotation = this.head.rotation;
        if (this.abnormalType != AbnormalType.TYPE_CRAWLER && this.state != TitanState.attack && this.state != TitanState.down && this.state != TitanState.hit && this.state != TitanState.recover && this.state != TitanState.hit_eye && !this.hasDie && (double) this.myDistance < 100.0 && Object.op_Inequality((Object) this.myHero, (Object) null))
        {
          Vector3 vector3 = Vector3.op_Subtraction(this.myHero.transform.position, ((Component) this).transform.position);
          this.angle = (float) (-(double) Mathf.Atan2(vector3.z, vector3.x) * 57.295780181884766);
          double angle = (double) this.angle;
          Quaternion rotation9 = ((Component) this).transform.rotation;
          double num11 = (double) ((Quaternion) ref rotation9).eulerAngles.y - 90.0;
          float num12 = Mathf.Clamp(-Mathf.DeltaAngle((float) angle, (float) num11), -40f, 40f);
          float num13 = Mathf.Clamp(Mathf.Atan2(this.neck.position.y + this.myLevel * 2f - this.myHero.transform.position.y, this.myDistance) * 57.29578f, -40f, 30f);
          Quaternion rotation10 = this.head.rotation;
          double num14 = (double) ((Quaternion) ref rotation10).eulerAngles.x + (double) num13;
          Quaternion rotation11 = this.head.rotation;
          double num15 = (double) ((Quaternion) ref rotation11).eulerAngles.y + (double) num12;
          Quaternion rotation12 = this.head.rotation;
          double z = (double) ((Quaternion) ref rotation12).eulerAngles.z;
          this.targetHeadRotation = Quaternion.Euler((float) num14, (float) num15, (float) z);
        }
        this.oldHeadRotation = this.state == TitanState.attack || this.state == TitanState.hit || this.state == TitanState.hit_eye ? Quaternion.Lerp(this.oldHeadRotation, this.targetHeadRotation, Time.deltaTime * 20f) : Quaternion.Lerp(this.oldHeadRotation, this.targetHeadRotation, Time.deltaTime * 10f);
      }
      this.head.rotation = this.oldHeadRotation;
    }
    if (((Component) this).animation.IsPlaying("die_headOff"))
      return;
    this.head.localScale = this.headscale;
  }

  public void headMovement2()
  {
    if (!this.hasDie)
    {
      this.targetHeadRotation = this.head.rotation;
      if (!this._ignoreLookTarget && this.abnormalType != AbnormalType.TYPE_CRAWLER && !this.hasDie && (double) this.myDistance < 100.0 && Object.op_Inequality((Object) this.myHero, (Object) null))
      {
        Vector3 vector3 = Vector3.op_Subtraction(this.myHero.transform.position, ((Component) this).transform.position);
        this.angle = (float) (-(double) Mathf.Atan2(vector3.z, vector3.x) * 57.295780181884766);
        double angle = (double) this.angle;
        Quaternion rotation1 = ((Component) this).transform.rotation;
        double num1 = (double) ((Quaternion) ref rotation1).eulerAngles.y - 90.0;
        float num2 = Mathf.Clamp(-Mathf.DeltaAngle((float) angle, (float) num1), -40f, 40f);
        float num3 = Mathf.Clamp(Mathf.Atan2(this.neck.position.y + this.myLevel * 2f - this.myHero.transform.position.y, this.myDistance) * 57.29578f, -40f, 30f);
        Quaternion rotation2 = this.head.rotation;
        double num4 = (double) ((Quaternion) ref rotation2).eulerAngles.x + (double) num3;
        Quaternion rotation3 = this.head.rotation;
        double num5 = (double) ((Quaternion) ref rotation3).eulerAngles.y + (double) num2;
        Quaternion rotation4 = this.head.rotation;
        double z = (double) ((Quaternion) ref rotation4).eulerAngles.z;
        this.targetHeadRotation = Quaternion.Euler((float) num4, (float) num5, (float) z);
      }
      this.oldHeadRotation = !this._fastHeadRotation ? Quaternion.Lerp(this.oldHeadRotation, this.targetHeadRotation, Time.deltaTime * 10f) : Quaternion.Lerp(this.oldHeadRotation, this.targetHeadRotation, Time.deltaTime * 20f);
      this.head.rotation = this.oldHeadRotation;
    }
    if (((Component) this).animation.IsPlaying("die_headOff"))
      return;
    this.head.localScale = this.headscale;
  }

  private void hit(string animationName, Vector3 attacker, float hitPauseTime)
  {
    this.state = TitanState.hit;
    this.hitAnimation = animationName;
    this.hitPause = hitPauseTime;
    this.playAnimation(this.hitAnimation);
    ((Component) this).animation[this.hitAnimation].time = 0.0f;
    ((Component) this).animation[this.hitAnimation].speed = 0.0f;
    Transform transform = ((Component) this).transform;
    Quaternion quaternion1 = Quaternion.LookRotation(Vector3.op_Subtraction(attacker, ((Component) this).transform.position));
    Quaternion quaternion2 = Quaternion.Euler(0.0f, ((Quaternion) ref quaternion1).eulerAngles.y, 0.0f);
    transform.rotation = quaternion2;
    this.needFreshCorePosition = true;
    if (!this.photonView.isMine || !Object.op_Inequality((Object) this.grabbedTarget, (Object) null))
      return;
    this.grabbedTarget.GetPhotonView().RPC("netUngrabbed", PhotonTargets.All);
  }

  public void hitAnkle()
  {
    if (this.hasDie || this.state == TitanState.down)
      return;
    if (Object.op_Inequality((Object) this.grabbedTarget, (Object) null))
      this.grabbedTarget.GetPhotonView().RPC("netUngrabbed", PhotonTargets.All);
    this.getDown();
  }

  [RPC]
  public void hitAnkleRPC(int viewID)
  {
    if (this.hasDie || this.state == TitanState.down)
      return;
    PhotonView photonView = PhotonView.Find(viewID);
    if (!Object.op_Inequality((Object) photonView, (Object) null))
      return;
    Vector3 vector3 = Vector3.op_Subtraction(((Component) photonView).gameObject.transform.position, ((Component) this).transform.position);
    if ((double) ((Vector3) ref vector3).magnitude >= 20.0)
      return;
    if (this.photonView.isMine && Object.op_Inequality((Object) this.grabbedTarget, (Object) null))
      this.grabbedTarget.GetPhotonView().RPC("netUngrabbed", PhotonTargets.All);
    this.getDown();
  }

  public void hitEye()
  {
    if (this.hasDie)
      return;
    this.justHitEye();
  }

  [RPC]
  public void hitEyeRPC(int viewID)
  {
    if (this.hasDie)
      return;
    Vector3 vector3 = Vector3.op_Subtraction(((Component) PhotonView.Find(viewID)).gameObject.transform.position, this.neck.position);
    if ((double) ((Vector3) ref vector3).magnitude >= 20.0)
      return;
    if (this.photonView.isMine && Object.op_Inequality((Object) this.grabbedTarget, (Object) null))
      this.grabbedTarget.GetPhotonView().RPC("netUngrabbed", PhotonTargets.All);
    if (this.hasDie)
      return;
    this.justHitEye();
  }

  public void hitL(Vector3 attacker, float hitPauseTime)
  {
    if (this.abnormalType == AbnormalType.TYPE_CRAWLER)
      return;
    if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
      this.hit("hit_eren_L", attacker, hitPauseTime);
    else
      this.photonView.RPC("hitLRPC", PhotonTargets.All, (object) attacker, (object) hitPauseTime);
  }

  [RPC]
  private void hitLRPC(Vector3 attacker, float hitPauseTime)
  {
    if (!this.photonView.isMine)
      return;
    Vector3 vector3 = Vector3.op_Subtraction(attacker, ((Component) this).transform.position);
    if ((double) ((Vector3) ref vector3).magnitude >= 80.0)
      return;
    this.hit("hit_eren_L", attacker, hitPauseTime);
  }

  public void hitR(Vector3 attacker, float hitPauseTime)
  {
    if (this.abnormalType == AbnormalType.TYPE_CRAWLER)
      return;
    if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
      this.hit("hit_eren_R", attacker, hitPauseTime);
    else
      this.photonView.RPC("hitRRPC", PhotonTargets.All, (object) attacker, (object) hitPauseTime);
  }

  [RPC]
  private void hitRRPC(Vector3 attacker, float hitPauseTime)
  {
    if (!this.photonView.isMine || this.hasDie)
      return;
    Vector3 vector3 = Vector3.op_Subtraction(attacker, ((Component) this).transform.position);
    if ((double) ((Vector3) ref vector3).magnitude >= 80.0)
      return;
    this.hit("hit_eren_R", attacker, hitPauseTime);
  }

  private void idle(float sbtime = 0.0f)
  {
    this.stuck = false;
    this.sbtime = sbtime;
    if (this.myDifficulty == 2 && (this.abnormalType == AbnormalType.TYPE_JUMPER || this.abnormalType == AbnormalType.TYPE_I))
      this.sbtime = Random.Range(0.0f, 1.5f);
    else if (this.myDifficulty >= 1)
      this.sbtime = 0.0f;
    this.sbtime = Mathf.Max(0.5f, this.sbtime);
    if (this.abnormalType == AbnormalType.TYPE_PUNK)
    {
      this.sbtime = 0.1f;
      if (this.myDifficulty == 1)
        this.sbtime += 0.4f;
    }
    this.state = TitanState.idle;
    if (this.abnormalType == AbnormalType.TYPE_CRAWLER)
      this.crossFadeIfNotPlaying("crawler_idle", 0.2f);
    else
      this.crossFadeIfNotPlaying(nameof (idle), 0.2f);
  }

  public bool IsGrounded()
  {
    LayerMask layerMask1 = LayerMask.op_Implicit(1 << LayerMask.NameToLayer("Ground"));
    LayerMask layerMask2 = LayerMask.op_Implicit(LayerMask.op_Implicit(LayerMask.op_Implicit(1 << LayerMask.NameToLayer("EnemyAABB"))) | LayerMask.op_Implicit(layerMask1));
    return Physics.Raycast(Vector3.op_Addition(((Component) this).gameObject.transform.position, Vector3.op_Multiply(Vector3.up, 0.1f)), Vector3.op_UnaryNegation(Vector3.up), 0.3f, ((LayerMask) ref layerMask2).value);
  }

  private void justEatHero(GameObject target, Transform hand)
  {
    if (!Object.op_Inequality((Object) target, (Object) null))
      return;
    if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER && this.photonView.isMine)
    {
      if (target.GetComponent<HERO>().HasDied())
        return;
      target.GetComponent<HERO>().markDie();
      if (this.nonAI)
      {
        object[] objArray = new object[2]
        {
          (object) this.photonView.viewID,
          (object) ((Object) this).name
        };
        target.GetComponent<HERO>().photonView.RPC("netDie2", PhotonTargets.All, objArray);
      }
      else
      {
        object[] objArray = new object[2]
        {
          (object) -1,
          (object) ((Object) this).name
        };
        target.GetComponent<HERO>().photonView.RPC("netDie2", PhotonTargets.All, objArray);
      }
    }
    else
    {
      if (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE)
        return;
      target.GetComponent<HERO>().die2(hand);
    }
  }

  private void justHitEye()
  {
    if (this.state == TitanState.hit_eye)
      return;
    if (this.state == TitanState.down || this.state == TitanState.sit)
      this.playAnimation("sit_hit_eye");
    else
      this.playAnimation("hit_eye");
    this.state = TitanState.hit_eye;
  }

  [RPC]
  public void labelRPC(int health, int maxHealth)
  {
    if (health < 0)
    {
      if (!Object.op_Inequality((Object) this.healthLabel, (Object) null))
        return;
      Object.Destroy((Object) this.healthLabel);
    }
    else
    {
      if (Object.op_Equality((Object) this.healthLabel, (Object) null))
      {
        this.healthLabel = (GameObject) Object.Instantiate(Resources.Load("UI/LabelNameOverHead"));
        ((Object) this.healthLabel).name = "LabelNameOverHead";
        this.healthLabel.transform.parent = ((Component) this).transform;
        this.healthLabel.transform.localPosition = new Vector3(0.0f, (float) (20.0 + 1.0 / (double) this.myLevel), 0.0f);
        if (this.abnormalType == AbnormalType.TYPE_CRAWLER)
          this.healthLabel.transform.localPosition = new Vector3(0.0f, (float) (10.0 + 1.0 / (double) this.myLevel), 0.0f);
        float num = 1f;
        if ((double) this.myLevel < 1.0)
          num = 1f / this.myLevel;
        this.healthLabel.transform.localScale = new Vector3(num, num, num);
        this.healthLabelEnabled = true;
      }
      string str = "[7FFF00]";
      float num1 = (float) health / (float) maxHealth;
      if ((double) num1 < 0.75 && (double) num1 >= 0.5)
        str = "[f2b50f]";
      else if ((double) num1 < 0.5 && (double) num1 >= 0.25)
        str = "[ff8100]";
      else if ((double) num1 < 0.25)
        str = "[ff3333]";
      this.healthLabel.GetComponent<UILabel>().text = str + Convert.ToString(health);
    }
  }

  public void lateUpdate()
  {
    if (GameMenu.Paused && IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
      return;
    if (((Component) this).animation.IsPlaying("run_walk"))
    {
      if ((double) ((Component) this).animation["run_walk"].normalizedTime % 1.0 > 0.10000000149011612 && (double) ((Component) this).animation["run_walk"].normalizedTime % 1.0 < 0.60000002384185791 && this.stepSoundPhase == 2)
      {
        this.stepSoundPhase = 1;
        Transform transform = ((Component) this).transform.Find("snd_titan_foot");
        ((Component) transform).GetComponent<AudioSource>().Stop();
        ((Component) transform).GetComponent<AudioSource>().Play();
      }
      if ((double) ((Component) this).animation["run_walk"].normalizedTime % 1.0 > 0.60000002384185791 && this.stepSoundPhase == 1)
      {
        this.stepSoundPhase = 2;
        Transform transform = ((Component) this).transform.Find("snd_titan_foot");
        ((Component) transform).GetComponent<AudioSource>().Stop();
        ((Component) transform).GetComponent<AudioSource>().Play();
      }
    }
    if (((Component) this).animation.IsPlaying("crawler_run"))
    {
      if ((double) ((Component) this).animation["crawler_run"].normalizedTime % 1.0 > 0.10000000149011612 && (double) ((Component) this).animation["crawler_run"].normalizedTime % 1.0 < 0.56000000238418579 && this.stepSoundPhase == 2)
      {
        this.stepSoundPhase = 1;
        Transform transform = ((Component) this).transform.Find("snd_titan_foot");
        ((Component) transform).GetComponent<AudioSource>().Stop();
        ((Component) transform).GetComponent<AudioSource>().Play();
      }
      if ((double) ((Component) this).animation["crawler_run"].normalizedTime % 1.0 > 0.56000000238418579 && this.stepSoundPhase == 1)
      {
        this.stepSoundPhase = 2;
        Transform transform = ((Component) this).transform.Find("snd_titan_foot");
        ((Component) transform).GetComponent<AudioSource>().Stop();
        ((Component) transform).GetComponent<AudioSource>().Play();
      }
    }
    if (((Component) this).animation.IsPlaying("run_abnormal"))
    {
      if ((double) ((Component) this).animation["run_abnormal"].normalizedTime % 1.0 > 0.4699999988079071 && (double) ((Component) this).animation["run_abnormal"].normalizedTime % 1.0 < 0.949999988079071 && this.stepSoundPhase == 2)
      {
        this.stepSoundPhase = 1;
        Transform transform = ((Component) this).transform.Find("snd_titan_foot");
        ((Component) transform).GetComponent<AudioSource>().Stop();
        ((Component) transform).GetComponent<AudioSource>().Play();
      }
      if (((double) ((Component) this).animation["run_abnormal"].normalizedTime % 1.0 > 0.949999988079071 || (double) ((Component) this).animation["run_abnormal"].normalizedTime % 1.0 < 0.4699999988079071) && this.stepSoundPhase == 1)
      {
        this.stepSoundPhase = 2;
        Transform transform = ((Component) this).transform.Find("snd_titan_foot");
        ((Component) transform).GetComponent<AudioSource>().Stop();
        ((Component) transform).GetComponent<AudioSource>().Play();
      }
    }
    this.headMovement();
    this.grounded = false;
  }

  public void lateUpdate2()
  {
    if (GameMenu.Paused && IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
      return;
    if (this.baseAnimation.IsPlaying("run_walk"))
    {
      if ((double) this.baseAnimation["run_walk"].normalizedTime % 1.0 > 0.10000000149011612 && (double) this.baseAnimation["run_walk"].normalizedTime % 1.0 < 0.60000002384185791 && this.stepSoundPhase == 2)
      {
        this.stepSoundPhase = 1;
        this.baseAudioSource.Stop();
        this.baseAudioSource.Play();
      }
      else if ((double) this.baseAnimation["run_walk"].normalizedTime % 1.0 > 0.60000002384185791 && this.stepSoundPhase == 1)
      {
        this.stepSoundPhase = 2;
        this.baseAudioSource.Stop();
        this.baseAudioSource.Play();
      }
    }
    else if (this.baseAnimation.IsPlaying("crawler_run"))
    {
      if ((double) this.baseAnimation["crawler_run"].normalizedTime % 1.0 > 0.10000000149011612 && (double) this.baseAnimation["crawler_run"].normalizedTime % 1.0 < 0.56000000238418579 && this.stepSoundPhase == 2)
      {
        this.stepSoundPhase = 1;
        this.baseAudioSource.Stop();
        this.baseAudioSource.Play();
      }
      else if ((double) this.baseAnimation["crawler_run"].normalizedTime % 1.0 > 0.56000000238418579 && this.stepSoundPhase == 1)
      {
        this.stepSoundPhase = 2;
        this.baseAudioSource.Stop();
        this.baseAudioSource.Play();
      }
    }
    else if (this.baseAnimation.IsPlaying("run_abnormal"))
    {
      if ((double) this.baseAnimation["run_abnormal"].normalizedTime % 1.0 > 0.4699999988079071 && (double) this.baseAnimation["run_abnormal"].normalizedTime % 1.0 < 0.949999988079071 && this.stepSoundPhase == 2)
      {
        this.stepSoundPhase = 1;
        this.baseAudioSource.Stop();
        this.baseAudioSource.Play();
      }
      else if (((double) this.baseAnimation["run_abnormal"].normalizedTime % 1.0 > 0.949999988079071 || (double) this.baseAnimation["run_abnormal"].normalizedTime % 1.0 < 0.4699999988079071) && this.stepSoundPhase == 1)
      {
        this.stepSoundPhase = 2;
        this.baseAudioSource.Stop();
        this.baseAudioSource.Play();
      }
    }
    this.headMovement2();
    this.grounded = false;
    this.updateLabel();
    this.updateCollider();
  }

  [RPC]
  private void laugh(float sbtime = 0.0f)
  {
    if (this.state != TitanState.idle && this.state != TitanState.turn && this.state != TitanState.chase)
      return;
    this.sbtime = sbtime;
    this.state = TitanState.laugh;
    this.crossFade(nameof (laugh), 0.2f);
  }

  public void loadskin()
  {
    this.skin = 0;
    this.eye = false;
    BaseCustomSkinSettings<TitanCustomSkinSet> titan = SettingsManager.CustomSkinSettings.Titan;
    if (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE && !this.photonView.isMine || !titan.SkinsEnabled.Value)
      return;
    int index1 = (int) Random.Range(0.0f, 5f);
    int index2 = index1;
    TitanCustomSkinSet selectedSet = (TitanCustomSkinSet) titan.GetSelectedSet();
    if (selectedSet.RandomizedPairs.Value)
      index2 = (int) Random.Range(0.0f, 5f);
    string body = ((TypedSetting<string>) selectedSet.Bodies.GetItemAt(index1)).Value;
    string eye = ((TypedSetting<string>) selectedSet.Eyes.GetItemAt(index2)).Value;
    this.skin = index1;
    if (eye.EndsWith(".jpg") || eye.EndsWith(".png") || eye.EndsWith(".jpeg"))
      this.eye = true;
    ((Component) this).GetComponent<TITAN_SETUP>().setVar(this.skin, this.eye);
    if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
      this.StartCoroutine(this.loadskinE(body, eye));
    else
      this.photonView.RPC("loadskinRPC", PhotonTargets.AllBuffered, (object) body, (object) eye);
  }

  public IEnumerator loadskinE(string body, string eye)
  {
    TITAN titan = this;
    while (!titan.hasSpawn)
      yield return (object) null;
    yield return (object) titan.StartCoroutine(titan._customSkinLoader.LoadSkinsFromRPC(new object[3]
    {
      (object) false,
      (object) body,
      (object) eye
    }));
  }

  [RPC]
  public void loadskinRPC(string body, string eye, PhotonMessageInfo info)
  {
    if (info.sender != this.photonView.owner)
      return;
    BaseCustomSkinSettings<TitanCustomSkinSet> titan = SettingsManager.CustomSkinSettings.Titan;
    if (!titan.SkinsEnabled.Value || titan.SkinsLocal.Value && !this.photonView.isMine)
      return;
    this.StartCoroutine(this.loadskinE(body, eye));
  }

  private bool longRangeAttackCheck()
  {
    if (this.abnormalType == AbnormalType.TYPE_PUNK && Object.op_Inequality((Object) this.myHero, (Object) null) && Object.op_Inequality((Object) this.myHero.rigidbody, (Object) null))
    {
      Vector3 line = Vector3.op_Multiply(Vector3.op_Multiply(this.myHero.rigidbody.velocity, Time.deltaTime), 30f);
      if ((double) ((Vector3) ref line).sqrMagnitude > 10.0)
      {
        if (this.simpleHitTestLineAndBall(line, Vector3.op_Subtraction(((Component) this).transform.Find("chkAeLeft").position, this.myHero.transform.position), 5f * this.myLevel))
        {
          this.attack("anti_AE_l");
          return true;
        }
        if (this.simpleHitTestLineAndBall(line, Vector3.op_Subtraction(((Component) this).transform.Find("chkAeLLeft").position, this.myHero.transform.position), 5f * this.myLevel))
        {
          this.attack("anti_AE_low_l");
          return true;
        }
        if (this.simpleHitTestLineAndBall(line, Vector3.op_Subtraction(((Component) this).transform.Find("chkAeRight").position, this.myHero.transform.position), 5f * this.myLevel))
        {
          this.attack("anti_AE_r");
          return true;
        }
        if (this.simpleHitTestLineAndBall(line, Vector3.op_Subtraction(((Component) this).transform.Find("chkAeLRight").position, this.myHero.transform.position), 5f * this.myLevel))
        {
          this.attack("anti_AE_low_r");
          return true;
        }
      }
      Vector3 vector3_1 = Vector3.op_Subtraction(this.myHero.transform.position, ((Component) this).transform.position);
      double num1 = -(double) Mathf.Atan2(vector3_1.z, vector3_1.x) * 57.295780181884766;
      Quaternion rotation = ((Component) this).gameObject.transform.rotation;
      double num2 = (double) ((Quaternion) ref rotation).eulerAngles.y - 90.0;
      float num3 = -Mathf.DeltaAngle((float) num1, (float) num2);
      if ((double) this.rockInterval > 0.0)
        this.rockInterval -= Time.deltaTime;
      else if ((double) Mathf.Abs(num3) < 5.0)
      {
        Vector3 vector3_2 = Vector3.op_Subtraction(Vector3.op_Addition(this.myHero.transform.position, line), ((Component) this).transform.position);
        float sqrMagnitude = ((Vector3) ref vector3_2).sqrMagnitude;
        if ((double) sqrMagnitude > 8000.0 && (double) sqrMagnitude < 90000.0)
        {
          this.attack("throw");
          this.rockInterval = 2f;
          return true;
        }
      }
    }
    return false;
  }

  private bool longRangeAttackCheck2()
  {
    if (this.abnormalType == AbnormalType.TYPE_PUNK && Object.op_Inequality((Object) this.myHero, (Object) null))
    {
      Vector3 line = Vector3.op_Multiply(Vector3.op_Multiply(this.myHero.rigidbody.velocity, Time.deltaTime), 30f);
      if ((double) ((Vector3) ref line).sqrMagnitude > 10.0)
      {
        if (this.simpleHitTestLineAndBall(line, Vector3.op_Subtraction(this.baseTransform.Find("chkAeLeft").position, this.myHero.transform.position), 5f * this.myLevel))
        {
          this.attack2("anti_AE_l");
          return true;
        }
        if (this.simpleHitTestLineAndBall(line, Vector3.op_Subtraction(this.baseTransform.Find("chkAeLLeft").position, this.myHero.transform.position), 5f * this.myLevel))
        {
          this.attack2("anti_AE_low_l");
          return true;
        }
        if (this.simpleHitTestLineAndBall(line, Vector3.op_Subtraction(this.baseTransform.Find("chkAeRight").position, this.myHero.transform.position), 5f * this.myLevel))
        {
          this.attack2("anti_AE_r");
          return true;
        }
        if (this.simpleHitTestLineAndBall(line, Vector3.op_Subtraction(this.baseTransform.Find("chkAeLRight").position, this.myHero.transform.position), 5f * this.myLevel))
        {
          this.attack2("anti_AE_low_r");
          return true;
        }
      }
      Vector3 vector3_1 = Vector3.op_Subtraction(this.myHero.transform.position, this.baseTransform.position);
      double num1 = -(double) Mathf.Atan2(vector3_1.z, vector3_1.x) * 57.295780181884766;
      Quaternion rotation = this.baseGameObjectTransform.rotation;
      double num2 = (double) ((Quaternion) ref rotation).eulerAngles.y - 90.0;
      float num3 = -Mathf.DeltaAngle((float) num1, (float) num2);
      if ((double) this.rockInterval > 0.0)
        this.rockInterval -= Time.deltaTime;
      else if ((double) Mathf.Abs(num3) < 5.0)
      {
        Vector3 vector3_2 = Vector3.op_Subtraction(Vector3.op_Addition(this.myHero.transform.position, line), this.baseTransform.position);
        float sqrMagnitude = ((Vector3) ref vector3_2).sqrMagnitude;
        if ((double) sqrMagnitude > 8000.0 && (double) sqrMagnitude < 90000.0 && SettingsManager.LegacyGameSettings.RockThrowEnabled.Value)
        {
          this.attack2("throw");
          this.rockInterval = 2f;
          return true;
        }
      }
    }
    return false;
  }

  public void moveTo(float posX, float posY, float posZ) => ((Component) this).transform.position = new Vector3(posX, posY, posZ);

  [RPC]
  public void moveToRPC(float posX, float posY, float posZ, PhotonMessageInfo info)
  {
    if (!info.sender.isMasterClient)
      return;
    ((Component) this).transform.position = new Vector3(posX, posY, posZ);
  }

  [RPC]
  private void netCrossFade(string aniName, float time)
  {
    ((Component) this).animation.CrossFade(aniName, time);
    this.CheckAnimationLookTarget(aniName);
  }

  [RPC]
  private void netDie()
  {
    this.asClientLookTarget = false;
    if (this.hasDie)
      return;
    this.hasDie = true;
    if (this.nonAI)
    {
      this.currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().setMainObject((GameObject) null);
      this.currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().setSpectorMode(true);
      this.currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = true;
      Hashtable propertiesToSet1 = new Hashtable();
      ((Dictionary<object, object>) propertiesToSet1).Add((object) PhotonPlayerProperty.dead, (object) true);
      PhotonNetwork.player.SetCustomProperties(propertiesToSet1);
      Hashtable propertiesToSet2 = new Hashtable();
      ((Dictionary<object, object>) propertiesToSet2).Add((object) PhotonPlayerProperty.deaths, (object) ((int) PhotonNetwork.player.customProperties[(object) PhotonPlayerProperty.deaths] + 1));
      PhotonNetwork.player.SetCustomProperties(propertiesToSet2);
    }
    this.dieAnimation();
  }

  [RPC]
  private void netPlayAnimation(string aniName)
  {
    ((Component) this).animation.Play(aniName);
    this.CheckAnimationLookTarget(aniName);
  }

  [RPC]
  private void netPlayAnimationAt(string aniName, float normalizedTime)
  {
    ((Component) this).animation.Play(aniName);
    this.CheckAnimationLookTarget(aniName);
    ((Component) this).animation[aniName].normalizedTime = normalizedTime;
  }

  [RPC]
  private void netSetAbnormalType(int type)
  {
    if (!this.hasload)
    {
      this.hasload = true;
      this.loadskin();
    }
    switch (type)
    {
      case 0:
        this.abnormalType = AbnormalType.NORMAL;
        ((Object) this).name = "Titan";
        this.runAnimation = "run_walk";
        ((Component) this).GetComponent<TITAN_SETUP>().setHair2();
        break;
      case 1:
        this.abnormalType = AbnormalType.TYPE_I;
        ((Object) this).name = "Aberrant";
        this.runAnimation = "run_abnormal";
        ((Component) this).GetComponent<TITAN_SETUP>().setHair2();
        break;
      case 2:
        this.abnormalType = AbnormalType.TYPE_JUMPER;
        ((Object) this).name = "Jumper";
        this.runAnimation = "run_abnormal";
        ((Component) this).GetComponent<TITAN_SETUP>().setHair2();
        break;
      case 3:
        this.abnormalType = AbnormalType.TYPE_CRAWLER;
        ((Object) this).name = "Crawler";
        this.runAnimation = "crawler_run";
        ((Component) this).GetComponent<TITAN_SETUP>().setHair2();
        break;
      case 4:
        this.abnormalType = AbnormalType.TYPE_PUNK;
        ((Object) this).name = "Punk";
        this.runAnimation = "run_abnormal_1";
        ((Component) this).GetComponent<TITAN_SETUP>().setHair2();
        break;
    }
    if (this.abnormalType == AbnormalType.TYPE_I || this.abnormalType == AbnormalType.TYPE_JUMPER || this.abnormalType == AbnormalType.TYPE_PUNK)
    {
      this.speed = 18f;
      if ((double) this.myLevel > 1.0)
        this.speed *= Mathf.Sqrt(this.myLevel);
      if (this.myDifficulty == 1)
        this.speed *= 1.4f;
      if (this.myDifficulty == 2)
        this.speed *= 1.6f;
      this.baseAnimation["turnaround1"].speed = 2f;
      this.baseAnimation["turnaround2"].speed = 2f;
    }
    if (this.abnormalType == AbnormalType.TYPE_CRAWLER)
    {
      this.chaseDistance += 50f;
      this.speed = 25f;
      if ((double) this.myLevel > 1.0)
        this.speed *= Mathf.Sqrt(this.myLevel);
      if (this.myDifficulty == 1)
        this.speed *= 2f;
      if (this.myDifficulty == 2)
        this.speed *= 2.2f;
      ((Component) this.baseTransform.Find("AABB")).gameObject.GetComponent<CapsuleCollider>().height = 10f;
      ((Component) this.baseTransform.Find("AABB")).gameObject.GetComponent<CapsuleCollider>().radius = 5f;
      ((Component) this.baseTransform.Find("AABB")).gameObject.GetComponent<CapsuleCollider>().center = new Vector3(0.0f, 5.05f, 0.0f);
    }
    if (this.nonAI)
    {
      this.speed = this.abnormalType != AbnormalType.TYPE_CRAWLER ? Mathf.Min(60f, this.speed) : Mathf.Min(70f, this.speed);
      this.baseAnimation["attack_jumper_0"].speed = 7f;
      this.baseAnimation["attack_crawler_jump_0"].speed = 4f;
    }
    this.baseAnimation["attack_combo_1"].speed = 1f;
    this.baseAnimation["attack_combo_2"].speed = 1f;
    this.baseAnimation["attack_combo_3"].speed = 1f;
    this.baseAnimation["attack_quick_turn_l"].speed = 1f;
    this.baseAnimation["attack_quick_turn_r"].speed = 1f;
    this.baseAnimation["attack_anti_AE_l"].speed = 1.1f;
    this.baseAnimation["attack_anti_AE_low_l"].speed = 1.1f;
    this.baseAnimation["attack_anti_AE_r"].speed = 1.1f;
    this.baseAnimation["attack_anti_AE_low_r"].speed = 1.1f;
    this.idle();
  }

  [RPC]
  private void netSetLevel(float level, int AI, int skinColor, PhotonMessageInfo info)
  {
    if (!info.sender.isMasterClient && !info.sender.isLocal && this.photonView.owner != info.sender)
      FengGameManagerMKII.instance.kickPlayerRCIfMC(info.sender, true, "titan netSetLevel");
    this.setLevel2(level, AI, skinColor);
    if ((double) level > 5.0)
    {
      this.headscale = new Vector3(1f, 1f, 1f);
    }
    else
    {
      if ((double) level >= 1.0 || !FengGameManagerMKII.level.StartsWith("Custom"))
        return;
      ((Component) this.myTitanTrigger).GetComponent<CapsuleCollider>().radius *= 2.5f - level;
    }
  }

  private void OnCollisionStay() => this.grounded = true;

  private void OnDestroy()
  {
    if (!Object.op_Inequality((Object) GameObject.Find("MultiplayerManager"), (Object) null))
      return;
    GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().removeTitan(this);
  }

  public void OnTitanDie(PhotonView view)
  {
    if (!FengGameManagerMKII.logicLoaded || !((Dictionary<object, object>) FengGameManagerMKII.RCEvents).ContainsKey((object) nameof (OnTitanDie)))
      return;
    RCEvent rcEvent = (RCEvent) FengGameManagerMKII.RCEvents[(object) nameof (OnTitanDie)];
    string[] rcVariableName = (string[]) FengGameManagerMKII.RCVariableNames[(object) nameof (OnTitanDie)];
    if (((Dictionary<object, object>) FengGameManagerMKII.titanVariables).ContainsKey((object) rcVariableName[0]))
      FengGameManagerMKII.titanVariables[(object) rcVariableName[0]] = (object) this;
    else
      ((Dictionary<object, object>) FengGameManagerMKII.titanVariables).Add((object) rcVariableName[0], (object) this);
    if (((Dictionary<object, object>) FengGameManagerMKII.playerVariables).ContainsKey((object) rcVariableName[1]))
      FengGameManagerMKII.playerVariables[(object) rcVariableName[1]] = (object) view.owner;
    else
      ((Dictionary<object, object>) FengGameManagerMKII.playerVariables).Add((object) rcVariableName[1], (object) view.owner);
    rcEvent.checkEvent();
  }

  private void playAnimation(string aniName)
  {
    ((Component) this).animation.Play(aniName);
    this.CheckAnimationLookTarget(aniName);
    if (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.MULTIPLAYER || !this.photonView.isMine)
      return;
    this.photonView.RPC("netPlayAnimation", PhotonTargets.Others, (object) aniName);
  }

  private void playAnimationAt(string aniName, float normalizedTime)
  {
    ((Component) this).animation.Play(aniName);
    this.CheckAnimationLookTarget(aniName);
    ((Component) this).animation[aniName].normalizedTime = normalizedTime;
    if (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.MULTIPLAYER || !this.photonView.isMine)
      return;
    this.photonView.RPC("netPlayAnimationAt", PhotonTargets.Others, (object) aniName, (object) normalizedTime);
  }

  private void playSound(string sndname)
  {
    this.playsoundRPC(sndname);
    if (!this.photonView.isMine)
      return;
    this.photonView.RPC("playsoundRPC", PhotonTargets.Others, (object) sndname);
  }

  [RPC]
  private void playsoundRPC(string sndname) => ((Component) ((Component) this).transform.Find(sndname)).GetComponent<AudioSource>().Play();

  public void pt()
  {
    if (this.controller.bite)
      this.attack2("bite");
    if (this.controller.bitel)
      this.attack2("bite_l");
    if (this.controller.biter)
      this.attack2("bite_r");
    if (this.controller.chopl)
      this.attack2("anti_AE_low_l");
    if (this.controller.chopr)
      this.attack2("anti_AE_low_r");
    if (this.controller.choptl)
      this.attack2("anti_AE_l");
    if (this.controller.choptr)
      this.attack2("anti_AE_r");
    if (this.controller.cover && (double) this.stamina > 75.0)
    {
      this.recoverpt();
      this.stamina -= 75f;
    }
    if (this.controller.grabbackl)
      this.grab("ground_back_l");
    if (this.controller.grabbackr)
      this.grab("ground_back_r");
    if (this.controller.grabfrontl)
      this.grab("ground_front_l");
    if (this.controller.grabfrontr)
      this.grab("ground_front_r");
    if (this.controller.grabnapel)
      this.grab("head_back_l");
    if (!this.controller.grabnaper)
      return;
    this.grab("head_back_r");
  }

  public void randomRun(Vector3 targetPt, float r)
  {
    this.state = TitanState.random_run;
    this.targetCheckPt = targetPt;
    this.targetR = r;
    this.random_run_time = Random.Range(1f, 2f);
    this.crossFade(this.runAnimation, 0.5f);
  }

  private void recover()
  {
    this.state = TitanState.recover;
    this.playAnimation("idle_recovery");
    this.getdownTime = Random.Range(2f, 5f);
  }

  private void recoverpt()
  {
    this.state = TitanState.recover;
    this.playAnimation("idle_recovery");
    this.getdownTime = Random.Range(1.8f, 2.5f);
  }

  private void remainSitdown()
  {
    this.state = TitanState.sit;
    this.playAnimation("sit_idle");
    this.getdownTime = Random.Range(10f, 30f);
  }

  public void resetLevel(float level)
  {
    this.myLevel = level;
    this.setmyLevel();
  }

  public void setAbnormalType(AbnormalType type, bool forceCrawler = false)
  {
    int type1 = 0;
    float num = 0.02f * (float) (IN_GAME_MAIN_CAMERA.difficulty + 1);
    if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.PVP_AHSS)
      num = 100f;
    switch (type)
    {
      case AbnormalType.NORMAL:
        type1 = (double) Random.Range(0.0f, 1f) >= (double) num ? 0 : 4;
        break;
      case AbnormalType.TYPE_I:
        type1 = (double) Random.Range(0.0f, 1f) >= (double) num ? 1 : 4;
        break;
      case AbnormalType.TYPE_JUMPER:
        type1 = (double) Random.Range(0.0f, 1f) >= (double) num ? 2 : 4;
        break;
      case AbnormalType.TYPE_CRAWLER:
        type1 = 3;
        if (Object.op_Inequality((Object) GameObject.Find("Crawler"), (Object) null) && Random.Range(0, 1000) > 5)
        {
          type1 = 2;
          break;
        }
        break;
      case AbnormalType.TYPE_PUNK:
        type1 = 4;
        break;
    }
    if (forceCrawler)
      type1 = 3;
    if (type1 == 4)
    {
      if (!LevelInfo.getInfo(FengGameManagerMKII.level).punk)
      {
        type1 = 1;
      }
      else
      {
        if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE && this.getPunkNumber() >= 3)
          type1 = 1;
        if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.SURVIVE_MODE)
        {
          switch (GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().wave)
          {
            case 5:
            case 10:
            case 15:
            case 20:
              break;
            default:
              type1 = 1;
              break;
          }
        }
      }
    }
    if (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE && this.photonView.isMine)
    {
      this.photonView.RPC("netSetAbnormalType", PhotonTargets.AllBuffered, (object) type1);
    }
    else
    {
      if (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE)
        return;
      this.netSetAbnormalType(type1);
    }
  }

  public void setAbnormalType2(AbnormalType type, bool forceCrawler)
  {
    bool flag = false;
    if (SettingsManager.LegacyGameSettings.TitanSpawnEnabled.Value)
      flag = true;
    if (FengGameManagerMKII.level.StartsWith("Custom"))
      flag = true;
    int type1 = 0;
    float num = 0.02f * (float) (IN_GAME_MAIN_CAMERA.difficulty + 1);
    if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.PVP_AHSS)
      num = 100f;
    switch (type)
    {
      case AbnormalType.NORMAL:
        type1 = (double) Random.Range(0.0f, 1f) >= (double) num ? 0 : 4;
        if (flag)
        {
          type1 = 0;
          break;
        }
        break;
      case AbnormalType.TYPE_I:
        type1 = (double) Random.Range(0.0f, 1f) >= (double) num ? 1 : 4;
        if (flag)
        {
          type1 = 1;
          break;
        }
        break;
      case AbnormalType.TYPE_JUMPER:
        type1 = (double) Random.Range(0.0f, 1f) >= (double) num ? 2 : 4;
        if (flag)
        {
          type1 = 2;
          break;
        }
        break;
      case AbnormalType.TYPE_CRAWLER:
        type1 = 3;
        if (Object.op_Inequality((Object) GameObject.Find("Crawler"), (Object) null) && Random.Range(0, 1000) > 5)
          type1 = 2;
        if (flag)
        {
          type1 = 3;
          break;
        }
        break;
      case AbnormalType.TYPE_PUNK:
        type1 = 4;
        break;
    }
    if (forceCrawler)
      type1 = 3;
    if (type1 == 4)
    {
      if (!LevelInfo.getInfo(FengGameManagerMKII.level).punk)
      {
        type1 = 1;
      }
      else
      {
        if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE && this.getPunkNumber() >= 3)
          type1 = 1;
        if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.SURVIVE_MODE)
        {
          switch (FengGameManagerMKII.instance.wave)
          {
            case 5:
            case 10:
            case 15:
            case 20:
              break;
            default:
              type1 = 1;
              break;
          }
        }
      }
      if (flag)
        type1 = 4;
    }
    if (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE && this.photonView.isMine)
    {
      this.photonView.RPC("netSetAbnormalType", PhotonTargets.AllBuffered, (object) type1);
    }
    else
    {
      if (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE)
        return;
      this.netSetAbnormalType(type1);
    }
  }

  [RPC]
  private void setIfLookTarget(bool bo) => this.asClientLookTarget = bo;

  private void setLevel(float level, int AI, int skinColor)
  {
    this.myLevel = level;
    this.myLevel = Mathf.Clamp(this.myLevel, 0.7f, 3f);
    this.attackWait += Random.Range(0.0f, 2f);
    this.chaseDistance += this.myLevel * 10f;
    ((Component) this).transform.localScale = new Vector3(this.myLevel, this.myLevel, this.myLevel);
    float num1 = Mathf.Min(Mathf.Pow(2f / this.myLevel, 0.35f), 1.25f);
    this.headscale = new Vector3(num1, num1, num1);
    this.head = ((Component) this).transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck/head");
    this.head.localScale = this.headscale;
    if (skinColor != 0)
      ((Renderer) this.mainMaterial.GetComponent<SkinnedMeshRenderer>()).material.color = skinColor != 1 ? (skinColor != 2 ? FengColor.titanSkin3 : FengColor.titanSkin2) : FengColor.titanSkin1;
    float num2 = Mathf.Clamp((float) (1.3999999761581421 - ((double) this.myLevel - 0.699999988079071) * 0.15000000596046448), 0.9f, 1.5f);
    foreach (AnimationState animationState in ((Component) this).animation)
      animationState.speed = num2;
    ((Component) this).rigidbody.mass *= this.myLevel;
    ((Component) this).rigidbody.rotation = Quaternion.Euler(0.0f, (float) Random.Range(0, 360), 0.0f);
    if ((double) this.myLevel > 1.0)
      this.speed *= Mathf.Sqrt(this.myLevel);
    this.myDifficulty = AI;
    if (this.myDifficulty == 1 || this.myDifficulty == 2)
    {
      foreach (AnimationState animationState in ((Component) this).animation)
        animationState.speed = num2 * 1.05f;
      if (this.nonAI)
        this.speed *= 1.1f;
      else
        this.speed *= 1.4f;
      this.chaseDistance *= 1.15f;
    }
    if (this.myDifficulty == 2)
    {
      foreach (AnimationState animationState in ((Component) this).animation)
        animationState.speed = num2 * 1.05f;
      if (this.nonAI)
        this.speed *= 1.1f;
      else
        this.speed *= 1.5f;
      this.chaseDistance *= 1.3f;
    }
    if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.ENDLESS_TITAN || IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.SURVIVE_MODE)
      this.chaseDistance = 999999f;
    if (this.nonAI)
      this.speed = this.abnormalType != AbnormalType.TYPE_CRAWLER ? Mathf.Min(60f, this.speed) : Mathf.Min(70f, this.speed);
    this.attackDistance = Vector3.Distance(((Component) this).transform.position, ((Component) this).transform.Find("ap_front_ground").position) * 1.65f;
  }

  private void setLevel2(float level, int AI, int skinColor)
  {
    this.myLevel = level;
    this.myLevel = Mathf.Clamp(this.myLevel, 0.1f, 50f);
    this.attackWait += Random.Range(0.0f, 2f);
    this.chaseDistance += this.myLevel * 10f;
    ((Component) this).transform.localScale = new Vector3(this.myLevel, this.myLevel, this.myLevel);
    float num1 = Mathf.Min(Mathf.Pow(2f / this.myLevel, 0.35f), 1.25f);
    this.headscale = new Vector3(num1, num1, num1);
    this.head = ((Component) this).transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck/head");
    this.head.localScale = this.headscale;
    if (skinColor != 0)
      ((Renderer) this.mainMaterial.GetComponent<SkinnedMeshRenderer>()).material.color = skinColor != 1 ? (skinColor != 2 ? FengColor.titanSkin3 : FengColor.titanSkin2) : FengColor.titanSkin1;
    float num2 = Mathf.Clamp((float) (1.3999999761581421 - ((double) this.myLevel - 0.699999988079071) * 0.15000000596046448), 0.9f, 1.5f);
    foreach (AnimationState animationState in ((Component) this).animation)
      animationState.speed = num2;
    ((Component) this).rigidbody.mass *= this.myLevel;
    ((Component) this).rigidbody.rotation = Quaternion.Euler(0.0f, (float) Random.Range(0, 360), 0.0f);
    if ((double) this.myLevel > 1.0)
      this.speed *= Mathf.Sqrt(this.myLevel);
    this.myDifficulty = AI;
    if (this.myDifficulty == 1 || this.myDifficulty == 2)
    {
      foreach (AnimationState animationState in ((Component) this).animation)
        animationState.speed = num2 * 1.05f;
      if (this.nonAI)
        this.speed *= 1.1f;
      else
        this.speed *= 1.4f;
      this.chaseDistance *= 1.15f;
    }
    if (this.myDifficulty == 2)
    {
      foreach (AnimationState animationState in ((Component) this).animation)
        animationState.speed = num2 * 1.05f;
      if (this.nonAI)
        this.speed *= 1.1f;
      else
        this.speed *= 1.5f;
      this.chaseDistance *= 1.3f;
    }
    if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.ENDLESS_TITAN || IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.SURVIVE_MODE)
      this.chaseDistance = 999999f;
    if (this.nonAI)
      this.speed = this.abnormalType != AbnormalType.TYPE_CRAWLER ? Mathf.Min(60f, this.speed) : Mathf.Min(70f, this.speed);
    this.attackDistance = Vector3.Distance(((Component) this).transform.position, ((Component) this).transform.Find("ap_front_ground").position) * 1.65f;
  }

  private void setmyLevel()
  {
    ((Component) this).animation.cullingType = (AnimationCullingType) 1;
    if (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE && this.photonView.isMine)
    {
      this.photonView.RPC("netSetLevel", PhotonTargets.AllBuffered, (object) this.myLevel, (object) GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().difficulty, (object) Random.Range(0, 4));
      ((Component) this).animation.cullingType = (AnimationCullingType) 0;
    }
    else
    {
      if (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE)
        return;
      this.setLevel2(this.myLevel, IN_GAME_MAIN_CAMERA.difficulty, Random.Range(0, 4));
    }
  }

  [RPC]
  private void setMyTarget(int ID)
  {
    if (ID == -1)
      this.myHero = (GameObject) null;
    PhotonView photonView = PhotonView.Find(ID);
    if (!Object.op_Inequality((Object) photonView, (Object) null))
      return;
    this.myHero = ((Component) photonView).gameObject;
  }

  public void setRoute(GameObject route)
  {
    this.checkPoints = new ArrayList();
    for (int index = 1; index <= 10; ++index)
      this.checkPoints.Add((object) route.transform.Find("r" + index.ToString()).position);
    this.checkPoints.Add((object) "end");
  }

  private bool simpleHitTestLineAndBall(Vector3 line, Vector3 ball, float R)
  {
    Vector3 vector3_1 = Vector3.Project(ball, line);
    Vector3 vector3_2 = Vector3.op_Subtraction(ball, vector3_1);
    return (double) ((Vector3) ref vector3_2).magnitude <= (double) R && (double) Vector3.Dot(line, vector3_1) >= 0.0 && (double) ((Vector3) ref vector3_1).sqrMagnitude <= (double) ((Vector3) ref line).sqrMagnitude;
  }

  private void sitdown()
  {
    this.state = TitanState.sit;
    this.playAnimation("sit_down");
    this.getdownTime = Random.Range(10f, 30f);
  }

  private void Start()
  {
    this.MultiplayerManager.addTitan(this);
    if (Object.op_Inequality((Object) Minimap.instance, (Object) null))
      Minimap.instance.TrackGameObjectOnMinimap(((Component) this).gameObject, Color.yellow, false, true);
    this.currentCamera = GameObject.Find("MainCamera");
    this.runAnimation = "run_walk";
    this.grabTF = new GameObject();
    ((Object) this.grabTF).name = "titansTmpGrabTF";
    this.head = this.baseTransform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck/head");
    this.neck = this.baseTransform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck");
    this.oldHeadRotation = this.head.rotation;
    if (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.MULTIPLAYER || this.photonView.isMine)
    {
      if (!this.hasSetLevel)
      {
        this.myLevel = Random.Range(0.7f, 3f);
        if (SettingsManager.LegacyGameSettings.TitanSizeEnabled.Value)
          this.myLevel = Random.Range(SettingsManager.LegacyGameSettings.TitanSizeMin.Value, SettingsManager.LegacyGameSettings.TitanSizeMax.Value);
        this.hasSetLevel = true;
      }
      this.spawnPt = this.baseTransform.position;
      this.setmyLevel();
      this.setAbnormalType2(this.abnormalType, false);
      if (Object.op_Equality((Object) this.myHero, (Object) null))
        this.findNearestHero2();
      this.controller = ((Component) this).gameObject.GetComponent<TITAN_CONTROLLER>();
      this.StartCoroutine(this.HandleSpawnCollisionCoroutine(2f, 20f));
    }
    if (this.maxHealth == 0 && SettingsManager.LegacyGameSettings.TitanHealthMode.Value > 0)
    {
      if (SettingsManager.LegacyGameSettings.TitanHealthMode.Value == 1)
        this.maxHealth = this.currentHealth = Random.Range(SettingsManager.LegacyGameSettings.TitanHealthMin.Value, SettingsManager.LegacyGameSettings.TitanHealthMax.Value + 1);
      else if (SettingsManager.LegacyGameSettings.TitanHealthMode.Value == 2)
        this.maxHealth = this.currentHealth = Mathf.Clamp(Mathf.RoundToInt(this.myLevel / 4f * (float) Random.Range(SettingsManager.LegacyGameSettings.TitanHealthMin.Value, SettingsManager.LegacyGameSettings.TitanHealthMax.Value + 1)), SettingsManager.LegacyGameSettings.TitanHealthMin.Value, SettingsManager.LegacyGameSettings.TitanHealthMax.Value);
    }
    this.lagMax = (float) (150.0 + (double) this.myLevel * 3.0);
    this.healthTime = Time.time;
    if (this.currentHealth > 0 && this.photonView.isMine)
      this.photonView.RPC("labelRPC", PhotonTargets.AllBuffered, (object) this.currentHealth, (object) this.maxHealth);
    this.hasExplode = false;
    this.colliderEnabled = true;
    this.isHooked = false;
    this.isLook = false;
    this.isThunderSpear = false;
    this.hasSpawn = true;
    this._hasRunStart = true;
  }

  public void suicide()
  {
    this.netDie();
    if (this.nonAI)
      GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().sendKillInfo(false, string.Empty, true, (string) PhotonNetwork.player.customProperties[(object) PhotonPlayerProperty.name]);
    GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().needChooseSide = true;
    GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().justSuicide = true;
  }

  public void testVisual(bool setCollider)
  {
    if (setCollider)
    {
      foreach (Renderer componentsInChild in ((Component) this).GetComponentsInChildren<Renderer>())
        componentsInChild.material.color = Color.white;
    }
    else
    {
      foreach (Renderer componentsInChild in ((Component) this).GetComponentsInChildren<Renderer>())
        componentsInChild.material.color = Color.black;
    }
  }

  [RPC]
  public void titanGetHit(int viewID, int speed)
  {
    PhotonView view = PhotonView.Find(viewID);
    if (!Object.op_Inequality((Object) view, (Object) null))
      return;
    Vector3 vector3 = Vector3.op_Subtraction(((Component) view).gameObject.transform.position, this.neck.position);
    if ((double) ((Vector3) ref vector3).magnitude >= (double) this.lagMax || this.hasDie || (double) Time.time - (double) this.healthTime <= 0.20000000298023224)
      return;
    this.healthTime = Time.time;
    if (!SettingsManager.LegacyGameSettings.TitanArmorEnabled.Value || speed >= SettingsManager.LegacyGameSettings.TitanArmor.Value)
      this.currentHealth -= speed;
    else if (this.abnormalType == AbnormalType.TYPE_CRAWLER && !SettingsManager.LegacyGameSettings.TitanArmorCrawlerEnabled.Value)
      this.currentHealth -= speed;
    if ((double) this.maxHealth > 0.0)
      this.photonView.RPC("labelRPC", PhotonTargets.AllBuffered, (object) this.currentHealth, (object) this.maxHealth);
    if ((double) this.currentHealth < 0.0)
    {
      if (PhotonNetwork.isMasterClient)
        this.OnTitanDie(view);
      this.photonView.RPC("netDie", PhotonTargets.OthersBuffered);
      if (Object.op_Inequality((Object) this.grabbedTarget, (Object) null))
        this.grabbedTarget.GetPhotonView().RPC("netUngrabbed", PhotonTargets.All);
      this.netDie();
      if (this.nonAI)
        FengGameManagerMKII.instance.titanGetKill(view.owner, speed, (string) PhotonNetwork.player.customProperties[(object) PhotonPlayerProperty.name]);
      else
        FengGameManagerMKII.instance.titanGetKill(view.owner, speed, ((Object) this).name);
    }
    else
      FengGameManagerMKII.instance.photonView.RPC("netShowDamage", view.owner, (object) speed);
  }

  public void toCheckPoint(Vector3 targetPt, float r)
  {
    this.state = TitanState.to_check_point;
    this.targetCheckPt = targetPt;
    this.targetR = r;
    this.crossFade(this.runAnimation, 0.5f);
  }

  public void toPVPCheckPoint(Vector3 targetPt, float r)
  {
    this.state = TitanState.to_pvp_pt;
    this.targetCheckPt = targetPt;
    this.targetR = r;
    this.crossFade(this.runAnimation, 0.5f);
  }

  private void turn(float d)
  {
    this.turnAnimation = this.abnormalType != AbnormalType.TYPE_CRAWLER ? ((double) d <= 0.0 ? "turnaround1" : "turnaround2") : ((double) d <= 0.0 ? "crawler_turnaround_L" : "crawler_turnaround_R");
    this.playAnimation(this.turnAnimation);
    ((Component) this).animation[this.turnAnimation].time = 0.0f;
    d = Mathf.Clamp(d, -120f, 120f);
    this.turnDeg = d;
    Quaternion rotation = ((Component) this).gameObject.transform.rotation;
    this.desDeg = ((Quaternion) ref rotation).eulerAngles.y + this.turnDeg;
    this.state = TitanState.turn;
  }

  public void UpdateHeroDistance()
  {
    if (GameMenu.Paused && IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.MULTIPLAYER || this.myDifficulty < 0 || this.nonAI)
      return;
    if (Object.op_Equality((Object) this.myHero, (Object) null))
    {
      this.myDistance = float.MaxValue;
    }
    else
    {
      Vector2 vector2_1;
      // ISSUE: explicit constructor call
      ((Vector2) ref vector2_1).\u002Ector(this.myHero.transform.position.x, this.myHero.transform.position.z);
      Vector2 vector2_2;
      // ISSUE: explicit constructor call
      ((Vector2) ref vector2_2).\u002Ector(this.baseTransform.position.x, this.baseTransform.position.z);
      this.myDistance = Vector2.Distance(vector2_1, vector2_2);
    }
  }

  public void update2()
  {
    this.UpdateHeroDistance();
    if (GameMenu.Paused && IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE || this.myDifficulty < 0 || IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE && !this.photonView.isMine)
      return;
    this.explode();
    if (!this.nonAI)
    {
      if (this.activeRad < int.MaxValue && (this.state == TitanState.idle || this.state == TitanState.wander || this.state == TitanState.chase))
      {
        if (this.checkPoints.Count > 1)
        {
          if ((double) Vector3.Distance((Vector3) this.checkPoints[0], this.baseTransform.position) > (double) this.activeRad)
            this.toCheckPoint((Vector3) this.checkPoints[0], 10f);
        }
        else if ((double) Vector3.Distance(this.spawnPt, this.baseTransform.position) > (double) this.activeRad)
          this.toCheckPoint(this.spawnPt, 10f);
      }
      if (Object.op_Inequality((Object) this.whoHasTauntMe, (Object) null))
      {
        this.tauntTime -= Time.deltaTime;
        if ((double) this.tauntTime <= 0.0)
          this.whoHasTauntMe = (GameObject) null;
        this.myHero = this.whoHasTauntMe;
        if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER && PhotonNetwork.isMasterClient)
          this.photonView.RPC("setMyTarget", PhotonTargets.Others, (object) this.myHero.GetPhotonView().viewID);
      }
    }
    if (this.hasDie)
    {
      this.dieTime += Time.deltaTime;
      if ((double) this.dieTime > 2.0 && !this.hasDieSteam)
      {
        this.hasDieSteam = true;
        if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
        {
          GameObject gameObject = (GameObject) Object.Instantiate(Resources.Load("FX/FXtitanDie1"));
          gameObject.transform.position = this.baseTransform.Find("Amarture/Core/Controller_Body/hip").position;
          gameObject.transform.localScale = this.baseTransform.localScale;
        }
        else if (this.photonView.isMine)
          PhotonNetwork.Instantiate("FX/FXtitanDie1", this.baseTransform.Find("Amarture/Core/Controller_Body/hip").position, Quaternion.Euler(-90f, 0.0f, 0.0f), 0).transform.localScale = this.baseTransform.localScale;
      }
      if ((double) this.dieTime <= 5.0)
        return;
      if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
      {
        GameObject gameObject = (GameObject) Object.Instantiate(Resources.Load("FX/FXtitanDie"));
        gameObject.transform.position = this.baseTransform.Find("Amarture/Core/Controller_Body/hip").position;
        gameObject.transform.localScale = this.baseTransform.localScale;
        Object.Destroy((Object) ((Component) this).gameObject);
      }
      else
      {
        if (!this.photonView.isMine)
          return;
        PhotonNetwork.Instantiate("FX/FXtitanDie", this.baseTransform.Find("Amarture/Core/Controller_Body/hip").position, Quaternion.Euler(-90f, 0.0f, 0.0f), 0).transform.localScale = this.baseTransform.localScale;
        PhotonNetwork.Destroy(((Component) this).gameObject);
        this.myDifficulty = -1;
      }
    }
    else
    {
      if (this.state == TitanState.hit)
      {
        if ((double) this.hitPause > 0.0)
        {
          this.hitPause -= Time.deltaTime;
          if ((double) this.hitPause <= 0.0)
          {
            this.baseAnimation[this.hitAnimation].speed = 1f;
            this.hitPause = 0.0f;
          }
        }
        if ((double) this.baseAnimation[this.hitAnimation].normalizedTime >= 1.0)
          this.idle();
      }
      if (!this.nonAI)
      {
        if (Object.op_Equality((Object) this.myHero, (Object) null))
          this.findNearestHero2();
        if ((this.state == TitanState.idle || this.state == TitanState.chase || this.state == TitanState.wander) && Object.op_Equality((Object) this.whoHasTauntMe, (Object) null) && Random.Range(0, 100) < 10)
          this.findNearestFacingHero2();
      }
      else
      {
        if ((double) this.stamina < (double) this.maxStamina)
        {
          if (this.baseAnimation.IsPlaying("idle"))
            this.stamina += Time.deltaTime * 30f;
          if (this.baseAnimation.IsPlaying("crawler_idle"))
            this.stamina += Time.deltaTime * 35f;
          if (this.baseAnimation.IsPlaying("run_walk"))
            this.stamina += Time.deltaTime * 10f;
        }
        if (this.baseAnimation.IsPlaying("run_abnormal_1"))
          this.stamina -= Time.deltaTime * 5f;
        if (this.baseAnimation.IsPlaying("crawler_run"))
          this.stamina -= Time.deltaTime * 15f;
        if ((double) this.stamina < 0.0)
          this.stamina = 0.0f;
        if (!GameMenu.Paused)
          GameObject.Find("stamina_titan").transform.localScale = new Vector3(this.stamina, 16f);
      }
      if (this.state == TitanState.laugh)
      {
        if ((double) this.baseAnimation["laugh"].normalizedTime < 1.0)
          return;
        this.idle(2f);
      }
      else if (this.state == TitanState.idle)
      {
        if (this.nonAI)
        {
          if (GameMenu.Paused)
            return;
          this.pt();
          if (this.abnormalType != AbnormalType.TYPE_CRAWLER)
          {
            if (this.controller.isAttackDown && (double) this.stamina > 25.0)
            {
              this.stamina -= 25f;
              this.attack2("combo_1");
            }
            else if (this.controller.isAttackIIDown && (double) this.stamina > 50.0)
            {
              this.stamina -= 50f;
              this.attack2("abnormal_jump");
            }
            else if (this.controller.isJumpDown && (double) this.stamina > 15.0)
            {
              this.stamina -= 15f;
              this.attack2("jumper_0");
            }
          }
          else if (this.controller.isAttackDown && (double) this.stamina > 40.0)
          {
            this.stamina -= 40f;
            this.attack2("crawler_jump_0");
          }
          if (!this.controller.isSuicide)
            return;
          this.suicide();
        }
        else if ((double) this.sbtime > 0.0)
        {
          this.sbtime -= Time.deltaTime;
        }
        else
        {
          if (!this.isAlarm)
          {
            if (this.abnormalType != AbnormalType.TYPE_PUNK && this.abnormalType != AbnormalType.TYPE_CRAWLER && (double) Random.Range(0.0f, 1f) < 0.004999999888241291)
            {
              this.sitdown();
              return;
            }
            if ((double) Random.Range(0.0f, 1f) < 0.019999999552965164)
            {
              this.wander();
              return;
            }
            if ((double) Random.Range(0.0f, 1f) < 0.0099999997764825821)
            {
              this.turn((float) Random.Range(30, 120));
              return;
            }
            if ((double) Random.Range(0.0f, 1f) < 0.0099999997764825821)
            {
              this.turn((float) Random.Range(-30, -120));
              return;
            }
          }
          this.angle = 0.0f;
          this.between2 = 0.0f;
          if ((double) this.myDistance < (double) this.chaseDistance || Object.op_Inequality((Object) this.whoHasTauntMe, (Object) null))
          {
            Vector3 vector3 = Vector3.op_Subtraction(this.myHero.transform.position, this.baseTransform.position);
            this.angle = (float) (-(double) Mathf.Atan2(vector3.z, vector3.x) * 57.295780181884766);
            double angle = (double) this.angle;
            Quaternion rotation = this.baseGameObjectTransform.rotation;
            double num = (double) ((Quaternion) ref rotation).eulerAngles.y - 90.0;
            this.between2 = -Mathf.DeltaAngle((float) angle, (float) num);
            if ((double) this.myDistance >= (double) this.attackDistance)
            {
              if (this.isAlarm || (double) Mathf.Abs(this.between2) < 90.0)
              {
                this.chase();
                return;
              }
              if (!this.isAlarm && (double) this.myDistance < (double) this.chaseDistance * 0.10000000149011612)
              {
                this.chase();
                return;
              }
            }
          }
          if (this.longRangeAttackCheck2())
            return;
          if ((double) this.myDistance < (double) this.chaseDistance)
          {
            if (this.abnormalType == AbnormalType.TYPE_JUMPER && ((double) this.myDistance > (double) this.attackDistance || (double) this.myHero.transform.position.y > (double) this.head.position.y + 4.0 * (double) this.myLevel) && (double) Mathf.Abs(this.between2) < 120.0 && (double) Vector3.Distance(this.baseTransform.position, this.myHero.transform.position) < 1.5 * (double) this.myHero.transform.position.y)
            {
              this.attack2("jumper_0");
              return;
            }
            if (this.abnormalType == AbnormalType.TYPE_CRAWLER && (double) this.myDistance < (double) this.attackDistance * 3.0 && (double) Mathf.Abs(this.between2) < 90.0 && (double) this.myHero.transform.position.y < (double) this.neck.position.y + 30.0 * (double) this.myLevel && (double) this.myHero.transform.position.y > (double) this.neck.position.y + 10.0 * (double) this.myLevel)
            {
              this.attack2("crawler_jump_0");
              return;
            }
          }
          if (this.abnormalType == AbnormalType.TYPE_PUNK && (double) this.myDistance < 90.0 && (double) Mathf.Abs(this.between2) > 90.0)
          {
            if ((double) Random.Range(0.0f, 1f) < 0.40000000596046448)
              this.randomRun(Vector3.op_Addition(this.baseTransform.position, new Vector3(Random.Range(-50f, 50f), Random.Range(-50f, 50f), Random.Range(-50f, 50f))), 10f);
            if ((double) Random.Range(0.0f, 1f) < 0.20000000298023224)
              this.recover();
            else if (Random.Range(0, 2) == 0)
              this.attack2("quick_turn_l");
            else
              this.attack2("quick_turn_r");
          }
          else
          {
            if ((double) this.myDistance < (double) this.attackDistance)
            {
              if (this.abnormalType == AbnormalType.TYPE_CRAWLER)
              {
                if ((double) this.myHero.transform.position.y + 3.0 > (double) this.neck.position.y + 20.0 * (double) this.myLevel || (double) Random.Range(0.0f, 1f) >= 0.10000000149011612)
                  return;
                this.chase();
                return;
              }
              string empty = string.Empty;
              string[] attackStrategy = this.GetAttackStrategy();
              if (attackStrategy != null)
                empty = attackStrategy[Random.Range(0, attackStrategy.Length)];
              if ((this.abnormalType == AbnormalType.TYPE_JUMPER || this.abnormalType == AbnormalType.TYPE_I) && (double) Mathf.Abs(this.between2) > 40.0)
              {
                if (empty.Contains("grab") || empty.Contains("kick") || empty.Contains("slap") || empty.Contains("bite"))
                {
                  if (Random.Range(0, 100) < 30)
                  {
                    this.turn(this.between2);
                    return;
                  }
                }
                else if (Random.Range(0, 100) < 90)
                {
                  this.turn(this.between2);
                  return;
                }
              }
              if (this.executeAttack2(empty))
                return;
              if (this.abnormalType == AbnormalType.NORMAL)
              {
                if (Random.Range(0, 100) < 30 && (double) Mathf.Abs(this.between2) > 45.0)
                {
                  this.turn(this.between2);
                  return;
                }
              }
              else if ((double) Mathf.Abs(this.between2) > 45.0)
              {
                this.turn(this.between2);
                return;
              }
            }
            if (!Object.op_Inequality((Object) this.PVPfromCheckPt, (Object) null))
              return;
            if (this.PVPfromCheckPt.state == CheckPointState.Titan)
            {
              if (Random.Range(0, 100) > 48)
              {
                GameObject chkPtNext = this.PVPfromCheckPt.chkPtNext;
                if (!Object.op_Inequality((Object) chkPtNext, (Object) null) || chkPtNext.GetComponent<PVPcheckPoint>().state == CheckPointState.Titan && Random.Range(0, 100) >= 20)
                  return;
                this.toPVPCheckPoint(chkPtNext.transform.position, (float) (5 + Random.Range(0, 10)));
                this.PVPfromCheckPt = chkPtNext.GetComponent<PVPcheckPoint>();
              }
              else
              {
                GameObject chkPtPrevious = this.PVPfromCheckPt.chkPtPrevious;
                if (!Object.op_Inequality((Object) chkPtPrevious, (Object) null) || chkPtPrevious.GetComponent<PVPcheckPoint>().state == CheckPointState.Titan && Random.Range(0, 100) >= 5)
                  return;
                this.toPVPCheckPoint(chkPtPrevious.transform.position, (float) (5 + Random.Range(0, 10)));
                this.PVPfromCheckPt = chkPtPrevious.GetComponent<PVPcheckPoint>();
              }
            }
            else
              this.toPVPCheckPoint(((Component) this.PVPfromCheckPt).transform.position, (float) (5 + Random.Range(0, 10)));
          }
        }
      }
      else if (this.state == TitanState.attack)
      {
        if (this.attackAnimation == "combo")
        {
          if (this.nonAI)
          {
            if (this.controller.isAttackDown)
              this.nonAIcombo = true;
            if (!this.nonAIcombo && (double) this.baseAnimation["attack_" + this.attackAnimation].normalizedTime >= 0.38499999046325684)
            {
              this.idle();
              return;
            }
          }
          if ((double) this.baseAnimation["attack_" + this.attackAnimation].normalizedTime >= 0.10999999940395355 && (double) this.baseAnimation["attack_" + this.attackAnimation].normalizedTime <= 0.15999999642372131)
          {
            GameObject gameObject = this.checkIfHitHand(this.baseTransform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R/forearm_R/hand_R/hand_R_001"));
            if (Object.op_Inequality((Object) gameObject, (Object) null))
            {
              Vector3 position = this.baseTransform.Find("Amarture/Core/Controller_Body/hip/spine/chest").position;
              switch (IN_GAME_MAIN_CAMERA.gametype)
              {
                case GAMETYPE.SINGLE:
                  gameObject.GetComponent<HERO>().die(Vector3.op_Multiply(Vector3.op_Multiply(Vector3.op_Subtraction(gameObject.transform.position, position), 15f), this.myLevel), false);
                  break;
                case GAMETYPE.MULTIPLAYER:
                  if (this.photonView.isMine && !gameObject.GetComponent<HERO>().HasDied())
                  {
                    gameObject.GetComponent<HERO>().markDie();
                    object[] objArray = new object[5]
                    {
                      (object) Vector3.op_Multiply(Vector3.op_Multiply(Vector3.op_Subtraction(gameObject.transform.position, position), 15f), this.myLevel),
                      (object) false,
                      (object) (!this.nonAI ? -1 : this.photonView.viewID),
                      (object) ((Object) this).name,
                      (object) true
                    };
                    gameObject.GetComponent<HERO>().photonView.RPC("netDie", PhotonTargets.All, objArray);
                    break;
                  }
                  break;
              }
            }
          }
          if ((double) this.baseAnimation["attack_" + this.attackAnimation].normalizedTime >= 0.27000001072883606 && (double) this.baseAnimation["attack_" + this.attackAnimation].normalizedTime <= 0.31999999284744263)
          {
            GameObject gameObject = this.checkIfHitHand(this.baseTransform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_L/upper_arm_L/forearm_L/hand_L/hand_L_001"));
            if (Object.op_Inequality((Object) gameObject, (Object) null))
            {
              Vector3 position = this.baseTransform.Find("Amarture/Core/Controller_Body/hip/spine/chest").position;
              switch (IN_GAME_MAIN_CAMERA.gametype)
              {
                case GAMETYPE.SINGLE:
                  gameObject.GetComponent<HERO>().die(Vector3.op_Multiply(Vector3.op_Multiply(Vector3.op_Subtraction(gameObject.transform.position, position), 15f), this.myLevel), false);
                  break;
                case GAMETYPE.MULTIPLAYER:
                  if (this.photonView.isMine && !gameObject.GetComponent<HERO>().HasDied())
                  {
                    gameObject.GetComponent<HERO>().markDie();
                    object[] objArray = new object[5]
                    {
                      (object) Vector3.op_Multiply(Vector3.op_Multiply(Vector3.op_Subtraction(gameObject.transform.position, position), 15f), this.myLevel),
                      (object) false,
                      (object) (!this.nonAI ? -1 : this.photonView.viewID),
                      (object) ((Object) this).name,
                      (object) true
                    };
                    gameObject.GetComponent<HERO>().photonView.RPC("netDie", PhotonTargets.All, objArray);
                    break;
                  }
                  break;
              }
            }
          }
        }
        if ((double) this.attackCheckTimeA != 0.0 && (double) this.baseAnimation["attack_" + this.attackAnimation].normalizedTime >= (double) this.attackCheckTimeA && (double) this.baseAnimation["attack_" + this.attackAnimation].normalizedTime <= (double) this.attackCheckTimeB)
        {
          if (this.leftHandAttack)
          {
            GameObject gameObject = this.checkIfHitHand(this.baseTransform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_L/upper_arm_L/forearm_L/hand_L/hand_L_001"));
            if (Object.op_Inequality((Object) gameObject, (Object) null))
            {
              Vector3 position = this.baseTransform.Find("Amarture/Core/Controller_Body/hip/spine/chest").position;
              switch (IN_GAME_MAIN_CAMERA.gametype)
              {
                case GAMETYPE.SINGLE:
                  gameObject.GetComponent<HERO>().die(Vector3.op_Multiply(Vector3.op_Multiply(Vector3.op_Subtraction(gameObject.transform.position, position), 15f), this.myLevel), false);
                  break;
                case GAMETYPE.MULTIPLAYER:
                  if (this.photonView.isMine && !gameObject.GetComponent<HERO>().HasDied())
                  {
                    gameObject.GetComponent<HERO>().markDie();
                    object[] objArray = new object[5]
                    {
                      (object) Vector3.op_Multiply(Vector3.op_Multiply(Vector3.op_Subtraction(gameObject.transform.position, position), 15f), this.myLevel),
                      (object) false,
                      (object) (!this.nonAI ? -1 : this.photonView.viewID),
                      (object) ((Object) this).name,
                      (object) true
                    };
                    gameObject.GetComponent<HERO>().photonView.RPC("netDie", PhotonTargets.All, objArray);
                    break;
                  }
                  break;
              }
            }
          }
          else
          {
            GameObject gameObject = this.checkIfHitHand(this.baseTransform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R/forearm_R/hand_R/hand_R_001"));
            if (Object.op_Inequality((Object) gameObject, (Object) null))
            {
              Vector3 position = this.baseTransform.Find("Amarture/Core/Controller_Body/hip/spine/chest").position;
              switch (IN_GAME_MAIN_CAMERA.gametype)
              {
                case GAMETYPE.SINGLE:
                  gameObject.GetComponent<HERO>().die(Vector3.op_Multiply(Vector3.op_Multiply(Vector3.op_Subtraction(gameObject.transform.position, position), 15f), this.myLevel), false);
                  break;
                case GAMETYPE.MULTIPLAYER:
                  if (this.photonView.isMine && !gameObject.GetComponent<HERO>().HasDied())
                  {
                    gameObject.GetComponent<HERO>().markDie();
                    object[] objArray = new object[5]
                    {
                      (object) Vector3.op_Multiply(Vector3.op_Multiply(Vector3.op_Subtraction(gameObject.transform.position, position), 15f), this.myLevel),
                      (object) false,
                      (object) (!this.nonAI ? -1 : this.photonView.viewID),
                      (object) ((Object) this).name,
                      (object) true
                    };
                    gameObject.GetComponent<HERO>().photonView.RPC("netDie", PhotonTargets.All, objArray);
                    break;
                  }
                  break;
              }
            }
          }
        }
        if (!this.attacked && (double) this.attackCheckTime != 0.0 && (double) this.baseAnimation["attack_" + this.attackAnimation].normalizedTime >= (double) this.attackCheckTime)
        {
          this.attacked = true;
          this.fxPosition = this.baseTransform.Find("ap_" + this.attackAnimation).position;
          GameObject gameObject = IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.MULTIPLAYER || !this.photonView.isMine ? (GameObject) Object.Instantiate(Resources.Load("FX/" + this.fxName), this.fxPosition, this.fxRotation) : PhotonNetwork.Instantiate("FX/" + this.fxName, this.fxPosition, this.fxRotation, 0);
          if (this.nonAI)
          {
            gameObject.transform.localScale = Vector3.op_Multiply(this.baseTransform.localScale, 1.5f);
            if (Object.op_Inequality((Object) gameObject.GetComponent<EnemyfxIDcontainer>(), (Object) null))
              gameObject.GetComponent<EnemyfxIDcontainer>().myOwnerViewID = this.photonView.viewID;
          }
          else
            gameObject.transform.localScale = this.baseTransform.localScale;
          if (Object.op_Inequality((Object) gameObject.GetComponent<EnemyfxIDcontainer>(), (Object) null))
            gameObject.GetComponent<EnemyfxIDcontainer>().titanName = ((Object) this).name;
          float num = Mathf.Min(1f, (float) (1.0 - (double) Vector3.Distance(this.currentCamera.transform.position, gameObject.transform.position) * 0.05000000074505806));
          this.currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().startShake(num, num);
        }
        if (this.attackAnimation == "throw")
        {
          if (!this.attacked && (double) this.baseAnimation["attack_" + this.attackAnimation].normalizedTime >= 0.10999999940395355)
          {
            this.attacked = true;
            Transform transform1 = this.baseTransform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R/forearm_R/hand_R/hand_R_001");
            this.throwRock = IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.MULTIPLAYER || !this.photonView.isMine ? (GameObject) Object.Instantiate(Resources.Load("FX/rockThrow"), transform1.position, transform1.rotation) : PhotonNetwork.Instantiate("FX/rockThrow", transform1.position, transform1.rotation, 0);
            this.throwRock.transform.localScale = this.baseTransform.localScale;
            Transform transform2 = this.throwRock.transform;
            transform2.position = Vector3.op_Subtraction(transform2.position, Vector3.op_Multiply(Vector3.op_Multiply(this.throwRock.transform.forward, 2.5f), this.myLevel));
            if (Object.op_Inequality((Object) this.throwRock.GetComponent<EnemyfxIDcontainer>(), (Object) null))
            {
              if (this.nonAI)
                this.throwRock.GetComponent<EnemyfxIDcontainer>().myOwnerViewID = this.photonView.viewID;
              this.throwRock.GetComponent<EnemyfxIDcontainer>().titanName = ((Object) this).name;
            }
            this.throwRock.transform.parent = transform1;
            if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER && this.photonView.isMine)
            {
              object[] objArray = new object[4]
              {
                (object) this.photonView.viewID,
                (object) this.baseTransform.localScale,
                (object) this.throwRock.transform.localPosition,
                (object) this.myLevel
              };
              this.throwRock.GetPhotonView().RPC("initRPC", PhotonTargets.Others, objArray);
            }
          }
          if ((double) this.baseAnimation["attack_" + this.attackAnimation].normalizedTime >= 0.10999999940395355)
            this.baseGameObjectTransform.rotation = Quaternion.Euler(0.0f, Mathf.Atan2(this.myHero.transform.position.x - this.baseTransform.position.x, this.myHero.transform.position.z - this.baseTransform.position.z) * 57.29578f, 0.0f);
          if (Object.op_Inequality((Object) this.throwRock, (Object) null) && (double) this.baseAnimation["attack_" + this.attackAnimation].normalizedTime >= 0.62000000476837158)
          {
            float num1 = 1f;
            float num2 = -20f;
            Vector3 v1;
            if (Object.op_Inequality((Object) this.myHero, (Object) null))
            {
              v1 = Vector3.op_Addition(Vector3.op_Division(Vector3.op_Subtraction(this.myHero.transform.position, this.throwRock.transform.position), num1), this.myHero.rigidbody.velocity);
              float num3 = this.myHero.transform.position.y + 2f * this.myLevel - this.throwRock.transform.position.y;
              // ISSUE: explicit constructor call
              ((Vector3) ref v1).\u002Ector(v1.x, (float) ((double) num3 / (double) num1 - 0.5 * (double) num2 * (double) num1), v1.z);
            }
            else
              v1 = Vector3.op_Addition(Vector3.op_Multiply(this.baseTransform.forward, 60f), Vector3.op_Multiply(Vector3.up, 10f));
            this.throwRock.GetComponent<RockThrow>().launch(v1);
            this.throwRock.transform.parent = (Transform) null;
            this.throwRock = (GameObject) null;
          }
        }
        if (this.attackAnimation == "jumper_0" || this.attackAnimation == "crawler_jump_0")
        {
          if (!this.attacked)
          {
            if ((double) this.baseAnimation["attack_" + this.attackAnimation].normalizedTime >= 0.68000000715255737)
            {
              this.attacked = true;
              if (Object.op_Equality((Object) this.myHero, (Object) null) || this.nonAI)
              {
                float num4 = 120f;
                Vector3 vector3 = Vector3.op_Addition(Vector3.op_Multiply(this.baseTransform.forward, this.speed), Vector3.op_Multiply(Vector3.up, num4));
                if (this.nonAI && this.abnormalType == AbnormalType.TYPE_CRAWLER)
                {
                  float num5 = 100f;
                  vector3 = Vector3.op_Addition(Vector3.op_Multiply(this.baseTransform.forward, Mathf.Min(this.speed * 2.5f, 100f)), Vector3.op_Multiply(Vector3.up, num5));
                }
                this.baseRigidBody.velocity = vector3;
              }
              else
              {
                double y1 = (double) this.myHero.rigidbody.velocity.y;
                float num6 = -20f;
                float gravity = this.gravity;
                float y2 = this.neck.position.y;
                float num7 = (float) (((double) num6 - (double) gravity) * 0.5);
                float num8 = (float) y1;
                float num9 = this.myHero.transform.position.y - y2;
                float num10 = Mathf.Abs((float) (((double) Mathf.Sqrt((float) ((double) num8 * (double) num8 - 4.0 * (double) num7 * (double) num9)) - (double) num8) / (2.0 * (double) num7)));
                Vector3 vector3_1 = Vector3.op_Addition(Vector3.op_Addition(this.myHero.transform.position, Vector3.op_Multiply(this.myHero.rigidbody.velocity, num10)), Vector3.op_Multiply(Vector3.op_Multiply(Vector3.op_Multiply(Vector3.op_Multiply(Vector3.up, 0.5f), num6), num10), num10));
                float y3 = vector3_1.y;
                if ((double) num9 < 0.0 || (double) y3 - (double) y2 < 0.0)
                {
                  float num11 = 60f;
                  this.baseRigidBody.velocity = Vector3.op_Addition(Vector3.op_Multiply(this.baseTransform.forward, Mathf.Min(this.speed * 2.5f, 100f)), Vector3.op_Multiply(Vector3.up, num11));
                  return;
                }
                float num12 = Mathf.Max(30f, this.gravity * Mathf.Sqrt(2f * (y3 - y2) / this.gravity));
                Vector3 vector3_2 = Vector3.op_Division(Vector3.op_Subtraction(vector3_1, this.baseTransform.position), num10);
                this.abnorma_jump_bite_horizon_v = new Vector3(vector3_2.x, 0.0f, vector3_2.z);
                Vector3 velocity = this.baseRigidBody.velocity;
                this.baseRigidBody.AddForce(Vector3.op_Subtraction(new Vector3(this.abnorma_jump_bite_horizon_v.x, velocity.y, this.abnorma_jump_bite_horizon_v.z), velocity), (ForceMode) 2);
                this.baseRigidBody.AddForce(Vector3.op_Multiply(Vector3.up, num12), (ForceMode) 2);
                Vector2.Angle(new Vector2(this.baseTransform.position.x, this.baseTransform.position.z), new Vector2(this.myHero.transform.position.x, this.myHero.transform.position.z));
                this.baseGameObjectTransform.rotation = Quaternion.Euler(0.0f, Mathf.Atan2(this.myHero.transform.position.x - this.baseTransform.position.x, this.myHero.transform.position.z - this.baseTransform.position.z) * 57.29578f, 0.0f);
              }
            }
            else
              this.baseRigidBody.velocity = Vector3.zero;
          }
          if ((double) this.baseAnimation["attack_" + this.attackAnimation].normalizedTime < 1.0)
            return;
          Debug.DrawLine(Vector3.op_Addition(this.baseTransform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck/head").position, Vector3.op_Multiply(Vector3.op_Multiply(Vector3.up, 1.5f), this.myLevel)), Vector3.op_Addition(Vector3.op_Addition(this.baseTransform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck/head").position, Vector3.op_Multiply(Vector3.op_Multiply(Vector3.up, 1.5f), this.myLevel)), Vector3.op_Multiply(Vector3.op_Multiply(Vector3.up, 3f), this.myLevel)), Color.green);
          Debug.DrawLine(Vector3.op_Addition(this.baseTransform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck/head").position, Vector3.op_Multiply(Vector3.op_Multiply(Vector3.up, 1.5f), this.myLevel)), Vector3.op_Addition(Vector3.op_Addition(this.baseTransform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck/head").position, Vector3.op_Multiply(Vector3.op_Multiply(Vector3.up, 1.5f), this.myLevel)), Vector3.op_Multiply(Vector3.op_Multiply(Vector3.forward, 3f), this.myLevel)), Color.green);
          GameObject gameObject = this.checkIfHitHead(this.head, 3f);
          if (Object.op_Inequality((Object) gameObject, (Object) null))
          {
            Vector3 position = this.baseTransform.Find("Amarture/Core/Controller_Body/hip/spine/chest").position;
            switch (IN_GAME_MAIN_CAMERA.gametype)
            {
              case GAMETYPE.SINGLE:
                gameObject.GetComponent<HERO>().die(Vector3.op_Multiply(Vector3.op_Multiply(Vector3.op_Subtraction(gameObject.transform.position, position), 15f), this.myLevel), false);
                break;
              case GAMETYPE.MULTIPLAYER:
                if (this.photonView.isMine && !gameObject.GetComponent<HERO>().HasDied())
                {
                  gameObject.GetComponent<HERO>().markDie();
                  object[] objArray = new object[5]
                  {
                    (object) Vector3.op_Multiply(Vector3.op_Multiply(Vector3.op_Subtraction(gameObject.transform.position, position), 15f), this.myLevel),
                    (object) true,
                    (object) (!this.nonAI ? -1 : this.photonView.viewID),
                    (object) ((Object) this).name,
                    (object) true
                  };
                  gameObject.GetComponent<HERO>().photonView.RPC("netDie", PhotonTargets.All, objArray);
                  break;
                }
                break;
            }
            this.attackAnimation = this.abnormalType != AbnormalType.TYPE_CRAWLER ? "jumper_1" : "crawler_jump_1";
            this.playAnimation("attack_" + this.attackAnimation);
          }
          if ((double) Mathf.Abs(this.baseRigidBody.velocity.y) >= 0.5 && (double) this.baseRigidBody.velocity.y >= 0.0 && !this.IsGrounded())
            return;
          this.attackAnimation = this.abnormalType != AbnormalType.TYPE_CRAWLER ? "jumper_1" : "crawler_jump_1";
          this.playAnimation("attack_" + this.attackAnimation);
        }
        else if (this.attackAnimation == "jumper_1" || this.attackAnimation == "crawler_jump_1")
        {
          if ((double) this.baseAnimation["attack_" + this.attackAnimation].normalizedTime < 1.0 || !this.grounded)
            return;
          this.attackAnimation = this.abnormalType != AbnormalType.TYPE_CRAWLER ? "jumper_2" : "crawler_jump_2";
          this.crossFade("attack_" + this.attackAnimation, 0.1f);
          this.fxPosition = this.baseTransform.position;
          GameObject gameObject = IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.MULTIPLAYER || !this.photonView.isMine ? (GameObject) Object.Instantiate(Resources.Load("FX/boom2"), this.fxPosition, this.fxRotation) : PhotonNetwork.Instantiate("FX/boom2", this.fxPosition, this.fxRotation, 0);
          gameObject.transform.localScale = Vector3.op_Multiply(this.baseTransform.localScale, 1.6f);
          float num = Mathf.Min(1f, (float) (1.0 - (double) Vector3.Distance(this.currentCamera.transform.position, gameObject.transform.position) * 0.05000000074505806));
          this.currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().startShake(num, num);
        }
        else if (this.attackAnimation == "jumper_2" || this.attackAnimation == "crawler_jump_2")
        {
          if ((double) this.baseAnimation["attack_" + this.attackAnimation].normalizedTime < 1.0)
            return;
          this.idle();
        }
        else if (this.baseAnimation.IsPlaying("tired"))
        {
          if ((double) this.baseAnimation["tired"].normalizedTime < 1.0 + (double) Mathf.Max(this.attackEndWait * 2f, 3f))
            return;
          this.idle(Random.Range(this.attackWait - 1f, 3f));
        }
        else
        {
          if ((double) this.baseAnimation["attack_" + this.attackAnimation].normalizedTime < 1.0 + (double) this.attackEndWait)
            return;
          if (this.nextAttackAnimation != null)
            this.attack2(this.nextAttackAnimation);
          else if (this.attackAnimation == "quick_turn_l" || this.attackAnimation == "quick_turn_r")
          {
            Transform baseTransform = this.baseTransform;
            Quaternion rotation1 = this.baseTransform.rotation;
            double x = (double) ((Quaternion) ref rotation1).eulerAngles.x;
            Quaternion rotation2 = this.baseTransform.rotation;
            double num = (double) ((Quaternion) ref rotation2).eulerAngles.y + 180.0;
            Quaternion rotation3 = this.baseTransform.rotation;
            double z = (double) ((Quaternion) ref rotation3).eulerAngles.z;
            Quaternion quaternion = Quaternion.Euler((float) x, (float) num, (float) z);
            baseTransform.rotation = quaternion;
            this.idle(Random.Range(0.5f, 1f));
            this.playAnimation("idle");
          }
          else if (this.abnormalType == AbnormalType.TYPE_I || this.abnormalType == AbnormalType.TYPE_JUMPER)
          {
            ++this.attackCount;
            if (this.attackCount > 3 && this.attackAnimation == "abnormal_getup")
            {
              this.attackCount = 0;
              this.crossFade("tired", 0.5f);
            }
            else
              this.idle(Random.Range(this.attackWait - 1f, 3f));
          }
          else
            this.idle(Random.Range(this.attackWait - 1f, 3f));
        }
      }
      else if (this.state == TitanState.grab)
      {
        if ((double) this.baseAnimation["grab_" + this.attackAnimation].normalizedTime >= (double) this.attackCheckTimeA && (double) this.baseAnimation["grab_" + this.attackAnimation].normalizedTime <= (double) this.attackCheckTimeB && Object.op_Equality((Object) this.grabbedTarget, (Object) null))
        {
          GameObject grabTarget = this.checkIfHitHand(this.currentGrabHand);
          if (Object.op_Inequality((Object) grabTarget, (Object) null))
          {
            if (this.isGrabHandLeft)
            {
              this.eatSetL(grabTarget);
              this.grabbedTarget = grabTarget;
            }
            else
            {
              this.eatSet(grabTarget);
              this.grabbedTarget = grabTarget;
            }
          }
        }
        if ((double) this.baseAnimation["grab_" + this.attackAnimation].normalizedTime < 1.0)
          return;
        if (Object.op_Inequality((Object) this.grabbedTarget, (Object) null))
          this.eat();
        else
          this.idle(Random.Range(this.attackWait - 1f, 2f));
      }
      else if (this.state == TitanState.eat)
      {
        if (!this.attacked && (double) this.baseAnimation[this.attackAnimation].normalizedTime >= 0.47999998927116394)
        {
          this.attacked = true;
          this.justEatHero(this.grabbedTarget, this.currentGrabHand);
        }
        Object.op_Equality((Object) this.grabbedTarget, (Object) null);
        if ((double) this.baseAnimation[this.attackAnimation].normalizedTime < 1.0)
          return;
        this.idle();
      }
      else if (this.state == TitanState.chase)
      {
        if (Object.op_Equality((Object) this.myHero, (Object) null))
        {
          this.idle();
        }
        else
        {
          if (this.longRangeAttackCheck2())
            return;
          if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.PVP_CAPTURE && Object.op_Inequality((Object) this.PVPfromCheckPt, (Object) null) && (double) this.myDistance > (double) this.chaseDistance)
            this.idle();
          else if (this.abnormalType == AbnormalType.TYPE_CRAWLER)
          {
            Vector3 vector3 = Vector3.op_Subtraction(this.myHero.transform.position, this.baseTransform.position);
            double num13 = -(double) Mathf.Atan2(vector3.z, vector3.x) * 57.295780181884766;
            Quaternion rotation = this.baseGameObjectTransform.rotation;
            double num14 = (double) ((Quaternion) ref rotation).eulerAngles.y - 90.0;
            float num15 = -Mathf.DeltaAngle((float) num13, (float) num14);
            if ((double) this.myDistance < (double) this.attackDistance * 3.0 && (double) Random.Range(0.0f, 1f) < 0.10000000149011612 && (double) Mathf.Abs(num15) < 90.0 && (double) this.myHero.transform.position.y < (double) this.neck.position.y + 30.0 * (double) this.myLevel && (double) this.myHero.transform.position.y > (double) this.neck.position.y + 10.0 * (double) this.myLevel)
            {
              this.attack2("crawler_jump_0");
            }
            else
            {
              GameObject gameObject = this.checkIfHitCrawlerMouth(this.head, 2.2f);
              if (Object.op_Inequality((Object) gameObject, (Object) null))
              {
                Vector3 position = this.baseTransform.Find("Amarture/Core/Controller_Body/hip/spine/chest").position;
                switch (IN_GAME_MAIN_CAMERA.gametype)
                {
                  case GAMETYPE.SINGLE:
                    gameObject.GetComponent<HERO>().die(Vector3.op_Multiply(Vector3.op_Multiply(Vector3.op_Subtraction(gameObject.transform.position, position), 15f), this.myLevel), false);
                    break;
                  case GAMETYPE.MULTIPLAYER:
                    if (this.photonView.isMine)
                    {
                      if (Object.op_Inequality((Object) gameObject.GetComponent<TITAN_EREN>(), (Object) null))
                      {
                        gameObject.GetComponent<TITAN_EREN>().hitByTitan();
                        break;
                      }
                      if (!gameObject.GetComponent<HERO>().HasDied())
                      {
                        gameObject.GetComponent<HERO>().markDie();
                        object[] objArray = new object[5]
                        {
                          (object) Vector3.op_Multiply(Vector3.op_Multiply(Vector3.op_Subtraction(gameObject.transform.position, position), 15f), this.myLevel),
                          (object) true,
                          (object) (!this.nonAI ? -1 : this.photonView.viewID),
                          (object) ((Object) this).name,
                          (object) true
                        };
                        gameObject.GetComponent<HERO>().photonView.RPC("netDie", PhotonTargets.All, objArray);
                        break;
                      }
                      break;
                    }
                    break;
                }
              }
              if ((double) this.myDistance >= (double) this.attackDistance || (double) Random.Range(0.0f, 1f) >= 0.019999999552965164)
                return;
              this.idle(Random.Range(0.05f, 0.2f));
            }
          }
          else if (this.abnormalType == AbnormalType.TYPE_JUMPER && ((double) this.myDistance > (double) this.attackDistance && (double) this.myHero.transform.position.y > (double) this.head.position.y + 4.0 * (double) this.myLevel || (double) this.myHero.transform.position.y > (double) this.head.position.y + 4.0 * (double) this.myLevel) && (double) Vector3.Distance(this.baseTransform.position, this.myHero.transform.position) < 1.5 * (double) this.myHero.transform.position.y)
          {
            this.attack2("jumper_0");
          }
          else
          {
            if ((double) this.myDistance >= (double) this.attackDistance)
              return;
            this.idle(Random.Range(0.05f, 0.2f));
          }
        }
      }
      else if (this.state == TitanState.wander)
      {
        if ((double) this.myDistance < (double) this.chaseDistance || Object.op_Inequality((Object) this.whoHasTauntMe, (Object) null))
        {
          Vector3 vector3 = Vector3.op_Subtraction(this.myHero.transform.position, this.baseTransform.position);
          double num16 = -(double) Mathf.Atan2(vector3.z, vector3.x) * 57.295780181884766;
          Quaternion rotation = this.baseGameObjectTransform.rotation;
          double num17 = (double) ((Quaternion) ref rotation).eulerAngles.y - 90.0;
          float num18 = -Mathf.DeltaAngle((float) num16, (float) num17);
          if (this.isAlarm || (double) Mathf.Abs(num18) < 90.0)
          {
            this.chase();
            return;
          }
          if (!this.isAlarm && (double) this.myDistance < (double) this.chaseDistance * 0.10000000149011612)
          {
            this.chase();
            return;
          }
        }
        if ((double) Random.Range(0.0f, 1f) >= 0.0099999997764825821)
          return;
        this.idle();
      }
      else if (this.state == TitanState.turn)
      {
        this.baseGameObjectTransform.rotation = Quaternion.Lerp(this.baseGameObjectTransform.rotation, Quaternion.Euler(0.0f, this.desDeg, 0.0f), (float) ((double) Time.deltaTime * (double) Mathf.Abs(this.turnDeg) * 0.014999999664723873));
        if ((double) this.baseAnimation[this.turnAnimation].normalizedTime < 1.0)
          return;
        this.idle();
      }
      else if (this.state == TitanState.hit_eye)
      {
        if (this.baseAnimation.IsPlaying("sit_hit_eye") && (double) this.baseAnimation["sit_hit_eye"].normalizedTime >= 1.0)
        {
          this.remainSitdown();
        }
        else
        {
          if (!this.baseAnimation.IsPlaying("hit_eye") || (double) this.baseAnimation["hit_eye"].normalizedTime < 1.0)
            return;
          if (this.nonAI)
            this.idle();
          else
            this.attack2("combo_1");
        }
      }
      else if (this.state == TitanState.to_check_point)
      {
        if (this.checkPoints.Count <= 0 && (double) this.myDistance < (double) this.attackDistance)
        {
          string empty = string.Empty;
          string[] attackStrategy = this.GetAttackStrategy();
          if (attackStrategy != null)
            empty = attackStrategy[Random.Range(0, attackStrategy.Length)];
          if (this.executeAttack2(empty))
            return;
        }
        if ((double) Vector3.Distance(this.baseTransform.position, this.targetCheckPt) >= (double) this.targetR)
          return;
        if (this.checkPoints.Count > 0)
        {
          if (this.checkPoints.Count == 1)
          {
            if (IN_GAME_MAIN_CAMERA.gamemode != GAMEMODE.BOSS_FIGHT_CT)
              return;
            this.MultiplayerManager.gameLose2();
            this.checkPoints = new ArrayList();
            this.idle();
          }
          else
          {
            if (this.checkPoints.Count == 4)
              this.MultiplayerManager.sendChatContentInfo("<color=#A8FF24>*WARNING!* An abnormal titan is approaching the north gate!</color>");
            this.targetCheckPt = (Vector3) this.checkPoints[0];
            this.checkPoints.RemoveAt(0);
          }
        }
        else
          this.idle();
      }
      else if (this.state == TitanState.to_pvp_pt)
      {
        if ((double) this.myDistance < (double) this.chaseDistance * 0.699999988079071)
          this.chase();
        if ((double) Vector3.Distance(this.baseTransform.position, this.targetCheckPt) >= (double) this.targetR)
          return;
        this.idle();
      }
      else if (this.state == TitanState.random_run)
      {
        this.random_run_time -= Time.deltaTime;
        if ((double) Vector3.Distance(this.baseTransform.position, this.targetCheckPt) >= (double) this.targetR && (double) this.random_run_time > 0.0)
          return;
        this.idle();
      }
      else if (this.state == TitanState.down)
      {
        this.getdownTime -= Time.deltaTime;
        if (this.baseAnimation.IsPlaying("sit_hunt_down") && (double) this.baseAnimation["sit_hunt_down"].normalizedTime >= 1.0)
          this.playAnimation("sit_idle");
        if ((double) this.getdownTime <= 0.0)
          this.crossFadeIfNotPlaying("sit_getup", 0.1f);
        if (!this.baseAnimation.IsPlaying("sit_getup") || (double) this.baseAnimation["sit_getup"].normalizedTime < 1.0)
          return;
        this.idle();
      }
      else if (this.state == TitanState.sit)
      {
        this.getdownTime -= Time.deltaTime;
        this.angle = 0.0f;
        this.between2 = 0.0f;
        if ((double) this.myDistance < (double) this.chaseDistance || Object.op_Inequality((Object) this.whoHasTauntMe, (Object) null))
        {
          if ((double) this.myDistance < 50.0)
          {
            this.isAlarm = true;
          }
          else
          {
            Vector3 vector3 = Vector3.op_Subtraction(this.myHero.transform.position, this.baseTransform.position);
            this.angle = (float) (-(double) Mathf.Atan2(vector3.z, vector3.x) * 57.295780181884766);
            double angle = (double) this.angle;
            Quaternion rotation = this.baseGameObjectTransform.rotation;
            double num = (double) ((Quaternion) ref rotation).eulerAngles.y - 90.0;
            this.between2 = -Mathf.DeltaAngle((float) angle, (float) num);
            if ((double) Mathf.Abs(this.between2) < 100.0)
              this.isAlarm = true;
          }
        }
        if (this.baseAnimation.IsPlaying("sit_down") && (double) this.baseAnimation["sit_down"].normalizedTime >= 1.0)
          this.playAnimation("sit_idle");
        if (((double) this.getdownTime <= 0.0 || this.isAlarm) && this.baseAnimation.IsPlaying("sit_idle"))
          this.crossFadeIfNotPlaying("sit_getup", 0.1f);
        if (!this.baseAnimation.IsPlaying("sit_getup") || (double) this.baseAnimation["sit_getup"].normalizedTime < 1.0)
          return;
        this.idle();
      }
      else
      {
        if (this.state != TitanState.recover)
          return;
        this.getdownTime -= Time.deltaTime;
        if ((double) this.getdownTime <= 0.0)
          this.idle();
        if (!this.baseAnimation.IsPlaying("idle_recovery") || (double) this.baseAnimation["idle_recovery"].normalizedTime < 1.0)
          return;
        this.idle();
      }
    }
  }

  public void updateCollider()
  {
    if (this.colliderEnabled)
    {
      if (this.isHooked || this.myTitanTrigger.isCollide || this.isLook || this.isThunderSpear)
        return;
      foreach (Collider baseCollider in this.baseColliders)
      {
        if (Object.op_Inequality((Object) baseCollider, (Object) null))
          baseCollider.enabled = false;
      }
      this.colliderEnabled = false;
    }
    else
    {
      if (!this.isHooked && !this.myTitanTrigger.isCollide && !this.isLook && !this.isThunderSpear)
        return;
      foreach (Collider baseCollider in this.baseColliders)
      {
        if (Object.op_Inequality((Object) baseCollider, (Object) null))
          baseCollider.enabled = true;
      }
      this.colliderEnabled = true;
    }
  }

  public void updateLabel()
  {
    if (!Object.op_Inequality((Object) this.healthLabel, (Object) null) || !this.healthLabel.GetComponent<UILabel>().isVisible)
      return;
    this.healthLabel.transform.LookAt(Vector3.op_Subtraction(Vector3.op_Multiply(2f, this.healthLabel.transform.position), ((Component) Camera.main).transform.position));
  }

  private void wander(float sbtime = 0.0f)
  {
    this.state = TitanState.wander;
    this.crossFade(this.runAnimation, 0.5f);
  }
}
