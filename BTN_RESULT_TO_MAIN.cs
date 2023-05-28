// Decompiled with JetBrains decompiler
// Type: BTN_RESULT_TO_MAIN
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

public class BTN_RESULT_TO_MAIN : MonoBehaviour
{
  private void OnClick()
  {
    Time.timeScale = 1f;
    if (PhotonNetwork.connected)
      PhotonNetwork.Disconnect();
    IN_GAME_MAIN_CAMERA.gametype = GAMETYPE.STOP;
    GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().gameStart = false;
    Screen.lockCursor = false;
    Screen.showCursor = true;
    Object.Destroy((Object) GameObject.Find("MultiplayerManager"));
    Application.LoadLevel("menu");
  }
}
