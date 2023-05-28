// Decompiled with JetBrains decompiler
// Type: TriggerColliderWeapon
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using GameProgress;
using Settings;
using System.Collections;
using UnityEngine;

public class TriggerColliderWeapon : MonoBehaviour
{
  public bool active_me;
  public GameObject currentCamera;
  public ArrayList currentHits = new ArrayList();
  public ArrayList currentHitsII = new ArrayList();
  public AudioSource meatDie;
  public int myTeam = 1;
  public float scoreMulti = 1f;

  private bool checkIfBehind(GameObject titan)
  {
    Transform transform = titan.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck/head");
    Vector3 vector3 = Vector3.op_Subtraction(((Component) this).transform.position, ((Component) transform).transform.position);
    return (double) Vector3.Angle(Vector3.op_UnaryNegation(((Component) transform).transform.forward), vector3) < 70.0;
  }

  public void clearHits()
  {
    this.currentHitsII = new ArrayList();
    this.currentHits = new ArrayList();
  }

  private void napeMeat(Vector3 vkill, Transform titan)
  {
    Transform transform = ((Component) titan).transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck");
    GameObject gameObject = (GameObject) Object.Instantiate(Resources.Load("titanNapeMeat"), transform.position, transform.rotation);
    gameObject.transform.localScale = titan.localScale;
    gameObject.rigidbody.AddForce(Vector3.op_Multiply(((Vector3) ref vkill).normalized, 15f), (ForceMode) 1);
    gameObject.rigidbody.AddForce(Vector3.op_Multiply(Vector3.op_UnaryNegation(titan.forward), 10f), (ForceMode) 1);
    gameObject.rigidbody.AddTorque(new Vector3((float) Random.Range(-100, 100), (float) Random.Range(-100, 100), (float) Random.Range(-100, 100)), (ForceMode) 1);
  }

  private void OnTriggerStay(Collider other)
  {
    if (!this.active_me)
      return;
    HERO component1 = ((Component) ((Component) this).transform.root).GetComponent<HERO>();
    if (!this.currentHitsII.Contains((object) ((Component) other).gameObject))
    {
      this.currentHitsII.Add((object) ((Component) other).gameObject);
      this.currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().startShake(0.1f, 0.1f);
      if (((Component) ((Component) other).transform.root).gameObject.tag == "titan")
      {
        component1.slashHit.Play();
        (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE ? (GameObject) Object.Instantiate(Resources.Load("hitMeat")) : PhotonNetwork.Instantiate("hitMeat", ((Component) this).transform.position, Quaternion.Euler(270f, 0.0f, 0.0f), 0)).transform.position = ((Component) this).transform.position;
        component1.useBlade();
      }
    }
    if (((Component) other).gameObject.tag == "playerHitbox")
    {
      if (!LevelInfo.getInfo(FengGameManagerMKII.level).pvp)
        return;
      float num = Mathf.Min(1f, (float) (1.0 - (double) Vector3.Distance(((Component) other).gameObject.transform.position, ((Component) this).transform.position) * 0.05000000074505806));
      HitBox component2 = ((Component) other).gameObject.GetComponent<HitBox>();
      if (!Object.op_Inequality((Object) component2, (Object) null) || !Object.op_Inequality((Object) ((Component) component2).transform.root, (Object) null) || ((Component) ((Component) component2).transform.root).GetComponent<HERO>().myTeam == this.myTeam || ((Component) ((Component) component2).transform.root).GetComponent<HERO>().isInvincible())
        return;
      HERO component3 = ((Component) ((Component) component2).transform.root).GetComponent<HERO>();
      switch (IN_GAME_MAIN_CAMERA.gametype)
      {
        case GAMETYPE.SINGLE:
          if (component3.isGrabbed)
            break;
          Vector3 vector3_1 = Vector3.op_Subtraction(((Component) component3).transform.position, ((Component) this).transform.position);
          component3.die(Vector3.op_Addition(Vector3.op_Multiply(Vector3.op_Multiply(((Vector3) ref vector3_1).normalized, num), 1000f), Vector3.op_Multiply(Vector3.up, 50f)), false);
          GameProgressManager.RegisterHumanKill(((Component) component1).gameObject, component3, KillWeapon.Blade);
          break;
        case GAMETYPE.MULTIPLAYER:
          if (component3.HasDied() || component3.isGrabbed)
            break;
          component3.markDie();
          object[] objArray = new object[5];
          Vector3 vector3_2 = Vector3.op_Subtraction(((Component) component2).transform.root.position, ((Component) this).transform.position);
          objArray[0] = (object) Vector3.op_Addition(Vector3.op_Multiply(Vector3.op_Multiply(((Vector3) ref vector3_2).normalized, num), 1000f), Vector3.op_Multiply(Vector3.up, 50f));
          objArray[1] = (object) false;
          objArray[2] = (object) ((Component) ((Component) this).transform.root).gameObject.GetPhotonView().viewID;
          objArray[3] = PhotonView.Find(((Component) ((Component) this).transform.root).gameObject.GetPhotonView().viewID).owner.customProperties[(object) PhotonPlayerProperty.name];
          objArray[4] = (object) false;
          component3.photonView.RPC("netDie", PhotonTargets.All, objArray);
          GameProgressManager.RegisterHumanKill(((Component) component1).gameObject, component3, KillWeapon.Blade);
          break;
      }
    }
    else if (((Component) other).gameObject.tag == "titanneck")
    {
      HitBox component4 = ((Component) other).gameObject.GetComponent<HitBox>();
      if (!Object.op_Inequality((Object) component4, (Object) null) || !this.checkIfBehind(((Component) ((Component) component4).transform.root).gameObject) || this.currentHits.Contains((object) component4))
        return;
      component4.hitPosition = Vector3.op_Multiply(Vector3.op_Addition(((Component) this).transform.position, ((Component) component4).transform.position), 0.5f);
      this.currentHits.Add((object) component4);
      this.meatDie.Play();
      TITAN component5 = ((Component) ((Component) component4).transform.root).GetComponent<TITAN>();
      if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
      {
        if (Object.op_Inequality((Object) component5, (Object) null) && !component5.hasDie)
        {
          Vector3 vector3 = Vector3.op_Subtraction(((Component) component1).rigidbody.velocity, ((Component) ((Component) component4).transform.root).rigidbody.velocity);
          int num = Mathf.Max(10, (int) ((double) ((Vector3) ref vector3).magnitude * 10.0 * (double) this.scoreMulti));
          if (SettingsManager.GeneralSettings.SnapshotsEnabled.Value)
            GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().startSnapShot2(((Component) component4).transform.position, num, ((Component) ((Component) component4).transform.root).gameObject, 0.02f);
          component5.die();
          this.napeMeat(this.currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().main_object.rigidbody.velocity, ((Component) component4).transform.root);
          GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().netShowDamage(num);
          GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().ReportKillToChatFeed("You", "Titan", num);
          GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().playerKillInfoSingleUpdate(num);
          GameProgressManager.RegisterDamage(((Component) component1).gameObject, ((Component) component5).gameObject, KillWeapon.Blade, num);
          GameProgressManager.RegisterTitanKill(((Component) component1).gameObject, component5, KillWeapon.Blade);
        }
      }
      else if (!PhotonNetwork.isMasterClient)
      {
        if (Object.op_Inequality((Object) component5, (Object) null))
        {
          if (!component5.hasDie)
          {
            Vector3 vector3 = Vector3.op_Subtraction(((Component) component1).rigidbody.velocity, ((Component) ((Component) component4).transform.root).rigidbody.velocity);
            int num = Mathf.Max(10, (int) ((double) ((Vector3) ref vector3).magnitude * 10.0 * (double) this.scoreMulti));
            if (SettingsManager.GeneralSettings.SnapshotsEnabled.Value)
            {
              GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().startSnapShot2(((Component) component4).transform.position, num, ((Component) ((Component) component4).transform.root).gameObject, 0.02f);
              ((Component) ((Component) component4).transform.root).GetComponent<TITAN>().asClientLookTarget = false;
            }
            object[] objArray = new object[2]
            {
              (object) ((Component) component1).gameObject.GetPhotonView().viewID,
              (object) num
            };
            ((Component) ((Component) component4).transform.root).GetComponent<TITAN>().photonView.RPC("titanGetHit", component5.photonView.owner, objArray);
            GameProgressManager.RegisterDamage(((Component) component1).gameObject, ((Component) component5).gameObject, KillWeapon.Blade, num);
            if (component5.WillDIe(num))
              GameProgressManager.RegisterTitanKill(((Component) component1).gameObject, component5, KillWeapon.Blade);
          }
        }
        else if (Object.op_Inequality((Object) ((Component) ((Component) component4).transform.root).GetComponent<FEMALE_TITAN>(), (Object) null))
        {
          ((Component) ((Component) this).transform.root).GetComponent<HERO>().useBlade(int.MaxValue);
          Vector3 vector3 = Vector3.op_Subtraction(this.currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().main_object.rigidbody.velocity, ((Component) ((Component) component4).transform.root).rigidbody.velocity);
          int num = Mathf.Max(10, (int) ((double) ((Vector3) ref vector3).magnitude * 10.0 * (double) this.scoreMulti));
          if (!((Component) ((Component) component4).transform.root).GetComponent<FEMALE_TITAN>().hasDie)
          {
            object[] objArray = new object[2]
            {
              (object) ((Component) ((Component) this).transform.root).gameObject.GetPhotonView().viewID,
              (object) num
            };
            ((Component) ((Component) component4).transform.root).GetComponent<FEMALE_TITAN>().photonView.RPC("titanGetHit", ((Component) ((Component) component4).transform.root).GetComponent<FEMALE_TITAN>().photonView.owner, objArray);
          }
        }
        else if (Object.op_Inequality((Object) ((Component) ((Component) component4).transform.root).GetComponent<COLOSSAL_TITAN>(), (Object) null))
        {
          ((Component) ((Component) this).transform.root).GetComponent<HERO>().useBlade(int.MaxValue);
          if (!((Component) ((Component) component4).transform.root).GetComponent<COLOSSAL_TITAN>().hasDie)
          {
            Vector3 vector3 = Vector3.op_Subtraction(this.currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().main_object.rigidbody.velocity, ((Component) ((Component) component4).transform.root).rigidbody.velocity);
            int num = Mathf.Max(10, (int) ((double) ((Vector3) ref vector3).magnitude * 10.0 * (double) this.scoreMulti));
            object[] objArray = new object[2]
            {
              (object) ((Component) ((Component) this).transform.root).gameObject.GetPhotonView().viewID,
              (object) num
            };
            ((Component) ((Component) component4).transform.root).GetComponent<COLOSSAL_TITAN>().photonView.RPC("titanGetHit", ((Component) ((Component) component4).transform.root).GetComponent<COLOSSAL_TITAN>().photonView.owner, objArray);
          }
        }
      }
      else if (Object.op_Inequality((Object) component5, (Object) null))
      {
        if (!component5.hasDie)
        {
          Vector3 vector3 = Vector3.op_Subtraction(((Component) component1).rigidbody.velocity, ((Component) ((Component) component4).transform.root).rigidbody.velocity);
          int num = Mathf.Max(10, (int) ((double) ((Vector3) ref vector3).magnitude * 10.0 * (double) this.scoreMulti));
          if (SettingsManager.GeneralSettings.SnapshotsEnabled.Value)
            GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().startSnapShot2(((Component) component4).transform.position, num, ((Component) ((Component) component4).transform.root).gameObject, 0.02f);
          component5.titanGetHit(((Component) component1).gameObject.GetPhotonView().viewID, num);
          GameProgressManager.RegisterDamage(((Component) component1).gameObject, ((Component) component5).gameObject, KillWeapon.Blade, num);
          if (component5.WillDIe(num))
            GameProgressManager.RegisterTitanKill(((Component) component1).gameObject, component5, KillWeapon.Blade);
        }
      }
      else if (Object.op_Inequality((Object) ((Component) ((Component) component4).transform.root).GetComponent<FEMALE_TITAN>(), (Object) null))
      {
        ((Component) ((Component) this).transform.root).GetComponent<HERO>().useBlade(int.MaxValue);
        if (!((Component) ((Component) component4).transform.root).GetComponent<FEMALE_TITAN>().hasDie)
        {
          Vector3 vector3 = Vector3.op_Subtraction(this.currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().main_object.rigidbody.velocity, ((Component) ((Component) component4).transform.root).rigidbody.velocity);
          int num = Mathf.Max(10, (int) ((double) ((Vector3) ref vector3).magnitude * 10.0 * (double) this.scoreMulti));
          if (SettingsManager.GeneralSettings.SnapshotsEnabled.Value)
            GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().startSnapShot2(((Component) component4).transform.position, num, (GameObject) null, 0.02f);
          ((Component) ((Component) component4).transform.root).GetComponent<FEMALE_TITAN>().titanGetHit(((Component) ((Component) this).transform.root).gameObject.GetPhotonView().viewID, num);
        }
      }
      else if (Object.op_Inequality((Object) ((Component) ((Component) component4).transform.root).GetComponent<COLOSSAL_TITAN>(), (Object) null))
      {
        ((Component) ((Component) this).transform.root).GetComponent<HERO>().useBlade(int.MaxValue);
        if (!((Component) ((Component) component4).transform.root).GetComponent<COLOSSAL_TITAN>().hasDie)
        {
          Vector3 vector3 = Vector3.op_Subtraction(this.currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().main_object.rigidbody.velocity, ((Component) ((Component) component4).transform.root).rigidbody.velocity);
          int num = Mathf.Max(10, (int) ((double) ((Vector3) ref vector3).magnitude * 10.0 * (double) this.scoreMulti));
          if (SettingsManager.GeneralSettings.SnapshotsEnabled.Value)
            GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().startSnapShot2(((Component) component4).transform.position, num, (GameObject) null, 0.02f);
          ((Component) ((Component) component4).transform.root).GetComponent<COLOSSAL_TITAN>().titanGetHit(((Component) ((Component) this).transform.root).gameObject.GetPhotonView().viewID, num);
        }
      }
      this.showCriticalHitFX();
    }
    else if (((Component) other).gameObject.tag == "titaneye")
    {
      if (this.currentHits.Contains((object) ((Component) other).gameObject))
        return;
      this.currentHits.Add((object) ((Component) other).gameObject);
      GameObject gameObject = ((Component) ((Component) other).gameObject.transform.root).gameObject;
      if (Object.op_Inequality((Object) gameObject.GetComponent<FEMALE_TITAN>(), (Object) null))
      {
        if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
        {
          if (gameObject.GetComponent<FEMALE_TITAN>().hasDie)
            return;
          gameObject.GetComponent<FEMALE_TITAN>().hitEye();
        }
        else if (!PhotonNetwork.isMasterClient)
        {
          if (gameObject.GetComponent<FEMALE_TITAN>().hasDie)
            return;
          object[] objArray = new object[1]
          {
            (object) ((Component) ((Component) this).transform.root).gameObject.GetPhotonView().viewID
          };
          gameObject.GetComponent<FEMALE_TITAN>().photonView.RPC("hitEyeRPC", PhotonTargets.MasterClient, objArray);
        }
        else
        {
          if (gameObject.GetComponent<FEMALE_TITAN>().hasDie)
            return;
          gameObject.GetComponent<FEMALE_TITAN>().hitEyeRPC(((Component) ((Component) this).transform.root).gameObject.GetPhotonView().viewID);
        }
      }
      else
      {
        if (gameObject.GetComponent<TITAN>().abnormalType == AbnormalType.TYPE_CRAWLER)
          return;
        if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
        {
          if (!gameObject.GetComponent<TITAN>().hasDie)
            gameObject.GetComponent<TITAN>().hitEye();
        }
        else if (!PhotonNetwork.isMasterClient)
        {
          if (!gameObject.GetComponent<TITAN>().hasDie)
          {
            object[] objArray = new object[1]
            {
              (object) ((Component) ((Component) this).transform.root).gameObject.GetPhotonView().viewID
            };
            gameObject.GetComponent<TITAN>().photonView.RPC("hitEyeRPC", PhotonTargets.MasterClient, objArray);
          }
        }
        else if (!gameObject.GetComponent<TITAN>().hasDie)
          gameObject.GetComponent<TITAN>().hitEyeRPC(((Component) ((Component) this).transform.root).gameObject.GetPhotonView().viewID);
        this.showCriticalHitFX();
      }
    }
    else
    {
      if (!(((Component) other).gameObject.tag == "titanankle") || this.currentHits.Contains((object) ((Component) other).gameObject))
        return;
      this.currentHits.Add((object) ((Component) other).gameObject);
      GameObject gameObject = ((Component) ((Component) other).gameObject.transform.root).gameObject;
      Vector3 vector3 = Vector3.op_Subtraction(this.currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().main_object.rigidbody.velocity, gameObject.rigidbody.velocity);
      int dmg = Mathf.Max(10, (int) ((double) ((Vector3) ref vector3).magnitude * 10.0 * (double) this.scoreMulti));
      if (Object.op_Inequality((Object) gameObject.GetComponent<TITAN>(), (Object) null) && gameObject.GetComponent<TITAN>().abnormalType != AbnormalType.TYPE_CRAWLER)
      {
        if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
        {
          if (gameObject.GetComponent<TITAN>().hasDie)
            return;
          gameObject.GetComponent<TITAN>().hitAnkle();
        }
        else
        {
          if (!PhotonNetwork.isMasterClient)
          {
            if (!gameObject.GetComponent<TITAN>().hasDie)
            {
              object[] objArray = new object[1]
              {
                (object) ((Component) ((Component) this).transform.root).gameObject.GetPhotonView().viewID
              };
              gameObject.GetComponent<TITAN>().photonView.RPC("hitAnkleRPC", PhotonTargets.MasterClient, objArray);
            }
          }
          else if (!gameObject.GetComponent<TITAN>().hasDie)
            gameObject.GetComponent<TITAN>().hitAnkle();
          this.showCriticalHitFX();
        }
      }
      else
      {
        if (!Object.op_Inequality((Object) gameObject.GetComponent<FEMALE_TITAN>(), (Object) null))
          return;
        if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
        {
          if (((Object) ((Component) other).gameObject).name == "ankleR")
          {
            if (Object.op_Inequality((Object) gameObject.GetComponent<FEMALE_TITAN>(), (Object) null) && !gameObject.GetComponent<FEMALE_TITAN>().hasDie)
              gameObject.GetComponent<FEMALE_TITAN>().hitAnkleR(dmg);
          }
          else if (Object.op_Inequality((Object) gameObject.GetComponent<FEMALE_TITAN>(), (Object) null) && !gameObject.GetComponent<FEMALE_TITAN>().hasDie)
            gameObject.GetComponent<FEMALE_TITAN>().hitAnkleL(dmg);
        }
        else if (((Object) ((Component) other).gameObject).name == "ankleR")
        {
          if (!PhotonNetwork.isMasterClient)
          {
            if (!gameObject.GetComponent<FEMALE_TITAN>().hasDie)
            {
              object[] objArray = new object[2]
              {
                (object) ((Component) ((Component) this).transform.root).gameObject.GetPhotonView().viewID,
                (object) dmg
              };
              gameObject.GetComponent<FEMALE_TITAN>().photonView.RPC("hitAnkleRRPC", PhotonTargets.MasterClient, objArray);
            }
          }
          else if (!gameObject.GetComponent<FEMALE_TITAN>().hasDie)
            gameObject.GetComponent<FEMALE_TITAN>().hitAnkleRRPC(((Component) ((Component) this).transform.root).gameObject.GetPhotonView().viewID, dmg);
        }
        else if (!PhotonNetwork.isMasterClient)
        {
          if (!gameObject.GetComponent<FEMALE_TITAN>().hasDie)
          {
            object[] objArray = new object[2]
            {
              (object) ((Component) ((Component) this).transform.root).gameObject.GetPhotonView().viewID,
              (object) dmg
            };
            gameObject.GetComponent<FEMALE_TITAN>().photonView.RPC("hitAnkleLRPC", PhotonTargets.MasterClient, objArray);
          }
        }
        else if (!gameObject.GetComponent<FEMALE_TITAN>().hasDie)
          gameObject.GetComponent<FEMALE_TITAN>().hitAnkleLRPC(((Component) ((Component) this).transform.root).gameObject.GetPhotonView().viewID, dmg);
        this.showCriticalHitFX();
      }
    }
  }

  private void showCriticalHitFX()
  {
    this.currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().startShake(0.2f, 0.3f);
    (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE ? (GameObject) Object.Instantiate(Resources.Load("redCross")) : PhotonNetwork.Instantiate("redCross", ((Component) this).transform.position, Quaternion.Euler(270f, 0.0f, 0.0f), 0)).transform.position = ((Component) this).transform.position;
  }

  private void Start() => this.currentCamera = GameObject.Find("MainCamera");
}
