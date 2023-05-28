// Decompiled with JetBrains decompiler
// Type: EnemyCheckCollider
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using Photon;
using UnityEngine;

public class EnemyCheckCollider : MonoBehaviour
{
  public bool active_me;
  private int count;
  public int dmg = 1;
  public bool isThisBite;

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
    if (((Component) other).gameObject.tag == "playerHitbox")
    {
      float num1 = Mathf.Min(1f, (float) (1.0 - (double) Vector3.Distance(((Component) other).gameObject.transform.position, ((Component) this).transform.position) * 0.05000000074505806));
      HitBox component = ((Component) other).gameObject.GetComponent<HitBox>();
      if (!Object.op_Inequality((Object) component, (Object) null) || !Object.op_Inequality((Object) ((Component) component).transform.root, (Object) null))
        return;
      if (this.dmg == 0)
      {
        Vector3 vector3 = Vector3.op_Subtraction(((Component) ((Component) component).transform.root).transform.position, ((Component) this).transform.position);
        float num2 = 0.0f;
        if (Object.op_Inequality((Object) ((Component) this).gameObject.GetComponent<SphereCollider>(), (Object) null))
          num2 = ((Component) this).transform.localScale.x * ((Component) this).gameObject.GetComponent<SphereCollider>().radius;
        if (Object.op_Inequality((Object) ((Component) this).gameObject.GetComponent<CapsuleCollider>(), (Object) null))
          num2 = ((Component) this).transform.localScale.x * ((Component) this).gameObject.GetComponent<CapsuleCollider>().height;
        float num3 = 5f;
        if ((double) num2 > 0.0)
          num3 = Mathf.Max(5f, num2 - ((Vector3) ref vector3).magnitude);
        if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
        {
          ((Component) ((Component) component).transform.root).GetComponent<HERO>().blowAway(Vector3.op_Addition(Vector3.op_Multiply(((Vector3) ref vector3).normalized, num3), Vector3.op_Multiply(Vector3.up, 1f)), (PhotonMessageInfo) null);
        }
        else
        {
          if (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.MULTIPLAYER)
            return;
          object[] objArray = new object[1]
          {
            (object) Vector3.op_Addition(Vector3.op_Multiply(((Vector3) ref vector3).normalized, num3), Vector3.op_Multiply(Vector3.up, 1f))
          };
          ((Component) ((Component) component).transform.root).GetComponent<HERO>().photonView.RPC("blowAway", PhotonTargets.All, objArray);
        }
      }
      else
      {
        if (((Component) ((Component) component).transform.root).GetComponent<HERO>().isInvincible())
          return;
        switch (IN_GAME_MAIN_CAMERA.gametype)
        {
          case GAMETYPE.SINGLE:
            if (((Component) ((Component) component).transform.root).GetComponent<HERO>().isGrabbed)
              break;
            Vector3 vector3_1 = Vector3.op_Subtraction(((Component) ((Component) component).transform.root).transform.position, ((Component) this).transform.position);
            ((Component) ((Component) component).transform.root).GetComponent<HERO>().die(Vector3.op_Addition(Vector3.op_Multiply(Vector3.op_Multiply(((Vector3) ref vector3_1).normalized, num1), 1000f), Vector3.op_Multiply(Vector3.up, 50f)), this.isThisBite);
            break;
          case GAMETYPE.MULTIPLAYER:
            if (((Component) ((Component) component).transform.root).GetComponent<HERO>().HasDied() || ((Component) ((Component) component).transform.root).GetComponent<HERO>().isGrabbed)
              break;
            ((Component) ((Component) component).transform.root).GetComponent<HERO>().markDie();
            int num4 = -1;
            string str = string.Empty;
            if (Object.op_Inequality((Object) ((Component) ((Component) this).transform.root).gameObject.GetComponent<EnemyfxIDcontainer>(), (Object) null))
            {
              num4 = ((Component) ((Component) this).transform.root).gameObject.GetComponent<EnemyfxIDcontainer>().myOwnerViewID;
              str = ((Component) ((Component) this).transform.root).gameObject.GetComponent<EnemyfxIDcontainer>().titanName;
            }
            object[] objArray = new object[5];
            Vector3 vector3_2 = Vector3.op_Subtraction(((Component) component).transform.root.position, ((Component) this).transform.position);
            objArray[0] = (object) Vector3.op_Addition(Vector3.op_Multiply(Vector3.op_Multiply(((Vector3) ref vector3_2).normalized, num1), 1000f), Vector3.op_Multiply(Vector3.up, 50f));
            objArray[1] = (object) this.isThisBite;
            objArray[2] = (object) num4;
            objArray[3] = (object) str;
            objArray[4] = (object) true;
            ((Component) ((Component) component).transform.root).GetComponent<HERO>().photonView.RPC("netDie", PhotonTargets.All, objArray);
            break;
        }
      }
    }
    else
    {
      if (!(((Component) other).gameObject.tag == "erenHitbox") || this.dmg <= 0 || ((Component) ((Component) other).gameObject.transform.root).gameObject.GetComponent<TITAN_EREN>().isHit)
        return;
      ((Component) ((Component) other).gameObject.transform.root).gameObject.GetComponent<TITAN_EREN>().hitByTitan();
    }
  }

  private void Start()
  {
    this.active_me = true;
    this.count = 0;
  }
}
