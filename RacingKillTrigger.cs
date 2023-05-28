// Decompiled with JetBrains decompiler
// Type: RacingKillTrigger
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

public class RacingKillTrigger : MonoBehaviour
{
  private void OnTriggerEnter(Collider other)
  {
    GameObject gameObject1 = ((Component) other).gameObject;
    if (gameObject1.layer != 8)
      return;
    GameObject gameObject2 = ((Component) gameObject1.transform.root).gameObject;
    if (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.MULTIPLAYER || !Object.op_Inequality((Object) gameObject2.GetPhotonView(), (Object) null) || !gameObject2.GetPhotonView().isMine)
      return;
    HERO component = gameObject2.GetComponent<HERO>();
    if (!Object.op_Inequality((Object) component, (Object) null))
      return;
    component.markDie();
    component.photonView.RPC("netDie2", PhotonTargets.All, (object) -1, (object) "Server");
  }
}
