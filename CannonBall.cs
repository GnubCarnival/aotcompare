// Decompiled with JetBrains decompiler
// Type: CannonBall
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using Photon;
using Settings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class CannonBall : MonoBehaviour
{
  private Vector3 correctPos;
  private Vector3 correctVelocity;
  public bool disabled;
  public Transform firingPoint;
  public bool isCollider;
  public HERO myHero;
  public List<TitanTrigger> myTitanTriggers;
  public float SmoothingDelay = 10f;

  private void Awake()
  {
    if (!Object.op_Inequality((Object) this.photonView, (Object) null))
      return;
    this.photonView.observed = (Component) this;
    this.correctPos = ((Component) this).transform.position;
    this.correctVelocity = Vector3.zero;
    ((Collider) ((Component) this).GetComponent<SphereCollider>()).enabled = false;
    if (!this.photonView.isMine)
      return;
    this.StartCoroutine(this.WaitAndDestroy(10f));
    this.myTitanTriggers = new List<TitanTrigger>();
  }

  public void destroyMe()
  {
    if (this.disabled)
      return;
    this.disabled = true;
    foreach (EnemyCheckCollider componentsInChild in PhotonNetwork.Instantiate("FX/boom4", ((Component) this).transform.position, ((Component) this).transform.rotation, 0).GetComponentsInChildren<EnemyCheckCollider>())
      componentsInChild.dmg = 0;
    if (SettingsManager.LegacyGameSettings.CannonsFriendlyFire.Value)
    {
      foreach (HERO player in FengGameManagerMKII.instance.getPlayers())
      {
        if (Object.op_Inequality((Object) player, (Object) null) && (double) Vector3.Distance(((Component) player).transform.position, ((Component) this).transform.position) <= 20.0 && !player.photonView.isMine)
        {
          GameObject gameObject = ((Component) player).gameObject;
          PhotonPlayer owner = gameObject.GetPhotonView().owner;
          if (SettingsManager.LegacyGameSettings.TeamMode.Value > 0 && PhotonNetwork.player.customProperties[(object) PhotonPlayerProperty.RCteam] != null && owner.customProperties[(object) PhotonPlayerProperty.RCteam] != null)
          {
            int num1 = RCextensions.returnIntFromObject(PhotonNetwork.player.customProperties[(object) PhotonPlayerProperty.RCteam]);
            int num2 = RCextensions.returnIntFromObject(owner.customProperties[(object) PhotonPlayerProperty.RCteam]);
            if (num1 == 0 || num1 != num2)
            {
              gameObject.GetComponent<HERO>().markDie();
              gameObject.GetComponent<HERO>().photonView.RPC("netDie2", PhotonTargets.All, (object) -1, (object) (RCextensions.returnStringFromObject(PhotonNetwork.player.customProperties[(object) PhotonPlayerProperty.name]) + " "));
              FengGameManagerMKII.instance.playerKillInfoUpdate(PhotonNetwork.player, 0);
            }
          }
          else
          {
            gameObject.GetComponent<HERO>().markDie();
            gameObject.GetComponent<HERO>().photonView.RPC("netDie2", PhotonTargets.All, (object) -1, (object) (RCextensions.returnStringFromObject(PhotonNetwork.player.customProperties[(object) PhotonPlayerProperty.name]) + " "));
            FengGameManagerMKII.instance.playerKillInfoUpdate(PhotonNetwork.player, 0);
          }
        }
      }
    }
    if (this.myTitanTriggers != null)
    {
      for (int index = 0; index < this.myTitanTriggers.Count; ++index)
      {
        if (Object.op_Inequality((Object) this.myTitanTriggers[index], (Object) null))
          this.myTitanTriggers[index].isCollide = false;
      }
    }
    PhotonNetwork.Destroy(((Component) this).gameObject);
  }

  public void FixedUpdate()
  {
    if (!this.photonView.isMine || this.disabled)
      return;
    LayerMask layerMask1 = LayerMask.op_Implicit(1 << LayerMask.NameToLayer("PlayerAttackBox"));
    LayerMask layerMask2 = LayerMask.op_Implicit(1 << LayerMask.NameToLayer("EnemyBox"));
    LayerMask layerMask3 = LayerMask.op_Implicit(LayerMask.op_Implicit(layerMask1) | LayerMask.op_Implicit(layerMask2));
    if (!this.isCollider)
    {
      LayerMask layerMask4 = LayerMask.op_Implicit(1 << LayerMask.NameToLayer("Ground"));
      layerMask3 = LayerMask.op_Implicit(LayerMask.op_Implicit(layerMask3) | LayerMask.op_Implicit(layerMask4));
    }
    Collider[] colliderArray = Physics.OverlapSphere(((Component) this).transform.position, 0.6f, ((LayerMask) ref layerMask3).value);
    bool flag = false;
    for (int index = 0; index < colliderArray.Length; ++index)
    {
      GameObject gameObject = ((Component) colliderArray[index]).gameObject;
      if (gameObject.layer == 16)
      {
        TitanTrigger component = gameObject.GetComponent<TitanTrigger>();
        if (!Object.op_Equality((Object) component, (Object) null) && !this.myTitanTriggers.Contains(component))
        {
          component.isCollide = true;
          this.myTitanTriggers.Add(component);
        }
      }
      else if (gameObject.layer == 10)
      {
        TITAN component = ((Component) gameObject.transform.root).gameObject.GetComponent<TITAN>();
        if (Object.op_Inequality((Object) component, (Object) null))
        {
          if (component.abnormalType == AbnormalType.TYPE_CRAWLER)
          {
            if (((Object) gameObject).name == "head")
            {
              component.photonView.RPC("DieByCannon", component.photonView.owner, (object) this.myHero.photonView.viewID);
              component.dieBlow(((Component) this).transform.position, 0.2f);
              index = colliderArray.Length;
            }
          }
          else if (((Object) gameObject).name == "head")
          {
            component.photonView.RPC("DieByCannon", component.photonView.owner, (object) this.myHero.photonView.viewID);
            component.dieHeadBlow(((Component) this).transform.position, 0.2f);
            index = colliderArray.Length;
          }
          else if ((double) Random.Range(0.0f, 1f) < 0.5)
            component.hitL(((Component) this).transform.position, 0.05f);
          else
            component.hitR(((Component) this).transform.position, 0.05f);
          this.destroyMe();
        }
      }
      else if (gameObject.layer == 9 && (((Object) gameObject.transform.root).name.Contains("CannonWall") || ((Object) gameObject.transform.root).name.Contains("CannonGround")))
        flag = true;
    }
    if (this.isCollider | flag)
      return;
    this.isCollider = true;
    ((Collider) ((Component) this).GetComponent<SphereCollider>()).enabled = true;
  }

  public void OnCollisionEnter(Collision myCollision)
  {
    if (!this.photonView.isMine)
      return;
    this.destroyMe();
  }

  public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
  {
    if (stream.isWriting)
    {
      stream.SendNext((object) ((Component) this).transform.position);
      stream.SendNext((object) ((Component) this).rigidbody.velocity);
    }
    else
    {
      this.correctPos = (Vector3) stream.ReceiveNext();
      this.correctVelocity = (Vector3) stream.ReceiveNext();
    }
  }

  public void Update()
  {
    if (this.photonView.isMine)
      return;
    ((Component) this).transform.position = Vector3.Lerp(((Component) this).transform.position, this.correctPos, Time.deltaTime * this.SmoothingDelay);
    ((Component) this).rigidbody.velocity = this.correctVelocity;
  }

  public IEnumerator WaitAndDestroy(float time)
  {
    yield return (object) new WaitForSeconds(time);
    this.destroyMe();
  }
}
