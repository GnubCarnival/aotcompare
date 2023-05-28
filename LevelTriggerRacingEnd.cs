// Decompiled with JetBrains decompiler
// Type: LevelTriggerRacingEnd
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

public class LevelTriggerRacingEnd : MonoBehaviour
{
  private bool disable;

  private void OnTriggerStay(Collider other)
  {
    if (this.disable || !(((Component) other).gameObject.tag == "Player"))
      return;
    if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
    {
      GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().gameWin2();
      this.disable = true;
    }
    else
    {
      if (!((Component) other).gameObject.GetComponent<HERO>().photonView.isMine)
        return;
      GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().multiplayerRacingFinsih();
      this.disable = true;
    }
  }

  private void Start() => this.disable = false;
}
