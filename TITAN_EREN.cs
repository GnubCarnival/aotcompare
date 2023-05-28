// Decompiled with JetBrains decompiler
// Type: TITAN_EREN
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using CustomSkins;
using Photon;
using Settings;
using System;
using System.Collections;
using UI;
using UnityEngine;

public class TITAN_EREN : MonoBehaviour
{
  private string attackAnimation;
  private Transform attackBox;
  private bool attackChkOnce;
  public GameObject bottomObject;
  public bool canJump = true;
  private ArrayList checkPoints = new ArrayList();
  public Camera currentCamera;
  private Vector3 dashDirection;
  private float dieTime;
  private float facingDirection;
  private float gravity = 500f;
  private bool grounded;
  public bool hasDied;
  private bool hasDieSteam;
  public bool hasSpawn;
  private string hitAnimation;
  private float hitPause;
  private ArrayList hitTargets;
  private bool isAttack;
  public bool isHit;
  private bool isHitWhileCarryingRock;
  private bool isNextAttack;
  private bool isPlayRoar;
  private bool isROCKMOVE;
  public float jumpHeight = 2f;
  private bool justGrounded;
  public float lifeTime = 9999f;
  private float lifeTimeMax = 9999f;
  public float maxVelocityChange = 100f;
  private GameObject myNetWorkName;
  private float myR;
  private bool needFreshCorePosition;
  private bool needRoar;
  private Vector3 oldCorePosition;
  public GameObject realBody;
  public GameObject rock;
  private bool rockHitGround;
  public bool rockLift;
  private int rockPhase;
  public float speed = 80f;
  private float sqrt2 = Mathf.Sqrt(2f);
  private int stepSoundPhase = 2;
  private Vector3 targetCheckPt;
  private float waitCounter;
  private ErenCustomSkinLoader _customSkinLoader;

  private void Awake() => this._customSkinLoader = ((Component) this).gameObject.AddComponent<ErenCustomSkinLoader>();

  public void born()
  {
    foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("titan"))
    {
      if (Object.op_Inequality((Object) gameObject.GetComponent<FEMALE_TITAN>(), (Object) null))
        gameObject.GetComponent<FEMALE_TITAN>().erenIsHere(((Component) this).gameObject);
    }
    if (!this.bottomObject.GetComponent<CheckHitGround>().isGrounded)
    {
      this.playAnimation("jump_air");
      this.needRoar = true;
    }
    else
    {
      this.needRoar = false;
      this.playAnimation(nameof (born));
      this.isPlayRoar = false;
    }
    this.playSound("snd_eren_shift");
    if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
      Object.Instantiate(Resources.Load("FX/Thunder"), Vector3.op_Addition(((Component) this).transform.position, Vector3.op_Multiply(Vector3.up, 23f)), Quaternion.Euler(270f, 0.0f, 0.0f));
    else if (this.photonView.isMine)
      PhotonNetwork.Instantiate("FX/Thunder", Vector3.op_Addition(((Component) this).transform.position, Vector3.op_Multiply(Vector3.up, 23f)), Quaternion.Euler(270f, 0.0f, 0.0f), 0);
    this.lifeTimeMax = this.lifeTime = 30f;
  }

  private void crossFade(string aniName, float time)
  {
    ((Component) this).animation.CrossFade(aniName, time);
    if (!PhotonNetwork.connected || !this.photonView.isMine)
      return;
    this.photonView.RPC("netCrossFade", PhotonTargets.Others, (object) aniName, (object) time);
  }

  [RPC]
  private void endMovingRock() => this.isROCKMOVE = false;

  private void falseAttack()
  {
    this.isAttack = false;
    this.isNextAttack = false;
    this.hitTargets = new ArrayList();
    this.attackChkOnce = false;
  }

  private void FixedUpdate()
  {
    if (GameMenu.Paused && IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
      return;
    if (this.rockLift)
    {
      this.RockUpdate();
    }
    else
    {
      if (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE && !this.photonView.isMine)
        return;
      if ((double) this.hitPause > 0.0)
        ((Component) this).rigidbody.velocity = Vector3.zero;
      else if (this.hasDied)
      {
        ((Component) this).rigidbody.velocity = Vector3.op_Addition(Vector3.zero, Vector3.op_Multiply(Vector3.up, ((Component) this).rigidbody.velocity.y));
        ((Component) this).rigidbody.AddForce(new Vector3(0.0f, -this.gravity * ((Component) this).rigidbody.mass, 0.0f));
      }
      else
      {
        if (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE && !this.photonView.isMine)
          return;
        Vector3 velocity1 = ((Component) this).rigidbody.velocity;
        if ((double) ((Vector3) ref velocity1).magnitude > 50.0)
        {
          Camera component = ((Component) this.currentCamera).GetComponent<Camera>();
          double fieldOfView = (double) ((Component) this.currentCamera).GetComponent<Camera>().fieldOfView;
          Vector3 velocity2 = ((Component) this).rigidbody.velocity;
          double num1 = (double) Mathf.Min(100f, ((Vector3) ref velocity2).magnitude);
          double num2 = (double) Mathf.Lerp((float) fieldOfView, (float) num1, 0.1f);
          component.fieldOfView = (float) num2;
        }
        else
          ((Component) this.currentCamera).GetComponent<Camera>().fieldOfView = Mathf.Lerp(((Component) this.currentCamera).GetComponent<Camera>().fieldOfView, 50f, 0.1f);
        if (this.bottomObject.GetComponent<CheckHitGround>().isGrounded)
        {
          if (!this.grounded)
            this.justGrounded = true;
          this.grounded = true;
          this.bottomObject.GetComponent<CheckHitGround>().isGrounded = false;
        }
        else
          this.grounded = false;
        float num3 = 0.0f;
        float num4 = 0.0f;
        if (!IN_GAME_MAIN_CAMERA.isTyping)
        {
          num4 = !SettingsManager.InputSettings.General.Forward.GetKey() ? (!SettingsManager.InputSettings.General.Back.GetKey() ? 0.0f : -1f) : 1f;
          num3 = !SettingsManager.InputSettings.General.Left.GetKey() ? (!SettingsManager.InputSettings.General.Right.GetKey() ? 0.0f : 1f) : -1f;
        }
        if (this.needFreshCorePosition)
        {
          this.oldCorePosition = Vector3.op_Subtraction(((Component) this).transform.position, ((Component) this).transform.Find("Amarture/Core").position);
          this.needFreshCorePosition = false;
        }
        if (this.isAttack || this.isHit)
        {
          Vector3 vector3 = Vector3.op_Subtraction(Vector3.op_Subtraction(((Component) this).transform.position, ((Component) this).transform.Find("Amarture/Core").position), this.oldCorePosition);
          this.oldCorePosition = Vector3.op_Subtraction(((Component) this).transform.position, ((Component) this).transform.Find("Amarture/Core").position);
          ((Component) this).rigidbody.velocity = Vector3.op_Addition(Vector3.op_Division(vector3, Time.deltaTime), Vector3.op_Multiply(Vector3.up, ((Component) this).rigidbody.velocity.y));
          ((Component) this).rigidbody.rotation = Quaternion.Lerp(((Component) this).gameObject.transform.rotation, Quaternion.Euler(0.0f, this.facingDirection, 0.0f), Time.deltaTime * 10f);
          if (this.justGrounded)
            this.justGrounded = false;
        }
        else if (this.grounded)
        {
          Vector3 vector3_1 = Vector3.zero;
          if (this.justGrounded)
          {
            this.justGrounded = false;
            vector3_1 = ((Component) this).rigidbody.velocity;
            if (((Component) this).animation.IsPlaying("jump_air"))
            {
              ((GameObject) Object.Instantiate(Resources.Load("FX/boom2_eren"), ((Component) this).transform.position, Quaternion.Euler(270f, 0.0f, 0.0f))).transform.localScale = Vector3.op_Multiply(Vector3.one, 1.5f);
              if (this.needRoar)
              {
                this.playAnimation("born");
                this.needRoar = false;
                this.isPlayRoar = false;
              }
              else
                this.playAnimation("jump_land");
            }
          }
          if (!((Component) this).animation.IsPlaying("jump_land") && !this.isAttack && !this.isHit && !((Component) this).animation.IsPlaying("born"))
          {
            Vector3 vector3_2;
            // ISSUE: explicit constructor call
            ((Vector3) ref vector3_2).\u002Ector(num3, 0.0f, num4);
            Quaternion rotation = ((Component) this.currentCamera).transform.rotation;
            float num5 = ((Quaternion) ref rotation).eulerAngles.y + (float) (-(double) (Mathf.Atan2(num4, num3) * 57.29578f) + 90.0);
            float num6 = (float) (-(double) num5 + 90.0);
            float num7 = Mathf.Cos(num6 * ((float) Math.PI / 180f));
            float num8 = Mathf.Sin(num6 * ((float) Math.PI / 180f));
            // ISSUE: explicit constructor call
            ((Vector3) ref vector3_1).\u002Ector(num7, 0.0f, num8);
            float num9 = (double) ((Vector3) ref vector3_2).magnitude <= 0.949999988079071 ? ((double) ((Vector3) ref vector3_2).magnitude >= 0.25 ? ((Vector3) ref vector3_2).magnitude : 0.0f) : 1f;
            vector3_1 = Vector3.op_Multiply(Vector3.op_Multiply(vector3_1, num9), this.speed);
            if ((double) num3 != 0.0 || (double) num4 != 0.0)
            {
              if (!((Component) this).animation.IsPlaying("run") && !((Component) this).animation.IsPlaying("jump_start") && !((Component) this).animation.IsPlaying("jump_air"))
                this.crossFade("run", 0.1f);
            }
            else
            {
              if (!((Component) this).animation.IsPlaying("idle") && !((Component) this).animation.IsPlaying("dash_land") && !((Component) this).animation.IsPlaying("dodge") && !((Component) this).animation.IsPlaying("jump_start") && !((Component) this).animation.IsPlaying("jump_air") && !((Component) this).animation.IsPlaying("jump_land"))
              {
                this.crossFade("idle", 0.1f);
                vector3_1 = Vector3.op_Multiply(vector3_1, 0.0f);
              }
              num5 = -874f;
            }
            if ((double) num5 != -874.0)
              this.facingDirection = num5;
          }
          Vector3 velocity3 = ((Component) this).rigidbody.velocity;
          Vector3 vector3_3 = Vector3.op_Subtraction(vector3_1, velocity3);
          vector3_3.x = Mathf.Clamp(vector3_3.x, -this.maxVelocityChange, this.maxVelocityChange);
          vector3_3.z = Mathf.Clamp(vector3_3.z, -this.maxVelocityChange, this.maxVelocityChange);
          vector3_3.y = 0.0f;
          if (((Component) this).animation.IsPlaying("jump_start") && (double) ((Component) this).animation["jump_start"].normalizedTime >= 1.0)
          {
            this.playAnimation("jump_air");
            vector3_3.y += 240f;
          }
          else if (((Component) this).animation.IsPlaying("jump_start"))
            vector3_3 = Vector3.op_UnaryNegation(((Component) this).rigidbody.velocity);
          ((Component) this).rigidbody.AddForce(vector3_3, (ForceMode) 2);
          ((Component) this).rigidbody.rotation = Quaternion.Lerp(((Component) this).gameObject.transform.rotation, Quaternion.Euler(0.0f, this.facingDirection, 0.0f), Time.deltaTime * 10f);
        }
        else
        {
          if (((Component) this).animation.IsPlaying("jump_start") && (double) ((Component) this).animation["jump_start"].normalizedTime >= 1.0)
          {
            this.playAnimation("jump_air");
            ((Component) this).rigidbody.AddForce(Vector3.op_Multiply(Vector3.up, 240f), (ForceMode) 2);
          }
          if (!((Component) this).animation.IsPlaying("jump") && !this.isHit)
          {
            Vector3 vector3_4;
            // ISSUE: explicit constructor call
            ((Vector3) ref vector3_4).\u002Ector(num3, 0.0f, num4);
            Quaternion rotation = ((Component) this.currentCamera).transform.rotation;
            float num10 = ((Quaternion) ref rotation).eulerAngles.y + (float) (-(double) (Mathf.Atan2(num4, num3) * 57.29578f) + 90.0);
            float num11 = (float) (-(double) num10 + 90.0);
            float num12 = Mathf.Cos(num11 * ((float) Math.PI / 180f));
            float num13 = Mathf.Sin(num11 * ((float) Math.PI / 180f));
            Vector3 vector3_5;
            // ISSUE: explicit constructor call
            ((Vector3) ref vector3_5).\u002Ector(num12, 0.0f, num13);
            float num14 = (double) ((Vector3) ref vector3_4).magnitude <= 0.949999988079071 ? ((double) ((Vector3) ref vector3_4).magnitude >= 0.25 ? ((Vector3) ref vector3_4).magnitude : 0.0f) : 1f;
            Vector3 vector3_6 = Vector3.op_Multiply(Vector3.op_Multiply(vector3_5, num14), this.speed * 2f);
            if ((double) num3 != 0.0 || (double) num4 != 0.0)
              ((Component) this).rigidbody.AddForce(vector3_6, (ForceMode) 1);
            else
              num10 = -874f;
            if ((double) num10 != -874.0)
              this.facingDirection = num10;
            if (!((Component) this).animation.IsPlaying(string.Empty) && !((Component) this).animation.IsPlaying("attack3_2") && !((Component) this).animation.IsPlaying("attack5"))
              ((Component) this).rigidbody.rotation = Quaternion.Lerp(((Component) this).gameObject.transform.rotation, Quaternion.Euler(0.0f, this.facingDirection, 0.0f), Time.deltaTime * 6f);
          }
        }
        ((Component) this).rigidbody.AddForce(new Vector3(0.0f, -this.gravity * ((Component) this).rigidbody.mass, 0.0f));
      }
    }
  }

  public void hitByFT(int phase)
  {
    if (this.hasDied)
      return;
    this.isHit = true;
    this.hitAnimation = "hit_annie_" + phase.ToString();
    this.falseAttack();
    this.playAnimation(this.hitAnimation);
    this.needFreshCorePosition = true;
    if (phase != 3)
      return;
    this.hasDied = true;
    Transform transform = ((Component) this).transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck");
    (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.MULTIPLAYER || !PhotonNetwork.isMasterClient ? (GameObject) Object.Instantiate(Resources.Load("bloodExplore"), Vector3.op_Addition(transform.position, Vector3.op_Multiply(Vector3.op_Multiply(Vector3.up, 1f), 4f)), Quaternion.Euler(270f, 0.0f, 0.0f)) : PhotonNetwork.Instantiate("bloodExplore", Vector3.op_Addition(transform.position, Vector3.op_Multiply(Vector3.op_Multiply(Vector3.up, 1f), 4f)), Quaternion.Euler(270f, 0.0f, 0.0f), 0)).transform.localScale = ((Component) this).transform.localScale;
    GameObject gameObject;
    if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER && PhotonNetwork.isMasterClient)
    {
      Vector3 position = transform.position;
      Quaternion rotation1 = transform.rotation;
      double num = 90.0 + (double) ((Quaternion) ref rotation1).eulerAngles.x;
      Quaternion rotation2 = transform.rotation;
      double y = (double) ((Quaternion) ref rotation2).eulerAngles.y;
      Quaternion rotation3 = transform.rotation;
      double z = (double) ((Quaternion) ref rotation3).eulerAngles.z;
      Quaternion rotation4 = Quaternion.Euler((float) num, (float) y, (float) z);
      gameObject = PhotonNetwork.Instantiate("bloodsplatter", position, rotation4, 0);
    }
    else
    {
      Object @object = Resources.Load("bloodsplatter");
      Vector3 position = transform.position;
      Quaternion rotation5 = transform.rotation;
      double num = 90.0 + (double) ((Quaternion) ref rotation5).eulerAngles.x;
      Quaternion rotation6 = transform.rotation;
      double y = (double) ((Quaternion) ref rotation6).eulerAngles.y;
      Quaternion rotation7 = transform.rotation;
      double z = (double) ((Quaternion) ref rotation7).eulerAngles.z;
      Quaternion quaternion = Quaternion.Euler((float) num, (float) y, (float) z);
      gameObject = (GameObject) Object.Instantiate(@object, position, quaternion);
    }
    gameObject.transform.localScale = ((Component) this).transform.localScale;
    gameObject.transform.parent = transform;
    (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.MULTIPLAYER || !PhotonNetwork.isMasterClient ? (GameObject) Object.Instantiate(Resources.Load("FX/justSmoke"), transform.position, Quaternion.Euler(270f, 0.0f, 0.0f)) : PhotonNetwork.Instantiate("FX/justSmoke", transform.position, Quaternion.Euler(270f, 0.0f, 0.0f), 0)).transform.parent = transform;
  }

  public void hitByFTByServer(int phase) => this.photonView.RPC("hitByFTRPC", PhotonTargets.All, (object) phase);

  [RPC]
  private void hitByFTRPC(int phase)
  {
    if (!this.photonView.isMine)
      return;
    this.hitByFT(phase);
  }

  public void hitByTitan()
  {
    if (this.isHit || this.hasDied || ((Component) this).animation.IsPlaying("born"))
      return;
    if (this.rockLift)
    {
      this.crossFade("die", 0.1f);
      this.isHitWhileCarryingRock = true;
      GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().gameLose2();
      this.photonView.RPC("rockPlayAnimation", PhotonTargets.All, (object) "set");
    }
    else
    {
      this.isHit = true;
      this.hitAnimation = "hit_titan";
      this.falseAttack();
      this.playAnimation(this.hitAnimation);
      this.needFreshCorePosition = true;
    }
  }

  public void hitByTitanByServer() => this.photonView.RPC("hitByTitanRPC", PhotonTargets.All);

  [RPC]
  private void hitByTitanRPC()
  {
    if (!this.photonView.isMine)
      return;
    this.hitByTitan();
  }

  public bool IsGrounded() => this.bottomObject.GetComponent<CheckHitGround>().isGrounded;

  public void lateUpdate()
  {
    if (GameMenu.Paused && IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE || this.rockLift || IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE && !this.photonView.isMine)
      return;
    Quaternion rotation1 = GameObject.Find("MainCamera").transform.rotation;
    double x = (double) ((Quaternion) ref rotation1).eulerAngles.x;
    Quaternion rotation2 = GameObject.Find("MainCamera").transform.rotation;
    double y = (double) ((Quaternion) ref rotation2).eulerAngles.y;
    Quaternion quaternion = Quaternion.Euler((float) x, (float) y, 0.0f);
    GameObject.Find("MainCamera").transform.rotation = Quaternion.Lerp(GameObject.Find("MainCamera").transform.rotation, quaternion, Time.deltaTime * 2f);
  }

  public void loadskin()
  {
    BaseCustomSkinSettings<ShifterCustomSkinSet> shifter = SettingsManager.CustomSkinSettings.Shifter;
    string url = ((ShifterCustomSkinSet) shifter.GetSelectedSet()).Eren.Value;
    if (!TextureDownloader.ValidTextureURL(url))
      return;
    if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
    {
      if (!shifter.SkinsEnabled.Value)
        return;
      this.StartCoroutine(this.loadskinE(url));
    }
    else
    {
      if (!this.photonView.isMine || !shifter.SkinsEnabled.Value)
        return;
      this.photonView.RPC("loadskinRPC", PhotonTargets.AllBuffered, (object) url);
    }
  }

  public IEnumerator loadskinE(string url)
  {
    TITAN_EREN titanEren = this;
    while (!titanEren.hasSpawn)
      yield return (object) null;
    yield return (object) titanEren.StartCoroutine(titanEren._customSkinLoader.LoadSkinsFromRPC(new object[1]
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
  private void netPlayAnimation(string aniName) => ((Component) this).animation.Play(aniName);

  [RPC]
  private void netPlayAnimationAt(string aniName, float normalizedTime)
  {
    ((Component) this).animation.Play(aniName);
    ((Component) this).animation[aniName].normalizedTime = normalizedTime;
  }

  [RPC]
  private void netTauntAttack(float tauntTime, float distance = 100f)
  {
    foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("titan"))
    {
      if ((double) Vector3.Distance(gameObject.transform.position, ((Component) this).transform.position) < (double) distance && Object.op_Inequality((Object) gameObject.GetComponent<TITAN>(), (Object) null))
        gameObject.GetComponent<TITAN>().beTauntedBy(((Component) this).gameObject, tauntTime);
      if (Object.op_Inequality((Object) gameObject.GetComponent<FEMALE_TITAN>(), (Object) null))
        gameObject.GetComponent<FEMALE_TITAN>().erenIsHere(((Component) this).gameObject);
    }
  }

  private void OnDestroy()
  {
    if (!Object.op_Inequality((Object) GameObject.Find("MultiplayerManager"), (Object) null))
      return;
    GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().removeET(this);
  }

  public void playAnimation(string aniName)
  {
    ((Component) this).animation.Play(aniName);
    if (!PhotonNetwork.connected || !this.photonView.isMine)
      return;
    this.photonView.RPC("netPlayAnimation", PhotonTargets.Others, (object) aniName);
  }

  private void playAnimationAt(string aniName, float normalizedTime)
  {
    ((Component) this).animation.Play(aniName);
    ((Component) this).animation[aniName].normalizedTime = normalizedTime;
    if (!PhotonNetwork.connected || !this.photonView.isMine)
      return;
    this.photonView.RPC("netPlayAnimationAt", PhotonTargets.Others, (object) aniName, (object) normalizedTime);
  }

  private void playSound(string sndname)
  {
    this.playsoundRPC(sndname);
    if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
      return;
    this.photonView.RPC("playsoundRPC", PhotonTargets.Others, (object) sndname);
  }

  [RPC]
  private void playsoundRPC(string sndname) => ((Component) ((Component) this).transform.Find(sndname)).GetComponent<AudioSource>().Play();

  [RPC]
  private void removeMe(PhotonMessageInfo info)
  {
    if (info.sender != this.photonView.owner)
    {
      FengGameManagerMKII.instance.kickPlayerRCIfMC(info.sender, true, "titan eren remove");
    }
    else
    {
      PhotonNetwork.RemoveRPCs(this.photonView);
      Object.Destroy((Object) ((Component) this).gameObject);
    }
  }

  [RPC]
  private void rockPlayAnimation(string anim)
  {
    this.rock.animation.Play(anim);
    this.rock.animation[anim].speed = 1f;
  }

  private void RockUpdate()
  {
    if (this.isHitWhileCarryingRock)
      return;
    if (this.isROCKMOVE)
    {
      this.rock.transform.position = ((Component) this).transform.position;
      this.rock.transform.rotation = ((Component) this).transform.rotation;
    }
    if (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE && !this.photonView.isMine)
      return;
    if (this.rockPhase == 0)
    {
      ((Component) this).rigidbody.AddForce(Vector3.op_UnaryNegation(((Component) this).rigidbody.velocity), (ForceMode) 2);
      ((Component) this).rigidbody.AddForce(new Vector3(0.0f, -10f * ((Component) this).rigidbody.mass, 0.0f));
      this.waitCounter += Time.deltaTime;
      if ((double) this.waitCounter <= 20.0)
        return;
      ++this.rockPhase;
      this.crossFade("idle", 1f);
      this.waitCounter = 0.0f;
      this.setRoute();
    }
    else if (this.rockPhase == 1)
    {
      ((Component) this).rigidbody.AddForce(Vector3.op_UnaryNegation(((Component) this).rigidbody.velocity), (ForceMode) 2);
      ((Component) this).rigidbody.AddForce(new Vector3(0.0f, -this.gravity * ((Component) this).rigidbody.mass, 0.0f));
      this.waitCounter += Time.deltaTime;
      if ((double) this.waitCounter <= 2.0)
        return;
      ++this.rockPhase;
      this.crossFade("run", 0.2f);
      this.waitCounter = 0.0f;
    }
    else if (this.rockPhase == 2)
    {
      Vector3 vector3 = Vector3.op_Subtraction(Vector3.op_Multiply(((Component) this).transform.forward, 30f), ((Component) this).rigidbody.velocity);
      vector3.x = Mathf.Clamp(vector3.x, -this.maxVelocityChange, this.maxVelocityChange);
      vector3.z = Mathf.Clamp(vector3.z, -this.maxVelocityChange, this.maxVelocityChange);
      vector3.y = 0.0f;
      ((Component) this).rigidbody.AddForce(vector3, (ForceMode) 2);
      if ((double) ((Component) this).transform.position.z >= -238.0)
        return;
      ((Component) this).transform.position = new Vector3(((Component) this).transform.position.x, 0.0f, -238f);
      ++this.rockPhase;
      this.crossFade("idle", 0.2f);
      this.waitCounter = 0.0f;
    }
    else if (this.rockPhase == 3)
    {
      ((Component) this).rigidbody.AddForce(Vector3.op_UnaryNegation(((Component) this).rigidbody.velocity), (ForceMode) 2);
      ((Component) this).rigidbody.AddForce(new Vector3(0.0f, -10f * ((Component) this).rigidbody.mass, 0.0f));
      this.waitCounter += Time.deltaTime;
      if ((double) this.waitCounter <= 1.0)
        return;
      ++this.rockPhase;
      this.crossFade("rock_lift", 0.1f);
      this.photonView.RPC("rockPlayAnimation", PhotonTargets.All, (object) "lift");
      this.waitCounter = 0.0f;
      this.targetCheckPt = (Vector3) this.checkPoints[0];
    }
    else if (this.rockPhase == 4)
    {
      ((Component) this).rigidbody.AddForce(Vector3.op_UnaryNegation(((Component) this).rigidbody.velocity), (ForceMode) 2);
      ((Component) this).rigidbody.AddForce(new Vector3(0.0f, -this.gravity * ((Component) this).rigidbody.mass, 0.0f));
      this.waitCounter += Time.deltaTime;
      if ((double) this.waitCounter <= 4.1999998092651367)
        return;
      ++this.rockPhase;
      this.crossFade("rock_walk", 0.1f);
      this.photonView.RPC("rockPlayAnimation", PhotonTargets.All, (object) "move");
      this.rock.animation["move"].normalizedTime = ((Component) this).animation["rock_walk"].normalizedTime;
      this.waitCounter = 0.0f;
      this.photonView.RPC("startMovingRock", PhotonTargets.All);
    }
    else if (this.rockPhase == 5)
    {
      if ((double) Vector3.Distance(((Component) this).transform.position, this.targetCheckPt) < 10.0)
      {
        if (this.checkPoints.Count > 0)
        {
          if (this.checkPoints.Count == 1)
          {
            ++this.rockPhase;
          }
          else
          {
            this.targetCheckPt = (Vector3) this.checkPoints[0];
            this.checkPoints.RemoveAt(0);
            GameObject[] gameObjectsWithTag = GameObject.FindGameObjectsWithTag("titanRespawn2");
            GameObject gameObject1 = GameObject.Find("titanRespawnTrost" + (7 - this.checkPoints.Count).ToString());
            if (Object.op_Inequality((Object) gameObject1, (Object) null))
            {
              foreach (GameObject gameObject2 in gameObjectsWithTag)
              {
                if (Object.op_Equality((Object) ((Component) gameObject2.transform.parent).gameObject, (Object) gameObject1))
                {
                  GameObject gameObject3 = GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().spawnTitan(70, gameObject2.transform.position, gameObject2.transform.rotation);
                  gameObject3.GetComponent<TITAN>().isAlarm = true;
                  gameObject3.GetComponent<TITAN>().chaseDistance = 999999f;
                }
              }
            }
          }
        }
        else
          ++this.rockPhase;
      }
      if (this.checkPoints.Count > 0 && Random.Range(0, 3000) < 10 - this.checkPoints.Count)
      {
        Vector3 vector3 = Vector3.op_Addition(((Component) this).transform.position, Quaternion.op_Multiply(Random.Range(0, 10) <= 5 ? Quaternion.op_Multiply(((Component) this).transform.rotation, Quaternion.Euler(0.0f, Random.Range(-30f, 30f), 0.0f)) : Quaternion.op_Multiply(((Component) this).transform.rotation, Quaternion.Euler(0.0f, Random.Range(150f, 210f), 0.0f)), new Vector3(Random.Range(100f, 200f), 0.0f, 0.0f)));
        LayerMask layerMask = LayerMask.op_Implicit(1 << LayerMask.NameToLayer("Ground"));
        float num = 0.0f;
        RaycastHit raycastHit;
        if (Physics.Raycast(Vector3.op_Addition(vector3, Vector3.op_Multiply(Vector3.up, 500f)), Vector3.op_UnaryNegation(Vector3.up), ref raycastHit, 1000f, ((LayerMask) ref layerMask).value))
          num = ((RaycastHit) ref raycastHit).point.y;
        Vector3 position = Vector3.op_Addition(vector3, Vector3.op_Multiply(Vector3.up, num));
        GameObject gameObject = GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().spawnTitan(70, position, ((Component) this).transform.rotation);
        gameObject.GetComponent<TITAN>().isAlarm = true;
        gameObject.GetComponent<TITAN>().chaseDistance = 999999f;
      }
      Vector3 vector3_1 = Vector3.op_Subtraction(Vector3.op_Multiply(((Component) this).transform.forward, 6f), ((Component) this).rigidbody.velocity);
      vector3_1.x = Mathf.Clamp(vector3_1.x, -this.maxVelocityChange, this.maxVelocityChange);
      vector3_1.z = Mathf.Clamp(vector3_1.z, -this.maxVelocityChange, this.maxVelocityChange);
      vector3_1.y = 0.0f;
      ((Component) this).rigidbody.AddForce(vector3_1, (ForceMode) 2);
      ((Component) this).rigidbody.AddForce(new Vector3(0.0f, -this.gravity * ((Component) this).rigidbody.mass, 0.0f));
      Vector3 vector3_2 = Vector3.op_Subtraction(this.targetCheckPt, ((Component) this).transform.position);
      double num1 = -(double) Mathf.Atan2(vector3_2.z, vector3_2.x) * 57.295780181884766;
      Quaternion rotation1 = ((Component) this).gameObject.transform.rotation;
      double num2 = (double) ((Quaternion) ref rotation1).eulerAngles.y - 90.0;
      float num3 = -Mathf.DeltaAngle((float) num1, (float) num2);
      Transform transform = ((Component) this).gameObject.transform;
      Quaternion rotation2 = ((Component) this).gameObject.transform.rotation;
      rotation1 = ((Component) this).gameObject.transform.rotation;
      Quaternion quaternion1 = Quaternion.Euler(0.0f, (float) ((double) ((Quaternion) ref rotation1).eulerAngles.y + (double) num3), 0.0f);
      double num4 = 0.800000011920929 * (double) Time.deltaTime;
      Quaternion quaternion2 = Quaternion.Lerp(rotation2, quaternion1, (float) num4);
      transform.rotation = quaternion2;
    }
    else if (this.rockPhase == 6)
    {
      ((Component) this).rigidbody.AddForce(Vector3.op_UnaryNegation(((Component) this).rigidbody.velocity), (ForceMode) 2);
      ((Component) this).rigidbody.AddForce(new Vector3(0.0f, -10f * ((Component) this).rigidbody.mass, 0.0f));
      ((Component) this).transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
      ++this.rockPhase;
      this.crossFade("rock_fix_hole", 0.1f);
      this.photonView.RPC("rockPlayAnimation", PhotonTargets.All, (object) "set");
      this.photonView.RPC("endMovingRock", PhotonTargets.All);
    }
    else
    {
      if (this.rockPhase != 7)
        return;
      ((Component) this).rigidbody.AddForce(Vector3.op_UnaryNegation(((Component) this).rigidbody.velocity), (ForceMode) 2);
      ((Component) this).rigidbody.AddForce(new Vector3(0.0f, -10f * ((Component) this).rigidbody.mass, 0.0f));
      if ((double) ((Component) this).animation["rock_fix_hole"].normalizedTime >= 1.2000000476837158)
      {
        this.crossFade("die", 0.1f);
        ++this.rockPhase;
        GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().gameWin2();
      }
      if ((double) ((Component) this).animation["rock_fix_hole"].normalizedTime < 0.62000000476837158 || this.rockHitGround)
        return;
      this.rockHitGround = true;
      if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER && PhotonNetwork.isMasterClient)
        PhotonNetwork.Instantiate("FX/boom1_CT_KICK", new Vector3(0.0f, 30f, 684f), Quaternion.Euler(270f, 0.0f, 0.0f), 0);
      else
        Object.Instantiate(Resources.Load("FX/boom1_CT_KICK"), new Vector3(0.0f, 30f, 684f), Quaternion.Euler(270f, 0.0f, 0.0f));
    }
  }

  public void setRoute()
  {
    GameObject gameObject = GameObject.Find("routeTrost");
    this.checkPoints = new ArrayList();
    for (int index = 1; index <= 7; ++index)
      this.checkPoints.Add((object) gameObject.transform.Find("r" + index.ToString()).position);
    this.checkPoints.Add((object) "end");
  }

  private void showAimUI()
  {
    GameObject gameObject1 = GameObject.Find("cross1");
    GameObject gameObject2 = GameObject.Find("cross2");
    GameObject gameObject3 = GameObject.Find("crossL1");
    GameObject gameObject4 = GameObject.Find("crossL2");
    GameObject gameObject5 = GameObject.Find("crossR1");
    GameObject gameObject6 = GameObject.Find("crossR2");
    GameObject gameObject7 = GameObject.Find("LabelDistance");
    Vector3 vector3 = Vector3.op_Multiply(Vector3.up, 10000f);
    gameObject6.transform.localPosition = vector3;
    gameObject5.transform.localPosition = vector3;
    gameObject4.transform.localPosition = vector3;
    gameObject3.transform.localPosition = vector3;
    gameObject7.transform.localPosition = vector3;
    gameObject2.transform.localPosition = vector3;
    gameObject1.transform.localPosition = vector3;
  }

  private void showSkillCD() => GameObject.Find("skill_cd_eren").GetComponent<UISprite>().fillAmount = this.lifeTime / this.lifeTimeMax;

  private void Start()
  {
    this.loadskin();
    FengGameManagerMKII.instance.addET(this);
    if (this.rockLift)
    {
      this.rock = GameObject.Find("rock");
      this.rock.animation["lift"].speed = 0.0f;
    }
    else
    {
      this.currentCamera = GameObject.Find("MainCamera").GetComponent<Camera>();
      this.oldCorePosition = Vector3.op_Subtraction(((Component) this).transform.position, ((Component) this).transform.Find("Amarture/Core").position);
      this.myR = this.sqrt2 * 6f;
      ((Component) this).animation["hit_annie_1"].speed = 0.8f;
      ((Component) this).animation["hit_annie_2"].speed = 0.7f;
      ((Component) this).animation["hit_annie_3"].speed = 0.7f;
    }
    this.hasSpawn = true;
  }

  [RPC]
  private void startMovingRock() => this.isROCKMOVE = true;

  public void update()
  {
    if (GameMenu.Paused && IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE || this.rockLift)
      return;
    if (((Component) this).animation.IsPlaying("run"))
    {
      if ((double) ((Component) this).animation["run"].normalizedTime % 1.0 > 0.30000001192092896 && (double) ((Component) this).animation["run"].normalizedTime % 1.0 < 0.75 && this.stepSoundPhase == 2)
      {
        this.stepSoundPhase = 1;
        Transform transform = ((Component) this).transform.Find("snd_eren_foot");
        ((Component) transform).GetComponent<AudioSource>().Stop();
        ((Component) transform).GetComponent<AudioSource>().Play();
      }
      if ((double) ((Component) this).animation["run"].normalizedTime % 1.0 > 0.75 && this.stepSoundPhase == 1)
      {
        this.stepSoundPhase = 2;
        Transform transform = ((Component) this).transform.Find("snd_eren_foot");
        ((Component) transform).GetComponent<AudioSource>().Stop();
        ((Component) transform).GetComponent<AudioSource>().Play();
      }
    }
    if (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE && !this.photonView.isMine)
      return;
    if (this.hasDied)
    {
      if ((double) ((Component) this).animation["die"].normalizedTime < 1.0 && !(this.hitAnimation == "hit_annie_3"))
        return;
      if (Object.op_Inequality((Object) this.realBody, (Object) null))
      {
        this.realBody.GetComponent<HERO>().backToHuman();
        this.realBody.transform.position = Vector3.op_Addition(((Component) this).transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck").position, Vector3.op_Multiply(Vector3.up, 2f));
        this.realBody = (GameObject) null;
      }
      this.dieTime += Time.deltaTime;
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
      if ((double) this.dieTime <= 5.0)
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
        PhotonNetwork.Destroy(this.photonView);
      }
    }
    else
    {
      if (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE && !this.photonView.isMine)
        return;
      if (this.isHit)
      {
        if ((double) ((Component) this).animation[this.hitAnimation].normalizedTime < 1.0)
          return;
        this.isHit = false;
        this.falseAttack();
        this.playAnimation("idle");
      }
      else
      {
        if ((double) this.lifeTime > 0.0)
        {
          this.lifeTime -= Time.deltaTime;
          if ((double) this.lifeTime <= 0.0)
          {
            this.hasDied = true;
            this.playAnimation("die");
            return;
          }
        }
        if (this.grounded && !this.isAttack && !((Component) this).animation.IsPlaying("jump_land") && !this.isAttack && !((Component) this).animation.IsPlaying("born"))
        {
          if (SettingsManager.InputSettings.Shifter.AttackDefault.GetKeyDown() || SettingsManager.InputSettings.Shifter.AttackSpecial.GetKeyDown())
          {
            bool flag = false;
            if (IN_GAME_MAIN_CAMERA.cameraMode == CAMERA_TYPE.WOW && SettingsManager.InputSettings.General.Back.GetKey() || SettingsManager.InputSettings.Shifter.AttackSpecial.GetKeyDown())
            {
              if (IN_GAME_MAIN_CAMERA.cameraMode == CAMERA_TYPE.WOW && SettingsManager.InputSettings.Shifter.AttackSpecial.GetKeyDown() && SettingsManager.InputSettings.Shifter.AttackSpecial.Contains((KeyCode) 324))
                flag = true;
              if (flag)
                flag = true;
              else
                this.attackAnimation = "attack_kick";
            }
            else
              this.attackAnimation = "attack_combo_001";
            if (!flag)
            {
              this.playAnimation(this.attackAnimation);
              ((Component) this).animation[this.attackAnimation].time = 0.0f;
              this.isAttack = true;
              this.needFreshCorePosition = true;
              if (this.attackAnimation == "attack_combo_001" || this.attackAnimation == "attack_combo_001")
                this.attackBox = ((Component) this).transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R/forearm_R/hand_R");
              else if (this.attackAnimation == "attack_combo_002")
                this.attackBox = ((Component) this).transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_L/upper_arm_L/forearm_L/hand_L");
              else if (this.attackAnimation == "attack_kick")
                this.attackBox = ((Component) this).transform.Find("Amarture/Core/Controller_Body/hip/thigh_R/shin_R/foot_R");
              this.hitTargets = new ArrayList();
            }
          }
          if (SettingsManager.InputSettings.Shifter.Roar.GetKeyDown())
          {
            this.crossFade("born", 0.1f);
            ((Component) this).animation["born"].normalizedTime = 0.28f;
            this.isPlayRoar = false;
          }
        }
        if (!this.isAttack)
        {
          if ((this.grounded || ((Component) this).animation.IsPlaying("idle")) && !((Component) this).animation.IsPlaying("jump_start") && !((Component) this).animation.IsPlaying("jump_air") && !((Component) this).animation.IsPlaying("jump_land") && SettingsManager.InputSettings.Shifter.Jump.GetKey())
            this.crossFade("jump_start", 0.1f);
        }
        else
        {
          if ((double) ((Component) this).animation[this.attackAnimation].time >= 0.10000000149011612 && SettingsManager.InputSettings.Shifter.AttackDefault.GetKeyDown())
            this.isNextAttack = true;
          float num1 = 0.0f;
          string str = string.Empty;
          float num2;
          float num3;
          if (this.attackAnimation == "attack_combo_001")
          {
            num2 = 0.4f;
            num3 = 0.5f;
            num1 = 0.66f;
            str = "attack_combo_002";
          }
          else if (this.attackAnimation == "attack_combo_002")
          {
            num2 = 0.15f;
            num3 = 0.25f;
            num1 = 0.43f;
            str = "attack_combo_003";
          }
          else if (this.attackAnimation == "attack_combo_003")
          {
            num1 = 0.0f;
            num2 = 0.31f;
            num3 = 0.37f;
          }
          else if (this.attackAnimation == "attack_kick")
          {
            num1 = 0.0f;
            num2 = 0.32f;
            num3 = 0.38f;
          }
          else
          {
            num2 = 0.5f;
            num3 = 0.85f;
          }
          if ((double) this.hitPause > 0.0)
          {
            this.hitPause -= Time.deltaTime;
            if ((double) this.hitPause <= 0.0)
            {
              ((Component) this).animation[this.attackAnimation].speed = 1f;
              this.hitPause = 0.0f;
            }
          }
          if ((double) num1 > 0.0 && this.isNextAttack && (double) ((Component) this).animation[this.attackAnimation].normalizedTime >= (double) num1)
          {
            if (this.hitTargets.Count > 0)
            {
              Transform hitTarget = (Transform) this.hitTargets[0];
              if (Object.op_Inequality((Object) hitTarget, (Object) null))
              {
                Transform transform = ((Component) this).transform;
                Quaternion quaternion1 = Quaternion.LookRotation(Vector3.op_Subtraction(hitTarget.position, ((Component) this).transform.position));
                Quaternion quaternion2 = Quaternion.Euler(0.0f, ((Quaternion) ref quaternion1).eulerAngles.y, 0.0f);
                transform.rotation = quaternion2;
                quaternion1 = ((Component) this).transform.rotation;
                this.facingDirection = ((Quaternion) ref quaternion1).eulerAngles.y;
              }
            }
            this.falseAttack();
            this.attackAnimation = str;
            this.crossFade(this.attackAnimation, 0.1f);
            ((Component) this).animation[this.attackAnimation].time = 0.0f;
            ((Component) this).animation[this.attackAnimation].speed = 1f;
            this.isAttack = true;
            this.needFreshCorePosition = true;
            if (this.attackAnimation == "attack_combo_002")
              this.attackBox = ((Component) this).transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_L/upper_arm_L/forearm_L/hand_L");
            else if (this.attackAnimation == "attack_combo_003")
              this.attackBox = ((Component) this).transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R/forearm_R/hand_R");
            this.hitTargets = new ArrayList();
          }
          if ((double) ((Component) this).animation[this.attackAnimation].normalizedTime >= (double) num2 && (double) ((Component) this).animation[this.attackAnimation].normalizedTime <= (double) num3 || !this.attackChkOnce && (double) ((Component) this).animation[this.attackAnimation].normalizedTime >= (double) num2)
          {
            if (!this.attackChkOnce)
            {
              if (this.attackAnimation == "attack_combo_002")
                this.playSound("snd_eren_swing2");
              else if (this.attackAnimation == "attack_combo_001")
                this.playSound("snd_eren_swing1");
              else if (this.attackAnimation == "attack_combo_003")
                this.playSound("snd_eren_swing3");
              this.attackChkOnce = true;
            }
            Collider[] colliderArray = Physics.OverlapSphere(((Component) this.attackBox).transform.position, 8f);
            for (int index1 = 0; index1 < colliderArray.Length; ++index1)
            {
              if (!Object.op_Equality((Object) ((Component) ((Component) colliderArray[index1]).gameObject.transform.root).GetComponent<TITAN>(), (Object) null))
              {
                bool flag = false;
                for (int index2 = 0; index2 < this.hitTargets.Count; ++index2)
                {
                  if (((Component) colliderArray[index1]).gameObject.transform.root == this.hitTargets[index2])
                  {
                    flag = true;
                    break;
                  }
                }
                if (!flag && !((Component) ((Component) colliderArray[index1]).gameObject.transform.root).GetComponent<TITAN>().hasDie)
                {
                  ((Component) this).animation[this.attackAnimation].speed = 0.0f;
                  if (this.attackAnimation == "attack_combo_002")
                  {
                    this.hitPause = 0.05f;
                    ((Component) ((Component) colliderArray[index1]).gameObject.transform.root).GetComponent<TITAN>().hitL(((Component) this).transform.position, this.hitPause);
                    ((Component) this.currentCamera).GetComponent<IN_GAME_MAIN_CAMERA>().startShake(1f, 0.03f);
                  }
                  else if (this.attackAnimation == "attack_combo_001")
                  {
                    ((Component) this.currentCamera).GetComponent<IN_GAME_MAIN_CAMERA>().startShake(1.2f, 0.04f);
                    this.hitPause = 0.08f;
                    ((Component) ((Component) colliderArray[index1]).gameObject.transform.root).GetComponent<TITAN>().hitR(((Component) this).transform.position, this.hitPause);
                  }
                  else if (this.attackAnimation == "attack_combo_003")
                  {
                    ((Component) this.currentCamera).GetComponent<IN_GAME_MAIN_CAMERA>().startShake(3f, 0.1f);
                    this.hitPause = 0.3f;
                    ((Component) ((Component) colliderArray[index1]).gameObject.transform.root).GetComponent<TITAN>().dieHeadBlow(((Component) this).transform.position, this.hitPause);
                  }
                  else if (this.attackAnimation == "attack_kick")
                  {
                    ((Component) this.currentCamera).GetComponent<IN_GAME_MAIN_CAMERA>().startShake(3f, 0.1f);
                    this.hitPause = 0.2f;
                    if (((Component) ((Component) colliderArray[index1]).gameObject.transform.root).GetComponent<TITAN>().abnormalType == AbnormalType.TYPE_CRAWLER)
                      ((Component) ((Component) colliderArray[index1]).gameObject.transform.root).GetComponent<TITAN>().dieBlow(((Component) this).transform.position, this.hitPause);
                    else if ((double) ((Component) ((Component) colliderArray[index1]).gameObject.transform.root).transform.localScale.x < 2.0)
                      ((Component) ((Component) colliderArray[index1]).gameObject.transform.root).GetComponent<TITAN>().dieBlow(((Component) this).transform.position, this.hitPause);
                    else
                      ((Component) ((Component) colliderArray[index1]).gameObject.transform.root).GetComponent<TITAN>().hitR(((Component) this).transform.position, this.hitPause);
                  }
                  this.hitTargets.Add((object) ((Component) colliderArray[index1]).gameObject.transform.root);
                  if (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE)
                    PhotonNetwork.Instantiate("hitMeatBIG", Vector3.op_Multiply(Vector3.op_Addition(((Component) colliderArray[index1]).transform.position, this.attackBox.position), 0.5f), Quaternion.Euler(270f, 0.0f, 0.0f), 0);
                  else
                    Object.Instantiate(Resources.Load("hitMeatBIG"), Vector3.op_Multiply(Vector3.op_Addition(((Component) colliderArray[index1]).transform.position, this.attackBox.position), 0.5f), Quaternion.Euler(270f, 0.0f, 0.0f));
                }
              }
            }
          }
          if ((double) ((Component) this).animation[this.attackAnimation].normalizedTime >= 1.0)
          {
            this.falseAttack();
            this.playAnimation("idle");
          }
        }
        if (((Component) this).animation.IsPlaying("jump_land") && (double) ((Component) this).animation["jump_land"].normalizedTime >= 1.0)
          this.crossFade("idle", 0.1f);
        if (((Component) this).animation.IsPlaying("born"))
        {
          if ((double) ((Component) this).animation["born"].normalizedTime >= 0.2800000011920929 && !this.isPlayRoar)
          {
            this.isPlayRoar = true;
            this.playSound("snd_eren_roar");
          }
          if ((double) ((Component) this).animation["born"].normalizedTime >= 0.5 && (double) ((Component) this).animation["born"].normalizedTime <= 0.699999988079071)
            ((Component) this.currentCamera).GetComponent<IN_GAME_MAIN_CAMERA>().startShake(0.5f, 1f);
          if ((double) ((Component) this).animation["born"].normalizedTime >= 1.0)
          {
            this.crossFade("idle", 0.1f);
            if (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE)
            {
              if (PhotonNetwork.isMasterClient)
                this.photonView.RPC("netTauntAttack", PhotonTargets.MasterClient, (object) 10f, (object) 500f);
              else
                this.netTauntAttack(10f, 500f);
            }
            else
              this.netTauntAttack(10f, 500f);
          }
        }
        this.showAimUI();
        this.showSkillCD();
      }
    }
  }
}
