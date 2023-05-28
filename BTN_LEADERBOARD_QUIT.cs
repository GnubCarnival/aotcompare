// Decompiled with JetBrains decompiler
// Type: BTN_LEADERBOARD_QUIT
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

public class BTN_LEADERBOARD_QUIT : MonoBehaviour
{
  public GameObject leaderboard;
  public GameObject mainMenu;

  private void OnClick()
  {
    NGUITools.SetActive(this.mainMenu, true);
    NGUITools.SetActive(this.leaderboard, false);
  }
}
