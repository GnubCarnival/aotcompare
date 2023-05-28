// Decompiled with JetBrains decompiler
// Type: RockThrow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using Photon;
using UnityEngine;

public class RockThrow : MonoBehaviour
{
  private bool launched;
  private Vector3 oldP;
  private Vector3 r;
  private Vector3 v;

  private void explore()
  {
    GameObject gameObject;
    if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER && PhotonNetwork.isMasterClient)
    {
      gameObject = PhotonNetwork.Instantiate("FX/boom6", ((Component) this).transform.position, ((Component) this).transform.rotation, 0);
      if (Object.op_Inequality((Object) ((Component) ((Component) this).transform.root).gameObject.GetComponent<EnemyfxIDcontainer>(), (Object) null))
      {
        gameObject.GetComponent<EnemyfxIDcontainer>().myOwnerViewID = ((Component) ((Component) this).transform.root).gameObject.GetComponent<EnemyfxIDcontainer>().myOwnerViewID;
        gameObject.GetComponent<EnemyfxIDcontainer>().titanName = ((Component) ((Component) this).transform.root).gameObject.GetComponent<EnemyfxIDcontainer>().titanName;
      }
    }
    else
      gameObject = (GameObject) Object.Instantiate(Resources.Load("FX/boom6"), ((Component) this).transform.position, ((Component) this).transform.rotation);
    gameObject.transform.localScale = ((Component) this).transform.localScale;
    float num = Mathf.Min(1f, (float) (1.0 - (double) Vector3.Distance(GameObject.Find("MainCamera").transform.position, gameObject.transform.position) * 0.05000000074505806));
    GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().startShake(num, num);
    if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
      Object.Destroy((Object) ((Component) this).gameObject);
    else
      PhotonNetwork.Destroy(this.photonView);
  }

  private void hitPlayer(GameObject hero)
  {
    if (!Object.op_Inequality((Object) hero, (Object) null) || hero.GetComponent<HERO>().HasDied() || hero.GetComponent<HERO>().isInvincible())
      return;
    switch (IN_GAME_MAIN_CAMERA.gametype)
    {
      case GAMETYPE.SINGLE:
        if (hero.GetComponent<HERO>().isGrabbed)
          break;
        hero.GetComponent<HERO>().die(Vector3.op_Addition(Vector3.op_Multiply(((Vector3) ref this.v).normalized, 1000f), Vector3.op_Multiply(Vector3.up, 50f)), false);
        break;
      case GAMETYPE.MULTIPLAYER:
        if (hero.GetComponent<HERO>().HasDied() || hero.GetComponent<HERO>().isGrabbed)
          break;
        hero.GetComponent<HERO>().markDie();
        int num = -1;
        string str = string.Empty;
        if (Object.op_Inequality((Object) ((Component) ((Component) this).transform.root).gameObject.GetComponent<EnemyfxIDcontainer>(), (Object) null))
        {
          num = ((Component) ((Component) this).transform.root).gameObject.GetComponent<EnemyfxIDcontainer>().myOwnerViewID;
          str = ((Component) ((Component) this).transform.root).gameObject.GetComponent<EnemyfxIDcontainer>().titanName;
        }
        object[] objArray = new object[5]
        {
          (object) Vector3.op_Addition(Vector3.op_Multiply(((Vector3) ref this.v).normalized, 1000f), Vector3.op_Multiply(Vector3.up, 50f)),
          (object) false,
          (object) num,
          (object) str,
          (object) true
        };
        hero.GetComponent<HERO>().photonView.RPC("netDie", PhotonTargets.All, objArray);
        break;
    }
  }

  [RPC]
  private void initRPC(int viewID, Vector3 scale, Vector3 pos, float level)
  {
    GameObject gameObject = ((Component) PhotonView.Find(viewID)).gameObject;
    Transform transform = gameObject.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R/forearm_R/hand_R/hand_R_001");
    ((Component) this).transform.localScale = gameObject.transform.localScale;
    ((Component) this).transform.parent = transform;
    ((Component) this).transform.localPosition = pos;
  }

  public void launch(Vector3 v1)
  {
    this.launched = true;
    this.oldP = ((Component) this).transform.position;
    this.v = v1;
    if (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.MULTIPLAYER || !PhotonNetwork.isMasterClient)
      return;
    this.photonView.RPC("launchRPC", PhotonTargets.Others, (object) this.v, (object) this.oldP);
  }

  [RPC]
  private void launchRPC(Vector3 v, Vector3 p)
  {
    this.launched = true;
    Vector3 vector3 = p;
    ((Component) this).transform.position = vector3;
    this.oldP = vector3;
    ((Component) this).transform.parent = (Transform) null;
    this.launch(v);
  }

  private void Start() => this.r = new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f), Random.Range(-5f, 5f));

  private void Update()
  {
    if (!this.launched)
      return;
    ((Component) this).transform.Rotate(this.r);
    this.v = Vector3.op_Subtraction(this.v, Vector3.op_Multiply(Vector3.op_Multiply(20f, Vector3.up), Time.deltaTime));
    Transform transform = ((Component) this).transform;
    transform.position = Vector3.op_Addition(transform.position, Vector3.op_Multiply(this.v, Time.deltaTime));
    if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER && !PhotonNetwork.isMasterClient)
      return;
    LayerMask layerMask1 = LayerMask.op_Implicit(1 << LayerMask.NameToLayer("Ground"));
    LayerMask layerMask2 = LayerMask.op_Implicit(1 << LayerMask.NameToLayer("Players"));
    LayerMask layerMask3 = LayerMask.op_Implicit(1 << LayerMask.NameToLayer("EnemyAABB"));
    LayerMask layerMask4 = LayerMask.op_Implicit(LayerMask.op_Implicit(layerMask2) | LayerMask.op_Implicit(layerMask1) | LayerMask.op_Implicit(layerMask3));
    foreach (RaycastHit raycastHit in Physics.SphereCastAll(((Component) this).transform.position, 2.5f * ((Component) this).transform.lossyScale.x, Vector3.op_Subtraction(((Component) this).transform.position, this.oldP), Vector3.Distance(((Component) this).transform.position, this.oldP), LayerMask.op_Implicit(layerMask4)))
    {
      if (LayerMask.LayerToName(((Component) ((RaycastHit) ref raycastHit).collider).gameObject.layer) == "EnemyAABB")
      {
        GameObject gameObject = ((Component) ((Component) ((RaycastHit) ref raycastHit).collider).gameObject.transform.root).gameObject;
        if (Object.op_Inequality((Object) gameObject.GetComponent<TITAN>(), (Object) null) && !gameObject.GetComponent<TITAN>().hasDie)
        {
          gameObject.GetComponent<TITAN>().hitAnkle();
          Vector3 position1 = ((Component) this).transform.position;
          if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
          {
            gameObject.GetComponent<TITAN>().hitAnkle();
          }
          else
          {
            if (Object.op_Inequality((Object) ((Component) ((Component) this).transform.root).gameObject.GetComponent<EnemyfxIDcontainer>(), (Object) null) && Object.op_Inequality((Object) PhotonView.Find(((Component) ((Component) this).transform.root).gameObject.GetComponent<EnemyfxIDcontainer>().myOwnerViewID), (Object) null))
            {
              Vector3 position2 = ((Component) PhotonView.Find(((Component) ((Component) this).transform.root).gameObject.GetComponent<EnemyfxIDcontainer>().myOwnerViewID)).transform.position;
            }
            gameObject.GetComponent<HERO>().photonView.RPC("hitAnkleRPC", PhotonTargets.All);
          }
        }
        this.explore();
      }
      else if (LayerMask.LayerToName(((Component) ((RaycastHit) ref raycastHit).collider).gameObject.layer) == "Players")
      {
        GameObject gameObject = ((Component) ((Component) ((RaycastHit) ref raycastHit).collider).gameObject.transform.root).gameObject;
        if (Object.op_Inequality((Object) gameObject.GetComponent<TITAN_EREN>(), (Object) null))
        {
          if (!gameObject.GetComponent<TITAN_EREN>().isHit)
            gameObject.GetComponent<TITAN_EREN>().hitByTitan();
        }
        else if (Object.op_Inequality((Object) gameObject.GetComponent<HERO>(), (Object) null) && !gameObject.GetComponent<HERO>().isInvincible())
          this.hitPlayer(gameObject);
      }
      else if (LayerMask.LayerToName(((Component) ((RaycastHit) ref raycastHit).collider).gameObject.layer) == "Ground")
        this.explore();
    }
    this.oldP = ((Component) this).transform.position;
  }
}
