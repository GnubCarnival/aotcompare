// Decompiled with JetBrains decompiler
// Type: LevelTriggerCheckPoint
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

public class LevelTriggerCheckPoint : MonoBehaviour
{
  private void OnTriggerStay(Collider other)
  {
    if (!(((Component) other).gameObject.tag == "Player"))
      return;
    if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
    {
      GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().checkpoint = ((Component) this).gameObject;
    }
    else
    {
      if (!((Component) other).gameObject.GetComponent<HERO>().photonView.isMine)
        return;
      GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().checkpoint = ((Component) this).gameObject;
    }
  }

  private void Start()
  {
  }
}
