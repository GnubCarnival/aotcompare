// Decompiled with JetBrains decompiler
// Type: BTN_PAUSE_MENU_QUIT
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

public class BTN_PAUSE_MENU_QUIT : MonoBehaviour
{
  private void OnClick()
  {
    if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
      Time.timeScale = 1f;
    else
      PhotonNetwork.Disconnect();
    Screen.lockCursor = false;
    Screen.showCursor = true;
    IN_GAME_MAIN_CAMERA.gametype = GAMETYPE.STOP;
    GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().gameStart = false;
    Object.Destroy((Object) GameObject.Find("MultiplayerManager"));
    Application.LoadLevel("menu");
  }
}
