// Decompiled with JetBrains decompiler
// Type: OnAwakeUsePhotonView
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using Photon;
using UnityEngine;

[RequireComponent(typeof (PhotonView))]
public class OnAwakeUsePhotonView : MonoBehaviour
{
  private void Awake()
  {
    if (!this.photonView.isMine)
      return;
    this.photonView.RPC("OnAwakeRPC", PhotonTargets.All);
  }

  [RPC]
  public void OnAwakeRPC() => Debug.Log((object) ("RPC: 'OnAwakeRPC' PhotonView: " + ((object) this.photonView)?.ToString()));

  [RPC]
  public void OnAwakeRPC(byte myParameter) => Debug.Log((object) ("RPC: 'OnAwakeRPC' Parameter: " + (object) myParameter + " PhotonView: " + (object) this.photonView));

  private void Start()
  {
    if (!this.photonView.isMine)
      return;
    this.photonView.RPC("OnAwakeRPC", PhotonTargets.All, (object) (byte) 1);
  }
}
