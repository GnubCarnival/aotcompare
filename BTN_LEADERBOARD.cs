// Decompiled with JetBrains decompiler
// Type: BTN_LEADERBOARD
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

public class BTN_LEADERBOARD : MonoBehaviour
{
  public GameObject leaderboard;
  public GameObject mainMenu;

  private void OnClick()
  {
    NGUITools.SetActive(this.mainMenu, false);
    NGUITools.SetActive(this.leaderboard, true);
  }
}
