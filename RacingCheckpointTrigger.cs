// Decompiled with JetBrains decompiler
// Type: RacingCheckpointTrigger
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

public class RacingCheckpointTrigger : MonoBehaviour
{
  private void OnTriggerEnter(Collider other)
  {
    GameObject gameObject1 = ((Component) other).gameObject;
    if (gameObject1.layer != 8)
      return;
    GameObject gameObject2 = ((Component) gameObject1.transform.root).gameObject;
    if (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.MULTIPLAYER || !Object.op_Inequality((Object) gameObject2.GetPhotonView(), (Object) null) || !gameObject2.GetPhotonView().isMine || !Object.op_Inequality((Object) gameObject2.GetComponent<HERO>(), (Object) null))
      return;
    FengGameManagerMKII.instance.chatRoom.addLINE("<color=#00ff00>Checkpoint set.</color>");
    gameObject2.GetComponent<HERO>().fillGas();
    FengGameManagerMKII.instance.racingSpawnPoint = ((Component) this).gameObject.transform.position;
    FengGameManagerMKII.instance.racingSpawnPointSet = true;
  }
}
