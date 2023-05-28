// Decompiled with JetBrains decompiler
// Type: Photon.MonoBehaviour
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

namespace Photon
{
  public class MonoBehaviour : MonoBehaviour
  {
    public PhotonView networkView
    {
      get
      {
        Debug.LogWarning((object) "Why are you still using networkView? should be PhotonView?");
        return PhotonView.Get((Component) this);
      }
    }

    public PhotonView photonView => PhotonView.Get((Component) this);
  }
}
