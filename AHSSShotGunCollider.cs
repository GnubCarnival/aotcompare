// Decompiled with JetBrains decompiler
// Type: AHSSShotGunCollider
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using GameProgress;
using Settings;
using System.Collections;
using UnityEngine;

public class AHSSShotGunCollider : MonoBehaviour
{
  public bool active_me;
  private int count;
  public GameObject currentCamera;
  public ArrayList currentHits = new ArrayList();
  public int dmg = 1;
  private int myTeam = 1;
  private string ownerName = string.Empty;
  public float scoreMulti;
  private int viewID = -1;

  private bool checkIfBehind(GameObject titan)
  {
    Transform transform = titan.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck/head");
    Vector3 vector3 = Vector3.op_Subtraction(((Component) this).transform.position, ((Component) transform).transform.position);
    Debug.DrawRay(((Component) transform).transform.position, Vector3.op_Multiply(Vector3.op_UnaryNegation(((Component) transform).transform.forward), 10f), Color.white, 5f);
    Debug.DrawRay(((Component) transform).transform.position, Vector3.op_Multiply(vector3, 10f), Color.green, 5f);
    return (double) Vector3.Angle(Vector3.op_UnaryNegation(((Component) transform).transform.forward), vector3) < 100.0;
  }

  private void FixedUpdate()
  {
    if (this.count > 1)
      this.active_me = false;
    else
      ++this.count;
  }

  private void OnTriggerStay(Collider other)
  {
    if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER && !((Component) ((Component) this).transform.root).gameObject.GetPhotonView().isMine || !this.active_me)
      return;
    GameObject gameObject1 = ((Component) ((Component) this).transform.root).gameObject;
    if (((Component) other).gameObject.tag == "playerHitbox")
    {
      if (!LevelInfo.getInfo(FengGameManagerMKII.level).pvp)
        return;
      float num = Mathf.Min(1f, (float) (1.0 - (double) Vector3.Distance(((Component) other).gameObject.transform.position, ((Component) this).transform.position) * 0.05000000074505806));
      HitBox component = ((Component) other).gameObject.GetComponent<HitBox>();
      if (!Object.op_Inequality((Object) component, (Object) null) || !Object.op_Inequality((Object) ((Component) component).transform.root, (Object) null) || ((Component) ((Component) component).transform.root).GetComponent<HERO>().myTeam == this.myTeam || ((Component) ((Component) component).transform.root).GetComponent<HERO>().isInvincible())
        return;
      switch (IN_GAME_MAIN_CAMERA.gametype)
      {
        case GAMETYPE.SINGLE:
          if (((Component) ((Component) component).transform.root).GetComponent<HERO>().isGrabbed)
            break;
          Vector3 vector3_1 = Vector3.op_Subtraction(((Component) ((Component) component).transform.root).transform.position, ((Component) this).transform.position);
          ((Component) ((Component) component).transform.root).GetComponent<HERO>().die(Vector3.op_Addition(Vector3.op_Multiply(Vector3.op_Multiply(((Vector3) ref vector3_1).normalized, num), 1000f), Vector3.op_Multiply(Vector3.up, 50f)), false);
          GameProgressManager.RegisterHumanKill(gameObject1, ((Component) ((Component) component).transform.root).GetComponent<HERO>(), KillWeapon.Gun);
          break;
        case GAMETYPE.MULTIPLAYER:
          if (((Component) ((Component) component).transform.root).GetComponent<HERO>().HasDied() || ((Component) ((Component) component).transform.root).GetComponent<HERO>().isGrabbed)
            break;
          ((Component) ((Component) component).transform.root).GetComponent<HERO>().markDie();
          object[] objArray = new object[5];
          Vector3 vector3_2 = Vector3.op_Subtraction(((Component) component).transform.root.position, ((Component) this).transform.position);
          objArray[0] = (object) Vector3.op_Addition(Vector3.op_Multiply(Vector3.op_Multiply(((Vector3) ref vector3_2).normalized, num), 1000f), Vector3.op_Multiply(Vector3.up, 50f));
          objArray[1] = (object) false;
          objArray[2] = (object) this.viewID;
          objArray[3] = (object) this.ownerName;
          objArray[4] = (object) false;
          ((Component) ((Component) component).transform.root).GetComponent<HERO>().photonView.RPC("netDie", PhotonTargets.All, objArray);
          GameProgressManager.RegisterHumanKill(gameObject1, ((Component) ((Component) component).transform.root).GetComponent<HERO>(), KillWeapon.Gun);
          break;
      }
    }
    else if (((Component) other).gameObject.tag == "erenHitbox")
    {
      if (this.dmg <= 0 || ((Component) ((Component) other).gameObject.transform.root).gameObject.GetComponent<TITAN_EREN>().isHit)
        return;
      ((Component) ((Component) other).gameObject.transform.root).gameObject.GetComponent<TITAN_EREN>().hitByTitan();
    }
    else if (((Component) other).gameObject.tag == "titanneck")
    {
      HitBox component1 = ((Component) other).gameObject.GetComponent<HitBox>();
      if (!Object.op_Inequality((Object) component1, (Object) null) || !this.checkIfBehind(((Component) ((Component) component1).transform.root).gameObject) || this.currentHits.Contains((object) component1))
        return;
      component1.hitPosition = Vector3.op_Multiply(Vector3.op_Addition(((Component) this).transform.position, ((Component) component1).transform.position), 0.5f);
      this.currentHits.Add((object) component1);
      if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
      {
        if (Object.op_Inequality((Object) ((Component) ((Component) component1).transform.root).GetComponent<TITAN>(), (Object) null) && !((Component) ((Component) component1).transform.root).GetComponent<TITAN>().hasDie)
        {
          TITAN component2 = ((Component) ((Component) component1).transform.root).GetComponent<TITAN>();
          Vector3 vector3 = Vector3.op_Subtraction(this.currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().main_object.rigidbody.velocity, ((Component) ((Component) component1).transform.root).rigidbody.velocity);
          int num = Mathf.Max(10, (int) ((double) ((Vector3) ref vector3).magnitude * 10.0 * (double) this.scoreMulti));
          GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().netShowDamage(num);
          GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().ReportKillToChatFeed("You", "Titan", num);
          GameProgressManager.RegisterDamage(gameObject1, ((Component) component2).gameObject, KillWeapon.Gun, num);
          if ((double) num > (double) component2.myLevel * 100.0)
          {
            component2.die();
            GameProgressManager.RegisterTitanKill(gameObject1, component2, KillWeapon.Gun);
            if (SettingsManager.GeneralSettings.SnapshotsEnabled.Value)
              GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().startSnapShot2(((Component) component1).transform.position, num, ((Component) ((Component) component1).transform.root).gameObject, 0.02f);
            GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().playerKillInfoSingleUpdate(num);
          }
        }
      }
      else if (!PhotonNetwork.isMasterClient)
      {
        if (Object.op_Inequality((Object) ((Component) ((Component) component1).transform.root).GetComponent<TITAN>(), (Object) null))
        {
          if (!((Component) ((Component) component1).transform.root).GetComponent<TITAN>().hasDie)
          {
            TITAN component3 = ((Component) ((Component) component1).transform.root).GetComponent<TITAN>();
            Vector3 vector3 = Vector3.op_Subtraction(this.currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().main_object.rigidbody.velocity, ((Component) ((Component) component1).transform.root).rigidbody.velocity);
            int num = Mathf.Max(10, (int) ((double) ((Vector3) ref vector3).magnitude * 10.0 * (double) this.scoreMulti));
            GameProgressManager.RegisterDamage(gameObject1, ((Component) component3).gameObject, KillWeapon.Gun, num);
            if ((double) num > (double) component3.myLevel * 100.0)
            {
              if (component3.WillDIe(num))
                GameProgressManager.RegisterTitanKill(gameObject1, component3, KillWeapon.Gun);
              if (SettingsManager.GeneralSettings.SnapshotsEnabled.Value)
              {
                GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().startSnapShot2(((Component) component1).transform.position, num, ((Component) ((Component) component1).transform.root).gameObject, 0.02f);
                ((Component) ((Component) component1).transform.root).GetComponent<TITAN>().asClientLookTarget = false;
              }
              object[] objArray = new object[2]
              {
                (object) ((Component) ((Component) this).transform.root).gameObject.GetPhotonView().viewID,
                (object) num
              };
              ((Component) ((Component) component1).transform.root).GetComponent<TITAN>().photonView.RPC("titanGetHit", ((Component) ((Component) component1).transform.root).GetComponent<TITAN>().photonView.owner, objArray);
            }
          }
        }
        else if (Object.op_Inequality((Object) ((Component) ((Component) component1).transform.root).GetComponent<FEMALE_TITAN>(), (Object) null))
        {
          Vector3 vector3 = Vector3.op_Subtraction(this.currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().main_object.rigidbody.velocity, ((Component) ((Component) component1).transform.root).rigidbody.velocity);
          int num = Mathf.Max(10, (int) ((double) ((Vector3) ref vector3).magnitude * 10.0 * (double) this.scoreMulti));
          if (!((Component) ((Component) component1).transform.root).GetComponent<FEMALE_TITAN>().hasDie)
          {
            object[] objArray = new object[2]
            {
              (object) ((Component) ((Component) this).transform.root).gameObject.GetPhotonView().viewID,
              (object) num
            };
            ((Component) ((Component) component1).transform.root).GetComponent<FEMALE_TITAN>().photonView.RPC("titanGetHit", ((Component) ((Component) component1).transform.root).GetComponent<FEMALE_TITAN>().photonView.owner, objArray);
          }
        }
        else if (Object.op_Inequality((Object) ((Component) ((Component) component1).transform.root).GetComponent<COLOSSAL_TITAN>(), (Object) null) && !((Component) ((Component) component1).transform.root).GetComponent<COLOSSAL_TITAN>().hasDie)
        {
          Vector3 vector3 = Vector3.op_Subtraction(this.currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().main_object.rigidbody.velocity, ((Component) ((Component) component1).transform.root).rigidbody.velocity);
          int num = Mathf.Max(10, (int) ((double) ((Vector3) ref vector3).magnitude * 10.0 * (double) this.scoreMulti));
          object[] objArray = new object[2]
          {
            (object) ((Component) ((Component) this).transform.root).gameObject.GetPhotonView().viewID,
            (object) num
          };
          ((Component) ((Component) component1).transform.root).GetComponent<COLOSSAL_TITAN>().photonView.RPC("titanGetHit", ((Component) ((Component) component1).transform.root).GetComponent<COLOSSAL_TITAN>().photonView.owner, objArray);
        }
      }
      else if (Object.op_Inequality((Object) ((Component) ((Component) component1).transform.root).GetComponent<TITAN>(), (Object) null))
      {
        TITAN component4 = ((Component) ((Component) component1).transform.root).GetComponent<TITAN>();
        if (!component4.hasDie)
        {
          Vector3 vector3 = Vector3.op_Subtraction(this.currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().main_object.rigidbody.velocity, ((Component) ((Component) component1).transform.root).rigidbody.velocity);
          int num = Mathf.Max(10, (int) ((double) ((Vector3) ref vector3).magnitude * 10.0 * (double) this.scoreMulti));
          GameProgressManager.RegisterDamage(gameObject1, ((Component) component4).gameObject, KillWeapon.Gun, num);
          if ((double) num > (double) component4.myLevel * 100.0)
          {
            if (component4.WillDIe(num))
              GameProgressManager.RegisterTitanKill(gameObject1, component4, KillWeapon.Gun);
            if (SettingsManager.GeneralSettings.SnapshotsEnabled.Value)
              GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().startSnapShot2(((Component) component1).transform.position, num, ((Component) ((Component) component1).transform.root).gameObject, 0.02f);
            ((Component) ((Component) component1).transform.root).GetComponent<TITAN>().titanGetHit(((Component) ((Component) this).transform.root).gameObject.GetPhotonView().viewID, num);
          }
        }
      }
      else if (Object.op_Inequality((Object) ((Component) ((Component) component1).transform.root).GetComponent<FEMALE_TITAN>(), (Object) null))
      {
        if (!((Component) ((Component) component1).transform.root).GetComponent<FEMALE_TITAN>().hasDie)
        {
          Vector3 vector3 = Vector3.op_Subtraction(this.currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().main_object.rigidbody.velocity, ((Component) ((Component) component1).transform.root).rigidbody.velocity);
          int num = Mathf.Max(10, (int) ((double) ((Vector3) ref vector3).magnitude * 10.0 * (double) this.scoreMulti));
          if (SettingsManager.GeneralSettings.SnapshotsEnabled.Value)
            GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().startSnapShot2(((Component) component1).transform.position, num, (GameObject) null, 0.02f);
          ((Component) ((Component) component1).transform.root).GetComponent<FEMALE_TITAN>().titanGetHit(((Component) ((Component) this).transform.root).gameObject.GetPhotonView().viewID, num);
        }
      }
      else if (Object.op_Inequality((Object) ((Component) ((Component) component1).transform.root).GetComponent<COLOSSAL_TITAN>(), (Object) null) && !((Component) ((Component) component1).transform.root).GetComponent<COLOSSAL_TITAN>().hasDie)
      {
        Vector3 vector3 = Vector3.op_Subtraction(this.currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().main_object.rigidbody.velocity, ((Component) ((Component) component1).transform.root).rigidbody.velocity);
        int num = Mathf.Max(10, (int) ((double) ((Vector3) ref vector3).magnitude * 10.0 * (double) this.scoreMulti));
        if (SettingsManager.GeneralSettings.SnapshotsEnabled.Value)
          GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().startSnapShot2(((Component) component1).transform.position, num, (GameObject) null, 0.02f);
        ((Component) ((Component) component1).transform.root).GetComponent<COLOSSAL_TITAN>().titanGetHit(((Component) ((Component) this).transform.root).gameObject.GetPhotonView().viewID, num);
      }
      this.showCriticalHitFX(((Component) other).gameObject.transform.position);
    }
    else if (((Component) other).gameObject.tag == "titaneye")
    {
      if (this.currentHits.Contains((object) ((Component) other).gameObject))
        return;
      this.currentHits.Add((object) ((Component) other).gameObject);
      GameObject gameObject2 = ((Component) ((Component) other).gameObject.transform.root).gameObject;
      if (Object.op_Inequality((Object) gameObject2.GetComponent<FEMALE_TITAN>(), (Object) null))
      {
        if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
        {
          if (gameObject2.GetComponent<FEMALE_TITAN>().hasDie)
            return;
          gameObject2.GetComponent<FEMALE_TITAN>().hitEye();
        }
        else if (!PhotonNetwork.isMasterClient)
        {
          if (gameObject2.GetComponent<FEMALE_TITAN>().hasDie)
            return;
          object[] objArray = new object[1]
          {
            (object) ((Component) ((Component) this).transform.root).gameObject.GetPhotonView().viewID
          };
          gameObject2.GetComponent<FEMALE_TITAN>().photonView.RPC("hitEyeRPC", PhotonTargets.MasterClient, objArray);
        }
        else
        {
          if (gameObject2.GetComponent<FEMALE_TITAN>().hasDie)
            return;
          gameObject2.GetComponent<FEMALE_TITAN>().hitEyeRPC(((Component) ((Component) this).transform.root).gameObject.GetPhotonView().viewID);
        }
      }
      else
      {
        if (gameObject2.GetComponent<TITAN>().abnormalType == AbnormalType.TYPE_CRAWLER)
          return;
        if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
        {
          if (!gameObject2.GetComponent<TITAN>().hasDie)
            gameObject2.GetComponent<TITAN>().hitEye();
        }
        else if (!PhotonNetwork.isMasterClient)
        {
          if (!gameObject2.GetComponent<TITAN>().hasDie)
          {
            object[] objArray = new object[1]
            {
              (object) ((Component) ((Component) this).transform.root).gameObject.GetPhotonView().viewID
            };
            gameObject2.GetComponent<TITAN>().photonView.RPC("hitEyeRPC", PhotonTargets.MasterClient, objArray);
          }
        }
        else if (!gameObject2.GetComponent<TITAN>().hasDie)
          gameObject2.GetComponent<TITAN>().hitEyeRPC(((Component) ((Component) this).transform.root).gameObject.GetPhotonView().viewID);
        this.showCriticalHitFX(((Component) other).gameObject.transform.position);
      }
    }
    else
    {
      if (!(((Component) other).gameObject.tag == "titanankle") || this.currentHits.Contains((object) ((Component) other).gameObject))
        return;
      this.currentHits.Add((object) ((Component) other).gameObject);
      GameObject gameObject3 = ((Component) ((Component) other).gameObject.transform.root).gameObject;
      Vector3 vector3 = Vector3.op_Subtraction(this.currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().main_object.rigidbody.velocity, gameObject3.rigidbody.velocity);
      int dmg = Mathf.Max(10, (int) ((double) ((Vector3) ref vector3).magnitude * 10.0 * (double) this.scoreMulti));
      if (Object.op_Inequality((Object) gameObject3.GetComponent<TITAN>(), (Object) null) && gameObject3.GetComponent<TITAN>().abnormalType != AbnormalType.TYPE_CRAWLER)
      {
        if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
        {
          if (gameObject3.GetComponent<TITAN>().hasDie)
            return;
          gameObject3.GetComponent<TITAN>().hitAnkle();
        }
        else
        {
          if (!PhotonNetwork.isMasterClient)
          {
            if (!gameObject3.GetComponent<TITAN>().hasDie)
            {
              object[] objArray = new object[1]
              {
                (object) ((Component) ((Component) this).transform.root).gameObject.GetPhotonView().viewID
              };
              gameObject3.GetComponent<TITAN>().photonView.RPC("hitAnkleRPC", PhotonTargets.MasterClient, objArray);
            }
          }
          else if (!gameObject3.GetComponent<TITAN>().hasDie)
            gameObject3.GetComponent<TITAN>().hitAnkle();
          this.showCriticalHitFX(((Component) other).gameObject.transform.position);
        }
      }
      else
      {
        if (!Object.op_Inequality((Object) gameObject3.GetComponent<FEMALE_TITAN>(), (Object) null))
          return;
        if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
        {
          if (((Object) ((Component) other).gameObject).name == "ankleR")
          {
            if (Object.op_Inequality((Object) gameObject3.GetComponent<FEMALE_TITAN>(), (Object) null) && !gameObject3.GetComponent<FEMALE_TITAN>().hasDie)
              gameObject3.GetComponent<FEMALE_TITAN>().hitAnkleR(dmg);
          }
          else if (Object.op_Inequality((Object) gameObject3.GetComponent<FEMALE_TITAN>(), (Object) null) && !gameObject3.GetComponent<FEMALE_TITAN>().hasDie)
            gameObject3.GetComponent<FEMALE_TITAN>().hitAnkleL(dmg);
        }
        else if (((Object) ((Component) other).gameObject).name == "ankleR")
        {
          if (!PhotonNetwork.isMasterClient)
          {
            if (!gameObject3.GetComponent<FEMALE_TITAN>().hasDie)
            {
              object[] objArray = new object[2]
              {
                (object) ((Component) ((Component) this).transform.root).gameObject.GetPhotonView().viewID,
                (object) dmg
              };
              gameObject3.GetComponent<FEMALE_TITAN>().photonView.RPC("hitAnkleRRPC", PhotonTargets.MasterClient, objArray);
            }
          }
          else if (!gameObject3.GetComponent<FEMALE_TITAN>().hasDie)
            gameObject3.GetComponent<FEMALE_TITAN>().hitAnkleRRPC(((Component) ((Component) this).transform.root).gameObject.GetPhotonView().viewID, dmg);
        }
        else if (!PhotonNetwork.isMasterClient)
        {
          if (!gameObject3.GetComponent<FEMALE_TITAN>().hasDie)
          {
            object[] objArray = new object[2]
            {
              (object) ((Component) ((Component) this).transform.root).gameObject.GetPhotonView().viewID,
              (object) dmg
            };
            gameObject3.GetComponent<FEMALE_TITAN>().photonView.RPC("hitAnkleLRPC", PhotonTargets.MasterClient, objArray);
          }
        }
        else if (!gameObject3.GetComponent<FEMALE_TITAN>().hasDie)
          gameObject3.GetComponent<FEMALE_TITAN>().hitAnkleLRPC(((Component) ((Component) this).transform.root).gameObject.GetPhotonView().viewID, dmg);
        this.showCriticalHitFX(((Component) other).gameObject.transform.position);
      }
    }
  }

  private void showCriticalHitFX(Vector3 position)
  {
    this.currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().startShake(0.2f, 0.3f);
    (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE ? (GameObject) Object.Instantiate(Resources.Load("redCross1")) : PhotonNetwork.Instantiate("redCross1", ((Component) this).transform.position, Quaternion.Euler(270f, 0.0f, 0.0f), 0)).transform.position = position;
  }

  private void Start()
  {
    if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)
    {
      if (!((Component) ((Component) this).transform.root).gameObject.GetPhotonView().isMine)
      {
        ((Behaviour) this).enabled = false;
        return;
      }
      if (Object.op_Inequality((Object) ((Component) ((Component) this).transform.root).gameObject.GetComponent<EnemyfxIDcontainer>(), (Object) null))
      {
        this.viewID = ((Component) ((Component) this).transform.root).gameObject.GetComponent<EnemyfxIDcontainer>().myOwnerViewID;
        this.ownerName = ((Component) ((Component) this).transform.root).gameObject.GetComponent<EnemyfxIDcontainer>().titanName;
        this.myTeam = ((Component) PhotonView.Find(this.viewID)).gameObject.GetComponent<HERO>().myTeam;
      }
    }
    else
      this.myTeam = GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().main_object.GetComponent<HERO>().myTeam;
    this.active_me = true;
    this.count = 0;
    this.currentCamera = GameObject.Find("MainCamera");
  }
}
