// Decompiled with JetBrains decompiler
// Type: Btn_to_Main_from_CC
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

public class Btn_to_Main_from_CC : MonoBehaviour
{
  private void OnClick()
  {
    PhotonNetwork.Disconnect();
    Screen.lockCursor = false;
    Screen.showCursor = true;
    IN_GAME_MAIN_CAMERA.gametype = GAMETYPE.STOP;
    GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().gameStart = false;
    Object.Destroy((Object) GameObject.Find("MultiplayerManager"));
    Application.LoadLevel("menu");
  }
}
