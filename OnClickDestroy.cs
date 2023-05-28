// Decompiled with JetBrains decompiler
// Type: OnClickDestroy
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using Photon;
using UnityEngine;

[RequireComponent(typeof (PhotonView))]
public class OnClickDestroy : MonoBehaviour
{
  public bool DestroyByRpc;

  [RPC]
  public void DestroyRpc()
  {
    Object.Destroy((Object) ((Component) this).gameObject);
    PhotonNetwork.UnAllocateViewID(this.photonView.viewID);
  }

  private void OnClick()
  {
    if (!this.DestroyByRpc)
      PhotonNetwork.Destroy(((Component) this).gameObject);
    else
      this.photonView.RPC("DestroyRpc", PhotonTargets.AllBuffered);
  }
}
