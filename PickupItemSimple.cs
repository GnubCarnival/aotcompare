// Decompiled with JetBrains decompiler
// Type: PickupItemSimple
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using Photon;
using UnityEngine;

[RequireComponent(typeof (PhotonView))]
public class PickupItemSimple : MonoBehaviour
{
  public bool PickupOnCollide;
  public float SecondsBeforeRespawn = 2f;
  public bool SentPickup;

  public void OnTriggerEnter(Collider other)
  {
    PhotonView component = ((Component) other).GetComponent<PhotonView>();
    if (!this.PickupOnCollide || !Object.op_Inequality((Object) component, (Object) null) || !component.isMine)
      return;
    this.Pickup();
  }

  public void Pickup()
  {
    if (this.SentPickup)
      return;
    this.SentPickup = true;
    this.photonView.RPC("PunPickupSimple", PhotonTargets.AllViaServer);
  }

  [RPC]
  public void PunPickupSimple(PhotonMessageInfo msgInfo)
  {
    if (this.SentPickup && msgInfo.sender.isLocal)
      ((Component) this).gameObject.GetActive();
    this.SentPickup = false;
    if (!((Component) this).gameObject.GetActive())
    {
      Debug.Log((object) ("Ignored PU RPC, cause item is inactive. " + ((Component) this).gameObject?.ToString()));
    }
    else
    {
      float num = this.SecondsBeforeRespawn - (float) (PhotonNetwork.time - msgInfo.timestamp);
      if ((double) num <= 0.0)
        return;
      ((Component) this).gameObject.SetActive(false);
      this.Invoke("RespawnAfter", num);
    }
  }

  public void RespawnAfter()
  {
    if (!Object.op_Inequality((Object) ((Component) this).gameObject, (Object) null))
      return;
    ((Component) this).gameObject.SetActive(true);
  }
}
