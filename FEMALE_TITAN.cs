// Decompiled with JetBrains decompiler
// Type: FEMALE_TITAN
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using CustomSkins;
using Photon;
using Settings;
using System;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;

public class FEMALE_TITAN : MonoBehaviour
{
  private Vector3 abnorma_jump_bite_horizon_v;
  public int AnkleLHP = 200;
  private int AnkleLHPMAX = 200;
  public int AnkleRHP = 200;
  private int AnkleRHPMAX = 200;
  private string attackAnimation;
  private float attackCheckTime;
  private float attackCheckTimeA;
  private float attackCheckTimeB;
  private bool attackChkOnce;
  public float attackDistance = 13f;
  private bool attacked;
  public float attackWait = 1f;
  private float attention = 10f;
  public GameObject bottomObject;
  public float chaseDistance = 80f;
  private Transform checkHitCapsuleEnd;
  private Vector3 checkHitCapsuleEndOld;
  private float checkHitCapsuleR;
  private Transform checkHitCapsuleStart;
  public GameObject currentCamera;
  private Transform currentGrabHand;
  private float desDeg;
  private float dieTime;
  private GameObject eren;
  private string fxName;
  private Vector3 fxPosition;
  private Quaternion fxRotation;
  private GameObject grabbedTarget;
  public GameObject grabTF;
  private float gravity = 120f;
  private bool grounded;
  public bool hasDie;
  private bool hasDieSteam;
  public bool hasspawn;
  public GameObject healthLabel;
  public float healthTime;
  private string hitAnimation;
  private bool isAttackMoveByCore;
  private bool isGrabHandLeft;
  public float lagMax;
  public int maxHealth;
  public float maxVelocityChange = 80f;
  public static float minusDistance = 99999f;
  public static GameObject minusDistanceEnemy;
  public float myDistance;
  public GameObject myHero;
  public int NapeArmor = 1000;
  private bool needFreshCorePosition;
  private string nextAttackAnimation;
  private Vector3 oldCorePosition;
  private float sbtime;
  public float size;
  public float speed = 80f;
  private bool startJump;
  private string state = "idle";
  private int stepSoundPhase = 2;
  private float tauntTime;
  private string turnAnimation;
  private float turnDeg;
  private GameObject whoHasTauntMe;
  private AnnieCustomSkinLoader _customSkinLoader;

  private void attack(string type)
  {
    this.state = nameof (attack);
    this.attacked = false;
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
    this.startJump = false;
    this.attackChkOnce = false;
    this.nextAttackAnimation = (string) null;
    this.fxName = (string) null;
    this.isAttackMoveByCore = false;
    this.attackCheckTime = 0.0f;
    this.attackCheckTimeA = 0.0f;
    this.attackCheckTimeB = 0.0f;
    this.fxRotation = Quaternion.Euler(270f, 0.0f, 0.0f);
    string key = type;
    if (key != null)
    {
      // ISSUE: reference to a compiler-generated field
      if (FEMALE_TITAN.fswitchSmap2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        FEMALE_TITAN.fswitchSmap2 = new Dictionary<string, int>(17)
        {
          {
            "combo_1",
            0
          },
          {
            "combo_2",
            1
          },
          {
            "combo_3",
            2
          },
          {
            "combo_blind_1",
            3
          },
          {
            "combo_blind_2",
            4
          },
          {
            "combo_blind_3",
            5
          },
          {
            "front",
            6
          },
          {
            "jumpCombo_1",
            7
          },
          {
            "jumpCombo_2",
            8
          },
          {
            "jumpCombo_3",
            9
          },
          {
            "jumpCombo_4",
            10
          },
          {
            "sweep",
            11
          },
          {
            "sweep_back",
            12
          },
          {
            "sweep_front_left",
            13
          },
          {
            "sweep_front_right",
            14
          },
          {
            "sweep_head_b_l",
            15
          },
          {
            "sweep_head_b_r",
            16
          }
        };
      }
      int num;
      // ISSUE: reference to a compiler-generated field
      if (FEMALE_TITAN.fswitchSmap2.TryGetValue(key, out num))
      {
        switch (num)
        {
          case 0:
            this.attackCheckTimeA = 0.63f;
            this.attackCheckTimeB = 0.8f;
            this.checkHitCapsuleEnd = ((Component) this).transform.Find("Amarture/Core/Controller_Body/hip/thigh_R/shin_R/foot_R");
            this.checkHitCapsuleStart = ((Component) this).transform.Find("Amarture/Core/Controller_Body/hip/thigh_R");
            this.checkHitCapsuleR = 5f;
            this.isAttackMoveByCore = true;
            this.nextAttackAnimation = "combo_2";
            break;
          case 1:
            this.attackCheckTimeA = 0.27f;
            this.attackCheckTimeB = 0.43f;
            this.checkHitCapsuleEnd = ((Component) this).transform.Find("Amarture/Core/Controller_Body/hip/thigh_L/shin_L/foot_L");
            this.checkHitCapsuleStart = ((Component) this).transform.Find("Amarture/Core/Controller_Body/hip/thigh_L");
            this.checkHitCapsuleR = 5f;
            this.isAttackMoveByCore = true;
            this.nextAttackAnimation = "combo_3";
            break;
          case 2:
            this.attackCheckTimeA = 0.15f;
            this.attackCheckTimeB = 0.3f;
            this.checkHitCapsuleEnd = ((Component) this).transform.Find("Amarture/Core/Controller_Body/hip/thigh_R/shin_R/foot_R");
            this.checkHitCapsuleStart = ((Component) this).transform.Find("Amarture/Core/Controller_Body/hip/thigh_R");
            this.checkHitCapsuleR = 5f;
            this.isAttackMoveByCore = true;
            break;
          case 3:
            this.isAttackMoveByCore = true;
            this.attackCheckTimeA = 0.72f;
            this.attackCheckTimeB = 0.83f;
            this.checkHitCapsuleStart = ((Component) this).transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R");
            this.checkHitCapsuleEnd = ((Component) this).transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R/forearm_R/hand_R/hand_R_001");
            this.checkHitCapsuleR = 4f;
            this.nextAttackAnimation = "combo_blind_2";
            break;
          case 4:
            this.isAttackMoveByCore = true;
            this.attackCheckTimeA = 0.5f;
            this.attackCheckTimeB = 0.6f;
            this.checkHitCapsuleStart = ((Component) this).transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R");
            this.checkHitCapsuleEnd = ((Component) this).transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R/forearm_R/hand_R/hand_R_001");
            this.checkHitCapsuleR = 4f;
            this.nextAttackAnimation = "combo_blind_3";
            break;
          case 5:
            this.isAttackMoveByCore = true;
            this.attackCheckTimeA = 0.2f;
            this.attackCheckTimeB = 0.28f;
            this.checkHitCapsuleStart = ((Component) this).transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R");
            this.checkHitCapsuleEnd = ((Component) this).transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R/forearm_R/hand_R/hand_R_001");
            this.checkHitCapsuleR = 4f;
            break;
          case 6:
            this.isAttackMoveByCore = true;
            this.attackCheckTimeA = 0.44f;
            this.attackCheckTimeB = 0.55f;
            this.checkHitCapsuleStart = ((Component) this).transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R");
            this.checkHitCapsuleEnd = ((Component) this).transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R/forearm_R/hand_R/hand_R_001");
            this.checkHitCapsuleR = 4f;
            break;
          case 7:
            this.isAttackMoveByCore = false;
            this.nextAttackAnimation = "jumpCombo_2";
            this.abnorma_jump_bite_horizon_v = Vector3.zero;
            break;
          case 8:
            this.isAttackMoveByCore = false;
            this.attackCheckTimeA = 0.48f;
            this.attackCheckTimeB = 0.7f;
            this.checkHitCapsuleStart = ((Component) this).transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R");
            this.checkHitCapsuleEnd = ((Component) this).transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R/forearm_R/hand_R/hand_R_001");
            this.checkHitCapsuleR = 4f;
            this.nextAttackAnimation = "jumpCombo_3";
            break;
          case 9:
            this.isAttackMoveByCore = false;
            this.checkHitCapsuleEnd = ((Component) this).transform.Find("Amarture/Core/Controller_Body/hip/thigh_L/shin_L/foot_L");
            this.checkHitCapsuleStart = ((Component) this).transform.Find("Amarture/Core/Controller_Body/hip/thigh_L");
            this.checkHitCapsuleR = 5f;
            this.attackCheckTimeA = 0.22f;
            this.attackCheckTimeB = 0.42f;
            break;
          case 10:
            this.isAttackMoveByCore = false;
            break;
          case 11:
            this.isAttackMoveByCore = true;
            this.attackCheckTimeA = 0.39f;
            this.attackCheckTimeB = 0.6f;
            this.checkHitCapsuleEnd = ((Component) this).transform.Find("Amarture/Core/Controller_Body/hip/thigh_R/shin_R/foot_R");
            this.checkHitCapsuleStart = ((Component) this).transform.Find("Amarture/Core/Controller_Body/hip/thigh_R");
            this.checkHitCapsuleR = 5f;
            break;
          case 12:
            this.isAttackMoveByCore = true;
            this.attackCheckTimeA = 0.41f;
            this.attackCheckTimeB = 0.48f;
            this.checkHitCapsuleStart = ((Component) this).transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R");
            this.checkHitCapsuleEnd = ((Component) this).transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R/forearm_R/hand_R/hand_R_001");
            this.checkHitCapsuleR = 4f;
            break;
          case 13:
            this.isAttackMoveByCore = true;
            this.attackCheckTimeA = 0.53f;
            this.attackCheckTimeB = 0.63f;
            this.checkHitCapsuleStart = ((Component) this).transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R");
            this.checkHitCapsuleEnd = ((Component) this).transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R/forearm_R/hand_R/hand_R_001");
            this.checkHitCapsuleR = 4f;
            break;
          case 14:
            this.isAttackMoveByCore = true;
            this.attackCheckTimeA = 0.5f;
            this.attackCheckTimeB = 0.62f;
            this.checkHitCapsuleStart = ((Component) this).transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_L/upper_arm_L");
            this.checkHitCapsuleEnd = ((Component) this).transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_L/upper_arm_L/forearm_L/hand_L/hand_L_001");
            this.checkHitCapsuleR = 4f;
            break;
          case 15:
            this.isAttackMoveByCore = true;
            this.attackCheckTimeA = 0.4f;
            this.attackCheckTimeB = 0.51f;
            this.checkHitCapsuleStart = ((Component) this).transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_L/upper_arm_L");
            this.checkHitCapsuleEnd = ((Component) this).transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_L/upper_arm_L/forearm_L/hand_L/hand_L_001");
            this.checkHitCapsuleR = 4f;
            break;
          case 16:
            this.isAttackMoveByCore = true;
            this.attackCheckTimeA = 0.4f;
            this.attackCheckTimeB = 0.51f;
            this.checkHitCapsuleStart = ((Component) this).transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R");
            this.checkHitCapsuleEnd = ((Component) this).transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R/forearm_R/hand_R/hand_R_001");
            this.checkHitCapsuleR = 4f;
            break;
        }
      }
    }
    this.checkHitCapsuleEndOld = ((Component) this.checkHitCapsuleEnd).transform.position;
    this.needFreshCorePosition = true;
  }

  private bool attackTarget(GameObject target)
  {
    Vector3 vector3 = Vector3.op_Subtraction(target.transform.position, ((Component) this).transform.position);
    double num1 = -(double) Mathf.Atan2(vector3.z, vector3.x) * 57.295780181884766;
    Quaternion rotation = ((Component) this).gameObject.transform.rotation;
    double num2 = (double) ((Quaternion) ref rotation).eulerAngles.y - 90.0;
    float num3 = -Mathf.DeltaAngle((float) num1, (float) num2);
    if (Object.op_Inequality((Object) this.eren, (Object) null) && (double) this.myDistance < 35.0)
    {
      this.attack("combo_1");
      return true;
    }
    string empty = string.Empty;
    ArrayList arrayList = new ArrayList();
    int num4;
    if ((double) this.myDistance < 40.0)
    {
      int num5 = (double) Mathf.Abs(num3) >= 90.0 ? ((double) num3 <= 0.0 ? 3 : 4) : ((double) num3 <= 0.0 ? 2 : 1);
      float num6 = target.transform.position.y - ((Component) this).transform.position.y;
      if ((double) Mathf.Abs(num3) < 90.0)
      {
        if ((double) num6 > 0.0 && (double) num6 < 12.0 && (double) this.myDistance < 22.0)
          arrayList.Add((object) "attack_sweep");
        if ((double) num6 >= 55.0 && (double) num6 < 90.0)
          arrayList.Add((object) "attack_jumpCombo_1");
      }
      if ((double) Mathf.Abs(num3) < 90.0 && (double) num6 > 12.0 && (double) num6 < 40.0)
        arrayList.Add((object) "attack_combo_1");
      if ((double) Mathf.Abs(num3) < 30.0)
      {
        if ((double) num6 > 0.0 && (double) num6 < 12.0 && (double) this.myDistance > 20.0 && (double) this.myDistance < 30.0)
          arrayList.Add((object) "attack_front");
        if ((double) this.myDistance < 12.0 && (double) num6 > 33.0 && (double) num6 < 51.0)
          arrayList.Add((object) "grab_up");
      }
      if ((double) Mathf.Abs(num3) > 100.0 && (double) this.myDistance < 11.0 && (double) num6 >= 15.0 && (double) num6 < 32.0)
        arrayList.Add((object) "attack_sweep_back");
      num4 = num5;
      switch (num4)
      {
        case 1:
          if ((double) this.myDistance >= 11.0)
          {
            if ((double) this.myDistance < 20.0)
            {
              if ((double) num6 >= 12.0 && (double) num6 < 21.0)
              {
                arrayList.Add((object) "grab_bottom_right");
                break;
              }
              if ((double) num6 >= 21.0 && (double) num6 < 32.0)
              {
                arrayList.Add((object) "grab_mid_right");
                break;
              }
              if ((double) num6 >= 32.0 && (double) num6 < 47.0)
              {
                arrayList.Add((object) "grab_up_right");
                break;
              }
              break;
            }
            break;
          }
          if ((double) num6 >= 21.0 && (double) num6 < 32.0)
          {
            arrayList.Add((object) "attack_sweep_front_right");
            break;
          }
          break;
        case 2:
          if ((double) this.myDistance >= 11.0)
          {
            if ((double) this.myDistance < 20.0)
            {
              if ((double) num6 >= 12.0 && (double) num6 < 21.0)
              {
                arrayList.Add((object) "grab_bottom_left");
                break;
              }
              if ((double) num6 >= 21.0 && (double) num6 < 32.0)
              {
                arrayList.Add((object) "grab_mid_left");
                break;
              }
              if ((double) num6 >= 32.0 && (double) num6 < 47.0)
              {
                arrayList.Add((object) "grab_up_left");
                break;
              }
              break;
            }
            break;
          }
          if ((double) num6 >= 21.0 && (double) num6 < 32.0)
          {
            arrayList.Add((object) "attack_sweep_front_left");
            break;
          }
          break;
        case 3:
          if ((double) this.myDistance >= 11.0)
          {
            arrayList.Add((object) "turn180");
            break;
          }
          if ((double) num6 >= 33.0 && (double) num6 < 51.0)
          {
            arrayList.Add((object) "attack_sweep_head_b_l");
            break;
          }
          break;
        case 4:
          if ((double) this.myDistance >= 11.0)
          {
            arrayList.Add((object) "turn180");
            break;
          }
          if ((double) num6 >= 33.0 && (double) num6 < 51.0)
          {
            arrayList.Add((object) "attack_sweep_head_b_r");
            break;
          }
          break;
      }
    }
    if (arrayList.Count > 0)
      empty = (string) arrayList[Random.Range(0, arrayList.Count)];
    else if (Random.Range(0, 100) < 10)
    {
      GameObject[] gameObjectsWithTag = GameObject.FindGameObjectsWithTag("Player");
      this.myHero = gameObjectsWithTag[Random.Range(0, gameObjectsWithTag.Length)];
      this.attention = Random.Range(5f, 10f);
      return true;
    }
    string key = empty;
    if (key != null)
    {
      // ISSUE: reference to a compiler-generated field
      if (FEMALE_TITAN.fswitchSmap1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        FEMALE_TITAN.fswitchSmap1 = new Dictionary<string, int>(17)
        {
          {
            "grab_bottom_left",
            0
          },
          {
            "grab_bottom_right",
            1
          },
          {
            "grab_mid_left",
            2
          },
          {
            "grab_mid_right",
            3
          },
          {
            "grab_up",
            4
          },
          {
            "grab_up_left",
            5
          },
          {
            "grab_up_right",
            6
          },
          {
            "attack_combo_1",
            7
          },
          {
            "attack_front",
            8
          },
          {
            "attack_jumpCombo_1",
            9
          },
          {
            "attack_sweep",
            10
          },
          {
            "attack_sweep_back",
            11
          },
          {
            "attack_sweep_front_left",
            12
          },
          {
            "attack_sweep_front_right",
            13
          },
          {
            "attack_sweep_head_b_l",
            14
          },
          {
            "attack_sweep_head_b_r",
            15
          },
          {
            "turn180",
            16
          }
        };
      }
      // ISSUE: reference to a compiler-generated field
      if (FEMALE_TITAN.fswitchSmap1.TryGetValue(key, out num4))
      {
        switch (num4)
        {
          case 0:
            this.grab("bottom_left");
            return true;
          case 1:
            this.grab("bottom_right");
            return true;
          case 2:
            this.grab("mid_left");
            return true;
          case 3:
            this.grab("mid_right");
            return true;
          case 4:
            this.grab("up");
            return true;
          case 5:
            this.grab("up_left");
            return true;
          case 6:
            this.grab("up_right");
            return true;
          case 7:
            this.attack("combo_1");
            return true;
          case 8:
            this.attack("front");
            return true;
          case 9:
            this.attack("jumpCombo_1");
            return true;
          case 10:
            this.attack("sweep");
            return true;
          case 11:
            this.attack("sweep_back");
            return true;
          case 12:
            this.attack("sweep_front_left");
            return true;
          case 13:
            this.attack("sweep_front_right");
            return true;
          case 14:
            this.attack("sweep_head_b_l");
            return true;
          case 15:
            this.attack("sweep_head_b_r");
            return true;
          case 16:
            this.turn180();
            return true;
        }
      }
    }
    return false;
  }

  private void Awake()
  {
    ((Component) this).rigidbody.freezeRotation = true;
    ((Component) this).rigidbody.useGravity = false;
    this._customSkinLoader = ((Component) this).gameObject.AddComponent<AnnieCustomSkinLoader>();
  }

  public void beTauntedBy(GameObject target, float tauntTime)
  {
    this.whoHasTauntMe = target;
    this.tauntTime = tauntTime;
  }

  private void chase()
  {
    this.state = nameof (chase);
    this.crossFade("run", 0.5f);
  }

  private RaycastHit[] checkHitCapsule(Vector3 start, Vector3 end, float r) => Physics.SphereCastAll(start, r, Vector3.op_Subtraction(end, start), Vector3.Distance(start, end));

  private GameObject checkIfHitHand(Transform hand)
  {
    float num = 9.6f;
    foreach (Collider collider in Physics.OverlapSphere(((Component) ((Component) hand).GetComponent<SphereCollider>()).transform.position, num + 1f))
    {
      if (((Component) ((Component) collider).transform.root).tag == "Player")
      {
        GameObject gameObject = ((Component) ((Component) collider).transform.root).gameObject;
        if (Object.op_Inequality((Object) gameObject.GetComponent<TITAN_EREN>(), (Object) null))
        {
          if (!gameObject.GetComponent<TITAN_EREN>().isHit)
            gameObject.GetComponent<TITAN_EREN>().hitByTitan();
          return gameObject;
        }
        if (Object.op_Inequality((Object) gameObject.GetComponent<HERO>(), (Object) null) && !gameObject.GetComponent<HERO>().isInvincible())
          return gameObject;
      }
    }
    return (GameObject) null;
  }

  private GameObject checkIfHitHead(Transform head, float rad)
  {
    float num1 = rad * 4f;
    foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("Player"))
    {
      if (Object.op_Equality((Object) gameObject.GetComponent<TITAN_EREN>(), (Object) null) && !gameObject.GetComponent<HERO>().isInvincible())
      {
        float num2 = gameObject.GetComponent<CapsuleCollider>().height * 0.5f;
        if ((double) Vector3.Distance(Vector3.op_Addition(gameObject.transform.position, Vector3.op_Multiply(Vector3.up, num2)), Vector3.op_Addition(((Component) head).transform.position, Vector3.op_Multiply(Vector3.op_Multiply(Vector3.up, 1.5f), 4f))) < (double) num1 + (double) num2)
          return gameObject;
      }
    }
    return (GameObject) null;
  }

  private void crossFade(string aniName, float time)
  {
    ((Component) this).animation.CrossFade(aniName, time);
    if (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.MULTIPLAYER || !PhotonNetwork.isMasterClient)
      return;
    this.photonView.RPC("netCrossFade", PhotonTargets.Others, (object) aniName, (object) time);
  }

  private void eatSet(GameObject grabTarget)
  {
    if (grabTarget.GetComponent<HERO>().isGrabbed)
      return;
    this.grabToRight();
    if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER && PhotonNetwork.isMasterClient)
    {
      object[] objArray1 = new object[2]
      {
        (object) this.photonView.viewID,
        (object) false
      };
      grabTarget.GetPhotonView().RPC("netGrabbed", PhotonTargets.All, objArray1);
      object[] objArray2 = new object[1]
      {
        (object) "grabbed"
      };
      grabTarget.GetPhotonView().RPC("netPlayAnimation", PhotonTargets.All, objArray2);
      this.photonView.RPC("grabToRight", PhotonTargets.Others);
    }
    else
    {
      grabTarget.GetComponent<HERO>().grabbed(((Component) this).gameObject, false);
      ((Component) grabTarget.GetComponent<HERO>()).animation.Play("grabbed");
    }
  }

  private void eatSetL(GameObject grabTarget)
  {
    if (grabTarget.GetComponent<HERO>().isGrabbed)
      return;
    this.grabToLeft();
    if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER && PhotonNetwork.isMasterClient)
    {
      object[] objArray1 = new object[2]
      {
        (object) this.photonView.viewID,
        (object) true
      };
      grabTarget.GetPhotonView().RPC("netGrabbed", PhotonTargets.All, objArray1);
      object[] objArray2 = new object[1]
      {
        (object) "grabbed"
      };
      grabTarget.GetPhotonView().RPC("netPlayAnimation", PhotonTargets.All, objArray2);
      this.photonView.RPC("grabToLeft", PhotonTargets.Others);
    }
    else
    {
      grabTarget.GetComponent<HERO>().grabbed(((Component) this).gameObject, true);
      ((Component) grabTarget.GetComponent<HERO>()).animation.Play("grabbed");
    }
  }

  public void erenIsHere(GameObject target) => this.myHero = this.eren = target;

  private void findNearestFacingHero()
  {
    GameObject[] gameObjectsWithTag = GameObject.FindGameObjectsWithTag("Player");
    GameObject gameObject1 = (GameObject) null;
    float num1 = float.PositiveInfinity;
    Vector3 position = ((Component) this).transform.position;
    float num2 = 180f;
    foreach (GameObject gameObject2 in gameObjectsWithTag)
    {
      Vector3 vector3_1 = Vector3.op_Subtraction(gameObject2.transform.position, position);
      float sqrMagnitude = ((Vector3) ref vector3_1).sqrMagnitude;
      if ((double) sqrMagnitude < (double) num1)
      {
        Vector3 vector3_2 = Vector3.op_Subtraction(gameObject2.transform.position, ((Component) this).transform.position);
        double num3 = -(double) Mathf.Atan2(vector3_2.z, vector3_2.x) * 57.295780181884766;
        Quaternion rotation = ((Component) this).gameObject.transform.rotation;
        double num4 = (double) ((Quaternion) ref rotation).eulerAngles.y - 90.0;
        if ((double) Mathf.Abs(-Mathf.DeltaAngle((float) num3, (float) num4)) < (double) num2)
        {
          gameObject1 = gameObject2;
          num1 = sqrMagnitude;
        }
      }
    }
    if (!Object.op_Inequality((Object) gameObject1, (Object) null))
      return;
    this.myHero = gameObject1;
    this.tauntTime = 5f;
  }

  private void findNearestHero()
  {
    this.myHero = this.getNearestHero();
    this.attention = Random.Range(5f, 10f);
  }

  private void FixedUpdate()
  {
    if (GameMenu.Paused && IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE || IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE && !this.photonView.isMine)
      return;
    if (this.bottomObject.GetComponent<CheckHitGround>().isGrounded)
    {
      this.grounded = true;
      this.bottomObject.GetComponent<CheckHitGround>().isGrounded = false;
    }
    else
      this.grounded = false;
    if (this.needFreshCorePosition)
    {
      this.oldCorePosition = Vector3.op_Subtraction(((Component) this).transform.position, ((Component) this).transform.Find("Amarture/Core").position);
      this.needFreshCorePosition = false;
    }
    if (this.state == "attack" && this.isAttackMoveByCore || this.state == "hit" || this.state == "turn180" || this.state == "anklehurt")
    {
      ((Component) this).rigidbody.velocity = Vector3.op_Addition(Vector3.op_Division(Vector3.op_Subtraction(Vector3.op_Subtraction(((Component) this).transform.position, ((Component) this).transform.Find("Amarture/Core").position), this.oldCorePosition), Time.deltaTime), Vector3.op_Multiply(Vector3.up, ((Component) this).rigidbody.velocity.y));
      this.oldCorePosition = Vector3.op_Subtraction(((Component) this).transform.position, ((Component) this).transform.Find("Amarture/Core").position);
    }
    else if (this.state == "chase")
    {
      if (Object.op_Equality((Object) this.myHero, (Object) null))
        return;
      Vector3 vector3_1 = Vector3.op_Subtraction(Vector3.op_Multiply(((Component) this).transform.forward, this.speed), ((Component) this).rigidbody.velocity);
      vector3_1.y = 0.0f;
      ((Component) this).rigidbody.AddForce(vector3_1, (ForceMode) 2);
      Vector3 vector3_2 = Vector3.op_Subtraction(this.myHero.transform.position, ((Component) this).transform.position);
      double num1 = -(double) Mathf.Atan2(vector3_2.z, vector3_2.x) * 57.295780181884766;
      Quaternion rotation1 = ((Component) this).gameObject.transform.rotation;
      double num2 = (double) ((Quaternion) ref rotation1).eulerAngles.y - 90.0;
      float num3 = -Mathf.DeltaAngle((float) num1, (float) num2);
      Transform transform = ((Component) this).gameObject.transform;
      Quaternion rotation2 = ((Component) this).gameObject.transform.rotation;
      Quaternion rotation3 = ((Component) this).gameObject.transform.rotation;
      Quaternion quaternion1 = Quaternion.Euler(0.0f, (float) ((double) ((Quaternion) ref rotation3).eulerAngles.y + (double) num3), 0.0f);
      double num4 = (double) this.speed * (double) Time.deltaTime;
      Quaternion quaternion2 = Quaternion.Lerp(rotation2, quaternion1, (float) num4);
      transform.rotation = quaternion2;
    }
    else if (this.grounded && !((Component) this).animation.IsPlaying("attack_jumpCombo_1"))
      ((Component) this).rigidbody.AddForce(new Vector3(-((Component) this).rigidbody.velocity.x, 0.0f, -((Component) this).rigidbody.velocity.z), (ForceMode) 2);
    ((Component) this).rigidbody.AddForce(new Vector3(0.0f, -this.gravity * ((Component) this).rigidbody.mass, 0.0f));
  }

  private void getDown()
  {
    this.state = "anklehurt";
    this.playAnimation("legHurt");
    this.AnkleRHP = this.AnkleRHPMAX;
    this.AnkleLHP = this.AnkleLHPMAX;
    this.needFreshCorePosition = true;
  }

  private GameObject getNearestHero()
  {
    GameObject[] gameObjectsWithTag = GameObject.FindGameObjectsWithTag("Player");
    GameObject nearestHero = (GameObject) null;
    float num = float.PositiveInfinity;
    Vector3 position = ((Component) this).transform.position;
    foreach (GameObject gameObject in gameObjectsWithTag)
    {
      if ((Object.op_Equality((Object) gameObject.GetComponent<HERO>(), (Object) null) || !gameObject.GetComponent<HERO>().HasDied()) && (Object.op_Equality((Object) gameObject.GetComponent<TITAN_EREN>(), (Object) null) || !gameObject.GetComponent<TITAN_EREN>().hasDied))
      {
        Vector3 vector3 = Vector3.op_Subtraction(gameObject.transform.position, position);
        float sqrMagnitude = ((Vector3) ref vector3).sqrMagnitude;
        if ((double) sqrMagnitude < (double) num)
        {
          nearestHero = gameObject;
          num = sqrMagnitude;
        }
      }
    }
    return nearestHero;
  }

  private float getNearestHeroDistance()
  {
    GameObject[] gameObjectsWithTag = GameObject.FindGameObjectsWithTag("Player");
    float nearestHeroDistance = float.PositiveInfinity;
    Vector3 position = ((Component) this).transform.position;
    foreach (GameObject gameObject in gameObjectsWithTag)
    {
      Vector3 vector3 = Vector3.op_Subtraction(gameObject.transform.position, position);
      float magnitude = ((Vector3) ref vector3).magnitude;
      if ((double) magnitude < (double) nearestHeroDistance)
        nearestHeroDistance = magnitude;
    }
    return nearestHeroDistance;
  }

  private void grab(string type)
  {
    this.state = nameof (grab);
    this.attacked = false;
    this.attackAnimation = type;
    if (((Component) this).animation.IsPlaying("attack_grab_" + type))
    {
      ((Component) this).animation["attack_grab_" + type].normalizedTime = 0.0f;
      this.playAnimation("attack_grab_" + type);
    }
    else
      this.crossFade("attack_grab_" + type, 0.1f);
    this.isGrabHandLeft = true;
    this.grabbedTarget = (GameObject) null;
    this.attackCheckTime = 0.0f;
    string key = type;
    if (key != null)
    {
      // ISSUE: reference to a compiler-generated field
      if (FEMALE_TITAN.fswitchSmap3 == null)
      {
        // ISSUE: reference to a compiler-generated field
        FEMALE_TITAN.fswitchSmap3 = new Dictionary<string, int>(7)
        {
          {
            "bottom_left",
            0
          },
          {
            "bottom_right",
            1
          },
          {
            "mid_left",
            2
          },
          {
            "mid_right",
            3
          },
          {
            "up",
            4
          },
          {
            "up_left",
            5
          },
          {
            "up_right",
            6
          }
        };
      }
      int num;
      // ISSUE: reference to a compiler-generated field
      if (FEMALE_TITAN.fswitchSmap3.TryGetValue(key, out num))
      {
        switch (num)
        {
          case 0:
            this.attackCheckTimeA = 0.28f;
            this.attackCheckTimeB = 0.38f;
            this.attackCheckTime = 0.65f;
            this.isGrabHandLeft = false;
            break;
          case 1:
            this.attackCheckTimeA = 0.27f;
            this.attackCheckTimeB = 0.37f;
            this.attackCheckTime = 0.65f;
            break;
          case 2:
            this.attackCheckTimeA = 0.27f;
            this.attackCheckTimeB = 0.37f;
            this.attackCheckTime = 0.65f;
            this.isGrabHandLeft = false;
            break;
          case 3:
            this.attackCheckTimeA = 0.27f;
            this.attackCheckTimeB = 0.36f;
            this.attackCheckTime = 0.66f;
            break;
          case 4:
            this.attackCheckTimeA = 0.25f;
            this.attackCheckTimeB = 0.32f;
            this.attackCheckTime = 0.67f;
            break;
          case 5:
            this.attackCheckTimeA = 0.26f;
            this.attackCheckTimeB = 0.4f;
            this.attackCheckTime = 0.66f;
            break;
          case 6:
            this.attackCheckTimeA = 0.26f;
            this.attackCheckTimeB = 0.4f;
            this.attackCheckTime = 0.66f;
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
  public void grabbedTargetEscape() => this.grabbedTarget = (GameObject) null;

  [RPC]
  public void grabToLeft()
  {
    Transform transform1 = ((Component) this).transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_L/upper_arm_L/forearm_L/hand_L/hand_L_001");
    this.grabTF.transform.parent = transform1;
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

  [RPC]
  public void grabToRight()
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

  public void hit(int dmg)
  {
    this.NapeArmor -= dmg;
    if (this.NapeArmor > 0)
      return;
    this.NapeArmor = 0;
  }

  public void hitAnkleL(int dmg)
  {
    if (this.hasDie || !(this.state != "anklehurt"))
      return;
    this.AnkleLHP -= dmg;
    if (this.AnkleLHP > 0)
      return;
    this.getDown();
  }

  [RPC]
  public void hitAnkleLRPC(int viewID, int dmg)
  {
    if (this.hasDie || !(this.state != "anklehurt"))
      return;
    PhotonView photonView = PhotonView.Find(viewID);
    if (!Object.op_Inequality((Object) photonView, (Object) null))
      return;
    if (Object.op_Inequality((Object) this.grabbedTarget, (Object) null))
      this.grabbedTarget.GetPhotonView().RPC("netUngrabbed", PhotonTargets.All);
    Vector3 vector3 = Vector3.op_Subtraction(((Component) photonView).gameObject.transform.position, ((Component) ((Component) this).transform.Find("Amarture/Core/Controller_Body")).transform.position);
    if ((double) ((Vector3) ref vector3).magnitude >= 20.0)
      return;
    this.AnkleLHP -= dmg;
    if (this.AnkleLHP <= 0)
      this.getDown();
    GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().sendKillInfo(false, (string) photonView.owner.customProperties[(object) PhotonPlayerProperty.name], true, "Female Titan's ankle", dmg);
    object[] objArray = new object[1]{ (object) dmg };
    GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().photonView.RPC("netShowDamage", photonView.owner, objArray);
  }

  public void hitAnkleR(int dmg)
  {
    if (this.hasDie || !(this.state != "anklehurt"))
      return;
    this.AnkleRHP -= dmg;
    if (this.AnkleRHP > 0)
      return;
    this.getDown();
  }

  [RPC]
  public void hitAnkleRRPC(int viewID, int dmg)
  {
    if (this.hasDie || !(this.state != "anklehurt"))
      return;
    PhotonView photonView = PhotonView.Find(viewID);
    if (!Object.op_Inequality((Object) photonView, (Object) null))
      return;
    if (Object.op_Inequality((Object) this.grabbedTarget, (Object) null))
      this.grabbedTarget.GetPhotonView().RPC("netUngrabbed", PhotonTargets.All);
    Vector3 vector3 = Vector3.op_Subtraction(((Component) photonView).gameObject.transform.position, ((Component) ((Component) this).transform.Find("Amarture/Core/Controller_Body")).transform.position);
    if ((double) ((Vector3) ref vector3).magnitude >= 20.0)
      return;
    this.AnkleRHP -= dmg;
    if (this.AnkleRHP <= 0)
      this.getDown();
    GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().sendKillInfo(false, (string) photonView.owner.customProperties[(object) PhotonPlayerProperty.name], true, "Female Titan's ankle", dmg);
    object[] objArray = new object[1]{ (object) dmg };
    GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().photonView.RPC("netShowDamage", photonView.owner, objArray);
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
    if (Object.op_Inequality((Object) this.grabbedTarget, (Object) null))
      this.grabbedTarget.GetPhotonView().RPC("netUngrabbed", PhotonTargets.All);
    Transform transform = ((Component) this).transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck");
    PhotonView photonView = PhotonView.Find(viewID);
    if (!Object.op_Inequality((Object) photonView, (Object) null))
      return;
    Vector3 vector3 = Vector3.op_Subtraction(((Component) photonView).gameObject.transform.position, ((Component) transform).transform.position);
    if ((double) ((Vector3) ref vector3).magnitude >= 20.0)
      return;
    this.justHitEye();
  }

  private void idle(float sbtime = 0.0f)
  {
    this.sbtime = sbtime;
    this.sbtime = Mathf.Max(0.5f, this.sbtime);
    this.state = nameof (idle);
    this.crossFade(nameof (idle), 0.2f);
  }

  public bool IsGrounded() => this.bottomObject.GetComponent<CheckHitGround>().isGrounded;

  private void justEatHero(GameObject target, Transform hand)
  {
    if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER && PhotonNetwork.isMasterClient)
    {
      if (target.GetComponent<HERO>().HasDied())
        return;
      target.GetComponent<HERO>().markDie();
      object[] objArray = new object[2]
      {
        (object) -1,
        (object) "Female Titan"
      };
      target.GetComponent<HERO>().photonView.RPC("netDie2", PhotonTargets.All, objArray);
    }
    else
    {
      if (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE)
        return;
      target.GetComponent<HERO>().die2(hand);
    }
  }

  private void justHitEye() => this.attack("combo_blind_1");

  private void killPlayer(GameObject hitHero)
  {
    if (!Object.op_Inequality((Object) hitHero, (Object) null))
      return;
    Vector3 position = ((Component) this).transform.Find("Amarture/Core/Controller_Body/hip/spine/chest").position;
    switch (IN_GAME_MAIN_CAMERA.gametype)
    {
      case GAMETYPE.SINGLE:
        if (hitHero.GetComponent<HERO>().HasDied())
          break;
        hitHero.GetComponent<HERO>().die(Vector3.op_Multiply(Vector3.op_Multiply(Vector3.op_Subtraction(hitHero.transform.position, position), 15f), 4f), false);
        break;
      case GAMETYPE.MULTIPLAYER:
        if (!PhotonNetwork.isMasterClient || hitHero.GetComponent<HERO>().HasDied())
          break;
        hitHero.GetComponent<HERO>().markDie();
        object[] objArray = new object[5]
        {
          (object) Vector3.op_Multiply(Vector3.op_Multiply(Vector3.op_Subtraction(hitHero.transform.position, position), 15f), 4f),
          (object) false,
          (object) -1,
          (object) "Female Titan",
          (object) true
        };
        hitHero.GetComponent<HERO>().photonView.RPC("netDie", PhotonTargets.All, objArray);
        break;
    }
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
        this.healthLabel.transform.localPosition = new Vector3(0.0f, 52f, 0.0f);
        float num = 4f;
        if ((double) this.size > 0.0 && (double) this.size < 1.0)
          num = Mathf.Min(4f / this.size, 15f);
        this.healthLabel.transform.localScale = new Vector3(num, num, num);
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
    if (((Component) this).animation.IsPlaying("run"))
    {
      if ((double) ((Component) this).animation["run"].normalizedTime % 1.0 > 0.10000000149011612 && (double) ((Component) this).animation["run"].normalizedTime % 1.0 < 0.60000002384185791 && this.stepSoundPhase == 2)
      {
        this.stepSoundPhase = 1;
        Transform transform = ((Component) this).transform.Find("snd_titan_foot");
        ((Component) transform).GetComponent<AudioSource>().Stop();
        ((Component) transform).GetComponent<AudioSource>().Play();
      }
      if ((double) ((Component) this).animation["run"].normalizedTime % 1.0 > 0.60000002384185791 && this.stepSoundPhase == 1)
      {
        this.stepSoundPhase = 2;
        Transform transform = ((Component) this).transform.Find("snd_titan_foot");
        ((Component) transform).GetComponent<AudioSource>().Stop();
        ((Component) transform).GetComponent<AudioSource>().Play();
      }
    }
    if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
      return;
    int num = this.photonView.isMine ? 1 : 0;
  }

  public void lateUpdate2()
  {
    if (GameMenu.Paused && IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
      return;
    if (((Component) this).animation.IsPlaying("run"))
    {
      if ((double) ((Component) this).animation["run"].normalizedTime % 1.0 > 0.10000000149011612 && (double) ((Component) this).animation["run"].normalizedTime % 1.0 < 0.60000002384185791 && this.stepSoundPhase == 2)
      {
        this.stepSoundPhase = 1;
        Transform transform = ((Component) this).transform.Find("snd_titan_foot");
        ((Component) transform).GetComponent<AudioSource>().Stop();
        ((Component) transform).GetComponent<AudioSource>().Play();
      }
      if ((double) ((Component) this).animation["run"].normalizedTime % 1.0 > 0.60000002384185791 && this.stepSoundPhase == 1)
      {
        this.stepSoundPhase = 2;
        Transform transform = ((Component) this).transform.Find("snd_titan_foot");
        ((Component) transform).GetComponent<AudioSource>().Stop();
        ((Component) transform).GetComponent<AudioSource>().Play();
      }
    }
    this.updateLabel();
    this.healthTime -= Time.deltaTime;
  }

  public void loadskin()
  {
    BaseCustomSkinSettings<ShifterCustomSkinSet> shifter = SettingsManager.CustomSkinSettings.Shifter;
    string url = ((ShifterCustomSkinSet) shifter.GetSelectedSet()).Annie.Value;
    if (!shifter.SkinsEnabled.Value || !TextureDownloader.ValidTextureURL(url))
      return;
    this.photonView.RPC("loadskinRPC", PhotonTargets.AllBuffered, (object) url);
  }

  public IEnumerator loadskinE(string url)
  {
    FEMALE_TITAN femaleTitan = this;
    while (!femaleTitan.hasspawn)
      yield return (object) null;
    yield return (object) femaleTitan.StartCoroutine(femaleTitan._customSkinLoader.LoadSkinsFromRPC(new object[1]
    {
      (object) url
    }));
  }

  [RPC]
  public void loadskinRPC(string url, PhotonMessageInfo info)
  {
    if (info.sender != this.photonView.owner)
      return;
    BaseCustomSkinSettings<ShifterCustomSkinSet> shifter = SettingsManager.CustomSkinSettings.Shifter;
    if (!shifter.SkinsEnabled.Value || shifter.SkinsLocal.Value && !this.photonView.isMine)
      return;
    this.StartCoroutine(this.loadskinE(url));
  }

  [RPC]
  private void netCrossFade(string aniName, float time) => ((Component) this).animation.CrossFade(aniName, time);

  [RPC]
  public void netDie()
  {
    if (this.hasDie)
      return;
    this.hasDie = true;
    this.crossFade("die", 0.05f);
  }

  [RPC]
  private void netPlayAnimation(string aniName) => ((Component) this).animation.Play(aniName);

  [RPC]
  private void netPlayAnimationAt(string aniName, float normalizedTime)
  {
    ((Component) this).animation.Play(aniName);
    ((Component) this).animation[aniName].normalizedTime = normalizedTime;
  }

  private void OnDestroy()
  {
    if (!Object.op_Inequality((Object) GameObject.Find("MultiplayerManager"), (Object) null))
      return;
    GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().removeFT(this);
  }

  private void playAnimation(string aniName)
  {
    ((Component) this).animation.Play(aniName);
    if (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.MULTIPLAYER || !PhotonNetwork.isMasterClient)
      return;
    this.photonView.RPC("netPlayAnimation", PhotonTargets.Others, (object) aniName);
  }

  private void playAnimationAt(string aniName, float normalizedTime)
  {
    ((Component) this).animation.Play(aniName);
    ((Component) this).animation[aniName].normalizedTime = normalizedTime;
    if (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.MULTIPLAYER || !PhotonNetwork.isMasterClient)
      return;
    this.photonView.RPC("netPlayAnimationAt", PhotonTargets.Others, (object) aniName, (object) normalizedTime);
  }

  private void playSound(string sndname)
  {
    this.playsoundRPC(sndname);
    if (Network.peerType != 1)
      return;
    this.photonView.RPC("playsoundRPC", PhotonTargets.Others, (object) sndname);
  }

  [RPC]
  private void playsoundRPC(string sndname) => ((Component) ((Component) this).transform.Find(sndname)).GetComponent<AudioSource>().Play();

  [RPC]
  public void setSize(float size, PhotonMessageInfo info)
  {
    size = Mathf.Clamp(size, 0.2f, 30f);
    if (!info.sender.isMasterClient)
      return;
    Transform transform = ((Component) this).transform;
    transform.localScale = Vector3.op_Multiply(transform.localScale, size * 0.25f);
    this.size = size;
  }

  private void Start()
  {
    this.startMain();
    this.size = 4f;
    if (Object.op_Inequality((Object) Minimap.instance, (Object) null))
      Minimap.instance.TrackGameObjectOnMinimap(((Component) this).gameObject, Color.black, false, true);
    if (this.photonView.isMine)
    {
      if (SettingsManager.LegacyGameSettings.TitanSizeEnabled.Value)
      {
        this.size = Random.Range(SettingsManager.LegacyGameSettings.TitanSizeMin.Value, SettingsManager.LegacyGameSettings.TitanSizeMax.Value);
        this.photonView.RPC("setSize", PhotonTargets.AllBuffered, (object) this.size);
      }
      this.lagMax = (float) (150.0 + (double) this.size * 3.0);
      this.healthTime = 0.0f;
      this.maxHealth = this.NapeArmor;
      if (SettingsManager.LegacyGameSettings.TitanHealthMode.Value > 0)
        this.maxHealth = this.NapeArmor = Random.Range(SettingsManager.LegacyGameSettings.TitanHealthMin.Value, SettingsManager.LegacyGameSettings.TitanHealthMax.Value);
      if (this.NapeArmor > 0)
        this.photonView.RPC("labelRPC", PhotonTargets.AllBuffered, (object) this.NapeArmor, (object) this.maxHealth);
      this.loadskin();
    }
    this.hasspawn = true;
  }

  private void startMain()
  {
    GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().addFT(this);
    ((Object) this).name = "Female Titan";
    this.grabTF = new GameObject();
    ((Object) this.grabTF).name = "titansTmpGrabTF";
    this.currentCamera = GameObject.Find("MainCamera");
    this.oldCorePosition = Vector3.op_Subtraction(((Component) this).transform.position, ((Component) this).transform.Find("Amarture/Core").position);
    if (Object.op_Equality((Object) this.myHero, (Object) null))
      this.findNearestHero();
    foreach (AnimationState animationState in ((Component) this).animation)
      animationState.speed = 0.7f;
    ((Component) this).animation["turn180"].speed = 0.5f;
    this.NapeArmor = 1000;
    this.AnkleLHP = 50;
    this.AnkleRHP = 50;
    this.AnkleLHPMAX = 50;
    this.AnkleRHPMAX = 50;
    bool flag = false;
    if (LevelInfo.getInfo(FengGameManagerMKII.level).respawnMode == RespawnMode.NEVER)
      flag = true;
    switch (IN_GAME_MAIN_CAMERA.difficulty)
    {
      case 0:
        this.NapeArmor = !flag ? 1000 : 1000;
        this.AnkleLHP = this.AnkleLHPMAX = !flag ? 50 : 50;
        this.AnkleRHP = this.AnkleRHPMAX = !flag ? 50 : 50;
        break;
      case 1:
        this.NapeArmor = !flag ? 3000 : 2500;
        this.AnkleLHP = this.AnkleLHPMAX = !flag ? 200 : 100;
        this.AnkleRHP = this.AnkleRHPMAX = !flag ? 200 : 100;
        foreach (AnimationState animationState in ((Component) this).animation)
          animationState.speed = 0.7f;
        ((Component) this).animation["turn180"].speed = 0.7f;
        break;
      case 2:
        this.NapeArmor = !flag ? 6000 : 4000;
        this.AnkleLHP = this.AnkleLHPMAX = !flag ? 1000 : 200;
        this.AnkleRHP = this.AnkleRHPMAX = !flag ? 1000 : 200;
        foreach (AnimationState animationState in ((Component) this).animation)
          animationState.speed = 1f;
        ((Component) this).animation["turn180"].speed = 0.9f;
        break;
    }
    if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.PVP_CAPTURE)
      this.NapeArmor = (int) ((double) this.NapeArmor * 0.800000011920929);
    ((Component) this).animation["legHurt"].speed = 1f;
    ((Component) this).animation["legHurt_loop"].speed = 1f;
    ((Component) this).animation["legHurt_getup"].speed = 1f;
  }

  [RPC]
  public void titanGetHit(int viewID, int speed)
  {
    Transform transform = ((Component) this).transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck");
    PhotonView photonView = PhotonView.Find(viewID);
    if (!Object.op_Inequality((Object) photonView, (Object) null))
      return;
    Vector3 vector3 = Vector3.op_Subtraction(((Component) photonView).gameObject.transform.position, ((Component) transform).transform.position);
    if ((double) ((Vector3) ref vector3).magnitude >= (double) this.lagMax || (double) this.healthTime > 0.0)
      return;
    if (!SettingsManager.LegacyGameSettings.TitanArmorEnabled.Value || speed >= SettingsManager.LegacyGameSettings.TitanArmor.Value)
      this.NapeArmor -= speed;
    if ((double) this.maxHealth > 0.0)
      this.photonView.RPC("labelRPC", PhotonTargets.AllBuffered, (object) this.NapeArmor, (object) this.maxHealth);
    if (this.NapeArmor <= 0)
    {
      this.NapeArmor = 0;
      if (!this.hasDie)
      {
        this.photonView.RPC("netDie", PhotonTargets.OthersBuffered);
        if (Object.op_Inequality((Object) this.grabbedTarget, (Object) null))
          this.grabbedTarget.GetPhotonView().RPC("netUngrabbed", PhotonTargets.All);
        this.netDie();
        GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().titanGetKill(photonView.owner, speed, ((Object) this).name);
      }
    }
    else
    {
      GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().sendKillInfo(false, (string) photonView.owner.customProperties[(object) PhotonPlayerProperty.name], true, "Female Titan's neck", speed);
      object[] objArray = new object[1]{ (object) speed };
      GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().photonView.RPC("netShowDamage", photonView.owner, objArray);
    }
    this.healthTime = 0.2f;
  }

  private void turn(float d)
  {
    this.turnAnimation = (double) d <= 0.0 ? "turnaround2" : "turnaround1";
    this.playAnimation(this.turnAnimation);
    ((Component) this).animation[this.turnAnimation].time = 0.0f;
    d = Mathf.Clamp(d, -120f, 120f);
    this.turnDeg = d;
    Quaternion rotation = ((Component) this).gameObject.transform.rotation;
    this.desDeg = ((Quaternion) ref rotation).eulerAngles.y + this.turnDeg;
    this.state = nameof (turn);
  }

  private void turn180()
  {
    this.turnAnimation = nameof (turn180);
    this.playAnimation(this.turnAnimation);
    ((Component) this).animation[this.turnAnimation].time = 0.0f;
    this.state = nameof (turn180);
    this.needFreshCorePosition = true;
  }

  public void update()
  {
    if (GameMenu.Paused && IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE || IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE && !this.photonView.isMine)
      return;
    if (this.hasDie)
    {
      this.dieTime += Time.deltaTime;
      if ((double) ((Component) this).animation["die"].normalizedTime >= 1.0)
      {
        this.playAnimation("die_cry");
        if (IN_GAME_MAIN_CAMERA.gamemode != GAMEMODE.PVP_CAPTURE)
        {
          for (int index = 0; index < 15; ++index)
            GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().randomSpawnOneTitan("titanRespawn", 50).GetComponent<TITAN>().beTauntedBy(((Component) this).gameObject, 20f);
        }
      }
      if ((double) this.dieTime > 2.0 && !this.hasDieSteam)
      {
        this.hasDieSteam = true;
        if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
        {
          GameObject gameObject = (GameObject) Object.Instantiate(Resources.Load("FX/FXtitanDie1"));
          gameObject.transform.position = ((Component) this).transform.Find("Amarture/Core/Controller_Body/hip").position;
          gameObject.transform.localScale = ((Component) this).transform.localScale;
        }
        else if (this.photonView.isMine)
          PhotonNetwork.Instantiate("FX/FXtitanDie1", ((Component) this).transform.Find("Amarture/Core/Controller_Body/hip").position, Quaternion.Euler(-90f, 0.0f, 0.0f), 0).transform.localScale = ((Component) this).transform.localScale;
      }
      if ((double) this.dieTime <= (IN_GAME_MAIN_CAMERA.gamemode != GAMEMODE.PVP_CAPTURE ? 20.0 : 5.0))
        return;
      if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
      {
        GameObject gameObject = (GameObject) Object.Instantiate(Resources.Load("FX/FXtitanDie"));
        gameObject.transform.position = ((Component) this).transform.Find("Amarture/Core/Controller_Body/hip").position;
        gameObject.transform.localScale = ((Component) this).transform.localScale;
        Object.Destroy((Object) ((Component) this).gameObject);
      }
      else
      {
        if (!this.photonView.isMine)
          return;
        PhotonNetwork.Instantiate("FX/FXtitanDie", ((Component) this).transform.Find("Amarture/Core/Controller_Body/hip").position, Quaternion.Euler(-90f, 0.0f, 0.0f), 0).transform.localScale = ((Component) this).transform.localScale;
        PhotonNetwork.Destroy(((Component) this).gameObject);
      }
    }
    else
    {
      if ((double) this.attention > 0.0)
      {
        this.attention -= Time.deltaTime;
        if ((double) this.attention < 0.0)
        {
          this.attention = 0.0f;
          GameObject[] gameObjectsWithTag = GameObject.FindGameObjectsWithTag("Player");
          this.myHero = gameObjectsWithTag[Random.Range(0, gameObjectsWithTag.Length)];
          this.attention = Random.Range(5f, 10f);
        }
      }
      if (Object.op_Inequality((Object) this.whoHasTauntMe, (Object) null))
      {
        this.tauntTime -= Time.deltaTime;
        if ((double) this.tauntTime <= 0.0)
          this.whoHasTauntMe = (GameObject) null;
        this.myHero = this.whoHasTauntMe;
      }
      if (Object.op_Inequality((Object) this.eren, (Object) null))
      {
        if (!this.eren.GetComponent<TITAN_EREN>().hasDied)
        {
          this.myHero = this.eren;
        }
        else
        {
          this.eren = (GameObject) null;
          this.myHero = (GameObject) null;
        }
      }
      if (Object.op_Equality((Object) this.myHero, (Object) null))
      {
        this.findNearestHero();
        if (Object.op_Inequality((Object) this.myHero, (Object) null))
          return;
      }
      this.myDistance = !Object.op_Equality((Object) this.myHero, (Object) null) ? Mathf.Sqrt((float) (((double) this.myHero.transform.position.x - (double) ((Component) this).transform.position.x) * ((double) this.myHero.transform.position.x - (double) ((Component) this).transform.position.x) + ((double) this.myHero.transform.position.z - (double) ((Component) this).transform.position.z) * ((double) this.myHero.transform.position.z - (double) ((Component) this).transform.position.z))) : float.MaxValue;
      if (this.state == "idle")
      {
        if (!Object.op_Inequality((Object) this.myHero, (Object) null))
          return;
        Vector3 vector3 = Vector3.op_Subtraction(this.myHero.transform.position, ((Component) this).transform.position);
        double num1 = -(double) Mathf.Atan2(vector3.z, vector3.x) * 57.295780181884766;
        Quaternion rotation = ((Component) this).gameObject.transform.rotation;
        double num2 = (double) ((Quaternion) ref rotation).eulerAngles.y - 90.0;
        float d = -Mathf.DeltaAngle((float) num1, (float) num2);
        if (this.attackTarget(this.myHero))
          return;
        if ((double) Mathf.Abs(d) < 90.0)
          this.chase();
        else if (Random.Range(0, 100) < 1)
          this.turn180();
        else if ((double) Mathf.Abs(d) > 100.0)
        {
          if (Random.Range(0, 100) >= 10)
            return;
          this.turn180();
        }
        else
        {
          if ((double) Mathf.Abs(d) <= 45.0 || Random.Range(0, 100) >= 30)
            return;
          this.turn(d);
        }
      }
      else if (this.state == "attack")
      {
        if (!this.attacked && (double) this.attackCheckTime != 0.0 && (double) ((Component) this).animation["attack_" + this.attackAnimation].normalizedTime >= (double) this.attackCheckTime)
        {
          this.attacked = true;
          this.fxPosition = ((Component) this).transform.Find("ap_" + this.attackAnimation).position;
          GameObject gameObject = IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.MULTIPLAYER || !PhotonNetwork.isMasterClient ? (GameObject) Object.Instantiate(Resources.Load("FX/" + this.fxName), this.fxPosition, this.fxRotation) : PhotonNetwork.Instantiate("FX/" + this.fxName, this.fxPosition, this.fxRotation, 0);
          gameObject.transform.localScale = ((Component) this).transform.localScale;
          float num = Mathf.Min(1f, (float) (1.0 - (double) Vector3.Distance(this.currentCamera.transform.position, gameObject.transform.position) * 0.05000000074505806));
          this.currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().startShake(num, num);
        }
        if ((double) this.attackCheckTimeA != 0.0 && ((double) ((Component) this).animation["attack_" + this.attackAnimation].normalizedTime >= (double) this.attackCheckTimeA && (double) ((Component) this).animation["attack_" + this.attackAnimation].normalizedTime <= (double) this.attackCheckTimeB || !this.attackChkOnce && (double) ((Component) this).animation["attack_" + this.attackAnimation].normalizedTime >= (double) this.attackCheckTimeA))
        {
          if (!this.attackChkOnce)
          {
            this.attackChkOnce = true;
            this.playSound("snd_eren_swing" + Random.Range(1, 3).ToString());
          }
          foreach (RaycastHit raycastHit in this.checkHitCapsule(this.checkHitCapsuleStart.position, this.checkHitCapsuleEnd.position, this.checkHitCapsuleR))
          {
            GameObject gameObject = ((Component) ((RaycastHit) ref raycastHit).collider).gameObject;
            if (gameObject.tag == "Player")
              this.killPlayer(gameObject);
            if (gameObject.tag == "erenHitbox")
            {
              if (this.attackAnimation == "combo_1")
              {
                if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER && PhotonNetwork.isMasterClient)
                  ((Component) gameObject.transform.root).gameObject.GetComponent<TITAN_EREN>().hitByFTByServer(1);
              }
              else if (this.attackAnimation == "combo_2")
              {
                if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER && PhotonNetwork.isMasterClient)
                  ((Component) gameObject.transform.root).gameObject.GetComponent<TITAN_EREN>().hitByFTByServer(2);
              }
              else if (this.attackAnimation == "combo_3" && IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER && PhotonNetwork.isMasterClient)
                ((Component) gameObject.transform.root).gameObject.GetComponent<TITAN_EREN>().hitByFTByServer(3);
            }
          }
          foreach (RaycastHit raycastHit in this.checkHitCapsule(this.checkHitCapsuleEndOld, this.checkHitCapsuleEnd.position, this.checkHitCapsuleR))
          {
            GameObject gameObject = ((Component) ((RaycastHit) ref raycastHit).collider).gameObject;
            if (gameObject.tag == "Player")
              this.killPlayer(gameObject);
          }
          this.checkHitCapsuleEndOld = this.checkHitCapsuleEnd.position;
        }
        if (this.attackAnimation == "jumpCombo_1" && (double) ((Component) this).animation["attack_" + this.attackAnimation].normalizedTime >= 0.64999997615814209 && !this.startJump && Object.op_Inequality((Object) this.myHero, (Object) null))
        {
          this.startJump = true;
          float y1 = this.myHero.rigidbody.velocity.y;
          float num3 = -20f;
          float gravity = this.gravity;
          float y2 = ((Component) this).transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck").position.y;
          float num4 = (float) (((double) num3 - (double) gravity) * 0.5);
          float num5 = y1;
          float num6 = this.myHero.transform.position.y - y2;
          float num7 = Mathf.Abs((float) (((double) Mathf.Sqrt((float) ((double) num5 * (double) num5 - 4.0 * (double) num4 * (double) num6)) - (double) num5) / (2.0 * (double) num4)));
          Vector3 vector3_1 = Vector3.op_Addition(Vector3.op_Addition(this.myHero.transform.position, Vector3.op_Multiply(this.myHero.rigidbody.velocity, num7)), Vector3.op_Multiply(Vector3.op_Multiply(Vector3.op_Multiply(Vector3.op_Multiply(Vector3.up, 0.5f), num3), num7), num7));
          float y3 = vector3_1.y;
          if ((double) num6 < 0.0 || (double) y3 - (double) y2 < 0.0)
          {
            this.idle();
            num7 = 0.5f;
            vector3_1 = Vector3.op_Addition(((Component) this).transform.position, Vector3.op_Multiply(y2 + 5f, Vector3.up));
            y3 = vector3_1.y;
          }
          float num8 = Mathf.Clamp((float) ((double) this.gravity * (double) Mathf.Sqrt(2f * (y3 - y2) / this.gravity) + 20.0), 20f, 90f);
          Vector3 vector3_2 = Vector3.op_Division(Vector3.op_Subtraction(vector3_1, ((Component) this).transform.position), num7);
          this.abnorma_jump_bite_horizon_v = new Vector3(vector3_2.x, 0.0f, vector3_2.z);
          Vector3 velocity = ((Component) this).rigidbody.velocity;
          Vector3 vector3_3;
          // ISSUE: explicit constructor call
          ((Vector3) ref vector3_3).\u002Ector(this.abnorma_jump_bite_horizon_v.x, num8, this.abnorma_jump_bite_horizon_v.z);
          if ((double) ((Vector3) ref vector3_3).magnitude > 90.0)
            vector3_3 = Vector3.op_Multiply(((Vector3) ref vector3_3).normalized, 90f);
          ((Component) this).rigidbody.AddForce(Vector3.op_Subtraction(vector3_3, velocity), (ForceMode) 2);
          Vector2.Angle(new Vector2(((Component) this).transform.position.x, ((Component) this).transform.position.z), new Vector2(this.myHero.transform.position.x, this.myHero.transform.position.z));
          ((Component) this).gameObject.transform.rotation = Quaternion.Euler(0.0f, Mathf.Atan2(this.myHero.transform.position.x - ((Component) this).transform.position.x, this.myHero.transform.position.z - ((Component) this).transform.position.z) * 57.29578f, 0.0f);
        }
        if (this.attackAnimation == "jumpCombo_3")
        {
          if ((double) ((Component) this).animation["attack_" + this.attackAnimation].normalizedTime < 1.0 || !this.IsGrounded())
            return;
          this.attack("jumpCombo_4");
        }
        else
        {
          if ((double) ((Component) this).animation["attack_" + this.attackAnimation].normalizedTime < 1.0)
            return;
          if (this.nextAttackAnimation != null)
          {
            this.attack(this.nextAttackAnimation);
            if (!Object.op_Inequality((Object) this.eren, (Object) null))
              return;
            Transform transform = ((Component) this).gameObject.transform;
            Quaternion quaternion1 = Quaternion.LookRotation(Vector3.op_Subtraction(this.eren.transform.position, ((Component) this).transform.position));
            Quaternion quaternion2 = Quaternion.Euler(0.0f, ((Quaternion) ref quaternion1).eulerAngles.y, 0.0f);
            transform.rotation = quaternion2;
          }
          else
          {
            this.findNearestHero();
            this.idle();
          }
        }
      }
      else if (this.state == "grab")
      {
        if ((double) ((Component) this).animation["attack_grab_" + this.attackAnimation].normalizedTime >= (double) this.attackCheckTimeA && (double) ((Component) this).animation["attack_grab_" + this.attackAnimation].normalizedTime <= (double) this.attackCheckTimeB && Object.op_Equality((Object) this.grabbedTarget, (Object) null))
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
        if ((double) ((Component) this).animation["attack_grab_" + this.attackAnimation].normalizedTime > (double) this.attackCheckTime && Object.op_Inequality((Object) this.grabbedTarget, (Object) null))
        {
          this.justEatHero(this.grabbedTarget, this.currentGrabHand);
          this.grabbedTarget = (GameObject) null;
        }
        if ((double) ((Component) this).animation["attack_grab_" + this.attackAnimation].normalizedTime < 1.0)
          return;
        this.idle();
      }
      else if (this.state == "turn")
      {
        ((Component) this).gameObject.transform.rotation = Quaternion.Lerp(((Component) this).gameObject.transform.rotation, Quaternion.Euler(0.0f, this.desDeg, 0.0f), (float) ((double) Time.deltaTime * (double) Mathf.Abs(this.turnDeg) * 0.10000000149011612));
        if ((double) ((Component) this).animation[this.turnAnimation].normalizedTime < 1.0)
          return;
        this.idle();
      }
      else if (this.state == "chase")
      {
        if (!Object.op_Equality((Object) this.eren, (Object) null) && (double) this.myDistance < 35.0 && this.attackTarget(this.myHero) || (double) this.getNearestHeroDistance() < 50.0 && Random.Range(0, 100) < 20 && this.attackTarget(this.getNearestHero()) || (double) this.myDistance >= (double) this.attackDistance - 15.0)
          return;
        this.idle(Random.Range(0.05f, 0.2f));
      }
      else if (this.state == "turn180")
      {
        if ((double) ((Component) this).animation[this.turnAnimation].normalizedTime < 1.0)
          return;
        Transform transform = ((Component) this).gameObject.transform;
        Quaternion rotation1 = ((Component) this).gameObject.transform.rotation;
        double x = (double) ((Quaternion) ref rotation1).eulerAngles.x;
        Quaternion rotation2 = ((Component) this).gameObject.transform.rotation;
        double num = (double) ((Quaternion) ref rotation2).eulerAngles.y + 180.0;
        Quaternion rotation3 = ((Component) this).gameObject.transform.rotation;
        double z = (double) ((Quaternion) ref rotation3).eulerAngles.z;
        Quaternion quaternion = Quaternion.Euler((float) x, (float) num, (float) z);
        transform.rotation = quaternion;
        this.idle();
        this.playAnimation("idle");
      }
      else
      {
        if (!(this.state == "anklehurt"))
          return;
        if ((double) ((Component) this).animation["legHurt"].normalizedTime >= 1.0)
          this.crossFade("legHurt_loop", 0.2f);
        if ((double) ((Component) this).animation["legHurt_loop"].normalizedTime >= 3.0)
          this.crossFade("legHurt_getup", 0.2f);
        if ((double) ((Component) this).animation["legHurt_getup"].normalizedTime < 1.0)
          return;
        this.idle();
        this.playAnimation("idle");
      }
    }
  }

  public void updateLabel()
  {
    if (!Object.op_Inequality((Object) this.healthLabel, (Object) null) || !this.healthLabel.GetComponent<UILabel>().isVisible)
      return;
    this.healthLabel.transform.LookAt(Vector3.op_Subtraction(Vector3.op_Multiply(2f, this.healthLabel.transform.position), ((Component) Camera.main).transform.position));
  }
}
