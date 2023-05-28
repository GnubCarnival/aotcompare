// Decompiled with JetBrains decompiler
// Type: BTN_START_SINGLE_GAMEPLAY
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using UnityEngine;

public class BTN_START_SINGLE_GAMEPLAY : MonoBehaviour
{
  private void OnClick()
  {
    string selection1 = GameObject.Find("PopupListMap").GetComponent<UIPopupList>().selection;
    string selection2 = GameObject.Find("PopupListCharacter").GetComponent<UIPopupList>().selection;
    IN_GAME_MAIN_CAMERA.difficulty = !GameObject.Find("CheckboxHard").GetComponent<UICheckbox>().isChecked ? (!GameObject.Find("CheckboxAbnormal").GetComponent<UICheckbox>().isChecked ? 0 : 2) : 1;
    IN_GAME_MAIN_CAMERA.gametype = GAMETYPE.SINGLE;
    IN_GAME_MAIN_CAMERA.singleCharacter = selection2.ToUpper();
    if (IN_GAME_MAIN_CAMERA.cameraMode == CAMERA_TYPE.TPS)
      Screen.lockCursor = true;
    Screen.showCursor = false;
    if (selection1 == "trainning_0")
      IN_GAME_MAIN_CAMERA.difficulty = -1;
    FengGameManagerMKII.level = selection1;
    Application.LoadLevel(LevelInfo.getInfo(selection1).mapName);
  }
}
