﻿// Decompiled with JetBrains decompiler
// Type: PickupItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using Photon;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (PhotonView))]
public class PickupItem : MonoBehaviour, IPunObservable
{
  public static HashSet<PickupItem> DisabledPickupItems = new HashSet<PickupItem>();
  public MonoBehaviour OnPickedUpCall;
  public bool PickupIsMine;
  public bool PickupOnTrigger;
  public float SecondsBeforeRespawn = 2f;
  public bool SentPickup;
  public double TimeOfRespawn;

  public void Drop()
  {
    if (!this.PickupIsMine)
      return;
    this.photonView.RPC("PunRespawn", PhotonTargets.AllViaServer);
  }

  public void Drop(Vector3 newPosition)
  {
    if (!this.PickupIsMine)
      return;
    this.photonView.RPC("PunRespawn", PhotonTargets.AllViaServer, (object) newPosition);
  }

  public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
  {
    if (stream.isWriting && (double) this.SecondsBeforeRespawn <= 0.0)
      stream.SendNext((object) ((Component) this).gameObject.transform.position);
    else
      ((Component) this).gameObject.transform.position = (Vector3) stream.ReceiveNext();
  }

  public void OnTriggerEnter(Collider other)
  {
    PhotonView component = ((Component) other).GetComponent<PhotonView>();
    if (!this.PickupOnTrigger || !Object.op_Inequality((Object) component, (Object) null) || !component.isMine)
      return;
    this.Pickup();
  }

  internal void PickedUp(float timeUntilRespawn)
  {
    ((Component) this).gameObject.SetActive(false);
    PickupItem.DisabledPickupItems.Add(this);
    this.TimeOfRespawn = 0.0;
    if ((double) timeUntilRespawn <= 0.0)
      return;
    this.TimeOfRespawn = PhotonNetwork.time + (double) timeUntilRespawn;
    this.Invoke("PunRespawn", timeUntilRespawn);
  }

  public void Pickup()
  {
    if (this.SentPickup)
      return;
    this.SentPickup = true;
    this.photonView.RPC("PunPickup", PhotonTargets.AllViaServer);
  }

  [RPC]
  public void PunPickup(PhotonMessageInfo msgInfo)
  {
    if (msgInfo.sender.isLocal)
      this.SentPickup = false;
    if (!((Component) this).gameObject.GetActive())
    {
      Debug.Log((object) ("Ignored PU RPC, cause item is inactive. " + (object) ((Component) this).gameObject + " SecondsBeforeRespawn: " + (object) this.SecondsBeforeRespawn + " TimeOfRespawn: " + (object) this.TimeOfRespawn + " respawn in future: " + (object) (this.TimeOfRespawn > PhotonNetwork.time)));
    }
    else
    {
      this.PickupIsMine = msgInfo.sender.isLocal;
      if (Object.op_Inequality((Object) this.OnPickedUpCall, (Object) null))
        ((Component) this.OnPickedUpCall).SendMessage("OnPickedUp", (object) this);
      if ((double) this.SecondsBeforeRespawn <= 0.0)
      {
        this.PickedUp(0.0f);
      }
      else
      {
        double timeUntilRespawn = (double) this.SecondsBeforeRespawn - (PhotonNetwork.time - msgInfo.timestamp);
        if (timeUntilRespawn <= 0.0)
          return;
        this.PickedUp((float) timeUntilRespawn);
      }
    }
  }

  [RPC]
  internal void PunRespawn()
  {
    PickupItem.DisabledPickupItems.Remove(this);
    this.TimeOfRespawn = 0.0;
    this.PickupIsMine = false;
    if (!Object.op_Inequality((Object) ((Component) this).gameObject, (Object) null))
      return;
    ((Component) this).gameObject.SetActive(true);
  }

  [RPC]
  internal void PunRespawn(Vector3 pos)
  {
    Debug.Log((object) "PunRespawn with Position.");
    this.PunRespawn();
    ((Component) this).gameObject.transform.position = pos;
  }

  public int ViewID => this.photonView.viewID;
}
