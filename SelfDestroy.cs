// Decompiled with JetBrains decompiler
// Type: SelfDestroy
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using Photon;
using UnityEngine;

public class SelfDestroy : MonoBehaviour
{
  public float CountDown = 5f;

  private void Start()
  {
  }

  private void Update()
  {
    this.CountDown -= Time.deltaTime;
    if ((double) this.CountDown > 0.0)
      return;
    switch (IN_GAME_MAIN_CAMERA.gametype)
    {
      case GAMETYPE.SINGLE:
        Object.Destroy((Object) ((Component) this).gameObject);
        break;
      case GAMETYPE.MULTIPLAYER:
        if (Object.op_Inequality((Object) this.photonView, (Object) null))
        {
          if (this.photonView.viewID == 0)
          {
            Object.Destroy((Object) ((Component) this).gameObject);
            break;
          }
          if (!this.photonView.isMine)
            break;
          PhotonNetwork.Destroy(((Component) this).gameObject);
          break;
        }
        Object.Destroy((Object) ((Component) this).gameObject);
        break;
    }
  }
}
