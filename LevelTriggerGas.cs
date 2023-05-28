// Decompiled with JetBrains decompiler
// Type: LevelTriggerGas
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

public class LevelTriggerGas : MonoBehaviour
{
  private void OnTriggerStay(Collider other)
  {
    if (!(((Component) other).gameObject.tag == "Player"))
      return;
    if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
    {
      ((Component) other).gameObject.GetComponent<HERO>().fillGas();
      Object.Destroy((Object) ((Component) this).gameObject);
    }
    else
    {
      if (!((Component) other).gameObject.GetComponent<HERO>().photonView.isMine)
        return;
      ((Component) other).gameObject.GetComponent<HERO>().fillGas();
      Object.Destroy((Object) ((Component) this).gameObject);
    }
  }

  private void Start()
  {
  }
}
