// Decompiled with JetBrains decompiler
// Type: COLOSSAL_TITAN
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using CustomSkins;
using Photon;
using Settings;
using System;
using System.Collections;
using UnityEngine;

public class COLOSSAL_TITAN : MonoBehaviour
{
  private string actionName;
  private string attackAnimation;
  private float attackCheckTime;
  private float attackCheckTimeA;
  private float attackCheckTimeB;
  private bool attackChkOnce;
  private int attackCount;
  private int attackPattern = -1;
  public GameObject bottomObject;
  private Transform checkHitCapsuleEnd;
  private Vector3 checkHitCapsuleEndOld;
  private float checkHitCapsuleR;
  private Transform checkHitCapsuleStart;
  public GameObject door_broken;
  public GameObject door_closed;
  public bool hasDie;
  public bool hasspawn;
  public GameObject healthLabel;
  public float healthTime;
  private bool isSteamNeed;
  public float lagMax;
  public int maxHealth;
  public static float minusDistance = 99999f;
  public static GameObject minusDistanceEnemy;
  public float myDistance;
  public GameObject myHero;
  public int NapeArmor = 10000;
  public int NapeArmorTotal = 10000;
  public GameObject neckSteamObject;
  public float size;
  private string state = "idle";
  public GameObject sweepSmokeObject;
  private float tauntTime;
  private float waitTime = 2f;
  private ColossalCustomSkinLoader _customSkinLoader;

  private void attack_sweep(string type = "")
  {
    this.callTitanHAHA();
    this.state = nameof (attack_sweep);
    this.attackAnimation = "sweep" + type;
    this.attackCheckTimeA = 0.4f;
    this.attackCheckTimeB = 0.57f;
    this.checkHitCapsuleStart = ((Component) this).transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R/forearm_R");
    this.checkHitCapsuleEnd = ((Component) this).transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R/forearm_R/hand_R/hand_R_001");
    this.checkHitCapsuleR = 20f;
    this.crossFade("attack_" + this.attackAnimation, 0.1f);
    this.attackChkOnce = false;
    this.sweepSmokeObject.GetComponent<ParticleSystem>().enableEmission = true;
    this.sweepSmokeObject.GetComponent<ParticleSystem>().Play();
    if (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.MULTIPLAYER)
      return;
    if (FengGameManagerMKII.LAN)
    {
      NetworkPeerType peerType = Network.peerType;
    }
    else
    {
      if (!PhotonNetwork.isMasterClient)
        return;
      this.photonView.RPC("startSweepSmoke", PhotonTargets.Others);
    }
  }

  private void Awake()
  {
    ((Component) this).rigidbody.freezeRotation = true;
    ((Component) this).rigidbody.useGravity = false;
    ((Component) this).rigidbody.isKinematic = true;
    this._customSkinLoader = ((Component) this).gameObject.AddComponent<ColossalCustomSkinLoader>();
  }

  public void beTauntedBy(GameObject target, float tauntTime)
  {
  }

  public void blowPlayer(GameObject player, Transform neck)
  {
    Vector3 vector3 = Vector3.op_UnaryNegation(Vector3.op_Subtraction(Vector3.op_Addition(neck.position, Vector3.op_Multiply(((Component) this).transform.forward, 50f)), player.transform.position));
    float num = 20f;
    switch (IN_GAME_MAIN_CAMERA.gametype)
    {
      case GAMETYPE.SINGLE:
        player.GetComponent<HERO>().blowAway(Vector3.op_Addition(Vector3.op_Multiply(((Vector3) ref vector3).normalized, num), Vector3.op_Multiply(Vector3.up, 1f)), (PhotonMessageInfo) null);
        break;
      case GAMETYPE.MULTIPLAYER:
        if (!PhotonNetwork.isMasterClient)
          break;
        object[] objArray = new object[1]
        {
          (object) Vector3.op_Addition(Vector3.op_Multiply(((Vector3) ref vector3).normalized, num), Vector3.op_Multiply(Vector3.up, 1f))
        };
        player.GetComponent<HERO>().photonView.RPC("blowAway", PhotonTargets.All, objArray);
        break;
    }
  }

  private void callTitan(bool special = false)
  {
    if (!special && GameObject.FindGameObjectsWithTag("titan").Length > 6)
      return;
    GameObject[] gameObjectsWithTag1 = GameObject.FindGameObjectsWithTag("titanRespawn");
    ArrayList arrayList = new ArrayList();
    foreach (GameObject gameObject in gameObjectsWithTag1)
    {
      if (((Object) gameObject.transform.parent).name == "titanRespawnCT")
        arrayList.Add((object) gameObject);
    }
    GameObject gameObject1 = (GameObject) arrayList[Random.Range(0, arrayList.Count)];
    string[] strArray = new string[1]{ "TITAN_VER3.1" };
    GameObject gameObject2 = !FengGameManagerMKII.LAN ? PhotonNetwork.Instantiate(strArray[Random.Range(0, strArray.Length)], gameObject1.transform.position, gameObject1.transform.rotation, 0) : (GameObject) Network.Instantiate(Resources.Load(strArray[Random.Range(0, strArray.Length)]), gameObject1.transform.position, gameObject1.transform.rotation, 0);
    if (special)
    {
      GameObject[] gameObjectsWithTag2 = GameObject.FindGameObjectsWithTag("route");
      GameObject route = gameObjectsWithTag2[Random.Range(0, gameObjectsWithTag2.Length)];
      while (((Object) route).name != "routeCT")
        route = gameObjectsWithTag2[Random.Range(0, gameObjectsWithTag2.Length)];
      gameObject2.GetComponent<TITAN>().setRoute(route);
      gameObject2.GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_I, false);
      gameObject2.GetComponent<TITAN>().activeRad = 0;
      gameObject2.GetComponent<TITAN>().toCheckPoint((Vector3) gameObject2.GetComponent<TITAN>().checkPoints[0], 10f);
    }
    else
    {
      float num1 = 0.7f;
      float num2 = 0.7f;
      switch (IN_GAME_MAIN_CAMERA.difficulty)
      {
        case 1:
          num1 = 0.4f;
          num2 = 0.7f;
          break;
        case 2:
          num1 = -1f;
          num2 = 0.7f;
          break;
      }
      if (GameObject.FindGameObjectsWithTag("titan").Length == 5)
        gameObject2.GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_JUMPER, false);
      else if ((double) Random.Range(0.0f, 1f) >= (double) num1)
      {
        if ((double) Random.Range(0.0f, 1f) < (double) num2)
          gameObject2.GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_JUMPER, false);
        else
          gameObject2.GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_CRAWLER, false);
      }
      gameObject2.GetComponent<TITAN>().activeRad = 200;
    }
    if (FengGameManagerMKII.LAN)
      ((GameObject) Network.Instantiate(Resources.Load("FX/FXtitanSpawn"), gameObject2.transform.position, Quaternion.Euler(-90f, 0.0f, 0.0f), 0)).transform.localScale = gameObject2.transform.localScale;
    else
      PhotonNetwork.Instantiate("FX/FXtitanSpawn", gameObject2.transform.position, Quaternion.Euler(-90f, 0.0f, 0.0f), 0).transform.localScale = gameObject2.transform.localScale;
  }

  private void callTitanHAHA()
  {
    ++this.attackCount;
    int num1 = 4;
    int num2 = 7;
    switch (IN_GAME_MAIN_CAMERA.difficulty)
    {
      case 1:
        num1 = 4;
        num2 = 6;
        break;
      case 2:
        num1 = 3;
        num2 = 5;
        break;
    }
    if (this.attackCount % num1 == 0)
      this.callTitan();
    if ((double) this.NapeArmor < (double) this.NapeArmorTotal * 0.3)
    {
      if (this.attackCount % (int) ((double) num2 * 0.5) != 0)
        return;
      this.callTitan(true);
    }
    else
    {
      if (this.attackCount % num2 != 0)
        return;
      this.callTitan(true);
    }
  }

  [RPC]
  private void changeDoor()
  {
    this.door_broken.SetActive(true);
    this.door_closed.SetActive(false);
  }

  private RaycastHit[] checkHitCapsule(Vector3 start, Vector3 end, float r) => Physics.SphereCastAll(start, r, Vector3.op_Subtraction(end, start), Vector3.Distance(start, end));

  private GameObject checkIfHitHand(Transform hand)
  {
    float num = 30f;
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

  private void crossFade(string aniName, float time)
  {
    ((Component) this).animation.CrossFade(aniName, time);
    if (FengGameManagerMKII.LAN || IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.MULTIPLAYER || !PhotonNetwork.isMasterClient)
      return;
    this.photonView.RPC("netCrossFade", PhotonTargets.Others, (object) aniName, (object) time);
  }

  private void findNearestHero() => this.myHero = this.getNearestHero();

  private GameObject getNearestHero()
  {
    GameObject[] gameObjectsWithTag = GameObject.FindGameObjectsWithTag("Player");
    GameObject nearestHero = (GameObject) null;
    float num1 = float.PositiveInfinity;
    foreach (GameObject gameObject in gameObjectsWithTag)
    {
      if ((Object.op_Equality((Object) gameObject.GetComponent<HERO>(), (Object) null) || !gameObject.GetComponent<HERO>().HasDied()) && (Object.op_Equality((Object) gameObject.GetComponent<TITAN_EREN>(), (Object) null) || !gameObject.GetComponent<TITAN_EREN>().hasDied))
      {
        float num2 = Mathf.Sqrt((float) (((double) gameObject.transform.position.x - (double) ((Component) this).transform.position.x) * ((double) gameObject.transform.position.x - (double) ((Component) this).transform.position.x) + ((double) gameObject.transform.position.z - (double) ((Component) this).transform.position.z) * ((double) gameObject.transform.position.z - (double) ((Component) this).transform.position.z)));
        if ((double) gameObject.transform.position.y - (double) ((Component) this).transform.position.y < 450.0 && (double) num2 < (double) num1)
        {
          nearestHero = gameObject;
          num1 = num2;
        }
      }
    }
    return nearestHero;
  }

  private void idle()
  {
    this.state = nameof (idle);
    this.crossFade(nameof (idle), 0.2f);
  }

  private void kick()
  {
    this.state = nameof (kick);
    this.actionName = "attack_kick_wall";
    this.attackCheckTime = 0.64f;
    this.attackChkOnce = false;
    this.crossFade(this.actionName, 0.1f);
  }

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
        if (FengGameManagerMKII.LAN)
        {
          if (hitHero.GetComponent<HERO>().HasDied())
            break;
          hitHero.GetComponent<HERO>().markDie();
          break;
        }
        if (hitHero.GetComponent<HERO>().HasDied())
          break;
        hitHero.GetComponent<HERO>().markDie();
        object[] objArray = new object[5]
        {
          (object) Vector3.op_Multiply(Vector3.op_Multiply(Vector3.op_Subtraction(hitHero.transform.position, position), 15f), 4f),
          (object) false,
          (object) -1,
          (object) "Colossal Titan",
          (object) true
        };
        hitHero.GetComponent<HERO>().photonView.RPC("netDie", PhotonTargets.All, objArray);
        break;
    }
  }

  [RPC]
  public void labelRPC(int health, int maxHealth, PhotonMessageInfo info)
  {
    if (info.sender != this.photonView.owner)
      FengGameManagerMKII.instance.kickPlayerRCIfMC(info.sender, true, "colossal labelRPC");
    else if (health < 0)
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
        this.healthLabel.transform.localPosition = new Vector3(0.0f, 430f, 0.0f);
        float num = 15f;
        if ((double) this.size > 0.0 && (double) this.size < 1.0)
          num = Mathf.Min(15f / this.size, 100f);
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

  public void loadskin()
  {
    if (!PhotonNetwork.isMasterClient || !SettingsManager.CustomSkinSettings.Shifter.SkinsEnabled.Value)
      return;
    string url = ((ShifterCustomSkinSet) SettingsManager.CustomSkinSettings.Shifter.GetSelectedSet()).Colossal.Value;
    if (!TextureDownloader.ValidTextureURL(url))
      return;
    this.photonView.RPC("loadskinRPC", PhotonTargets.AllBuffered, (object) url);
  }

  public IEnumerator loadskinE(string url)
  {
    COLOSSAL_TITAN colossalTitan = this;
    while (!colossalTitan.hasspawn)
      yield return (object) null;
    yield return (object) colossalTitan.StartCoroutine(colossalTitan._customSkinLoader.LoadSkinsFromRPC(new object[1]
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

  private void neckSteam()
  {
    this.neckSteamObject.GetComponent<ParticleSystem>().Stop();
    this.neckSteamObject.GetComponent<ParticleSystem>().Play();
    if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)
    {
      if (FengGameManagerMKII.LAN)
      {
        if (Network.peerType == 1)
          ;
      }
      else if (PhotonNetwork.isMasterClient)
        this.photonView.RPC("startNeckSteam", PhotonTargets.Others);
    }
    this.isSteamNeed = true;
    Transform neck = ((Component) this).transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck");
    float num = 30f;
    foreach (Collider collider in Physics.OverlapSphere(Vector3.op_Subtraction(((Component) neck).transform.position, Vector3.op_Multiply(((Component) this).transform.forward, 10f)), num))
    {
      if (((Component) ((Component) collider).transform.root).tag == "Player")
      {
        GameObject gameObject = ((Component) ((Component) collider).transform.root).gameObject;
        if (Object.op_Equality((Object) gameObject.GetComponent<TITAN_EREN>(), (Object) null) && Object.op_Inequality((Object) gameObject.GetComponent<HERO>(), (Object) null))
          this.blowPlayer(gameObject, neck);
      }
    }
  }

  [RPC]
  private void netCrossFade(string aniName, float time) => ((Component) this).animation.CrossFade(aniName, time);

  [RPC]
  public void netDie()
  {
    if (this.hasDie)
      return;
    this.hasDie = true;
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
    GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().removeCT(this);
  }

  private void playAnimation(string aniName)
  {
    ((Component) this).animation.Play(aniName);
    if (FengGameManagerMKII.LAN || IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.MULTIPLAYER || !PhotonNetwork.isMasterClient)
      return;
    this.photonView.RPC("netPlayAnimation", PhotonTargets.Others, (object) aniName);
  }

  private void playAnimationAt(string aniName, float normalizedTime)
  {
    ((Component) this).animation.Play(aniName);
    ((Component) this).animation[aniName].normalizedTime = normalizedTime;
    if (FengGameManagerMKII.LAN || IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.MULTIPLAYER || !PhotonNetwork.isMasterClient)
      return;
    this.photonView.RPC("netPlayAnimationAt", PhotonTargets.Others, (object) aniName, (object) normalizedTime);
  }

  private void playSound(string sndname)
  {
    this.playsoundRPC(sndname);
    if (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.MULTIPLAYER)
      return;
    if (FengGameManagerMKII.LAN)
    {
      NetworkPeerType peerType = Network.peerType;
    }
    else
    {
      if (!PhotonNetwork.isMasterClient)
        return;
      this.photonView.RPC("playsoundRPC", PhotonTargets.Others, (object) sndname);
    }
  }

  [RPC]
  private void playsoundRPC(string sndname) => ((Component) ((Component) this).transform.Find(sndname)).GetComponent<AudioSource>().Play();

  [RPC]
  private void removeMe(PhotonMessageInfo info)
  {
    if (info != null && info.sender != this.photonView.owner)
      FengGameManagerMKII.instance.kickPlayerRCIfMC(info.sender, true, "colossal remove");
    else
      Object.Destroy((Object) ((Component) this).gameObject);
  }

  [RPC]
  public void setSize(float size, PhotonMessageInfo info)
  {
    size = Mathf.Clamp(size, 0.1f, 50f);
    if (info == null || !info.sender.isMasterClient)
      return;
    Transform transform = ((Component) this).transform;
    transform.localScale = Vector3.op_Multiply(transform.localScale, size * 0.05f);
    this.size = size;
  }

  private void slap(string type)
  {
    this.callTitanHAHA();
    this.state = nameof (slap);
    this.attackAnimation = type;
    if (type == "r1" || type == "r2")
      this.checkHitCapsuleStart = ((Component) this).transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R/forearm_R/hand_R/hand_R_001");
    if (type == "l1" || type == "l2")
      this.checkHitCapsuleStart = ((Component) this).transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_L/upper_arm_L/forearm_L/hand_L/hand_L_001");
    this.attackCheckTime = 0.57f;
    this.attackChkOnce = false;
    this.crossFade("attack_slap_" + this.attackAnimation, 0.1f);
  }

  private void Start()
  {
    this.startMain();
    this.size = 20f;
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
    GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().addCT(this);
    if (Object.op_Equality((Object) this.myHero, (Object) null))
      this.findNearestHero();
    ((Object) this).name = nameof (COLOSSAL_TITAN);
    this.NapeArmor = 1000;
    bool flag = false;
    if (LevelInfo.getInfo(FengGameManagerMKII.level).respawnMode == RespawnMode.NEVER)
      flag = true;
    switch (IN_GAME_MAIN_CAMERA.difficulty)
    {
      case 0:
        this.NapeArmor = !flag ? 5000 : 2000;
        break;
      case 1:
        this.NapeArmor = !flag ? 8000 : 3500;
        IEnumerator enumerator1 = ((Component) this).animation.GetEnumerator();
        try
        {
          while (enumerator1.MoveNext())
            ((AnimationState) enumerator1.Current).speed = 1.02f;
          break;
        }
        finally
        {
          if (enumerator1 is IDisposable disposable)
            disposable.Dispose();
        }
      case 2:
        this.NapeArmor = !flag ? 12000 : 5000;
        IEnumerator enumerator2 = ((Component) this).animation.GetEnumerator();
        try
        {
          while (enumerator2.MoveNext())
            ((AnimationState) enumerator2.Current).speed = 1.05f;
          break;
        }
        finally
        {
          if (enumerator2 is IDisposable disposable)
            disposable.Dispose();
        }
    }
    this.NapeArmorTotal = this.NapeArmor;
    this.state = "wait";
    Transform transform = ((Component) this).transform;
    transform.position = Vector3.op_Addition(transform.position, Vector3.op_Multiply(Vector3.op_UnaryNegation(Vector3.up), 10000f));
    if (FengGameManagerMKII.LAN)
      ((Behaviour) ((Component) this).GetComponent<PhotonView>()).enabled = false;
    else
      ((Behaviour) ((Component) this).GetComponent<NetworkView>()).enabled = false;
    this.door_broken = GameObject.Find("door_broke");
    this.door_closed = GameObject.Find("door_fine");
    this.door_broken.SetActive(false);
    this.door_closed.SetActive(true);
  }

  [RPC]
  private void startNeckSteam()
  {
    this.neckSteamObject.GetComponent<ParticleSystem>().Stop();
    this.neckSteamObject.GetComponent<ParticleSystem>().Play();
  }

  [RPC]
  private void startSweepSmoke()
  {
    this.sweepSmokeObject.GetComponent<ParticleSystem>().enableEmission = true;
    this.sweepSmokeObject.GetComponent<ParticleSystem>().Play();
  }

  private void steam()
  {
    this.callTitanHAHA();
    this.state = nameof (steam);
    this.actionName = "attack_steam";
    this.attackCheckTime = 0.45f;
    this.crossFade(this.actionName, 0.1f);
    this.attackChkOnce = false;
  }

  [RPC]
  private void stopSweepSmoke()
  {
    this.sweepSmokeObject.GetComponent<ParticleSystem>().enableEmission = false;
    this.sweepSmokeObject.GetComponent<ParticleSystem>().Stop();
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
    this.neckSteam();
    if (this.NapeArmor <= 0)
    {
      this.NapeArmor = 0;
      if (!this.hasDie)
      {
        if (FengGameManagerMKII.LAN)
        {
          this.netDie();
        }
        else
        {
          this.photonView.RPC("netDie", PhotonTargets.OthersBuffered);
          this.netDie();
          GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().titanGetKill(photonView.owner, speed, ((Object) this).name);
        }
      }
    }
    else
    {
      GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().sendKillInfo(false, (string) photonView.owner.customProperties[(object) PhotonPlayerProperty.name], true, "Colossal Titan's neck", speed);
      object[] objArray = new object[1]{ (object) speed };
      GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().photonView.RPC("netShowDamage", photonView.owner, objArray);
    }
    this.healthTime = 0.2f;
  }

  public void update()
  {
    if (!(this.state != "null"))
      return;
    if (this.state == "wait")
    {
      this.waitTime -= Time.deltaTime;
      if ((double) this.waitTime > 0.0)
        return;
      ((Component) this).transform.position = new Vector3(30f, 0.0f, 784f);
      Object.Instantiate(Resources.Load("FX/ThunderCT"), Vector3.op_Addition(((Component) this).transform.position, Vector3.op_Multiply(Vector3.up, 350f)), Quaternion.Euler(270f, 0.0f, 0.0f));
      GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().flashBlind();
      if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
        this.idle();
      else if ((!FengGameManagerMKII.LAN ? (this.photonView.isMine ? 1 : 0) : (this.networkView.isMine ? 1 : 0)) != 0)
        this.idle();
      else
        this.state = "null";
    }
    else if (!(this.state == "idle"))
    {
      if (this.state == "attack_sweep")
      {
        if ((double) this.attackCheckTimeA != 0.0 && ((double) ((Component) this).animation["attack_" + this.attackAnimation].normalizedTime >= (double) this.attackCheckTimeA && (double) ((Component) this).animation["attack_" + this.attackAnimation].normalizedTime <= (double) this.attackCheckTimeB || !this.attackChkOnce && (double) ((Component) this).animation["attack_" + this.attackAnimation].normalizedTime >= (double) this.attackCheckTimeA))
        {
          if (!this.attackChkOnce)
            this.attackChkOnce = true;
          foreach (RaycastHit raycastHit in this.checkHitCapsule(this.checkHitCapsuleStart.position, this.checkHitCapsuleEnd.position, this.checkHitCapsuleR))
          {
            GameObject gameObject = ((Component) ((RaycastHit) ref raycastHit).collider).gameObject;
            if (gameObject.tag == "Player")
              this.killPlayer(gameObject);
            if (gameObject.tag == "erenHitbox" && this.attackAnimation == "combo_3" && IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER && (!FengGameManagerMKII.LAN ? (PhotonNetwork.isMasterClient ? 1 : 0) : (Network.isServer ? 1 : 0)) != 0)
              ((Component) gameObject.transform.root).gameObject.GetComponent<TITAN_EREN>().hitByFTByServer(3);
          }
          foreach (RaycastHit raycastHit in this.checkHitCapsule(this.checkHitCapsuleEndOld, this.checkHitCapsuleEnd.position, this.checkHitCapsuleR))
          {
            GameObject gameObject = ((Component) ((RaycastHit) ref raycastHit).collider).gameObject;
            if (gameObject.tag == "Player")
              this.killPlayer(gameObject);
          }
          this.checkHitCapsuleEndOld = this.checkHitCapsuleEnd.position;
        }
        if ((double) ((Component) this).animation["attack_" + this.attackAnimation].normalizedTime < 1.0)
          return;
        this.sweepSmokeObject.GetComponent<ParticleSystem>().enableEmission = false;
        this.sweepSmokeObject.GetComponent<ParticleSystem>().Stop();
        if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER && !FengGameManagerMKII.LAN)
          this.photonView.RPC("stopSweepSmoke", PhotonTargets.Others);
        this.findNearestHero();
        this.idle();
        this.playAnimation("idle");
      }
      else if (this.state == "kick")
      {
        if (!this.attackChkOnce && (double) ((Component) this).animation[this.actionName].normalizedTime >= (double) this.attackCheckTime)
        {
          this.attackChkOnce = true;
          this.door_broken.SetActive(true);
          this.door_closed.SetActive(false);
          if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER && !FengGameManagerMKII.LAN)
            this.photonView.RPC("changeDoor", PhotonTargets.OthersBuffered);
          if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)
          {
            if (FengGameManagerMKII.LAN)
            {
              Network.Instantiate(Resources.Load("FX/boom1_CT_KICK"), Vector3.op_Addition(Vector3.op_Addition(((Component) this).transform.position, Vector3.op_Multiply(((Component) this).transform.forward, 120f)), Vector3.op_Multiply(((Component) this).transform.right, 30f)), Quaternion.Euler(270f, 0.0f, 0.0f), 0);
              Network.Instantiate(Resources.Load("rock"), Vector3.op_Addition(Vector3.op_Addition(((Component) this).transform.position, Vector3.op_Multiply(((Component) this).transform.forward, 120f)), Vector3.op_Multiply(((Component) this).transform.right, 30f)), Quaternion.Euler(0.0f, 0.0f, 0.0f), 0);
            }
            else
            {
              PhotonNetwork.Instantiate("FX/boom1_CT_KICK", Vector3.op_Addition(Vector3.op_Addition(((Component) this).transform.position, Vector3.op_Multiply(((Component) this).transform.forward, 120f)), Vector3.op_Multiply(((Component) this).transform.right, 30f)), Quaternion.Euler(270f, 0.0f, 0.0f), 0);
              PhotonNetwork.Instantiate("rock", Vector3.op_Addition(Vector3.op_Addition(((Component) this).transform.position, Vector3.op_Multiply(((Component) this).transform.forward, 120f)), Vector3.op_Multiply(((Component) this).transform.right, 30f)), Quaternion.Euler(0.0f, 0.0f, 0.0f), 0);
            }
          }
          else
          {
            Object.Instantiate(Resources.Load("FX/boom1_CT_KICK"), Vector3.op_Addition(Vector3.op_Addition(((Component) this).transform.position, Vector3.op_Multiply(((Component) this).transform.forward, 120f)), Vector3.op_Multiply(((Component) this).transform.right, 30f)), Quaternion.Euler(270f, 0.0f, 0.0f));
            Object.Instantiate(Resources.Load("rock"), Vector3.op_Addition(Vector3.op_Addition(((Component) this).transform.position, Vector3.op_Multiply(((Component) this).transform.forward, 120f)), Vector3.op_Multiply(((Component) this).transform.right, 30f)), Quaternion.Euler(0.0f, 0.0f, 0.0f));
          }
        }
        if ((double) ((Component) this).animation[this.actionName].normalizedTime < 1.0)
          return;
        this.findNearestHero();
        this.idle();
        this.playAnimation("idle");
      }
      else if (this.state == "slap")
      {
        if (!this.attackChkOnce && (double) ((Component) this).animation["attack_slap_" + this.attackAnimation].normalizedTime >= (double) this.attackCheckTime)
        {
          this.attackChkOnce = true;
          GameObject gameObject;
          if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)
          {
            gameObject = !FengGameManagerMKII.LAN ? PhotonNetwork.Instantiate("FX/boom1", this.checkHitCapsuleStart.position, Quaternion.Euler(270f, 0.0f, 0.0f), 0) : (GameObject) Network.Instantiate(Resources.Load("FX/boom1"), this.checkHitCapsuleStart.position, Quaternion.Euler(270f, 0.0f, 0.0f), 0);
            if (Object.op_Inequality((Object) gameObject.GetComponent<EnemyfxIDcontainer>(), (Object) null))
              gameObject.GetComponent<EnemyfxIDcontainer>().titanName = ((Object) this).name;
          }
          else
            gameObject = (GameObject) Object.Instantiate(Resources.Load("FX/boom1"), this.checkHitCapsuleStart.position, Quaternion.Euler(270f, 0.0f, 0.0f));
          gameObject.transform.localScale = new Vector3(5f, 5f, 5f);
        }
        if ((double) ((Component) this).animation["attack_slap_" + this.attackAnimation].normalizedTime < 1.0)
          return;
        this.findNearestHero();
        this.idle();
        this.playAnimation("idle");
      }
      else if (this.state == "steam")
      {
        if (!this.attackChkOnce && (double) ((Component) this).animation[this.actionName].normalizedTime >= (double) this.attackCheckTime)
        {
          this.attackChkOnce = true;
          if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)
          {
            if (FengGameManagerMKII.LAN)
            {
              Network.Instantiate(Resources.Load("FX/colossal_steam"), Vector3.op_Addition(((Component) this).transform.position, Vector3.op_Multiply(((Component) this).transform.up, 185f)), Quaternion.Euler(270f, 0.0f, 0.0f), 0);
              Network.Instantiate(Resources.Load("FX/colossal_steam"), Vector3.op_Addition(((Component) this).transform.position, Vector3.op_Multiply(((Component) this).transform.up, 303f)), Quaternion.Euler(270f, 0.0f, 0.0f), 0);
              Network.Instantiate(Resources.Load("FX/colossal_steam"), Vector3.op_Addition(((Component) this).transform.position, Vector3.op_Multiply(((Component) this).transform.up, 50f)), Quaternion.Euler(270f, 0.0f, 0.0f), 0);
            }
            else
            {
              PhotonNetwork.Instantiate("FX/colossal_steam", Vector3.op_Addition(((Component) this).transform.position, Vector3.op_Multiply(((Component) this).transform.up, 185f)), Quaternion.Euler(270f, 0.0f, 0.0f), 0);
              PhotonNetwork.Instantiate("FX/colossal_steam", Vector3.op_Addition(((Component) this).transform.position, Vector3.op_Multiply(((Component) this).transform.up, 303f)), Quaternion.Euler(270f, 0.0f, 0.0f), 0);
              PhotonNetwork.Instantiate("FX/colossal_steam", Vector3.op_Addition(((Component) this).transform.position, Vector3.op_Multiply(((Component) this).transform.up, 50f)), Quaternion.Euler(270f, 0.0f, 0.0f), 0);
            }
          }
          else
          {
            Object.Instantiate(Resources.Load("FX/colossal_steam"), Vector3.op_Addition(((Component) this).transform.position, Vector3.op_Multiply(((Component) this).transform.forward, 185f)), Quaternion.Euler(270f, 0.0f, 0.0f));
            Object.Instantiate(Resources.Load("FX/colossal_steam"), Vector3.op_Addition(((Component) this).transform.position, Vector3.op_Multiply(((Component) this).transform.forward, 303f)), Quaternion.Euler(270f, 0.0f, 0.0f));
            Object.Instantiate(Resources.Load("FX/colossal_steam"), Vector3.op_Addition(((Component) this).transform.position, Vector3.op_Multiply(((Component) this).transform.forward, 50f)), Quaternion.Euler(270f, 0.0f, 0.0f));
          }
        }
        if ((double) ((Component) this).animation[this.actionName].normalizedTime < 1.0)
          return;
        if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)
        {
          if (FengGameManagerMKII.LAN)
          {
            Network.Instantiate(Resources.Load("FX/colossal_steam_dmg"), Vector3.op_Addition(((Component) this).transform.position, Vector3.op_Multiply(((Component) this).transform.up, 185f)), Quaternion.Euler(270f, 0.0f, 0.0f), 0);
            Network.Instantiate(Resources.Load("FX/colossal_steam_dmg"), Vector3.op_Addition(((Component) this).transform.position, Vector3.op_Multiply(((Component) this).transform.up, 303f)), Quaternion.Euler(270f, 0.0f, 0.0f), 0);
            Network.Instantiate(Resources.Load("FX/colossal_steam_dmg"), Vector3.op_Addition(((Component) this).transform.position, Vector3.op_Multiply(((Component) this).transform.up, 50f)), Quaternion.Euler(270f, 0.0f, 0.0f), 0);
          }
          else
          {
            GameObject gameObject1 = PhotonNetwork.Instantiate("FX/colossal_steam_dmg", Vector3.op_Addition(((Component) this).transform.position, Vector3.op_Multiply(((Component) this).transform.up, 185f)), Quaternion.Euler(270f, 0.0f, 0.0f), 0);
            if (Object.op_Inequality((Object) gameObject1.GetComponent<EnemyfxIDcontainer>(), (Object) null))
              gameObject1.GetComponent<EnemyfxIDcontainer>().titanName = ((Object) this).name;
            GameObject gameObject2 = PhotonNetwork.Instantiate("FX/colossal_steam_dmg", Vector3.op_Addition(((Component) this).transform.position, Vector3.op_Multiply(((Component) this).transform.up, 303f)), Quaternion.Euler(270f, 0.0f, 0.0f), 0);
            if (Object.op_Inequality((Object) gameObject2.GetComponent<EnemyfxIDcontainer>(), (Object) null))
              gameObject2.GetComponent<EnemyfxIDcontainer>().titanName = ((Object) this).name;
            GameObject gameObject3 = PhotonNetwork.Instantiate("FX/colossal_steam_dmg", Vector3.op_Addition(((Component) this).transform.position, Vector3.op_Multiply(((Component) this).transform.up, 50f)), Quaternion.Euler(270f, 0.0f, 0.0f), 0);
            if (Object.op_Inequality((Object) gameObject3.GetComponent<EnemyfxIDcontainer>(), (Object) null))
              gameObject3.GetComponent<EnemyfxIDcontainer>().titanName = ((Object) this).name;
          }
        }
        else
        {
          Object.Instantiate(Resources.Load("FX/colossal_steam_dmg"), Vector3.op_Addition(((Component) this).transform.position, Vector3.op_Multiply(((Component) this).transform.forward, 185f)), Quaternion.Euler(270f, 0.0f, 0.0f));
          Object.Instantiate(Resources.Load("FX/colossal_steam_dmg"), Vector3.op_Addition(((Component) this).transform.position, Vector3.op_Multiply(((Component) this).transform.forward, 303f)), Quaternion.Euler(270f, 0.0f, 0.0f));
          Object.Instantiate(Resources.Load("FX/colossal_steam_dmg"), Vector3.op_Addition(((Component) this).transform.position, Vector3.op_Multiply(((Component) this).transform.forward, 50f)), Quaternion.Euler(270f, 0.0f, 0.0f));
        }
        if (this.hasDie)
        {
          if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
            Object.Destroy((Object) ((Component) this).gameObject);
          else if (FengGameManagerMKII.LAN)
          {
            if (this.networkView.isMine)
              ;
          }
          else if (PhotonNetwork.isMasterClient)
            PhotonNetwork.Destroy(this.photonView);
          GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().gameWin2();
        }
        this.findNearestHero();
        this.idle();
        this.playAnimation("idle");
      }
      else
      {
        int num1 = this.state == string.Empty ? 1 : 0;
      }
    }
    else if (this.attackPattern == -1)
    {
      this.slap("r1");
      ++this.attackPattern;
    }
    else if (this.attackPattern == 0)
    {
      this.attack_sweep(string.Empty);
      ++this.attackPattern;
    }
    else if (this.attackPattern == 1)
    {
      this.steam();
      ++this.attackPattern;
    }
    else if (this.attackPattern == 2)
    {
      this.kick();
      ++this.attackPattern;
    }
    else if (this.isSteamNeed || this.hasDie)
    {
      this.steam();
      this.isSteamNeed = false;
    }
    else if (Object.op_Equality((Object) this.myHero, (Object) null))
    {
      this.findNearestHero();
    }
    else
    {
      Vector3 vector3 = Vector3.op_Subtraction(this.myHero.transform.position, ((Component) this).transform.position);
      double num2 = -(double) Mathf.Atan2(vector3.z, vector3.x) * 57.295780181884766;
      Quaternion rotation = ((Component) this).gameObject.transform.rotation;
      double num3 = (double) ((Quaternion) ref rotation).eulerAngles.y - 90.0;
      float num4 = -Mathf.DeltaAngle((float) num2, (float) num3);
      this.myDistance = Mathf.Sqrt((float) (((double) this.myHero.transform.position.x - (double) ((Component) this).transform.position.x) * ((double) this.myHero.transform.position.x - (double) ((Component) this).transform.position.x) + ((double) this.myHero.transform.position.z - (double) ((Component) this).transform.position.z) * ((double) this.myHero.transform.position.z - (double) ((Component) this).transform.position.z)));
      float num5 = this.myHero.transform.position.y - ((Component) this).transform.position.y;
      if ((double) this.myDistance < 85.0 && Random.Range(0, 100) < 5)
      {
        this.steam();
      }
      else
      {
        if ((double) num5 > 310.0 && (double) num5 < 350.0)
        {
          if ((double) Vector3.Distance(this.myHero.transform.position, ((Component) this).transform.Find("APL1").position) < 40.0)
          {
            this.slap("l1");
            return;
          }
          if ((double) Vector3.Distance(this.myHero.transform.position, ((Component) this).transform.Find("APL2").position) < 40.0)
          {
            this.slap("l2");
            return;
          }
          if ((double) Vector3.Distance(this.myHero.transform.position, ((Component) this).transform.Find("APR1").position) < 40.0)
          {
            this.slap("r1");
            return;
          }
          if ((double) Vector3.Distance(this.myHero.transform.position, ((Component) this).transform.Find("APR2").position) < 40.0)
          {
            this.slap("r2");
            return;
          }
          if ((double) this.myDistance < 150.0 && (double) Mathf.Abs(num4) < 80.0)
          {
            this.attack_sweep(string.Empty);
            return;
          }
        }
        if ((double) num5 < 300.0 && (double) Mathf.Abs(num4) < 80.0 && (double) this.myDistance < 85.0)
        {
          this.attack_sweep("_vertical");
        }
        else
        {
          switch (Random.Range(0, 7))
          {
            case 0:
              this.slap("l1");
              break;
            case 1:
              this.slap("l2");
              break;
            case 2:
              this.slap("r1");
              break;
            case 3:
              this.slap("r2");
              break;
            case 4:
              this.attack_sweep(string.Empty);
              break;
            case 5:
              this.attack_sweep("_vertical");
              break;
            case 6:
              this.steam();
              break;
          }
        }
      }
    }
  }

  public void update2()
  {
    this.healthTime -= Time.deltaTime;
    this.updateLabel();
    if (!(this.state != "null"))
      return;
    if (this.state == "wait")
    {
      this.waitTime -= Time.deltaTime;
      if ((double) this.waitTime > 0.0)
        return;
      ((Component) this).transform.position = new Vector3(30f, 0.0f, 784f);
      Object.Instantiate(Resources.Load("FX/ThunderCT"), Vector3.op_Addition(((Component) this).transform.position, Vector3.op_Multiply(Vector3.up, 350f)), Quaternion.Euler(270f, 0.0f, 0.0f));
      ((Component) Camera.main).GetComponent<IN_GAME_MAIN_CAMERA>().flashBlind();
      if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
        this.idle();
      else if ((!FengGameManagerMKII.LAN ? (this.photonView.isMine ? 1 : 0) : (this.networkView.isMine ? 1 : 0)) != 0)
        this.idle();
      else
        this.state = "null";
    }
    else if (this.state != "idle")
    {
      if (this.state == "attack_sweep")
      {
        if ((double) this.attackCheckTimeA != 0.0 && ((double) ((Component) this).animation["attack_" + this.attackAnimation].normalizedTime >= (double) this.attackCheckTimeA && (double) ((Component) this).animation["attack_" + this.attackAnimation].normalizedTime <= (double) this.attackCheckTimeB || !this.attackChkOnce && (double) ((Component) this).animation["attack_" + this.attackAnimation].normalizedTime >= (double) this.attackCheckTimeA))
        {
          if (!this.attackChkOnce)
            this.attackChkOnce = true;
          foreach (RaycastHit raycastHit in this.checkHitCapsule(this.checkHitCapsuleStart.position, this.checkHitCapsuleEnd.position, this.checkHitCapsuleR))
          {
            GameObject gameObject = ((Component) ((RaycastHit) ref raycastHit).collider).gameObject;
            if (gameObject.tag == "Player")
              this.killPlayer(gameObject);
            if (gameObject.tag == "erenHitbox" && this.attackAnimation == "combo_3" && IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER && (!FengGameManagerMKII.LAN ? (PhotonNetwork.isMasterClient ? 1 : 0) : (Network.isServer ? 1 : 0)) != 0)
              ((Component) gameObject.transform.root).gameObject.GetComponent<TITAN_EREN>().hitByFTByServer(3);
          }
          foreach (RaycastHit raycastHit in this.checkHitCapsule(this.checkHitCapsuleEndOld, this.checkHitCapsuleEnd.position, this.checkHitCapsuleR))
          {
            GameObject gameObject = ((Component) ((RaycastHit) ref raycastHit).collider).gameObject;
            if (gameObject.tag == "Player")
              this.killPlayer(gameObject);
          }
          this.checkHitCapsuleEndOld = this.checkHitCapsuleEnd.position;
        }
        if ((double) ((Component) this).animation["attack_" + this.attackAnimation].normalizedTime < 1.0)
          return;
        this.sweepSmokeObject.GetComponent<ParticleSystem>().enableEmission = false;
        this.sweepSmokeObject.GetComponent<ParticleSystem>().Stop();
        if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER && !FengGameManagerMKII.LAN)
          this.photonView.RPC("stopSweepSmoke", PhotonTargets.Others);
        this.findNearestHero();
        this.idle();
        this.playAnimation("idle");
      }
      else if (this.state == "kick")
      {
        if (!this.attackChkOnce && (double) ((Component) this).animation[this.actionName].normalizedTime >= (double) this.attackCheckTime)
        {
          this.attackChkOnce = true;
          this.door_broken.SetActive(true);
          this.door_closed.SetActive(false);
          if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER && !FengGameManagerMKII.LAN)
            this.photonView.RPC("changeDoor", PhotonTargets.OthersBuffered);
          if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)
          {
            if (FengGameManagerMKII.LAN)
            {
              Network.Instantiate(Resources.Load("FX/boom1_CT_KICK"), Vector3.op_Addition(Vector3.op_Addition(((Component) this).transform.position, Vector3.op_Multiply(((Component) this).transform.forward, 120f)), Vector3.op_Multiply(((Component) this).transform.right, 30f)), Quaternion.Euler(270f, 0.0f, 0.0f), 0);
              Network.Instantiate(Resources.Load("rock"), Vector3.op_Addition(Vector3.op_Addition(((Component) this).transform.position, Vector3.op_Multiply(((Component) this).transform.forward, 120f)), Vector3.op_Multiply(((Component) this).transform.right, 30f)), Quaternion.Euler(0.0f, 0.0f, 0.0f), 0);
            }
            else
            {
              PhotonNetwork.Instantiate("FX/boom1_CT_KICK", Vector3.op_Addition(Vector3.op_Addition(((Component) this).transform.position, Vector3.op_Multiply(((Component) this).transform.forward, 120f)), Vector3.op_Multiply(((Component) this).transform.right, 30f)), Quaternion.Euler(270f, 0.0f, 0.0f), 0);
              PhotonNetwork.Instantiate("rock", Vector3.op_Addition(Vector3.op_Addition(((Component) this).transform.position, Vector3.op_Multiply(((Component) this).transform.forward, 120f)), Vector3.op_Multiply(((Component) this).transform.right, 30f)), Quaternion.Euler(0.0f, 0.0f, 0.0f), 0);
            }
          }
          else
          {
            Object.Instantiate(Resources.Load("FX/boom1_CT_KICK"), Vector3.op_Addition(Vector3.op_Addition(((Component) this).transform.position, Vector3.op_Multiply(((Component) this).transform.forward, 120f)), Vector3.op_Multiply(((Component) this).transform.right, 30f)), Quaternion.Euler(270f, 0.0f, 0.0f));
            Object.Instantiate(Resources.Load("rock"), Vector3.op_Addition(Vector3.op_Addition(((Component) this).transform.position, Vector3.op_Multiply(((Component) this).transform.forward, 120f)), Vector3.op_Multiply(((Component) this).transform.right, 30f)), Quaternion.Euler(0.0f, 0.0f, 0.0f));
          }
        }
        if ((double) ((Component) this).animation[this.actionName].normalizedTime < 1.0)
          return;
        this.findNearestHero();
        this.idle();
        this.playAnimation("idle");
      }
      else if (this.state == "slap")
      {
        if (!this.attackChkOnce && (double) ((Component) this).animation["attack_slap_" + this.attackAnimation].normalizedTime >= (double) this.attackCheckTime)
        {
          this.attackChkOnce = true;
          GameObject gameObject;
          if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)
          {
            gameObject = !FengGameManagerMKII.LAN ? PhotonNetwork.Instantiate("FX/boom1", this.checkHitCapsuleStart.position, Quaternion.Euler(270f, 0.0f, 0.0f), 0) : (GameObject) Network.Instantiate(Resources.Load("FX/boom1"), this.checkHitCapsuleStart.position, Quaternion.Euler(270f, 0.0f, 0.0f), 0);
            if (Object.op_Inequality((Object) gameObject.GetComponent<EnemyfxIDcontainer>(), (Object) null))
              gameObject.GetComponent<EnemyfxIDcontainer>().titanName = ((Object) this).name;
          }
          else
            gameObject = (GameObject) Object.Instantiate(Resources.Load("FX/boom1"), this.checkHitCapsuleStart.position, Quaternion.Euler(270f, 0.0f, 0.0f));
          gameObject.transform.localScale = new Vector3(5f, 5f, 5f);
        }
        if ((double) ((Component) this).animation["attack_slap_" + this.attackAnimation].normalizedTime < 1.0)
          return;
        this.findNearestHero();
        this.idle();
        this.playAnimation("idle");
      }
      else if (this.state == "steam")
      {
        if (!this.attackChkOnce && (double) ((Component) this).animation[this.actionName].normalizedTime >= (double) this.attackCheckTime)
        {
          this.attackChkOnce = true;
          if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)
          {
            if (FengGameManagerMKII.LAN)
            {
              Network.Instantiate(Resources.Load("FX/colossal_steam"), Vector3.op_Addition(((Component) this).transform.position, Vector3.op_Multiply(((Component) this).transform.up, 185f)), Quaternion.Euler(270f, 0.0f, 0.0f), 0);
              Network.Instantiate(Resources.Load("FX/colossal_steam"), Vector3.op_Addition(((Component) this).transform.position, Vector3.op_Multiply(((Component) this).transform.up, 303f)), Quaternion.Euler(270f, 0.0f, 0.0f), 0);
              Network.Instantiate(Resources.Load("FX/colossal_steam"), Vector3.op_Addition(((Component) this).transform.position, Vector3.op_Multiply(((Component) this).transform.up, 50f)), Quaternion.Euler(270f, 0.0f, 0.0f), 0);
            }
            else
            {
              PhotonNetwork.Instantiate("FX/colossal_steam", Vector3.op_Addition(((Component) this).transform.position, Vector3.op_Multiply(((Component) this).transform.up, 185f)), Quaternion.Euler(270f, 0.0f, 0.0f), 0);
              PhotonNetwork.Instantiate("FX/colossal_steam", Vector3.op_Addition(((Component) this).transform.position, Vector3.op_Multiply(((Component) this).transform.up, 303f)), Quaternion.Euler(270f, 0.0f, 0.0f), 0);
              PhotonNetwork.Instantiate("FX/colossal_steam", Vector3.op_Addition(((Component) this).transform.position, Vector3.op_Multiply(((Component) this).transform.up, 50f)), Quaternion.Euler(270f, 0.0f, 0.0f), 0);
            }
          }
          else
          {
            Object.Instantiate(Resources.Load("FX/colossal_steam"), Vector3.op_Addition(((Component) this).transform.position, Vector3.op_Multiply(((Component) this).transform.forward, 185f)), Quaternion.Euler(270f, 0.0f, 0.0f));
            Object.Instantiate(Resources.Load("FX/colossal_steam"), Vector3.op_Addition(((Component) this).transform.position, Vector3.op_Multiply(((Component) this).transform.forward, 303f)), Quaternion.Euler(270f, 0.0f, 0.0f));
            Object.Instantiate(Resources.Load("FX/colossal_steam"), Vector3.op_Addition(((Component) this).transform.position, Vector3.op_Multiply(((Component) this).transform.forward, 50f)), Quaternion.Euler(270f, 0.0f, 0.0f));
          }
        }
        if ((double) ((Component) this).animation[this.actionName].normalizedTime < 1.0)
          return;
        if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)
        {
          if (FengGameManagerMKII.LAN)
          {
            Network.Instantiate(Resources.Load("FX/colossal_steam_dmg"), Vector3.op_Addition(((Component) this).transform.position, Vector3.op_Multiply(((Component) this).transform.up, 185f)), Quaternion.Euler(270f, 0.0f, 0.0f), 0);
            Network.Instantiate(Resources.Load("FX/colossal_steam_dmg"), Vector3.op_Addition(((Component) this).transform.position, Vector3.op_Multiply(((Component) this).transform.up, 303f)), Quaternion.Euler(270f, 0.0f, 0.0f), 0);
            Network.Instantiate(Resources.Load("FX/colossal_steam_dmg"), Vector3.op_Addition(((Component) this).transform.position, Vector3.op_Multiply(((Component) this).transform.up, 50f)), Quaternion.Euler(270f, 0.0f, 0.0f), 0);
          }
          else
          {
            GameObject gameObject1 = PhotonNetwork.Instantiate("FX/colossal_steam_dmg", Vector3.op_Addition(((Component) this).transform.position, Vector3.op_Multiply(((Component) this).transform.up, 185f)), Quaternion.Euler(270f, 0.0f, 0.0f), 0);
            if (Object.op_Inequality((Object) gameObject1.GetComponent<EnemyfxIDcontainer>(), (Object) null))
              gameObject1.GetComponent<EnemyfxIDcontainer>().titanName = ((Object) this).name;
            GameObject gameObject2 = PhotonNetwork.Instantiate("FX/colossal_steam_dmg", Vector3.op_Addition(((Component) this).transform.position, Vector3.op_Multiply(((Component) this).transform.up, 303f)), Quaternion.Euler(270f, 0.0f, 0.0f), 0);
            if (Object.op_Inequality((Object) gameObject2.GetComponent<EnemyfxIDcontainer>(), (Object) null))
              gameObject2.GetComponent<EnemyfxIDcontainer>().titanName = ((Object) this).name;
            GameObject gameObject3 = PhotonNetwork.Instantiate("FX/colossal_steam_dmg", Vector3.op_Addition(((Component) this).transform.position, Vector3.op_Multiply(((Component) this).transform.up, 50f)), Quaternion.Euler(270f, 0.0f, 0.0f), 0);
            if (Object.op_Inequality((Object) gameObject3.GetComponent<EnemyfxIDcontainer>(), (Object) null))
              gameObject3.GetComponent<EnemyfxIDcontainer>().titanName = ((Object) this).name;
          }
        }
        else
        {
          Object.Instantiate(Resources.Load("FX/colossal_steam_dmg"), Vector3.op_Addition(((Component) this).transform.position, Vector3.op_Multiply(((Component) this).transform.forward, 185f)), Quaternion.Euler(270f, 0.0f, 0.0f));
          Object.Instantiate(Resources.Load("FX/colossal_steam_dmg"), Vector3.op_Addition(((Component) this).transform.position, Vector3.op_Multiply(((Component) this).transform.forward, 303f)), Quaternion.Euler(270f, 0.0f, 0.0f));
          Object.Instantiate(Resources.Load("FX/colossal_steam_dmg"), Vector3.op_Addition(((Component) this).transform.position, Vector3.op_Multiply(((Component) this).transform.forward, 50f)), Quaternion.Euler(270f, 0.0f, 0.0f));
        }
        if (this.hasDie)
        {
          if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
            Object.Destroy((Object) ((Component) this).gameObject);
          else if (FengGameManagerMKII.LAN)
          {
            if (this.networkView.isMine)
              ;
          }
          else if (PhotonNetwork.isMasterClient)
            PhotonNetwork.Destroy(this.photonView);
          GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().gameWin2();
        }
        this.findNearestHero();
        this.idle();
        this.playAnimation("idle");
      }
      else
      {
        int num1 = this.state == string.Empty ? 1 : 0;
      }
    }
    else if (this.attackPattern == -1)
    {
      this.slap("r1");
      ++this.attackPattern;
    }
    else if (this.attackPattern == 0)
    {
      this.attack_sweep(string.Empty);
      ++this.attackPattern;
    }
    else if (this.attackPattern == 1)
    {
      this.steam();
      ++this.attackPattern;
    }
    else if (this.attackPattern == 2)
    {
      this.kick();
      ++this.attackPattern;
    }
    else if (this.isSteamNeed || this.hasDie)
    {
      this.steam();
      this.isSteamNeed = false;
    }
    else if (Object.op_Equality((Object) this.myHero, (Object) null))
    {
      this.findNearestHero();
    }
    else
    {
      Vector3 vector3 = Vector3.op_Subtraction(this.myHero.transform.position, ((Component) this).transform.position);
      double num2 = -(double) Mathf.Atan2(vector3.z, vector3.x) * 57.295780181884766;
      Quaternion rotation = ((Component) this).gameObject.transform.rotation;
      double num3 = (double) ((Quaternion) ref rotation).eulerAngles.y - 90.0;
      float num4 = -Mathf.DeltaAngle((float) num2, (float) num3);
      this.myDistance = Mathf.Sqrt((float) (((double) this.myHero.transform.position.x - (double) ((Component) this).transform.position.x) * ((double) this.myHero.transform.position.x - (double) ((Component) this).transform.position.x) + ((double) this.myHero.transform.position.z - (double) ((Component) this).transform.position.z) * ((double) this.myHero.transform.position.z - (double) ((Component) this).transform.position.z)));
      float num5 = this.myHero.transform.position.y - ((Component) this).transform.position.y;
      if ((double) this.myDistance < 85.0 && Random.Range(0, 100) < 5)
      {
        this.steam();
      }
      else
      {
        if ((double) num5 > 310.0 && (double) num5 < 350.0)
        {
          if ((double) Vector3.Distance(this.myHero.transform.position, ((Component) this).transform.Find("APL1").position) < 40.0)
          {
            this.slap("l1");
            return;
          }
          if ((double) Vector3.Distance(this.myHero.transform.position, ((Component) this).transform.Find("APL2").position) < 40.0)
          {
            this.slap("l2");
            return;
          }
          if ((double) Vector3.Distance(this.myHero.transform.position, ((Component) this).transform.Find("APR1").position) < 40.0)
          {
            this.slap("r1");
            return;
          }
          if ((double) Vector3.Distance(this.myHero.transform.position, ((Component) this).transform.Find("APR2").position) < 40.0)
          {
            this.slap("r2");
            return;
          }
          if ((double) this.myDistance < 150.0 && (double) Mathf.Abs(num4) < 80.0)
          {
            this.attack_sweep(string.Empty);
            return;
          }
        }
        if ((double) num5 < 300.0 && (double) Mathf.Abs(num4) < 80.0 && (double) this.myDistance < 85.0)
        {
          this.attack_sweep("_vertical");
        }
        else
        {
          switch (Random.Range(0, 7))
          {
            case 0:
              this.slap("l1");
              break;
            case 1:
              this.slap("l2");
              break;
            case 2:
              this.slap("r1");
              break;
            case 3:
              this.slap("r2");
              break;
            case 4:
              this.attack_sweep(string.Empty);
              break;
            case 5:
              this.attack_sweep("_vertical");
              break;
            case 6:
              this.steam();
              break;
          }
        }
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
